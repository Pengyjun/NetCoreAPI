using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目产值表
    /// </summary>
    [SugarTable("t_projectoutputvalue",IsDisabledDelete =true)]
    public class ProjectOutPutValue
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(Length =36,IsPrimaryKey = true)]
        public Guid Id { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length =36)]
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 每日计划产值
        /// </summary>
        [SugarColumn(ColumnDataType ="decimal(18,2)")]
        public decimal PlanOutPut { get; set; }
        /// <summary>
        /// 每日完成产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ActualOutPut { get; set; }
        /// <summary>
        /// 插入时间  根据此时间进行筛选
        /// </summary>
        [SugarColumn(ColumnDataType ="datetime")]
        public DateTime SubTime { get; set; }
        /// <summary>
        /// 日期查询
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? DateDay { get; set; }
    }
}
