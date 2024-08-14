using GHMonitoringCenterApi.Domain.Enums;
using System.ComponentModel;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject
{
    /// <summary>
    /// 项目月报产报构成基础数据响应dto
    /// </summary>
    public class MonthReportForProjectBaseDataResponseDto
    {
        /// <summary>
        /// key
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 施工性质、产值属性、资源枚举类型
        /// </summary>
        public RouseType RouseType { get; set; }
        /// <summary>
        /// 自有/分包类型
        /// </summary>
        public ConstructionOutPutType ShipRouseType { get; set; }
        /// <summary>
        /// 施工性质
        /// </summary>
        public int? ConstructionNatureType { get; set; }
    }
    /// <summary>
    /// 施工性质、产值属性、资源枚举类型
    /// </summary>
    public enum RouseType
    {
        /// <summary>
        /// 资源
        /// </summary>
        [Description("资源")]
        Rouse = 1,
        /// <summary>
        /// 施工性质
        /// </summary>
        [Description("施工性质")]
        ConstructionNature = 2,
        /// <summary>
        /// 自有
        /// </summary>
        [Description("自有")]
        Self = 3,
        /// <summary>
        /// 分包
        /// </summary>
        [Description("分包")]
        Sub = 4
    }
}
