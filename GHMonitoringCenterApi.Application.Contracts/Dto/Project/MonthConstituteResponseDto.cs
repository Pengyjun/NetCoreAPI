using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 月报构成响应Dto
    /// </summary>
    public class MonthConstituteResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 项目日报Id
        /// </summary>
        public Guid? DayId { get; set; }

        /// <summary>
        /// 填报时间(int)
        /// </summary>
        public int FillingInt { get; set; }

        /// <summary>
        /// 填报时间(时间)
        /// </summary>
        public DateTime FillingTime { get; set; }

        /// <summary>
        ///  施工分类  
        /// </summary>
        public string? ProjectWBSName { get; set; }

        /// <summary>
        /// 实际日产量(m³)
        /// </summary>
        public decimal? ActualDailyOutput { get; set; }

        /// <summary>
        /// 实际日产值(元)
        /// </summary>
        public decimal? RealDailyOutputValue { get; set; }
    }
}
