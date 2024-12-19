using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Menus;
using HNKC.CrewManagePlatform.Services.Menus;
using HNKC.CrewManagePlatform.Util;
using HNKC.CrewManagePlatform.Web.ActionResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserMenuController : ControllerBase
    {
        public IMenuService menuService { get; set; }
        public UserMenuController(IMenuService menuService)
        {
            this.menuService=menuService;
        }

        



        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="roltId"></param>
        /// <returns></returns>
        [HttpGet("SearchUserMenus")]
       public   async  Task<IActionResult> SearchUserMenusAsync()
        {
            var data=await menuService.SearchUserMenusAsync();
            return Ok(data);    
        }

        /// <summary>
        /// 添加用户菜单
        /// </summary>
        /// <param name="roltId"></param>
        /// <returns></returns>
        [HttpPost("AddMenus")]
        public async Task<Result> AddMenusAsync([FromBody]UserMenuRequest userMenuRequest)
        {
            return   await menuService.AddMenusAsync(userMenuRequest);
        }



        /// <summary>
        /// 删除用户菜单
        /// </summary>
        /// <param name="roltId"></param>
        /// <returns></returns>
        [HttpPost("RemoveMenus")]
        public async Task<Result> RemoveMenusAsync([FromBody] BaseRequest  baseRequest)
        {
           return await menuService.RemoveMenusAsync(baseRequest);
        }



        /// <summary>
        /// 修改用户菜单
        /// </summary>
        /// <param name="roltId"></param>
        /// <returns></returns>
        [HttpPost("ModifyMenus")]
        public async Task<Result> ModifyMenusAsync([FromBody] UserMenuRequest userMenuRequest)
        {
           return  await menuService.ModifyMenusAsync(userMenuRequest);
        }
    }
}
