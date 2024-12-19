using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Menus
{

    /// <summary>
    /// 用户菜单响应
    /// </summary>
    public class UserMenuResponse: BaseResponse
    {
       
        /// <summary>
        /// 
        /// </summary>
        public int Mid { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int Parentid { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 组件ID
        /// </summary>
        public string? ComponentUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }
}
