using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;

namespace HNKC.CrewManagePlatform.Services.Interface.Contract
{
    /// <summary>
    /// 合同接口
    /// </summary>
    public interface IContractService
    {
        /// <summary>
        /// 合同列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<ContractSearch>> SearchContractAsync(ContractRequest requestBody);
        /// <summary>
        /// 续签详情
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> GetContractRenewalAsync(BaseRequest requestBody);
    }
}
