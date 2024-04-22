using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 船舶日报（包含：有项目-船舶日报，无项目-船舶日报[动态船舶日报]）
    /// </summary>
    [SugarTable("t_shipdayreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_SDR_UQ_KEY",  nameof(ProjectId), OrderByType.Asc,  nameof(ShipId),  OrderByType.Asc,  nameof(DateDay), OrderByType.Asc ,  true)]
    public class ShipDayReport : BaseEntity<Guid>
    {

        /// <summary>
        /// 项目id(备注：动态-船舶日报，值为Guid.Empty )
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶日报类型(1:项目-船舶日报,2:动态-船舶日报)
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public ShipDayReportType ShipDayReportType { get; set; }

        /// <summary>
        /// 所在港口
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PortId { get; set; }

        /// <summary>
        /// 施工区域
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ConstructionArea { get; set; }

        /// <summary>
        ///平均挖深（m）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? AverageExcavationDepth { get; set; }

        /// <summary>
        ///平均挖宽（m）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? AverageExcavationWidth { get; set; }

        /// <summary>
        ///船报产量(方)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ShipReportedProduction { get; set; }

        /// <summary>
        ///前进距/舱/驳数
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2")]
        public decimal ForwardNumber { get; set; }

        /// <summary>
        ///油耗
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2")]
        public decimal OilConsumption { get; set; }

        /// <summary>
        ///管线长度（m）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? PipelineLength { get; set; }

        /// <summary>
        ///浮管长度(m)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? FloatingTubeLength { get; set; }

        /// <summary>
        ///岸管长度（m）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? ShorePipeLength { get; set; }

        /// <summary>
        /// 沉管长度（m）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? ImmersedTubeLength { get; set; }

        /// <summary>
        /// 估算单价（元/m³）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal EstimatedUnitPrice { get; set; }

        /// <summary>
        /// 燃油单价（元/吨）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? FuelUnitPrice { get; set; }

        /// <summary>
        ///估算产值（元）(船报产量*估算单价) 注：产量和单价都是保留小数两位相乘，保证数据完整性保留四位小数
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal? EstimatedOutputAmount { get; set; }

        /// <summary>
        ///估算成本(元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? EstimatedCostAmount { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? Remarks { get; set; }

        /// <summary>
        /// 特殊事项报告（0：无,1:异常预警，2：嘉奖通报，3：提醒事项）
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int IsHaveProductionWarning { get; set; }

        /// <summary>
        /// 特殊事项信息
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ProductionWarningContent { get; set; }

        /// <summary>
        /// 施工模式
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ConstructionmMode { get; set; }

        /// <summary>
        ///吹矩(m)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? BlowTorque { get; set; }

        /// <summary>
        ///船存燃油（t）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ShipboardFuel { get; set; }

        /// <summary>
        ///燃油补给（t）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? FuelSupply { get; set; }

        #region 生产运转（单位小时）

        /// <summary>
        ///挖泥
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? Dredge { get; set; }

        /// <summary>
        ///航行
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? Sail { get; set; }

        /// <summary>
        ///吹水
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? BlowingWater { get; set; }

        /// <summary>
        ///抛泥
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? SedimentDisposal { get; set; }

        /// <summary>
        ///吹岸
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? BlowShore { get; set; }

        #endregion

        #region 生产停歇 （单位小时）

        /// <summary>
        ///开工展布
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ConstructionLayout { get; set; }

        /// <summary>
        ///备机
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? StandbyMachine { get; set; }

        /// <summary>
        ///下(移)锚览
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? DownAnchor { get; set; }

        /// <summary>
        ///移船
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? MovingShip { get; set; }

        /// <summary>
        ///焊绞刀/齿座
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? WeldingCutter { get; set; }

        /// <summary>
        ///换齿
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ChangeGear { get; set; }

        /// <summary>
        ///补给
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? Supply { get; set; }

        /// <summary>
        ///测量影响
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? MeasurementImpact { get; set; }

        /// <summary>
        ///敷设管线
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? LayingPipelines { get; set; }

        /// <summary>
        ///调整管线
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? AdjustingPipeline { get; set; }

        /// <summary>
        ///清泥泵
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? CleaningPump { get; set; }

        /// <summary>
        ///清绞刀/耙头/吸口
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? CleaningCRS { get; set; }

        /// <summary>
        ///抓斗加油
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? GrabRefueling { get; set; }

        /// <summary>
        ///等换驳
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? WaitingReplaceRefueling { get; set; }

        /// <summary>
        ///换钢丝
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ReplaceWire { get; set; }

        /// <summary>
        ///避让船舶
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? AvoidingShip { get; set; }

        /// <summary>
        ///土质 (字典表存储 TypeNo=5,可以多个类型逗号分割)
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? SoilQuality { get; set; }

        #endregion

        #region 非生产停歇（单位小时）

        /// <summary>
        ///天气影响
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? WeatherImpact { get; set; }

        /// <summary>
        ///潮流影响
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? TideImpact { get; set; }

        /// <summary>
        ///突发故障
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? SuddenFailure { get; set; }

        /// <summary>
        ///等备件待修
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? WaitingSparePartRepair { get; set; }

        /// <summary>
        ///等油水料
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? WaitingOilWaterMaterial { get; set; }

        /// <summary>
        ///等驳等拖
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? WaitingBargeAndTowing { get; set; }

        /// <summary>
        ///通知停工
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? NotifyShutdown { get; set; }

        /// <summary>
        ///设备改装及维护
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? EquipmentModificationMaintenance { get; set; }

        /// <summary>
        ///待命
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? Standby { get; set; }

        /// <summary>
        ///浮管/岸管故障
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? FSPipeFailure { get; set; }

        /// <summary>
        ///沉管故障
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? SunkenTubeFailure { get; set; }

        /// <summary>
        ///社会干扰
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? SocialInterference { get; set; }

        /// <summary>
        ///围堰/排水问题
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? CofferdamDrainageIssues { get; set; }

        /// <summary>
        ///其它
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? Other { get; set; }

        #endregion

        #region 调遣 （单位小时）

        /// <summary>
        ///执行调遣
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ExecuteDispatch { get; set; }

        /// <summary>
        ///准备调遣
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? PrepareDispatch { get; set; }

        /// <summary>
        ///船舶改装
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ShipConversion { get; set; }

        /// <summary>
        ///出海封仓
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? SealedForGoSea { get; set; }

        /// <summary>
        ///申请检验
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ApplyForInspection { get; set; }

        #endregion

        #region  定期停歇 （单位小时）

        /// <summary>
        ///船厂/基地修理
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? ShipyardRepair { get; set; }

        /// <summary>
        ///月度预防检修
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? MonthlyPreventiveMaintenance { get; set; }

        #endregion

        #region  其它 （单位小时）

        /// <summary>
        ///非工地待命
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? OffSiteStandby { get; set; }

        /// <summary>
        ///清关报关
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal? CustomsClearanceAndDeclaration { get; set; }

        #endregion

        /// <summary>
        /// 船舶状态
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public ProjectShipState ShipState { get; set; }
        /// <summary>
        /// 斗容/舱容
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? CabinCapacityOrDouRong { get; set; }

    }
}
