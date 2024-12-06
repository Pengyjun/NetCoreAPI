using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 新增/修改项目日报
    /// <para>ProjectId+DateDay都可以指定日期的一条日报</para>
    /// </summary>
    public class AddOrUpdateDayReportRequestDto : IValidatableObject, IResetModelProperty
    {

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
        /// 施工日志（步骤一）
        /// </summary>
        public ReqDayReportConstructionInfo? Construction { get; set; }

        /// <summary>
        /// 文件信息（步骤二）
        /// </summary>
        public ReqDayReportFileInfo? FileInfo { get; set; }

        /// <summary>
        /// 日报信息（步骤三）
        /// </summary>
        public ReqDayReport? DayReport { get; set; }

        /// <summary>
        /// 日报步骤（1（步骤一）：保存施工日志，2（步骤二）：保存影响文件，3（步骤三）：保存日报信息）
        /// </summary>
        public DayReportProcessStep Step { get; set; }

        /// <summary>
        /// 页面入口（1：列表页，2：首页 默认列表页）
        /// </summary>
        public PageEnter? Enter { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] {nameof(ProjectId) });
            }
            if (Step == DayReportProcessStep.DayReportConstructionFinish)
            {
                if (Construction == null)
                {
                     yield return new ValidationResult("施工日志不能为空", new string[] { nameof(Construction) });
                }
            }
            else if (Step == DayReportProcessStep.DayReportUploadFileFinish)
            {
                if (FileInfo == null)
                {
                    yield return new ValidationResult("文件不能为空", new string[] { nameof(FileInfo) });
                }
            }
            else if (Step == DayReportProcessStep.DayReportFinish)
            {
                if (DayReport == null)
                {
                    yield return new ValidationResult("日报信息不能为空", new string[] { nameof(DayReport) });
                }
            }
            else
            {
                yield return new ValidationResult("步骤不存在", new string[] { nameof(Step) });
            }
        }

        /// <summary>
        /// 重置Model属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (Construction != null)
            {
                Construction.ResetModelProperty();
            }
            if (DayReport != null)
            {
                DayReport.ResetModelProperty();
            }

        }

        /// <summary>
        /// 日报信息
        /// </summary>
        public class ReqDayReport : IValidatableObject, IResetModelProperty
        {

            /// <summary>
            /// 天气（映射字典表）
            /// </summary>
            public int Weather { get; set; }

            /// <summary>
            /// 现场施工人员数量
            /// </summary>
            public int SiteConstructionPersonNum { get; set; }

            /// <summary>
            /// 现场管理人员数量
            /// </summary>
            public int SiteManagementPersonNum { get; set; }

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
            /// 项目负责人
            /// </summary>
            public string? TeamLeader { get; set; }

            /// <summary>
            /// 班组长确认时间
            /// </summary>
            public DateTime? TeamLeaderConfirmTime { get; set; }

            /// <summary>
            ///  投入设备（台）
            /// </summary>

            public int ConstructionDeviceNum { get; set; }

            /// <summary>
            ///  危大工程施工（项）
            /// </summary>
            public int HazardousConstructionNum { get; set; }

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


            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (SiteConstructionPersonNum < 0)
                {
                    yield return new ValidationResult("现场施工人员数量不能小于0", new string[] { nameof(SiteConstructionPersonNum) });
                }
                if (SiteManagementPersonNum < 0)
                {
                    yield return new ValidationResult("现场管理人员数量不能小于0", new string[] { nameof(SiteManagementPersonNum) });
                }
                //if ((SiteConstructionPersonNum < SiteManagementPersonNum))
                //{
                //    yield return new ValidationResult("现场施工人员数量不能小于现场管理人员数量", new string[] { nameof(SiteConstructionPersonNum), nameof(SiteManagementPersonNum) });
                //}
                //if (string.IsNullOrWhiteSpace(ConstructionDevice))
                //{
                //    yield return new ValidationResult("施工设备不能为空", new string[] { nameof(ConstructionDevice) });
                //}
                if (string.IsNullOrWhiteSpace(ConstructionRemarks))
                {
                    yield return new ValidationResult("施工备注不能为空", new string[] { nameof(ConstructionRemarks) });
                }
                if (ConstructionDeviceNum<0)
                {
                    yield return new ValidationResult(" 投入设备（台）不能小于0", new string[] { nameof(ConstructionDeviceNum) });
                }
                if (HazardousConstructionNum < 0)
                {
                    yield return new ValidationResult(" 危大工程施工（项）不能小于0", new string[] { nameof(HazardousConstructionNum) });
                }
                if (string.IsNullOrWhiteSpace(OtherRecord))
                {
                    yield return new ValidationResult("其他记录不能为空", new string[] { nameof(OtherRecord) });
                }
                if (string.IsNullOrWhiteSpace(TeamLeader))
                {
                    yield return new ValidationResult("项目负责人不能为空", new string[] { nameof(TeamLeader) });
                }
                if(LandWorkplace!=null&&LandWorkplace < 0)
                {
                    yield return new ValidationResult("陆地9人以上作业地点（处）不能小于0", new string[] { nameof(LandWorkplace) });
                }
                if (FewLandWorkplace  != null && FewLandWorkplace < 0)
                {
                    yield return new ValidationResult("陆域3-9人以上作业地点（处）不能小于0", new string[] { nameof(FewLandWorkplace) });
                }
                if (SiteShipNum != null && SiteShipNum < 0)
                {
                    yield return new ValidationResult("现场船舶（艘）不能小于0", new string[] { nameof(SiteShipNum) });
                }
                if (OnShipPersonNum != null && OnShipPersonNum < 0)
                {
                    yield return new ValidationResult("在船人员（人）不能小于0", new string[] { nameof(OnShipPersonNum) });
                }
            }

            /// <summary>
            /// 重置Model属性
            /// </summary>
            public void ResetModelProperty()
            {
                LandWorkplace = LandWorkplace ?? 0;
                FewLandWorkplace = FewLandWorkplace ?? 0;
                SiteShipNum = SiteShipNum ?? 0;
                OnShipPersonNum= OnShipPersonNum ?? 0;
        }
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        public class ReqDayReportFileInfo : IValidatableObject
        {
         
            /// <summary>
            /// 文件集合
            /// </summary>
            public FileInfoRequestDto[]? Files { get; set; }
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Files == null || !Files.Any())
                {
                    yield return new ValidationResult("文件不能为空", new string[] { nameof(Files) });
                }
 
            }


        }

     

        /// <summary>
        /// 施工日志信息
        /// </summary>
        public class ReqDayReportConstructionInfo : IValidatableObject, IResetModelProperty
        {
            /// <summary>
            /// 特殊事项报告（0：无,1:异常预警，2：嘉奖通报,3：提醒事项）
            /// <para>不变更为枚举，其他业务代码已经用到该字段，先保证统一性</para>
            /// </summary>
            public int IsHaveProductionWarning { get; set; }

            /// <summary>
            /// 特殊事项报告内容
            /// </summary>
            public string? ProductionWarningContent { get; set; }
            /// <summary>
            /// 偏差预警
            /// </summary>
            public string? DeviationWarning { get; set; }
            /// <summary>
            /// 是否偏低    1 是偏高  0是偏低
            /// </summary>
            public int? IsLow { get; set; }

            ///// <summary>
            ///// 已完成合同金额(元)
            ///// </summary>
            //public decimal CompleteAmount { get; set; }

            ///// <summary>
            ///// 当月计划产值（元）
            ///// </summary>
            //public decimal MonthPlannedProductionAmount { get; set; }

            /// <summary>
            /// 是否显示，春节停工计划
            ///<para>备注1：true:表示当前时间在12月23-1月15范围内，false ：不在12月23-1月15范围内</para>
            /// </summary>
            public  bool IsShowWorkStatusOfSpringFestival { get; set; }

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
            public ReqConstruction[] DayReportConstructions { get; set; } = new ReqConstruction[0];

            /// <summary>
            /// 币种Id
            /// </summary>
            public Guid CurrencyId { get; set; }

            /// <summary>
            /// 币种汇率
            /// </summary>
            public decimal CurrencyExchangeRate { get; set; }


            /// <summary>
            /// 产能较低原因
            /// <para>低于当日计划产值80%</para>
            /// </summary>
            public string? LowProductionReason { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                //if (CompleteAmount < 0)
                //{
                //    yield return new ValidationResult("已完成合同金额不能小于0", new string[] { nameof(CompleteAmount) });
                //}
                //if (MonthPlannedProductionAmount < 0)
                //{
                //    yield return new ValidationResult("当月计划产值不能小于0", new string[] { nameof(MonthPlannedProductionAmount) });
                //}
                //if (DayActualProductionAmount < 0)
                //{
                //    yield return new ValidationResult("当日实际产值不能小于0", new string[] { nameof(DayActualProductionAmount) });
                //}

                //if (IsShowWorkStatusOfSpringFestival)
                //{
                //    if (WorkStatusOfSpringFestival != WorkStatusOfSpringFestival.StopWork && WorkStatusOfSpringFestival != WorkStatusOfSpringFestival.Working)
                //    {
                //        yield return new ValidationResult("春节期间停工计划不存在", new string[] { nameof(WorkStatusOfSpringFestival) });
                //    }
                //    if (WorkStatusOfSpringFestival == WorkStatusOfSpringFestival.StopWork)
                //    {
                //        if (StartStopWorkOfSpringFestival == null)
                //        {
                //            yield return new ValidationResult("春节停工计划时间不能为空", new string[] { nameof(StartStopWorkOfSpringFestival) });
                //        }
                //        if (EndStopWorkOfSpringFestival == null)
                //        {
                //            yield return new ValidationResult("春节期间后复工计划时间不能为空", new string[] { nameof(EndStopWorkOfSpringFestival) });
                //        }
                //        if (StartStopWorkOfSpringFestival != null && EndStopWorkOfSpringFestival != null && EndStopWorkOfSpringFestival < StartStopWorkOfSpringFestival)
                //        {
                //            yield return new ValidationResult("春节期间后复工计划时间不能小于春节停工计划时间", new string[] { nameof(EndStopWorkOfSpringFestival) });
                //        }
                //    }
                //}

                if (DayReportConstructions == null || !DayReportConstructions.Any())
                {
                    yield return new ValidationResult("施工记录集合不能为空", new string[] { nameof(DayReportConstructions) });
                }
                else
                {
                    if (DayReportConstructions.DistinctBy(t => new {  t.ProjectWBSId, t.OutPutType, ShipId =(t.OutPutType== ConstructionOutPutType.SubPackage?t.SubShipId:t.OwnerShipId) ,t.UnitPrice,t.ConstructionNature}).Count()
                             != DayReportConstructions.Count())
                    {
                        yield return new ValidationResult("施工记录存在重复", new string[] { nameof(DayReportConstructions) });
                    }
				}

            }

            /// <summary>
            ///  重置Model属性
            /// </summary>
            public void ResetModelProperty()
            {
                if (WorkStatusOfSpringFestival != WorkStatusOfSpringFestival.StopWork)
                {
                    StartStopWorkOfSpringFestival = null;
                    EndStopWorkOfSpringFestival = null;
                }
                if (DayReportConstructions != null && DayReportConstructions.Any())
                {
                    foreach (var dayReportConstruction in DayReportConstructions)
                    {
                        dayReportConstruction.ResetModelProperty();
                    }
                }
                if (!IsShowWorkStatusOfSpringFestival)
                {
                    WorkStatusOfSpringFestival = WorkStatusOfSpringFestival.None;
                }
                if (!IsShowWorkStatusOfSpringFestival || WorkStatusOfSpringFestival != WorkStatusOfSpringFestival.StopWork)
                {
                    StartStopWorkOfSpringFestival = null;
                    EndStopWorkOfSpringFestival = null;
                    StartStopWorkOfSpringFestival = null;
                }
                if(IsHaveProductionWarning==0)
                {
                    ProductionWarningContent = null;
                }
            }
        }

        /// <summary>
        /// 施工记录
        /// </summary>
        public class ReqConstruction : IValidatableObject, IResetModelProperty
        {
            /// <summary>
            /// 施工日志Id
            /// </summary>
            public Guid? DayReportConstructionId { get; set; }

            /// <summary>
            /// 施工分类Id
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
			/// 单价
			/// </summary>
			public decimal UnitPrice { get; set; }

            /// <summary>
            /// 外包支出
            /// </summary>
            public decimal OutsourcingExpensesAmount { get; set; }

            /// <summary>
            /// 实际日产量(m³)
            /// </summary>
            public decimal ActualDailyProduction { get; set; }

            /// <summary>
            /// 实际日产值(元)
            /// </summary>
            public decimal ActualDailyProductionAmount { get; set; }

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
            ///// 已完成合同金额(元)
            ///// </summary>
            //public decimal? CompleteAmount { get; set; }

            /// <summary>
            /// 施工性质
            /// </summary>
            public int? ConstructionNature { get; set; }

            #endregion 

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                //if ((ProjectWBSId == null || ProjectWBSId == Guid.Empty))
                //{
                //    yield return new ValidationResult("施工分类不能为空", new string[] { nameof(ProjectWBSId) });
                //}
                if (OutPutType == ConstructionOutPutType.Self|| OutPutType == ConstructionOutPutType.SubOwner)
                {
                    if (OwnerShipId == null || OwnerShipId == Guid.Empty)
                    {
                        yield return new ValidationResult("资源不能为空", new string[] { nameof(OwnerShipId) });
                    }
                }
                else if (OutPutType == ConstructionOutPutType.SubPackage)
                {
                    if (SubShipId == null || SubShipId == Guid.Empty)
                    {
                        yield return new ValidationResult("资源不能为空", new string[] { nameof(SubShipId) });
                    }
                }
                else
                {
                    yield return new ValidationResult(" 产值属性只支持（自有，分包，分包-自有）", new string[] { nameof(OutPutType) });
                }
                if(UnitPrice<0)
                {
                    yield return new ValidationResult("单价(元)不能小于0", new string[] { nameof(UnitPrice) });
                }
                if (OutsourcingExpensesAmount < 0)
                {
                    yield return new ValidationResult("外包支出(元)不能小于0", new string[] { nameof(OutsourcingExpensesAmount) });
                }
                if (ActualDailyProduction < 0)
                {
                    yield return new ValidationResult("实际日产量(m³)不能小于0", new string[] { nameof(ActualDailyProduction) });
                }
                if (ConstructionNature==null)
                {
                    yield return new ValidationResult("施工性质不能为空", new string[] { nameof(ConstructionNature) });
                }

                #region  新增字段暂停开发-注释留存

                //if(MeteringOutputOfWeekAmount!=null&&DateTime.Now.DayOfWeek!=DayOfWeek.Friday)
                //{
                //    yield return new ValidationResult("当周计量产值只允许周五填报", new string[] { nameof(MeteringOutputOfWeekAmount) });
                //}
                //if (ActualOutputOfMonthAmount != null && DateTime.Now.Day != 25)
                //{
                //    yield return new ValidationResult("当月实际产值只允许每月25号填报", new string[] { nameof(ActualOutputOfMonthAmount) });
                //}

                #endregion
            }

            /// <summary>
            /// 重置Model属性
            /// </summary>
            public void ResetModelProperty()
            {
                if (OutPutType != ConstructionOutPutType.Self&& OutPutType != ConstructionOutPutType.SubOwner)
                {
                    OwnerShipId = null;
                }
                if (OutPutType != ConstructionOutPutType.SubPackage)
                {
                    SubShipId = null;
                }
                if(ProjectWBSId==null)
                {
                    ProjectWBSId = Guid.Empty;
                }
            }
        }
    }

}
