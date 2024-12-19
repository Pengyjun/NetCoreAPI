using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 服务薄枚举
    /// </summary>
    public enum ServiceBookEnum
    {
        /// <summary>
        /// 国内
        /// </summary>
        [Description("国内")]
        InChina = 0,
        /// <summary>
        /// 国外
        /// </summary>
        [Description("国外")]
        Abroad = 1
    }
}
