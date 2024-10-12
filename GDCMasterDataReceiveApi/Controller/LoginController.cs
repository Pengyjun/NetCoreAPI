using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 登录控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly IBaseService _baseService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseService"></param>
        public LoginController(IBaseService baseService)
        {
            this._baseService = baseService;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetUserLoginInfo")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> GetUserLoginInfoAsync([FromBody] LoginDto requestDto)
        {
            return await _baseService.GetUserLoginInfoAsync(requestDto);
        }
        /// <summary>
        /// 设置/修改密码
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("SetPassword")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> SetPasswordAsync([FromBody] LoginDto requestDto)
        {
            return await _baseService.SetPasswordAsync(requestDto);
        }
    }
}
