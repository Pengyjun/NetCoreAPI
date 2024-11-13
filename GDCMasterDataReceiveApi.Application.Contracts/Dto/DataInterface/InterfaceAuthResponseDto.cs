using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DataInterface
{
    /// <summary>
    /// 接口授权响应DTO
    /// </summary>
    public class InterfaceAuthResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
        public string? PId { get; set; }
        /// <summary>
        /// 系统标识
        /// </summary>
        public string? SystemIdentity { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        [DisplayName("系统名称")]
        public string? SystemName { get; set; }
        /// <summary>
        /// 系统授权码
        /// </summary>
        [DisplayName("系统授权码")]
        public string? SystemAppKey { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        [DisplayName("接口名称")]
        public string? InterfaceName { get; set; }
        /// <summary>
        /// 接口中文名称
        /// </summary>
        [DisplayName("接口中文名称")]
        public string? InterfaceZhName { get; set; }
        /// <summary>
        /// 接口授权码
        /// </summary>
        [DisplayName("接口授权码")]
        public string? InterfaceAuthCode { get; set; }
        /// <summary>
        ///接口访问IP限制 * 全部 
        /// </summary>
        [DisplayName("接口访问IP限制(*:全部)")]
        public string? AccessRestrictedIP { get; set; }

        /// <summary>
        /// 接口是否启用 1是
        /// </summary>
        [DisplayName("接口是否启用 1是")]
        public int? Enable { get; set; }

        /// <summary>
        /// 接口返回值是否加密
        /// </summary>
        [DisplayName("接口返回值是否加密 1是")]
        public int? ReturnDataEncrypt { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string? Remark { get; set; }
    }
}
