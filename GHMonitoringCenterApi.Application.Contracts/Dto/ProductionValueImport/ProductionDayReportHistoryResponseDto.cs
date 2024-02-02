using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Models;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport
{

    /// <summary>
    /// 导入生产日报推送历史数据
    /// </summary>
    public class ProductionDayReportHistoryResponseDto
    {

        public List<CompanyProjectBasePoduction> CompanyProjectBasePoduction { get; set; }
        public List<CompanyBasePoductionValue> CompanyBasePoductionValue { get; set; }
        public List<CompanyShipBuildInfo> CompanyShipBuildInfo { get; set; }
        public List<CompanyShipProductionValueInfo> CompanyShipProductionValueInfo { get; set; }
        public List<SpecialProjectInfo> SpecialProjectInfo { get; set; }
        public List<CompanyWriteReportInfo> CompanyWriteReportInfo { get; set; }
        public List<CompanyUnWriteReportInfo> CompanyUnWriteReportInfo { get; set; }
        public List<CompanyShipUnWriteReportInfo> CompanyShipUnWriteReportInfo { get; set; }
        public List<ShipProductionValue> ShipProductionValue { get; set; }
        public List<ProjectShiftProductionInfo> ProjectShiftProductionInfo { get; set; }
        public List<UnProjectShitInfo> UnProjectShitInfo { get; set; }
        public List<ExcelTitle> ExcelTitle { get; set; }


        //public  CompanyProjectBasePoduction CompanyProjectBasePoduction { get; set; }
        //public  CompanyBasePoductionValue CompanyBasePoductionValue { get; set; }
        //public  CompanyShipBuildInfo CompanyShipBuildInfo { get; set; }
        //public  CompanyShipProductionValueInfo CompanyShipProductionValueInfo { get; set; }
        //public  SpecialProjectInfo SpecialProjectInfo { get; set; }
        //public  CompanyWriteReportInfo CompanyWriteReportInfo { get; set; }
        //public  CompanyUnWriteReportInfo CompanyUnWriteReportInfo { get; set; }
        //public  CompanyShipUnWriteReportInfo CompanyShipUnWriteReportInfo { get; set; }
        //public  ShipProductionValue ShipProductionValue { get; set; }
        //public  ProjectShiftProductionInfo ProjectShiftProductionInfo { get; set; }
        //public  UnProjectShitInfo UnProjectShitInfo { get; set; }
        //public  ExcelTitle ExcelTitle { get; set; }
    }


    #region 项目总体生产情况

    /// <summary>
    /// 各个公司基本项目情况
    /// </summary>
    public class CompanyProjectBasePoduction
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///合同项目数量
        /// </summary>
        public int OnContractProjectCount { get; set; }
        /// <summary>
        /// 在建项目数量
        /// </summary>
        public int OnBuildProjectCount { get; set; }
        /// <summary>
        /// 停缓建数量
        /// </summary>
        public int StopBuildProjectCount { get; set; }
        /// <summary>
        /// 未开工数量
        /// </summary>
        public int NotWorkCount { get; set; }

        /// <summary>
        /// 在建数量占比
        /// </summary>
        public decimal BuildCountPercent { get; set; }
        /// <summary>
        /// 设备数量
        /// </summary>
        public int FacilityCount { get; set; }

        /// <summary>
        /// 工人数量
        /// </summary>
        public int WorkerCount { get; set; }
        /// <summary>
        /// 危大施工项数量
        /// </summary>
        public int RiskWorkCount { get; set; }

    }


    /// <summary>
    /// 各个公司基本产值情况
    /// </summary>
    public class CompanyBasePoductionValue
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当日产值
        /// </summary>
        public decimal DayProductionValue { get; set; }
        /// <summary>
        /// 年累计总产值
        /// </summary>
        public decimal TotalYearProductionValue { get; set; }
        /// <summary>
        /// 年度产值占比
        /// </summary>
        public decimal YearProductionValueProgressPercent { get; set; }
        /// <summary>
        /// 较产值进度
        /// </summary>
        public decimal ProductionValueProgressPercent { get; set; }

        /// <summary>
        /// 超序时进度
        /// 计算公式 1  年度指标完成率=累计完成/年度指标 （其中年度指标由广航局每年提供）
        ///          2  序时进度=本年已过多少天/365*100%
        ///          3  超序时进度=(1-2)*100%=(年度指标完成率-序时进度)*100%
        /// </summary>
        public decimal SupersequenceProgress { get; set; }

    }

    #region 各个公司产值比较折线图数据
    /// <summary>
    /// 各个公司产值比较折线图数据
    /// </summary>
    public class CompanyProductionCompare
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 计划产值最大值  
        /// </summary>
        public decimal YMax { get; set; }
        /// <summary>
        /// X轴的数据
        /// </summary>
        public List<string> XAxisData { get; set; }
        /// <summary>
        /// 计划产值
        /// </summary>
        public List<decimal> PlanProductuin { get; set; }
        /// <summary>
        /// 完成产值
        /// </summary>
        public List<decimal> CompleteProductuin { get; set; }
        /// <summary>
        /// 计划完成率  完成率除以计划完成率
        /// </summary>
        public List<decimal> PlanCompleteRate { get; set; }
        /// <summary>
        /// 时间进度   就是截止今天是本施工月第几天/30.5天
        /// </summary>
        public List<decimal> TimeSchedule { get; set; }

    }
    public class YService
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 计划产值
        /// </summary>
        public decimal PlanProdution { get; set; }
        /// <summary>
        /// 完成产值
        /// </summary>
        public decimal CompleteProdution { get; set; }

    }
    #endregion

    #region 项目产值完成排名
    /// <summary>
    /// 项目产值完成排名
    /// </summary>
    public class ProjectRank
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        ///当日产值
        /// </summary>
        public decimal DayActualValue { get; set; }
        /// <summary>
        /// 当年计划产值
        /// </summary>
        public decimal CurrentYearPlanProductionValue { get; set; }
        /// <summary>
        /// 当年完成产值
        /// </summary>
        public decimal CurrentYearCompleteProductionValue { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal CompleteRate { get; set; }




    }
    #endregion

    #region 项目产值强度表格
    /// <summary>
    /// 项目产值强度表格
    /// </summary>
    public class ProjectIntensity
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当月日均计划
        /// </summary>
        public decimal? PlanDayProduciton { get; set; }
        /// <summary>
        /// 当日实际产值
        /// </summary>
        public decimal? DayProduciton { get; set; }
        /// <summary>
        /// 当日产值完成率 
        /// </summary>
        public decimal? CompleteDayProducitonRate { get; set; }
        /// <summary>
        /// 当日产值强度较低原因
        /// </summary>
        public string? DayProductionIntensityDesc { get; set; }
    }
    #endregion

    #endregion

    #region 自有船施工运转情况
    /// <summary>
    /// 各个公司自有船施工运转情况
    /// </summary>
    public class CompanyShipBuildInfo
    {
        public int DateDay { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 每个公司船舶的数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 施工数量
        /// </summary>
        public int BuildCount { get; set; }
        /// <summary>
        /// 检修数量
        /// </summary>
        public int ReconditionCount { get; set; }
        /// <summary>
        /// 调遣数量
        /// </summary>
        public int AssignCount { get; set; }
        /// <summary>
        /// 待命数量
        /// </summary>
        public int AwaitCount { get; set; }

        /// <summary>
        /// 开工率
        /// </summary>
        public decimal BuildPercent { get; set; }
    }
    /// <summary>
    /// 各个公司自有船施工产值情况
    /// </summary>
    public class CompanyShipProductionValueInfo
    {
        public int DateDay { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 当日产值
        /// </summary>
        public decimal DayProductionValue { get; set; }
        /// <summary>
        /// 当日运转小时
        /// </summary>
        public decimal DayTurnHours { get; set; }
        /// <summary>
        /// 时间利用率
        /// </summary>
        public decimal TimePercent { get; set; }
        /// <summary>
        /// 当年累计产值
        /// </summary>
        public decimal YearTotalProductionValue { get; set; }
        /// <summary>
        /// 当年累计运转小时
        /// </summary>
        public decimal YearTotalTurnHours { get; set; }
    }
    #endregion

    #region 特殊情况
    /// <summary>
    /// 特殊情况
    /// </summary>
    public class SpecialProjectInfo
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string SourceMatter { get; set; }
        /// <summary>
        /// 类型  1异常预警 2 嘉奖通报
        /// </summary>
        public int Type { get; set; }
        public string TypeDesc { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
    #endregion

    #region 各单位填报情况
    /// <summary>
    /// 各单位填报情况
    /// </summary>
    public class CompanyWriteReportInfo
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ProjectId { get; set; }
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 在建项目数量
        /// </summary>
        public int OnBulidCount { get; set; }
        /// <summary>
        /// 未填报数量
        /// </summary>
        public int UnReportCount { get; set; }
        /// <summary>
        /// 填报率
        /// </summary>
        public decimal WritePercent { get; set; }
        /// <summary>
        /// 数据质量程度 几颗星
        /// </summary>
        public int QualityLevel { get; set; }

    }

    #endregion

    #region 项目生产数据存在不完整部分主要是以下项目未填报
    /// <summary>
    /// 项目生产数据存在不完整部分主要是以下项目未填报
    /// </summary>
    public class CompanyUnWriteReportInfo
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 未填报次数
        /// </summary>
        public int Count { get; set; }

        public string unCount { get; set; }
    }
    #endregion

    #region 船舶生产数据存在不完整部分主要是项目部未填报以下船舶
    /// <summary>
    /// 船舶生产数据存在不完整部分主要是项目部未填报以下船舶
    /// </summary>
    public class CompanyShipUnWriteReportInfo
    {
        public int DateDay { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string ShipName { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string OnProjectName { get; set; }
    }
    #endregion


    #region 自由船舶前五的
    /// <summary>
    /// 船舶产值
    /// </summary>
    public class ShipProductionValue
    {
        public int DateDay { get; set; }
        public Guid? ShipId { get; set; }
        public string ShipName { get; set; }

        public decimal? ShipDayOutput { get; set; }
        public decimal? ShipYearOutput { get; set; }
        public decimal? TimePercent { get; set; }

        /// <summary>
        /// 运转时间(小时)
        /// </summary>
        [JsonIgnore]
        public decimal WorkingHours { get; set; }

        /// <summary>
        /// 施工天数（天）
        /// </summary>
        [JsonIgnore]
        public decimal ConstructionDays { get; set; }

    }
    #endregion


    #region  施工船舶产值强度低于80%  暂时不需要
    //public class ShipOutputIntensity
    //{
    //    /// <summary>
    //    /// 船舶名称
    //    /// </summary>
    //    public string ShipName { get; set; }
    //    /// <summary>
    //    /// 日均生产指标(万元)
    //    /// </summary>
    //    public decimal? DailyProductionIndex { get; set; }
    //    /// <summary>
    //    /// 当日船舶产值(万元)
    //    /// </summary>
    //    public decimal? DayShipOutPut { get; set; }
    //    /// <summary>
    //    /// 指标完成率(升序)
    //    /// </summary>
    //    public decimal? IndicatorRate { get; set; }
    //    /// <summary>
    //    /// 当日船舶生产不理想原因
    //    /// </summary>
    //    public string? ShipRemark { get; set; }
    //}
    #endregion


}
