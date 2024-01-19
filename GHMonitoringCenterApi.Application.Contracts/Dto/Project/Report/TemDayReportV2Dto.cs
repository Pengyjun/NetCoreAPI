//using GHMonitoringCenterApi.Domain.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations;
//using GHMonitoringCenterApi.Domain.Models;
//using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;

//*==============项目日报多人填报Dto备份类
//namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
//{
//    /// <summary>
//    /// 保存日报请求
//    /// </summary>
//    public class SaveDayReportV2RequestDto : DayReportV2Dto<SaveDayReportV2RequestDto.ReqConstructionDto>, IValidatableObject, IResetModelProperty
//    {

//        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//        {
//            if ( ProjectId == null || ProjectId == Guid.Empty)
//            {
//                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
//            }
//            if (DateDay != DateTime.Now.ToDateDay())
//            {
//                yield return new ValidationResult("项目日报只能修改今天的数据", new string[] { nameof(DateDay) });
//            }


//        }


//        /// <summary>
//        /// 施工记录类
//        /// </summary>
//        public class ReqConstructionDto : ConstructionDto, IValidatableObject, IResetModelProperty
//        {

//            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//            {
  
              
//            }

//            /// <summary>
//            /// 重置Model属性
//            /// </summary>
//            public void ResetModelProperty()
//            {
               
//            }
//        }

//        /// <summary>
//        /// 重置Model属性
//        /// </summary>
//        public void ResetModelProperty()
//        {
//           if(DayReportConstructions!=null)
//            {
//               foreach(var Construction in DayReportConstructions)
//                {
//                    Construction.ResetModelProperty();
//                }
//            }
//        }
//    }


//    /// <summary>
//    /// 项目日报dto
//    /// </summary>
//    public abstract class DayReportV2Dto<TConstructionDto> where TConstructionDto : DayReportV2Dto<TConstructionDto>.ConstructionDto
//    {
//        /// <summary>
//        /// 项目Id
//        /// </summary>

//        public Guid? ProjectId { get; set; }

//        /// <summary>
//        /// 填报日期(例：20230418)
//        /// </summary>
//        public int? DateDay { get; set; }

//        /// <summary>
//        /// 天气（映射字典表）
//        /// </summary>
//        public int? Weather { get; set; }

//        /// <summary>
//        /// 现场施工人员数量
//        /// </summary>
//        public int? SiteConstructionPersonNum { get; set; }

//        /// <summary>
//        /// 现场管理人员
//        /// </summary>
//        public int? SiteManagementPersonNum { get; set; }

//        /// <summary>
//        /// 施工设备
//        /// </summary>
//        public string? ConstructionDevice { get; set; }

//        /// <summary>
//        /// 施工备注（提示：请输入项目管理工作、存在问题）
//        /// </summary>
//        public string? ConstructionRemarks { get; set; }

//        /// <summary>
//        /// 其他记录
//        /// </summary>
//        public string? OtherRecord { get; set; }


//        /// <summary>
//        /// 班组长
//        /// </summary>
//        public string? TeamLeader { get; set; }

//        /// <summary>
//        /// 班组长确认时间
//        /// </summary>
//        public DateTime? TeamLeaderConfirmTime { get; set; }

//        /// <summary>
//        /// 是否存在生产异常预警
//        /// </summary>
//        public bool? IsHaveProductionWarning { get; set; }

//        /// <summary>
//        /// 生产异常预警信息
//        /// </summary>
//        public string? ProductionWarningContent { get; set; }

//        /// <summary>
//        /// 已完成合同金额(元)
//        /// </summary>
//        public decimal? CompleteAmount { get; set; }

//        /// <summary>
//        /// 当月计划产值（元）
//        /// </summary>
//        public decimal? MonthPlannedProductionAmount { get; set; }

//        /// <summary>
//        /// 是否显示，春节停工计划 
//        /// <para>备注1：true:表示当前时间在12月23-1月15范围内，false ：不在12月23-1月15范围内</para>
//        /// <para>备注2：true：显示 【春节停工计划 】,fasle：不显示【春节停工计划】 </para>
//        /// </summary>
//        public bool IsShowWorkStatusOfSpringFestival
//        {
//            get
//            {
//                var nowTime = DateTime.Now;
//                if (nowTime >= new DateTime(nowTime.Year, 12, 23) && nowTime < new DateTime(nowTime.Year + 1, 1, 1))
//                {
//                    return true;
//                }
//                else if (nowTime >= new DateTime(nowTime.Year, 1, 1) && nowTime < new DateTime(nowTime.Year, 1, 16))
//                {
//                    return true;
//                }
//                return false;
//            }
//        }

//        /// <summary>
//        /// 春节停工计划（1：春节期间不停工，2：春节期间停工）
//        /// </summary>
//        public WorkStatusOfSpringFestival WorkStatusOfSpringFestival { get; set; }

//        /// <summary>
//        ///春节停工计划时间（ 春节开始停工时间）
//        /// </summary>
//        public DateTime? StartStopWorkOfSpringFestival { get; set; }

//        /// <summary>
//        /// 春节期间后复工计划时间（春节结束停工时间）
//        /// </summary>
//        public DateTime? EndStopWorkOfSpringFestival { get; set; }

//        /// <summary>
//        /// 项目名称
//        /// </summary>
//        public string? ProjectName { get; set; }

//        /// <summary>
//        /// 创建人姓名
//        /// </summary>
//        public string? CreateUserName { get; set; }

//        #region 文件信息

//        /// <summary>
//        /// 文件Id
//        /// </summary>
//        public Guid? FileId { get; set; }

//        /// <summary>
//        /// 文件名称
//        /// </summary>
//        public string? Name { get; set; }

//        /// <summary>
//        /// 原始文件名称
//        /// </summary>
//        public string? OriginName { get; set; }

//        /// <summary>
//        /// 后缀名称
//        /// </summary>
//        public string? SuffixName { get; set; }

//        #endregion

//        /// <summary>
//        /// 施工记录集合
//        /// </summary>
//        public TConstructionDto[] DayReportConstructions { get; set; } = new TConstructionDto[0];

//        /// <summary>
//        /// 日期(时间格式)
//        /// </summary>
//        public DateTime? DateDayTime
//        {
//            get
//            {
//                if (DateDay == null)
//                {
//                    return null;
//                }
//                if (ConvertHelper.TryConvertDateTimeFromDateDay((int)DateDay, out DateTime dayTime))
//                {
//                    return dayTime;
//                }
//                return null;
//            }
//        }

//        /// <summary>
//        /// 施工记录
//        /// </summary>
//        public abstract class ConstructionDto
//        {

//            /// <summary>
//            /// 施工日志Id
//            /// </summary>
//            public Guid? DayReportConstructionId { get; set; }

//            /// <summary>
//            /// 施工分类Id(注：ProjectWBS.Id)
//            /// </summary>
//            public Guid? ProjectWBSId { get; set; }

//            /// <summary>
//            ///  产值属性（自有：1，分包：2）
//            /// </summary>
//            public ConstructionOutPutType OutPutType { get; set; }

//            /// <summary>
//            /// 自有船舶Id
//            /// </summary>
//            public Guid? OwnerShipId { get; set; }

//            /// <summary>
//            /// 分包船舶Id 或 往来单位Id
//            /// </summary>
//            public Guid? SubShipId { get; set; }

//            /// <summary>
//            /// 单价(元)
//            /// </summary>
//            public decimal? UnitPrice { get; set; }

//            /// <summary>
//            /// 外包支出(元)
//            /// </summary>
//            public decimal? OutsourcingExpensesAmount { get; set; }

//            /// <summary>
//            /// 实际日产量(m³)
//            /// </summary>
//            public decimal? ActualDailyProduction { get; set; }

//            /// <summary>
//            /// 实际日产值(元)
//            /// </summary>
//            public decimal? ActualDailyProductionAmount { get; set; }

//            /// <summary>
//            /// 施工一级层级名称
//            /// </summary>
//            public string? ProjectWBSLevel1Name { get; set; }

//            /// <summary>
//            /// 施工层级名称
//            /// </summary>
//            public string? ProjectWBSName { get; set; }

//            /// <summary>
//            /// 自有船舶名称
//            /// </summary>
//            public string? OwnerShipName { get; set; }

//            /// <summary>
//            /// 分包船舶名称 或 往来单位名称
//            /// </summary>
//            public string? SubShipName { get; set; }
//        }
//    }
   
//}
