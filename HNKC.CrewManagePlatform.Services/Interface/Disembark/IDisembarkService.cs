using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 年休假计划列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<PageResult<AnnualLeavePlanResponseDto>> SearchAnnualLeavePlanAsync(AnnualLeavePlanRequestDto requestDto);

        /// <summary>
        /// 年休计划  获取船舶人员信息
        /// </summary>
        /// <param name="ShipId"></param>
        /// <returns></returns>
        Task<Result> SearchLeavePlanUserAsync(SearchLeavePlanUserRequestDto requestDto);
        /// <summary>
        /// 新增或修改年休计划
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<Result> SaveLeavePlanUserVacationAsync(AddLeavePlanVacationRequestDto requestDto);
        /// <summary>
        /// 获取休假日期详情
        /// </summary>
        /// <returns></returns>
        Task<Result> SearchLeaveDetailAsync(SearchLeavePlanUserRequestDto requestDto);

        /// <summary>
        /// 年休假规则验证
        /// </summary>
        /// <returns></returns>
        Task<Result> LeaveCheckRuleAsync(LeaveCheckRuleRequestDto requestDto);

    }
}