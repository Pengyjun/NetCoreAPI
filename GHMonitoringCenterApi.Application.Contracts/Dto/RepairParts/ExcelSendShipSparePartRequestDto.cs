using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 导入发船备件清单请求Dto
    /// </summary>
    public class ExcelSendShipSparePartRequestDto
    {
        /// <summary>
        /// 单据日期
        /// </summary>
        [ExcelColumnName("单据日期")]
        public DateTime? DocumentDate { get; set; }
        /// <summary>
        /// 签收日期
        /// </summary>
        [ExcelColumnName("签收日期")]
        public DateTime? ReceivedOn { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        [ExcelColumnName("来源类型")]
        public string? SourceType { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        [ExcelColumnName("来源编号")]
        public string? SourceNumber { get; set; }
        /// <summary>
        /// 备件类别
        /// </summary>
        [ExcelColumnName("备件类别")]
        public string? SparePartsType { get; set; }
        /// <summary>
        /// 备件名称
        /// </summary>
        [ExcelColumnName("备件名称")]
        public string? SparePartsName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [ExcelColumnName("规格型号")]
        public string? SpecificationModel { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        [ExcelColumnName("材质")]
        public string? MaterialQuality { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [ExcelColumnName("单位")]
        public string? Unit { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        [ExcelColumnName("供应商名称")]
        public string? SupplierName { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        [ExcelColumnName("出库数量")]
        public int? OutboundQuantity { get; set; }
        /// <summary>
        /// 出库单价
        /// </summary>
        [ExcelColumnName("出库单价")]
        public decimal? DeliveryUnitPrice { get; set; }
        /// <summary>
        /// 出库金额
        /// </summary>
        [ExcelColumnName("出库金额")]
        public decimal? OutboundAmount { get; set; }
        /// <summary>
        /// 领料单位编码
        /// </summary>
        [ExcelColumnName("领料单位编码")]
        public string? UnitCode { get; set; }
        /// <summary>
        /// 领料单位
        /// </summary>
        [ExcelColumnName("领料单位")]
        public string? MaterialRequisitionUnit { get; set; }

        /// <summary>
        /// 是否提交财务
        /// </summary>
        [ExcelColumnName("是否提交财务")]
        public string? SubmitFinance { get; set; }
        /// <summary>
        /// 提交财务时间
        /// </summary>
        [ExcelColumnName("提交财务时间")]
        public DateTime? SubmitFinanceTime { get; set; }
        /// <summary>
        /// 调整是否提交财务
        /// </summary>
        [ExcelColumnName("调整是否提交财务")]
        public string? AdjustSubmitFinance { get; set; }
        /// <summary>
        /// 调整提交财务时间
        /// </summary>
        [ExcelColumnName("调整提交财务时间")]
        public DateTime? AdjustSubmitFinanceTime { get; set; }
        /// <summary>
        /// 出库单据备注
        /// </summary>
        [ExcelColumnName("出库单据备注")]
        public string? OutboundRemarks { get; set; }
    }
}
