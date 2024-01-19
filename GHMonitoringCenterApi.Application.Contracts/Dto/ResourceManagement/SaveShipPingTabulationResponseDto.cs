using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement
{
    /// <summary>
    /// 保存列表请求Dto
    /// </summary>
    public class SaveShipPingTabulationResponseDto
    {
        /// <summary>
        /// 请求类型  true是添加   false是修改
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        /// 保存类型
        /// </summary>
        public ConstructionOutPutType? Type { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? ShipPingId { get; set; }
        /// <summary>
        /// 往来单位Id
        /// </summary>
        public Guid? DealingUnitId { get; set; }
    }
}
