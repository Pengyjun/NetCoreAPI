using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    public class JjtProjectDayReport
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }
        public Guid CompanyId { get; set; }

        public int DateDay { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 当日实际产值
        /// </summary>
        public decimal DayActualProductionAmount { get; set; }
    }
}
