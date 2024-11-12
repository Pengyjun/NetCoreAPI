using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// excel历史数据转化中间表
    /// </summary>
    [SugarTable("t_excelconverttable", IsDisabledDelete = true)]
    public class ExcelConvertTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal? Val { get; set; }
        public string? ProjectId { get; set; }
        public int Year { get; set; }
    }
}
