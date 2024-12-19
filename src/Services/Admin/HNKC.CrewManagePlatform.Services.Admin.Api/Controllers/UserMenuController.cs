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
        [HttpPost("SearchUserMenus")]
        public async Task<IActionResult> SearchUserMenusAsync([FromBody]UserMenuRequest userMenuRequest)
        {
            var data = await menuService.AddMenusAsync(userMenuRequest);
            return new OkMessageObjectResult("添加成功");
        }
    }
}
