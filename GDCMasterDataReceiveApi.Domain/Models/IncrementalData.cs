using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 增量数据统计
    /// </summary>
    [SugarTable("t_incrementaldata", IsDisabledDelete = true)]
    public class IncrementalData : BaseEntity<long>
    {
        /// <summary>
        /// 新增数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int InsertNums { get; set; }
        /// <summary>
        /// 修改数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int UpdateNums { get; set; }
        /// <summary>
        /// 推送天
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
        /// <summary>
        /// 推送月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }
        /// <summary>
        /// 新增/修改的表名
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? TableName { get; set; }
    }
    /// <summary>
    /// 增量数据统计明细ids
    /// </summary>
    [SugarTable("t_incrementaldetailsdata", IsDisabledDelete = true)]
    public class IncrementalDetailsData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int DateDay { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int DateMonth { get; set; }
        /// <summary>
        /// 关联表id
        /// </summary>
        public long RelationTableId {  get; set; }
        /// <summary>
        /// 主键id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }
        /// <summary>
        /// 新增/修改的表名
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? TableName { get; set; }
    }
}
