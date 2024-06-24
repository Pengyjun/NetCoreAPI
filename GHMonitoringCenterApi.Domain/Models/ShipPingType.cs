using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶类型
    /// </summary>
    [SugarTable("t_shippingtype", IsDisabledDelete = true)]
    public class ShipPingType : BaseEntity<Guid>
    {
        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 36)]
        public string? Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Code { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Remarks { get; set; }
    }
}
