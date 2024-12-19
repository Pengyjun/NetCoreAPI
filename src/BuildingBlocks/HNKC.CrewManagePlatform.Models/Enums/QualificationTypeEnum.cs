using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 学历类型
    /// </summary>
    public enum QualificationTypeEnum
    {
        /// <summary>
        /// 全日制
        /// </summary>
        [Description("全日制")]
        FullTime = 1,
        /// <summary>
        /// 非全日制
        /// </summary>
        [Description("非全日制")]
        NoFullTime = 2
    }
    /// <summary>
    /// 学历
    /// </summary>
    public enum QualificationEnum
    {
        /// <summary>
        /// 高中
        /// </summary>
        [Description("高中")]
        GaoZhong = 0,
        /// <summary>
        /// 专科
        /// </summary>
        [Description("专科")]
        ZhuanKe = 1,
        /// <summary>
        /// 本科
        /// </summary>
        [Description("本科")]
        BenKe = 2,
        /// <summary>
        /// 硕士
        /// </summary>
        [Description("硕士")]
        ShuoShi = 3,
        /// <summary>
        /// 博士
        /// </summary>
        [Description("博士")]
        BoShi = 4
    }
}
