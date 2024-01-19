using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 获取备件仓储运输清单请求Dto
    /// </summary>
    public class GetSparePartStorageListRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string? ProjectCode { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string? Supplier { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string? ResponsiblePerson { get; set; }
    }
}
