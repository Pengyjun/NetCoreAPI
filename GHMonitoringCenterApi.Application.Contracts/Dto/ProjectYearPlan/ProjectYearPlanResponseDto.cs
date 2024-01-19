using GHMonitoringCenterApi.Domain.Shared.Util;
using MiniExcelLibs.Attributes;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan
{


    public class ProjectYearPlanResponseDto
    {
        ///合计相关字段
        /// 一月份合计产值
        /// </summary>
        public decimal TotalOneProductionValue { get; set; }
        /// <summary>
        /// 二月份合计产值
        /// </summary>
        public decimal TotalTwoProductionValue { get; set; }

        public decimal TotalThreeProductionValue { get; set; }
        public decimal TotalFourProductionValue { get; set; }
        public decimal TotalFiveProductionValue { get; set; }
        public decimal TotalSixProductionValue { get; set; }
        public decimal TotalSevenProductionValue { get; set; }
        public decimal TotalEightProductionValue { get; set; }
        public decimal TotalNineProductionValue { get; set; }
        public decimal TotalTenProductionValue { get; set; }
        public decimal TotalElevenProductionValue { get; set; }
        public decimal TotalTwelveProductionValue { get; set; }


        /// <summary>
        /// 总产值合计
        /// </summary>
        public decimal TotalYearProductionValue { get; set; }


        /// <summary>
        /// 剩余总产值合计
        /// </summary>
        public decimal TotalResidueProductionValue { get; set; }

        public List<ProjectYearPlanDetails> ProjectYearPlanDetails { get; set; }=new List<ProjectYearPlanDetails>();
    }
    /// <summary>
    /// 项目年初计划响应DTO
    /// </summary>
    //    public class ProjectYearPlanDetails
    //    {
    //        /// <summary>
    //        /// 项目类型
    //        /// </summary>

    //        public Guid  ProjectType { get; set; } 
    //        /// <summary>
    //        /// 项目名称
    //        /// </summary>
    //        [ExcelColumnName("项目名称")]
    //        public string ProjectName { get; set; }
    //        /// <summary>
    //        /// 公司ID
    //        /// </summary>
    //        [ExcelIgnore]
    //        public Guid CompanyId { get; set; }
    //        /// <summary>
    //        /// 公司名称
    //        /// </summary>
    //        [ExcelColumnName("公司名称")]
    //        public string CompanyName { get; set; }

    //        /// <summary>
    //        /// 项目年初计划树来源于项目WBS
    //        /// </summary>
    //        // public List<ProjectYearPlanTree> ProjectYearPlanTrees { get; set; } 
    //        [ExcelColumnName("合同额")]
    //        /// <summary>
    //        ///合同额
    //        /// </summary>
    //        public decimal ContractAmount { get; set; }
    //        /// <summary>
    //        /// 项目状态
    //        /// </summary>
    //        [ExcelColumnName("项目状态")]
    //        public string ProjectStatus { get; set; }
    //        /// <summary>
    //        ///主键ID
    //        /// </summary>
    //        //public Guid Id { get; set; }
    //        /// <summary>
    //        /// 开累产值
    //        /// </summary>
    //        [ExcelColumnName("开累产值")]
    //        public decimal TotalProductionValue { get; set; }

    //        /// <summary>
    //        /// 剩余产值
    //        /// </summary>
    //        [ExcelColumnName("剩余产值")]
    //        public decimal SurplusProductionValue { get { return Math.Round((ContractAmount - TotalProductionValue),2); } }

    //        /// <summary>
    //        /// 项目ID
    //        /// </summary>
    //        [ExcelIgnore]
    //        public Guid ProjectId { get; set; }
    //        [ExcelColumnName("年总工程量")]
    //        /// <summary>
    //        /// 年总工程量
    //        /// </summary>
    //        public decimal YearTotalQuantity { get; set; }
    //        /// <summary>
    //        /// 年总产值
    //        /// </summary>
    //        [ExcelColumnName("年总产值")]
    //        public decimal YearTotalProductionValue { get; set; }

    //        /// <summary>
    //        /// 一月份工程量
    //        /// </summary>
    //        [ExcelColumnName("一月份工程量")]
    //        public decimal OneQuantity { get; set; }
    //        /// <summary>
    //        /// 一月份产值
    //        /// </summary>
    //        [ExcelColumnName("一月份产值")]
    //        public decimal OneProductionValue { get; set; }
    //        /// <summary>
    //        /// 二月份工程量
    //        /// </summary>
    //        [ExcelColumnName("二月份工程量")]
    //        public decimal TwoQuantity { get; set; }
    //        /// <summary>
    //        /// 二月份产值
    //        /// </summary>
    //        [ExcelColumnName("二月份产值")]
    //        public decimal TwoProductionValue { get; set; }
    //        [ExcelColumnName("3月份工程量")]
    //        public decimal ThreeQuantity { get; set; }
    //        [ExcelColumnName("3月份产值")]
    //        public decimal ThreeProductionValue { get; set; }
    //        [ExcelColumnName("4月份工程量")]
    //        public decimal FourQuantity { get; set; }
    //        [ExcelColumnName("4月份产值")]
    //        public decimal FourProductionValue { get; set; }
    //        [ExcelColumnName("5月份工程量")]
    //        public decimal FiveQuantity { get; set; }
    //        [ExcelColumnName("5月份产值")]
    //        public decimal FiveProductionValue { get; set; }
    //        [ExcelColumnName("6月份工程量")]
    //        public decimal SixQuantity { get; set; }
    //        [ExcelColumnName("6月份产值")]
    //        public decimal SixProductionValue { get; set; }
    //        [ExcelColumnName("7月份工程量")]
    //        public decimal SevenQuantity { get; set; }
    //        [ExcelColumnName("7月份产值")]
    //        public decimal SevenProductionValue { get; set; }
    //        [ExcelColumnName("8月份工程量")]
    //        public decimal EightQuantity { get; set; }
    //        [ExcelColumnName("8月份产值")]
    //        public decimal EightProductionValue { get; set; }
    //        [ExcelColumnName("9月份工程量")]
    //        public decimal NineQuantity { get; set; }
    //        [ExcelColumnName("9月份产值")]
    //        public decimal NineProductionValue { get; set; }
    //        [ExcelColumnName("10月份工程量")]
    //        public decimal TenQuantity { get; set; }
    //        [ExcelColumnName("10月份产值")]
    //        public decimal TenProductionValue { get; set; }
    //        [ExcelColumnName("11月份工程量")]
    //        public decimal ElevenQuantity { get; set; }
    //        [ExcelColumnName("11月份产值")]
    //        public decimal ElevenProductionValue { get; set; }
    //        [ExcelColumnName("12月份工程量")]
    //        public decimal TwelveQuantity { get; set; }
    //        [ExcelColumnName("12月份产值")]
    //        public decimal TwelveProductionValue { get; set; }
    //        [ExcelIgnore]
    //        //x轴数据
    //        public List<string> XData  { get; set; }
    //        [ExcelIgnore]
    //        //y轴数据
    //        public List<decimal> YData  { get; set; }

    //}
    public class ProjectYearPlanDetails
    {

        public decimal ExchangeRate { get; set; }

        public Guid  ProjectType { get; set; } 
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExcelColumnName("项目名称")]
        [ExcelColumnWidth(45)]
        public string ProjectName { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [ExcelColumnWidth(45)]
        [ExcelIgnore]
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [ExcelColumnWidth(45)]
        [ExcelColumnName("公司名称")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 项目年初计划树来源于项目WBS
        /// </summary>
        [ExcelColumnWidth(45)]
        [ExcelColumnName("合同额")]
        /// <summary>
        ///合同额
        /// </summary>
        public decimal ContractAmount { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [ExcelColumnName("项目状态")]
        [ExcelColumnWidth(45)]
        public string ProjectStatus { get; set; }
        /// <summary>
        ///主键ID
        /// </summary>
        //public Guid Id { get; set; }
        /// <summary>
        /// 开累产值
        /// </summary>
        [ExcelColumnWidth(45)]
        [ExcelColumnName("开累产值")]
        public decimal TotalProductionValue { get; set; }

        /// <summary>
        /// 剩余产值
        /// </summary>
        [ExcelColumnWidth(45)]
        [ExcelColumnName("剩余产值")]
        public decimal SurplusProductionValue { get { return Math.Round((ContractAmount - TotalProductionValue) /1, 2); } }

        /// <summary>
        /// 项目ID
        /// </summary>
        [ExcelIgnore]
        public Guid ProjectId { get; set; }
        //[ExcelColumnName("年总工程量")]
        /// <summary>
        /// 年总工程量
        /// </summary>
        //[ExcelColumnWidth(45)]
        //public decimal YearTotalQuantity { get; set; }
        /// <summary>
        /// 年总产值
        /// </summary>
        [ExcelColumnWidth(45)]
        [ExcelColumnName("年总产值")]
        public decimal YearTotalProductionValue { get; set; }

        ///// <summary>
        ///// 一月份工程量
        ///// </summary>
        //[ExcelColumnName("一月份工程量")]
        //public decimal OneQuantity { get; set; }
        /// <summary>
        /// 一月份产值
        /// </summary>
        [ExcelColumnName("一月份产值")]
        [ExcelColumnWidth(45)]
        public decimal OneProductionValue { get; set; }
        ///// <summary>
        ///// 二月份工程量
        ///// </summary>
        //[ExcelColumnName("二月份工程量")]
        //public decimal TwoQuantity { get; set; }
        /// <summary>
        /// 二月份产值
        /// </summary>
        [ExcelColumnName("二月份产值")]
        [ExcelColumnWidth(45)]
        public decimal TwoProductionValue { get; set; }
        //[ExcelColumnName("3月份工程量")]
        //public decimal ThreeQuantity { get; set; }
        [ExcelColumnName("3月份产值")]
        [ExcelColumnWidth(45)]
        public decimal ThreeProductionValue { get; set; }
        //[ExcelColumnName("4月份工程量")]
        //public decimal FourQuantity { get; set; }
        [ExcelColumnName("4月份产值")]
        [ExcelColumnWidth(45)]
        public decimal FourProductionValue { get; set; }
        //[ExcelColumnName("5月份工程量")]
        //public decimal FiveQuantity { get; set; }
        [ExcelColumnName("5月份产值")]
        [ExcelColumnWidth(45)]
        public decimal FiveProductionValue { get; set; }
        //[ExcelColumnName("6月份工程量")]
        //public decimal SixQuantity { get; set; }
        [ExcelColumnName("6月份产值")]
        [ExcelColumnWidth(45)]
        public decimal SixProductionValue { get; set; }
        //[ExcelColumnName("7月份工程量")]
        //public decimal SevenQuantity { get; set; }
        [ExcelColumnName("7月份产值")]
        [ExcelColumnWidth(45)]
        public decimal SevenProductionValue { get; set; }
        //[ExcelColumnName("8月份工程量")]
        //public decimal EightQuantity { get; set; }
        [ExcelColumnName("8月份产值")]
        [ExcelColumnWidth(45)]
        public decimal EightProductionValue { get; set; }
        //[ExcelColumnName("9月份工程量")]
        //public decimal NineQuantity { get; set; }
        [ExcelColumnName("9月份产值")]
        [ExcelColumnWidth(45)]
        public decimal NineProductionValue { get; set; }
        ////[ExcelColumnName("10月份工程量")]
        //public decimal TenQuantity { get; set; }
        [ExcelColumnName("10月份产值")]
        [ExcelColumnWidth(45)]
        public decimal TenProductionValue { get; set; }
        //[ExcelColumnName("11月份工程量")]
        //public decimal ElevenQuantity { get; set; }
        [ExcelColumnName("11月份产值")]
        [ExcelColumnWidth(45)]
        public decimal ElevenProductionValue { get; set; }
        //[ExcelColumnName("12月份工程量")]
        //public decimal TwelveQuantity { get; set; }
        [ExcelColumnName("12月份产值")]
        [ExcelColumnWidth(45)]
        public decimal TwelveProductionValue { get; set; }
        [ExcelIgnore]
        //x轴数据
        public List<string> XData { get; set; }
        [ExcelIgnore]
        //y轴数据
        public List<decimal> YData { get; set; }

    }


    public class ProjectYearPlanTree : ProjectPlanTreeNode<ProjectYearPlanTree> 
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ProjectWbsId { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal _Quantity { get; set; }
        /// <summary>
        /// 工程量
        /// </summary>
        //public decimal ComplaneQuantity { get; set; }
        //public decimal _Quantity
        //{
        //    get
        //    {
        //        if (Quantity != null && Quantity != 0)
        //            return Quantity;
        //        return Children.Sum(c => Math.Round(c._Quantity, 2));
        //    }
        //    set { Quantity = value; }
        //}
        /// <summary>
        /// 单价
        /// </summary>
        public decimal _UnitPrice { get; set; }


        //public decimal _UnitPrice
        //{
        //    get
        //    {
        //        if (UnitPrice!=null&& UnitPrice!=0)
        //            return UnitPrice;
        //        return Children.Sum(c => Math.Round(c._UnitPrice,2));
        //    }
        //    set { UnitPrice = value; }
        //}
        /// <summary>
        ///合同额
        /// </summary>
        public decimal ContractAmount { get { return (Math.Round(_UnitPrice * _Quantity, 2)); } }

        /// <summary>
        /// 累计工程量
        /// </summary>
        public decimal TotalQuantity;
        public decimal _TotalQuantity
        {
            get
            {
                if (TotalQuantity != null && TotalQuantity != 0)
                    return TotalQuantity;
                return Children.Sum(c => Math.Round(c._TotalQuantity, 2));
            }
            set { TotalQuantity = value; }
        }
        /// <summary>
        /// 累计产值
        /// </summary>
        public decimal TotalProductuinValue;
        public decimal _TotalProductuinValue
        {
            get
            {
                if (TotalProductuinValue != null && TotalProductuinValue != 0)
                    return TotalProductuinValue;
                return Children.Sum(c => Math.Round(c._TotalProductuinValue, 2));
            }
            set { TotalProductuinValue = value; }
        }

        /// <summary>
        /// 是否可以删除  true 可以 false不可以  默认不可以删除
        /// </summary>
        public bool IsDelete { get; set; } = false;



        #region MyRegion
        /// <summary>
        /// 剩余工程量
        /// </summary>
        public decimal? ResidueQuantity { get { return _Quantity - _TotalQuantity; } }
        /// <summary>
        /// 剩余合同额
        /// </summary>
        public decimal? ResidueProductuinValue { get { return _UnitPrice - _TotalProductuinValue; } }

        /// <summary>
        /// 当年计划产值
        /// </summary>
        public decimal   CurrentYearPlanProductionValue { get {

               var one= OneProductionValue.HasValue ? OneProductionValue.Value:0;
               var two = TwoProductionValue.HasValue ? TwoProductionValue.Value:0;
               var thre = ThreeProductionValue.HasValue ? ThreeProductionValue.Value:0;
               var four = FourProductionValue.HasValue ? FourProductionValue.Value:0;
               var five = FiveProductionValue.HasValue ? FiveProductionValue.Value:0;
               var six = SixProductionValue.HasValue ? SixProductionValue.Value:0;
               var seven = SevenProductionValue.HasValue ? SevenProductionValue.Value:0;
               var eight = EightProductionValue.HasValue ? EightProductionValue.Value:0;
               var nine = NineProductionValue.HasValue ? NineProductionValue.Value:0;
               var ten = TenProductionValue.HasValue ? TenProductionValue.Value:0;
               var eleven = ElevenProductionValue.HasValue ? ElevenProductionValue.Value:0;
               var twelve = TwelveProductionValue.HasValue ? TwelveProductionValue.Value:0;
               return (one+two+thre+four+five+six+seven+eight+eleven+twelve+ nine+ ten)/10000;
            } }


        /// <summary>
        /// 一月份工程量
        /// </summary>
        public decimal? OneQuantity { get; set; }
        /// <summary>
        /// 一月份产值
        /// </summary>
        public decimal? OneProductionValue { get; set; }
        /// <summary>
        /// 二月份工程量
        /// </summary>
        public decimal? TwoQuantity { get; set; }
        /// <summary>
        /// 二月份产值
        /// </summary>
        public decimal? TwoProductionValue { get; set; }


        public decimal? ThreeQuantity { get; set; }
        public decimal? ThreeProductionValue { get; set; }
        public decimal? FourQuantity { get; set; }
        public decimal? FourProductionValue { get; set; }
        public decimal? FiveQuantity { get; set; }
        public decimal? FiveProductionValue { get; set; }
        public decimal? SixQuantity { get; set; }
        public decimal? SixProductionValue { get; set; }
        public decimal? SevenQuantity { get; set; }
        public decimal? SevenProductionValue { get; set; }
        public decimal? EightQuantity { get; set; }
        public decimal? EightProductionValue { get; set; }
        public decimal? NineQuantity { get; set; }
        public decimal? NineProductionValue { get; set; }
        public decimal? TenQuantity { get; set; }
        public decimal? TenProductionValue { get; set; }
        public decimal? ElevenQuantity { get; set; }
        public decimal? ElevenProductionValue { get; set; }
        public decimal? TwelveQuantity { get; set; }
        public decimal? TwelveProductionValue { get; set; }

        #endregion

    }



    public class ReportDetails 
    {
        /// <summary>
        /// 施工分类Id
        /// </summary>
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// keyId
        /// </summary>
        public string? KeyId { get; set; }

        /// <summary>
        /// Pid
        /// </summary>
        public string? Pid { get; set; }

        /// <summary>
        /// 施工分类名称
        /// </summary>
        public string? ProjectWBSName { get; set; }

        /// <summary>
        /// 单价(元)
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? ContractQuantity { get; set; }

        /// <summary>
        /// 合同额
        /// </summary>
        public decimal? ContractAmount { get; set; }


        /// <summary>
        /// 子级集合
        /// </summary>
        ////public TTreeDetail[] Children { get; set; } = new TTreeDetail[0];

        /////// <summary>
        /////// 月报明细
        /////// </summary>
        ////public List<TDetail> ReportDetails { get; set; } = new List<TDetail>();
    }
}
