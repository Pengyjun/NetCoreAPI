using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{

    /// <summary>
    /// 角色分配权限请求DTO
    /// </summary>
    public class RoleAddMenuRequestDto : IValidatableObject
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 菜单id集合
        /// </summary>
        public List<Guid> MenusIds { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleId == Guid.Empty)
            {
                yield return new ValidationResult("RoleId不能为空", new string[] { nameof(RoleAddMenuRequestDto.RoleId) });
            }

            if (!MenusIds.Any())
            {
                yield return new ValidationResult("菜单id集合不能为空", new string[] { nameof(RoleAddMenuRequestDto.MenusIds) });
            }
        }
    }
}
