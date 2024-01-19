using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.MenuDelete
{
    /// <summary>
    /// 删除菜单请求Dto
    /// </summary>
    public class DeleteMenuRequestDto : IValidatableObject
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == null )
            {
                yield return new ValidationResult("ID不能为空", new string[] { nameof(DeleteMenuRequestDto.Id) });
            }
        }
    }
}
