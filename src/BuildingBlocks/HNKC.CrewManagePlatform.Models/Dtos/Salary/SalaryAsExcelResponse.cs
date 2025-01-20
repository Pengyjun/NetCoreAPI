﻿using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Salary
{

    /// <summary>
    /// 读取excel内容填充实体
    /// </summary>
    public class SalaryAsExcelResponse
    {
        /// <summary>
        /// 倒计时3天
        /// </summary>
        public double CutOffTime { get; set; }
        /// <summary>
        /// 应发工资
        /// </summary>
       //public decimal WagesPayable { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        [ExcelColumnName("职工号")]
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 姓名
          /// </summary>
        [ExcelColumnName("姓名")]
        public string? Name { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [ExcelColumnName("部门名称")]
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [ExcelColumnName("手机号")]
        public string? Phone { get; set; }


        /// <summary>
        /// 日期
        /// </summary>
        [ExcelColumnName("日期")]
        public int DataMonth { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [ExcelColumnName("年份")]
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [ExcelColumnName("月份")]
        public int Month { get; set; }


        /// <summary>
        /// 基础工资
        /// </summary>
        [ExcelColumnName("基础工资")]
        public decimal BaseWage { get; set; }

        /// <summary>
        /// 职级工资
        /// </summary>
        [ExcelColumnName("职级工资")]
        public decimal RankWage { get; set; }


        /// <summary>
        /// 岗位工资
        /// </summary>
        [ExcelColumnName("岗位工资")]
        public decimal PostWage { get; set; }


        /// <summary>
        /// 技能工资
        /// </summary>
        [ExcelColumnName("技能工资")]
        public decimal SkillWage { get; set; }

        /// <summary>
        /// 工龄工资
        /// </summary>
        [ExcelColumnName("工龄工资")]
        public decimal WorkAgeWage { get; set; }


        /// <summary>
        /// 月度绩效工资
        /// </summary>
        [ExcelColumnName("月度绩效工资")]
        public decimal MonthPerformance { get; set; }


        /// <summary>
        /// 季度绩效结算
        /// </summary>
        [ExcelColumnName("季度绩效结算")]
        public decimal QuarterPerformance { get; set; }



        /// <summary>
        /// 船员顶岗工资
        /// </summary>
        [ExcelColumnName("船员顶岗工资")]
        public decimal RegularWage { get; set; }


        /// <summary>
        /// 船员培训绩效
        /// </summary>
        [ExcelColumnName("船员培训绩效")]
        public decimal TrainPerformance { get; set; }

        /// <summary>
        /// 加班工资
        /// </summary>
        [ExcelColumnName("加班工资")]
        public decimal OvertimeWage { get; set; }

        /// <summary>
        /// 补发补扣工资
        /// </summary>
        [ExcelColumnName("补发补扣工资")]
        public decimal ReissueBuckleMoney { get; set; }

        /// <summary>
        /// 其它工资
        /// </summary>
        [ExcelColumnName("其它工资")]
        public decimal OtherWage { get; set; }


        /// <summary>
        /// 节日工资
        /// </summary>
        [ExcelColumnName("节日工资")]
        public decimal HolidaysWage { get; set; }


        /// <summary>
        /// 职称补贴
        /// </summary>
        [ExcelColumnName("职称补贴")]
        public decimal NameSubsidy { get; set; }


        /// <summary>
        /// 证书补贴
        /// </summary>
        [ExcelColumnName("证书补贴")]
        public decimal CertificateSubsidy { get; set; }

        /// <summary>
        /// 技能补贴
        /// </summary>
        [ExcelColumnName("技能补贴")]
        public decimal SkillSubsidy { get; set; }


        /// <summary>
        /// 船员证书补贴
        /// </summary>
        [ExcelColumnName("船员证书补贴")]
        public decimal CrewCertificateSubsidy { get; set; }

        /// <summary>
        /// 师带徒津贴
        /// </summary>
        [ExcelColumnName("师带徒津贴")]
        public decimal MasterApprenticeAllowance { get; set; }













        /// <summary>
        /// 值班补贴
        /// </summary>
        [ExcelColumnName("值班补贴")]
        public decimal DutyAllowance { get; set; }





        /// <summary>
        /// 高温津贴
        /// </summary>
        [ExcelColumnName("高温津贴")]
        public decimal HyperthermiaAllowance { get; set; }



        /// <summary>
        /// 安全津贴
        /// </summary>
        [ExcelColumnName("安全津贴")]
        public decimal SecurityAllowance { get; set; }

        /// <summary>
        /// 硫化氢补贴
        /// </summary>
        [ExcelColumnName("硫化氢补贴")]
        public decimal HydrogenSulfideSubsidy { get; set; }


        /// <summary>
        /// 住房补贴
        /// </summary>
        [ExcelColumnName("住房补贴")]
        public decimal HouseeSubsidy { get; set; }


        /// <summary>
        /// 伙食津贴
        /// </summary>
        [ExcelColumnName("伙食津贴")]
        public decimal FoodSubsidy { get; set; }



        /// <summary>
        /// 转制补贴
        /// </summary>
        [ExcelColumnName("转制补贴")]
        public decimal ConversionSubsidy { get; set; }



        /// <summary>
        /// 其他补贴
        /// </summary>
        [ExcelColumnName("其他补贴")]
        public decimal OtherSubsidy { get; set; }


        /// <summary>
        /// 海外工龄津贴
        /// </summary>
        [ExcelColumnName("海外工龄津贴")]
        public decimal OverseasWorkAgeAllowance { get; set; }


        /// <summary>
        /// 海外家庭补贴
        /// </summary>
        [ExcelColumnName("海外家庭补贴")]
        public decimal OverseasFamilyAllowance { get; set; }



        /// <summary>
        /// 上年绩效结算
        /// </summary>
        [ExcelColumnName("上年绩效结算")]
        public decimal UpYearPerformance { get; set; }

        /// <summary>
        /// 本年绩效预结算
        /// </summary>
        [ExcelColumnName("本年绩效预结算")]
        public decimal YearPerformance { get; set; }

        /// <summary>
        /// 单位奖励
        /// </summary>
        [ExcelColumnName("单位奖励")]
        public decimal UnitAward { get; set; }


        /// <summary>
        /// 代局发放奖励
        /// </summary>
        [ExcelColumnName("代局发放奖励")]
        public decimal AgentIssueAward { get; set; }



        /// <summary>
        /// 工会慰问金
        /// </summary>
        [ExcelColumnName("工会慰问金")]
        public decimal UnionCondolenceMoney { get; set; }

        /// <summary>
        /// 应发合计
        /// </summary>
        [ExcelColumnName("应发合计")]
        public decimal TotalDue { get; set; }



        /// <summary>
        /// 代垫款(负数扣回)
        /// </summary>
        [ExcelColumnName("代垫款(负数扣回)")]
        public decimal AdvanceMoney { get; set; }


        /// <summary>
        /// 补发(补扣)款
        /// </summary>
        [ExcelColumnName("补发(补扣)款")]
        public decimal ReissueDeductMoney { get; set; }



        /// <summary>
        ///个人养老
        /// </summary>
        [ExcelColumnName("个人养老")]
        public decimal IndividualPension { get; set; }



        /// <summary>
        ///个人医疗
        /// </summary>
        [ExcelColumnName("个人医疗")]
        public decimal PersonalMedicine { get; set; }




        /// <summary>
        ///个人失业
        /// </summary>
        [ExcelColumnName("个人失业")]
        public decimal PersonalUnemployment { get; set; }
















        /// <summary>
        ///个人公积金
        /// </summary>
        [ExcelColumnName("个人公积金")]
        public decimal IndividualReserveFund { get; set; }

        /// <summary>
        /// 个人年金
        /// </summary>
        [ExcelColumnName("个人年金")]
        public decimal IndividualAnnuity { get; set; }
        /// <summary>
        ///个人所得税
        /// </summary>
        [ExcelColumnName("个人所得税")]
        public decimal IndividualIncomeTax { get; set; }

        /// <summary>
        ///个人扣款合计
        /// </summary>
        [ExcelColumnName("个人扣款合计")]
        public decimal TotalPersonalDeductions { get; set; }

        /// <summary>
        ///实发工资合计
        /// </summary>
        [ExcelColumnName("实发工资合计")]
        public decimal TotalPay { get; set; }


        /// <summary>
        ///国内进卡笔数
        /// </summary>
        [ExcelColumnName("国内进卡笔数")]
        public decimal NumberIncomingCardsChina { get; set; }
        /// <summary>
        ///国内进卡金额合计
        /// </summary>
        [ExcelColumnName("国内进卡金额合计")]
        public decimal TotalAmountDomesticCreditCard { get; set; }
        /// <summary>
        ///境外发放（人民币）
        /// </summary>
        [ExcelColumnName("境外发放（人民币）")]
        [ExcelColumnIndex(51)]
        public decimal OffShoreIssuanceRMB { get; set; }

        /// <summary>
        ///境外发放（美元）
        /// </summary>
        [ExcelColumnName("境外发放  （美元）")]
        [ExcelColumnIndex(52)]
        public decimal OffShoreReleaseUS { get; set; }

        /// <summary>
        ///单位养老
        /// </summary>
        [ExcelColumnName("单位养老")]
        public decimal UnitPension { get; set; }


        /// <summary>
        ///单位医疗
        /// </summary>
        [ExcelColumnName("单位医疗")]
        public decimal UnitMedicine { get; set; }


        /// <summary>
        ///单位失业
        /// </summary>
        [ExcelColumnName("单位失业")]
        public decimal UnitUnemployment { get; set; }


        /// <summary>
        ///单位公积金
        /// </summary>
        [ExcelColumnName("单位公积金")]
        public decimal UnitReserveFund { get; set; }


        /// <summary>
        ///单位年金
        /// </summary>
        [ExcelColumnName("单位年金")]
        public decimal UnitAnnuity { get; set; }


        /// <summary>
        ///单位缴费合计
        /// </summary>
        [ExcelColumnName("单位缴费合计")]
        public decimal TotalUnitPayment { get; set; }


        /// <summary>
        ///狭义人工成本
        /// </summary>
        [ExcelColumnName("狭义人工成本")]
        public decimal NarrowLaborCost { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
