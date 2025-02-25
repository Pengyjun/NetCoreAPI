namespace GHMonitoringCenterApi.Application.Contracts.Dto.Common
{
    /// <summary>
    /// 用户授权数据允许访问dto
    /// </summary>
    public class UserAuthForDataDto
    {
        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 授权访问的公司Id集合
        /// </summary>
        public Guid?[] CompanyIds { get; set; }= new Guid?[0];
    }
}
