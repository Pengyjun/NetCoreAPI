using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 船舶资源属性Dto
    /// </summary>
    public  class ShipResourceDto
    {
        /// <summary>
        /// 产值属性
        /// </summary>
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
    }
}
