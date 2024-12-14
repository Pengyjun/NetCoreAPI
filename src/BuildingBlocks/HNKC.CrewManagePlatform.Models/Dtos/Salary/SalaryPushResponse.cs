using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Salary
{

    /// <summary>
    /// 工资推送响应
    /// </summary>
    public class SalaryPushResponse
    {
        public string Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string? WorkNumber { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 业务类型 1是批量 0是个人
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 1是成功 0是失败
        /// </summary>
        public int PushResult { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
