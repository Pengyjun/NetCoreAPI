using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{

    /// <summary>
    ///下拉基本响应DTO
    /// </summary>
    public class BasePullDownResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string? Code { get; set; }
    }
}
