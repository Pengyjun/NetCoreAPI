using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 年度考核
    /// </summary>
    [SugarTable("t_yearcheck", IsDisabledDelete = true, TableDescription = "年度考核")]
    public class YearCheck : BaseEntity<long>
    {
        /// <summary>
        /// 考核结果：优秀...
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0", ColumnDescription = "考核结果")]
        public CheckEnum CheckType { get; set; }
        /// <summary>
        /// 考核模式：2024年度考核
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "考核模式")]
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 考核文件
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "考核文件")]
        public Guid? TrainingScan { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid TrainingId { get; set; }
    }
}
