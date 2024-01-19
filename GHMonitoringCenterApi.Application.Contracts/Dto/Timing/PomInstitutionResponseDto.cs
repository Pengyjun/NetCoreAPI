using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{


    /// <summary>
    /// pom机构数据
    /// </summary>
    public class PomInstitutionResponseDto
    {
        public Guid Id { get; set; }

        public string? Oid { get; set; }
        public string? Poid { get; set; }
        public string? Gpoid { get; set; }
        public string? Ocode { get; set; }
        public string? Name { get; set; }
        public string? Shortname { get; set; }
        public string? Status { get; set; }
        public string? Sno { get; set; }
        public string? Orule { get; set; }
        public string? Grule { get; set; }
        public string? Grade { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedAt { get; set; }
    }
}
