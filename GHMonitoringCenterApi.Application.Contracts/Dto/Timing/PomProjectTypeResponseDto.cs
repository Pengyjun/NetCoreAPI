using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 项目类型
    /// </summary>
    public class PomProjectTypeResponseDto
    {
        /// <summary>
        /// ID
        /// </summary>
        
        public Guid Id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
       
        public string? Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
       
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string? Remarks { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        
        public string? Sequence { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        
        public string? Unit { get; set; }




    }
}
