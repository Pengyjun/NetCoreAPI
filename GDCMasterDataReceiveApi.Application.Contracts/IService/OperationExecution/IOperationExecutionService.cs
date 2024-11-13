using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.OperationExecution
{
    /// <summary>
    /// 增删改接口层
    /// </summary>
    [DependencyInjection]
    public interface IOperationExecutionService
    {
        /// <summary>
        /// 增改用户信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveDataAsync(OperationExecutionRequestDto requestDto);
    }
}
