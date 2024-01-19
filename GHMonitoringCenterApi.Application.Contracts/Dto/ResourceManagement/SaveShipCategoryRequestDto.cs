using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement
{
    /// <summary>
    /// 新增或删除类别
    /// </summary>
    public class SaveShipCategoryRequestDto
    {
        /// <summary>
        /// 请求类型  true是添加   false是修改
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 类别类型
        /// </summary>
        public ConstructionOutPutType? CategoryType { get; set; }
    }
}
