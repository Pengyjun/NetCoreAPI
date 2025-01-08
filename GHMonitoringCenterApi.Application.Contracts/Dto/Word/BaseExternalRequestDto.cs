using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Word
{
    public class BaseExternalRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartTime { get; set; }= DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        /// <summary>
        /// 结束日期
        /// </summary>
        public string? EndTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
      
    }
}
