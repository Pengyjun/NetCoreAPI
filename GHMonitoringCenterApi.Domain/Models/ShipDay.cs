using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
		/// <summary>
		/// 船舶日报表
		/// </summary>
		[SugarTable("t_shipday", IsDisabledDelete = true)]
		public class ShipDay
		{
			/// <summary>
			/// 主键Id
			/// </summary>
			[SugarColumn(Length = 36, IsPrimaryKey = true)]
			public Guid? Id { get; set; }
			/// <summary>
			/// ItemID
			/// </summary>
			[SugarColumn(ColumnDataType = "int")]
			public int PomId { get; set; }
			/// <summary>
			/// 标题
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? Title { get; set; }
			/// <summary>
			/// 修改时间
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? UpdateTime { get; set; }
			/// <summary>
			/// 创建时间
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? CreateTime { get; set; }
			/// <summary>
			/// 创建者
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? CreateUser { get; set; }
			/// <summary>
			/// 项目Id
			/// </summary>
			[SugarColumn(Length = 36)]
			public Guid? ProjectId { get; set; }
			/// <summary>
			/// 项目名称
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ProjectName { get; set; }
			/// <summary>
			/// 项目编码
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ProjectCode { get; set; }
			/// <summary>
			/// 船舶名称
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipName { get; set; }
			/// <summary>
			/// 船舶编码
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipCode { get; set; }
			/// <summary>
			/// 日期
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipDate { get; set; }
			/// <summary>
			/// 施工区域
			/// </summary>
			[SugarColumn(Length = 2000)]
			public string? ConstructionArea { get; set; }
			/// <summary>
			/// 平均挖深
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? AverageDeep { get; set; }
			/// <summary>
			/// 浮管长度
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? FloatingPipelineLength { get; set; }
			/// <summary>
			/// 前进距/舱/驳数
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ForwardNum { get; set; }
			/// <summary>
			/// 船报产量(方)
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipReportProduction { get; set; }
			/// <summary>
			/// 平均挖宽
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? AverageWide { get; set; }
			/// <summary>
			/// 岸管长度
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShorePipeLength { get; set; }
			/// <summary>
			/// 油耗
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? OilConsume { get; set; }
			/// <summary>
			/// 备注
			/// </summary>
			[SugarColumn(Length = 3000)]
			public string? Remark { get; set; }
			/// <summary>
			///  沉管长度
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ImmersedTubeLength { get; set; }
			/// <summary>
			/// 挖泥
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? Dredge { get; set; }
			/// <summary>
			/// 航行
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? Sail { get; set; }
			/// <summary>
			/// 吹水
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? BlowingWater { get; set; }
			/// <summary>
			/// 抛泥
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? SedimentDisposal { get; set; }
			/// <summary>
			/// 吹岸
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? BlowShore { get; set; }
			/// <summary>
			/// 开工展布
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ConstructionLayout { get; set; }
			/// <summary>
			/// 备机
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? StandbyMachine { get; set; }
			/// <summary>
			/// 下(移)锚览
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? DownAnchorView { get; set; }
			/// <summary>
			/// 移船
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? MoveShip { get; set; }
			/// <summary>
			/// 焊绞刀/齿座
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? WeldingSeat { get; set; }
			/// <summary>
			/// 换齿
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? GearChange { get; set; }
			/// <summary>
			/// 补给
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? Supply { get; set; }
			/// <summary>
			/// 测量影响
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? MeasureEffect { get; set; }
			/// <summary>
			/// 敷设管线
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? LayingPipelines { get; set; }
			/// <summary>
			/// 调整管线
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? AdjustPipelines { get; set; }
			/// <summary>
			/// 清泥泵
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? MudCleaningPump { get; set; }
			/// <summary>
			/// 抓斗加油
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? GrabRefueling { get; set; }
			/// <summary>
			/// 等换驳
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? WaitingTransfer { get; set; }
			/// <summary>
			/// 换钢丝
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ReplaceSteelWire { get; set; }
			/// <summary>
			/// 避让船舶
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? AvoidanceShip { get; set; }
			/// <summary>
			/// 天气影响
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? WeatherEffect { get; set; }
			/// <summary>
			/// 潮流影响
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? TrendEffect { get; set; }
			/// <summary>
			/// 突发故障
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? SuddenFailure { get; set; }
			/// <summary>
			/// 等备件待修
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? WaitRepaired { get; set; }
			/// <summary>
			/// 等油水料
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? WaitOilWater { get; set; }
			/// <summary>
			/// 等驳等拖
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? WaitRefuteDrag { get; set; }
			/// <summary>
			/// 通知停工
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? NotifyShutdown { get; set; }
			/// <summary>
			/// 设备改装及维护
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? EquipmentRefitMaintenance { get; set; }
			/// <summary>
			/// 窝工
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? Idleness { get; set; }
			/// <summary>
			/// 浮管/岸管故障
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? FloatingShorePipeFault { get; set; }
			/// <summary>
			/// 沉管故障
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ImmersedTubeFault { get; set; }
			/// <summary>
			/// 社会干扰
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? SocialInterference { get; set; }
			/// <summary>
			/// 围堰/排水问题
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? CofferdamDrainageIssues { get; set; }
			/// <summary>
			/// 非生产性停歇其他
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? NonCloseDownOther { get; set; }
			/// <summary>
			/// 执行调遣
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ExecuteDispatch { get; set; }
			/// <summary>
			/// 船舶改装
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipRefit { get; set; }
			/// <summary>
			/// 出海封仓
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? OutSealWarehouse { get; set; }
			/// <summary>
			/// 申请检验
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ApplyInspection { get; set; }
			/// <summary>
			/// 船厂/基地修理
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipyardBaseRepair { get; set; }
			/// <summary>
			/// 月度预防检修
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? MonthPreventOverhaul { get; set; }
			/// <summary>
			/// 非工地待命
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? NonSiteStandby { get; set; }
			/// <summary>
			/// 清关报关
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? CustomsClearance { get; set; }
			/// <summary>
			/// 土质
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? SoilQuality { get; set; }
			/// <summary>
			/// 土质简称
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? SoilQualityShort { get; set; }
			/// <summary>
			/// 清理绞刀/耙头/吸头
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ClearHead { get; set; }
			/// <summary>
			/// 船舶操作(h)
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipOperate { get; set; }
			/// <summary>
			/// 船机故障(h)
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? ShipFault { get; set; }
			/// <summary>
			/// 故障及检修(y)
			/// </summary>
			[SugarColumn(Length = 50)]
			public string? FaultOverhaul { get; set; }
			/// <summary>
			/// 管线管理(h)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? PipelineManage { get; set; }
			/// <summary>
			/// 管线长度
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? PipelineLength { get; set; }
			/// <summary>
			/// 生产停歇(y)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ProductionStoppage { get; set; }
			/// <summary>
			/// 运转时间(h)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? OperatingTime { get; set; }
			/// <summary>
			/// 施工效率
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ConstructionEfficiency { get; set; }
			/// <summary>
			/// 时间利用率(%)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? TimeUtilizationRate { get; set; }
			/// <summary>
			/// 预计万方油耗
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ExpectWanfangOilConsume { get; set; }
			/// <summary>
			/// 准备调遣
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? PrepareDispatch { get; set; }
			/// <summary>
			/// 自然影响(h)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? NaturalEffect { get; set; }
			/// <summary>
			/// 其他停工(h)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? OtherShutdown { get; set; }
			/// <summary>
			/// 日报状态
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? DailyState { get; set; }
			/// <summary>
			/// 船机成本(不含燃油)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ShipCostNonOil { get; set; }
			/// <summary>
			/// 预计油耗
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ExpectOilConsume { get; set; }
			/// <summary>
			/// 参考单价
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ReferPrice { get; set; }
			/// <summary>
			/// 项目简称
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ProjectShortName { get; set; }
			/// <summary>
			/// 估算油价
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? EstimateOilPrice { get; set; }
			/// <summary>
			/// 船舶类型
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ShipType { get; set; }
			/// <summary>
			/// 估算产值
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? EstimateOutputValue { get; set; }
			/// <summary>
			/// ItemGuid
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ItemGuid { get; set; }
			/// <summary>
			/// 文档路径
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? FilePath { get; set; }
			/// <summary>
			/// 船机成本
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ShipCost { get; set; }
			/// <summary>
			/// 船舶名称 排序
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? ShipNameSort { get; set; }
			/// <summary>
			/// 其他非生产停歇(S)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? OtherNonCloseDownS { get; set; }
			/// <summary>
			/// 非生产停歇(y)
			/// </summary>
			[SugarColumn(Length = 255)]
			public string? NonCloseDownY { get; set; }
		}
}
