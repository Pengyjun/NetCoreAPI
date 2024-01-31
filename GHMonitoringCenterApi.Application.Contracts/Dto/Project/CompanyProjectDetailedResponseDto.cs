using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{

    /// <summary>
    /// 公司在手项目清单
    /// </summary>
    public class CompanyProjectDetailedResponseDto
    {
        public Guid  Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>

        public string ProjectName { get; set; }     
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>

        public string TypeName { get; set; }

        /// <summary>
        /// 配需
        /// </summary>
        public int StatusSort { get; set; }
        public string StatusName { get; set; }
        /// <summary>
        /// 合同额（万元）
        /// </summary>
        public decimal ContractAmount { get; set; }
        /// <summary>
        /// 工程进度
        /// </summary>
        public decimal ProjectProgress { get; set; }
        /// <summary>
        /// 开工日期
        /// </summary>
        public DateTime CommencementDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get { return   (CommencementDate.Year==DateTime.Now.Year?"新中标":""); }}
    }
}
