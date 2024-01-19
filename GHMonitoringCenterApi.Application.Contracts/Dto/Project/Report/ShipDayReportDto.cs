using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 船舶日报Dto
    /// </summary>
    public abstract  class ShipDayReportDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶日报类型(1:项目-船舶日报,2:动态船舶日报)
        /// </summary>
        public ShipDayReportType ShipDayReportType { get; set; }

        /// <summary>
        /// 所在港口
        /// </summary>
        public Guid? PortId { get; set; }

        /// <summary>
        /// 施工区域
        /// </summary>
        public string? ConstructionArea { get; set; }

        /// <summary>
        ///平均挖深（m）
        /// </summary>
        public decimal? AverageExcavationDepth { get; set; }

        /// <summary>
        ///平均挖宽（m）
        /// </summary>
        public decimal? AverageExcavationWidth { get; set; }

        /// <summary>
        ///船报产量(方)
        /// </summary>
        public decimal? ShipReportedProduction { get; set; }

        /// <summary>
        ///前进距/舱/驳数
        /// </summary>
        public decimal? ForwardNumber { get; set; }

        /// <summary>
        ///油耗
        /// </summary>
        public decimal? OilConsumption { get; set; }

        /// <summary>
        ///管线长度（m）
        /// </summary>
        public decimal? PipelineLength { get; set; }

        /// <summary>
        ///浮管长度(m)
        /// </summary>
        public decimal? FloatingTubeLength { get; set; }

        /// <summary>
        ///岸管长度（m）
        /// </summary>
        public decimal? ShorePipeLength { get; set; }

        /// <summary>
        /// 沉管长度（m）
        /// </summary>
        public decimal? ImmersedTubeLength { get; set; }

        /// <summary>
        /// 估算单价（元/m³）
        /// </summary>
        public decimal? EstimatedUnitPrice { get; set; }

        /// <summary>
        /// 燃油单价（元/吨）
        /// </summary>
        public decimal? FuelUnitPrice { get; set; }

        /// <summary>
        ///估算产值（元）
        /// </summary>
        public decimal? EstimatedOutputAmount { get; set; }

        /// <summary>
        ///估算成本(元)
        /// </summary>
        public decimal? EstimatedCostAmount { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// 特殊事项报告（0：无,1:异常预警，2：嘉奖通报，3：提醒事项）
        /// </summary>
        public int IsHaveProductionWarning { get; set; }

        /// <summary>
        /// 特殊事项信息
        /// </summary>
        public string? ProductionWarningContent { get; set; }

        /// <summary>
        /// 施工模式
        /// </summary>
        public string? ConstructionmMode { get; set; }

        /// <summary>
        ///吹矩(m)
        /// </summary>
        public decimal? BlowTorque { get; set; }

        /// <summary>
        ///船存燃油（t）
        /// </summary>
        public decimal? ShipboardFuel { get; set; }


        #region 生产运转（单位小时）

        /// <summary>
        ///挖泥
        /// </summary>
        public decimal? Dredge { get; set; }

        /// <summary>
        ///航行
        /// </summary>
        public decimal? Sail { get; set; }

        /// <summary>
        ///吹水
        /// </summary>
        public decimal? BlowingWater { get; set; }

        /// <summary>
        ///抛泥
        /// </summary>
        public decimal? SedimentDisposal { get; set; }

        /// <summary>
        ///吹岸
        /// </summary>
        public decimal? BlowShore { get; set; }

        /// <summary>
        ///燃油补给（t）
        /// </summary>
        public decimal? FuelSupply { get; set; }

        #endregion

        #region 生产停歇 （单位小时）

        /// <summary>
        ///开工展布
        /// </summary>
        public decimal? ConstructionLayout { get; set; }

        /// <summary>
        ///备机
        /// </summary>
        public decimal? StandbyMachine { get; set; }

        /// <summary>
        ///下(移)锚览
        /// </summary>
        public decimal? DownAnchor { get; set; }

        /// <summary>
        ///移船
        /// </summary>
        public decimal? MovingShip { get; set; }

        /// <summary>
        ///焊绞刀/齿座
        /// </summary>
        public decimal? WeldingCutter { get; set; }

        /// <summary>
        ///换齿
        /// </summary>
        public decimal? ChangeGear { get; set; }

        /// <summary>
        ///补给
        /// </summary>
        public decimal? Supply { get; set; }

        /// <summary>
        ///测量影响
        /// </summary>
        public decimal? MeasurementImpact { get; set; }

        /// <summary>
        ///敷设管线
        /// </summary>
        public decimal? LayingPipelines { get; set; }

        /// <summary>
        ///调整管线
        /// </summary>
        public decimal? AdjustingPipeline { get; set; }

        /// <summary>
        ///清泥泵
        /// </summary>
        public decimal? CleaningPump { get; set; }

        /// <summary>
        ///清绞刀/耙头/吸口
        /// </summary>
        public decimal? CleaningCRS { get; set; }

        /// <summary>
        ///抓斗加油
        /// </summary>
        public decimal? GrabRefueling { get; set; }

        /// <summary>
        ///等换驳
        /// </summary>
        public decimal? WaitingReplaceRefueling { get; set; }

        /// <summary>
        ///换钢丝
        /// </summary>
        public decimal? ReplaceWire { get; set; }

        /// <summary>
        ///避让船舶
        /// </summary>
        public decimal? AvoidingShip { get; set; }

        /// <summary>
        ///土质 (字典表存储 TypeNo=5,可以多个类型逗号分割)
        /// </summary>
        public string? SoilQuality { get; set; }

        #endregion

        #region 非生产停歇（单位小时）

        /// <summary>
        ///天气影响
        /// </summary>
        public decimal? WeatherImpact { get; set; }

        /// <summary>
        ///潮流影响
        /// </summary>
        public decimal? TideImpact { get; set; }

        /// <summary>
        ///突发故障
        /// </summary>
        public decimal? SuddenFailure { get; set; }

        /// <summary>
        ///等备件待修
        /// </summary>
        public decimal? WaitingSparePartRepair { get; set; }

        /// <summary>
        ///等油水料
        /// </summary>
        public decimal? WaitingOilWaterMaterial { get; set; }

        /// <summary>
        ///等驳等拖
        /// </summary>
        public decimal? WaitingBargeAndTowing { get; set; }

        /// <summary>
        ///通知停工
        /// </summary>
        public decimal? NotifyShutdown { get; set; }

        /// <summary>
        ///设备改装及维护
        /// </summary>
        public decimal? EquipmentModificationMaintenance { get; set; }

        /// <summary>
        ///待命
        /// </summary>
        public decimal? Standby { get; set; }

        /// <summary>
        ///浮管/岸管故障
        /// </summary>
        public decimal? FSPipeFailure { get; set; }

        /// <summary>
        ///沉管故障
        /// </summary>
        public decimal? SunkenTubeFailure { get; set; }

        /// <summary>
        ///社会干扰
        /// </summary>
        public decimal? SocialInterference { get; set; }

        /// <summary>
        ///围堰/排水问题
        /// </summary>
        public decimal? CofferdamDrainageIssues { get; set; }

        /// <summary>
        ///其它
        /// </summary>
        public decimal? Other { get; set; }

        #endregion

        #region 调遣 （单位小时）

        /// <summary>
        ///执行调遣
        /// </summary>
        public decimal? ExecuteDispatch { get; set; }

        /// <summary>
        ///准备调遣
        /// </summary>
        public decimal? PrepareDispatch { get; set; }

        /// <summary>
        ///船舶改装
        /// </summary>
        public decimal? ShipConversion { get; set; }

        /// <summary>
        ///出海封仓
        /// </summary>
        public decimal? SealedForGoSea { get; set; }

        /// <summary>
        ///申请检验
        /// </summary>
        public decimal? ApplyForInspection { get; set; }

        #endregion

        #region  定期停歇 （单位小时）

        /// <summary>
        ///船厂/基地修理
        /// </summary>
        public decimal? ShipyardRepair { get; set; }

        /// <summary>
        ///月度预防检修
        /// </summary>
        public decimal? MonthlyPreventiveMaintenance { get; set; }

        #endregion

        #region  其它 （单位小时）

        /// <summary>
        ///非工地待命
        /// </summary>
        public decimal? OffSiteStandby { get; set; }

        /// <summary>
        ///清关报关
        /// </summary>
        public decimal? CustomsClearanceAndDeclaration { get; set; }

        #endregion

        /// <summary>
        /// 船舶状态(船舶动态清单)
        /// </summary>
        public ProjectShipState? ShipState { get; set; }
    }
}
