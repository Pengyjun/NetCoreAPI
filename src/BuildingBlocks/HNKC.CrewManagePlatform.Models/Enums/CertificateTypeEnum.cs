using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 技能证书类型
    /// </summary>
    public enum CertificateTypeEnum
    {
        /// <summary>
        /// 高级技师
        /// </summary>
        [Description("高级技师")]
        HighJs = 1,
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
        Elementary = 4,
        /// <summary>
        /// 电焊证
        /// </summary>
        [Description("电焊证")]
        DianHan = 5,
        /// <summary>
        /// 防辐射证
        /// </summary>
        [Description("防辐射证")]
        FangFuShe = 6
    }
    /// <summary>
    /// 证书
    /// </summary>
    public enum CertificatesEnum
    {
        /// <summary>
        /// 第一适任证
        /// </summary>
        [Description("第一适任证")]
        FCertificate = 1,
        /// <summary>
        /// 第二适任证
        /// </summary>
        [Description("第二适任证")]
        SCertificate = 2,
        /// <summary>
        /// 培训合格证
        /// </summary>
        [Description("培训合格证")]
        PXHGZ = 3,
        /// <summary>
        /// 健康证
        /// </summary>
        [Description("健康证")]
        JKZ = 4,
        /// <summary>
        /// 海员证
        /// </summary>
        [Description("海员证")]
        HYZ = 5,
        /// <summary>
        /// 护照
        /// </summary>
        [Description("护照")]
        HZ = 6
    }
}
