using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 船舶定员标准请求类
    /// </summary>
    public class ShipPersonRequestDto
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 职务id
        /// </summary>
        public string? JobTypeId { get; set; }
        /// <summary>
        /// 职务名称
        /// </summary>
        public string? JobTypeName { get; set; }
    }
}
