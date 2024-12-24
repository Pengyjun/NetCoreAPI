using HNKC.CrewManagePlatform.Models.CommonRequest;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 列表请求
    /// </summary>
    public class ContractRequest : PageRequest
    {
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentType { get; set; }
    }
}
