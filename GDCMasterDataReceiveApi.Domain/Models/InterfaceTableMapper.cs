using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{

    /// <summary>
    /// 接口和表名映射关系
    /// </summary>
    [SugarTable("t_interfacetablemapper", IsDisabledDelete = true)]
    public class InterfaceTableMapper
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? InterfaceName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? TableName { get; set; }
        /// <summary>
        /// 是否删除  0删除  1有效
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public int IsDelete { get; set; }
    }
}
