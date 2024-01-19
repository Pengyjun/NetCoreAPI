using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 资源管理临时表
    /// </summary>
    [SugarTable("t_qualitytemporary", IsDisabledDelete = true)]
    public class QualityTemporary
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 36)]
        public string Name { get; set; }
        /// <summary>
        /// 项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ItemQuantity { get; set; }
        /// <summary>
        /// 未填报
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int NotFillNums { get; set; }
        /// <summary>
        /// 填报率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(10,2)")]
        public decimal FillProp { get; set; }
    }
}
