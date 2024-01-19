using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{


    /// <summary>
    /// 删除角色下面的用户请求DTO
    /// </summary>
    public class RemoveRoleUserRequestDto:IValidatableObject
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Guid InstitutionId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleId == Guid.Empty)
            {
                yield return new ValidationResult("角色Id不能为空", new string[] { nameof(RemoveRoleUserRequestDto.RoleId) });
            }
            if (UserId == Guid.Empty)
            {
                yield return new ValidationResult("用户Id不能为空", new string[] { nameof(RemoveRoleUserRequestDto.UserId) });
            }
            if(InstitutionId==Guid.Empty) 
            {
                yield return new ValidationResult("机构Id不能为空", new string[] { nameof(RemoveRoleUserRequestDto.UserId) });
            }
        }
    }
}
