using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 职务晋升
    /// </summary>
    [SugarTable("t_promotion", IsDisabledDelete = true, TableDescription = "职务晋升")]
    public class Promotion : BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "用户id")]
        public Guid UserId { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "所在船舶")]
        public string? OnShip { get; set; }
        /// <summary>
        /// 调动任职
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "调动任职")]
        public string? Postition { get; set; }
        /// <summary>
        /// 调动时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "调动时间")]
        public DateTime? PromotionTime { get; set; }
        /// <summary>
        /// 调单文件 ,拼接
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "调单文件")]
        public string? PromotionScan { get; set; }

    }
}
