using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.UnReport
{


    /// <summary>
    /// 
    /// </summary>
    public class UnpRrojectReportJjtResponseDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 用户的人资编码  发消息使用
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string  Name { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string Reportformer { get; set; }

        /// <summary>
        /// 未填报日期
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 项目日报Id
        /// </summary>
        public Guid? DayReportId { get; set; }
        /// <summary>
        /// 船舶日报ID
        /// </summary>
        public Guid? DayShipReportId { get; set; }
       
    }
}
