using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{
    public class ShipCompleteRequestDto:BaseRequestDto
    {

        /// <summary>
        /// 船舶名称
        /// </summary>

        public string? ShipName { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>

        public int? StartMonth { get; set; } = DateTime.Now.ToString("yyyyMM").ObjToInt();
        /// <summary>
        /// 结束时间
        /// </summary>

        public int? EndMonth { get; set; }

        /// <summary>
        /// 年份
        /// </summary>

        public int? Year { get; set; } = DateTime.Now.Year;
    }
}
