using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 特种设备证书
    /// </summary>
    [SugarTable("t_specialequips", IsDisabledDelete = true, TableDescription = "特种设备证书")]
    public class SpecialEquips : BaseEntity<long>
    {
        /// <summary>
        /// 证书类型
        /// </summary>
        [SugarColumn(Length = 5, ColumnDescription = "证书类型")]
        public CertificateTypeEnum SpecialEquipsCertificateType { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "有效日期")]
        public DateTime? SpecialEquipsEffectiveTime { get; set; }
        /// <summary>
        /// 年审日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "年审日期")]
        public DateTime? AnnualReviewTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "扫描件")]
        public Guid? SpecialEquipsScans { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid SpecialEquipId { get; set; }
    }
}
