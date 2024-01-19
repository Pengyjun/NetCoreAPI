using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 保存修理项目清单请求Dto
    /// </summary>
    public class SaveRepairItemsListRequestDto
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
        /// 修理类别
        /// </summary>
        public string? ShipRepairType { get; set; }

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
        /// 实施开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 结算额（元 含税）
        /// </summary>
        public decimal? SettlementAmount { get; set; }

        /// <summary>
        /// 报账单号
        /// </summary>
        public string? ExpenseNO { get; set; }

        /// <summary>
        /// 付款单号
        /// </summary>
        public string? PayNo { get; set; }

        /// <summary>
        /// 招议标情况
        /// </summary>
        public string? BiddingSituation { get; set; }

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
