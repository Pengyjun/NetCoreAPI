using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Enums
{

    /// <summary>
    /// 日报进程步骤（1：施工日志已完成，2：文件已上传，3：日报数据已完成）
    /// </summary>
    public enum DayReportProcessStep
    {

        /// <summary>
        /// 施工日志已完成
        /// </summary>
        DayReportConstructionFinish = 1,

        /// <summary>
        /// 文件已上传
        /// </summary>
        DayReportUploadFileFinish = 2,

        /// <summary>
        /// 日报数据已完成
        /// </summary>
        DayReportFinish = 3
    }

    /// <summary>
    /// 页面入口（1：列表页入口，2：首页入口）
    /// </summary>
    public enum PageEnter
    {

        /// <summary>
        /// 列表页入口
        /// </summary>
        List = 1,

        /// <summary>
        /// 首页入口
        /// </summary>
        Index = 2
    }

    /// <summary>
    /// 业务数据模型状态(0:无状态,1:新增,2:更新,3:删除)
    /// </summary>
    public enum ModelState
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None=0,

        /// <summary>
        /// 新增
        /// </summary>
        Add = 1,

        /// <summary>
        /// 更新
        /// </summary>
        Update = 2,

        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3
    }

    /// <summary>
    /// 任务提交类型(1:新增任务,2:重置任务)
    /// </summary>
    public enum JobSubmitType
    {
        /// <summary>
        /// 新增任务
        /// </summary>
        AddJob = 1,

        /// <summary>
        /// 重置任务
        /// </summary>
        ResetJob = 2
    }

    /// <summary>
    /// 任务-业务数据请求类型(1:查找草稿数据，2：查找库中存储数据)
    /// </summary>
    public enum JobBizRequestType
    {
        /// <summary>
        /// 查找草稿数据
        /// </summary>
        FindJobBizData = 1,

        /// <summary>
        /// 查找库中存储数据
        /// </summary>
        FindDBBizData = 2
    }

    /// <summary>
    /// 填报状态（1：未填报，2：已填报）
    /// </summary>
    public enum FillReportStatus
    {
        /// <summary>
        /// 未填报
        /// </summary>
        UnReport=1,

        /// <summary>
        /// 已填报
        /// </summary>
        Reported= 2
    }

    /// <summary>
    /// 任务集合查找类型
    /// </summary>
    public enum JobsFindType
    {
        /// <summary>
        /// 审批人任务
        /// </summary>
        ApproverJobs=0,

        /// <summary>
        /// 所有任务
        /// </summary>
        AllJobs=100
    }

    /// <summary>
    /// 授权状态
    /// </summary>
    public enum AuthorizeState
    {
        /// <summary>
        /// 未授权
        /// </summary>
        [Description("未授权")]
        UnAuthorize = 1,

        /// <summary>
        /// 授权中
        /// </summary>
        [Description("授权中")]
        Authorizing = 2,

        /// <summary>
        /// 授权已过期
        /// </summary>
        [Description("授权已过期")]
        AuthorizeExpired =3
    }


}
