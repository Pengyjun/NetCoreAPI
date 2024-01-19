using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
	/// <summary>
	/// 船舶日报响应类
	/// </summary>
	public class ShipsDayReportResponseDto
	{
		/// <summary>
		/// excel导出所需标题字段
		/// </summary>
		public string TimeValue { get; set; }
		/// <summary>
		/// 船舶日报信息
		/// </summary>
		public List<ShipsDayReportInfo> shipsDayReportInfos { get; set; }

		/// <summary>
		/// 需要汇总的值
		/// </summary>
		public SumShipDayValue sumShipDayValue { get; set; }
	}

	/// <summary>
	/// 船舶日报信息
	/// </summary>
	public class ShipsDayReportInfo
	{
		/// <summary>
		/// Id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// 项目Id
		/// </summary>
		public Guid ProjectId { get; set; }
		/// <summary>
		/// 船舶Id
		/// </summary>
		public Guid? ShipId { get; set; }
		/// <summary>
		/// 船舶名称
		/// </summary>
		public string? ShipName { get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
		public string? ProjectName { get; set; }
		/// <summary>
		/// 船舶状态
		/// </summary>
		public int ShipState { get; set; }
		/// <summary>
		/// 船舶状态名称
		/// </summary>
		public string ShipStateName { get; set; }
		/// <summary>
		/// 船舶日报类型 是否关联项目  1：关联  2：不关联
		/// </summary>
		public int shipDayReportType { get; set; }

		/// <summary>
		/// 船舶类型
		/// </summary>
		public string? TypeClass { get; set; }
		/// <summary>
		/// 所属公司
		/// </summary>
		public string? ShipCompany { get; set; }
		/// <summary>
		/// 施工区域
		/// </summary>
		public string? AreaName { get; set; }
        /// <summary>
        /// 平均挖深
        /// </summary>
        public decimal? AverageEDepth { get; set; }
		/// <summary>
		/// 浮管长度
		/// </summary>
		public decimal? FloatingLength { get; set; }
        /// <summary>
        /// 前进距/舱/驳数
        /// </summary>
        public decimal? ForwardDistanceCabinNumberOfBarges { get; set; }
		/// <summary>
		/// 万方油耗
		/// </summary>
		public decimal? WanfangOilConsumption { get; set; }
		/// <summary>
		/// 当日毛利率（%）
		/// </summary>
		public decimal? DayGrossMargin { get; set; }
		/// <summary>
		/// 平均挖宽
		/// </summary>
		public decimal? AverageExcavationWidth { get; set; }
        /// <summary>
        /// 岸管长度
        /// </summary>
        public decimal? LengthOfShorePipe { get; set; }
        /// <summary>
        /// 沉管长度
        /// </summary>
        public decimal? ImmersedTubeLength { get; set; }
        /// <summary>
        /// 开工展布
        /// </summary>
        public decimal? ConstructionAndLayout { get; set; }
		/// <summary>
		/// 备机
		/// </summary>
        public decimal? StandbyMachine { get; set; }
        /// <summary>
        /// 下(移)锚览
        /// </summary>
        public decimal? DownMoveAnchorView { get; set; }
        /// <summary>
        /// 移船
        /// </summary>
        public decimal? MovingAShip { get; set; }
        /// <summary>
        /// 焊绞刀/齿座
        /// </summary>
        public decimal? WeldingCutterToothSeat { get; set; }
        /// <summary>
        /// 换齿
        /// </summary>
        public decimal? GearChange { get; set; }
        /// <summary>
        /// 补给
        /// </summary>
        public decimal? Supply { get; set; }
        /// <summary>
        /// 测量影响
        /// </summary>
        public decimal? MeasurementImpact { get; set; }
        /// <summary>
        /// 敷设管线
        /// </summary>
        public decimal? LayingPipelines { get; set; }
        /// <summary>
        /// 调整管线
        /// </summary>
        public decimal? AdjustingThePipeline { get; set; }
        /// <summary>
        /// 清泥泵
        /// </summary>
        public decimal? MudCleaningPump { get; set; }
        /// <summary>
        /// 抓斗加油
        /// </summary>
        public decimal? GrabRefueling { get; set; }
        /// <summary>
        /// 等换驳
        /// </summary>
        public decimal? WaitingForTransfer { get; set; }
        /// <summary>
        /// 换钢丝
        /// </summary>
        public decimal? ReplaceTheSteelWire { get; set; }
        /// <summary>
        /// 避让船舶
        /// </summary>
        public decimal? AvoidingShips { get; set; }
        /// <summary>
        /// 天气影响
        /// </summary>
        public decimal? WeatherEffect { get; set; }
        /// <summary>
        /// 潮流影响
        /// </summary>
        public decimal? TidalInfluence { get; set; }
        /// <summary>
        /// 突发故障
        /// </summary>
        public decimal? SuddenFailure { get; set; }
        /// <summary>
        /// 等备件待修
        /// </summary>
        public decimal? WaitingForSparePartsToBeRepaired { get; set; }
        /// <summary>
        /// 等油水料
        /// </summary>
        public decimal? OilAndWaterEquivalentMaterials { get; set; }
        /// <summary>
        /// 等驳等拖
        /// </summary>
        public decimal? WaitingForBargeAndTowing { get; set; }
        /// <summary>
        /// 通知停工
        /// </summary>
        public decimal? NotifyShutdown { get; set; }
        /// <summary>
        /// 设备改装及维护
        /// </summary>
        public decimal? EquipmentModificationAndMaintenance { get; set; }
        /// <summary>
        /// 浮管/岸管故障
        /// </summary>
        public decimal? FloatingShorePipeFailure { get; set; }
        /// <summary>
        /// 沉管故障
        /// </summary>
        public decimal? SunkenTubeFailure { get; set; }
        /// <summary>
        /// 社会干扰
        /// </summary>
        public decimal? SocialInterference { get; set; }
        /// <summary>
        /// 围堰/排水问题
        /// </summary>
        public decimal? CofferdamDrainageIssues { get; set; }
        /// <summary>
        /// 非生产性停歇_其它
        /// </summary>
        public decimal? NonProductiveDowntime_Other { get; set; }
        /// <summary>
        /// 执行调遣
        /// </summary>
        public decimal? ExecuteDispatch { get; set; }
        /// <summary>
        /// 船舶改装
        /// </summary>
        public decimal? ShipConversion { get; set; }
        /// <summary>
        /// 出海封仓
        /// </summary>
        public decimal? SealedWarehouseAtSea { get; set; }
        /// <summary>
        /// 申请检验
        /// </summary>
        public decimal? ApplyForInspection { get; set; }
        /// <summary>
        /// 船厂/基地修理
        /// </summary>
        public decimal? ShipyardBaseRepair { get; set; }
        /// <summary>
        /// 月度预防检修
        /// </summary>
        public decimal? MonthlyPreventiveMaintenance { get; set; }
        /// <summary>
        /// 非工地待命
        /// </summary>
        public decimal? OffSiteStandby { get; set; }
		/// <summary>
		/// 待命
		/// </summary>
		public decimal? Standby { get; set; }
		/// <summary>
		/// 清关报关
		/// </summary>
		public decimal? CustomsClearanceAndCustomsDeclaration { get; set; }
        /// <summary>
        /// 土质
        /// </summary>
        public string? SoilQuality { get; set; }
        /// <summary>
        /// 清理绞刀/耙头/吸口
        /// </summary>
        public decimal? CleanTheCutterRakeHeadSuctionPort { get; set; }
        /// <summary>
        /// 准备调遣
        /// </summary>
        public decimal? PrepareForDispatch { get; set; }
        /// <summary>
        /// 估算油价
        /// </summary>
        public decimal? EstimatingOilPrices { get; set; }
		/// <summary>
		/// 估算单价
		/// </summary>
        public decimal? EstimatedUnitPrice { get; set; }
        /// <summary>
        /// 船舶产量(方)
        /// </summary>
        public decimal? ShipReportedProduction { get; set; }

		/// <summary>
		/// 管线长度(m)
		/// </summary>
		public decimal? PipeLineLength { get; set; }

		/// <summary>
		/// 油耗
		/// </summary>
		public decimal? OilConsumption { get; set; }
		/// <summary>
		/// 估算成本(元)
		/// </summary>
		public decimal? EstimatedCostAmount { get; set; }
		/// <summary>
		/// 估算产值(元)
		/// </summary>
		public decimal? EstimatedOutputAmount { get; set; }

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
		/// 生产运转时间
		/// </summary>
		public decimal? ProductionOperatingTime { get; set; }

		/// <summary>
		///抛泥
		/// </summary>
		public decimal? SedimentDisposal { get; set; }

		/// <summary>
		///吹岸
		/// </summary>
		public decimal? BlowShore { get; set; }

		/// <summary>
		/// 运转时间(h)
		/// </summary>
		public decimal? OperatingTime { get; set; }
		/// <summary>
		/// 时间利用率(%)
		/// </summary>
		public decimal? TimeAvailability { get; set; }
		/// <summary>
		/// 施工效率
		/// </summary>
		public decimal? ConstructionEfficiency { get; set; }
		/// <summary>
		/// 生产停歇(h)
		/// </summary>
		public decimal? ProductionStoppage { get; set; }
		/// <summary>
		/// 非生产停歇(h)
		/// </summary>
		public decimal? NonProductionStoppage { get; set; }
		/// <summary>
		/// 调遣(h)
		/// </summary>
		public decimal? Dispatch { get; set; }
		/// <summary>
		/// 定时停歇(h)
		/// </summary>
		public decimal? TimedPause { get; set; }
		/// <summary>
		/// 其他(h)
		/// </summary>
		public decimal? OtherTime { get; set; }
		/// <summary>
		/// 管线管理
		/// </summary>
        public decimal? PipeLineManageMent { get; set; }
		/// <summary>
		/// 船机故障
		/// </summary>
		public decimal? ShipEngineFailure { get; set; }
		/// <summary>
		/// 故障及修理
		/// </summary>
		public decimal? FaultsRepairs { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string? Remarks { get; set; }

		/// <summary>
		/// 进场时间
		/// </summary>
		public DateTime? EnterTime { get; set; }

		/// <summary>
		/// 退场时间
		/// </summary>
		public DateTime? Quittime { get; set; }

		/// <summary>
		/// 填报日期(例：20230418)
		/// </summary>
		public int? DateDay { get; set; }
		/// <summary>
		/// 日期(时间格式)
		/// </summary>
		public string? DateDayTime
		{
			get
			{
				if (ConvertHelper.TryConvertDateTimeFromDateDay((int)DateDay, out DateTime dayTime))
				{
					return dayTime.ToString("yyyy-MM-dd");
				}
				return null;
			}
		}
	}

	/// <summary>
	/// 需要汇总的值
	/// </summary>
	public class SumShipDayValue
	{
		// <summary>
		/// 船舶产量(方)
		/// </summary>
		public decimal? SumShipReportedProduction { get; set; }
		/// <summary>
		/// 油耗
		/// </summary>
		public decimal? SumOilConsumption { get; set; }
		/// <summary>
		/// 估算成本(元)
		/// </summary>
		public decimal? SumEstimatedCostAmount { get; set; }
		/// <summary>
		/// 估算产值(元)
		/// </summary>
		public decimal? SumEstimatedOutputAmount { get; set; }
		/// <summary>
		/// 运转时间(h)
		/// </summary>
		public decimal? SumOperatingTime { get; set; }
		/// <summary>
		/// 生产停歇(h)
		/// </summary>
		public decimal? SumProductionStoppage { get; set; }
		/// <summary>
		/// 非生产停歇(h)
		/// </summary>
		public decimal? SumNonProductionStoppage { get; set; }
		/// <summary>
		/// 调遣(h)
		/// </summary>
		public decimal? SumDispatch { get; set; }
		/// <summary>
		/// 定时停歇(h)
		/// </summary>
		public decimal? SumTimedPause { get; set; }
		/// <summary>
		/// 其他(h)
		/// </summary>
		public decimal? SumOtherTime { get; set; }
	}

}
