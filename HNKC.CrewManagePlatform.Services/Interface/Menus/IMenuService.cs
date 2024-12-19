using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Menus;
using HNKC.CrewManagePlatform.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Menus
{

    /// <summary>
    /// 菜单接口层
    /// </summary>
    public interface IMenuService
    {


        /// <summary>
        /// 搜索用户菜单
        /// </summary>
        /// <param name="oid">所属公司OID</param>
        /// <param name="roltId"></param>
        /// <returns></returns>
        Task<List<UserMenuResponseTree>> SearchUserMenusAsync();




        /// <summary>
        /// 添加用户菜单
        /// </summary>
        /// <param name="oid">所属公司OID</param>
        /// <param name="roltId"></param>
        /// <returns></returns>
        Task<bool> AddMenusAsync(UserMenuRequest userMenuRequest);


    }
}
