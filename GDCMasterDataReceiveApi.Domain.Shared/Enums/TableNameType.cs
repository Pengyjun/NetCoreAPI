using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Domain.Shared.Enums
{
    /// <summary>
    /// 表名枚举
    /// </summary>
    public enum TableNameType
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Description("t_user")]
        User = 1,
        /// <summary>
        /// 机构
        /// </summary>
        [Description("t_institution")]
        Institution = 2
    }
    /// <summary>
    /// 下发数据筛选类型
    /// </summary>
    public enum IssuedEType
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = 0,
        /// <summary>
        /// 项目类型(针对值域表)  中交业务类型
        /// </summary>
        CCCCPType = 1,

    }
    /// <summary>
    /// 表
    /// </summary>
    public enum TableEType
    {
        /// <summary>
        /// 值域表
        /// </summary>
        VDomain = 1
    }
}
