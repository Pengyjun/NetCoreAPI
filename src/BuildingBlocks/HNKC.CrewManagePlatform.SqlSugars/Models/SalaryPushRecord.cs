using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 工资推送记录表
    /// </summary>
    [SugarTable("t_salarypushrecord", IsDisabledDelete = true, TableDescription = "工资推送记录表")]
    public class SalaryPushRecord:BaseEntity<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "用户ID")]
        public long UserId { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "年份")]
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "月份")]
        public int Month { get; set; }
        /// <summary>
        /// 1是批量推送 0是个人推送
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "1是批量推送 0是个人推送")]
        public int BusinessType { get; set; }
        /// <summary>
        /// 1是成功  0是失败
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "1是成功  0是失败")]
        public int Result { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        [SugarColumn(Length =512, ColumnDescription = "失败原因")]
        public string? Fail { get; set; }
    }
}
