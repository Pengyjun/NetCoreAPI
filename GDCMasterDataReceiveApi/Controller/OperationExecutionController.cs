using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.OperationExecution;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 增删改控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OperationExecutionController : BaseController
    {
        private readonly IOperationExecutionService _operate;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="operate"></param>
        public OperationExecutionController(IOperationExecutionService operate)
        {
            this._operate = operate;
        }
        /// <summary>
        /// 增改往来单位信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdateUser")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> InsertOrUpdateUserAsync([FromBody] OperationExecutionRequestDto requestDto)
        {
            return await _operate.InsertOrUpdateAsync(requestDto);
        }
    }
}
