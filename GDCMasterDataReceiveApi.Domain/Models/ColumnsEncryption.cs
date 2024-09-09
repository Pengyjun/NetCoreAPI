using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 加密列
    /// </summary>
    [SugarTable("t_columnsencryption", IsDisabledDelete = true)]
    public class ColumnsEncryption : BaseEntity<long>
    {
        /// <summary>
        /// 列名
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Column { get; set; }
        /// <summary>
        /// 密文
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Encrytion { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? Description { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Extend { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Extend2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Extend3 { get; set; }
    }
}
