using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport
{

    /// <summary>
    /// 调整开累数响应DTO
    /// </summary>
    public class ProjectAdjustResponseDto
    {
       
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int? DateMonth { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string? Current { get; set; }

        public decimal ExchangeRate { get; set; }
        //public decimal? TaxRate { get; set; }

        public List<ProjectAdjustProductionValueResponseDto> projectAdjustProductionValueResponseDtos { get; set; }

    }



    public class ProjectAdjustItemResponseDto 
    {
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 原工程量
        /// </summary>
        public decimal SourceTotalWorkQuantities { get; set; }
        /// <summary>
        /// 实际工程量
        /// </summary>
        public decimal? TotalWorkQuantities { get; set; }


        /// <summary>
        /// 原产值（人民币  欧元  美元  ）
        /// </summary>
        public decimal? SourceTotalProductionValue { get; set; }
        /// <summary>
        /// 实际产值（人民币  欧元  美元  ）
        /// </summary>
        public decimal TotalProductionValue { get; set; }


        /// <summary>
        /// 原产值（人民币  ）
        /// </summary>
        public decimal? SourceTotalRmbProductionValue { get; set; }
        /// <summary>
        /// 实际产值（人民币  ）
        /// </summary>
        public decimal? TotalRmbProductionValue { get; set; }


        /// <summary>
        /// 原外包支出（人民币  欧元  美元  ）
        /// </summary>
        public decimal? SourceTotalOutsourcingExpenditure { get; set; }
        /// <summary>
        /// 实际外包支出（人民币  欧元  美元  ）
        /// </summary>
        public decimal? TotalOutsourcingExpenditure { get; set; }



        /// <summary>
        /// 原外包支出（人民币   ）
        /// </summary>
        public decimal? SourceTotalRmbOutsourcingExpenditure { get; set; }
        /// <summary>
        /// 实际外包支出（人民币）
        /// </summary>
        public decimal? TotalRmbOutsourcingExpenditure { get; set; }

        public List<ProjectAdjustProductionValueResponseDto> projectAdjustProductionValueResponseDtos { get; set; }
    }
}
