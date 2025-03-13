using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 新增年度休假计划
    /// </summary>
    public class AddLeavePlanVacationRequestDto
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
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 职务id
        /// </summary>
        public Guid JobTypeId { get; set; }
        /// <summary>
        /// 第一持有适任证id
        /// </summary>
        public Guid CertificateId { get; set; }
        /// <summary>
        /// 是否在船过年 去年
        /// </summary>
        public bool IsOnShipLastYear { get; set; }
        /// <summary>
        /// 是否在船过年 当前年
        /// </summary>
        public bool IsOnShipCurrentYear { get; set; }
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
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 休假计划
    /// </summary>
    public class VacationInfo
    {
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
