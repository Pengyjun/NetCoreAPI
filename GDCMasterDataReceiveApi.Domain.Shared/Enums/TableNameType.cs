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
}
