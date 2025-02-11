using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 船舶完成产量产值表
    /// </summary>
    [SugarTable("t_shipcompleteproduction", IsDisabledDelete = true)]
    public class ShipCompleteProduction : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ShipName { get; set; }


        /// <summary>
        /// 工程量  单位 万m³
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal QuantityWork { get; set; }

        /// <summary>
        /// 单价  单位元m³
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? DateDay { get; set; }

        /// <summary>
        /// 完成产值 
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal CompleteOutputValue { get; set; }
        /// <summary>
        /// 计划产值 
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal PlanOutputValue { get; set; }

      /// <summary>
      /// 产值偏差  完成产值-计划产值  单位（万元）
      /// </summary>
        public decimal DiffProductionValue { get; set; }

        /// <summary>
        /// 偏差原因
        /// </summary>
        [SugarColumn(Length = 1024)]
        public string? Reason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length =512)]
        public string? Remark { get; set; }
    }
}
