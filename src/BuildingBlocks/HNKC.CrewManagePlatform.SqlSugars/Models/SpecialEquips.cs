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
        /// 扫描件 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "扫描件")]
        public string? SpecialEquipsScans { get; set; }
    }
}
