using GHMonitoringCenterApi.Application.Contracts.Dto.Role;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{

    /// <summary>
    /// 添加或修改菜单请求DTO
    /// </summary>
    public class AddOrUpdateMenuRequestDto:BasePrimaryRequestDto
    {
        /// <summary>
        /// 请求类型  true是添加   false是修改
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>

        public string Name { get; set; }
        /// <summary>
        /// 菜单url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否是根菜单 如果添加第一级菜单则为true 否则为false
        /// </summary>
        public bool IsRootMenu { get; set; }
        /// <summary>
        /// 根菜单ID
        /// </summary>
        public int Mid { get; set; }
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单icon图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>

        public int? Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        public string? Remark { get; set; }
     

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           
            if (RequestType)
            {
                Id = GuidUtil.Next();
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("菜单名称不能为空", new string[] { nameof(AddOrUpdateMenuRequestDto.Name) });
            }
            if (string.IsNullOrWhiteSpace(Url))
            {
                yield return new ValidationResult("Url不能为空", new string[] { nameof(AddOrUpdateMenuRequestDto.Url) });
            }
            if (!string.IsNullOrWhiteSpace(Remark) && Remark.Length > 200)
            {
                yield return new ValidationResult("菜单描述不能超过200长度", new string[] { nameof(AddOrUpdateRoleRequestDto.Remark) });
            }
            if (!IsRootMenu)
            {
                //if (Mid < 1)
                //{
                //    yield return new ValidationResult("根菜单ID必须大于等于1", new string[] { nameof(AddOrUpdateMenuRequestDto.Mid) });

                //}

                if (ParentId <0)
                {
                    yield return new ValidationResult("父菜单ID必须大于等于0", new string[] { nameof(AddOrUpdateMenuRequestDto.ParentId) });
                }
            }
        }
    }
}
