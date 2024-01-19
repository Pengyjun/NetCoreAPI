using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 港口数据
    /// </summary>
    [SugarTable("t_portdata", IsDisabledDelete = true)]
    public class PortData:BaseEntity<Guid>
    {
        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? PomId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ShortName { get; set; }
        /// <summary>
        /// 港口编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Code { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", IsNullable = true)]
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", IsNullable = true)]
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Remarks { get; set; }
    }
}
