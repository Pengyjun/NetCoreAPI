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
    /// 角色表
    /// </summary>
    [SugarTable("t_role", IsDisabledDelete = true)]
    public class Role:BaseEntity<long>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Name { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Type { get; set; }
        /// <summary>
        /// 是否是系统管理员 例如admin 是系统管理员   广航局的管理员不属于系统管理员
        /// 系统管理员只有一个  角色类型和广航局的管理员的角色类型一致、
        /// 此字段属于标志作用和业务上没有多大关系
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 是否存在审批的功能  系统管理员不会有审批功能
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsApprove { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType ="int")]
        public int? Sort { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Remark { get; set; }

        /// <summary>
        /// 操作类型  0是全部操作  1只有查询操作   
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int OperationType { get; set; }
    }
}
