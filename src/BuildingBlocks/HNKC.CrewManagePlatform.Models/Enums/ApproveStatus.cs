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

    /// <summary>
    /// 审批操作状态
    /// </summary>
    public enum ApproveOperateStatus
    {
        /// <summary>
        /// 发起申请
        /// </summary>
        [Description("发起申请")]
        Initiate = -1,

        /// <summary>
        /// 待审批
        /// </summary>
        [Description("待审批")]
        Pending = 0,

        /// <summary>
        /// 通过
        /// </summary>
        [Description("审批通过")]
        Pass = 1,
        /// <summary>
        /// 未通过
        /// </summary>
        [Description("审批拒绝")]
        Reject = 2
    }
}
