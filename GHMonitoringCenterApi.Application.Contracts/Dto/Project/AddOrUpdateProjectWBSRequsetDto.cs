using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// wbs新增或修改
    /// </summary>
    public class AddOrUpdateProjectWBSRequsetDto
    {
        /// <summary>
        /// 添加true  修改false
        /// </summary>
        public bool RequestType { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>    
        public string? Name { get; set; }
    }
}
