namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 月报编辑按钮权限控制 列表
    /// </summary>
    public class BtnEditMonthlyReportSearch
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 授权开始时间 针对日报
        /// </summary>
        public string? DailyStartTime { get; set; }
        /// <summary>
        /// 授权结束时间 针对日报
        /// </summary>
        public string? DailyEndTime { get; set; }
        /// <summary>
        /// true 启用
        /// </summary>
        public bool DailyReportEnableStatus { get; set; }
        /// <summary>
        /// true 启用
        /// </summary>
        public bool MonthReportEnableStatus { get; set; }
        /// <summary>
        /// 月报允许修改的月份 字符串拼接 202401,202501
        /// </summary>
        public string? MonthTime { get; set; }
        /// <summary>
        /// 月报允许修改的月份有效时间  默认三天  日报默认当前时间不做处理
        /// </summary>
        public string? MonthEffectiveTime { get; set; }
    }
    /// <summary>
    /// 保存
    /// </summary>
    public class SaveBtnEditMonthlyReport
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 权限类型  1 月报 2日报
        /// </summary>
        public int PermissionType { get; set; }
        /// <summary>
        /// 授权开始时间 针对日报
        /// </summary>
        public DateTime DailyStartTime { get; set; }
        /// <summary>
        /// 授权结束时间 针对日报
        /// </summary>
        public DateTime DailyEndTime { get; set; }
        /// <summary>
        /// true 启用
        /// </summary>
        public bool DailyReportEnable { get; set; } = false;
        /// <summary>
        /// true 启用
        /// </summary>
        public bool MonthReportEnable { get; set; } = false;
        /// <summary>
        /// 月报允许修改的月份 字符串拼接 202401,202501
        /// </summary>
        public List<string>? MonthTime { get; set; }
        /// <summary>
        /// 月报允许修改的月份有效时间  默认三天  日报默认当前时间不做处理
        /// </summary>
        public DateTime MonthEffectiveTime { get; set; }
    }
}
