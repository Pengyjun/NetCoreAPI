using HNKC.CrewManagePlatform.Models.CommonResult;
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
        /// 合同续签
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveContractAsync(ConntractRenewal requestBody);
        /// <summary>
        /// 职务晋升列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<PromotionSearch>> SearchPromotionAsync(PromotionRequest requestBody);
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SavePromotionAsync(PositionPromotion requestBody);
        /// <summary>
        /// 培训记录
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<TrainingRecordSearch>> SearchTrainingRecordAsync(TrainingRecordRequest requestBody);
        /// <summary>
        /// 保存培训记录
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveTrainingRecordAsync(SaveTrainingRecord requestBody);
        /// <summary>
        /// 年度考核列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<YearCheckSearch>> SearchYearCheckAsync(YearCheckRequest requestBody);
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveYearCheckAsync(SaveYearCheck requestBody);
        /// <summary>
        /// 提醒证书/合同统计数
        /// </summary>
        /// <returns></returns>
        Task<Result> RemindCountAsync();
    }
}
