using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement
{
    /// <summary>
    /// 获取类别列表响应Dto
    /// </summary>
    public class SearchShipCategoryResponsetDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类别类型
        /// </summary>
        public ConstructionOutPutType? ShipType { get; set; }
    }
}
