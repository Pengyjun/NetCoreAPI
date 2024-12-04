using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 重点项目监控配置表
    /// </summary>
    [SugarTable("t_keyproject", IsDisabledDelete = true)]
    public class KeyProject :BaseEntity<Guid>
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? ProjectName { get; set; }
        /// <summary>
        /// 区间值  -+区间
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? Interval { get; set; }

    }
}
