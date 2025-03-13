using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 年休计划假期表
    /// </summary>
    [SugarTable("t_leaveplanuservacation", IsDisabledDelete = true, TableDescription = "年休计划假期表")]
    public class LeavePlanUserVacation : BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid UserId { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month { get; set; }
        /// <summary>
        /// 上半月或下半月  1：上半月  2：下半月
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int VacationHalfMonth { get; set; }
        /// <summary>
        /// 0.5个月 单位
        /// </summary>
        [SugarColumn(ColumnDataType = "double")]
        public double VacationMonth { get; set; }
    }
}
