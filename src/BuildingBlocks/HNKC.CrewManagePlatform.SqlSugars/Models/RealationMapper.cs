using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 关系映射表
    /// </summary>
    [SugarTable("t_realationmapper", IsDisabledDelete = true, TableDescription = "关系映射表")]
    public class RealationMapper : BaseEntity<long>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(Length = 19, ColumnDescription = "证书主键")]
        public long PrimaryKey { get; set; }
        /// <summary>
        /// 外键表
        /// </summary>
        [SugarColumn(Length = 19, ColumnDescription = "关联外键")]
        public long ForeignKey { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(Length = 19, ColumnDescription = "用户id")]
        public long UserId { get; set; }
        /// <summary>
        /// 1.证书类型2.学历3.
        /// </summary>
        [SugarColumn(Length = 8, ColumnDescription = "映射类型：1.证书类型 2.学历 3.")]
        public int Type { get; set; }
    }
}
