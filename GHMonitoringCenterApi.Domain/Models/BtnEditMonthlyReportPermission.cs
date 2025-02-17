using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 月报修改按钮权限表
    /// </summary>
    [SugarTable("t_btneditmonthlyreportpermission", IsDisabledDelete = true)]
    public class BtnEditMonthlyReportPermission : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid UserId { get; set; }
        /// <summary>
        /// 权限类型  1 月报 2日报
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int PermissionType { get; set; }
        /// <summary>
        /// 授权开始时间 针对日报
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime DailyStartTime { get; set; }
        /// <summary>
        /// 授权结束时间 针对日报
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime DailyEndTime { get; set; }
        /// <summary>
        /// true 启用
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool DailyReportEnable { get; set; } = false;
        /// <summary>
        /// true 启用
        /// </summary>
        public bool MonthReportEnable { get; set; } = false;
        /// <summary>
        /// 月报允许修改的月份 字符串拼接 202401,202501
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? MonthTime { get; set; }
        /// <summary>
        /// 月报允许修改的月份有效时间  默认三天  日报默认当前时间不做处理
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime MonthEffectiveTime { get; set; }
    }
}
