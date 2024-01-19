using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 保存船舶月报请求
    /// </summary>
    public  class SaveShipDynamicMonthReportRequestDto: ShipDynamicMonthReportDto<SaveShipDynamicMonthReportRequestDto.ReqShipDynamicMonthReportDetail>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ShipId) });
            }
        }

        /// <summary>
        /// 船舶月报明细
        /// </summary>
        public class ReqShipDynamicMonthReportDetail: ShipDynamicMonthReportDetailDto,IValidatableObject
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (StartTime>EndTime)
                {
                    yield return new ValidationResult("起始时间不能大于结束时间", new string[] { nameof(ShipId) });
                }
            }
        }
    }
}
