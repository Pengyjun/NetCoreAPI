using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 查询项目-项目结构树返回结果
    /// </summary>
    public  class ProjectWBSComposeResponseDto
    {

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 节点集合
        /// </summary>
        public ResProjectWBSNode[] Nodes { get; set; }= new ResProjectWBSNode[0]; 

        public class ResProjectWBSNode : ProjectWBSModelDto<ResProjectWBSNode>
        {
         
        }
    }


}
