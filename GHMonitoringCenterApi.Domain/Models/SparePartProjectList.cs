﻿using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 备件项目清单表
    /// </summary>

    [SugarTable("t_sparepartprojectlist", IsDisabledDelete = true)]
    public class SparePartProjectList : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ProjectNo { get; set; }

        /// <summary>
        /// 备件类别
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? SparePartType { get; set; }

        /// <summary>
        /// 项目摘要
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ProjectDesc { get; set; }


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
        /// 合同额（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)", DefaultValue = "0")]
        public decimal? ContractMoney { get; set; }

        /// <summary>
        /// 合同签订日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ContractTime { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ContractNo { get; set; }

        /// <summary>
        /// 结算金额
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
        /// 经办人
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ResponsiblePerson { get; set; }
    }
}
