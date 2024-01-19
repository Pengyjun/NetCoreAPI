using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Enums;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 新增/修改安监日报
    /// </summary>
    public class AddOrUpdateSafeSupervisionDayReportRequestDto : SafeSupervisionDayReportDto, IValidatableObject, IResetModelProperty
    {
        
        /// <summary>
        /// 是否展示疫情相关内容
        /// </summary>
        public bool IsShowEpidemicRelevant { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ( ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
            if(IsShowEpidemicRelevant)
            {
                if (ManagementNumber < 0)
                {
                    yield return new ValidationResult("【今日返回】项目管理人员数量不能小于0", new string[] { nameof(ManagementNumber) });
                }
                if (PersonNumber == null||PersonNumber < 0)
                {
                    yield return new ValidationResult("【今日返回】船舶人员数量不能小于0", new string[] { nameof(PersonNumber) });
                }
                if (WorkerNumber==null||WorkerNumber < 0)
                {
                    yield return new ValidationResult("【今日返回】产业工人数量不能小于0", new string[] { nameof(WorkerNumber) });
                }
                if (FromHighRiskAreasPersonNumber==null||FromHighRiskAreasPersonNumber < 0)
                {
                    yield return new ValidationResult("【今日返回】来自中高风险地区人数不能小于0", new string[] { nameof(FromHighRiskAreasPersonNumber) });
                }
                if (InManagementNumber==null||InManagementNumber < 0)
                {
                    yield return new ValidationResult("【当前在场】项目管理人员数量不能小于0", new string[] { nameof(ManagementNumber) });
                }
                if (InPersonNumber==null||InPersonNumber < 0)
                {
                    yield return new ValidationResult("【当前在场】船舶人员数量不能小于0", new string[] { nameof(InPersonNumber) });
                }
                if (InWorkerNumber==null||InWorkerNumber < 0)
                {
                    yield return new ValidationResult("【当前在场】产业工人数量不能小于0", new string[] { nameof(InWorkerNumber) });
                }
                if (InHighRiskAreasPersonNumber==null||InHighRiskAreasPersonNumber < 0)
                {
                    yield return new ValidationResult("【当前在场】来自中高风险地区人数不能小于0", new string[] { nameof(InHighRiskAreasPersonNumber) });
                }
                if (QuarantineManagementNum==null||QuarantineManagementNum < 0)
                {
                    yield return new ValidationResult("【当前隔离】项目管理人员数量不能小于0", new string[] { nameof(QuarantineManagementNum) });
                }
                if (QuarantinePersonNum==null||QuarantinePersonNum < 0)
                {
                    yield return new ValidationResult("【当前隔离】船舶人员数量不能小于0", new string[] { nameof(QuarantinePersonNum) });
                }
                if (QuarantineWorkerNum==null||QuarantineWorkerNum < 0)
                {
                    yield return new ValidationResult("【当前隔离】产业工人数量不能小于0", new string[] { nameof(QuarantineWorkerNum) });
                }
                if (string.IsNullOrWhiteSpace(QuarantineReason))
                {
                    yield return new ValidationResult("隔离原因不能为空", new string[] { nameof(QuarantineReason) });
                }
                if (DiagnosisNum == null|| DiagnosisNum < 0)
                {
                    yield return new ValidationResult("确诊人数不能小于0", new string[] { nameof(DiagnosisNum) });
                }
                if (SuspectsNum==null||SuspectsNum < 0)
                {
                    yield return new ValidationResult("疑似人数不能小于0", new string[] { nameof(SuspectsNum) });
                }
                if (ShouldManagementNum==null||ShouldManagementNum < 0)
                {
                    yield return new ValidationResult("【应在场】项目管理人员数量不能小于0", new string[] { nameof(ShouldManagementNum) });
                }
                if (ShouldPersonNum==null||ShouldPersonNum < 0)
                {
                    yield return new ValidationResult("【应在场】船舶人员数量不能小于0", new string[] { nameof(ShouldPersonNum) });
                }
                if (ShouldWorkerNum==null||ShouldWorkerNum < 0)
                {
                    yield return new ValidationResult("【应在场】产业工人数量不能小于0", new string[] { nameof(ShouldWorkerNum) });
                }
            }
            if (IsWork && WorkDate == null)
            {
                yield return new ValidationResult("地方政府许可复工日期不能为空", new string[] { nameof(WorkDate) });
            }
            if (WorkStatus==null||!EnumExtension.EnumToList<SafeSupervisionWorkStatus>().Any(t=>t.EnumValue==(int) WorkStatus))
            {
                yield return new ValidationResult("项目复工状态不存在", new string[] { nameof(WorkStatus) });
            }
            if (WorkStatus == SafeSupervisionWorkStatus.UnWorkOfSpringFestival)
            {
                if (string.IsNullOrWhiteSpace(ConstructionContent))
                {
                    yield return new ValidationResult("当天主要施工内容【未停工】", new string[] { nameof(ConstructionContent) });
                }
                if (string.IsNullOrWhiteSpace(ProductionSituation))
                {
                    yield return new ValidationResult("当天安全生产情况【未停工】", new string[] { nameof(ProductionSituation) });
                }
            }
            else if (WorkStatus == SafeSupervisionWorkStatus.UnWorkOfResuming)
            {
                if (PlanWorkDate == null)
                {
                    yield return new ValidationResult("【未开复工】项目现计划复工日期", new string[] { nameof(PlanWorkDate) });
                }
                if (string.IsNullOrWhiteSpace(Reason))
                {
                    yield return new ValidationResult("未开复工原因", new string[] { nameof(Reason) });
                }
                if (string.IsNullOrWhiteSpace(Details))
                {
                    yield return new ValidationResult("项目未开复工原因具体情况", new string[] { nameof(Details) });
                }
            }
            else if (WorkStatus == SafeSupervisionWorkStatus.WorkOfResuming)
            {
                if (ActualWorkDate == null)
                {
                    yield return new ValidationResult("【已复工】项目实际复工日期", new string[] { nameof(ActualWorkDate) });
                }
                if (string.IsNullOrWhiteSpace(ActualConstructionContent))
                {
                    yield return new ValidationResult("当天主要施工内容【已复工】", new string[] { nameof(ActualConstructionContent) });
                }
                if (string.IsNullOrWhiteSpace(ActualSituation))
                {
                    yield return new ValidationResult("当天安全生产情况【已复工】", new string[] { nameof(ActualSituation) });
                }
            }
            if (MaskNum==null||MaskNum < 0)
            {
                yield return new ValidationResult("口罩（个）不能小于0", new string[] { nameof(MaskNum) });
            }
            if (ThermometerNum==null||ThermometerNum < 0)
            {
                yield return new ValidationResult("体温计（个）不能小于0", new string[] { nameof(ThermometerNum) });
            }
            if (DisinfectantNum==null||DisinfectantNum < 0)
            {
                yield return new ValidationResult("消毒液（升）不能小于0", new string[] { nameof(DisinfectantNum) });
            }
            if (string.IsNullOrWhiteSpace(Measures))
            {
                yield return new ValidationResult("存储和消防安全措施不能为空", new string[] { nameof(Measures) });
            }
            if (Situation <= 0 && Situation > SafeMonitoringSituation.Accident)
            {
                yield return new ValidationResult("安全生产情况不存在", new string[] { nameof(WorkStatus) });
            }
            if (IsSuperiorSupervision)
            {
                if (SuperiorSupervisionCount == null || SuperiorSupervisionCount < 0)
                {
                    yield return new ValidationResult("上级督查次数不能小于0", new string[] { nameof(SuperiorSupervisionCount) });
                }
                if (!EnumExtension.EnumToList<SuperiorSupervisionForm>().Any(t => t.EnumValue == (int)SuperiorSupervisionForm))
                {
                    yield return new ValidationResult("上级督查形式不存在", new string[] { nameof(SuperiorSupervisionForm) });
                }
                if (SuperiorSupervisionDate == null)
                {
                    yield return new ValidationResult("上级督查时间不能为空", new string[] { nameof(SuperiorSupervisionDate) });
                }
                if (string.IsNullOrWhiteSpace(SupervisionUnit))
                {
                    yield return new ValidationResult("督查单位不能为空", new string[] { nameof(SupervisionUnit) });
                }
                if (string.IsNullOrWhiteSpace(SupervisionLeader))
                {
                    yield return new ValidationResult("上级督查领导不能为空", new string[] { nameof(SupervisionLeader) });
                }
                if (string.IsNullOrWhiteSpace(SupervisionOther))
                {
                    yield return new ValidationResult("上级督查其他人员不能为空", new string[] { nameof(SupervisionOther) });
                }
            }
        }

        /// <summary>
        /// 重置Model属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (WorkStatus != SafeSupervisionWorkStatus.UnWorkOfSpringFestival)
            {
                ConstructionContent = null;
                ProductionSituation = null;
            }
            if (WorkStatus != SafeSupervisionWorkStatus.UnWorkOfResuming)
            {
                PlanWorkDate = null;
                Reason = null;
                Details = null;
            }
            if (WorkStatus != SafeSupervisionWorkStatus.WorkOfResuming)
            {
                ActualWorkDate = null;
                ActualConstructionContent = null;
                ActualSituation = null;
            }
            if (!IsSuperiorSupervision)
            {
                SuperiorSupervisionCount = null;
                SuperiorSupervisionForm = 0;
                SuperiorSupervisionDate = null;
                SupervisionUnit = null;
                SupervisionLeader = null;
                SupervisionOther = null;
            }
            if(!IsShowEpidemicRelevant)
            {
                ManagementNumber = 0;
                PersonNumber = 0;
                WorkerNumber = 0;
                FromHighRiskAreasPersonNumber = 0;
                InManagementNumber = 0;
                InPersonNumber = 0;
                InWorkerNumber = 0;
                InHighRiskAreasPersonNumber = 0;
                QuarantineManagementNum = 0;
                QuarantinePersonNum = 0;
                QuarantineWorkerNum = 0;
                QuarantineReason = "无";
                DiagnosisNum = 0;
                SuspectsNum = 0;
                ShouldManagementNum = 0;
                ShouldPersonNum = 0;
                ShouldWorkerNum = 0;
            }
        }
    }
}
