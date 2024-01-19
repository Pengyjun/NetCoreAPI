using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 船舶部分字段模型
    /// </summary>
    public  class ShipPartDto
    {

        /// <summary>
        /// PomId
        /// </summary>
        public Guid? PomId { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 船舶所属公司
        /// </summary>
        public Guid? CompanyId { get;set; }

        /// <summary>
        /// 船舶来源类型
        /// </summary>
        public ShipType ShipType { get; set; }

        /// <summary>
        /// 船舶种类类型Id（ship.TypeId）
        /// </summary>
        public Guid? ShipKindTypeId { get; set; }

        /// <summary>
        /// 填报时间(int 类型)
        /// </summary>
        public int FillReportDateDay{ get; set; }
    }
}
