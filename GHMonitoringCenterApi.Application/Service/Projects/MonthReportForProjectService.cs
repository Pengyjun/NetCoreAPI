using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using SqlSugar;

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
        /// 服务注入
        /// </summary>
        public MonthReportForProjectService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
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
        /// <returns></returns>
        public async Task<List<ConveretTreeForProjectWBSDto>> WBSConvertTree(Guid projectId, int dateMonth)
        {
            /***
             * 1.获取请求数据
             * 2.获取当月月报数据
             * 3.获取年度月报详细
             * 4.获取开累月报详细
             * 5.获取WBS数据
             */
            var requestList = await GetWBSDataAsync(projectId, dateMonth);
            var mReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowMonth).ToList();
            var yReportList = requestList.Where(x => x.ValueType == ValueEnumType.NowYear).ToList();
            var hReportList = requestList.Where(x => x.ValueType == ValueEnumType.AccumulatedCommencement).ToList();
            var wbsList = requestList.Where(x => x.ValueType == ValueEnumType.None).ToList();

            ////映射体转换初始化
            var mapperWbsList = new List<ConveretTreeForProjectWBSDto>();
            var mappermReportList = new List<ConveretTreeForProjectWBSDto>();
            var mapperyReportList = new List<ConveretTreeForProjectWBSDto>();
            var mapperhReportList = new List<ConveretTreeForProjectWBSDto>();

            /***
             * 映射对应响应体
             * 1.映射wbs响应体
             * 2.映射月报详细响应体
            */
            _mapper.Map<List<ProjectWBSDto>, List<ConveretTreeForProjectWBSDto>>(wbsList, mapperWbsList);
            _mapper.Map<List<ProjectWBSDto>, List<ConveretTreeForProjectWBSDto>>(mReportList, mappermReportList);
            _mapper.Map<List<ProjectWBSDto>, List<ConveretTreeForProjectWBSDto>>(yReportList, mapperyReportList);
            _mapper.Map<List<ProjectWBSDto>, List<ConveretTreeForProjectWBSDto>>(hReportList, mapperhReportList);

            var tt = await GetChildren(projectId, dateMonth, "0", mapperWbsList, mappermReportList, mapperyReportList, mapperhReportList);
            //转换wbs树
            return tt;
        }

        /// <summary>
        /// 获取子集数据，处理月报详细数据逻辑
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateMonth"></param>
        /// <param name="nodePId"></param>
        /// <param name="mapperWbsList">wbs响应体</param>
        /// <param name="mappermReportList">当月月报详细响应体</param>
        /// <param name="mapperyReportList">当年月报详细响应体</param>
        /// <param name="mapperhReportList">开累（历史）月报详细响应体</param>
        /// <returns></returns>
        public async Task<List<ConveretTreeForProjectWBSDto>> GetChildren(Guid projectId, int dateMonth, string? nodePId, List<ConveretTreeForProjectWBSDto> mapperWbsList, List<ConveretTreeForProjectWBSDto> mappermReportList, List<ConveretTreeForProjectWBSDto> mapperyReportList, List<ConveretTreeForProjectWBSDto> mapperhReportList)
        {
            var mainNodes = mapperWbsList.Where(x => x.Pid == nodePId).ToList();
            var otherNodes = mapperWbsList.Where(x => x.Pid != nodePId).ToList();
            if (otherNodes != null && otherNodes.Any())
            {
                foreach (var node in mainNodes)
                {
                    node.Node = await GetChildren(projectId, dateMonth, node.KeyId, otherNodes, mappermReportList, mapperyReportList, mapperhReportList);

                    /***
                     * 最后一层节点处理月报详细数据
                     * 1.最后一层节点重新赋值项目月报详细数据
                     */
                    if (node.Node.Count == 0)
                    {
                        node.Node = MReportForProjectList(node.KeyId, node.ProjectWBSId, mappermReportList, mapperyReportList, mapperhReportList);
                    }
                }
            }

            return mainNodes;
        }
        /// <summary>
        /// 最后一层节点处理月报详细数据
        /// </summary>
        /// <param name="pId">上级的树id（匹配最后的根节点keyid）</param>
        /// <param name="wbsId">wbsid（施工分类）</param>
        /// <param name="mReportList">当月月报详细数据</param>
        /// <param name="yReportList">当年月报详细数据(做统计数据使用)</param>
        /// <param name="hReportList">开累月报详细数据(做统计数据使用)</param>
        /// <returns></returns>
        public List<ConveretTreeForProjectWBSDto> MReportForProjectList(string pId, Guid wbsId, List<ConveretTreeForProjectWBSDto> mReportList, List<ConveretTreeForProjectWBSDto> yReportList, List<ConveretTreeForProjectWBSDto> hReportList)
        {
            /***
             * 1.根据(当前施工分类)wbsid获取所有资源（船舶）信息
             * 2.统计资源（每条船）年度、开累值
             */
            var mReport = mReportList.Where(x => x.ProjectWBSId == wbsId).ToList();
            foreach (var report in mReport)
            {
                //年度统计
                report.YearCompleteProductionAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId).Sum(x => x.CompleteProductionAmount);
                report.YearCompletedQuantity = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId).Sum(x => x.CompletedQuantity);
                report.YearOutsourcingExpensesAmount = yReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId).Sum(x => x.OutsourcingExpensesAmount);

                //开累统计
                report.TotalCompleteProductionAmount = hReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId).Sum(x => x.CompleteProductionAmount);
                report.TotalCompletedQuantity = hReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId).Sum(x => x.CompletedQuantity);
                report.TotalOutsourcingExpensesAmount = hReportList.Where(x => x.ProjectId == report.ProjectId && report.ShipId == x.ShipId && x.ProjectWBSId == wbsId).Sum(x => x.OutsourcingExpensesAmount);
            }

            return mReport;
        }
        /// <summary>
        /// 查询基础/最新wbs数据
        /// </summary>
        /// <param name="pId">项目id</param>
        /// <param name="dateMonth">填报日期</param>
        /// <returns></returns>
        public async Task<List<ProjectWBSDto>> GetWBSDataAsync(Guid pId, int dateMonth)
        {
            var pWBS = new List<ProjectWBSDto>();
            var calculatePWBS = new List<ProjectWBSDto>();

            //空的项目id  不返回数据; 一个项目对应一个wbs
            if (pId == Guid.Empty) return pWBS;

            //获取本身的项目wbs数据
            pWBS = await _dbContext.Queryable<ProjectWBS>()
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
                    UnitPrice = p.UnitPrice,//wbs初始的单价
                    ContractAmount = SqlFunc.ToDecimal(p.EngQuantity) * SqlFunc.ToDecimal(p.UnitPrice)//WBS的初始合同产值=工程量*单价
                })
                .ToListAsync();

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
                       Remark = p.Remark
                   })
                   .ToListAsync();

                /***
                 * 1.取当月的填报数据，当月完成产值处理(单价*工程量),其他不变
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
                        CompleteProductionAmount = Convert.ToDecimal(nowMonth.UnitPrice) * Convert.ToDecimal(nowMonth.CompletedQuantity),
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
                        TotalCompletedQuantity = nowMonth.TotalCompletedQuantity,
                        TotalCompleteProductionAmount = nowMonth.TotalCompleteProductionAmount,
                        TotalOutsourcingExpensesAmount = nowMonth.TotalOutsourcingExpensesAmount,
                        YearCompletedQuantity = nowMonth.YearCompletedQuantity,
                        YearCompleteProductionAmount = nowMonth.YearCompleteProductionAmount,
                        YearOutsourcingExpensesAmount = nowMonth.YearOutsourcingExpensesAmount
                    });
                }
                //WBS树追加月报明细树 追加当月的月报详细数据
                if (nowMonthReport != null && nowMonthReport.Any()) pWBS.AddRange(nowMonthReport);

                /***
                 * 当月填报详细中需要包含历史中存在的资源（船舶）信息，所以在此处valuetype=当月,其他基础信息不变作为本月数据
                 * 1.取除当月之前的所有填报数据
                 * 2.根据资源（船舶id）去重
                 * 3.需要填写的单价、工程量、外包支出、备注全部初始化
                 */
                var historyMonthReport = new List<ProjectWBSDto>();
                foreach (var kvp in calculatePWBS.Where(x => x.DateMonth < dateMonth).GroupBy(i => i.ShipId, (key, val) => val.First()))
                {
                    historyMonthReport.Add(new ProjectWBSDto
                    {
                        CompletedQuantity = 0M,
                        UnitPrice = 0M,
                        OutsourcingExpensesAmount = 0M,
                        ValueType = ValueEnumType.NowMonth,
                        CompleteProductionAmount = kvp.CompleteProductionAmount,
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
                        Remark = kvp.Remark,
                        ShipId = kvp.ShipId,
                        TotalCompletedQuantity = kvp.TotalCompletedQuantity,
                        TotalCompleteProductionAmount = kvp.TotalCompleteProductionAmount,
                        TotalOutsourcingExpensesAmount = kvp.TotalOutsourcingExpensesAmount,
                        YearCompletedQuantity = kvp.YearCompletedQuantity,
                        YearCompleteProductionAmount = kvp.YearCompleteProductionAmount,
                        YearOutsourcingExpensesAmount = kvp.YearOutsourcingExpensesAmount
                    });
                }
                //WBS树追加月报明细树 追加历史的月报详细数据
                if (historyMonthReport != null && historyMonthReport.Any()) pWBS.AddRange(historyMonthReport);

                /***
                 * 当年数据，值不做处理 类型字段区分字段valuetype改为当年
                 * 1.截取年份字段
                 */
                int year = Convert.ToInt32(dateMonth.ToString().Substring(0, 4));
                var nowYearMonthReport = new List<ProjectWBSDto>();
                foreach (var nowYear in calculatePWBS.Where(x => x.DateYear == year).ToList())
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
        #endregion
    }
}
