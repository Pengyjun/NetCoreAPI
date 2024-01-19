using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction
{
    /// <summary>
    /// 详情年度查询dto
    /// </summary>
    public class ProjectPlanSearchRequestDto:IValidatableObject
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageIndex < 1)
            {
                yield return new ValidationResult("页码参数不合法值必须大于等于1", new string[] { nameof(BaseRequestDto.PageIndex) });
            }
            if (PageSize < 5)
            {
                yield return new ValidationResult("页大小参数不合法值必须大于等于5 默认是10", new string[] { nameof(BaseRequestDto.PageSize) });
            }
        }
    }
}
