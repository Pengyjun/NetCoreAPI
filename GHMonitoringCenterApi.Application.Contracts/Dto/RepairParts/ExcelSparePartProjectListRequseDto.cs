using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 备件项目清单excel导入
    /// </summary>
    public class ExcelSparePartProjectListRequseDto
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        [ExcelColumnName("项目编号")]
        public string? ProjectNo { get; set; }

        /// <summary>
        /// 备件类别
        /// </summary>
        [ExcelColumnName("备件类别")]
        public string? SparePartType { get; set; }

        /// <summary>
        /// 项目摘要
        /// </summary>
        [ExcelColumnName("项目摘要")]
        public string? ProjectDesc { get; set; }


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
        /// 合同额（元）
        /// </summary>
        [ExcelColumnName("合同额（元）")]
        public decimal? ContractMoney { get; set; }

        /// <summary>
        /// 合同签订日期
        /// </summary>
        [ExcelColumnName("合同签订日期")]
        public DateTime? ContractTime { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [ExcelColumnName("合同编号")]
        public string? ContractNo { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        [ExcelColumnName("结算金额")]
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
        /// 经办人
        /// </summary>
        [ExcelColumnName("经办人")]
        public string? ResponsiblePerson { get; set; }
    }
}
