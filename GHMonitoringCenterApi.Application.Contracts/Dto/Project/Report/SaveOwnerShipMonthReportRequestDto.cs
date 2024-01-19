using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{

    /// <summary>
    /// 保存一条自有船舶月报
    /// </summary>
    public class SaveOwnerShipMonthReportRequestDto : OwnerShipMonthReportDto<SaveOwnerShipMonthReportRequestDto.ReqOwnerShipMonthReportSoil>, IValidatableObject
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
            if(EnterTime!=null&& QuitTime!=null&& QuitTime<EnterTime )
            {
                yield return new ValidationResult("退场时间不能小于进场时间", new string[] { nameof(QuitTime) });
            }
            if (ConstructionDays == null || ConstructionDays < 0)
            {
                yield return new ValidationResult("施工天数不能为空或小于0", new string[] { nameof(ConstructionDays) });
            }
            if(ContractDetailType==null)
            {
                yield return new ValidationResult("合同清单不能为空", new string[] { nameof(ContractDetailType) });
            }
            //if (WorkingHours == null || WorkingHours < 0)
            //{
            //    yield return new ValidationResult("运转（h）不能为空或小于0", new string[] { nameof(QuitTime) });
            //}
            //if (Production == null || Production < 0)
            //{
            //    yield return new ValidationResult("产量（方）不能为空或小于0", new string[] { nameof(Production) });
            //}
            //if (ProductionAmount == null || ProductionAmount < 0)
            //{
            //    yield return new ValidationResult("产值（元）不能为空或小于0", new string[] { nameof(ProductionAmount) });
            //}
            if (WorkModeId == null || WorkModeId == Guid.Empty)
            {
                yield return new ValidationResult("工艺方式Id不能为空", new string[] { nameof(WorkModeId) });
            }
            if (WorkTypeId == null || WorkTypeId == Guid.Empty)
            {
                yield return new ValidationResult("疏浚吹填分类Id不能为空", new string[] { nameof(WorkTypeId) });
            }
            if (ConditionGradeId == null || ConditionGradeId == Guid.Empty)
            {
                yield return new ValidationResult("工况级别Id不能为空", new string[] { nameof(ConditionGradeId) });
            }
            if (DigDeep == null || DigDeep < 0)
            {
                yield return new ValidationResult("挖深（m）不能为空或小于0", new string[] { nameof(DigDeep) });
            }
            if (BlowingDistance == null || BlowingDistance < 0)
            {
                yield return new ValidationResult("吹距(KM)不能为空或小于0", new string[] { nameof(BlowingDistance) });
            }
            if (HaulDistance == null || HaulDistance < 0)
            {
                yield return new ValidationResult("运距(KM)不能为空或小于0", new string[] { nameof(HaulDistance) });
            }
            if (Soils == null || !Soils.Any())
            {
                yield return new ValidationResult("主要施工土质不能为空或小于0", new string[] { nameof(Soils) });
            }
            else
            {
                if (Soils.Sum(t=>t.Proportion)!=100m)
                {
                    yield return new ValidationResult("主要施工土质所占比重不等于100", new string[] { nameof(Soils) });
                }
            }
        }
       
        /// <summary>
        /// 主要施工土质
        /// </summary>
        public class ReqOwnerShipMonthReportSoil : OwnerShipMonthReportSoilDto, IValidatableObject
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (SoilId == Guid.Empty)
                {
                    yield return new ValidationResult("疏浚土分类Id不能为空", new string[] { nameof(SoilId) });
                }
                if (SoilGradeId == Guid.Empty)
                {
                    yield return new ValidationResult("疏浚土分级Id不能为空", new string[] { nameof(SoilGradeId) });
                }
                if (Proportion <= 0)
                {
                    yield return new ValidationResult("所占比重不能小于等于0", new string[] { nameof(Proportion) });
                }
            }
        }
    }

   
}
