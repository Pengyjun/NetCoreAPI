using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 用户备注
    /// </summary>
    [SugarTable("t_usernotes", IsDisabledDelete = true, TableDescription = "用户备注")]
    public class UserNotes : BaseEntity<long>
    {
        /// <summary>
        /// 备注信息
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "备注信息")]
        public string? Content { get; set; }
        /// <summary>
        /// 填写备注的用户id
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "备注信息")]
        public string? WritedUserId { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? UserNoteId { get; set; }
    }
}
