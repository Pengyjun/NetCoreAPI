using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备分类属性表
    /// </summary>
    [SugarTable("t_deviceclassattribute", IsDisabledDelete = true)]
    public class DeviceClassAttribute : BaseEntity<long>
    {
        /// <summary>
        /// 11
        /// 分类编码:分类的唯一性编码
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "ClassCode")]
        public string? ZCLASS { get; set; }
        /// <summary>
        /// 属性编码:属性唯一代码
        /// 10
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Code")]
        public string? ZATTRCODE { get; set; }
        /// <summary>
        /// 属性名称
        /// 100
        /// </summary>
        [SugarColumn(Length = 512, ColumnName = "Name")]
        public string? ZATTRNAME { get; set; }
        /// <summary>
        /// 属性计量单位
        /// 50
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Unit")]
        public string? ZATTRUNIT { get; set; }
        /// <summary>
        /// 备注
        /// 300
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Remark")]
        public string? ZREMARK { get; set; }
    }
}
