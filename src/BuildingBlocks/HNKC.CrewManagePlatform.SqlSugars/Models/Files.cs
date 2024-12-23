using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 文件
    /// </summary>
    [SugarTable("t_files", IsDisabledDelete = true, TableDescription = "文件")]
    public class Files : BaseEntity<long>
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "文件名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "原始文件名称")]
        public string? OriginName { get; set; }
        /// <summary>
        /// 文件id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "文件id")]
        public Guid? FileId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "用户id")]
        public Guid? UserId { get; set; }
        /// <summary>
        /// 后缀名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "后缀名称")]
        public string? SuffixName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "文件类型")]
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0", ColumnDescription = "文件大小")]
        public long? FileSize { get; set; }
    }
}
