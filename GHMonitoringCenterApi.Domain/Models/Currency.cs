using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 币种表
    /// </summary>
    [SugarTable("t_currency", IsDisabledDelete = true)]
    public class Currency:BaseEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 货币数字代码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Zcurrencycode { get; set; }
        /// <summary>
        /// 货币名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Zcurrencyname { get; set; }
        /// <summary>
        /// 货币字母代码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Zcurrencyalphabet { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Zremarks { get; set; }
    }
}
