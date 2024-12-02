using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 表数据下发配置
    /// </summary>
    [SugarTable("t_tablesissuedsettings", IsDisabledDelete = true)]
    public class TablesIssuedSettings : BaseEntity<long>
    {
        /// <summary>
        /// 表(目前涉及值域表) 1
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public TableEType Table { get; set; }
        /// <summary>
        /// 下发单位
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Oid { get; set; }
        /// <summary>
        /// 需要查询类型  ,拼接
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? Type { get; set; }
        /// <summary>
        /// 下发数据筛选类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public IssuedEType IssuedType { get; set; }
    }
}
