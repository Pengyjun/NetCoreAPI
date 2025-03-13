using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 获取年休计划人员配置
    /// </summary>
    public class SearchLeavePlanUserResponseDto
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 职务Id
        /// </summary>
        public string? JobTypeId { get; set; }
        /// <summary>
        /// 职务名称
        /// </summary>
        public string JobTypeName { get; set; }
        /// <summary>
        /// 第一适任证书Id
        /// </summary>
        public Guid? CertificateId { get; set; }
        /// <summary>
        /// 第一适任证书名称
        /// </summary>
        public string CertificateName { get; set; }
        /// <summary>
        /// 第一适任证书扫描件
        /// </summary>
        public string CertificateScans { get; set; }
        /// <summary>
        /// 是否在船过年 去年
        /// </summary>
        public bool IsOnShipLastYear { get; set; }
        /// <summary>
        /// 是否在船过年 当前年
        /// </summary>
        public bool IsOnShipCurrentYear { get; set; }
    }
}
