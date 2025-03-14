using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using UtilsSharp.Shared.Standard;

namespace HNKC.CrewManagePlatform.Services.Interface.Disembark
{
    /// <summary>
    /// 离船
    /// </summary>
    public interface IDisembarkService
    {
        /// <summary>
        /// 查询离船申请列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PageResult<DepartureApplyVo>> DepartureApplyListAsync(DepartureApplyQuery query);

        /// <summary>
        /// 提交离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SubmitDepartureApplyAsync(DepartureApplyDto requestBody);


        /// <summary>
        /// 查询离船申请单
        /// </summary>
        /// <param name="applyCode"></param>
        /// <returns></returns>
        Task<DepartureApplyDetailVo> DepartureApplyDetail(Guid applyCode);

        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SubmitApprove(SubmitApproveRequestDto requestBody);

        /// <summary>
        /// 保存离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveCrewDisembarkAsync(DisembarkRequest requestBody);

        /// <summary>
        /// 离船申请列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchDisembark>> SearchCrewDisembarkAsync(SearchDisembarkRequest requestBody);

        /// <summary>
        /// 船舶用户列表
        /// </summary>
        /// <returns></returns>
        Task<Result> ShipUserListAsync(Guid shipId);

        /// <summary>
        /// 船舶审批用户列表
        /// </summary>
        /// <returns></returns>
        Task<Result> ApproveUserListAsync(Guid shipId);

        /// <summary>
        /// 填报实际离船时间
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> RegisterActualTime(RegisterActualTimeDto requestBody);
    }
}