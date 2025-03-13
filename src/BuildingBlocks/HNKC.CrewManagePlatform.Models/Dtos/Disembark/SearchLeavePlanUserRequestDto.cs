using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 获取年休计划人员配置 请求dto
    /// </summary>
    public class SearchLeavePlanUserRequestDto
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid ShipId { get; set; }
    }
}
