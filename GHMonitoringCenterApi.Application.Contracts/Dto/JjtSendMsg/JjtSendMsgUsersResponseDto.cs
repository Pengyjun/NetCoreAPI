
namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 交建通发送人员列表 响应对象
    /// </summary>
    public class JjtSendMsgUsersResponseDto 
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { get;set; }
        /// <summary>
        /// 用户人资编码
        /// </summary>
        public string? UserGroupCode { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 当前用户类型
        /// </summary>
        public int UserType { get; set; }
    }
}
