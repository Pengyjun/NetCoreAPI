using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{


    /// <summary>
    /// 公司在手项目请求DTO
    /// </summary>
    public class CompanyProjectDetailsdRequestDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 状态ID
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 开工日期
        /// </summary>
        public DateTime? CommencementDate { get; set; }
    }
}
