using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    ///转换汇率金额Dto
    /// </summary>
    public class ExchangeRateAmountDto
    {
        /// <summary>
        /// /币种Id
        /// </summary>
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal CurrencyExchangeRate { get; set; }

        /// <summary>
        /// 币种金额
        /// </summary>
        public decimal CurrencyAmount { get; set; }

        /// <summary>
        /// 换算后人民币金额
        /// </summary>
        public decimal ExchangeRMBAmount { get; set; }

        
    }
}
