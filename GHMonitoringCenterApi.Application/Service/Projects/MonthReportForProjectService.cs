﻿using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Linq;
using UtilsSharp;
using Models = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Service.Projects
{
    /// <summary>
    /// 项目月报重写接口实现
    /// </summary>
    public class MonthReportForProjectService : IMonthReportForProjectService
    {
        /// <summary>
        /// 上下文
        /// </summary>
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 映射服务
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// 业务授权层
        /// </summary>
        private readonly IBizAuthorizeService _bizAuthorizeService;
        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        /// <summary>
        /// 服务注入
        /// </summary>
        public MonthReportForProjectService(ISqlSugarClient dbContext, IMapper mapper, GlobalObject globalObject, IBizAuthorizeService bizAuthorizeService)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._globalObject = globalObject;
            this._bizAuthorizeService = bizAuthorizeService;
        }

        #region 处理WBS树
        /***
         * 1.查询基础/最新wbs数据：调用方法 GetWBSDataAsync
         * 2.wbs计算转换成树：调用方法 WBSConvertTree
         */
        /// <summary>
        /// wbs转换树
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateMonth"></param>
        /// <param name="bData">施工性质、产值属性</param>
        /// <param name="isStaging">是否是暂存</param>
        /// <param name="stagingList">暂存的数据</param>
        /// <param name="dayRep">读取日报数据作为估算值</param>
        /// <returns></returns>
        private async Task<List<ProjectWBSDto>> WBSConvertTree(Guid projectId, int? dateMonth, List<MonthReportForProjectBaseDataResponseDto> bData, bool isStaging, List<ProjectWBSDto> stagingList, bool dayRep, bool IsExistZcAndMp, bool exhaustedBtn)
        {
            /***
             * 1.获取请求数据
             * 2.获取当月月报数据
             * 3.获取年度月报详细
             * 4.获取开累月报详细
             * 5.获取WBS数据
             * 6.获取基础数据(施工性质、产值属性、资源)
             */
            var requestList = await GetWBSDataAsync(projectId, dateMonth, dayRep, IsExistZcAndMp, exhaustedBtn);
            var mReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowMonth).OrderBy(x => x.DateMonth).ToList();
            var yReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowYear).OrderBy(x => x.DateMonth).ToList();
            var klReportList = requestList.Where(x => x.ValueType == ValueEnumType.AccumulatedCommencement).OrderBy(x => x.DateMonth).ToList();
            var wbsList = requestList.Where(x => x.ValueType == ValueEnumType.None).ToList();

            //2025年前没有出现过的资源月报数据
            var addBefore2024 = await _dbContext.Queryable<MonthReportDetailAdd>().Where(t => t.IsDelete == 1).ToListAsync();
            var pwbsIds = addBefore2024.Select(x => x.ProjectWBSId).ToList();
            var projectIds = addBefore2024.Select(x => x.ProjectId.ToString()).Distinct().ToList();
            if (isStaging)
            {
                //本月的数据为暂存的数据  清零是为了不做重复计算
                List<ProjectWBSDto> newMRep = new(); //为了合并当月月报暂存的分组
                List<ProjectWBSDto> newYRep = new(); //为了合并当月月报暂存的分组
                List<ProjectWBSDto> newKlRep = new(); //为了合并当月月报暂存的分组

                foreach (var item in stagingList)
                {
                    var mRep = mReportList.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ProjectWBSId == item.ProjectWBSId);
                    if (mRep == null)
                    {
                        if (Convert.ToInt32(item.CompletedQuantity) == 0)//过滤
                        {
                            item.IsAllowDelete = false;
                        }
                        newMRep.Add(item);
                    }
                    else
                    {
                        mRep.IsAllowDelete = true;
                        mRep.CompleteProductionAmount = item.CompleteProductionAmount;
                        mRep.CompletedQuantity = item.CompletedQuantity;
                        mRep.OutsourcingExpensesAmount = item.OutsourcingExpensesAmount;
                    }

                    var yRep = yReportList.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ProjectWBSId == item.ProjectWBSId);
                    if (yRep == null)
                    {
                        if (Convert.ToInt32(item.CompletedQuantity) == 0)//过滤
                        {
                            item.IsAllowDelete = false;
                        }
                        newYRep.Add(item);
                    }
                    else
                    {
                        yRep.IsAllowDelete = true;

                        yRep.YearCompletedQuantity = item.YearCompletedQuantity;
                        yRep.YearCompleteProductionAmount = item.YearCompleteProductionAmount;
                        yRep.YearOutsourcingExpensesAmount = item.YearOutsourcingExpensesAmount;
                    }

                    var kRep = klReportList.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ProjectWBSId == item.ProjectWBSId);
                    if (kRep == null)
                    {
                        if (Convert.ToInt32(item.CompletedQuantity) == 0)//过滤
                        {
                            item.IsAllowDelete = false;
                        }
                        newKlRep.Add(item);
                    }
                    else
                    {
                        kRep.IsAllowDelete = true;

                        kRep.TotalCompletedQuantity = item.TotalCompletedQuantity;
                        kRep.TotalCompleteProductionAmount = item.TotalCompleteProductionAmount;
                        kRep.TotalOutsourcingExpensesAmount = item.TotalOutsourcingExpensesAmount;
                    }
                }
                mReportList.AddRange(newMRep);
                yReportList.AddRange(newYRep);
                klReportList.AddRange(newKlRep);
            }

            //获取当前项目所有的月报存在的wbsid  不包含的wbsid 全部去掉
            var mpWbsIds = await _dbContext.Queryable<MonthReportDetail>().Where(x => x.IsDelete == 1 && x.ProjectId == projectId).Select(x => x.ProjectWBSId).ToListAsync();

            if (!dayRep)
            {
                //如果是日报作为累计数  还要加上日报中的wbs
                var dWbsIds = mReportList.Where(x => x.IsDayRep == true).Select(x => x.ProjectWBSId).Distinct().ToList();
                mpWbsIds.AddRange(dWbsIds);
                mpWbsIds = mpWbsIds.Distinct().ToList();
            }

            //转换wbs树
            var pWbsTree = BuildTree("0", wbsList, mpWbsIds, mReportList, yReportList, klReportList, bData, addBefore2024, dateMonth, isStaging);

            return pWbsTree;
        }
        /// <summary>
        /// 树节点
        /// </summary>
        /// <param name="rootPid"></param>
        /// <param name="wbsList"></param>
        /// <param name="mpWbsIds"></param>
        /// <param name="mReportList"></param>
        /// <param name="yReportList"></param>
        /// <param name="klReportList"></param>
        /// <param name="bData"></param>
        /// <param name="addBefore2024"></param>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        public List<ProjectWBSDto> BuildTree(string? rootPid, List<ProjectWBSDto> wbsList, List<Guid>? mpWbsIds, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData, List<MonthReportDetailAdd> addBefore2024, int? dateMonth, bool isStaging)
        {
            var tree = new List<ProjectWBSDto>();
            // 获取所有主节点
            var mainNodes = wbsList.Where(x => x.Pid == rootPid).ToList();

            foreach (var node in mainNodes)
            {
                // 递归获取子节点
                var children = BuildTree(node.KeyId, wbsList, mpWbsIds, mReportList, yReportList, klReportList, bData, addBefore2024, dateMonth, isStaging);
                // 判断当前节点是否是最后节点
                if (!children.Any()) // 如果没有子节点
                {
                    if (!mpWbsIds.Contains(node.ProjectWBSId) && node.IsDelete == 0)
                    {
                        continue; // 跳过这个节点，不添加到树中
                    }
                }
                node.Children.AddRange(children);
                tree.Add(node);

                var values = mReportList.Where(x => x.ProjectWBSId == node.ProjectWBSId).ToList();
                var finallyList = MReportForProjectList(node.ProjectWBSId, values, yReportList, klReportList, bData, addBefore2024, dateMonth, isStaging);
                if (!finallyList.Any())//如果没有子节点  看当前节点是否存在2025年前需要补录的开累数据
                {
                    List<ProjectWBSDto> add = new();
                    var pwbsIds = addBefore2024.Select(x => x.ProjectWBSId).ToList();
                    var projectIds = addBefore2024.Select(x => x.ProjectId.ToString()).ToList();
                    //追加202412月前没存在过的wbs数据 如果之前存在过则相加产值、工程量、外包支出、否则追加整条数据
                    #region 追加202412月前没存在过的wbs数据 如果之前存在过则相加产值、工程量、外包支出、否则追加整条数据
                    if (dateMonth != null && dateMonth > 202412)
                    {
                        if (pwbsIds.Contains(node.ProjectWBSId) && projectIds.Contains(node.ProjectId))
                        {
                            var addList = addBefore2024.Where(x => x.ProjectWBSId == node.ProjectWBSId).ToList();
                            //追加整条
                            foreach (var item2 in addList)
                            {
                                add.Add(new ProjectWBSDto
                                {
                                    Id = Guid.Empty,
                                    ProjectId = item2.ProjectId.ToString(),
                                    UnitPrice = item2.UnitPrice,
                                    OutPutType = item2.OutPutType,
                                    ShipId = item2.ShipId,
                                    ProjectWBSId = item2.ProjectWBSId,
                                    ConstructionNature = item2.ConstructionNature,
                                    TotalOutsourcingExpensesAmount = item2.OutsourcingExpensesAmount,
                                    TotalCompletedQuantity = item2.CompletedQuantity,
                                    TotalCompleteProductionAmount = item2.CompleteProductionAmount,
                                    ShipName = item2.ShipName,
                                });
                            }
                            finallyList.AddRange(add);
                        }
                    }
                    #endregion
                }

                node.ReportDetails.AddRange(finallyList);
            }

            return tree;
        }
        /// <summary>
        /// 最后一层节点处理月报详细数据
        /// </summary>
        /// <param name="wbsId">wbsid（施工分类）</param>
        /// <param name="mReportList">当月月报详细数据</param>
        /// <param name="yReportList">当年月报详细数据(做统计数据使用)</param>
        /// <param name="klReportList">开累月报详细数据(做统计数据使用)</param>
        /// <param name="bData">施工性质、产值属性</param>
        /// <param name="addBefore2024">施工性质、产值属性</param>
        /// <returns></returns>
        private List<ProjectWBSDto> MReportForProjectList(Guid wbsId, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData, List<MonthReportDetailAdd> addBefore2024, int? dateMonth, bool isStaging)
        {
            /***
             * 1.根据(当前施工分类)wbsid获取所有资源（船舶）信息
             * 2.统计资源（每条船）年度、开累值
             */
            List<ProjectWBSDto> addHis = new();
            var pwbsIds = addBefore2024.Select(x => x.ProjectWBSId).ToList();
            var projectIds = addBefore2024.Select(x => x.ProjectId.ToString()).ToList();
            var mReport = mReportList.OrderBy(x => x.ShipId).ThenBy(x => x.DateMonth).Where(x => x.ProjectWBSId == wbsId).ToList();
            foreach (var report in mReport)
            {
                //当月统计
                report.CompleteProductionAmount = mReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.CompleteProductionAmount);
                report.CompletedQuantity = mReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.CompletedQuantity);
                report.OutsourcingExpensesAmount = mReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.OutsourcingExpensesAmount);

                //年度统计
                report.YearCompleteProductionAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.YearCompleteProductionAmount);
                report.YearCompletedQuantity = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.YearCompletedQuantity);
                report.YearOutsourcingExpensesAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.YearOutsourcingExpensesAmount);

                //开累统计
                report.TotalCompleteProductionAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.TotalCompleteProductionAmount);
                report.TotalCompletedQuantity = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.TotalCompletedQuantity);
                report.TotalOutsourcingExpensesAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.TotalOutsourcingExpensesAmount);

                report.ActualOutAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.ActualOutAmount);
                report.ActualCompQuantity = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.ActualCompQuantity);
                report.ActualCompAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.ActualCompAmount);

                report.OldHQuantity = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.OldHQuantity);
                report.OldCurrencyHValue = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.OldCurrencyHValue);
                report.OldCurrencyHOutValue = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.OldCurrencyHOutValue);

                if (report.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())
                {
                    report.RMBHValue = 0;
                    report.RMBHOutValue = 0;
                }
                else
                {
                    report.RMBHValue = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice && x.OutPutType == report.OutPutType).Sum(x => x.RMBHValue);
                    report.RMBHOutValue = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice && x.OutPutType == report.OutPutType).Sum(x => x.RMBHOutValue);
                }

                /***
                 * 基本信息处理
                 */
                #region 基础信息处理
                report.ConstructionNatureName = bData.FirstOrDefault(x => RouseType.ConstructionNature == x.RouseType && x.ConstructionNatureType == report.ConstructionNature)?.Name;
                report.OutPutTypeName = bData.FirstOrDefault(x => (x.RouseType == RouseType.Self || x.RouseType == RouseType.Sub) && report.OutPutType == x.ShipRouseType)?.Name;
                report.ShipName = bData.FirstOrDefault(x => x.Id == report.ShipId)?.Name;
                report.DetailId = klReportList.FirstOrDefault(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId)?.DetailId;
                #endregion

                //追加202412月前没存在过的wbs数据 如果之前存在过则相加产值、工程量、外包支出、否则追加整条数据
                #region 追加202412月前没存在过的wbs数据 如果之前存在过则相加产值、工程量、外包支出、否则追加整条数据  
                if (dateMonth != null && dateMonth > 202412)
                {
                    if (isStaging == false)//跳过暂存状态
                    {
                        if (pwbsIds.Contains(report.ProjectWBSId) && projectIds.Contains(report.ProjectId))
                        {
                            //如果当月数据在追加数据表中存在 则相加
                            var existBefore2024 = addBefore2024.FirstOrDefault(x => x.ProjectWBSId == report.ProjectWBSId && x.UnitPrice == report.UnitPrice && x.ShipId == report.ShipId && x.ProjectId.ToString() == report.ProjectId && x.ConstructionNature == report.ConstructionNature);

                            if (existBefore2024 != null)
                            {
                                report.TotalCompletedQuantity += existBefore2024.CompletedQuantity;
                                report.TotalCompleteProductionAmount += existBefore2024.CompleteProductionAmount;
                                report.TotalOutsourcingExpensesAmount += existBefore2024.OutsourcingExpensesAmount;
                            }
                            else
                            {
                                var addList = addBefore2024.Where(x => x.ProjectWBSId == report.ProjectWBSId).ToList();
                                //追加整条
                                foreach (var item2 in addList)
                                {
                                    addHis.Add(new ProjectWBSDto
                                    {
                                        Id = Guid.Empty,
                                        ProjectId = item2.ProjectId.ToString(),
                                        UnitPrice = item2.UnitPrice,
                                        OutPutType = item2.OutPutType,
                                        ShipId = item2.ShipId,
                                        ShipName = item2.ShipName,
                                        ProjectWBSId = item2.ProjectWBSId,
                                        ConstructionNature = item2.ConstructionNature,
                                        TotalOutsourcingExpensesAmount = item2.OutsourcingExpensesAmount,
                                        TotalCompletedQuantity = item2.CompletedQuantity,
                                        TotalCompleteProductionAmount = item2.CompleteProductionAmount,
                                    });
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            mReport.AddRange(addHis);
            if (isStaging == false)//跳过暂存状态
            {
                //排除掉本月有的 追加的 重复资源
                List<ProjectWBSDto> addRes = new();
                var gpHis = mReport.GroupBy(x => new { x.ShipId, x.ProjectId, x.ConstructionNature, x.OutPutType, x.UnitPrice, x.ProjectWBSId }).ToList();
                foreach (var item in gpHis)
                {
                    if (pwbsIds.Contains(item.Key.ProjectWBSId) && projectIds.Contains(item.Key.ProjectId))
                    {
                        var exist = mReport.Where(x => x.ShipId == item.Key.ShipId && x.ProjectId == item.Key.ProjectId && x.ConstructionNature == item.Key.ConstructionNature && x.OutPutType == item.Key.OutPutType && x.UnitPrice == item.Key.UnitPrice && x.ProjectWBSId == item.Key.ProjectWBSId).ToList();
                        if (exist.Count == 1 || exist.Count == 0)
                        {
                            continue;
                        }
                        else
                        {
                            mReport = mReport.Where(x => !(x.ShipId == item.Key.ShipId && x.ProjectId == item.Key.ProjectId && x.ConstructionNature == item.Key.ConstructionNature && x.OutPutType == item.Key.OutPutType && x.UnitPrice == item.Key.UnitPrice && x.ProjectWBSId == item.Key.ProjectWBSId)).ToList();
                            var add = exist.Where(x => x.Id != Guid.Empty).ToList();
                            addRes.AddRange(add);
                        }
                    }
                }
                //最终结果
                mReport.AddRange(addRes);
            }
            return mReport;
        }
        /// <summary>
        /// 查询基础/最新wbs数据
        /// </summary>
        /// <param name="pId">项目id</param>
        /// <param name="dateMonth">填报日期</param>
        /// <param name="dayRep">填报日期</param>
        /// <returns></returns>
        private async Task<List<ProjectWBSDto>> GetWBSDataAsync(Guid pId, int? dateMonth, bool dayRep, bool IsExistZcAndMp, bool exhaustedBtn)
        {
            var pWBS = new List<ProjectWBSDto>();
            var calculatePWBS = new List<ProjectWBSDto>();

            //是否计算是统计的偏差月
            bool pianchaMonth = dateMonth > 202412 ? true : false;
            var historyKaiLei = await _dbContext.Queryable<MonthReportDetailHistory>().Where(t => t.IsDelete == 1).ToListAsync();

            //空的项目id  不返回数据; 一个项目对应一个wbs
            if (pId == Guid.Empty) return pWBS;

            //获取本身的项目wbs数据
            pWBS = await HandleWBSDataAsync(pId, dateMonth);

            //获取项目当年的汇率
            int yearParam = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
            var project = await _dbContext.Queryable<Project>().Where(t => t.Id == pId && t.IsDelete == 1).FirstAsync();

            //获取需要计算的月报填报数据 
            if (dateMonth != 0 && dateMonth.ToString().Length == 6)
            {
                var mPList = await _dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && x.DateMonth != 202306)
                    .ToListAsync();

                var mPIds = mPList.Select(x => x.Id).ToList();

                //获取当月前需要计算的的所有填报数据(累计的所有数据/开累)
                calculatePWBS = await _dbContext.Queryable<MonthReportDetail>()
                   .Where(p => mPIds.Contains(p.MonthReportId) && !string.IsNullOrEmpty(p.ProjectId.ToString()) && p.ProjectId != Guid.Empty && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId && p.DateMonth <= dateMonth)
                   .LeftJoin<MonthReportDetailHistory>((p, y) => p.Id == y.Id)
                   .Select((p, y) => new ProjectWBSDto
                   {
                       Id = p.Id,
                       ProjectId = p.ProjectId.ToString(),
                       ProjectWBSId = p.ProjectWBSId,
                       UnitPrice = p.UnitPrice,//月报明细填的单价
                       CompletedQuantity = p.CompletedQuantity,//月报明细填的工程量
                       ConstructionNature = p.ConstructionNature,
                       DateMonth = p.DateMonth,
                       DateYear = p.DateYear,
                       OutsourcingExpensesAmount = p.OutsourcingExpensesAmount,//月报明细填的外包支出
                       ShipId = p.ShipId,
                       OutPutType = p.OutPutType,
                       ValueType = ValueEnumType.AccumulatedCommencement,
                       Remark = p.Remark,
                       DetailId = p.Id,
                       CompleteProductionAmount = p.UnitPrice * p.CompletedQuantity, //p.CompleteProductionAmount  外币|人民币
                       ActualCompAmount = y.ActualCompAmount,
                       ActualCompQuantity = y.ActualCompQuantity,
                       ActualOutAmount = y.CurrencyOutsourcingExpensesAmount,
                       RMBHOutValue = y.RMBHOutValue,
                       RMBHValue = y.RMBHValue,
                       CurrencyId = project.CurrencyId
                   })
                   .ToListAsync();

                //新项目处理逻辑  赋初始值
                if (!calculatePWBS.Any())
                {
                    var shipid = GuidUtil.Next();
                    calculatePWBS = await _dbContext.Queryable<ProjectWBS>()
                  .Where(p => !string.IsNullOrEmpty(p.ProjectId.ToString()) && SqlFunc.ToGuid(p.ProjectId) == pId)
                  .Select((p) => new ProjectWBSDto
                  {
                      Id = p.Id,
                      ProjectId = p.ProjectId.ToString(),
                      ProjectWBSId = p.Id,
                      UnitPrice = p.UnitPrice,
                      DateMonth = SqlFunc.ToInt32(dateMonth),
                      DateYear = DateTime.Now.Year,
                      ValueType = ValueEnumType.AccumulatedCommencement,
                      DetailId = p.Id,
                      ShipId = shipid,
                      OutPutType = ConstructionOutPutType.Self,
                      CurrencyId = project.CurrencyId
                  })
                  .ToListAsync();
                }



                if (exhaustedBtn == true)
                {
                    calculatePWBS = calculatePWBS.Where(x => x.DateMonth <= 202412).ToList();
                    //月报明细历史表
                    var monthHistory = await _dbContext.Queryable<MonthReportDetailHistory>().Where(t => t.IsDelete == 1 && t.ProjectId == pId && t.DateMonth <= 202412).ToListAsync();
                    foreach (var item in monthHistory)
                    {
                        var isExist = calculatePWBS.Where(t => t.ProjectId == item.ProjectId.ToString() && t.ShipId == item.ShipId && t.ProjectWBSId == item.ProjectWBSId && t.Id == item.Id).Any();
                        if (!isExist)
                        {
                            var model = new ProjectWBSDto
                            {
                                Id = item.Id,
                                ProjectId = item.ProjectId.ToString(),
                                ProjectWBSId = item.ProjectWBSId,
                                UnitPrice = item.UnitPrice,//月报明细填的单价
                                CompletedQuantity = item.CompletedQuantity,//月报明细填的工程量
                                ConstructionNature = item.ConstructionNature,
                                DateMonth = item.DateMonth,
                                DateYear = item.DateYear,
                                OutsourcingExpensesAmount = item.OutsourcingExpensesAmount,//月报明细填的外包支出
                                ShipId = item.ShipId,
                                OutPutType = item.OutPutType,
                                ValueType = ValueEnumType.AccumulatedCommencement,
                                Remark = item.Remark,
                                DetailId = item.MonthReportId,
                                CompleteProductionAmount = item.UnitPrice * item.CompletedQuantity, //p.CompleteProductionAmount  外币|人民币
                                ActualCompAmount = item.ActualCompAmount,
                                ActualCompQuantity = item.ActualCompQuantity,
                                ActualOutAmount = item.CurrencyOutsourcingExpensesAmount,
                                RMBHOutValue = item.RMBHOutValue,
                                RMBHValue = item.RMBHValue,
                                CurrencyId = project.CurrencyId
                            };
                            calculatePWBS.Add(model);
                        }
                    }

                }

                foreach (var item in calculatePWBS)
                {
                    item.EngQuantity = pWBS.FirstOrDefault(x => x.ProjectWBSId == item.ProjectWBSId)?.EngQuantity;
                    item.Pid = pWBS.FirstOrDefault(x => x.ProjectWBSId == item.ProjectWBSId)?.Pid;
                    item.KeyId = pWBS.FirstOrDefault(x => x.ProjectWBSId == item.ProjectWBSId)?.KeyId;
                    item.IsDelete = item.ProjectWBSId == Guid.Empty ? 1 : pWBS.FirstOrDefault(x => x.ProjectWBSId == item.ProjectWBSId).IsDelete;
                }

                /***
                 * 1.取当月的填报数据
                 * 2.业务区分字段为当月
                 * 3.查询当月的月报详细数据允许删除
                 */

                var nowMonthReport = new List<ProjectWBSDto>();
                foreach (var nowMonth in calculatePWBS.Where(x => x.DateMonth == dateMonth).ToList())
                {
                    nowMonthReport.Add(new ProjectWBSDto
                    {
                        ValueType = ValueEnumType.NowMonth,
                        IsAllowDelete = true,
                        CompleteProductionAmount = nowMonth.CompleteProductionAmount,
                        CompletedQuantity = nowMonth.CompletedQuantity,
                        UnitPrice = nowMonth.UnitPrice,
                        ConstructionNature = nowMonth.ConstructionNature,
                        ContractAmount = nowMonth.ContractAmount,
                        DateMonth = nowMonth.DateMonth,
                        DateYear = nowMonth.DateYear,
                        EngQuantity = nowMonth.EngQuantity,
                        Id = nowMonth.Id,
                        KeyId = nowMonth.KeyId,
                        Name = nowMonth.Name,
                        OutPutType = nowMonth.OutPutType,
                        OutsourcingExpensesAmount = nowMonth.OutsourcingExpensesAmount,
                        Pid = nowMonth.Pid,
                        ProjectId = nowMonth.ProjectId,
                        ProjectWBSId = nowMonth.ProjectWBSId,
                        Remark = nowMonth.Remark,
                        ShipId = nowMonth.ShipId,
                        DetailId = nowMonth.DetailId
                    });
                }

                /***
                 * 当月填报详细中需要包含历史中存在的资源（船舶）信息，所以在此处valuetype=当月,其他基础信息不变作为本月数据
                 * 1.不包含已经存在的资源（船舶）根据项目wbsid 资源（船舶id）去重
                 * 2.取除当月之前的所有填报数据
                 * 3.根据资源（船舶id）去重
                 * 4.需要填写的单价、工程量、外包支出、备注全部初始化
                 */

                var historyMonthReport = new List<ProjectWBSDto>();
                foreach (var kvp in calculatePWBS
                    .Where(x => x.DateMonth < dateMonth))
                {
                    historyMonthReport.Add(new ProjectWBSDto
                    {
                        CompletedQuantity = kvp.CompletedQuantity,// 0M,//
                        UnitPrice = kvp.UnitPrice,//0M,//
                        OutsourcingExpensesAmount = kvp.OutsourcingExpensesAmount,//0M, 
                        ValueType = ValueEnumType.NowMonth,
                        CompleteProductionAmount = kvp.CompleteProductionAmount,//0M,// 
                        Remark = kvp.Remark,//string.Empty,string.Empty,//
                        ConstructionNature = kvp.ConstructionNature,
                        ContractAmount = kvp.ContractAmount,
                        DateMonth = kvp.DateMonth,
                        DateYear = kvp.DateYear,
                        EngQuantity = kvp.EngQuantity,
                        Id = kvp.Id,
                        KeyId = kvp.KeyId,
                        Name = kvp.Name,
                        OutPutType = kvp.OutPutType,
                        Pid = kvp.Pid,
                        ProjectId = kvp.ProjectId,
                        ProjectWBSId = kvp.ProjectWBSId,
                        ShipId = kvp.ShipId,
                        DetailId = kvp.DetailId
                    });
                }

                #region 如果需要没有填写月报  将传入月-1月26  至  传入月25日报数据统计 作为月报估算值
                //如果需要没有填写月报  将传入月-1月26  至  传入月25日报数据统计 作为月报估算值
                ConvertHelper.TryParseFromDateMonth(dateMonth.Value, out DateTime monthTime);
                int startDay = Convert.ToInt32(monthTime.AddMonths(-1).ToDateMonth() + "26");
                int endDay = Convert.ToInt32(dateMonth + "25");
                int yy = Convert.ToInt32(monthTime.Year + "0101");

                List<ProjectWBSDto> dayRepList = new();
                List<DayReportConstruction> allDayReportList = new();
                List<DayReportConstruction> yearList = new();
                List<DayReportConstruction> dayReportList = new();

                decimal dayRepYearAmount = 0M;
                decimal dayRepYearQuantity = 0M;
                decimal dayRepYearOut = 0M;

                if (!dayRep)
                {
                    //所有施工数据 (年 、日 、累计)
                    allDayReportList = await _dbContext.Queryable<DayReportConstruction>()
                      .Where(t => t.IsDelete == 1 && t.DateDay <= endDay && pId == t.ProjectId)
                      .ToListAsync();

                    //年
                    yearList = allDayReportList
                      .Where(t => t.IsDelete == 1 && t.DateDay >= yy && t.DateDay <= endDay && pId == t.ProjectId)
                      .ToList();

                    //获取月施工日报数据
                    dayReportList = allDayReportList
                        .Where(t => t.IsDelete == 1 && t.DateDay >= startDay && t.DateDay <= endDay && pId == t.ProjectId)
                        .ToList();

                    //自有
                    var ownDReportList = dayReportList.Where(x => x.OutPutType == ConstructionOutPutType.Self)
                        .GroupBy(x => new { x.ProjectId, x.OwnerShipId, x.UnitPrice, x.ProjectWBSId })
                        .ToList();
                    foreach (var ownRep in ownDReportList)
                    {
                        var gOwnList = dayReportList.Where(x => x.ProjectId == ownRep.Key.ProjectId && x.OwnerShipId == ownRep.Key.OwnerShipId && x.UnitPrice == ownRep.Key.UnitPrice && x.ProjectWBSId == ownRep.Key.ProjectWBSId).ToList();

                        var gYearOwnList = yearList.Where(x => x.ProjectId == ownRep.Key.ProjectId && x.OwnerShipId == ownRep.Key.OwnerShipId && x.UnitPrice == ownRep.Key.UnitPrice && x.ProjectWBSId == ownRep.Key.ProjectWBSId).ToList();

                        var gTotalOwnList = allDayReportList.Where(x => x.ProjectId == ownRep.Key.ProjectId && x.OwnerShipId == ownRep.Key.OwnerShipId && x.UnitPrice == ownRep.Key.UnitPrice && x.ProjectWBSId == ownRep.Key.ProjectWBSId).ToList();

                        dayRepList.Add(new ProjectWBSDto
                        {
                            CompleteProductionAmount = gOwnList.Sum(x => x.UnitPrice * x.ActualDailyProduction),
                            OutPutType = ConstructionOutPutType.Self,
                            CompletedQuantity = gOwnList.Sum(x => x.ActualDailyProduction),
                            UnitPrice = ownRep.Key.UnitPrice,
                            OutsourcingExpensesAmount = gOwnList.Sum(x => x.OutsourcingExpensesAmount),
                            ShipId = ownRep.Key.OwnerShipId.Value,
                            ConstructionNature = gOwnList.FirstOrDefault()?.ConstructionNature,
                            ProjectId = ownRep.Key.ProjectId.ToString(),
                            ProjectWBSId = ownRep.Key.ProjectWBSId,
                            DateMonth = dateMonth.Value,
                            IsAllowDelete = true,
                            IsDayRep = true,
                            DateYear = Convert.ToInt32(dateMonth.ToString().Substring(0, 4)),
                            ValueType = ValueEnumType.NowMonth,
                            YearCompleteProductionAmount = gYearOwnList.Sum(x => x.UnitPrice * x.ActualDailyProduction),
                            YearCompletedQuantity = gYearOwnList.Sum(x => x.ActualDailyProduction),
                            YearOutsourcingExpensesAmount = gYearOwnList.Sum(x => x.OutsourcingExpensesAmount),
                            TotalCompleteProductionAmount = gTotalOwnList.Sum(x => x.UnitPrice * x.ActualDailyProduction),
                            TotalCompletedQuantity = gTotalOwnList.Sum(x => x.ActualDailyProduction),
                            TotalOutsourcingExpensesAmount = gTotalOwnList.Sum(x => x.OutsourcingExpensesAmount),
                        });
                    }

                    //分包/往来单位
                    var otherDReportList = dayReportList.Where(x => x.OutPutType != ConstructionOutPutType.Self)
                        .GroupBy(x => new { x.ProjectId, x.SubShipId, x.UnitPrice, x.ProjectWBSId })
                        .ToList();

                    foreach (var othRep in otherDReportList)
                    {
                        var othList = dayReportList.Where(x => x.ProjectId == othRep.Key.ProjectId && x.SubShipId == othRep.Key.SubShipId && x.UnitPrice == othRep.Key.UnitPrice && x.ProjectWBSId == othRep.Key.ProjectWBSId).ToList();

                        var gYearSubList = yearList.Where(x => x.ProjectId == othRep.Key.ProjectId && x.SubShipId == othRep.Key.SubShipId && x.UnitPrice == othRep.Key.UnitPrice && x.ProjectWBSId == othRep.Key.ProjectWBSId).ToList();

                        var gTotalSubList = allDayReportList.Where(x => x.ProjectId == othRep.Key.ProjectId && x.SubShipId == othRep.Key.SubShipId && x.UnitPrice == othRep.Key.UnitPrice && x.ProjectWBSId == othRep.Key.ProjectWBSId).ToList();
                        var ShipId = Guid.Empty;
                        if (othRep != null && othRep.Key != null && othRep.Key.SubShipId != null)
                        {
                            ShipId = othRep.Key.SubShipId.Value;
                        }

                        dayRepList.Add(new ProjectWBSDto
                        {
                            CompleteProductionAmount = othList.Sum(x => x.UnitPrice * x.ActualDailyProduction),
                            OutPutType = ConstructionOutPutType.SubPackage,
                            CompletedQuantity = othList.Sum(x => x.ActualDailyProduction),
                            UnitPrice = othRep.Key.UnitPrice,
                            OutsourcingExpensesAmount = othList.Sum(x => x.OutsourcingExpensesAmount),
                            ShipId = ShipId,
                            ConstructionNature = othList.FirstOrDefault()?.ConstructionNature,
                            ProjectId = othRep.Key.ProjectId.ToString(),
                            ProjectWBSId = othRep.Key.ProjectWBSId,
                            DateMonth = dateMonth.Value,
                            IsAllowDelete = true,
                            IsDayRep = true,
                            DateYear = Convert.ToInt32(dateMonth.ToString().Substring(0, 4)),
                            ValueType = ValueEnumType.NowMonth,
                            YearCompleteProductionAmount = gYearSubList.Sum(x => x.UnitPrice * x.ActualDailyProduction),
                            YearCompletedQuantity = gYearSubList.Sum(x => x.ActualDailyProduction),
                            YearOutsourcingExpensesAmount = gYearSubList.Sum(x => x.OutsourcingExpensesAmount),
                            TotalCompleteProductionAmount = gTotalSubList.Sum(x => x.UnitPrice * x.ActualDailyProduction),
                            TotalCompletedQuantity = gTotalSubList.Sum(x => x.ActualDailyProduction),
                            TotalOutsourcingExpensesAmount = gTotalSubList.Sum(x => x.OutsourcingExpensesAmount),
                        });
                    }
                }
                #endregion

                //处理当月&历史数据
                var handleList = new List<ProjectWBSDto>();
                var endHandleList = new List<ProjectWBSDto>();
                handleList.AddRange(nowMonthReport);
                handleList.AddRange(historyMonthReport);
                if (!dayRep) handleList.AddRange(dayRepList);

                var gList = handleList.GroupBy(x => new { x.ProjectId, x.ShipId, x.UnitPrice, x.ProjectWBSId,x.Id }).ToList();
                foreach (var item in gList)
                {
                    var model = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null)
                    {
                        model.CompletedQuantity = nowMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);
                        model.CompleteProductionAmount = nowMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);
                        model.OutsourcingExpensesAmount = nowMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

                        var isAllowDelete = nowMonthReport.FirstOrDefault(t => t.ProjectId == model.ProjectId && t.ShipId == model.ShipId && t.UnitPrice == model.UnitPrice && t.ProjectWBSId == model.ProjectWBSId);
                        if (isAllowDelete != null) isAllowDelete.IsAllowDelete = true;

                        endHandleList.Add(model);
                    }
                }

                #region 如果是需要取日报的数据 如果月报中包含日报的资源数据 本月月报即使没有填 也要放开进行累加
                if (!dayRep)
                {
                    foreach (var item in endHandleList)
                    {
                        var model = dayRepList.Where(t => t.DateMonth == dateMonth && t.ProjectId == item.ProjectId && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ProjectWBSId == item.ProjectWBSId).FirstOrDefault();
                        if (model != null)
                        {
                            item.IsDayRep = true;
                            item.IsAllowDelete = true;

                            item.CompleteProductionAmount = dayReportList.Where(x => x.ProjectId.ToString() == item.ProjectId && (x.OwnerShipId == item.ShipId || x.SubShipId == item.ShipId) && x.UnitPrice == item.UnitPrice && x.ProjectWBSId == item.ProjectWBSId).Sum(x => x.UnitPrice * x.ActualDailyProduction);
                            dayRepYearAmount += model.CompleteProductionAmount;

                            item.CompletedQuantity = dayReportList.Where(x => x.ProjectId.ToString() == item.ProjectId && (x.OwnerShipId == item.ShipId || x.SubShipId == item.ShipId) && x.UnitPrice == item.UnitPrice && x.ProjectWBSId == item.ProjectWBSId).Sum(x => x.ActualDailyProduction);
                            dayRepYearQuantity += model.CompletedQuantity;

                            item.OutsourcingExpensesAmount = dayReportList.Where(x => x.ProjectId.ToString() == item.ProjectId && (x.OwnerShipId == item.ShipId || x.SubShipId == item.ShipId) && x.UnitPrice == item.UnitPrice && x.ProjectWBSId == item.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);
                            dayRepYearOut += model.OutsourcingExpensesAmount;
                        }
                    }
                }
                #endregion

                #region 本月是否有填写数据
                foreach (var item in endHandleList)
                {
                    var model = nowMonthReport.Where(t => t.DateMonth == dateMonth && t.ProjectId == item.ProjectId && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ProjectWBSId == item.ProjectWBSId).FirstOrDefault();
                    if (model == null && item.IsDayRep == false) { item.CompletedQuantity = 0M; item.CompleteProductionAmount = 0M; item.OutsourcingExpensesAmount = 0M; }
                }
                #endregion

                //WBS树追加月报明细树 追加历史的月报详细数据
                if (endHandleList != null && endHandleList.Any()) pWBS.AddRange(endHandleList);

                /***
                 * 当年数据，值不做处理 类型字段区分字段valuetype改为当年
                 * 1.截取年份字段
                 */

                string year = dateMonth.ToString().Substring(0, 4);
                int startMonth = Convert.ToInt32(year + "01");
                var nowYearMonthReport = new List<ProjectWBSDto>();
                var yReport = new List<ProjectWBSDto>();
                foreach (var nowYear in calculatePWBS.Where(x => x.DateMonth >= startMonth && x.DateMonth <= dateMonth).ToList())
                {
                    nowYearMonthReport.Add(new ProjectWBSDto
                    {
                        ValueType = ValueEnumType.NowYear,
                        CompleteProductionAmount = nowYear.CompleteProductionAmount,
                        CompletedQuantity = nowYear.CompletedQuantity,
                        UnitPrice = nowYear.UnitPrice,
                        ConstructionNature = nowYear.ConstructionNature,
                        ContractAmount = nowYear.ContractAmount,
                        DateMonth = nowYear.DateMonth,
                        DateYear = nowYear.DateYear,
                        EngQuantity = nowYear.EngQuantity,
                        Id = nowYear.Id,
                        KeyId = nowYear.KeyId,
                        Name = nowYear.Name,
                        DetailId = nowYear.DetailId,
                        OutPutType = nowYear.OutPutType,
                        OutsourcingExpensesAmount = nowYear.OutsourcingExpensesAmount,
                        Pid = nowYear.Pid,
                        ProjectId = nowYear.ProjectId,
                        ProjectWBSId = nowYear.ProjectWBSId,
                        Remark = nowYear.Remark,
                        ShipId = nowYear.ShipId
                    });
                }

                //WBS树追加月报明细树 追加当年的月报详细数据
                foreach (var item in gList)
                {
                    dayRepYearAmount = 0M;
                    dayRepYearQuantity = 0M;
                    dayRepYearOut = 0M;

                    var model = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null)
                    {
                        if (IsExistZcAndMp)//如果没有暂存 没有填报
                        {
                            dayRepYearAmount = dayReportList.Where(x => x.ProjectId.ToString() == model.ProjectId && (x.OwnerShipId == model.ShipId || x.SubShipId == model.ShipId) && x.UnitPrice == model.UnitPrice && x.ProjectWBSId == model.ProjectWBSId).Sum(x => x.UnitPrice * x.ActualDailyProduction);

                            dayRepYearQuantity = dayReportList.Where(x => x.ProjectId.ToString() == model.ProjectId && (x.OwnerShipId == model.ShipId || x.SubShipId == model.ShipId) && x.UnitPrice == model.UnitPrice && x.ProjectWBSId == model.ProjectWBSId).Sum(x => x.ActualDailyProduction);

                            dayRepYearOut = dayReportList.Where(x => x.ProjectId.ToString() == model.ProjectId && (x.OwnerShipId == model.ShipId || x.SubShipId == model.ShipId) && x.UnitPrice == model.UnitPrice && x.ProjectWBSId == model.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);
                        }

                        //合并计算 每条资源的年产值
                        model.YearCompleteProductionAmount = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount) + dayRepYearAmount;

                        model.YearCompletedQuantity = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity) + dayRepYearQuantity;

                        model.YearOutsourcingExpensesAmount = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount) + dayRepYearOut;

                        yReport.Add(model);
                    }
                }
                if (yReport != null && yReport.Any()) pWBS.AddRange(yReport);

                /***
                 * 追加开累数据 calculatePWBS
                 */

                var calPwbs = new List<ProjectWBSDto>();
                foreach (var item in gList)
                {
                    dayRepYearAmount = 0M;
                    dayRepYearQuantity = 0M;
                    dayRepYearOut = 0M;

                    var model = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null)
                    {
                        if (IsExistZcAndMp)//如果没有暂存 没有填报
                        {
                            dayRepYearAmount = dayReportList.Where(x => x.ProjectId.ToString() == model.ProjectId && (x.OwnerShipId == model.ShipId || x.SubShipId == model.ShipId) && x.UnitPrice == model.UnitPrice && x.ProjectWBSId == model.ProjectWBSId).Sum(x => x.UnitPrice * x.ActualDailyProduction);

                            dayRepYearQuantity = dayReportList.Where(x => x.ProjectId.ToString() == model.ProjectId && (x.OwnerShipId == model.ShipId || x.SubShipId == model.ShipId) && x.UnitPrice == model.UnitPrice && x.ProjectWBSId == model.ProjectWBSId).Sum(x => x.ActualDailyProduction);

                            dayRepYearOut = dayReportList.Where(x => x.ProjectId.ToString() == model.ProjectId && (x.OwnerShipId == model.ShipId || x.SubShipId == model.ShipId) && x.UnitPrice == model.UnitPrice && x.ProjectWBSId == model.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);
                        }
                        if (!pianchaMonth)
                        {
                            //合并计算 每条资源的累计值
                            model.TotalCompleteProductionAmount = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount) + dayRepYearAmount;

                            model.TotalCompletedQuantity = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity) + dayRepYearQuantity;

                            model.TotalOutsourcingExpensesAmount = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount) + dayRepYearOut;
                        }
                        else
                        {
                            //计算202412修复的开累值  
                            var amount = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => Convert.ToDecimal(x.ActualCompAmount));
                            var quantity = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => Convert.ToDecimal(x.ActualCompQuantity));
                            var outAmount = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => Convert.ToDecimal(x.CurrencyOutsourcingExpensesAmount));

                            //如果是从开累按钮进入列表
                            if (exhaustedBtn)
                            {
                                // 不加当月日报作为新的累计数
                                model.TotalCompleteProductionAmount = amount;
                                model.TotalCompletedQuantity = quantity;
                                model.TotalOutsourcingExpensesAmount = outAmount;

                                model.OldCurrencyHValue = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);
                                model.OldHQuantity = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);
                                model.OldCurrencyHOutValue = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

                                model.ActualCompAmount = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.ActualCompAmount);
                                model.ActualCompQuantity = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.ActualCompQuantity);
                                model.ActualOutAmount = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CurrencyOutsourcingExpensesAmount);
                                model.RMBHOutValue = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.RMBHOutValue);
                                model.RMBHValue = historyKaiLei.Where(t => t.ProjectId.ToString() == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.RMBHValue);
                            }
                            else
                            {
                                // + 之后月的累计月 + 日报数作为预算累计数
                                model.TotalCompleteProductionAmount = amount + calculatePWBS.Where(t => t.DateMonth > 202412 && t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount) + dayRepYearAmount;

                                model.TotalCompletedQuantity = quantity + calculatePWBS.Where(t => t.DateMonth > 202412 && t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity) + dayRepYearQuantity;

                                model.TotalOutsourcingExpensesAmount = outAmount + calculatePWBS.Where(t => t.DateMonth > 202412 && t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount) + dayRepYearOut;
                            }
                        }
                        calPwbs.Add(model);
                    }
                }
                pWBS.AddRange(calPwbs);
            }

            return pWBS;
        }
        /// <summary>
        /// 项目结构调整后的wbs数据不包含
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        private async Task<List<ProjectWBSDto>> HandleWBSDataAsync(Guid pId, int? dateMonth)
        {
            var pWBSList = new List<ProjectWBSDto>();

            var wbsHsitoryList = await _dbContext.Queryable<ProjectWbsHistoryMonth>()
                .Where(p => p.ProjectId == pId.ToString())
                .Select(p => new ProjectWBSDto
                {
                    EngQuantity = p.EngQuantity,//wbs初始的工程量
                    KeyId = p.KeyId,
                    Name = p.Name,
                    Id = p.Id,
                    Pid = p.Pid,
                    ValueType = ValueEnumType.None,
                    ProjectId = p.ProjectId,
                    ProjectWBSId = p.Id,
                    ProjectWBSName = p.Name,
                    IsDelete = p.IsDelete,
                    DateMonth = SqlFunc.ToInt32(p.DateMonth),
                    UnitPrice = p.UnitPrice,//wbs初始的单价
                    ContractAmount = p.EngQuantity * p.UnitPrice//WBS的初始合同产值=工程量*单价
                })
                .ToListAsync();

            /***
             * 1.如果当前项目没有删除过wbs数据那么返回最新的wbs数据
             * 2.如果存在wbs当月删除数据 取wbs删除的数据 且 追加新增的最新wbs树  
             * 3.获取删除后的小于当月的最大的wbs数据 且 追加新增的最新wbs树
             * 4.如果没有小于当月的删除的数据 那么当月的所有数据(包含已删除的)就是之前的全部wbs树 且 追加新增的最新wbs树
             * 5.否则 获取最新的wbs数据
             */

            if (wbsHsitoryList.Any())
            {
                var deleteMonthList = wbsHsitoryList.Where(x => x.DateMonth == dateMonth && x.IsDelete == 1)
                    .ToList();

                if (deleteMonthList != null && deleteMonthList.Any())
                {
                    //取当月删除的数据
                    pWBSList.AddRange(deleteMonthList);

                    //唯一键
                    var pIds = pWBSList.Select(x => x.Id)
                        .ToList();

                    //追加最新的wbs数据
                    var nPWBSList = await _dbContext.Queryable<ProjectWBS>()
                                 .Where(p => !string.IsNullOrEmpty(p.ProjectId) && SqlFunc.ToGuid(p.ProjectId) == pId && !pIds.Contains(p.Id))
                                 .Select(p => new ProjectWBSDto
                                 {
                                     EngQuantity = p.EngQuantity,//wbs初始的工程量
                                     KeyId = p.KeyId,
                                     Name = p.Name,
                                     Id = p.Id,
                                     Pid = p.Pid,
                                     ValueType = ValueEnumType.None,
                                     ProjectId = p.ProjectId,
                                     ProjectWBSId = p.Id,
                                     IsDelete = p.IsDelete,
                                     ProjectWBSName = p.Name,
                                     UnitPrice = p.UnitPrice,//wbs初始的单价
                                     ContractAmount = p.EngQuantity * p.UnitPrice//WBS的初始合同产值=工程量*单价
                                 })
                                 .ToListAsync();

                    pWBSList.AddRange(nPWBSList);

                    return pWBSList;
                }
                else
                {
                    var deleteWBSList = wbsHsitoryList.Where(x => x.DateMonth < dateMonth)
                        .ToList();

                    if (deleteWBSList != null && deleteWBSList.Any())
                    {
                        //唯一键
                        var pIds = pWBSList.Select(x => x.Id)
                            .ToList();

                        //追加最新的wbs数据
                        var nPWBSList = await _dbContext.Queryable<ProjectWBS>()
                                     .Where(p => !string.IsNullOrEmpty(p.ProjectId) && SqlFunc.ToGuid(p.ProjectId) == pId && !pIds.Contains(p.Id))
                                     .Select(p => new ProjectWBSDto
                                     {
                                         EngQuantity = p.EngQuantity,//wbs初始的工程量
                                         KeyId = p.KeyId,
                                         Name = p.Name,
                                         Id = p.Id,
                                         Pid = p.Pid,
                                         ValueType = ValueEnumType.None,
                                         ProjectId = p.ProjectId,
                                         ProjectWBSId = p.Id,
                                         ProjectWBSName = p.Name,
                                         IsDelete = p.IsDelete,
                                         UnitPrice = p.UnitPrice,//wbs初始的单价
                                         ContractAmount = p.EngQuantity * p.UnitPrice//WBS的初始合同产值=工程量*单价
                                     })
                                     .ToListAsync();

                        pWBSList.AddRange(nPWBSList);

                        return pWBSList;
                    }
                    else
                    {
                        int maxDateMonth = wbsHsitoryList.Max(x => x.DateMonth);

                        //历史删除的全部数据
                        pWBSList = wbsHsitoryList.Where(x => x.DateMonth == maxDateMonth)
                            .ToList();

                        //唯一键
                        var pIds = pWBSList.Select(x => x.Id)
                            .ToList();

                        //追加最新的wbs数据
                        var nPWBSList = await _dbContext.Queryable<ProjectWBS>()
                                     .Where(p => !string.IsNullOrEmpty(p.ProjectId) && SqlFunc.ToGuid(p.ProjectId) == pId && !pIds.Contains(p.Id))
                                     .Select(p => new ProjectWBSDto
                                     {
                                         EngQuantity = p.EngQuantity,//wbs初始的工程量
                                         KeyId = p.KeyId,
                                         Name = p.Name,
                                         Id = p.Id,
                                         Pid = p.Pid,
                                         ValueType = ValueEnumType.None,
                                         ProjectId = p.ProjectId,
                                         ProjectWBSId = p.Id,
                                         ProjectWBSName = p.Name,
                                         IsDelete = p.IsDelete,
                                         UnitPrice = p.UnitPrice,//wbs初始的单价
                                         ContractAmount = p.EngQuantity * p.UnitPrice//WBS的初始合同产值=工程量*单价
                                     })
                                     .ToListAsync();

                        pWBSList.AddRange(nPWBSList);

                        return pWBSList;

                    }
                }
            }

            pWBSList = await _dbContext.Queryable<ProjectWBS>()
               .Where(p => !string.IsNullOrEmpty(p.ProjectId) && SqlFunc.ToGuid(p.ProjectId) == pId)
               .Select(p => new ProjectWBSDto
               {
                   EngQuantity = p.EngQuantity,//wbs初始的工程量
                   KeyId = p.KeyId,
                   Name = p.Name,
                   Id = p.Id,
                   Pid = p.Pid,
                   ValueType = ValueEnumType.None,
                   ProjectId = p.ProjectId,
                   ProjectWBSId = p.Id,
                   ProjectWBSName = p.Name,
                   IsDelete = p.IsDelete,
                   UnitPrice = p.UnitPrice,//wbs初始的单价
                   ContractAmount = p.EngQuantity * p.UnitPrice//WBS的初始合同产值=工程量*单价
               })
               .ToListAsync();
            return pWBSList;
        }
        #endregion
        /// <summary>
        /// 月报产报列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<MonthReportForProjectResponseDto>> SearchMonthReportForProjectAsync(ProjectMonthReportRequestDto model)
        {
            var responseAjaxResult = new ResponseAjaxResult<MonthReportForProjectResponseDto>();
            var result = new MonthReportForProjectResponseDto();
            model.ResetModelProperty();

            //当前时间月份
            var nowDateMonth = GetDefaultReportDateMonth();
            var dateMonth = model.DateMonth ?? nowDateMonth;


            /***
             * 1.获取项目字段信息 
             * 2.取项目月报字段信息
             * 3.其他定义字段
             * 4.取树基础数据
             * 5.取树
             */

            #region 获取项目字段信息

            //获取项目基本信息
            var project = await _dbContext.Queryable<Project>()
                .Where(x => x.IsDelete == 1 && x.Id == model.ProjectId)
                .FirstAsync();
            if (project == null) { responseAjaxResult.FailResult(HttpStatusCode.ParameterError, "找不到项目"); return responseAjaxResult; }

            //获取当前项目币种
            result.CurrencyId = project.CurrencyId.Value;

            //项目id
            result.ProjectId = project.Id;

            //获取当前项目汇率
            int year = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
            var currencyConverterList = await _dbContext.Queryable<CurrencyConverter>().Where(t => t.CurrencyId == project.CurrencyId.ToString() && t.IsDelete == 1).ToListAsync();
            var currencyConverter = currencyConverterList.FirstOrDefault(x => x.Year == year);
            if (currencyConverter == null || currencyConverter.ExchangeRate == null) { responseAjaxResult.FailResult(HttpStatusCode.ParameterError, "汇率数据不存在"); return responseAjaxResult; }
            result.CurrencyExchangeRate = (decimal)currencyConverter.ExchangeRate;
            #endregion

            #region 处理调整后的开累数针对国外项目
            if (dateMonth > 202412)
            {
                if (project.CurrencyId != "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())
                {
                    result.RMBTotalAmount = await _dbContext.Queryable<MonthReport>().Where(x => x.DateMonth > 202412 && x.DateMonth <= dateMonth && x.ProjectId == project.Id).SumAsync(x => x.CompleteProductionAmount)
                        +
                        await _dbContext.Queryable<MonthReportDetailHistory>().Where(x => x.ProjectId == project.Id).SumAsync(x => Convert.ToDecimal(x.RMBHValue));

                    result.RMBTotalOutAmount = await _dbContext.Queryable<MonthReport>().Where(x => x.DateMonth > 202412 && x.DateMonth <= dateMonth && x.ProjectId == project.Id).SumAsync(x => x.OutsourcingExpensesAmount)
                        +
                        await _dbContext.Queryable<MonthReportDetailHistory>().Where(x => x.ProjectId == project.Id).SumAsync(x => Convert.ToDecimal(x.RMBHOutValue));
                }
            }
            #endregion

            #region 取项目月报字段信息

            //截止到当前月份的所有月报信息（目的：统计年累、开累）
            var mpData = await _dbContext.Queryable<MonthReport>()
                .Where(x => x.IsDelete == 1 && x.ProjectId == result.ProjectId)
                .ToListAsync();
            var monthReportData = mpData
                .Where(x => x.IsDelete == 1 && x.DateMonth != 202306 && x.DateMonth <= dateMonth && x.ProjectId == result.ProjectId)
                .ToList();

            //获取当前月报基本信息
            var monthReport = monthReportData.FirstOrDefault(x => x.DateMonth == dateMonth);

            //截止到当前月份的月报明细信息
            var monthReportDetailsData = await _dbContext.Queryable<MonthReportDetail>()
                .Where(x => x.IsDelete == 1 && x.ProjectId == model.ProjectId)
                .ToListAsync();

            //当月月报明细
            var monthReportDetails = monthReport != null ? monthReportDetailsData.Where(x => x.IsDelete == 1 && x.DateMonth == dateMonth && x.MonthReportId == monthReport.Id)
                .ToList() : new List<MonthReportDetail> { };

            //当年月报明细
            var yMonthReports = monthReportData.Where(x => x.DateYear == year)
                .ToList();
            var yMonthReportIds = yMonthReports.Select(x => x.Id)
                .ToList();
            var yMonthReportDetails = monthReportDetailsData.Where(x => x.IsDelete == 1 && x.DateYear == year && yMonthReportIds.Contains(x.MonthReportId))
                .ToList();

            //本月&年&累计完成产值和
            result.CompleteProductionAmount = result.CurrencyExchangeRate == 0M ? 0 : monthReportDetails.Sum(x => x.CompleteProductionAmount) / result.CurrencyExchangeRate;
            result.TotalProductionAmount = result.CurrencyExchangeRate == 0M ? 0 : yMonthReportDetails.Sum(x => x.CompleteProductionAmount) / result.CurrencyExchangeRate;
            result.TotalCompleteProductionAmount = result.CurrencyExchangeRate == 0M ? 0 : monthReportDetailsData.Sum(x => x.CompleteProductionAmount) / result.CurrencyExchangeRate;

            //本月&年&累计完成产量和
            result.CompletedQuantity = monthReportDetails.Sum(x => x.CompletedQuantity);
            result.YearCompletedQuantity = yMonthReportDetails.Sum(x => x.CompletedQuantity);
            result.TotalCompletedQuantity = monthReportDetailsData.Sum(x => x.CompletedQuantity);

            //累计&本年外包支出
            result.TotalOutsourcingExpensesAmount = result.CurrencyExchangeRate == 0M ? 0 : monthReportDetailsData.Sum(x => x.OutsourcingExpensesAmount) / result.CurrencyExchangeRate;
            result.YearOutsourcingExpensesAmount = result.CurrencyExchangeRate == 0M ? 0 : yMonthReportDetails.Sum(x => x.OutsourcingExpensesAmount) / result.CurrencyExchangeRate;

            //累计合同工程量&合同产值
            //获取wbs
            var wbs = await _dbContext.Queryable<ProjectWBS>().Where(x => x.IsDelete == 1 && x.ProjectId == model.ProjectId.ToString())
                .ToListAsync();
            result.ContractQuantity = wbs.Sum(x => x.EngQuantity);
            result.ContractAmount = wbs.Sum(x => (x.UnitPrice * x.EngQuantity));

            //历史甲方确认产值、付款金额
            var historys = await GetProjectProductionValue(model.ProjectId, dateMonth == null ? 0 : dateMonth);

            //本月&年&开累甲方确认产值(元)
            result.PartyAConfirmedProductionAmount = monthReport == null ? 0M : monthReport.PartyAConfirmedProductionAmount;
            result.CurrentYearOffirmProductionValue = historys.Item1;
            result.TotalYearKaileaOffirmProductionValue = historys.Item2;

            //本月&年&开累甲方付款金额(元)
            result.PartyAPayAmount = monthReport == null ? 0M : monthReport.PartyAPayAmount;
            result.CurrenYearCollection = historys.Item3;
            result.TotalYearCollection = historys.Item4;

            #region 追加历史的外包支出、工程量
            var his = mpData.FirstOrDefault(x => x.DateMonth == 202306);
            result.TopTitleName = "旧系统开累数据（2023年7月前）";

            if (his != null)
            {
                if (!model.ExhaustedBtn)
                {
                    result.HOutValue = his.OutsourcingExpensesAmount;
                    result.HQuantity = his.CompletedQuantity;
                    result.HValue = his.CompleteProductionAmount;
                    result.CurrencyHValue = his.CurrencyCompleteProductionAmount;
                    result.CurrencyOutHValue = his.CurrencyOutsourcingExpensesAmount;
                    //是否计算是统计的偏差月
                    bool pianchaMonth = dateMonth > 202412 ? true : false;
                    if (pianchaMonth)
                    {
                        result.HOutValue = his.RMBHOutValue;
                        result.HQuantity = his.ActualCompCompletedQuantity;
                        result.HValue = his.RMBHValue;
                        result.CurrencyHValue = his.ActualCompAmount;
                        result.CurrencyOutHValue = his.ActualCompHOutValue;
                        if (project.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//人民币
                        {
                            result.HValue = his.ActualCompAmount;
                            result.HOutValue = his.ActualCompHOutValue;
                        }
                    }
                }
                result.OldHQuantity = his.CompletedQuantity;
                result.OldHOutValue = his.OutsourcingExpensesAmount;
                result.OldCurrencyHOutValue = his.CurrencyOutsourcingExpensesAmount;
                result.OldHValue = his.CompleteProductionAmount;
                result.OldCurrencyHValue = his.CurrencyCompleteProductionAmount;

                //top 根节点202306历史数据置顶  
                result.TopCurrencyHOutValue = his.ActualCompHOutValue;
                result.TopHQuantity = his.ActualCompCompletedQuantity;
                result.TopCurrencyHValue = his.ActualCompAmount;
                result.TopRMBHValue = his.RMBHValue;
                result.TopRMBHOutValue = his.RMBHOutValue;
            }
            #endregion

            //本年产值计划
            result.TotalRollingPlanAmount = yMonthReports.Sum(p => p.RollingPlanForNextMonth);

            //本年累计产值
            result.YearTotalProductionAmount = result.CurrencyExchangeRate == 0M ? 0 : yMonthReports.Sum(x => x.CompleteProductionAmount) / result.CurrencyExchangeRate;

            //是否获取项目日报数据统计作为当月月报估算值  如果当月月报没有填的情况下 系统计算上月26（含）至传入月25日产值日报
            bool dayRep = false;
            bool IsExistZcAndMp = false;//是否暂存与月报都没有填 false 不处理  true没填

            if (monthReport != null)
            {
                //填过月报   不进行统计
                dayRep = true;

                //月度应收账款(元)
                result.ReceivableAmount = monthReport.ReceivableAmount;

                //下月滚动计划
                result.RollingPlanForNextMonth = monthReport.RollingPlanForNextMonth;

                //进度偏差主因
                result.ProgressDeviationReason = monthReport.ProgressDeviationReason;

                //进度偏差原因简述
                result.ProgressDeviationDescription = monthReport.ProgressDeviationDescription;

                //主要形象进度描述
                result.ProgressDescription = monthReport.ProgressDescription;

                //下月预计成本(元)
                result.NextMonthEstimateCostAmount = monthReport.NextMonthEstimateCostAmount;

                //本月实际成本(元)
                result.CostAmount = monthReport.CostAmount;

                //成本偏差主因
                result.CostDeviationReason = monthReport.CostDeviationReason;

                //成本偏差原因简述
                result.CostDeviationDescription = monthReport.CostDeviationDescription;

                //存在问题简述
                result.ProblemDescription = monthReport.ProblemDescription;

                //解决事项简述
                result.SolveProblemDescription = monthReport.SolveProblemDescription;

                //需公司协调事项
                result.CoordinationMatters = monthReport.CoordinationMatters;
            }
            #endregion

            #region 其他定义字段

            //任务提交类型
            result.JobSubmitType = monthReport != null && monthReport.Status == MonthReportStatus.ApproveReject ? JobSubmitType.ResetJob : JobSubmitType.AddJob;

            //状态
            result.ModelState = monthReport == null ? ModelState.Add : ModelState.Update;

            //任务id
            result.JobId = monthReport?.JobId;

            // 月报状态
            result.Status = monthReport?.Status ?? MonthReportStatus.None;
            // 月报状态文本
            result.StatusText = string.Empty;
            if (monthReport != null && monthReport.Status != MonthReportStatus.Finish)
            { result.StatusText = monthReport.Status == MonthReportStatus.ApproveReject ? monthReport.StatusText + ",原因：" + monthReport.RejectReason : monthReport.StatusText; }

            // 是否从暂存中取的业务数据
            result.IsFromStaging = false;

            //月报是否暂存
            result.IsCanStaging = false;

            //是否可提交
            result.IsCanSubmit = await IsCanSubmitAsync(monthReport, dateMonth, nowDateMonth);
            var stagingData = await _dbContext.Queryable<StagingData>().FirstAsync(t => t.BizType == StagingBizType.SaveMonthReport && t.ProjectId == model.ProjectId && t.DateMonth == dateMonth && t.IsDelete == 1);

            result.DateMonth = dateMonth;

            //取树基础数据
            var bData = await GetBaseDataAsync();

            //取树
            List<ProjectWBSDto> treeDetails = new List<ProjectWBSDto>();

            // 判断业务来源暂存
            if (result.IsCanSubmit && dateMonth == nowDateMonth)
            {
                result.IsCanStaging = true;
                if (stagingData == null && monthReport == null)//没有暂存数据 没有填报
                {
                    dayRep = false; IsExistZcAndMp = true;
                }
                else if (stagingData == null || (!stagingData.IsEffectStaging) || stagingData.BizData == null) { }
                else
                {
                    result.IsFromStaging = true; result.StatusText = "暂存中";

                    if (!string.IsNullOrWhiteSpace(stagingData.BizData))
                    {
                        // 解析 JSON 字符串为 JObject
                        var jsonObject = JObject.Parse(stagingData.BizData);

                        // 获取所有的 ReportDetails 数据
                        var jsonString = GetAllReportDetails(jsonObject).ToJson();

                        var resList = JsonConvert.DeserializeObject<List<ProjectWBSDto>>(jsonString);

                        if (resList != null && resList.Any())
                        {
                            var wbss = await _dbContext.Queryable<ProjectWBS>().Where(x => x.ProjectId == model.ProjectId.ToString()).ToListAsync();
                            //历史记录
                            var tIds = resList.Where(x => x.IsAllowDelete).Select(x => x.DetailId).ToList();
                            foreach (var item in resList)
                            {
                                item.IsAllowDelete = true;
                                item.ProjectId = result.ProjectId.ToString();

                                item.KeyId = wbss.FirstOrDefault(x => x.Id == item.ProjectWBSId)?.KeyId;
                                item.IsDelete = wbss.FirstOrDefault(x => x.Id == item.ProjectWBSId).IsDelete;

                                //item.YearCompleteProductionAmount = item.CompleteProductionAmount;
                                //item.YearCompletedQuantity = item.CompletedQuantity;
                                //item.YearOutsourcingExpensesAmount = item.OutsourcingExpensesAmount;

                                //item.TotalCompleteProductionAmount = item.CompleteProductionAmount;
                                //item.TotalCompletedQuantity = item.CompletedQuantity;
                                //item.TotalOutsourcingExpensesAmount = item.OutsourcingExpensesAmount;
                            }
                            dayRep = true;
                            treeDetails = await WBSConvertTree(model.ProjectId, dateMonth, bData, result.IsFromStaging, resList, dayRep, IsExistZcAndMp, model.ExhaustedBtn);
                            //树组合
                            result.TreeDetails = treeDetails;

                            #region 下一步 字段 
                            var saveModel = CastDeserializeObject<SaveProjectMonthReportRequestDto>(stagingData.BizData);
                            result.PartyAConfirmedProductionAmount = saveModel.PartyAConfirmedProductionAmount;
                            result.PartyAPayAmount = saveModel.PartyAPayAmount;
                            result.ReceivableAmount = saveModel.ReceivableAmount;
                            result.ProgressDeviationReason = saveModel.ProgressDeviationReason;
                            result.ProgressDescription = saveModel.ProgressDescription;
                            result.CostAmount = saveModel.CostAmount;
                            result.CostDeviationReason = saveModel.CostDeviationReason;
                            result.NextMonthEstimateCostAmount = saveModel.NextMonthEstimateCostAmount;
                            result.ProgressDeviationDescription = saveModel.ProgressDeviationDescription;
                            result.CostDeviationDescription = saveModel.CostDeviationDescription;
                            result.CoordinationMatters = saveModel.CoordinationMatters;
                            result.ProblemDescription = saveModel.ProblemDescription;
                            result.SolveProblemDescription = saveModel.SolveProblemDescription;
                            result.RollingPlanForNextMonth = saveModel.RollingPlanForNextMonth;
                            #endregion

                            //数据回显
                            responseAjaxResult.SuccessResult(result);
                            return responseAjaxResult;
                        }
                    }
                }
            }
            #endregion

            //取树
            treeDetails = await WBSConvertTree(result.ProjectId, dateMonth, bData, result.IsFromStaging, new List<ProjectWBSDto>(), dayRep, IsExistZcAndMp, model.ExhaustedBtn);

            //树组合
            result.TreeDetails = treeDetails;

            //数据回显
            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 强制转换
        /// </summary>
        /// <exception cref="Exception">强制转换异常</exception>
        private T CastDeserializeObject<T>(string? bizData) where T : class
        {
            if (string.IsNullOrWhiteSpace(bizData))
            {
                throw new Exception("bizData不能为空");
            }
            var model = JsonConvert.DeserializeObject<T>(bizData);
            if (model == null)
            {
                throw new Exception("bizData反序列化失败");
            }
            return model;
        }
        /// <summary>
        /// 获取所有ReportDetails
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private List<JToken> GetAllReportDetails(JObject obj)
        {
            // 使用 LINQ 查询 JObject 的所有子节点 reportDetails
            return obj.Descendants()
                      // 筛选出所有类型为 JProperty 且名称为 "ReportDetails" 的节点
                      .Where(t => t.Type == JTokenType.Property && (((JProperty)t).Name == "ReportDetails" || ((JProperty)t).Name == "reportDetails"))
                      // 对于每个匹配的 JProperty，获取其值中的所有子节点
                      .SelectMany(t => ((JProperty)t).Value.Children())
                      // 转换为 List<JToken> 并返回
                      .ToList();
        }
        /// <summary>
        /// 处理并获取施工性质、产值属性、资源基础数据
        /// </summary>
        /// <returns></returns>
        private async Task<List<MonthReportForProjectBaseDataResponseDto>> GetBaseDataAsync()
        {
            var bData = new List<MonthReportForProjectBaseDataResponseDto>();

            /***
             * 1.字典数据处理
             * 2.施工性质基础数据
             * 3.产值属性基础数据
             * 4.资源基础数据
             * 5.往来单位在响应树做处理
             */

            //字典表处理
            var diData = await _dbContext.Queryable<DictionaryTable>()
                .Where(t => (t.TypeNo == (int)DictionaryTypeNo.ConstructionNature || t.TypeNo == (int)DictionaryTypeNo.CustomShip) && t.IsDelete == 1)
                .Select(t => new
                {
                    Id = t.Id,
                    Name = t.Name,
                    Type = t.Type,
                    TypeNo = t.TypeNo
                })
                .ToListAsync();

            //施工性质
            var constructionNatures = diData.Where(t => t.TypeNo == (int)DictionaryTypeNo.ConstructionNature)
                .Select(t => new MonthReportForProjectBaseDataResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    RouseType = RouseType.ConstructionNature,
                    ConstructionNatureType = t.Type
                })
                .ToList();
            if (constructionNatures != null && constructionNatures.Any()) bData.AddRange(constructionNatures);

            //自定义产值属性
            bData.Add(new MonthReportForProjectBaseDataResponseDto { Id = Guid.Empty, Name = "自有", ShipRouseType = ConstructionOutPutType.Self, RouseType = RouseType.Self });
            bData.Add(new MonthReportForProjectBaseDataResponseDto { Id = Guid.Empty, Name = "分包", ShipRouseType = ConstructionOutPutType.SubPackage, RouseType = RouseType.Sub });

            //自有
            var ownerShips = await _dbContext.Queryable<OwnerShip>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new MonthReportForProjectBaseDataResponseDto
                {
                    Id = x.PomId,
                    Name = x.Name,
                    ShipRouseType = ConstructionOutPutType.Self,
                    RouseType = RouseType.Self
                })
                .ToListAsync();
            if (ownerShips != null && ownerShips.Any()) bData.AddRange(ownerShips);

            //分包
            var subShips = await _dbContext.Queryable<SubShip>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new MonthReportForProjectBaseDataResponseDto
                {
                    Id = x.PomId,
                    Name = x.Name,
                    ShipRouseType = ConstructionOutPutType.SubPackage,
                    RouseType = RouseType.Sub
                })
                .ToListAsync();
            if (subShips != null && subShips.Any()) bData.AddRange(subShips);

            //资源
            var dIds = await _dbContext.Queryable<MonthReportDetail>()
                .Where(x => x.IsDelete == 1)
                .Select(x => x.ShipId)
                .Distinct()
                .ToListAsync();

            var dealList = await _dbContext.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 && (x.ZBPTYPE == "02" || x.ZBPTYPE == "03"))
                 .Select(x => new MonthReportForProjectBaseDataResponseDto
                 {
                     Id = x.PomId,
                     Name = x.ZBPNAME_ZH,
                     RouseType = RouseType.Rouse
                 }).ToListAsync();

            var dealingUnits = await _dbContext.Queryable<DealingUnit>()
                .Where(x => x.IsDelete == 1 && dIds.Contains(x.PomId.Value))
                .Select(x => new MonthReportForProjectBaseDataResponseDto
                {
                    Id = x.PomId,
                    Name = x.ZBPNAME_ZH,
                    RouseType = RouseType.Rouse
                })
                .ToListAsync();
            if (dealingUnits != null && dealingUnits.Any())
            {
                bData.AddRange(dealingUnits);
                bData.AddRange(dealList);
                bData = bData.Distinct().ToList();
            }

            //资源：客户自定义
            var customResource = diData.Where(t => t.TypeNo == (int)DictionaryTypeNo.CustomShip)
               .Select(t => new MonthReportForProjectBaseDataResponseDto
               {
                   Id = t.Id,
                   Name = t.Name,
                   RouseType = RouseType.Rouse,
                   ConstructionNatureType = t.Type
               })
               .ToList();
            if (customResource != null && customResource.Any()) bData.AddRange(customResource);

            return bData;
        }
        /// <summary>
        /// 获取默认允许填报的月份
        /// </summary>
        /// <returns></returns>
        private int GetDefaultReportDateMonth()
        {
            DateTime time = DateTime.Now;
            return time.Day < 26 ? time.AddMonths(-1).ToDateMonth() : time.ToDateMonth();
        }
        /// <summary>
        ///是否允许提交
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCanSubmitAsync(MonthReport? monthReport, int dateMonth, int nowDateMonth)
        {
            if (dateMonth > nowDateMonth)
            {
                return false;
            }
            if (monthReport != null && monthReport.Status == MonthReportStatus.Approveling)
            {
                return false;
            }
            if (monthReport != null && monthReport.Status == MonthReportStatus.Finish)
            {
                var unFinishIob = await GetUnFinishMonthReportJobPartAsync(monthReport.ProjectId, monthReport.DateMonth);
                if (unFinishIob != null)
                {
                    return false;
                }
            }
            if (dateMonth == nowDateMonth && monthReport != null && monthReport.Status == MonthReportStatus.Finish)
            {
                return await _bizAuthorizeService.IsAuthorizedAsync(_currentUser.Id, BizModule.MonthReport);
            }
            if (dateMonth < nowDateMonth && (monthReport == null || monthReport.Status == MonthReportStatus.Finish))
            {
                return await _bizAuthorizeService.IsAuthorizedAsync(_currentUser.Id, BizModule.MonthReport);
            }
            return true;
        }
        /// <summary>
        /// 获取未完成项目月报任务
        /// </summary>
        /// <returns></returns>
        private async Task<Models.Job?> GetUnFinishMonthReportJobPartAsync(Guid projectId, int datemonth)
        {
            var query = _dbContext.Queryable<Models.Job>().Where(t => t.IsDelete == 1 && t.ProjectId == projectId && t.DateMonth == datemonth && t.BizModule == BizModule.MonthReport && t.IsFinish == false);
            return await query.Select(t => new Models.Job { Id = t.Id, ApproveStatus = t.ApproveStatus, IsFinish = t.IsFinish }).FirstAsync();
        }
        /// <summary>
        /// 第一个 本年甲方确认产值
        /// 第二个 开累甲方确认产值
        /// 第三个 本年甲方付款金额
        /// 第四个 开累甲方付款金额
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        public async Task<Tuple<decimal, decimal, decimal, decimal>> GetProjectProductionValue(Guid projectId, int dateMonth)
        {
            var currentYearOffirmProductionValue = 0M;
            var totalYearKaileaOffirmProductionValue = 0M;
            var currenYearCollection = 0M;
            var totalYearCollection = 0M;
            try
            {
                int currentYear = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
                //var currentYear = DateTime.Now.Year;
                var projectMonthReportHistory = await _dbContext.Queryable<ProjectMonthReportHistory>()
                   .Where(x => x.IsDelete == 1 && x.ProjectId == projectId).FirstAsync();

                var currentTotalYearOffirmProductionValue = await _dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateMonth < dateMonth && x.Status != MonthReportStatus.Revoca).ToListAsync();

                //本年甲方确认产值(当年)
                var initMonth = new DateTime(currentYear, 1, 1).ToDateMonth();
                currentYearOffirmProductionValue = currentTotalYearOffirmProductionValue.Where(x => x.DateMonth >= initMonth && x.DateMonth <= dateMonth)
                   //原来的// x.DateYear==currentYear)
                   .Sum(x => x.PartyAConfirmedProductionAmount);

                //开累甲方确认产值(历史数据+2023-7至12月的数据+2024年的数据）
                if (projectMonthReportHistory != null && currentTotalYearOffirmProductionValue.Any())
                {
                    totalYearKaileaOffirmProductionValue = projectMonthReportHistory.KaileiOwnerConfirmation.Value * 10000
                       + currentTotalYearOffirmProductionValue.Sum(x => x.PartyAConfirmedProductionAmount);
                }
                else
                {
                    totalYearKaileaOffirmProductionValue =
                            currentTotalYearOffirmProductionValue.Sum(x => x.PartyAConfirmedProductionAmount);
                }

                var currenTotalYearCollection = await _dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateYear <= currentYear && x.DateMonth < dateMonth && x.Status != MonthReportStatus.Revoca).ToListAsync();

                //本年甲方付款金额
                currenYearCollection = currenTotalYearCollection.Where(x => x.DateYear == currentYear).Sum(x => x.PartyAPayAmount);
                if (projectMonthReportHistory != null && currenTotalYearCollection.Any())
                {
                    //开累甲方付款金额
                    totalYearCollection = projectMonthReportHistory.KaileiProjectPayment.Value * 10000
                    + currenTotalYearCollection.Sum(x => x.PartyAPayAmount);
                }
                else
                {
                    //开累甲方付款金额
                    totalYearCollection = currenTotalYearCollection.Sum(x => x.PartyAPayAmount);
                }

            }
            catch (Exception ex)
            { }
            return Tuple.Create(currentYearOffirmProductionValue, totalYearKaileaOffirmProductionValue, currenYearCollection, totalYearCollection);
        }
        /// <summary>
        /// 修改月报开累数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public async Task<ResponseAjaxResult<bool>> SaveMonthReportForProjectHistoryAsync(SaveMonthReportForProjectHistoryDto model)
        {
            ResponseAjaxResult<bool> rt = new();

            if (model.ProjectHistorys != null && model.ProjectHistorys.Any())
            {
                List<MonthReportDetailHistory> rsh = new();
                List<MonthReportDetailHistory> rsh2 = new();
                var ids = model.ProjectHistorys.Select(x => x.Id).ToList();
                var rrh = await _dbContext.Queryable<MonthReportDetailHistory>().Where(t => t.IsDelete == 1 && t.ProjectId == model.ProjectId).ToListAsync();
                foreach (var item in model.ProjectHistorys)
                {
                    var f = rrh.FirstOrDefault(x => x.Id == item.Id);
                    if (f != null)
                    {
                        f.ShipId = item.ShipId;
                        f.CurrencyOutsourcingExpensesAmount = item.ActualOutAmount;
                        f.ActualCompQuantity = item.ActualCompQuantity;
                        f.ActualCompAmount = item.ActualCompAmount;
                        f.RMBHValue = item.RMBHValue;
                        f.RMBHOutValue = item.RMBHOutValue;
                        rsh.Add(f);
                    }
                }

                #region 资源处理 
                //其他资源都清空 除了传入行数据
                int dateMonth = 202412;//受控的资源 后续如果需要调整 此处设置资源 的日期   且历史表同步月报资源数据
                var rsh2Child = rrh.Where(t => t.ProjectId == model.ProjectId && !ids.Contains(t.Id) && t.DateMonth <= dateMonth).ToList();
                foreach (var ot in rsh2Child)
                {
                    ot.IsDelete = 0;
                }
                rsh2.AddRange(rsh2Child);

                if (rsh2.Any())
                {
                    await _dbContext.Updateable(rsh2).UpdateColumns(x => new
                    {
                        x.IsDelete
                    })
                    .ExecuteCommandAsync();
                }
                #endregion
                if (rsh.Any())
                {
                    await _dbContext.Updateable(rsh).WhereColumns(t => t.Id).UpdateColumns(x => new
                    {
                        x.CurrencyOutsourcingExpensesAmount,
                        x.ActualCompQuantity,
                        x.ActualCompAmount,
                        x.RMBHOutValue,
                        x.RMBHValue,
                        x.ShipId
                    })
                    .ExecuteCommandAsync();
                }

                var rr = await _dbContext.Queryable<MonthReportDetail>().Where(t => t.IsDelete == 1 && model.ProjectHistorys.Select(x => x.Id).Contains(t.Id)).ToListAsync();
                foreach (var item in rr)
                {
                    var f = model.ProjectHistorys.FirstOrDefault(x => x.Id == item.Id);
                    if (f != null)
                    {
                        item.ShipId = f.ShipId;
                    }
                }
                if (rr.Any())
                {
                    await _dbContext.Updateable(rr).UpdateColumns(x => new { x.ShipId })
                    .ExecuteCommandAsync();
                }
            }
            //202306历史月报修改
            var mainTab = await _dbContext.Queryable<MonthReport>().FirstAsync(t => t.IsDelete == 1 && model.ProjectId == t.ProjectId && t.DateMonth == 202306);
            if (mainTab != null)
            {
                mainTab.ActualCompAmount = Convert.ToDecimal(model.TopCurrencyHValue);
                mainTab.ActualCompHOutValue = Convert.ToDecimal(model.TopCurrencyHOutValue);
                mainTab.ActualCompCompletedQuantity = Convert.ToDecimal(model.TopHQuantity);
                mainTab.RMBHOutValue = Convert.ToDecimal(model.TopRMBHOutValue);
                mainTab.RMBHValue = Convert.ToDecimal(model.TopRMBHValue);
                await _dbContext.Updateable(mainTab).UpdateColumns(x => new
                {
                    x.ActualCompAmount,
                    x.ActualCompHOutValue,
                    x.ActualCompCompletedQuantity,
                    x.RMBHOutValue,
                    x.RMBHValue,
                })
                .ExecuteCommandAsync();
            }
            else// 追加202306的历史数据
            {
                var project = await _dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && t.Id == model.ProjectId).FirstAsync();
                if (project != null)
                {
                    MonthReport insertTable = new();
                    insertTable.Id = GuidUtil.Next();
                    insertTable.CurrencyId = "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid();
                    insertTable.ProjectId = project.Id;
                    insertTable.DateMonth = 202306;
                    insertTable.CompletedQuantity = Convert.ToDecimal(model.TopHQuantity);
                    insertTable.CompleteProductionAmount = Convert.ToDecimal(model.TopCurrencyHValue);
                    insertTable.OutsourcingExpensesAmount = Convert.ToDecimal(model.TopCurrencyHOutValue);
                    insertTable.ActualCompCompletedQuantity = Convert.ToDecimal(model.TopHQuantity);
                    insertTable.ActualCompAmount = Convert.ToDecimal(model.TopCurrencyHValue);
                    insertTable.ActualCompHOutValue = Convert.ToDecimal(model.TopCurrencyHOutValue);
                    if (project.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                    {
                        insertTable.RMBHValue = Convert.ToDecimal(model.TopCurrencyHValue);
                        insertTable.RMBHOutValue = Convert.ToDecimal(model.TopCurrencyHOutValue);
                        insertTable.CompleteProductionAmount = Convert.ToDecimal(model.TopCurrencyHValue);
                        insertTable.OutsourcingExpensesAmount = Convert.ToDecimal(model.TopCurrencyHOutValue);
                    }
                    else//国外
                    {
                        insertTable.RMBHValue = Convert.ToDecimal(model.TopRMBHValue);
                        insertTable.RMBHOutValue = Convert.ToDecimal(model.TopRMBHOutValue);
                        insertTable.CurrencyCompleteProductionAmount = Convert.ToDecimal(model.TopCurrencyHValue);
                        insertTable.CurrencyOutsourcingExpensesAmount = Convert.ToDecimal(model.TopCurrencyHOutValue);
                        insertTable.CompleteProductionAmount = Convert.ToDecimal(model.TopRMBHValue);
                        insertTable.OutsourcingExpensesAmount = Convert.ToDecimal(model.TopRMBHOutValue);
                    }
                    await _dbContext.Insertable(insertTable).ExecuteCommandAsync();
                }
            }
            rt.SuccessResult(true);
            return rt;
        }
        /// <summary>
        /// 启用 保存修改月报开累数据按钮  true 启用
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> EnableSaveMonthRepHistoryBtnAsync()
        {
            ResponseAjaxResult<bool> rt = new();
            bool enableBtn = false;

            var rr = await _dbContext.Queryable<MonRepHistoryMdConfig>().FirstAsync(t => t.IsDelete == 1 && t.Enable == true);
            if (_currentUser.CurrentLoginIsAdmin || _currentUser.Account == "2016146340") { enableBtn = true; return rt.SuccessResult(enableBtn); }
            if (rr != null)
            {
                //获取限制时间
                if (rr.StartTime <= DateTime.Now && DateTime.Now <= rr.EndTime)
                {
                    if (rr.UserId == "*")//所有用户
                        enableBtn = true;
                    else
                    {
                        var uIds = rr.UserId?.Split(',')
                        .Select(id => string.IsNullOrEmpty(id) ? (Guid?)null : (Guid?)Guid.Parse(id))
                        .ToList();
                        if (uIds.Contains(_currentUser.Id)) enableBtn = true;
                    }
                }
            }
            else rt.SuccessResult(enableBtn);
            return rt.SuccessResult(enableBtn);
        }
    }
}
