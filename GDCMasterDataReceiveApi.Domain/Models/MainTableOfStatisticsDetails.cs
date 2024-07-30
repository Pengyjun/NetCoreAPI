using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 统计数据库 增改量细表
    /// </summary>
    [SugarTable("t_maintableofstatisticsdetails", IsDisabledDelete = true)]
    public class MainTableOfStatisticsDetails : BaseEntity<long>
    {
        /// <summary>
        /// 增改量主表id
        /// </summary>
        [SugarColumn(ColumnDataType = "number", Length = 19)]
        public long MainTableOfStatisticsId { get; set; }
        /// <summary>
        /// 增改表主键id（如 user表主键id  "增改量主表id 对 userid;1对多"）
        /// </summary>
        [SugarColumn(Length = 100)]
        public string TableId { get; set; }
        /// <summary>
        /// 新增/修改当前表（value：insert/modify）
        /// </summary>
        [SugarColumn(Length = 20)]
        public string InsertOrModify { get; set; }
    }
}
