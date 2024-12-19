using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 签证类型
    /// </summary>
    public enum VisaTypeEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        Normal = 0,
        /// <summary>
        /// 入境签证
        /// </summary>
        [Description("入境签证")]
        Entry = 1,
        /// <summary>
        /// 过境签证
        /// </summary>
        [Description("过境签证")]
        Transit = 2
    }
}
