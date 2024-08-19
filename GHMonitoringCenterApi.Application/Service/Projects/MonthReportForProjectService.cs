﻿using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using SqlSugar;
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
        /// <returns></returns>
        private async Task<List<ProjectWBSDto>> WBSConvertTree(Guid projectId, int? dateMonth, List<MonthReportForProjectBaseDataResponseDto> bData)
        {
            /***
             * 1.获取请求数据
             * 2.获取当月月报数据
             * 3.获取年度月报详细
             * 4.获取开累月报详细
             * 5.获取WBS数据
             * 6.获取基础数据(施工性质、产值属性、资源)
             */
            var requestList = await GetWBSDataAsync(projectId, dateMonth);
            var mReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowMonth).OrderBy(x => x.DateMonth).ToList();
            var yReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowYear).OrderBy(x => x.DateMonth).ToList();
            var klReportList = requestList.Where(x => x.ValueType == ValueEnumType.AccumulatedCommencement).OrderBy(x => x.DateMonth).ToList();
            var wbsList = requestList.Where(x => x.ValueType == ValueEnumType.None).ToList();

            var pWbsTree = await GetChildren(projectId, dateMonth, "0", wbsList, mReportList, yReportList, klReportList, bData);

            //转换wbs树
            return pWbsTree;
        }
        /// <summary>
        /// 获取子集数据，处理月报详细数据逻辑
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateMonth"></param>
        /// <param name="nodePId"></param>
        /// <param name="wbsList">wbs响应体</param>
        /// <param name="mReportList">当月月报详细响应体</param>
        /// <param name="yReportList">当年月报详细响应体</param>
        /// <param name="klReportList">开累（历史）月报详细响应体</param>
        /// <param name="bData">施工性质、产值属性</param>
        /// <returns></returns>
        private async Task<List<ProjectWBSDto>> GetChildren(Guid projectId, int? dateMonth, string? nodePId, List<ProjectWBSDto> wbsList, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData)
        {
            var mainNodes = wbsList.Where(x => x.Pid == nodePId).ToList();
            var otherNodes = wbsList.Where(x => x.Pid != nodePId).ToList();

            foreach (ProjectWBSDto node in mainNodes)
            {
                if (!node.Children.Any())
                    node.Children = await GetChildren(projectId, dateMonth, node.KeyId, otherNodes, mReportList, yReportList, klReportList, bData);
                else
                {
                    /***
                     * 最后一层节点处理月报详细数据
                     * 1.最后一层节点重新赋值项目月报详细数据
                     */

                    foreach (ProjectWBSDto finallyNode in node.Children)
                    {
                        finallyNode.ReportDetails = GetFinallyChildren(dateMonth, finallyNode.ProjectWBSId, node.Children, mReportList, yReportList, klReportList, bData).ToList();
                    }
                }

                if (!node.Children.Any())
                    //当前节点是最终子节点
                    node.ReportDetails = MReportForProjectList(dateMonth, node.ProjectWBSId, mReportList, yReportList, klReportList, bData).ToList();
            }

            return mainNodes;
        }
        /// <summary>
        /// 获取最终子节点（资源/船舶数据）
        /// </summary>
        /// <param name="childrens"></param>
        /// <param name="mReportList"></param>
        /// <param name="yReportList"></param>
        /// <param name="klReportList"></param>
        /// <param name="bData"></param>
        /// <param name="wbsId"></param>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        private List<ProjectWBSDto> GetFinallyChildren(int? dateMonth, Guid wbsId, List<ProjectWBSDto> childrens, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData)
        {
            List<ProjectWBSDto> childs = new List<ProjectWBSDto>();

            var children = childrens.FirstOrDefault(x => x.ProjectWBSId == wbsId);

            if (children != null)
            {
                if (children.Children == null || !children.Children.Any())
                {
                    /***
                     * 当前节点是最终子节点
                     */

                    childs = MReportForProjectList(dateMonth, children.ProjectWBSId, mReportList, yReportList, klReportList, bData);

                    /***
                     * 1.统计当前父节点当年（产值、工程量、外包支出）&& 累计（产值、工程量、外包支出）
                     * 2.重调方法获取最终子节点
                     */

                    children.ContractAmount = children.Children == null ? 0M : children.Children.Sum(x => x.ContractAmount);
                    children.EngQuantity = children.Children == null ? 0M : children.Children.Sum(x => x.EngQuantity);

                    int year = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
                    int initDate = new DateTime(year, 1, 1).ToDateMonth();
                }
                else
                {
                    GetFinallyChildren(dateMonth, children.ProjectWBSId, children.Children, mReportList, yReportList, klReportList, bData);
                }

            }

            return childs;
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
        private List<ProjectWBSDto> MReportForProjectList(int? dateMonth, Guid wbsId, List<ProjectWBSDto> mReportList, List<ProjectWBSDto> yReportList, List<ProjectWBSDto> klReportList, List<MonthReportForProjectBaseDataResponseDto> bData)
        {
            /***
             * 1.根据(当前施工分类)wbsid获取所有资源（船舶）信息
             * 2.统计资源（每条船）年度、开累值
             */

            int year = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
            int initDate = new DateTime(year, 1, 1).ToDateMonth();

            var mReport = mReportList.Where(x => x.ProjectWBSId == wbsId).ToList();
            foreach (var report in mReport)
            {
                //年度统计
                report.YearCompleteProductionAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.CompleteProductionAmount);
                report.YearCompletedQuantity = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.CompletedQuantity);
                report.YearOutsourcingExpensesAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.OutsourcingExpensesAmount);

                //开累统计
                report.TotalCompleteProductionAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.CompleteProductionAmount);
                report.TotalCompletedQuantity = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.CompletedQuantity);
                report.TotalOutsourcingExpensesAmount = klReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId && x.UnitPrice == report.UnitPrice).Sum(x => x.OutsourcingExpensesAmount);

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
        /// <returns></returns>
        private async Task<List<ProjectWBSDto>> GetWBSDataAsync(Guid pId, int? dateMonth)
        {
            var pWBS = new List<ProjectWBSDto>();
            var calculatePWBS = new List<ProjectWBSDto>();

            //空的项目id  不返回数据; 一个项目对应一个wbs
            if (pId == Guid.Empty) return pWBS;

            //获取本身的项目wbs数据
            pWBS = await HandleWBSDataAsync(pId, dateMonth);

            //获取需要计算的月报填报数据 
            if (dateMonth != 0 && dateMonth.ToString().Length == 6)
            {
                //获取当月前需要计算的的所有填报数据(累计的所有数据/开累)
                calculatePWBS = await _dbContext.Queryable<MonthReportDetail>()
                   .Where(p => !string.IsNullOrEmpty(p.ProjectId.ToString()) && p.ProjectId != Guid.Empty && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId && p.DateMonth <= dateMonth)
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
                       CompleteProductionAmount = SqlFunc.ToDecimal(p.UnitPrice) * SqlFunc.ToDecimal(p.CompletedQuantity)
                   })
                   .ToListAsync();

                foreach (var item in calculatePWBS)
                {
                    item.EngQuantity = pWBS.FirstOrDefault(x => x.ProjectWBSId == item.ProjectWBSId)?.EngQuantity;
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
                        DetailId = nowMonth.DetailId,
                        TotalCompletedQuantity = nowMonth.TotalCompletedQuantity,
                        TotalCompleteProductionAmount = nowMonth.TotalCompleteProductionAmount,
                        TotalOutsourcingExpensesAmount = nowMonth.TotalOutsourcingExpensesAmount,
                        YearCompletedQuantity = nowMonth.YearCompletedQuantity,
                        YearCompleteProductionAmount = nowMonth.YearCompleteProductionAmount,
                        YearOutsourcingExpensesAmount = nowMonth.YearOutsourcingExpensesAmount
                    });
                }

                //WBS树追加月报明细树 追加当月的月报详细数据
                //if (nowMonthReport != null && nowMonthReport.Any()) pWBS.AddRange(nowMonthReport);

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
                        CompletedQuantity = kvp.CompletedQuantity,// 0M,
                        UnitPrice = kvp.UnitPrice,//0M,
                        OutsourcingExpensesAmount = kvp.OutsourcingExpensesAmount,// 0M,
                        ValueType = ValueEnumType.NowMonth,
                        CompleteProductionAmount = kvp.CompleteProductionAmount,// 0M,
                        Remark = kvp.Remark,//string.Empty,
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
                        DetailId = kvp.DetailId,
                        TotalCompletedQuantity = kvp.TotalCompletedQuantity,
                        TotalCompleteProductionAmount = kvp.TotalCompleteProductionAmount,
                        TotalOutsourcingExpensesAmount = kvp.TotalOutsourcingExpensesAmount,
                        YearCompletedQuantity = kvp.YearCompletedQuantity,
                        YearCompleteProductionAmount = kvp.YearCompleteProductionAmount,
                        YearOutsourcingExpensesAmount = kvp.YearOutsourcingExpensesAmount
                    });
                }

                //处理当月&历史数据
                var handleList = new List<ProjectWBSDto>();
                var endHandleList = new List<ProjectWBSDto>();
                handleList.AddRange(nowMonthReport);
                handleList.AddRange(historyMonthReport);

                var gList = handleList.GroupBy(x => new { x.ProjectId, x.ShipId, x.UnitPrice, x.ProjectWBSId }).ToList();
                foreach (var item in gList)
                {
                    var model = handleList.Where(t => t.ProjectId == item.Key.ProjectId && t.ShipId == item.Key.ShipId && t.UnitPrice == item.Key.UnitPrice && t.ProjectWBSId == item.Key.ProjectWBSId).FirstOrDefault();
                    if (model != null) endHandleList.Add(model);
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
                        OutPutType = nowYear.OutPutType,
                        OutsourcingExpensesAmount = nowYear.OutsourcingExpensesAmount,
                        Pid = nowYear.Pid,
                        ProjectId = nowYear.ProjectId,
                        ProjectWBSId = nowYear.ProjectWBSId,
                        Remark = nowYear.Remark,
                        ShipId = nowYear.ShipId,
                        TotalCompletedQuantity = nowYear.TotalCompletedQuantity,
                        TotalCompleteProductionAmount = nowYear.TotalCompleteProductionAmount,
                        TotalOutsourcingExpensesAmount = nowYear.TotalOutsourcingExpensesAmount,
                        YearCompletedQuantity = nowYear.YearCompletedQuantity,
                        YearCompleteProductionAmount = nowYear.YearCompleteProductionAmount,
                        YearOutsourcingExpensesAmount = nowYear.YearOutsourcingExpensesAmount
                    });
                }

                //WBS树追加月报明细树 追加当年的月报详细数据
                if (nowYearMonthReport != null && nowYearMonthReport.Any()) pWBS.AddRange(nowYearMonthReport);

                /***
                 * 追加开累数据 calculatePWBS
                 */

                pWBS.AddRange(calculatePWBS);
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
                    ContractAmount = SqlFunc.ToDecimal(p.EngQuantity) * SqlFunc.ToDecimal(p.UnitPrice)//WBS的初始合同产值=工程量*单价
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
                                 .Where(p => !string.IsNullOrEmpty(p.ProjectId) && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId && !pIds.Contains(p.Id))
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
                                     UnitPrice = p.UnitPrice,//wbs初始的单价
                                     ContractAmount = SqlFunc.ToDecimal(p.EngQuantity) * SqlFunc.ToDecimal(p.UnitPrice)//WBS的初始合同产值=工程量*单价
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
                                     .Where(p => !string.IsNullOrEmpty(p.ProjectId) && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId && !pIds.Contains(p.Id))
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
                                         UnitPrice = p.UnitPrice,//wbs初始的单价
                                         ContractAmount = SqlFunc.ToDecimal(p.EngQuantity) * SqlFunc.ToDecimal(p.UnitPrice)//WBS的初始合同产值=工程量*单价
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
                                     .Where(p => !string.IsNullOrEmpty(p.ProjectId) && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId && !pIds.Contains(p.Id))
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
                                         UnitPrice = p.UnitPrice,//wbs初始的单价
                                         ContractAmount = SqlFunc.ToDecimal(p.EngQuantity) * SqlFunc.ToDecimal(p.UnitPrice)//WBS的初始合同产值=工程量*单价
                                     })
                                     .ToListAsync();

                        pWBSList.AddRange(nPWBSList);

                        return pWBSList;

                    }
                }
            }

            pWBSList = await _dbContext.Queryable<ProjectWBS>()
               .Where(p => !string.IsNullOrEmpty(p.ProjectId) && p.IsDelete == 1 && SqlFunc.ToGuid(p.ProjectId) == pId)
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
                   UnitPrice = p.UnitPrice,//wbs初始的单价
                   ContractAmount = SqlFunc.ToDecimal(p.EngQuantity) * SqlFunc.ToDecimal(p.UnitPrice)//WBS的初始合同产值=工程量*单价
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
            var currencyConverter = await _dbContext.Queryable<CurrencyConverter>().Where(t => t.CurrencyId == project.CurrencyId.ToString() && t.Year == DateTime.Now.Year && t.IsDelete == 1).SingleAsync();
            if (currencyConverter == null || currencyConverter.ExchangeRate == null) { responseAjaxResult.FailResult(HttpStatusCode.ParameterError, "汇率数据不存在"); return responseAjaxResult; }
            result.CurrencyExchangeRate = (decimal)currencyConverter.ExchangeRate;
            #endregion

            #region 取项目月报字段信息

            //截止到当前月份的所有月报信息（目的：统计年累、开累）
            var monthReportData = await _dbContext.Queryable<MonthReport>()
                .Where(x => x.IsDelete == 1 && x.DateMonth <= dateMonth && x.ProjectId == model.ProjectId)
                .ToListAsync();

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
            int year = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
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
            result.CurrentYearOffirmProductionValue = yMonthReports.Sum(x => x.PartyAConfirmedProductionAmount);
            result.TotalYearKaileaOffirmProductionValue = monthReportData.Sum(x => x.PartyAConfirmedProductionAmount) + historys.Item2;

            //本月&年&开累甲方付款金额(元)
            result.PartyAPayAmount = monthReport == null ? 0M : monthReport.PartyAPayAmount;
            result.CurrenYearCollection = yMonthReports.Sum(x => x.PartyAPayAmount);
            result.TotalYearCollection = monthReportData.Sum(x => x.PartyAPayAmount) + historys.Item4;

            if (monthReport != null)
            {
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

            // 判断业务来源暂存
            if (result.IsCanSubmit && dateMonth == nowDateMonth)
            {
                if (stagingData == null || (!stagingData.IsEffectStaging) || stagingData.BizData == null) { }
                else { result.IsFromStaging = true; result.StatusText = "暂存中"; }
                result.IsCanStaging = true;
            }

            result.DateMonth = dateMonth;
            #endregion

            //取树基础数据
            var bData = await GetBaseDataAsync();

            //取树
            var treeDetails = await WBSConvertTree(model.ProjectId, dateMonth, bData);

            //树组合
            result.TreeDetails = treeDetails;

            //数据回显
            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
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
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateYear <= currentYear).ToListAsync();

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
