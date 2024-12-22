using HNKC.CrewManagePlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 保存请求体
    /// </summary>
    public class CrewArchivesRequest : BaseRequest
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public BaseInfoDto? BaseInfoDto { get; set; }
        /// <summary>
        /// 适任及证书
        /// </summary>
        public CertificateOfCompetencyDto? CertificateOfCompetencyDto { get; set; }
        /// <summary>
        /// 学历信息
        /// </summary>
        public EducationalBackgroundDto? EducationalBackgroundDto { get; set; }
        /// <summary>
        /// 职务晋升
        /// </summary>
        public PromotionDto? PromotionDto { get; set; }
        /// <summary>
        /// 任职船舶
        /// </summary>
        public WorkShipDto? WorkShipDto { get; set; }
        /// <summary>
        /// 培训记录
        /// </summary>
        public TrainingRecordDto? TrainingRecordDto { get; set; }
        /// <summary>
        /// 年度考核
        /// </summary>
        public YearCheckDto? YearCheckDto { get; set; }
    }
    /// <summary>
    /// 保存备注请求体
    /// </summary>
    public class NotesRequest : BaseRequest, IValidatableObject
    {
        /// <summary>
        /// 1新增 2修改
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public SaveNotes? SaveNotes { get; set; }
        /// <summary>
        /// 校验bid
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BId == Guid.Empty || string.IsNullOrWhiteSpace(BId.ToString()))
            {
                yield return new ValidationResult("业务主键不能为空", new string[] { nameof(BId) });
            }
        }
    }
    /// <summary>
    /// 备注
    /// </summary>
    public class SaveNotes
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
    }
    /// <summary>
    /// 切换用户状态
    /// </summary>
    public class ToggleUserStatus : BaseRequest, IValidatableObject
    {
        /// <summary>
        /// 1删除 2恢复
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        public CrewStatusEnum? DeactivateStatus { get; set; }
        /// <summary>
        /// 如果是删除  不可选在岗 调休  待岗状态
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == 1)
            {
                if (DeactivateStatus == CrewStatusEnum.Normal || DeactivateStatus == CrewStatusEnum.XiuJia || DeactivateStatus == CrewStatusEnum.DaiGang)
                {
                    yield return new ValidationResult("不可选的删除原因", new string[] { nameof(DeactivateStatus) });
                }
            }
        }
    }
}
