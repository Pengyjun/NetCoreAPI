using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 修理项目清单表
    /// </summary>

    [SugarTable("t_repairprojectlist", IsDisabledDelete = true)]
    public class RepairProjectList:BaseEntity<Guid>
    {
        /// <summary>
        /// 入账年度
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ProjectNo { get; set; }

        /// <summary>
        /// 船名
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ShipName { get; set; }

        /// <summary>
        /// 项目摘要
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ProjectDesc { get; set; }

        /// <summary>
        /// 修理类别
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ShipRepairType { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Supplier { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ContractName { get; set; }

        /// <summary>
        /// 合同签订日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractTime { get; set; }

        /// <summary>
        /// 合同额（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)", DefaultValue = "0")]
        public decimal? ContractMoney { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ContractNo { get; set; }

        /// <summary>
        /// 实施开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 完工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 结算额（元 含税）
        /// </summary>

        [SugarColumn(ColumnDataType = "decimal(18,6)", DefaultValue = "0")]
        public decimal? SettlementAmount { get; set; }

        /// <summary>
        /// 报账单号
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ExpenseNO { get; set; }

        /// <summary>
        /// 付款单号
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? PayNo { get; set; }

        /// <summary>
        /// 招议标情况
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? BiddingSituation { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Remark { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ResponsiblePerson { get; set; }

    }
}
