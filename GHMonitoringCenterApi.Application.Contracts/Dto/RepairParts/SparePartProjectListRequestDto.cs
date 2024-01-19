using GHMonitoringCenterApi.Domain.Enums;
using MiniExcelLibs.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{


    /// <summary>
    /// 修理项目清单请求DTO
    /// </summary>
    public class SparePartProjectListRequestDto
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        [ExcelColumnName("入账年度")]
        public string? ProjectNo { get; set; }

        /// <summary>
        /// 船名
        /// </summary>
        [ExcelColumnName("船名")]
        public string? ShipName { get; set; }

        /// <summary>
        /// 项目摘要
        /// </summary>
        [ExcelColumnName("项目摘要")]
        public string? ProjectDesc { get; set; }

        /// <summary>
        /// 修理类别
        /// </summary>
        [ExcelColumnName("修理类别")]
        public string? ShipRepairType { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [ExcelColumnName("供应商")]
        public string? Supplier { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        [ExcelColumnName("合同名称")]
        public string? ContractName { get; set; }

        /// <summary>
        /// 合同签订日期
        /// </summary>
        [ExcelColumnName("合同签订日期")]
        public DateTime? ContractTime { get; set; }

        /// <summary>
        /// 合同额（元）
        /// </summary>
        [ExcelColumnName("合同额（元）")]
        public decimal? ContractMoney { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [ExcelColumnName("合同编号")]
        public string? ContractNo { get; set; }

        /// <summary>
        /// 实施开始时间
        /// </summary>
        [ExcelColumnName("实施开始日期")]
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 完工日期
        /// </summary>
        [ExcelColumnName("完工日期")]
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 结算额（元 含税）
        /// </summary>
        [ExcelColumnName("结算额(元，含税)")]
        public decimal? SettlementAmount { get; set; }

        /// <summary>
        /// 报账单号
        /// </summary>
        [ExcelColumnName("报账单号")]
        public string? ExpenseNO { get; set; }

        /// <summary>
        /// 付款单号
        /// </summary>
        [ExcelColumnName("付款单号")]
        public string? PayNo { get; set; }

        /// <summary>
        /// 招议标情况
        /// </summary>
        [ExcelColumnName("招议标情况")]
        public string? BiddingSituation { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]
        public string? Remark { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [ExcelColumnName("经办人")]
        public string? ResponsiblePerson { get; set; }
    }
}
