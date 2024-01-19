using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement
{
    /// <summary>
    /// 获取设备管理请求Dto
    /// </summary>
    public class SearchEquipmentManagementRequestDto : BaseRequestDto
    {
        /// <summary>
        ///  设备类型 1. 水上设备  2. 陆域设备   3. 特种设备
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StarTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
