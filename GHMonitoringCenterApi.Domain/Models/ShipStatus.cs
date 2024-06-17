using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶状态
    /// </summary>
    [SugarTable("t_shipstatus", IsDisabledDelete = true)]
    public class ShipStatus : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Sequence { get; set; }
        public string Remarks { get; set; }

    }
}
