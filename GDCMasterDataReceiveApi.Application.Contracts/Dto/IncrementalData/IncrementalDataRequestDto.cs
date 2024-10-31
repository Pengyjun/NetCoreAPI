using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.IncrementalData
{
    /// <summary>
    /// 
    /// </summary>
    public class IncrementalDataRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }
    }
}
