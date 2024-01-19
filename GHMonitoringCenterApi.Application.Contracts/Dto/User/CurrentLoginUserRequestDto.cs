using GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.User
{

    /// <summary>
    /// 当前登录用户用户信息请求DTO
    /// </summary>
    public class CurrentLoginUserRequestDto:IValidatableObject
    {
        /// <summary>
        /// 当前登录机构ID
        /// </summary>
        public Guid? InstitutionId { get; set; }
        /// <summary>
        /// 当前登录角色ID
        /// </summary>
        public Guid? RoleId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InstitutionId==null|| InstitutionId.Value==Guid.Empty)
            {
                yield return new ValidationResult("当前机构ID不能为空", new string[] { nameof(CurrentLoginUserRequestDto.InstitutionId) });
            }
            if (RoleId == null || RoleId.Value == Guid.Empty)
            {
                yield return new ValidationResult("当前角色ID不能为空", new string[] { nameof(CurrentLoginUserRequestDto.RoleId) });
            }
        }
    }
}
