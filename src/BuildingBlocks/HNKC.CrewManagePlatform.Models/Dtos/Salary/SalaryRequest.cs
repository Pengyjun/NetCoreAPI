using HNKC.CrewManagePlatform.Models.CommonRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Salary
{
    public class SalaryRequest: PageRequest
    {
        /// <summary>
        /// 所属年份
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// 所属月份
        /// </summary>
        public int? Month { get; set; }
        /// <summary>
        /// 姓名
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
    }
}
