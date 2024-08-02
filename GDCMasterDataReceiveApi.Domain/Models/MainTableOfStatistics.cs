using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 统计数据库增改量主表
    /// </summary>
    [SugarTable("t_maintableofstatistics", IsDisabledDelete = true)]
    public class MainTableOfStatistics : BaseEntity<long>
    {
        /// <summary>
        /// 更新/新增的表
        /// </summary>
        [SugarColumn(Length = 100)]
        public string TableName { get; set; }
        /// <summary>
        /// 当日表增量数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int InsertNums { get; set; }
        /// <summary>
        /// 当日表修改数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ModifyNums { get; set; }
        /// <summary>
        /// 新增之前的总数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int BeforeInsertNums { get; set; }
        /// <summary>
        /// 修改之前的总数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int BeforeModifyNums { get; set; }
        /// <summary>
        /// 表更新日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
        /// <summary>
        /// 表更新时间 (某天某时新增/更新数据)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int HourOfTheDay { get; set; }
    }
}
