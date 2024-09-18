using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.IRuleService
{
    /// <summary>
    /// 接口规则列表
    /// </summary>
    [DependencyInjection]
    public interface IInterfaceRuleService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InterfaceNamesResponseDto>>> GetInterfaceNamesAsync();
    }
}
