using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService
{
    /// <summary>
    /// 列表接口
    /// </summary>
    [DependencyInjection]
    public interface ISearchService
    {
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="louDongDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync(LouDongRequestDto louDongDto);
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UserSearchResponseDto>>> GetUserSearchAsync(UserSearchRequestDto requestDto);
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<UserSearchDetailsDto>> GetUserDetailsAsync(string uId);
        /// <summary>
        /// 机构树
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstitutionDto>>> GetInstitutionAsync(InstitutionRequestDto requestDto);
        /// <summary>
        /// 机构详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<InstitutionDetatilsDto>> GetInstitutionDetailsAsync(string Id);
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectSearchDto>>> GetProjectSearchAsync(ProjectRequestDto requestDto);
        /// <summary>
        /// 项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectDetailsDto>> GetProjectDetailsAsync(string id);
    }
}
