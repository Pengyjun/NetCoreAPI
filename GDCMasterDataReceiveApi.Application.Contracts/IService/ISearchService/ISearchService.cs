using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
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
    }
}
