using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class JjtSendMsgDayRepResponse
    {
        public Guid? Id { get; set; }
        public string ? Name { get; set; }
        public string ? ReportForMertel { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
