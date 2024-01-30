using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// excel 产运营监控日报  title
    /// </summary>
    [SugarTable("t_exceltitle", IsDisabledDelete = true)]
    public class ExcelTitle : BaseEntity<Guid>
    {
        /// <summary>
        /// 标题 一：项目总体生产情况 二：自由船舶施工运转情况 三：特殊情况 四：各单位填报情况 五：元旦期间各在建项目带班生产动态  (固定值)
        /// </summary>
        [SugarColumn(Length = 100)]
        public string TitleName { get; set; }
        /// <summary>
        /// 1：第一张表 title 2：第二张表title 依次类推
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Type { get; set; }
        /// <summary>
        /// 内容 
        /// </summary>
        [SugarColumn(Length = 500)]
        public string TtileContent { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month { get; set; }
        /// <summary>
        /// 天
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
    }
}
