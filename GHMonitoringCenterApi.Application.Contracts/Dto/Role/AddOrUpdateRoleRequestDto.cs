using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{


    /// <summary>
    /// 添加或修改角色
    /// </summary>
    public class AddOrUpdateRoleRequestDto:BasePrimaryRequestDto
    {
        /// <summary>
        /// 请求类型   true新增角色  false编辑角色  默认是true新增
        /// </summary>
        public bool RequestType { get; set; }

        /// <summary>
        /// 机构ID  修改不需要传  新增传递
        /// </summary>
        public Guid InstitutionId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否有审批功能
        /// </summary>
        public bool IsApprove { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作权限  0 全部  1 查询
        /// </summary>
        public int OperationType { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RequestType)
            {
                Id = GuidUtil.Next();
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("角色名称不能为空", new string[] { nameof(AddOrUpdateRoleRequestDto.Name) });
            }
            if (Type<1)
            {
                yield return new ValidationResult("角色类型必须大于等于1小于等于6", new string[] { nameof(AddOrUpdateRoleRequestDto.Type) });
            }

            if (RequestType&&InstitutionId==Guid.Empty)
            {
                yield return new ValidationResult("机构ID不能为空", new string[] { nameof(AddOrUpdateRoleRequestDto.InstitutionId) });
            }

            if (!string.IsNullOrWhiteSpace(Remark)&& Remark.Length>200)
            {
                yield return new ValidationResult("角色描述不能超过200长度", new string[] { nameof(AddOrUpdateRoleRequestDto.Remark) });
            }
        }
    }
}
