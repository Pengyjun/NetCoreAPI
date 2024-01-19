using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 帮助中心菜单
    /// </summary>
    [SugarTable("t_helpcentermenu", IsDisabledDelete = true)]
    public class HelpCenterMenu : BaseEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        /// <summary>
        /// 子id
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int MId { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int PId { get; set; }
       
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Sort { get; set; }
        /// <summary>
        /// 判断帮助中心与手册  0 为帮助中心   1 为帮助手册  默认为0  
        public int Determine { get; set; }
    }
}
