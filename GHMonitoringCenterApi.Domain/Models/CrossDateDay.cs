using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 日期表
    /// </summary>
    [SugarTable("t_crossdateday", IsDisabledDelete = true)]
    public class CrossDateDay 
    {
        /// <summary>
        /// 日期（数字格式）
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(ColumnDataType = "date")]
        public DateTime  DateDayTime { get; set; }

        /// <summary>
        /// 是否是节假日
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsHoliday { get; set; }

        /// <summary>
        /// 节假日类型（元旦：101，春节：121，清明：405，劳动：501，端午：505，中秋：815,国庆：1001）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public HolidayType? HolidayType { get; set; }

        /// <summary>
        /// 农历日期
        /// </summary>
        [SugarColumn(ColumnDataType = "date")]
        public DateTime? ChineseDayTime { get; set; }


    }
}
