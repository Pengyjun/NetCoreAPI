using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 保存分包船舶月报请求对象
    /// </summary>
    public class SaveSubShipMonthReportRequestDto : SubShipMonthReportDto, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ShipId) });
            }
            if (EnterTime == null)
            {
                yield return new ValidationResult("进场时间不能为空", new string[] { nameof(EnterTime) });
            }
            if (QuitTime == null)
            {
                yield return new ValidationResult("退场时间不能为空", new string[] { nameof(QuitTime) });
            }
            if (EnterTime != null && QuitTime != null && QuitTime < EnterTime)
            {
                yield return new ValidationResult("退场时间不能小于进场时间", new string[] { nameof(QuitTime) });
            }
            if (WorkingHours == null || WorkingHours < 0)
            {
                yield return new ValidationResult("运转（h）不能为空或小于0", new string[] { nameof(WorkingHours) });
            }
            if (ConstructionDays == null | ConstructionDays < 0)
            {
                yield return new ValidationResult("施工天数不能为空或小于0", new string[] { nameof(ConstructionDays) });
            }
            if (Production == null || Production < 0)
            {
                yield return new ValidationResult("产量（方）不能为空或小于0", new string[] { nameof(Production) });
            }
            if (ProductionAmount == null | ProductionAmount < 0)
            {
                yield return new ValidationResult("产值（元）不能为空或小于0", new string[] { nameof(ProductionAmount) });
            }
            if (ContractDetailType == null)
            {
                yield return new ValidationResult("合同清单类型不能为空", new string[] { nameof(ContractDetailType) });
            }
            if (DynamicDescriptionType == null)
            {
                yield return new ValidationResult("当前动态描述不能为空", new string[] { nameof(DynamicDescriptionType) });
            }
            if (NextMonthPlanProduction == null || NextMonthPlanProduction < 0)
            {
                yield return new ValidationResult("下月计划工程量（万方）不能为空或小于0", new string[] { nameof(NextMonthPlanProduction) });
            }
            if (NextMonthPlanProductionAmount == null || NextMonthPlanProductionAmount < 0)
            {
                yield return new ValidationResult("下月计划产值（万元）不能为空或小于0", new string[] { nameof(NextMonthPlanProductionAmount) });
            }
        }
    }
}
