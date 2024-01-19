
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 交建通发送消息用户列表
    /// </summary>
    [SugarTable("t_jjtmessageuser", IsDisabledDelete = true)]
    public class JjtMessageUser : BaseEntity<Guid>
    {
        /// <summary>
        /// 人资编码
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? UserGroupCode { get; set; }
        /// <summary>
        /// 人员名称名称
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? UserName { get; set; }
        ///// <summary>
        ///// 预提醒人员
        ///// </summary>
        //[SugarColumn(ColumnDataType = "text")]
        //public string? ExpectUserGroupCodes { get; set; }
        ///// <summary>
        ///// 预提醒人员名称
        ///// </summary>
        //[SugarColumn(ColumnDataType = "text")]
        //public string? ExpectUserNames { get; set; }
        /// <summary>
        /// 当天日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int NowDay { get; set; }
        ///// <summary>
        ///// 预计提醒时间
        ///// </summary>
        //[SugarColumn(ColumnDataType = "datetime")]
        //public DateTime? ExpectTime { get; set; }
        /// <summary>
        /// 人员类型  1 第一批用户 2第二批用户
        /// </summary>
        public int UserType { get;set; }
    }
}
