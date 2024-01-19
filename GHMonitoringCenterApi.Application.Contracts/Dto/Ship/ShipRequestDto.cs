using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 自有分包船舶请求dto
    /// </summary>
    public class ShipRequestDto: IValidatableObject
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 关键词搜索
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 200;

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageIndex < 1)
            {
                yield return new ValidationResult("页码参数不合法值必须大于等于1", new string[] { nameof(BaseRequestDto.PageIndex) });
            }
            if (PageSize < 200)
            {
                yield return new ValidationResult("页大小参数不合法值必须大于等于200", new string[] { nameof(BaseRequestDto.PageSize) });
            }
        }
    }
}
