using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Information
{
    /// <summary>
    /// 人员信息响应Dto
    /// </summary>
    public class InformationResponseDto
    {
        /// <summary>
        /// 用户
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 登录帐号
        /// </summary>
        public string? LoginAccount { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? Company { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
    }
}
