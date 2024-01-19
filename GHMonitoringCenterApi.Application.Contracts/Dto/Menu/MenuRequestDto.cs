using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{


    /// <summary>
    /// 菜单请求DTO
    /// </summary>
    public class MenuRequestDto:IValidatableObject
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid? RoleId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleId == null || RoleId.Value == Guid.Empty)
            {
                yield return new ValidationResult("角色ID不能为空", new string[] { nameof(MenuRequestDto.RoleId) });
            }
        }
    }
}
