using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Holiday
{

    public class HolidayResponseItem {

        public HolidayResponseDto time { get; set; }

    }
    /// <summary>
    /// 节假日响应DTO
    /// </summary>
    public class HolidayResponseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string? Date { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 是否是节假日
        /// </summary>
        public bool IsOffDay { get; set; }
    }
}
