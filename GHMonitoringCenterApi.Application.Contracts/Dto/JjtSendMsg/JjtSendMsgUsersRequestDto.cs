using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 获取交建通发送消息用户列表 请求dto
    /// </summary>
    public class JjtSendMsgUsersRequestDto : BaseRequestDto, IValidatableObject
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        //public string? UserName { get; set; }
        /// <summary>
        ///分配用户类型  0 默认全部  1第一批用户  2第二批用户
        /// </summary>
        public int UserType { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserType != 1 && UserType != 2 && UserType != 0)
            {
                yield return new ValidationResult("传入用户类型错误", new string[] { nameof(UserType) });
            }
        }
    }
}
