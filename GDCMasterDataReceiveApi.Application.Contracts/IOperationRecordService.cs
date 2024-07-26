using GDCMasterDataReceiveApi.Domain.Shared;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 日志记录接口层
    /// </summary>

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
