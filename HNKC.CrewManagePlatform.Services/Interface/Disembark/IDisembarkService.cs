﻿using HNKC.CrewManagePlatform.Models.CommonResult;
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
        /// 船舶排班
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveCrewRotaAsync(SaveSchedulingRequest requestBody);
        /// <summary>
        /// 船员船舶排班回显
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> CrewRotaListAsync(SchedulingRequest requestBody);
        /// <summary>
        /// 船舶排班用户列表
        /// </summary>
        /// <returns></returns>
        Task<Result> CrewRotaUserListAsync(Guid shipId);
        /// <summary>
        /// 值班管理列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchCrewRota>> SearchCrewRotaAsync(SearchCrewRotaRequest requestBody);

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
    }
}
