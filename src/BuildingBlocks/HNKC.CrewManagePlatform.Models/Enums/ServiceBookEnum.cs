using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 服务薄枚举
    /// </summary>
    public enum ServiceBookEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        Normal = 0,
        /// <summary>
        /// 国内
        /// </summary>
        [Description("国内")]
        InChina = 1,
        /// <summary>
        /// 国外
        /// </summary>
        [Description("国外")]
        Abroad = 2
    }
}
