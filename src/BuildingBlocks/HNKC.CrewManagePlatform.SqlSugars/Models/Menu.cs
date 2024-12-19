using HNKC.CrewManagePlatform.SqlSugars.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{

    /// <summary>
    /// 菜单表
    /// </summary>
    [SugarTable("t_menu", IsDisabledDelete = true)]
    public class Menu : BaseEntity<long>
    {
        public string? Name { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? MId { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ParentId { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? Url { get; set; }
        /// <summary>
        /// 前端组件URL
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? ComponentUrl{ get; set; }
        /// <summary>
        /// ICON图标
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Remark { get; set; }
    }
}
