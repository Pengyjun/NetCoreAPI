using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备属性值列表
    /// </summary>
    [SugarTable("t_deviceattributevalue", IsDisabledDelete = true)]
    public class DeviceAttributeValue : BaseEntity<long>
    {
        /// <summary>
        /// 分类编码: 分类的唯一性编码
        /// 11
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "ClassCode")]
        public string ZCLASS { get; set; }
        /// <summary>
        /// 属性编码: 属性唯一代码
        /// 10 
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "Code")]
        public string ZATTRCODE { get; set; }
        /// <summary>
        /// 属性值编码: 属性值唯一代码
        /// 10
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "ValueCode")]
        public string ZVALUECODE { get; set; }
        /// <summary>
        /// 属性值名称
        /// 100
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "Name")]
        public string ZVALUENAME { get; set; }
    }
}
