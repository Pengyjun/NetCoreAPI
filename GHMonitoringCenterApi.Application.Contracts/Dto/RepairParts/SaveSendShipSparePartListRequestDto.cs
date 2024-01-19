using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 保存发船备件清单请求Dto
    /// </summary>
    public class SaveSendShipSparePartListRequestDto
    {
        /// <summary>
		/// 请求类型  true是添加   false是修改
		/// </summary>
		public bool RequestType { get; set; }
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 单据日期
        /// </summary>
        public DateTime? DocumentDate { get; set; }
        /// <summary>
        /// 签收日期
        /// </summary>
        public DateTime? ReceivedOn { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public string? SourceType { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        public string? SourceNumber { get; set; }
        /// <summary>
        /// 备件类别
        /// </summary>
        public string? SparePartsType { get; set; }
        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparePartsName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string? SpecificationModel { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public string? MaterialQuality { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string? Unit { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string? SupplierName { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        public int? OutboundQuantity { get; set; }
        /// <summary>
        /// 出库单价
        /// </summary>
        public decimal? DeliveryUnitPrice { get; set; }
        /// <summary>
        /// 出库金额
        /// </summary>
        public decimal? OutboundAmount { get; set; }
        /// <summary>
        /// 领料单位编码
        /// </summary>
        public string? UnitCode { get; set; }
        /// <summary>
        /// 领料单位
        /// </summary>
        public string? MaterialRequisitionUnit { get; set; }

        /// <summary>
        /// 是否提交财务 1，是   2  否
        /// </summary>
        public string? SubmitFinance { get; set; }
        /// <summary>
        /// 提交财务时间
        /// </summary>
        public DateTime? SubmitFinanceTime { get; set; }
        /// <summary>
        /// 调整是否提交财务 1，是   2  否
        /// </summary>
        public string? AdjustSubmitFinance { get; set; }
        /// <summary>
        /// 调整提交财务时间
        /// </summary>
        public DateTime? AdjustSubmitFinanceTime { get; set; }
        /// <summary>
        /// 出库单据备注
        /// </summary>
        public string? OutboundRemarks { get; set; }
    }
}
