using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
   /// <summary>
   /// 自有船舶月报返回结果
   /// </summary>
    public  class OwnerShipMonthReportResponseDto: OwnerShipMonthReportDto<OwnerShipMonthReportResponseDto.ResOwnerShipMonthReportSoil>
    {
        /// <summary>
        /// 填报月份时间格式（前端可以根据需求获取年，月拼接）
        /// </summary>
        public DateTime DateMonthTime
        {
            get
            {
                ConvertHelper.TryParseFromDateMonth(DateMonth, out DateTime monthTime);
                return monthTime;
            }
        }

        /// <summary>
        /// 运转（h）
        /// </summary>
        public decimal WorkingHours { get; set; }

        /// <summary>
        /// 产量（方）
        /// </summary>
        public decimal Production { get; set; }

        /// <summary>
        /// 产值（元）
        /// </summary>
        public decimal ProductionAmount { get; set; }



        /// <summary>
        /// 主要施工土质
        /// </summary>
        public class ResOwnerShipMonthReportSoil : OwnerShipMonthReportSoilDto
        {
        }
    }

}
