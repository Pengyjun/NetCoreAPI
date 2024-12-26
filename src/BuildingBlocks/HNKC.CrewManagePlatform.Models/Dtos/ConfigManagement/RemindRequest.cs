using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class RemindRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 提醒类型  1合同 2 证书
        /// </summary>
        public CertificatesEnum Types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? TypesName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int RemindType { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// 是否启用 启用 1
        /// </summary>
        public int Enable { get; set; }
    }
}
