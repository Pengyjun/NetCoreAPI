using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备分类属性值表
    /// </summary>
    [SugarTable("t_deviceclassattributevalue", IsDisabledDelete = true)]
    public class DeviceClassAttributeValue : BaseEntity<long>
    {
        /// <summary>
        /// 分类编码: 分类的唯一性编码
        /// 11
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "ClassCode")]
        public string ZCLASS { get; set; }
        /// <summary>
        /// 属性编码: 属性唯一代码
        /// 10 
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Code")]
        public string ZATTRCODE { get; set; }
        /// <summary>
        /// 属性值编码: 属性值唯一代码
        /// 10
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "ValueCode")]
        public string ZVALUECODE { get; set; }
        /// <summary>
        /// 属性值名称
        /// 100
        /// </summary>
        [SugarColumn(Length = 512, ColumnName = "Name")]
        public string ZVALUENAME { get; set; }
    }
}
