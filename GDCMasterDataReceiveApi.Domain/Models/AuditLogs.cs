using SqlSugar;
namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 审计表
    /// </summary>
    [SugarTable("t_auditlogs", IsDisabledDelete = true)]
    public class AuditLogs
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, Length = 36)]
        public Guid Id { get; set; }
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ApplicationName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>

        [SugarColumn(Length = 50)]
        public string? UserName { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? RequestTime { get; set; }

        /// <summary>
        /// sql语句
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? Sql { get; set; }

        /// <summary>
        /// sql执行时间
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? SqlExecutionDuration { get; set; }
        /// <summary>
        /// 耗时时间
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ExecutionDuration { get; set; }
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ClientIpAddress { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? BrowserInfo { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? HttpMethod { get; set; }

        /// <summary>
        /// Action方法名
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ActionMethodName { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? RequestParames { get; set; }
        /// <summary>
        /// 请求路由
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Url { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? Exceptions { get; set; }
        /// <summary>
        /// 请求状态码
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? HttpStatusCode { get; set; }

    }
}
