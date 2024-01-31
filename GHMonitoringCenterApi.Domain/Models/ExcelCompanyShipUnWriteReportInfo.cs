using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel 船舶生产数据存在不完整部分主要是项目部未填报以下船舶
    /// </summary>
    [SugarTable("t_excelcompanyshipunwritereportinfo", IsDisabledDelete = true)]
    public class ExcelCompanyShipUnWriteReportInfo : BaseEntity<Guid>
    {  /// <summary>
       /// 船舶名称
       /// </summary>
        [SugarColumn(Length = 20)]
        public string ShipName { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        [SugarColumn(Length = 200)]
        public string OnProjectName { get; set; }

    }
}
