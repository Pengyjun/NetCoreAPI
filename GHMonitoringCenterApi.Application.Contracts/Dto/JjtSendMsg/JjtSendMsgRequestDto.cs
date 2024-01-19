
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 交建通发消息列表请求对象
    /// </summary>
    public class JjtSendMsgRequestDto : IValidatableObject
    {
        /// <summary>
        /// 关键字搜索
        /// </summary>
        public string? KeyWord { get; set; }
        /// <summary>
        /// 选择日期
        /// </summary>
        public DateTime? Date { get; set; } = DateTime.MinValue;
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
