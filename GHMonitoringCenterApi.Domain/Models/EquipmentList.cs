using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 设备表
    /// </summary>
    [SugarTable("t_equipmentlist", IsDisabledDelete = true)]
    public class EquipmentList : BaseEntity<Guid>
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? DeviceName { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Oid { get; set; }
        /// <summary>
        /// 子级
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Poid { get; set; }
    }
}
