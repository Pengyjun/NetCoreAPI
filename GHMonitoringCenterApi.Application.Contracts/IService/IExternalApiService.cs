using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService
{
    /// <summary>
    /// 对外接口
    /// </summary>
    public interface IExternalApiService
    {
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync();
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync();
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync();
        /// <summary>
        /// 获取项目干系人
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync();
        /// <summary>
        /// 项目状态
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectStatusInfos>>> GetProjectStatusInfosAsync();
    }
}
