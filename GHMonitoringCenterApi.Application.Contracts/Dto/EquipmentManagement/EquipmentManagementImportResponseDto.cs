using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    /// 非自有设备导入Dto
    /// </summary>
    public class EquipmentManagementImportResponseDto
    {
        /// <summary>
        /// 水上设备
        /// </summary>
        ExportMarineEquipment? exportMarineEquipment { get; set; }
        /// <summary>
        /// 陆域设备
        /// </summary>
        ExportLandEquipment? exportLandEquipment { get; set; }
        /// <summary>
        /// 特种设备
        /// </summary>
        ExportSpecialEquipment? exportSpecialEquipment { get; set; }
    }
}
