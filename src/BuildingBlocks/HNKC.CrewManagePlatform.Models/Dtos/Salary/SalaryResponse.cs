using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Salary
{
    /// <summary>
    /// 工资列表返回DTO
    /// </summary>
    public class SalaryResponse:BaseResponse
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
        /// 身份证号
        /// </summary>
        public string? CardId { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }


        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal BaseWage { get; set; }

        /// <summary>
        /// 职级工资
        /// </summary>
        public decimal RankWage { get; set; }


        /// <summary>
        /// 岗位工资
        /// </summary>
        public decimal PostWage { get; set; }


        /// <summary>
        /// 技能工资
        /// </summary>
        public decimal SkillWage { get; set; }

        /// <summary>
        /// 工龄工资
        /// </summary>
        public decimal WorkAgeWage { get; set; }


        /// <summary>
        /// 月度绩效
        /// </summary>
        public decimal MonthPerformance { get; set; }


        /// <summary>
        /// 季度绩效
        /// </summary>
        public decimal QuarterPerformance { get; set; }



        /// <summary>
        /// 顶岗工资
        /// </summary>
        public decimal RegularWage { get; set; }


        /// <summary>
        /// 培训绩效
        /// </summary>
        public decimal TrainPerformance { get; set; }

        /// <summary>
        /// 加班工资
        /// </summary>
        public decimal OvertimeWage { get; set; }

        /// <summary>
        /// 补发补扣
        /// </summary>
        public decimal ReissueBuckleMoney { get; set; }

        /// <summary>
        /// 其他工资
        /// </summary>
        public decimal OtherWage { get; set; }


        /// <summary>
        /// 节日工资
        /// </summary>
        public decimal HolidaysWage { get; set; }

        /// <summary>
        /// 昵称补贴
        /// </summary>
        public decimal NameSubsidy { get; set; }
        /// <summary>
        /// 技能补贴
        /// </summary>
        public decimal SkillSubsidy { get; set; }

        /// <summary>
        /// 证书补贴
        /// </summary>
        public decimal CertificateSubsidy { get; set; }


        /// <summary>
        /// 师徒补贴
        /// </summary>
        public decimal MasterApprenticeSubsidy { get; set; }

    }
}
