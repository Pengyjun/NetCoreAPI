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
    public class ProjectAdjustProductionValueResponseDto
    {
        public Guid ProjectId { get; set; }
        public Guid WbsId { get; set; }
        public string NodeId { get; set; }
        public string PNodeId { get; set; }
        public string ConstructionClassificationName { get; set; }
        public string ConstructionType  { get; set; }
        public string ConstructionTypeName  { get; set; }
        public string ProductionProperty { get; set; }
        public string ProductionPropertyName { get; set; }
        public string ResourceName { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// 原单价
        /// </summary>
        public decimal SourceUnitPrice { get; set; }
        /// <summary>
        /// 实际单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 原工程量
        /// </summary>
        public decimal SourceWorkQuantities { get; set; }
        /// <summary>
        /// 实际工程量
        /// </summary>
        public decimal  WorkQuantities { get; set; }
        /// <summary>
        /// 原产值
        /// </summary>
        public decimal SourceProductionValue { get; set; }
        /// <summary>
        /// 实际产值
        /// </summary>
        public decimal ProductionValue { get; set; }
        /// <summary>
        /// 原外包支出
        /// </summary>
        public decimal SourceOutsourcingExpenditure { get; set; }
        /// <summary>
        /// 实际外包支出
        /// </summary>
        public decimal OutsourcingExpenditure { get; set; }

        /// <summary>
        /// 是否是新数据   如果是新增的资源 此值传1  如果是修改的此值传2   如果已经存在的此值是0
        /// </summary>
        public int IsNew { get; set; }

        public List<ProjectAdjustProductionValueResponseDto> Childs { get; set; }
    }

}
