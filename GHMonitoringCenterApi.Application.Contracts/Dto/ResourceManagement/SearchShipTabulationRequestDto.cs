using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement
{
    /// <summary>
    /// 获取船舶列表
    /// </summary>
    public class SearchShipTabulationRequestDto
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 自有船舶Id
        /// </summary>
        public Guid? OwnerShipPingId { get; set; }
        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string? OwnerShipPingName { get; set; }
        /// <summary>
        /// 往来单位Id
        /// </summary>
        public string? DealingUnitId { get; set; }
        /// <summary>
        /// 往来单位名称
        /// </summary>
        public string? DealingUnitName { get; set; }
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public ConstructionOutPutType? Type { get; set; }
        /// <summary>
        /// 是否可以删除  true 是  false 否
        /// </summary>
        public bool RequestType { get; set; }
    }
}
