using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 用户信息接口层
    /// </summary>
    [DependencyInjection]
    public interface ICurrentUser
    {
        /// <summary>
        /// 验证用户信息并返回全局用户对象
        /// </summary>
        /// <returns></returns>
        Task<GlobalCurrentUser> UserAuthenticatedAsync();

    }
}
