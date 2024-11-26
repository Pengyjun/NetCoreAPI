using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Domain.Shared.Enums
{
    /// <summary>
    /// 数据治理枚举
    /// </summary>
    public class DataGovernment
    {
    }
    /// <summary>
    /// 规则类型
    /// </summary>
    public enum GruleType
    {
        /// <summary>
        /// 完整性
        /// </summary>
        [Description("完整性")]
        WZX = 1,
        /// <summary>
        /// 唯一性
        /// </summary>
        [Description("唯一性")]
        WYX = 2
    }
    /// <summary>
    /// 规则级别
    /// </summary>
    public enum GruleGradeType
    {
        /// <summary>
        /// 低
        /// </summary>
        [Description("低")]
        Low = 1,
        /// <summary>
        /// 中
        /// </summary>
        [Description("中")]
        In = 2,
        /// <summary>
        /// 高
        /// </summary>
        [Description("高")]
        High = 3
    }
}
