using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 领导班子响应DTO
    /// </summary>
    public class PomProjectLeaderResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目干系人员职位类型
        /// 1：项目经理； 2：执行总经理； 3：项目书记；
        /// 4：项目总工； 5：安全总监； 6：常务副经理；
        /// 7：项目副经理； 8：现场主要负责人； 9：现场技术负责人
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 项目 Id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目 Code
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 人员 Id
        /// </summary>
        public Guid AssistantManagerId { get; set; }
        /// <summary>
        /// 是否在任 true:在任 false:离任
        /// </summary>
        public bool IsPresent { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
