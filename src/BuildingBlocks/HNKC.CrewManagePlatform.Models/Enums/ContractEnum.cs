using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 合同枚举
    /// </summary>
    public enum ContractEnum
    {
        /// <summary>
        /// 固定期限
        /// </summary>
        [Description("固定期限")]
        FixedTerm = 0,
        /// <summary>
        /// 无固定期限
        /// </summary>
        [Description("无固定期限")]
        NoFixedTerm = 1
    }
}
