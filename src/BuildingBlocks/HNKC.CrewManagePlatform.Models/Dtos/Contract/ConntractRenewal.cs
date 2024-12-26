using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 合同续签
    /// </summary>
    public class ConntractRenewal : BaseRequest
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
        public DateTime EntryTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
