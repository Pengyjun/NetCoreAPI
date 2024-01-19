using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Common
{


    /// <summary>
    /// 职位类型人员数据请求DTO
    /// </summary>
    public class PositionUserRequestDto:IValidatableObject
    {
       /// <summary>
       /// 根据关键词搜索  名称 手机号进行搜索
       /// </summary>
        public string? KeyWords { get; set; }
        /// <summary>
        /// 职位类型
        /// </summary>
        public int Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type <= 0)
            {
                yield return new ValidationResult("职位类型必须大于0", new string[] { nameof(PositionUserRequestDto.Type) });
            }
        }
    }
}
