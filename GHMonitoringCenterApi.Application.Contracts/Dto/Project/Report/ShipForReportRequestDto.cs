using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 船舶列表请求
    /// </summary>
    public  class ShipsForReportRequestDto : BaseRequestDto,IResetModelProperty
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份
        /// </summary>
        public int? DateMonth { get; set; }

        /// <summary>
        /// 填报月份时间
        /// </summary>
        public DateTime? DateMonthTime { get; set; }

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {
           if(DateMonth==null&& DateMonthTime!=null)
            {
                DateMonth = DateMonthTime.Value.ToDateMonth();
            }
        }
    }
}
