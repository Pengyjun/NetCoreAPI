using HNKC.CrewManagePlatform.Models.CommonRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Salary
{

    /// <summary>
    /// 工资推送请求DTO
    /// </summary>
    public class SalaryPushRequest: PageRequest
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Required(AllowEmptyStrings =true)]
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? PushTime { get; set; }
        /// <summary>
        /// 推送结果
        /// </summary>
        public int? PushResult { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int? BusinessType { get; set; }
    }
}
