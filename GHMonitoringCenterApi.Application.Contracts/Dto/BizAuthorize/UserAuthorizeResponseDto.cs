using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize
{
    /// <summary>
    /// 授权业务返回对象
    /// </summary>
    public  class UserAuthorizeResponseDto
    {

        /// <summary>
        ///  授权Id
        /// </summary>
        public Guid? AuthorizeId { get; set; }

        /// <summary>
        ///  授权模式
        /// </summary>
        public AuthorizeMode? AuthorizeMode { get; set; }

        /// <summary>
        /// 授权的用户id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string? UserPhone{ get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 到期时间文本格式
        /// </summary>
        public string? ExpireText { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool IsExpired { get; set; }
    }
}
