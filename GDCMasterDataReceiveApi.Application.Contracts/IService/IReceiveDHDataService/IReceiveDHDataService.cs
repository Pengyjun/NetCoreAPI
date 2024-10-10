using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveDHDataService
{
    /// <summary>
    /// 接收DH相关数据接口
    /// </summary>
    [DependencyInjection]
    public interface IReceiveDHDataService
    {
        /// <summary>
        /// Dh机构数据写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveOrganzationAsync();
    }
}
