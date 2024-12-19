using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 考核枚举
    /// </summary>
    public enum CheckEnum
    {
        /// <summary>
        /// 优秀
        /// </summary>
        [Description("优秀")]
        Excellent = 1,
        /// <summary>
        /// 称职
        /// </summary>
        [Description("称职")]
        Competent = 2,
        /// <summary>
        /// 基本称职
        /// </summary>
        [Description("基本称职")]
        BasiCcompetent = 3,
        /// <summary>
        /// 不称职
        /// </summary>
        [Description("不称职")]
        InBasicCompetent = 4
    }
}
