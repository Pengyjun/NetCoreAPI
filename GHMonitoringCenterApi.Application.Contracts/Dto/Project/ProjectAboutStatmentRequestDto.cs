using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 项目与报表负责人列表请求dto
    /// </summary>
    public class ProjectAboutStatmentRequestDto : BaseRequestDto
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
        public string[]? ProjectStatusId { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public Guid? ProjectRegionId { get; set; }
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
        /// 是否转换为人民币
        /// </summary>
        public bool IsConvert { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? SearchTime { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? Reportformer { get; set; }

        /// <summary>
        /// 是否报表负责人为null
        /// </summary>
        public bool IsReportformerNull { get; set; }
    }
}
