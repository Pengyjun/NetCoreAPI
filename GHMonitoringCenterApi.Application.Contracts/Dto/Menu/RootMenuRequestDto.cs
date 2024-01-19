using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{

    /// <summary>
    /// 根菜单请求DTO
    /// </summary>
    public class RootMenuRequestDto : IValidatableObject
    {
        /// <summary>
        /// 父菜单ID 默认是0
        /// </summary>
        public int ParentId { get; set; } = 0;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ParentId < 0)
            {
                yield return new ValidationResult("父菜单ID必须大于等于0", new string[] { nameof(RootMenuRequestDto.ParentId) });
            }
        }
    }
}
