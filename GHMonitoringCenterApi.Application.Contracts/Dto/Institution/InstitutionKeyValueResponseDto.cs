using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Institution
{


    public class InstitutionKeyValueResponseDto
    {
        /// <summary>
        /// 所属公司pomId
        /// </summary>
        public Guid   CPomId { get; set; }

        /// <summary>
        /// 所属公司部门POMID
        /// </summary>
        public Guid DPomId { get; set; }
        /// <summary>
        /// 所属公司OID
        /// </summary>
        public string? Coid { get; set; }

        /// <summary>
        /// 所属公司poid
        /// </summary>
        public string? Cpoid { get; set; }
        /// <summary>
        /// 所属公司名称（简称）
        /// </summary>
        public string? AffCompanName { get; set; }
        /// <summary>
        /// 公司名称（全称）
        /// </summary>
        public string? AffCompanyFullName { get; set; }
        /// <summary>
        /// 所属项目部OID
        /// </summary>
        public string? Doid { get; set; }

        /// <summary>
        /// 所属项目部POID
        /// </summary>
        public string? Dpoid { get; set; }
        /// <summary>
        /// 所属项目部名称
        /// </summary>
        public string? AffDepartmentName { get; set; }



    }
}
