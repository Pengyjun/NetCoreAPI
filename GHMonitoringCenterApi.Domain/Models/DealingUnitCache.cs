using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 往来单位表  中间缓存表
    /// </summary>
    [SugarTable("t_dealingunitcache", IsDisabledDelete = true)]
    public class DealingUnitCache:BaseEntity<Guid>
    {
        /// <summary>
        /// pomId
        /// </summary>
        [SugarColumn(Length =36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 往来单位主数据编码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZBP { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZORG { get; set; }
        /// <summary>
        /// 名称（中文）
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? ZBPNAME_ZH { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZUSCC { get; set; }
        /// <summary>
        /// 名称（英文）
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? ZBPNAME_EN { get; set; }
        /// <summary>
        /// 纳税人识别号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZTRNO { get; set; }
        /// <summary>
        /// 工商注册号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZBRNO { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZIDNO { get; set; }
        /// <summary>
        /// 往来单位状态
        /// </summary>
        [SugarColumn(Length = 20)]
        public string? ZBPSTATE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDataType ="text")]
        public string? Remark { get; set; }
    }
}
