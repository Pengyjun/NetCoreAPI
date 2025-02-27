using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{


    /// <summary>
    /// 交建通发消息（图文消息） 监控运营生产日报
    /// </summary>
    public class JjtSendMessageMonitoringDayReportResponseDto
    {
        /// <summary>
        /// 当前年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 当前月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 数据质量程度 几颗星
        /// </summary>
        public int QualityLevel { get; set; }
        /// <summary>
        /// 通报日期
        /// </summary>
        public string DayTime { get; set; }
        /// <summary>
        /// 项目总体生产情况
        /// </summary>
        public ProjectBasePoduction projectBasePoduction { get; set; }
        /// <summary>
        /// 自有船施工运转情况
        /// </summary>
        public OwnerShipBuildInfo OwnerShipBuildInfo { get; set; }
        /// <summary>
        /// 特殊情况
        /// </summary>
        public List<SpecialProjectInfo> SpecialProjectInfo { get; set; }
        /// <summary>
        /// 各单位填报情况
        /// </summary>
        public List<CompanyWriteReportInfo> CompanyWriteReportInfos { get; set; }
        /// <summary>
        /// 项目生产数据存在不完整部分主要是以下项目未填报
        /// </summary>
        public List<CompanyUnWriteReportInfo> CompanyUnWriteReportInfos { get; set; }
        /// <summary>
        /// 船舶生产数据存在不完整部分主要是项目部未填报以下船舶
        /// </summary>
        public List<CompanyShipUnWriteReportInfo> CompanyShipUnWriteReportInfos { get; set; }
        public List<EachCompanyProductionValue> EachCompanyProductionValue { get; set; }
        public List<ImpProjectWarning>? ImpProjectWarning { get; set; }

        public ProjectWokrItem? ProjectWokrItems { get; set; }

        /// <summary>
        /// 用于判断当前返回值是否是用于手机端日报图片数据返回
        /// </summary>
        public bool IsPhone { get; set; }

    }

    #region 项目总体生产情况
    /// <summary>
    /// 项目总体生产情况
    /// </summary>
    public class ProjectBasePoduction
    {
        //排名前十的 当日产值合计
        // public decimal TotalTopTenProductionValue { get; set; } 
        /// <summary>
        /// 总日产值
        /// </summary>
        public decimal YearTopFiveTotalOutput { get; set; }

        /// <summary>
        /// 总年产值
        /// </summary>
        public decimal YearFiveTotalOutput { get; set; }
        /// <summary>
        /// 占自有船舶总产值的百分比
        /// </summary>
        public decimal YearFiveTimeRate { get; set; }
        /// <summary>
        /// 年日报运转时间百分比
        /// </summary>
        public decimal YearTotalTopFiveOutputPercent { get; set; }
        public decimal TotalCurrentYearPlanProductionValue { get; set; }
        public decimal TotalCurrentYearCompleteProductionValue { get; set; }
        public decimal TotalCompleteRate { get; set; }
        public decimal SumCompleteRate { get; set; }
        /// <summary>
        /// 收入保障指数 几颗星  广航局当日总产值除以3000万
        /// </summary>
        public int IncomeSecurityLevel { get; set; }

        /// <summary>
        ///广航局合同项目数量
        /// </summary>
        public int TotalOnContractProjectCount { get; set; }
        /// <summary>
        /// 广航局在建项目数量
        /// </summary>
        public int TotalOnBuildProjectCount { get; set; }
        /// <summary>
        /// 广航局停缓建数量
        /// </summary>
        public int TotalStopBuildProjectCount { get; set; }
        /// <summary>
        /// 广航局在建数量占比
        /// </summary>
        public decimal TotalBuildCountPercent { get; set; }

        /// <summary>
        /// 当日产值
        /// </summary>
        public decimal DayProductionValue { get; set; }
        /// <summary>
        /// 年累计总产值
        /// </summary>
        public decimal TotalYearProductionValue { get; set; }
        /// <summary>
        /// 较产值进度 滞后
        /// </summary>
        public decimal ProductionValueProgressPercent { get; set; }
        /// <summary>
        /// 超序时进度
        /// </summary>
        public decimal SupersequenceProgress { get; set; }

        /// <summary>
        /// 总设备数量
        /// </summary>
        public int TotalFacilityCount { get; set; }

        /// <summary>
        /// 总工人数量
        /// </summary>
        public int TotalWorkerCount { get; set; }
        /// <summary>
        /// 总危大施工项数量
        /// </summary>
        public int TotalRiskWorkCount { get; set; }

        /// <summary>
        /// 产值前10条汇总
        /// </summary>
        public decimal SumProjectRanksTen { get; set; }
        /// <summary>
        /// 产值总汇总
        /// </summary>
        public decimal SumProjectRanks { get; set; }
        /// <summary>
        /// 总产值占比
        /// </summary>
        public decimal TotalProportion { get; set; }
        /// <summary>
        /// 各个公司基本项目情况
        /// </summary>
        public List<CompanyProjectBasePoduction> CompanyProjectBasePoductions { get; set; }
        /// <summary>
        /// 各个公司基本产值情况
        /// </summary>
        public List<CompanyBasePoductionValue> CompanyBasePoductionValues { get; set; }
        /// <summary>
        /// 柱形图
        /// </summary>
        public CompanyProductionCompare CompanyProductionCompares { get; set; }

        /// <summary>
        /// 项目完成产值排名
        /// </summary>
        public List<ProjectRank> ProjectRanks { get; set; }

        /// <summary>
        /// 项目产值强度表格
        /// </summary>
        public List<ProjectIntensity> ProjectIntensities { get; set; }
    }

    /// <summary>
    /// 各个公司基本项目情况
    /// </summary>
    public class CompanyProjectBasePoduction
    {
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
        /// <summary>
        ///  公司当日完成产值 单位元
        /// </summary>
        public decimal? CompanyDayProductionValue { get; set; }

        /// <summary>
        /// 公司当年累计产值 单位元
        /// </summary>
        public decimal? YearCompanyProductionValue { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
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
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        ///当日产值
        /// </summary>
        public decimal? DayActualValue { get; set; }
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
    /// 自有船施工运转情况
    /// </summary>
    public class OwnerShipBuildInfo
    {

        /// <summary>
        /// 广航局三类船舶总数量
        /// </summary>
        public int TotalCount { get; set; }
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

        /// <summary>
        /// 自由船舶施工产值
        /// </summary>
        public decimal BulidProductionValue { get; set; }
        /// <summary>
        /// 当日运转小时
        /// </summary>
        public decimal DayTurnHours { get; set; }
        /// <summary>
        /// 当年累计产值
        /// </summary>
        public decimal YearTotalProductionValue { get; set; }
        /// <summary>
        /// 当年累计运转小时
        /// </summary>
        public decimal YearTotalTurnHours { get; set; }
        /// <summary>
        /// 各个公司自有船施工运转情况集合
        /// 
        /// </summary>
        public List<CompanyShipBuildInfo> companyShipBuildInfos { get; set; }
        /// <summary>
        /// 各个公司自有船施工产值情况集合
        /// </summary>
        public List<CompanyShipProductionValueInfo> companyShipProductionValueInfos { get; set; }

        public List<ShipProductionValue> companyShipTopFiveInfoList { get; set; }
    }

    /// <summary>
    /// 各个公司自有船施工运转情况
    /// </summary>
    public class CompanyShipBuildInfo
    {
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

        /// <summary>
        /// 在场天数
        /// </summary>
        public decimal OnDays { get; set; }
    }
    #endregion

    #region 特殊情况
    /// <summary>
    /// 特殊情况
    /// </summary>
    public class SpecialProjectInfo
    {
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

        public int? Sort { get; set; }
    }
    #endregion

    #region 船舶生产数据存在不完整部分主要是项目部未填报以下船舶
    /// <summary>
    /// 船舶生产数据存在不完整部分主要是项目部未填报以下船舶
    /// </summary>
    public class CompanyShipUnWriteReportInfo
    {
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

    #region  施工船舶产值强度低于80%
    public class ShipOutputIntensity
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string ShipName { get; set; }
        /// <summary>
        /// 日均生产指标(万元)
        /// </summary>
        public decimal? DailyProductionIndex { get; set; }
        /// <summary>
        /// 当日船舶产值(万元)
        /// </summary>
        public decimal? DayShipOutPut { get; set; }
        /// <summary>
        /// 指标完成率(升序)
        /// </summary>
        public decimal? IndicatorRate { get; set; }
        /// <summary>
        /// 当日船舶生产不理想原因
        /// </summary>
        public string? ShipRemark { get; set; }
    }
    #endregion


    #region 更新船舶在场天数DTO
    /// <summary>
    /// 更新船舶在场天数DTO
    /// </summary>
    public class ShipOnDay
    {
        public Guid Id { get; set; }
        public decimal OnDayCount { get; set; }

    }
    #endregion

    /// <summary>
    /// 船舶产值
    /// </summary>
    public class ShipProductionValue
    {

        public Guid? ShipId { get; set; }
        public string ShipName { get; set; }

        public decimal? ShipDayOutput { get; set; }
        public decimal? ShipYearOutput { get; set; }
        public decimal? TimePercent { get; set; }

        public int Status { get; set; }

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



    #region 各单位完成产值计划产值对比
    public class EachCompanyProductionValue
    {
        /// <summary>
        /// 公司名称   为空不按公司   
        /// </summary>
        public string? ConpanyName { get; set; }
        /// <summary>
        /// X轴
        /// </summary>
        public string? XAxle { get; set; }
        /// <summary>
        /// 计划产值
        /// </summary>
        public decimal? YAxlePlanValue { get; set; }
        /// <summary>
        /// 完成产值
        /// </summary>
        public decimal? YAxleCompleteValue { get; set; }
    }
    #endregion 按天统计重点项目预警值
    public class ImpProjectWarning
    {
        /// <summary>
        /// 当日完成产值
        /// </summary>
        public decimal? DayAmount { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 是否偏低  1是偏高  0是偏低
        /// </summary>
        public int? IsLow { get; set; }
        /// <summary>
        /// 偏差内容
        /// </summary>
        public string? DeviationWarning { get; set; }
    }



    #region 节后停工  来了未开工的项目和今日已开工的项目 临时用
    /// <summary>
    /// 节后停工  来了未开工的项目和今日已开工的项目
    /// </summary>
    public class ProjectWokrItem
    {

        public List<DayWorkProject> DayWorkProject { get; set; } = new List<DayWorkProject>();
        public List<DayWorkProject> NoWorkProject { get; set; } = new List<DayWorkProject>();

    }

    public class DayWorkProject
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }

        public decimal DayAmount { get; set; }
        public string DeviationWarning { get; set; }
        public bool IsShow { get; set; } = true;
    }
    #endregion
}
