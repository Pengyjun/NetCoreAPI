using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    ///  水上设备
    /// </summary>
    public class ExportMarineEquipmentResponseDto
    {
        /// <summary>
        /// 水上设备
        /// </summary>
        List<ExportMarineEquipment>? exportMarineEquipment { get; set; }
    }
}
