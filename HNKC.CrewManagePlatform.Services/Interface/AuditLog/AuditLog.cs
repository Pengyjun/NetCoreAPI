using HNKC.CrewManagePlatform.Models.Dtos.AuditLog;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.AuditLog
{
    public class AuditLog:IAuditLogService
    {
        #region  注入日志
        /// <summary>
        /// 注入日志
        /// </summary>
        private readonly ILogger<AuditLog> logger;
        public AuditLog(ILogger<AuditLog> logger)
        {
            this.logger = logger;
        }
        #endregion
        /// <summary>
        /// 审计日志记录请求参数
        /// </summary>
        /// <param name="actionName">请求方法名称</param>
        /// <param name="requestParame">请求参数</param>
        public async Task SetupParameAsync(string actionName, string requestParame)
        {
            RecordRequestInfo recordRequestInfo = new RecordRequestInfo()
            {
                RequestInfo = new RequestInfo() { Input = requestParame },
                SqlExecInfos = new List<SqlExecInfo>(),
                BrowserInfo = new BrowserInfo(),
                Exceptions = new  Models.Dtos.AuditLog.Exceptions()

            };
            #region 是否开启审计日志
            if (Convert.ToBoolean(AppsettingsHelper.GetValue("AuditLogs:IsOpen")))
            {
                recordRequestInfo.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                recordRequestInfo.HttpMethod = HttpContentAccessFactory.Current.Request.Method;
                recordRequestInfo.ApplicationName = "CrewManagePlatform";
                recordRequestInfo.Url = HttpContentAccessFactory.Current.Request.Path;
                recordRequestInfo.RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                recordRequestInfo.ActionMethodName = actionName;
                recordRequestInfo.BrowserInfo.Browser = HttpContentAccessFactory.Current.Request.Headers["User-Agent"].ToString();
                CacheHelper cache = new CacheHelper();
                int cacheSeconds = int.Parse(AppsettingsHelper.GetValue("Redis:DefaultKeyCacheSeconds"));
                cache.Set(HttpContentAccessFactory.Current.Request.HttpContext.TraceIdentifier.ToLower() + "Log", recordRequestInfo, cacheSeconds);
            }
            #endregion

        }

        /// <summary>
        /// 保存审计日志
        /// </summary>
        public async Task SaveAuditLogAsync()
        {
            CacheHelper cache = new CacheHelper();
            var cacheKey = string.Empty;
            try
            {
                cacheKey = $"{HttpContentAccessFactory.Current.Request.HttpContext.TraceIdentifier.ToLower()}Log";
                var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                var auditLogInfo = cache.Get<RecordRequestInfo>(cacheKey);
                //接口请求时间
                var stamp1 = TimeHelper.DateTimeToTimeStamp(Convert.ToDateTime(auditLogInfo.RequestTime), TimeStampType.毫秒);
                //接口响应时间
                var stamp2 = TimeHelper.DateTimeToTimeStamp(Convert.ToDateTime(currentTime), TimeStampType.毫秒);
                if (auditLogInfo != null)
                {
                    auditLogInfo.ExecutionDuration = Math.Abs(stamp1 - stamp2).ToString();
                    var sql = auditLogInfo.SqlExecInfos.Select(x => x.Sql).ToList();
                    var sqlTotalTime = auditLogInfo.SqlExecInfos.Select(x => x.SqlTotalTime).ToList();
                    var db = HttpContentAccessFactory.Current.Request.HttpContext.RequestServices.GetService<ISqlSugarClient>();
                    AuditLogs auditLogs = new AuditLogs()
                    {
                        ApplicationName = auditLogInfo.ApplicationName,
                        BrowserInfo = auditLogInfo.BrowserInfo.Browser,
                        ClientIpAddress = Common.Utils.GetIP(),
                        Exceptions = auditLogInfo.Exceptions.ExceptionInfo,
                        ExecutionDuration = auditLogInfo.ExecutionDuration,
                        HttpMethod = auditLogInfo.HttpMethod,
                        HttpStatusCode = auditLogInfo.HttpStatusCode,
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        RequestParames = auditLogInfo.RequestInfo.Input,
                        RequestTime = auditLogInfo.RequestTime,
                        Sql = string.Join('|', sql.Select(x => x)),
                        SqlExecutionDuration = string.Join('|', sqlTotalTime.Select(x => x)),
                        Url = auditLogInfo.Url,
                        ActionMethodName = auditLogInfo.ActionMethodName,
                    };
                    if (db != null)
                    {
                        await db.Insertable<AuditLogs>(auditLogs).ExecuteCommandAsync();
                    }


                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "保存请求信息到数据库时出现错误");
            }
            finally
            {
                try
                {
                    cache.Remove(cacheKey);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "删除redis缓存出现错误");
                }
            }
        }
    }
}
