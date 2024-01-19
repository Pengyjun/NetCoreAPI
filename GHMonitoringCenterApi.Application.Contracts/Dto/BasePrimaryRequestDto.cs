using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    /// 对所有表的主键请求进行封装
    /// </summary>
    public class BasePrimaryRequestDto : IValidatableObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(Id) });
            }
        }
    }
}
