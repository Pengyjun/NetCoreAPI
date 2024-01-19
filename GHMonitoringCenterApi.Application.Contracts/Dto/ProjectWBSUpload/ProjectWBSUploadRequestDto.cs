using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload
{
    /// <summary>
    /// 上传项目组织结构Dto
    /// </summary>
    public class ProjectWBSUploadRequestDto
    {
        public List<ProjectWBSUpload> projectWBSUploads { get; set; }
    }
    public class ProjectWBSUpload: TreeNodeParentIds<ProjectWBSUpload>
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 工程量
        /// </summary>
        public decimal? EngQuantity { get; set; }

        /// <summary>
        /// 项目编号（清单编码）
        /// </summary>
        public string? ProjectNum { get; set; }
    }
}
