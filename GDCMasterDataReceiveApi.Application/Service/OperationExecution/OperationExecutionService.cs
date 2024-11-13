using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.OperationExecution;
using GDCMasterDataReceiveApi.Domain.Shared;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Service.OperationExecution
{
    /// <summary>
    /// 增删改实现
    /// </summary>
    public class OperationExecutionService : IOperationExecutionService
    {
        private readonly ISqlSugarClient _dbContext;
        private static bool success = false;
        /// <summary>
        /// 构造函数，注入数据库上下文
        /// </summary>
        /// <param name="dbContext"></param>
        public OperationExecutionService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 增改用户信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InsertOrUpdateUserAsync(OperationExecutionRequestDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            if (requestDto.EntityJson != null)
            {

            }
            responseAjaxResult.SuccessResult(success);
            return responseAjaxResult;
        }

        //private async Task<>
    }
}
