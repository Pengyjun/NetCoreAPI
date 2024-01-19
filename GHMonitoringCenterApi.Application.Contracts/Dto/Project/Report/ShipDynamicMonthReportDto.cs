using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 船舶动态月报Dto
    /// </summary>
    public abstract  class ShipDynamicMonthReportDto<TDetailDto> where TDetailDto: ShipDynamicMonthReportDetailDto
    {
        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public TDetailDto[] Details { get; set; }= new TDetailDto[0];
    }

    /// <summary>
    /// 船舶动态月报明细Dto
    /// </summary>
    public abstract class ShipDynamicMonthReportDetailDto
    {
        /// <summary>
        /// 项目月报明细Id
        /// </summary>
        public Guid? DetailId { get; set; }

        /// <summary>
        /// 船舶月报Id
        /// </summary>
        public Guid ShipDynamicMonthReportId { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 船舶状态
        /// </summary>
        public ProjectShipState ShipState { get; set; }

        /// <summary>
        /// 所在港口
        /// </summary>
        public Guid? PortId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }

}
