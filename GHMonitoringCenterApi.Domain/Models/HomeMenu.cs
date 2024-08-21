using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 菜单时间表
    /// </summary>
    [SugarTable("t_homemenu", IsDisabledDelete = true)]
    public class HomeMenu : BaseEntity<Guid>
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? MenuName { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Column { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }


        /// <summary>
        /// 调整URL
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Url { get; set; }
        /// <summary>
        /// 结构类型monthreport 月报
        /// </summary>
        [SugarColumn(ColumnDataType = "bit", Length = 1)]
        public bool? Display { get; set; }
    }
}
