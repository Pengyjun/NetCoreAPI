using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
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
        public async Task<IActionResult> ResetPwdAsync([FromBody] ResetPwdResquest ResetPwdResquest)
        {
            var data= await userManagerService.ResetPwdAsync(ResetPwdResquest);
            return Ok(data);
        }

        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUser")]
        public async Task<IActionResult> SearchUserAsync([FromQuery]PageRequest pageRequest)
        {
            var data = await userManagerService.SearchUserAsync(pageRequest);
            return Ok(data);
        }



        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("RemoveUser")]
        public async Task<IActionResult> RemoveUserAsync([FromQuery] BaseRequest baseRequest)
        {
            var data = await userManagerService.RemoveUserAsync(baseRequest);
            return Ok(data);
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest  addUserRequest)
        {
            var data = await userManagerService.AddUserAsync(addUserRequest);
            return Ok(data);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("ModifyUser")]
        public async Task<IActionResult> ModifyUserAsync([FromBody] ModifyUserResquest  modifyUserResquest)
        {
            var data = await userManagerService.ModifyUserAsync(modifyUserResquest);
            return Ok(data);
        }
    }
}
