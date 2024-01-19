
namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 交建通发送消息 详情dto
    /// </summary>
    public class JjtSendMsgDetailsResponse
    {
        /// <summary>
        /// xxx月xxx日广航局生产运营监控日报
        /// </summary>
        public string? Date { get; set; } = DateTime.Now.AddDays(-1).ToString("MM月dd日");
        /// <summary>
        /// 项目总数
        /// </summary>
        public int PTotalNums { get; set; }
        /// <summary>
        /// 项目在建数
        /// </summary>
        public int PInBuildNums { get; set; }
        /// <summary>
        /// 项目停工数
        /// </summary>
        public int PStopNums { get; set; }
        /// <summary>
        /// 停工数占比
        /// </summary>
        public decimal? PStopProp { get; set; }
        /// <summary>
        /// 在建项目产值
        /// </summary>
        public decimal? PInBuildOutputVal { get; set; }
        /// <summary>
        /// 年累施工产值  亿元
        /// </summary>
        public decimal? YearProjectAccumulateOutputVal { get; set; }
        /// <summary>
        /// 超前或滞后
        /// </summary>
        public decimal? LeadOrLag { get; set; }
        /// <summary>
        /// 三类施工船总数
        /// </summary>
        public int WorkShipTotalNums { get; set; }
        /// <summary>
        /// 施工数
        /// </summary>
        public int WorkShipNums { get; set; }
        /// <summary>
        /// 检修数
        /// </summary>
        public int OverHaulNums { get; set; }
        /// <summary>
        /// 调遣数
        /// </summary>
        public int DispatchNums { get; set; }
        /// <summary>
        /// 待命数
        /// </summary>
        public int StandbyNums { get; set; }
        /// <summary>
        /// 开工率
        /// </summary>
        public decimal? WorkRate { get; set; }
        /// <summary>
        /// 日自有船舶施工产值  万元
        /// </summary>
        public decimal? DayWorkOutputVal { get; set; }
        /// <summary>
        /// 运转时间
        /// </summary>
        public decimal WorkHours { get; set; }
        /// <summary>
        /// 年累船舶产值 亿元
        /// </summary>
        public decimal? YearShipAccumulateOutputVal { get; set; }
        /// <summary>
        /// 年累运转时间
        /// </summary>
        public decimal? YearAccumulateWorkHours { get; set; }
        /// <summary>
        /// 特殊情况数量
        /// </summary>
        public int SpecialNums { get; set; }
        /// <summary>
        /// 评分 1：一颗星[0-30) 2:两颗星[30-60) 3:三颗星[60-80) 4:四颗星[80-90) 5:五颗星[90-100)
        /// 计算公式：（项目当日产值/3300*50%+船舶当日产值/490*25%+项目填报率*20%+船舶填报率*5%）*100
        /// </summary>
        public int Score { get; set; }
        public List<TableProjectOverallProdNumsDto> TableProjectOverallProdNumsDto { get; set; }
        public List<TableProjectOverallProdOpValDto> TableProjectOverallProdOpValDto { get; set; }
        public List<TableOwnShipWorkNumsDto> TableOwnShipWorkNumsDto { get; set; }
        public List<TableOwnShipWorkOpValDto> TableOwnShipWorkOpValDto { get; set; }
        public List<TableSpecialDto> TableSpecialDto { get; set; }
        public List<TableNotFillDto> TableNotFillDto { get; set; }
        public List<TableUnitFillRepDto> TableUnitFillRepDto { get; set; }
        public List<TableShipNotFillRepDto> TableShipNotFillRepDto { get; set; }

    }
    /// <summary>
    /// 项目总体生产情况项目个数表格
    /// </summary>
    public class TableProjectOverallProdNumsDto
    {
        /// <summary>
        /// 业务单位
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// 合同项目数
        /// </summary>
        public int PNums { get; set; }
        /// <summary>
        /// 在建项目数
        /// </summary>
        public int PInBuildNums { get; set; }
        /// <summary>
        /// 暂停项目数
        /// </summary>
        public int PStopNums { get; set; }
        /// <summary>
        /// 单位占比
        /// </summary>
        public decimal? UnitProp { get; set; }
    }
    ///<summary>
    /// 项目总体生产情况项目产值表格
    /// </summary>
    public class TableProjectOverallProdOpValDto
    {
        /// <summary>
        /// 业务单位
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// 当日产值 万元
        /// </summary>
        public decimal? DayOutputVal { get; set; }
        /// <summary>
        /// 年累产值
        /// </summary>
        public decimal? YearProjectAccumulateOutputVal { get; set; }
        /// <summary>
        /// 年度产值占比 占广航局
        /// </summary>
        public decimal? YearAccumulateOutputValProp { get; set; }
        /// <summary>
        /// 较年度计划比例
        /// </summary>
        public decimal? ComparedToAnnualPlanRate { get; set; }

    }
    /// <summary>
    /// 自有船舶施工运转数量情况
    /// </summary>
    public class TableOwnShipWorkNumsDto
    {
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 总艘数
        /// </summary>
        public int ShipTotalNums { get; set; }
        /// <summary>
        /// 施工数
        /// </summary>
        public int WorkNums { get; set; }
        /// <summary>
        /// 修理数
        /// </summary>
        public int OverHaulNums { get; set; }
        /// <summary>
        /// 调遣数
        /// </summary>
        public int DispatchNums { get; set; }
        /// <summary>
        /// 待命数
        /// </summary>
        public int StandbyNums { get; set; }
        /// <summary>
        /// 开工率
        /// </summary>
        public decimal? WorkRate { get; set; }
    }
    /// <summary>
    /// 自有船舶施工运转产值情况
    /// </summary>
    public class TableOwnShipWorkOpValDto
    {
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 当日产值 万元
        /// </summary>
        public decimal? DayWorkOutputVal { get; set; }
        /// <summary>
        /// 当日运转时间
        /// </summary>
        public decimal WorkHours { get; set; }
        /// <summary>
        /// 年累产值
        /// </summary>
        public decimal? YearAccumulateOutputVal { get; set; }
        /// <summary>
        /// 当年运转时间
        /// </summary>
        public decimal YearWorkHours { get; set; }
        /// <summary>
        /// 时间利用率
        /// </summary>
        public decimal? HoursRate { get; set; }
    }
    /// <summary>
    /// 特殊情况表格
    /// </summary>
    public class TableSpecialDto
    {
        /// <summary>
        /// 事项分类
        /// </summary>
        public string? WorkType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Describe { get; set; }
    }
    /// <summary>
    /// 未填报
    /// </summary>
    public class TableNotFillDto
    {
        /// <summary>
        /// 未报项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 未填报次数
        /// </summary>
        public int NotFillCount { get; set; }
    }
    /// <summary>
    /// 各单位产值日报填报率情况
    /// </summary>
    public class TableUnitFillRepDto
    {
        /// <summary>
        /// 业务单位
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// 在建数量
        /// </summary>
        public int PInBuildNums { get; set; }
        /// <summary>
        /// 未填项目数量  在建-已填
        /// </summary>
        public int NotFillNums { get; set; }
        /// <summary>
        /// 填报率
        /// </summary>
        public decimal FillProp { get; set; }
    }
    /// <summary>
    /// 项目未报船舶
    /// </summary>
    public class TableShipNotFillRepDto
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string? ProjectName { get; set; }
    }
}
