using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    /// 获取设备信息下拉接口请求Dto
    /// </summary>
    public class GetDeviceInformationRequesDto : BaseRequestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int? Type { get; set; }
    }
}
