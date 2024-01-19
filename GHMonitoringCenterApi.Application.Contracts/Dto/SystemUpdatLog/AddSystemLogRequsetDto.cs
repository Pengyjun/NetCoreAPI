using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog
{
    /// <summary>
    /// 添加系统日志请求Dto
    /// </summary>
    public class AddSystemLogRequsetDto : IValidatableObject
    {
        /// <summary>
        /// 系统日志内容
        /// </summary>
        public string? LogContent { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LogContent.Length>1000)
            {
                yield return new ValidationResult("系统日志内容不能超过500字", new string[] { nameof(AddSystemLogRequsetDto.LogContent) });
            }
        }
    }
}
