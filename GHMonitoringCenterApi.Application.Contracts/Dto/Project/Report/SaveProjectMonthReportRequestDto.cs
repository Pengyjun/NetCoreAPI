using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using UtilsSharp;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.SaveProjectMonthReportRequestDto;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 保存项目月报
    /// </summary>
    public class SaveProjectMonthReportRequestDto : ProjectMonthReportDto<ReqTreeProjectWBSDetailDto, ReqMonthReportDetail>, IValidatableObject, IResetModelProperty
    {


        /// <summary>
        /// （当前）驳回原因
        /// </summary>
        public string? RejectReason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不存在", new string[] { nameof(ProjectId) });
            }
            if (PartyAConfirmedProductionAmount == null || PartyAConfirmedProductionAmount < 0)
            {
                yield return new ValidationResult("本月甲方确认产值不能为空或小于0", new string[] { nameof(PartyAConfirmedProductionAmount) });
            }
            if (PartyAPayAmount == null || PartyAPayAmount < 0)
            {
                yield return new ValidationResult("本月甲方确认产值不能为空或小于0", new string[] { nameof(PartyAPayAmount) });
            }
            if (ReceivableAmount == null && ReceivableAmount < 0)
            {
                yield return new ValidationResult("本月应收金额不能为空或小于0", new string[] { nameof(ReceivableAmount) });
            }
            if (CostAmount == null && CostAmount < 0)
            {
                yield return new ValidationResult("本月实际成本不能为空或小于0", new string[] { nameof(CostAmount) });
            }
            if (ReceivableAmount == null && ReceivableAmount < 0)
            {
                yield return new ValidationResult("本月应收金额不能为空或小于0", new string[] { nameof(ReceivableAmount) });
            }
            if (ProgressDeviationReason == null || !EnumExtension.EnumToList<DeviationReason>().Any(t => t.EnumValue == (int)ProgressDeviationReason))
            {
                yield return new ValidationResult("进度偏差主因不在范围内", new string[] { nameof(ProgressDeviationReason) });
            }
            if (CostDeviationReason == null || !EnumExtension.EnumToList<DeviationReason>().Any(t => t.EnumValue == (int)CostDeviationReason))
            {
                yield return new ValidationResult("成本偏差主因不在范围内", new string[] { nameof(CostDeviationReason) });
            }
            if (string.IsNullOrWhiteSpace(ProgressDescription))
            {
                yield return new ValidationResult("主要形象进度描述不能为空", new string[] { nameof(ProgressDescription) });
            }
            if(CompleteProductionAmount==null||CompleteProductionAmount < 0)
            {
                yield return new ValidationResult("本月完成产值不能为空或小于0", new string[] { nameof(CompleteProductionAmount) });
            }
            if (CompletedQuantity == null || CompletedQuantity < 0)
            {
                yield return new ValidationResult("本月完成产量不能为空或小于0", new string[] { nameof(CompletedQuantity) });
            }
            if(!IsNonConstruction&& (TreeDetails==null|| !TreeDetails.Any()))
            {
                yield return new ValidationResult("项目产报构成不能为空", new string[] { nameof(TreeDetails) });
            }
        }

        /// <summary>
        ///  重置model属性
        /// </summary>
        public void ResetModelProperty()
        {
            TreeDetails = TreeDetails ?? new ReqTreeProjectWBSDetailDto[0];
            OutsourcingExpensesAmount = OutsourcingExpensesAmount ?? 0;
            foreach (var detail in TreeDetails)
            {
                detail.ResetModelProperty();
            }
        }

        /// <summary>
        /// 树状明细
        /// </summary>
        public class ReqTreeProjectWBSDetailDto : TreeProjectWBSDetailDto<ReqTreeProjectWBSDetailDto, ReqMonthReportDetail>, IResetModelProperty
        {

            /// <summary>
            ///  重置model属性
            /// </summary>
            public void ResetModelProperty()
            {
                if(ReportDetails!=null)
                {
                    foreach(var item in ReportDetails)
                    {
                        item.ResetModelProperty();
                    }
                }
                if(Children!=null)
                {
                    foreach(var node in Children)
                    {
                        node.ResetModelProperty();
                    }
                }
            }
        }

        /// <summary>
        /// 项目月报明细
        /// </summary>
        public class ReqMonthReportDetail : MonthReportDetailDto<ReqMonthReportDetail>, IValidatableObject, IResetModelProperty
        {

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if(ProjectWBSId==Guid.Empty)
                {
                    yield return new ValidationResult("施工分类不能为空", new string[] { nameof(ProjectWBSId) });
                }
                if (OutPutType == null || !EnumExtension.EnumToList<ConstructionOutPutType>().Any(t => t.EnumValue == (int)OutPutType))
                {
                    yield return new ValidationResult("产值属性不存在", new string[] { nameof(OutPutType) });
                }
                if (ShipId == null || ShipId == Guid.Empty)
                {
                    yield return new ValidationResult("资源不能为空", new string[] { nameof(ShipId) });
                }
                if (UnitPrice != null && UnitPrice < 0)
                {
                    yield return new ValidationResult("单价不能小于0", new string[] { nameof(UnitPrice) });
                }
            }

            /// <summary>
            ///  重置model属性
            /// </summary>
            public void ResetModelProperty()
            {
                UnitPrice = UnitPrice ?? 0;
                CompletedQuantity = CompletedQuantity ?? 0;
                OutsourcingExpensesAmount = OutsourcingExpensesAmount ?? 0;
            }
        }
    }
}
