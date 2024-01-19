using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶资源扩展表
    /// </summary>
    [SugarTable("t_shipresources", IsDisabledDelete = true)]
    public class ShipResources:BaseEntity<Guid>
    {
        /// <summary>
        /// 资源名称Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? DictionaryTableNameId { get; set; }
        /// <summary>
        /// 船舶Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipPingId { get; set; }
        /// <summary>
        /// 往来单位Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? DealingUnitsId { get; set; }
        /// <summary>
        /// 资源类型  1：自有 2：分包
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ConstructionOutPutType? Type { get; set; }
    }
}
