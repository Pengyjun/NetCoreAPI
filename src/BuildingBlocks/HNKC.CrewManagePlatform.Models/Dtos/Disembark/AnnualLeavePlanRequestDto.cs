using HNKC.CrewManagePlatform.Models.CommonRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 年休假计划请求类
    /// </summary>
    public class AnnualLeavePlanRequestDto : PageRequest
    {
        /// <summary>
        /// 休假年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 是否填报
        /// </summary>
        public bool? IsSubmit { get; set; }
    }
}
