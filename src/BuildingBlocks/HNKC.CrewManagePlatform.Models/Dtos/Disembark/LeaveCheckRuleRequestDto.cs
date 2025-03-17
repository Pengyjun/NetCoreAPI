using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 年休假规则验证
    /// </summary>
    public class LeaveCheckRuleRequestDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid ShipId { get; set; }
        /// <summary>
        /// 职务id
        /// </summary>
        public Guid JobTypeId { get; set; }
        /// <summary>
        /// 职务名称
        /// </summary>
        public string? JobTypeName { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 上半月或下半月  1：上半月  2：下半月
        /// </summary>
        public int VacationHalfMonth { get; set; }
    }
}
