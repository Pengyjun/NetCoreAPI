using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    public class CompanyDayProductionValueResponseDto
    {
        public int  DateDay { get; set; }
        public decimal TotalYearProductionValue { get; set; }

        public List<CompanyItem> CompanyItems { get; set; }
    }



    public class CompanyItem {
        public Guid? CompanyId { get; set; }

        public decimal DayProductionValue { get; set; }

        public decimal YearProductionValue { get; set; }

    }
}
