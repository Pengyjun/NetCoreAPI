using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 工资表
    /// </summary>
    [SugarTable("t_salary", IsDisabledDelete = true, TableDescription = "工资表")]
    public class Salary : BaseEntity<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "用户ID")]
        public long UserId { get; set; }
        /// <summary>
        /// 职工号 
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "职工号")]
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "年份")]
        public int DataMonth { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "年份")]
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "月份")]
        public int Month { get; set; }


        /// <summary>
        /// 基本工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "基本工资")]
        public decimal BaseWage { get; set; }

        /// <summary>
        /// 职级工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "职级工资")]
        public decimal RankWage { get; set; }


        /// <summary>
        /// 岗位工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "岗位工资")]
        public decimal PostWage { get; set; }


        /// <summary>
        /// 技能工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "技能工资")]
        public decimal SkillWage { get; set; }

        /// <summary>
        /// 工龄工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "工龄工资")]
        public decimal WorkAgeWage { get; set; }


        /// <summary>
        /// 月度绩效
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "月度绩效")]
        public decimal MonthPerformance { get; set; }


        /// <summary>
        /// 季度绩效
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "季度绩效")]
        public decimal QuarterPerformance { get; set; }



        /// <summary>
        /// 顶岗工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "顶岗工资")]
        public decimal RegularWage { get; set; }


        /// <summary>
        /// 培训绩效
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "培训绩效")]
        public decimal TrainPerformance { get; set; }

        /// <summary>
        /// 加班工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "加班工资")]
        public decimal OvertimeWage { get; set; }

        /// <summary>
        /// 补发补扣
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "补发补扣")]
        public decimal ReissueBuckleMoney { get; set; }

        /// <summary>
        /// 其他工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "其他工资")]
        public decimal OtherWage { get; set; }


        /// <summary>
        /// 节日工资
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "节日工资")]
        public decimal HolidaysWage { get; set; }


        /// <summary>
        /// 昵称补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "昵称补贴")]
        public decimal NameSubsidy { get; set; }


        /// <summary>
        /// 证书补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "证书补贴")]
        public decimal CertificateSubsidy { get; set; }

        /// <summary>
        /// 技能补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "技能补贴")]
        public decimal SkillSubsidy { get; set; }


        /// <summary>
        /// 船员证书补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "船员证书补贴")]
        public decimal CrewCertificateSubsidy { get; set; }

        /// <summary>
        /// 师带徒津贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "师带徒津贴")]
        public decimal MasterApprenticeAllowance { get; set; }













        /// <summary>
        /// 值班津贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "值班津贴")]
        public decimal DutyAllowance { get; set; }





        /// <summary>
        /// 高温津贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "高温津贴")]
        public decimal HyperthermiaAllowance { get; set; }



        /// <summary>
        /// 安全津贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "安全津贴")]
        public decimal SecurityAllowance { get; set; }

        /// <summary>
        /// 硫化氢补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "硫化氢补贴")]
        public decimal HydrogenSulfideSubsidy{ get; set; }


        /// <summary>
        /// 住房补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "住房补贴")]
        public decimal HouseeSubsidy { get; set; }


        /// <summary>
        /// 伙食补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "伙食补贴")]
        public decimal FoodSubsidy { get; set; }



        /// <summary>
        /// 转制补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "转制补贴")]
        public decimal ConversionSubsidy { get; set; }



        /// <summary>
        /// 其他补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "其他补贴")]
        public decimal OtherSubsidy { get; set; }


        /// <summary>
        /// 海外工龄津贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "海外工龄津贴")]
        public decimal OverseasWorkAgeAllowance { get; set; }


        /// <summary>
        /// 海外家庭补贴
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "海外家庭补贴")]
        public decimal OverseasFamilyAllowance { get; set; }



        /// <summary>
        /// 上一年绩效结算
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "上一年绩效结算")]
        public decimal UpYearPerformance { get; set; }

        /// <summary>
        /// 本年年绩效结算
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "本年年绩效结算")]
        public decimal YearPerformance { get; set; }

        /// <summary>
        /// 单位奖励
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位奖励")]
        public decimal UnitAward { get; set; }


        /// <summary>
        /// 代局发放奖励
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "代局发放奖励")]
        public decimal AgentIssueAward { get; set; }



        /// <summary>
        /// 工会慰问金
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "工会慰问金")]
        public decimal UnionCondolenceMoney { get; set; }

        /// <summary>
        /// 应发合计
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "应发合计")]
        public decimal TotalDue { get; set; }



        /// <summary>
        /// 代垫款(负数扣回)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "代垫款(负数扣回)")]
        public decimal AdvanceMoney { get; set; }


        /// <summary>
        /// 补发(补扣)款
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "补发(补扣)款")]
        public decimal ReissueDeductMoney { get; set; }



        /// <summary>
        ///个人养老
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人养老")]
        public decimal IndividualPension { get; set; }



        /// <summary>
        ///个人医疗
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人医疗")]
        public decimal PersonalMedicine { get; set; }




        /// <summary>
        ///个人失业
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人失业")]
        public decimal PersonalUnemployment { get; set; }
















        /// <summary>
        ///个人公积金
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人公积金")]
        public decimal IndividualReserveFund { get; set; }

        /// <summary>
        /// 个人年金
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人年金")]
        public decimal IndividualAnnuity { get; set; }


        /// <summary>
        ///个人所得税
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人所得税")]
        public decimal IndividualIncomeTax { get; set; }

        /// <summary>
        ///个人扣款合计
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "个人扣款合计")]
        public decimal TotalPersonalDeductions { get; set; }

        /// <summary>
        ///实发工资合计
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "实发工资合计")]
        public decimal TotalPay { get; set; }


        /// <summary>
        ///国内进卡笔数
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "国内进卡笔数")]
        public decimal NumberIncomingCardsChina { get; set; }
        /// <summary>
        ///国内进卡金额合计
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "国内进卡金额合计")]
        public decimal TotalAmountDomesticCreditCard { get; set; }
        /// <summary>
        ///境外发放（人民币）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "境外发放（人民币）")]
        public decimal OffShoreIssuanceRMB { get; set; }

        /// <summary>
        ///境外发放（美元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "境外发放（美元）")]
        public decimal OffShoreReleaseUS { get; set; }

        /// <summary>
        ///单位养老
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位养老")]
        public decimal UnitPension { get; set; }


        /// <summary>
        ///单位医疗
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位医疗")]
        public decimal UnitMedicine { get; set; }


        /// <summary>
        ///单位失业
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位失业")]
        public decimal UnitUnemployment { get; set; }


        /// <summary>
        ///单位公积金
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位公积金")]
        public decimal UnitReserveFund { get; set; }


        /// <summary>
        ///单位年金
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位年金")]
        public decimal UnitAnnuity { get; set; }


        /// <summary>
        ///单位缴费合计
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "单位缴费合计")]
        public decimal TotalUnitPayment { get; set; }


        /// <summary>
        ///狭义人工成本
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,4)", ColumnDescription = "狭义人工成本")]
        public decimal NarrowLaborCost { get; set; }


        /// <summary>
        ///备注
        /// </summary>
        [SugarColumn(Length =512, ColumnDescription = "备注")]
        public string? Remark { get; set; }

        /// <summary>
        ///数据来源 0时候导入  1是新增
        /// </summary>
        [SugarColumn(ColumnDataType ="int", ColumnDescription = "数据来源 0时候导入  1是新增")]
        public int DataSource { get; set; }

        /// <summary>
        ///手机短信链接地址
        /// </summary>
        [SugarColumn(Length =1024, ColumnDescription = "手机短信链接地址")]
        public string? PhoneUrl { get; set; }
    }
}
