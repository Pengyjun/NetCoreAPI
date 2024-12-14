using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.UserManager;
using HNKC.CrewManagePlatform.Services.Admin.Api.Filters;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : BaseController
    {
        public IUserManagerService  userManagerService { get; set; }

        public UserManagerController(IUserManagerService userManagerService)
        {
            this.userManagerService = userManagerService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("UserLogin")]
        [AllowAnonymous]
        public async Task<Result> UserLoginAsync([FromBody] UserLoginRequest userLogin) 
        {
            return await userManagerService.UserLoginAsync(userLogin);
           
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("ResetPwd")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPwdAsync([FromBody] ResetPwdResquest ResetPwdResquest)
        {
            var data= await userManagerService.ResetPwdAsync(ResetPwdResquest);
            return Ok(data);
        }
    }
}
