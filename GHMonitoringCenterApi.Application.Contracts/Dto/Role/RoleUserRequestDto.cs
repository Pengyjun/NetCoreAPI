using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{


    /// <summary>
    /// 角色用户请求DTO
    /// </summary>
    public class RoleUserRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleId == Guid.Empty)
            {
                yield return new ValidationResult("角色Id不能为空", new string[]{nameof(RoleUserRequestDto.RoleId)});
            }
        }
    }
}
