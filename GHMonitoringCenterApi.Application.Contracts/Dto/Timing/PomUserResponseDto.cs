using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{


    /// <summary>
    /// pom用户数据响应DTO
    /// </summary>
    public class PomUserResponseDto
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string? IdentityCard { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string? LoginName { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string? GroupLoginId { get; set; }
        /// <summary>
        /// 集团分组编码
        /// </summary>
        public string? GroupCode { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string? Number { get; set; }
    }
}
