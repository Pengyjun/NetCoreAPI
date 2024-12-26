using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 日报相应数据类
    /// </summary>
    public class ProjectDayReportResponseDto
    {
        /// <summary>
        /// 是否是必填日报  true是 false不是
        /// </summary>
        public bool? IsFillDay { get; set; }
        #region 新增两个字段
        /// <summary>
        /// 是否是重点项目   true是  fasle不是
        /// </summary>
        public bool IsKeyProject { get; set; }
        /// <summary>
        /// 最近半个月的平均产值  
        /// </summary>
        public decimal? MonthAveProduction { get; set; }
        /// <summary>
        /// -+区间比
        /// </summary>
        public decimal? Interval {  get; set; }
        #endregion


        //辅助字段 目前只有项目年初计划使用
        public decimal DayAmount { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>

        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报日期(例：20230418)
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 是否是节假日
        /// </summary>
        public bool IsHoliday { get; set; }

        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        public int ProjectCategory { get; set; }

        /// <summary>
        /// 日报进程状态
        /// </summary>
        public DayReportProcessStatus ProcessStatus { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// 施工日志（步骤一）
        /// </summary>
        public ResDayReportConstructionInfo Construction { get; set; } = new();

        /// <summary>
        /// 影像文件信息（步骤二）
        /// </summary>
        public ResDayReportFileInfo FileInfo { get; set; } = new();

        /// <summary>
        /// 日报信息（步骤三）
        /// </summary>
        public ResDayReport DayReport { get; set; } = new();
       
        /// <summary>
        /// 日期(时间格式)
        /// </summary>
        public DateTime? DateDayTime
        {
            get
            {
                ConvertHelper.TryConvertDateTimeFromDateDay(DateDay, out DateTime dayTime);
                return dayTime;
            }
        }

        /// <summary>
        /// 是否可提交
        /// </summary>
        public bool IsCanSubmit { get; set; }

        /// <summary>
        /// 日报信息
        /// </summary>
        public class ResDayReport
        {

            /// <summary>
            /// 天气（映射字典表）
            /// </summary>
            public int? Weather { get; set; }

            /// <summary>
            /// 现场施工人员数量
            /// </summary>
            public int? SiteConstructionPersonNum { get; set; }

            /// <summary>
            /// 现场管理人员
            /// </summary>
            public int? SiteManagementPersonNum { get; set; }

            /// <summary>
            /// 施工设备
            /// </summary>
            public string? ConstructionDevice { get; set; }

            /// <summary>
            /// 施工备注（提示：请输入项目管理工作、存在问题）
            /// </summary>
            public string? ConstructionRemarks { get; set; }

            /// <summary>
            /// 其他记录
            /// </summary>
            public string? OtherRecord { get; set; }


            /// <summary>
            /// 班组长
            /// </summary>
            public string? TeamLeader { get; set; }

            /// <summary>
            /// 班组长确认时间
            /// </summary>
            public DateTime? TeamLeaderConfirmTime { get; set; }

            /// <summary>
            ///  投入设备（台）
            /// </summary>
            public int? ConstructionDeviceNum { get; set; }

            /// <summary>
            ///  危大工程施工（项）
            /// </summary>
            public int? HazardousConstructionNum { get; set; }

            /// <summary>
            /// 陆地9人以上作业地点（处）
            /// </summary>
            public int? LandWorkplace { get; set; }

            /// <summary>
            /// 带班领导
            /// </summary>
            public string? ShiftLeader { get; set; }

            /// <summary>
            /// 带班领导电话
            /// </summary>
            public string? ShiftLeaderPhone { get; set; }

            /// <summary>
            /// 陆域3-9人以上作业地点（处）
            /// </summary>
            public int? FewLandWorkplace { get; set; }

            /// <summary>
            /// 现场船舶（艘）
            /// </summary>

            public int? SiteShipNum { get; set; }

            /// <summary>
            /// 在船人员（人）
            /// </summary>
            public int? OnShipPersonNum { get; set; }

            /// <summary>
            ///  简述当日危大工程施工内容
            /// </summary>
            public string? HazardousConstructionDescription { get; set; }

        }

        /// <summary>
        /// 日报-文件部分的信息
        /// </summary>
        public class ResDayReportFileInfo
        {
          
            /// <summary>
            /// 文件集合
            /// </summary>
            public ResFileInfo[] Files { get; set; } = new ResFileInfo[0];
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        public class ResFileInfo
        {
            /// <summary>
            /// 文件Id
            /// </summary>
            public Guid? FileId { get; set; }

            /// <summary>
            /// 文件名称
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// 原始文件名称
            /// </summary>
            public string? OriginName { get; set; }

            /// <summary>
            /// 后缀名称
            /// </summary>
            public string? SuffixName { get; set; }

            /// <summary>
            /// 文件地址
            /// </summary>
            public string? Url { get; set; }

        }

        /// <summary>
        /// 日报-施工部分的信息
        /// </summary>
        public class ResDayReportConstructionInfo
        {
            /// <summary>
            /// 特殊事项报告（0：无,1:异常预警，2：嘉奖通报,3：提醒事项）
            /// </summary>
            public int IsHaveProductionWarning { get; set; }

            /// <summary>
            /// 生产异常预警信息
            /// </summary>
            public string? ProductionWarningContent { get; set; }
            /// <summary>
            /// 偏差预警
            /// </summary>
            public string? DeviationWarning { get; set; }

            ///// <summary>
            ///// 已完成合同金额(元)
            ///// </summary>
            //public decimal? CompleteAmount { get; set; }

            ///// <summary>
            ///// 当月计划产值（元）
            ///// </summary>
            //public decimal? MonthPlannedProductionAmount { get; set; }

            /// <summary>
            /// 是否显示，春节停工计划 
            /// <para>备注1：true:表示当前时间在12月23-1月15范围内，false ：不在12月23-1月15范围内</para>
            /// <para>备注2：true：显示 【春节停工计划 】,fasle：不显示【春节停工计划】 </para>
            /// </summary>
            public bool IsShowWorkStatusOfSpringFestival { get; set; }

            /// <summary>
            /// 春节停工计划（1：春节期间不停工，2：春节期间停工）
            /// </summary>
            public WorkStatusOfSpringFestival WorkStatusOfSpringFestival { get; set; }

            /// <summary>
            ///春节停工计划时间（ 春节开始停工时间）
            /// </summary>
            public DateTime? StartStopWorkOfSpringFestival { get; set; }

            /// <summary>
            /// 春节期间后复工计划时间（春节结束停工时间）
            /// </summary>
            public DateTime? EndStopWorkOfSpringFestival { get; set; }

            /// <summary>
            /// 施工记录集合
            /// </summary>
            public ResConstruction[] DayReportConstructions { get; set; } = new ResConstruction[0];

            /// <summary>
            /// 币种Id（默认人民币）
            /// </summary>
            public Guid CurrencyId { get; set; }

            /// <summary>
            /// 币种汇率
            /// </summary>
            public decimal CurrencyExchangeRate { get; set; }

            /// <summary>
            /// 当日计划产值（元）
            /// </summary>
            public decimal DayPlannedProductionAmount { get; set; }

            /// <summary>
            /// 产能较低原因
            /// <para>低于当日计划产值80%<</para>
            /// </summary>
            public string? LowProductionReason { get; set; }

        }

        /// <summary>
        /// 施工记录
        /// </summary>
        public class ResConstruction
        {

            /// <summary>
            /// 施工日志Id
            /// </summary>
            public Guid? DayReportConstructionId { get; set; }

            /// <summary>
            /// 施工分类Id(注：ProjectWBS.Id)
            /// </summary>
            public Guid? ProjectWBSId { get; set; }

            /// <summary>
            ///  产值属性（自有：1，分包：2，分包-自有：4）
            /// </summary>
            public ConstructionOutPutType OutPutType { get; set; }

            /// <summary>
            /// 自有船舶Id
            /// </summary>
            public Guid? OwnerShipId { get; set; }

            /// <summary>
            /// 分包船舶Id 或 往来单位Id
            /// </summary>
            public Guid? SubShipId { get; set; }

            /// <summary>
            /// 单价(元)
            /// </summary>
            public decimal? UnitPrice { get; set; }

            /// <summary>
            /// 外包支出(元)
            /// </summary>
            public decimal? OutsourcingExpensesAmount { get; set; }

            /// <summary>
            /// 实际日产量(m³)
            /// </summary>
            public decimal? ActualDailyProduction { get; set; }

            /// <summary>
            /// 实际日产值(元)
            /// </summary>
            public decimal? ActualDailyProductionAmount { get; set; }

            /// <summary>
            /// 施工性质
            /// </summary>
            public int? ConstructionNature { get; set; }

            #region  新增字段暂停开发-注释留存

            ///// <summary>
            ///// 当周计量产值(元)
            ///// </summary>
            //public decimal? MeteringOutputOfWeekAmount { get; set; }

            ///// <summary>
            ///// 当月实际产值(元)
            ///// </summary>
            //public decimal? ActualOutputOfMonthAmount { get; set; }

            ///// <summary>
            ///// 当月计量产值(元) 注：系统计算的来数据不可更改
            ///// </summary>
            //public decimal? MeteringOutputOfMonthAmount { get; set; }

            ///// <summary>
            ///// 已完成合同金额(元)
            ///// </summary>
            //public decimal? CompleteAmount { get; set; }

            #endregion 

            #region 扩展属性

            /// <summary>
            /// 施工一级层级名称
            /// </summary>
            public string? ProjectWBSLevel1Name { get; set; }

            /// <summary>
            /// 施工层级名称
            /// </summary>
            public string? ProjectWBSName { get; set; }

            /// <summary>
            /// 自有船舶名称
            /// </summary>
            public string? OwnerShipName { get; set; }

            /// <summary>
            /// 分包船舶名称 或 往来单位名称
            /// </summary>
            public string? SubShipName { get; set; }

            #endregion

        }

    }
}
