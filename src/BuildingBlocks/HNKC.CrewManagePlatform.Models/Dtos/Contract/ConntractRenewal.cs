using HNKC.CrewManagePlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 合同续签
    /// </summary>
    public class ConntractRenewal : BaseRequest, IValidatableObject
    {
        /// <summary>
        /// 主键id  用来做增改判断
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentType { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public ContractEnum ContractType { get; set; }
        /// <summary>
        /// 合同主体
        /// </summary>
        public string? ContractMain { get; set; }
        /// <summary>
        /// 劳务公司
        /// </summary>
        public string? LaborCompany { get; set; }
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(ContractMain) && ContractMain.Length > 20) yield return new ValidationResult("合同主体过长", new string[] { nameof(ContractMain) });
            if (!string.IsNullOrWhiteSpace(LaborCompany) && LaborCompany.Length > 20) yield return new ValidationResult("劳务公司过长", new string[] { nameof(LaborCompany) });
        }
    }
}
