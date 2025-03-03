using GHMonitoringCenterApi.Domain.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos
{
    /// <summary>
    /// 公司产值信息
    /// </summary>
    public class CompanyProductionValueInfoResponseDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? DateDay { get; set; }

        ///// <summary>
        ///// 第一月完成产值
        ///// </summary>

        //public decimal? OneCompleteProductionValue { get; set; }
        ///// <summary>
        ///// 第二月完成产值
        ///// </summary>

        //public decimal? TwoCompleteProductionValue { get; set; }

        //public decimal? ThreeCompleteProductionValue { get; set; }

        //public decimal? FourCompleteProductionValue { get; set; }
        //public decimal? FiveCompleteProductionValue { get; set; }

        //public decimal? SixCompleteProductionValue { get; set; }

        //public decimal? SevenCompleteProductionValue { get; set; }

        //public decimal? EightCompleteProductionValue { get; set; }

        //public decimal? NineCompleteProductionValue { get; set; }

        //public decimal? TenCompleteProductionValue { get; set; }

        //public decimal? ElevenCompleteProductionValue { get; set; }

        //public decimal? TwelveCompleteProductionValue { get; set; }

        /// <summary>
        /// 第一月计划产值
        /// </summary>
        public decimal? OnePlanProductionValue { get; set; }
        /// <summary>
        /// 第二月计划产值
        /// </summary>
        public decimal? TwoPlanProductionValue { get; set; }

        /// <summary>
        /// 第三月计划产值
        /// </summary>
        public decimal? ThreePlaProductionValue { get; set; }

        /// <summary>
        /// 第四月计划产值
        /// </summary>
        public decimal? FourPlaProductionValue { get; set; }

        /// <summary>
        /// 第五月计划产值
        /// </summary>
        public decimal? FivePlaProductionValue { get; set; }

        /// <summary>
        /// 第六月计划产值
        /// </summary>
        public decimal? SixPlaProductionValue { get; set; }

        /// <summary>
        /// 第七月计划产值
        /// </summary>
        public decimal? SevenPlaProductionValue { get; set; }

        /// <summary>
        /// 第八月计划产值
        /// </summary>
        public decimal? EightPlaProductionValue { get; set; }

        /// <summary>
        /// 第九月计划产值
        /// </summary>
        public decimal? NinePlaProductionValue { get; set; }

        /// <summary>
        /// 第十月计划产值
        /// </summary>
        public decimal? TenPlaProductionValue { get; set; }

        /// <summary>
        /// 第十一月计划产值
        /// </summary>
        public decimal? ElevenPlaProductionValue { get; set; }

        /// <summary>
        /// 第十二月计划产值
        /// </summary>
        public decimal? TwelvePlaProductionValue { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public List<MonthlyDataProductionValue> MonthlyDatas { get; set; }

        /// <summary>
        /// 调整值列表
        /// </summary>
        public List<CompanyAdjustmentValue> adjustmentValues { get; set; }

        /// <summary>
        /// 每年指标
        /// </summary>
        //public decimal? YearIndex { get; set; }
    }


    public class MonthlyDataProductionValue
    {
        public int Month { get; set; }
        public decimal Total { get; set; }

        /// <summary>
        /// 实际日期
        /// </summary>
        //public int ActualDate { get; set; }
    }
}
