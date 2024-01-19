using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 船级杜
    /// </summary>
    public class PomShipClassicResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// pomId
        /// </summary>
        public Guid PomId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 国别
        /// </summary>
        public string Country { get; set; }
        public string Contact { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string Post { get; set; }

        public int Sequence { get; set; }
        public string Remarks { get; set; }
    }
}
