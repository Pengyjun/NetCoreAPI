using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCInterfaceApi.Application.Contracts.Dto.IncrementalData
{
    /// <summary>
    /// 授权系统响应DTO
    /// </summary>
    public class SystemResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string? SystemName { get; set; }
        /// <summary>
        /// 系统简称
        /// </summary>
        public string? SystemShortName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int? Enable { get; set; }

        public string? SystemApiId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SystemApiName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 授权APPkey
        /// </summary>
        public string? AppKey { get; set; }
    }
}
