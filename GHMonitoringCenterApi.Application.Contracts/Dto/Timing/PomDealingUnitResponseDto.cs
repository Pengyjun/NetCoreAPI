using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{

    /// <summary>
    /// 往来单位响应数据DTO
    /// </summary>
    public class PomDealingUnitResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// PomId
        /// </summary>
        public Guid? PomId { get; set; }
        /// <summary>
        /// 往来单位主数据编码
        /// </summary>
        public string? ZBP { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? ZORG { get; set; }
        /// <summary>
        /// 名称（中文）
        /// </summary>
        public string? ZBPNAME_ZH { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string? ZUSCC { get; set; }
        /// <summary>
        /// 名称（英文）
        /// </summary>
        public string? ZBPNAME_EN { get; set; }
        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public string? ZTRNO { get; set; }
        /// <summary>
        /// 工商注册号
        /// </summary>
        public string? ZBRNO { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string? ZIDNO { get; set; }
        /// <summary>
        /// 往来单位状态
        /// </summary>
        public string? ZBPSTATE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string? UpdatedBy { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

    }
}
