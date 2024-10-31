using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCDataSecurityApi.Application.Contracts.Dto.IncrementalData
{
    /// <summary>
    /// 
    /// </summary>
    public class DataInterfaceResponseDto
    {
        /// <summary>
        /// 外部系统ID
        /// </summary>
        public string? AppSystemId { get; set; }
        /// <summary>
        /// 接口主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// appKey
        /// </summary>
        public string? AppKey { get; set; }
        /// <summary>
        /// 系统标识
        /// </summary>
        //public string? SystemIdentity { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string? InterfaceName { get; set; }
        /// <summary>
        /// 接口授权码
        /// </summary>
        public string? AppinterfaceCode { get; set; }
        /// <summary>
        /// 接口是否启用
        /// </summary>
        public int? IsEnable { get; set; }
        /// <summary>
        /// 是否加密 1  加密  0未加密 
        /// </summary>
        public int? IsEncrypt { get; set; }
        /// <summary>
        /// 接口访问AP限制
        /// </summary>
        public string? AccessRestrictedIP { get; set; }
    }
}
