using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 显示列表查询参数请求类
    /// </summary>
    public class ProjectSearchRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属项目部
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string[]? ProjectStatusId { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public Guid? ProjectRegionId { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public Guid? ProjectTypeId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 属性标签
        /// </summary>
        public string[]? TagName { get; set; }
        /// <summary>
        /// 所在省份
        /// </summary>
        public Guid? ProjectAreaId { get; set; }
        /// <summary>
        /// 是否转换为人民币
        /// </summary>
        public bool IsConvert { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型Id
        /// </summary>
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// 船舶动态
        /// </summary>
        public ProjectShipState? ShipState { get; set; }
        /// <summary>
        /// 是否是对外接口   如果是 为true   权限更改为最高权限级别
        /// </summary>
        public bool IsDuiWai { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>

        public string? ManagerType { get; set; }
    }
}
