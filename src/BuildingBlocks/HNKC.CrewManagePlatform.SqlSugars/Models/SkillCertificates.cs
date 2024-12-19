using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 技能证书
    /// </summary>
    [SugarTable("t_skillcertificates", IsDisabledDelete = true, TableDescription = "技能证书")]
    public class SkillCertificates : BaseEntity<long>
    {
        /// <summary>
        /// 证书类型
        /// </summary>
        [SugarColumn(Length = 5, ColumnDescription = "证书类型")]
        public CertificateTypeEnum SkillCertificateType { get; set; }
        /// <summary>
        /// 扫描件 ，拼接
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "扫描件")]
        public string? SkillScans { get; set; }
    }
}
