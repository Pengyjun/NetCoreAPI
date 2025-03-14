using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 年度计划基础填报信息
    /// </summary>
    [SugarTable("t_leaveplansubmitinfo", IsDisabledDelete = true, TableDescription = "年度计划基础填报信息")]
    public class LeavePlanSubmitInfo : BaseEntity<long>
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
        /// <summary>
        /// 是否已填报
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsSubmit { get; set; }
        /// <summary>
        /// 填报用户
        /// </summary>
        [SugarColumn(Length = 50)]
        public string SubUser { get; set; }
    }
}
