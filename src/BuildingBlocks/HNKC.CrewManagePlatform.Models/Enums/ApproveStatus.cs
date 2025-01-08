using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ApproveStatus
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Description("审批中")]
        PExamination = 0,
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Pass = 1,
        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        Reject = 2
    }
}
