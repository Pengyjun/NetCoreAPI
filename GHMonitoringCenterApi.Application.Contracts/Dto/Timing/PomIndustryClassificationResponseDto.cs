using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 项目行业分类标准
    /// </summary>
    public class PomIndustryClassificationResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// parentId
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        public string? Sequence { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Remarks { get; set; }
    }
}
