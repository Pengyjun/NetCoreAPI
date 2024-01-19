using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 帮助中心详情
    /// </summary>
    [SugarTable("t_helpcenterdetails", IsDisabledDelete = true)]
    public class HelpCenterDetails : BaseEntity<Guid>
    {
        /// <summary>
        /// 帮助中心菜单Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid HelpCenterMenuId { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? Details { get; set; }
        /// <summary>
        /// 是否重用 默认true  true 否  false 是
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool Reutilizando { get; set; }
        /// <summary>
        /// 判断帮助中心与手册  0 为帮助中心   1 为帮助手册  默认为0
        /// </summary>
        public int Determine { get; set; } = 0;


    }
}
