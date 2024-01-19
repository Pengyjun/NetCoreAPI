using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter
{
    /// <summary>
    /// 添加或修改帮助中心详情请求Dto
    /// </summary>
    public class SaveHelpCenterDetailsRequsetDto : IValidatableObject
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 帮助中心菜单Id
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 是否重用 默认true  true 否  false 是
        /// </summary>
        public bool Reutilizando { get; set; }
        /// <summary>
        /// 请求类型  true是添加   false是修改
        /// </summary>
        public bool RequestType { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Details == null)
            {
                yield return new ValidationResult("名称不能为空", new string[] { nameof(SaveHelpCenterDetailsRequsetDto.Details) });
            }
        }
    }
}
