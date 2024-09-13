using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 值域表
    /// </summary>
    [SugarTable("t_valuedomain", IsDisabledDelete = true)]
    public class ValueDomain:BaseEntity<long>
    {
        /// <summary>
        /// 值域编码
        /// </summary>
        [SugarColumn(Length =64)]
        public string? ZDOM_CODE { get; set; }


        /// <summary>
        /// 值域编码描述
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_DESC { get; set; }


        /// <summary>
        /// 域值 
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_VALUE { get; set; }


        /// <summary>
        /// 域值描述 
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 域值层级 
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_LEVEL { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZREMARKS { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZCHTIME { get; set; }


        /// <summary>
        /// 版本
        /// </summary>
        [SugarColumn(Length = 16)]
        public string? ZVERSION { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(Length = 8)]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
       [SugarColumn(Length =16)]
        public string? ZDELETE { get; set; }
    }
}
