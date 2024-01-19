
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 交建通发送消息 修改对象
    /// </summary>
    public class JjtSendMsgDataRequestDto : IValidatableObject
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string? MsgContent { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == Guid.Empty)
            {
                yield return new ValidationResult("Id参数不能为空且类型是GUID类型", new string[] { nameof(Id) });
            }
            if (!string.IsNullOrWhiteSpace(MsgContent))
            {
                if (!MsgContent.Contains("自有船施工船舶"))
                {
                    yield return new ValidationResult("内容不包含“自有船施工船舶”字样，消息格式错误，编辑失败", new string[] { nameof(MsgContent) });
                }
            }
        }
    }
}
