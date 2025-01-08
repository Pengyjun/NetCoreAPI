using System.ComponentModel.DataAnnotations;

namespace HNKC.CrewManagePlatform.Models.CommonRequest
{
    public class PageRequest : IValidatableObject
    {
        /// <summary>
        /// 关键字搜索
        /// </summary>
        public string? KeyWords { get; set; }     
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageIndex < 1)
            {
                yield return new ValidationResult("页码参数不合法值必须大于等于1", new string[] { nameof(PageIndex) });
            }
            if (PageSize < 5)
            {
                yield return new ValidationResult("页大小参数不合法值必须大于等于5 默认是10", new string[] { nameof(PageSize) });
            }
        }
    }
}
