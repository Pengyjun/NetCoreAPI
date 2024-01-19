using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{

    /// <summary>
    /// 菜单列表请求DTO
    /// </summary>
    public class MenuListRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 是否开启分页  默认true
        /// </summary>
        public bool IsPage { get; set; } = true;

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleId == Guid.Empty)
            {
                yield return new ValidationResult("角色ID不能为空", new string[] { nameof(MenuListRequestDto.RoleId) });
            }
        }
    }
}
