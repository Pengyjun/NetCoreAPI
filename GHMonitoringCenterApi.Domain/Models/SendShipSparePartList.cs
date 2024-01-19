using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 发船备件清单表
    /// </summary>

    [SugarTable("t_sendshipsparepartlist", IsDisabledDelete = true)]
    public class SendShipSparePartList:BaseEntity<Guid>
    {
        /// <summary>
        /// 单据日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? DocumentDate { get; set; }
        /// <summary>
        /// 签收日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ReceivedOn { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SourceType { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SourceNumber { get; set; }
        /// <summary>
        /// 备件类别
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SparePartsType { get; set; }
        /// <summary>
        /// 备件名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SparePartsName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SpecificationModel { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? MaterialQuality { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Unit { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? SupplierName { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        [SugarColumn(Length = 255)]
        public int? OutboundQuantity { get; set; }
        /// <summary>
        /// 出库单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)", DefaultValue = "0")]
        public decimal? DeliveryUnitPrice { get; set; }
        /// <summary>
        /// 出库金额
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)", DefaultValue = "0")]
        public decimal? OutboundAmount { get; set; }
        /// <summary>
        /// 领料单位编码
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? UnitCode { get; set; }
        /// <summary>
        /// 领料单位
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? MaterialRequisitionUnit { get; set; }

        /// <summary>
        /// 是否提交财务
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? SubmitFinance { get; set; }
        /// <summary>
        /// 提交财务时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? SubmitFinanceTime { get; set; }
        /// <summary>
        /// 调整是否提交财务
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? AdjustSubmitFinance { get; set; }
        /// <summary>
        /// 调整提交财务时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? AdjustSubmitFinanceTime { get; set; }
        /// <summary>
        /// 出库单据备注
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? OutboundRemarks { get; set; }


    }
}
