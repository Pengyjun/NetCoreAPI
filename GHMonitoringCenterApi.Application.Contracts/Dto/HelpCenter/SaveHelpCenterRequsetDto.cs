using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter
{
    /// <summary>
    /// 添加或修改帮助中心菜单请求Dto
    /// </summary>
    public class SaveHelpCenterRequsetDto : IValidatableObject
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }      
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? Details { get; set; }
        /// <summary>
        /// 是否重用 true 是  false 否
        /// </summary>
        public bool Reutilizando { get; set; }
        /// <summary>
        /// 请求类型  true是添加   false是修改
        /// </summary>
        public bool RequestType { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == null)
            {
                yield return new ValidationResult("名称不能为空", new string[] { nameof(Name) });
            }
            if (Name.Length>200)
            {
                yield return new ValidationResult("名称长度不能超过200", new string[] { nameof(Name) });
            }
        }
    }
}

