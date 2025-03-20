using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 月报明细开累数调整表
    /// </summary>
    [SugarTable("t_projectadjustproductionvaluedetails", IsDisabledDelete = true)]
    public class ProjectAdjustProductionValueDetails : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 月报明细ID
        /// </summary>
        public Guid MonthDetailId { get; set; }
       
        /// <summary>
        /// WBSid
        /// </summary>
        public Guid? WbsId { get; set; }
        /// <summary>
        /// 节点ID
        /// </summary>
        public string? NodeId { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public string? PNodeId { get; set; }
        /// <summary>
        /// 施工分类名称
        /// </summary>
        public string? ConstructionClassificationName { get; set; }
        /// <summary>
        /// 施工性质
        /// </summary>
        public string? ConstructionType { get; set; }
        /// <summary>
        /// 施工性质名称
        /// </summary>
        public string? ConstructionTypeName { get; set; }
        /// <summary>
        /// 产值属性
        /// </summary>
        public string? ProductionProperty { get; set; }
        /// <summary>
        /// 产值属性名称
        /// </summary>
        public string? ProductionPropertyName { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public string? ResourceId { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? ExchangeRate { get; set; }

        /// <summary>
        /// 原单价
        /// </summary>
        public decimal? SourceUnitPrice { get; set; }
        /// <summary>
        /// 实际单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 原工程量
        /// </summary>
        public decimal SourceWorkQuantities { get; set; }
        /// <summary>
        /// 实际工程量
        /// </summary>
        public decimal? WorkQuantities { get; set; }
        /// <summary>
        /// 原产值（人民币  欧元  美元  ）
        /// </summary>
        public decimal? SourceProductionValue { get; set; }
        /// <summary>
        /// 实际产值（人民币  欧元  美元  ）
        /// </summary>
        public decimal ProductionValue { get; set; }


        /// <summary>
        /// 原产值（人民币  ）
        /// </summary>
        public decimal? SourceRmbProductionValue { get; set; }
        /// <summary>
        /// 实际产值（人民币  ）
        /// </summary>
        public decimal? RmbProductionValue { get; set; }
        /// <summary>
        /// 原外包支出（人民币  欧元  美元  ）
        /// </summary>
        public decimal? SourceOutsourcingExpenditure { get; set; }
        /// <summary>
        /// 实际外包支出（人民币  欧元  美元  ）
        /// </summary>
        public decimal? OutsourcingExpenditure { get; set; }



        /// <summary>
        /// 原外包支出（人民币   ）
        /// </summary>
        public decimal? SourceRmbOutsourcingExpenditure { get; set; }
        /// <summary>
        /// 实际外包支出（人民币）
        /// </summary>
        public decimal? RmbOutsourcingExpenditure { get; set; }


        /// <summary>
        /// 是否是新数据   如果是新增的资源 此值传1  如果是修改的此值传2   如果已经存在的此值是0
        /// </summary>
        public int IsNew { get; set; }
    }

    /// <summary>
    /// 开累数调整主表
    /// </summary>
    [SugarTable("t_projectadjustproductionvalue", IsDisabledDelete = true)]
    public class ProjectAdjustProductionValue:BaseEntity<Guid>{

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
    }
}
