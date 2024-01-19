using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 没填日报的船舶请求dto
    /// </summary>
    public  class UnReportShipsRequestDto : BaseRequestDto, IResetModelProperty
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属项目部
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public Guid?[]? ProjectStatusId { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public Guid? ProjectRegionId { get; set; }
        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? ShipPingId { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public Guid? ProjectTypeId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 属性标签
        /// </summary>
        public string[]? TagName { get; set; }
        /// <summary>
        /// 所在省份
        /// </summary>
        public Guid? ProjectAreaId { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public Guid? ShipTypeId { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (StartTime == null)
            {
                StartTime = DateTime.Now.AddDays(-1);
            }
            if (EndTime == null)
            {
                EndTime = DateTime.Now.AddDays(-1);
            }
        }
    }
}
