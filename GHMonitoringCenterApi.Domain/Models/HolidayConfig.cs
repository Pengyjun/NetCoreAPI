using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    ///节假日配置
    /// </summary>
    [SugarTable("t_holidayconfig", IsDisabledDelete = true)]
    public class HolidayConfig:BaseEntity<Guid>
    {
        /// <summary>
        /// 节假日标题
        /// </summary>
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Title { get; set; }

        /// <summary>
        /// 是否是节假日  0不是  1是
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? IsHoliday { get; set; }
        /// <summary>
        /// 节假日日期
        /// </summary>
        public DateTime HolidayTime { get; set; }

        /// <summary>
        /// 节假日日期
        /// </summary>
        public int DateDay { get; set; }
    }
}
