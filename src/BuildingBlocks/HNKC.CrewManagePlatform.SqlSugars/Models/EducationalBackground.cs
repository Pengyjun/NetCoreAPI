using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 学历信息
    /// </summary>
    [SugarTable("t_educationalbackground", IsDisabledDelete = true, TableDescription = "学历信息")]
    public class EducationalBackground : BaseEntity<long>
    {
        /// <summary>
        /// 学历类型：全日制...
        /// </summary>
        [SugarColumn(Length = 5, ColumnDescription = "学历类型")]
        public QualificationTypeEnum QualificationType { get; set; }
        /// <summary>
        /// 学校
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "学校")]
        public string? School { get; set; }
        /// <summary>
        /// 学历：本科...
        /// </summary>
        [SugarColumn(Length = 5, ColumnDescription = "学历")]
        public QualificationEnum Qualification { get; set; }
        /// <summary>
        /// 专业：计算机网络
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "专业")]
        public string? Major { get; set; }
        /// <summary>
        /// 入学日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "入学日期")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 毕业日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "毕业日期")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 证书 ,拼接
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "证书")]
        public string? QualificationScans { get; set; }
    }
}
