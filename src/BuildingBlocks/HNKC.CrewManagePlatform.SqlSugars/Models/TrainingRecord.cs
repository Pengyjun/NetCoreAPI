using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 培训记录
    /// </summary>
    [SugarTable("t_trainingrecord", IsDisabledDelete = true, TableDescription = "培训记录")]
    public class TrainingRecord : BaseEntity<long>
    {
        /// <summary>
        /// 培训类型：安全培训...
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "联系方式")]
        public string? TrainingType { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid TrainingId { get; set; }
        /// <summary>
        /// 培训日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "培训日期")]
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 培训文件
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "培训文件")]
        public Guid? TrainingScan { get; set; }
    }
}
