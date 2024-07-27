using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 日志记录接口层
    /// </summary>
    [DependencyInjection]
    public interface IOperationRecordService
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logRequestDto"></param>
        /// <returns></returns>
        Task WirteLogAsync(LogInfo logRequestDto);
    }
}
