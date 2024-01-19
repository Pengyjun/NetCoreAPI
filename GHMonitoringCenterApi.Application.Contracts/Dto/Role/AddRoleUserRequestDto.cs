using GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{
    /// <summary>
    /// 新增用户角色响应Dto
    /// </summary>
    public class AddRoleUserRequestDto : IValidatableObject
    {
        /// <summary>
        ///  用户id集合
        /// </summary>
        public List<RoleUser> RoleUsers { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Guid? InstitutionId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid? RoleId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            if (RoleId == null || RoleId == Guid.Empty)
            {
                yield return new ValidationResult("机构ID不能为空", new string[] { nameof(AddRoleUserRequestDto.RoleId) });
            }
            if (InstitutionId == null || InstitutionId == Guid.Empty)
            {
                yield return new ValidationResult("角色ID不能为空", new string[] { nameof(AddRoleUserRequestDto.InstitutionId) });
            }
        }


   /// <summary>
   /// 角色下面的用户信息
   /// </summary>
    public class RoleUser :IValidatableObject
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == null||UserId==Guid.Empty)
            {
                yield return new ValidationResult("用户ID不能为空", new string[] { nameof(RoleUser.UserId) });
            }
        }
        }
    }
}
