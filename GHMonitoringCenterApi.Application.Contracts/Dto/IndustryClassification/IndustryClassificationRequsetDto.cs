using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Currency
{
    /// <summary>
    /// 返回Id
    /// </summary>
    public class IndustryClassificationRequsetDto : IValidatableObject
    {
        public IndustryClassificationRequsetDto()
        {
        }


        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(Id) });
            }
        }
    }
}
