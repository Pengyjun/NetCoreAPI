using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 元数据关联表
    /// </summary>
    [SugarTable("t_metadatarelation", IsDisabledDelete = true)]
    public class MetaDataRelation : BaseEntity<long>
    {
        /// <summary>
        /// 列名
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ColumnsName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TableName { get; set; }
        /// <summary>
        /// 原列名数据长度
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ColumnsLength { get; set; }
    }
}
