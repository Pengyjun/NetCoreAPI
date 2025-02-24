using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 日报填报记录  0是默认值  1是不管什么状态  都算是当填写日报了（无论是否是已填写日报就算）  2是 在建项目正常填写的日报
    /// </summary>
    [SugarTable("t_daywritereportrecord", IsDisabledDelete = true)]
    public class DayWriteReportRecord:BaseEntity<Guid>
    {
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }
        [SugarColumn(ColumnDataType = "int",DefaultValue ="0")]
        public int DateDay { get; set; }

        /// <summary>
        /// 默认是空  在建项目  空就是缺少填写日报
        /// 如果值为0 项目是其他状态  可以理解为停工  
        /// 如果是1则正常填报 项目状态是在建的
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int One { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Two { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Three { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Four { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Five { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Six { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Seven { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Eight { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Nine { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Ten { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Eleven { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Twelve { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Thirteen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Fourteen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Fifteen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Sixteen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Seventeen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Eighteen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Nineteen { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Twenty { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyOne { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyTwo { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyThree { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyFour { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyFive { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentySix { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentySeven { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyEight { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int TwentyNine { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int Thirty { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int ThirtyOne { get; set; }
    }
}
