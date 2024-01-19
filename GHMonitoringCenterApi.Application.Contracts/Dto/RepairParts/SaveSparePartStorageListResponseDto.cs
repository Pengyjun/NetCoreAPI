using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 保存备件仓储运输清单请求Dto
    /// </summary>
    public class SaveSparePartStorageListResponseDto
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
        /// 项目编号
        /// </summary>
        public string? ProjectNo { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 项目摘要
        /// </summary>
        public string? ProjectDesc { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string? Supplier { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        public string? ContractName { get; set; }
        /// <summary>
        /// 合同签订日期
        /// </summary>
        public DateTime? ContractTime { get; set; }
        /// <summary>
        /// 合同额（元）
        /// </summary>
        public decimal? ContractMoney { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string? ContractNo { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal? SettlementAmount { get; set; }
        /// <summary>
        /// 报账单号
        /// </summary>
        public string? ExpenseNO { get; set; }
        /// <summary>
        /// 付款单号
        /// </summary>
        public string? BidNegotiationSituation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string? ResponsiblePerson { get; set; }
    }
}
