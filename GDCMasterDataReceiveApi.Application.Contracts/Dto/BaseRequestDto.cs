using System.ComponentModel.DataAnnotations;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 基本请求DTO封装
    /// </summary>
    public class BaseRequestDto : IValidatableObject
    {
        /// <summary>
        /// 是否全量导出   默认false  如果不需要导出则此字段可忽略
        /// </summary>
        public bool IsFullExport { get; set; } = false;
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
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>

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
