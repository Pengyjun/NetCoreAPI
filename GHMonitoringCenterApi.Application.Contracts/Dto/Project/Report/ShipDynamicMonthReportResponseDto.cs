using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{

    /// <summary>
    /// 船舶月报返回结果
    /// </summary>
    public class ShipDynamicMonthReportResponseDto : ShipDynamicMonthReportDto<ShipDynamicMonthReportResponseDto.ResShipDynamicMonthReportDetail>
    {

        /// <summary>
        /// 船舶月报明细
        /// </summary>
        public class ResShipDynamicMonthReportDetail : ShipDynamicMonthReportDetailDto
        {

        }
    }
}
