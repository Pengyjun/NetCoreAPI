using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    public class CompanyDayProductionValueResponseDto
    {
        public string  DateDay { get; set; }
        public decimal TotalYearProductionValue { get; set; }

        public List<CompanyItem> CompanyItems { get; set; }
    }



    public class CompanyItem {
        /// <summary>
        ///  公司当日完成产值 单位元
        /// </summary>
        public decimal? CompanyDayProductionValue { get; set; }

        /// <summary>
        /// 公司当年累计产值 单位元
        /// </summary>
        public decimal? YearCompanyProductionValue { get; set; }
        public Guid? CompanyId { get; set; }

        public decimal DayProductionValue { get; set; }

        public decimal YearProductionValue { get; set; }

    }
}
