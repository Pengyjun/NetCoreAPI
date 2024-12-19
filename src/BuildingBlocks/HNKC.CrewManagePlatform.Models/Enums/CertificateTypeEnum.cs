using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 证书类型
    /// </summary>
    public enum CertificateTypeEnum
    {
        /// <summary>
        /// 高级技师
        /// </summary>
        [Description("高级技师")]
        HighJs = 0,
        /// <summary>
        /// 技师
        /// </summary>
        [Description("技师")]
        Js = 1,
        /// <summary>
        /// 高级工
        /// </summary>
        [Description("高级工")]
        HighWork = 2,
        /// <summary>
        /// 中级工
        /// </summary>
        [Description("中级工")]
        CenterWork = 3,
        /// <summary>
        /// 初级工
        /// </summary>
        [Description("初级工")]
        Elementary = 4
    }
}
