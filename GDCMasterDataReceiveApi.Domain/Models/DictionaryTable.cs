using GDCDataSecurityApi.Domain;
using GDCMasterDataReceiveApi.Domain;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCDataSecurityApi.Domain.Models
{
    /// <summary>
    /// 字典表
    /// </summary>
    [SugarTable("t_dictionarytable", IsDisabledDelete = true)]
    public class DictionaryTable:BaseEntity<long>
    {

        /// <summary>
        /// 类型编号
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? TypeNo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(Length =32)]
        public string? Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Name { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Remark { get; set; }
    }
}
