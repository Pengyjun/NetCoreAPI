
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 交建通发消息  增修改通知用户
    /// </summary>
    public class JjtSendMsgModifyUsersRequestDto : IValidatableObject
    {
        /// <summary>
        /// 人员数据列表
        /// </summary>
        public List<JjtSendMsgModifyUsers> UserInfos { get; set; }
        /// <summary>
        /// 新增还是修改  true 新增 false修改
        /// </summary>
        public bool InsOrModify { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserInfos == null)
            {
                yield return new ValidationResult("传入对象类型不能为空", new string[] { nameof(UserInfos) });
            }
        }
    }
    /// <summary>
    /// 增修改通知用户 model
    /// </summary>
    public class JjtSendMsgModifyUsers : IValidatableObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 用户人资编码
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 用户名称（带电话）
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 第几批用户 1 第一批 2 第二批 为0就是删除的数据   三者可同时存在
        /// </summary>
        public int UserType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(UserCode))
            {
                yield return new ValidationResult("编码不能为空", new string[] { nameof(UserCode) });
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                yield return new ValidationResult("名称不能为空", new string[] { nameof(UserName) });
            }
            if (UserType != 0 && UserType != 1 && UserType != 2)
            {
                yield return new ValidationResult("用户类型只能为0或1或2", new string[] { nameof(UserType) });
            }
        }
    }
}
