using Aspose.Words;
using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Collections.Generic;
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
        private async Task<List<ProjectWBSDto>> WBSConvertTree(Guid projectId, int? dateMonth, List<MonthReportForProjectBaseDataResponseDto> bData, bool isStaging, List<ProjectWBSDto> stagingList, bool dayRep)
        {
            /***
             * 1.获取请求数据
             * 2.获取当月月报数据
             * 3.获取年度月报详细
             * 4.获取开累月报详细
             * 5.获取WBS数据
             * 6.获取基础数据(施工性质、产值属性、资源)
             */
            var requestList = await GetWBSDataAsync(projectId, dateMonth, dayRep);
            var mReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowMonth).OrderBy(x => x.DateMonth).ToList();
            var yReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowYear).OrderBy(x => x.DateMonth).ToList();
            var klReportList = requestList.Where(x => x.ValueType == ValueEnumType.AccumulatedCommencement).OrderBy(x => x.DateMonth).ToList();
            var wbsList = requestList.Where(x => x.ValueType == ValueEnumType.None).ToList();

            if (isStaging)
            {
                //本月的数据为暂存的数据  清零是为了不做重复计算
                List<ProjectWBSDto> newMRep = new(); //为了合并当月月报暂存的分组
                foreach (var item in stagingList)
                {
                    var mRep = mReportList.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ProjectWBSId == item.ProjectWBSId);
                    if (mRep == null)
                    {
                        newMRep.Add(item);
                    }
                    else
                    {
                        mRep.IsAllowDelete = true;

                        mRep.CompleteProductionAmount = item.CompleteProductionAmount;
                        mRep.CompletedQuantity = item.CompletedQuantity;
                        mRep.OutsourcingExpensesAmount = item.OutsourcingExpensesAmount;

                        mRep.YearCompletedQuantity = item.YearCompletedQuantity;
                        mRep.YearCompleteProductionAmount = item.YearCompleteProductionAmount;
                        mRep.YearOutsourcingExpensesAmount = item.YearOutsourcingExpensesAmount;

                        mRep.TotalCompleteProductionAmount = item.TotalCompleteProductionAmount;
                        mRep.TotalCompletedQuantity = item.TotalCompletedQuantity;
                        mRep.TotalOutsourcingExpensesAmount = item.TotalOutsourcingExpensesAmount;
                    }
                }
                mReportList.AddRange(newMRep);
                yReportList.AddRange(stagingList);
                klReportList.AddRange(stagingList);
            }

            //获取当前项目所有的月报存在的wbsid  不包含的wbsid 全部去掉
            var mpWbsIds = await _dbContext.Queryable<MonthReportDetail>().Where(x => x.IsDelete == 1 && x.ProjectId == projectId).Select(x => x.ProjectWBSId).ToListAsync();

            //转换wbs树

            var pWbsTree = BuildTree("0", wbsList, mpWbsIds, mReportList, yReportList, klReportList, bData);

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
        /// <returns></returns>
        public List<ProjectWBSDto> BuildTree(string? rootPid, List<ProjectWBSDto> wbsList, List<Guid>? mpWbsIds, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData)
        {
            var tree = new List<ProjectWBSDto>();
            // 获取所有主节点
            var mainNodes = wbsList.Where(x => x.Pid == rootPid).ToList();

            foreach (var node in mainNodes)
            {
                // 递归获取子节点
                var children = BuildTree(node.KeyId, wbsList, mpWbsIds, mReportList, yReportList, klReportList, bData);
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
                var finallyList = MReportForProjectList(node.ProjectWBSId, values, yReportList, klReportList, bData);
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
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        private List<ProjectWBSDto> MReportForProjectList(Guid wbsId, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData)
        {
            /***
             * 1.根据(当前施工分类)wbsid获取所有资源（船舶）信息
             * 2.统计资源（每条船）年度、开累值
             */
            var mReport = mReportList.OrderBy(x => x.ShipId).ThenBy(x => x.DateMonth).Where(x => x.ProjectWBSId == wbsId).ToList();
            foreach (var report in mReport)
            {
                //年度统计
                report.YearCompleteProductionAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.YearCompleteProductionAmount);
                report.YearCompletedQuantity = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.YearCompletedQuantity);
                report.YearOutsourcingExpensesAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.YearOutsourcingExpensesAmount);

                //开累统计
                report.TotalCompleteProductionAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.TotalCompleteProductionAmount);
                report.TotalCompletedQuantity = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.TotalCompletedQuantity);
                report.TotalOutsourcingExpensesAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.TotalOutsourcingExpensesAmount);
                /***
                 * 基本信息处理
                 */
                #region 基础信息处理
                report.ConstructionNatureName = bData.FirstOrDefault(x => RouseType.ConstructionNature == x.RouseType && x.ConstructionNatureType == report.ConstructionNature)?.Name;
                report.OutPutTypeName = bData.FirstOrDefault(x => (x.RouseType == RouseType.Self || x.RouseType == RouseType.Sub) && report.OutPutType == x.ShipRouseType)?.Name;
                report.ShipName = bData.FirstOrDefault(x => x.Id == report.ShipId)?.Name;
                report.DetailId = klReportList.FirstOrDefault(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId)?.DetailId;
                #endregion
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
        private async Task<List<ProjectWBSDto>> GetWBSDataAsync(Guid pId, int? dateMonth, bool dayRep)
        {
            var pWBS = new List<ProjectWBSDto>();
            var calculatePWBS = new List<ProjectWBSDto>();

            //空的项目id  不返回数据; 一个项目对应一个wbs
            if (pId == Guid.Empty) return pWBS;

            //获取本身的项目wbs数据
            pWBS = await HandleWBSDataAsync(pId, dateMonth);

            //获取项目当年的汇率
            int yearParam = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
            var project = await _dbContext.Queryable<Project>().Where(t => t.Id == pId && t.IsDelete == 1).FirstAsync();
            var pRate = 1M;
            if (project != null)
            {
                var val = await _dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.Year == yearParam && t.CurrencyId == project.CurrencyId.ToString()).FirstAsync();
                if (val != null)
                {
                    pRate = val.ExchangeRate.Value;
                }
                else
                {
                    throw new Exception("未查到汇率");
                }
            }

            //获取需要计算的月报填报数据 
            if (dateMonth != 0 && dateMonth.ToString().Length == 6)
            {
                var mPIds = await _dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && x.DateMonth != 202306)//&& x.DateMonth != 202306
                    .Select(x => x.Id)
                    .ToListAsync();

                //获取当月前需要计算的的所有填报数据(累计的所有数据/开累)
                calculatePWBS = await _dbContext.Queryable<MonthReportDetail>()
                   .Where(p => mPIds.Contains(p.MonthReportId) && !string.IsNullOrEmpty(p.ProjectId.ToString()) && p.ProjectId != Guid.Empty && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId && p.DateMonth <= dateMonth)
                   .Select(p => new ProjectWBSDto
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
                       CompleteProductionAmount = p.CompleteProductionAmount// p.UnitPrice * p.CompletedQuantity
                   })
                   .ToListAsync();

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
                if (!dayRep)
                {
                    //所有施工数据 (年 、日 、累计)
                    var allDayReportList = await _dbContext.Queryable<DayReportConstruction>()
                       .Where(t => t.IsDelete == 1 && t.DateDay <= endDay && pId == t.ProjectId)
                       .ToListAsync();

                    //年
                    var yearList = allDayReportList
                       .Where(t => t.IsDelete == 1 && t.DateDay >= yy && t.DateDay <= endDay && pId == t.ProjectId)
                       .ToList();

                    //获取月施工日报数据
                    var dayReportList = allDayReportList
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
                            CompleteProductionAmount = gOwnList.Sum(x => x.ActualDailyProductionAmount),
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
                            DateYear = Convert.ToInt32(dateMonth.ToString().Substring(0, 4)),
                            ValueType = ValueEnumType.NowMonth,
                            YearCompleteProductionAmount = gYearOwnList.Sum(x => x.ActualDailyProductionAmount),
                            YearCompletedQuantity = gYearOwnList.Sum(x => x.ActualDailyProduction),
                            YearOutsourcingExpensesAmount = gYearOwnList.Sum(x => x.OutsourcingExpensesAmount),
                            TotalCompleteProductionAmount = gTotalOwnList.Sum(x => x.ActualDailyProductionAmount),
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

                        dayRepList.Add(new ProjectWBSDto
                        {
                            CompleteProductionAmount = othList.Sum(x => x.ActualDailyProductionAmount) / pRate,
                            OutPutType = ConstructionOutPutType.SubPackage,
                            CompletedQuantity = othList.Sum(x => x.ActualDailyProduction),
                            UnitPrice = othRep.Key.UnitPrice,
                            OutsourcingExpensesAmount = othList.Sum(x => x.OutsourcingExpensesAmount),
                            ShipId = othRep.Key.SubShipId.Value,
                            ConstructionNature = othList.FirstOrDefault()?.ConstructionNature,
                            ProjectId = othRep.Key.ProjectId.ToString(),
                            ProjectWBSId = othRep.Key.ProjectWBSId,
                            DateMonth = dateMonth.Value,
                            IsAllowDelete = true,
                            DateYear = Convert.ToInt32(dateMonth.ToString().Substring(0, 4)),
                            ValueType = ValueEnumType.NowMonth,
                            YearCompleteProductionAmount = gYearSubList.Sum(x => x.ActualDailyProductionAmount) / pRate,
                            YearCompletedQuantity = gYearSubList.Sum(x => x.ActualDailyProduction),
                            YearOutsourcingExpensesAmount = gYearSubList.Sum(x => x.OutsourcingExpensesAmount),
                            TotalCompleteProductionAmount = gTotalSubList.Sum(x => x.ActualDailyProductionAmount) / pRate,
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

                var gList = handleList.GroupBy(x => new { x.ProjectId, x.ShipId, x.UnitPrice, x.ProjectWBSId }).ToList();
                foreach (var item in gList)
                {
                    var model = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null)
                    {
                        var isAllowDelete = nowMonthReport.FirstOrDefault(t => t.ProjectId == model.ProjectId && t.ShipId == model.ShipId && t.UnitPrice == model.UnitPrice && t.ProjectWBSId == model.ProjectWBSId);
                        if (isAllowDelete != null) isAllowDelete.IsAllowDelete = true;

                        model.CompleteProductionAmount = model.CompleteProductionAmount;

                        //合并计算 每条资源的年产值、累计值、外包支出 
                        model.YearCompleteProductionAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);

                        model.YearCompletedQuantity = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);

                        model.YearOutsourcingExpensesAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

                        model.TotalCompleteProductionAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);

                        model.TotalCompletedQuantity = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);

                        model.TotalOutsourcingExpensesAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

                        endHandleList.Add(model);
                    }
                }

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
                    var model = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null)
                    {
                        model.CompleteProductionAmount = model.CompleteProductionAmount;

                        //合并计算 每条资源的年产值、累计值、外包支出 
                        model.YearCompleteProductionAmount = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);

                        model.YearCompletedQuantity = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);

                        model.YearOutsourcingExpensesAmount = nowYearMonthReport.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

                        model.TotalCompleteProductionAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);

                        model.TotalCompletedQuantity = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);

                        model.TotalOutsourcingExpensesAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

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
                    var model = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null)
                    {
                        model.CompleteProductionAmount = model.CompleteProductionAmount;

                        //合并计算 每条资源的年产值、累计值、外包支出 
                        model.YearCompleteProductionAmount = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);

                        model.YearCompletedQuantity = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);

                        model.YearOutsourcingExpensesAmount = calculatePWBS.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

                        model.TotalCompleteProductionAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompleteProductionAmount);

                        model.TotalCompletedQuantity = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.CompletedQuantity);

                        model.TotalOutsourcingExpensesAmount = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).Sum(x => x.OutsourcingExpensesAmount);

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

            if (his != null)
            {
                result.HOutValue = his.OutsourcingExpensesAmount;
                result.HQuantity = his.CompletedQuantity;
                result.HValue = currencyConverterList.FirstOrDefault(x => x.Year == his.DateYear) != null ? his.CompleteProductionAmount / currencyConverterList.FirstOrDefault(x => x.Year == his.DateYear).ExchangeRate.Value : 0M;
            }
            #endregion

            //是否获取项目日报数据统计作为当月月报估算值  如果当月月报没有填的情况下 系统计算上月26（含）至传入月25日产值日报
            bool dayRep = false;

            if (monthReport != null)
            {
                //填过月报   不进行统计
                dayRep = true;

                //月度应收账款(元)
                result.ReceivableAmount = monthReport.ReceivableAmount;

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
                #endregion
            }

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
                if (stagingData == null || (!stagingData.IsEffectStaging) || stagingData.BizData == null) { }
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
                                item.ProjectId = result.ProjectId.ToString();

                                item.KeyId = wbss.FirstOrDefault(x => x.Id == item.ProjectWBSId)?.KeyId;
                                item.IsDelete = wbss.FirstOrDefault(x => x.Id == item.ProjectWBSId).IsDelete;

                                item.YearCompleteProductionAmount = item.CompleteProductionAmount;
                                item.YearCompletedQuantity = item.CompletedQuantity;
                                item.YearOutsourcingExpensesAmount = item.OutsourcingExpensesAmount;

                                item.TotalCompleteProductionAmount = item.CompleteProductionAmount;
                                item.TotalCompletedQuantity = item.CompletedQuantity;
                                item.TotalOutsourcingExpensesAmount = item.OutsourcingExpensesAmount;
                            }
                            dayRep = true;
                            treeDetails = await WBSConvertTree(model.ProjectId, dateMonth, bData, result.IsFromStaging, resList, dayRep);
                            //树组合
                            result.TreeDetails = treeDetails;

                            //数据回显
                            responseAjaxResult.SuccessResult(result);
                            return responseAjaxResult;
                        }
                    }
                }
            }
            #endregion

            //取树
            treeDetails = await WBSConvertTree(result.ProjectId, dateMonth, bData, result.IsFromStaging, new List<ProjectWBSDto>(), dayRep);

            //树组合
            result.TreeDetails = treeDetails;

            //数据回显
            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
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
            var dealingUnits = await _dbContext.Queryable<DealingUnit>()
                .Where(x => x.IsDelete == 1 && dIds.Contains(x.PomId.Value))
                .Select(x => new MonthReportForProjectBaseDataResponseDto
                {
                    Id = x.PomId,
                    Name = x.ZBPNAME_ZH,
                    RouseType = RouseType.Rouse
                })
                .ToListAsync();
            if (dealingUnits != null && dealingUnits.Any()) bData.AddRange(dealingUnits);

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
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateMonth <= dateMonth).ToListAsync();

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
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateYear <= currentYear && x.DateMonth <= dateMonth).ToListAsync();

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
    }
}
