using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.StagingMonthReportRequestDto;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{

    /// <summary>
    /// 暂存月报请求Dto
    /// </summary>
    public class StagingMonthReportRequestDto : ProjectMonthReportDto<ReqStagingTreeProjectWBSDetailDto, ReqStagingMonthReportDetail>
    {


        /// <summary>
        /// 树状明细
        /// </summary>
        public class ReqStagingTreeProjectWBSDetailDto : TreeProjectWBSDetailDto<ReqStagingTreeProjectWBSDetailDto, ReqStagingMonthReportDetail>
        {

        }

        /// <summary>
        /// 项目月报明细
        /// </summary>
        public class ReqStagingMonthReportDetail : MonthReportDetailDto<ReqStagingMonthReportDetail>
        {

        }
    }
}
