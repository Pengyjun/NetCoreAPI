using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 水上设备管理表
    /// </summary>
    [SugarTable("t_marineequipment", IsDisabledDelete = true)]
    public class MarineEquipment : BaseEntity<Guid>
    {
        /// <summary>
        /// 填报月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ReportingMonth { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 分包商名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SubcontractorName { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ContractAmount { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// 航区Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? NavigationAreaId { get; set; }
        /// <summary>
        /// 海上移动识别码（MMSI）
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? MMSI { get; set; }
        /// <summary>
        /// 船舶所有人
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ShipOwner { get; set; }
        /// <summary>
        /// 完工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? CompletionDate { get; set; }
        /// <summary>
        /// 船舶规格尺寸
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ShipSizeSpecifications { get; set; }
        /// <summary>
        /// 规格1
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Specification1 { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Specification2 { get; set; }
        /// <summary>
        /// 船舶定员
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ShipCapacity { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EntryDate { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ExitDate { get; set; }
        /// <summary>
        /// 是否在场  1，是  2，否
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? PresentNot { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? Notes { get; set; }

    }
}
