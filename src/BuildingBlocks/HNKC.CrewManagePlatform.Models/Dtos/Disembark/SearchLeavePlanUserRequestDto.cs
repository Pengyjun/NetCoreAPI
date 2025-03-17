using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 获取年休计划人员配置 请求dto
    /// </summary>
    public class SearchLeavePlanUserRequestDto
    {
        /// <summary>
        /// 关键字查询  姓名/手机号/身份证号/职工号
        /// </summary>
        public string? KeyWords { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        public string[]? ShipId { get; set; }
        /// <summary>
        /// 适任证书id
        /// </summary>
        public Guid? CertificateId { get; set; }
        /// <summary>
        /// 获取休假日期详情接口使用   1 当前年在船过年情况  2 去年在船过年情况
        /// </summary>
        public int Year { get; set; }
    }
}
