using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Const;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Filters
{
    public class GlobalExceptionFilter : IAsyncResultFilter, IAsyncExceptionFilter
    {
        #region  注入日志
        /// <summary>
        /// 注入日志
        /// </summary>
        private readonly ILogger<GlobalExceptionFilter> logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }
        #endregion

        #region 自定义接口入参格式化错误信息
        /// <summary>
        /// 自定义接口入参格式化错误信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            RecordRequestInfo recordRequestInfo = new RecordRequestInfo();
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            try
            {

                ResponseAjaxResult<List<ErrorMessage>> responseAjaxResult = new();
                //格式化请求参数错误提示0HMOVSRFRALDE:00000019
                if (context.Result != null && context.Result.ToString().IndexOf("FileContentResult") < 0 && context.Result.ToString().IndexOf("FileStreamResult") < 0 && context.Result != null && context.Result.ToString().IndexOf("EmptyResult") < 0 && ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).StatusCode == 400)
                {
                    var errMsg = ((Microsoft.AspNetCore.Mvc.ValidationProblemDetails)((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value).Errors;
                    List<ErrorMessage> errors = new();
                    foreach (var msg in errMsg)
                    {
                        ErrorMessage errorMessage = new ErrorMessage()
                        {
                            Key = msg.Key,
                            Value = msg.Value[0],
                        };
                        if (msg.Value[0].IndexOf("required") >= 0)
                        {
                            errorMessage.Value = $"{msg.Key}字段不能为空";
                        }
                        if (msg.Value[0].IndexOf("Guid") >= 0)
                        {
                            errorMessage.Value = $"{msg.Key}GUID类型的不能为空字符串空的GUID=00000000-0000-0000-0000-000000000000";
                        }
                        errors.Add(errorMessage);
                    }
                    responseAjaxResult.Data = errors;
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_PARAMETER_ERROR, HttpStatusCode.ParameterError);
                    //把400的状态吗记录到日志文件
                    logger.LogWarning($"用户请求{context.HttpContext.Request.Path}方法出现400的错误信息:{responseAjaxResult.ToJson()}");
                    context.Result = new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                        {
                            //首字母小写问题
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }),

                        //StatusCode = (int)HttpStatusCode.ParameterError,
                        StatusCode = (int)HttpStatusCode.Success,
                        ContentType = "application/json charset=utf-8"
                    };
                }
            }
            catch (Exception ex)
            {
                recordRequestInfo.Exceptions.ExceptionInfo = ex.Message + Environment.NewLine + ex.StackTrace;
                logger.LogError(ex, "自定义接口入参格式化错误信息出现错误");
            }

            #region 是否开启审计日志
            if (Convert.ToBoolean(AppsettingsHelper.GetValue("AuditLogs:IsOpen")))
            {
                var redis = RedisUtil.Instance;
                var res = await redis.GetAsync(context.HttpContext.TraceIdentifier.ToLower());
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            recordRequestInfo = JsonConvert.DeserializeObject<RecordRequestInfo>(res);

                            if (recordRequestInfo == null || string.IsNullOrWhiteSpace(recordRequestInfo.RequestTime))
                            {
                                await next.Invoke();
                                return;
                            }
                            var stamp1 = TimeHelper.DateTimeToTimeStamp(Convert.ToDateTime(recordRequestInfo.RequestTime), TimeStampType.毫秒);
                            var stamp2 = TimeHelper.DateTimeToTimeStamp(Convert.ToDateTime(currentTime), TimeStampType.毫秒);
                            recordRequestInfo.ExecutionDuration = Math.Abs(stamp1 - stamp2).ToString();
                            if (context.Result != null && context.Result.ToString().IndexOf("ContentResult") >= 0)
                            {
                                recordRequestInfo.HttpStatusCode = (int)((ContentResult)context.Result).StatusCode;
                            }
                            else
                            {
                                recordRequestInfo.HttpStatusCode = context.HttpContext.Response.StatusCode;
                            }
                            var db = context.HttpContext.RequestServices.GetService<ISqlSugarClient>();
                            // var userService = context.HttpContext.RequestServices.GetService<IUserService>();
                            var sql = recordRequestInfo.SqlExecInfos.Select(x => x.Sql).ToList();
                            var sqlTotalTime = recordRequestInfo.SqlExecInfos.Select(x => x.SqlTotalTime).ToList();
                            AuditLogs auditLogs = new AuditLogs()
                            {
                                ApplicationName = recordRequestInfo.ApplicationName,
                                BrowserInfo = recordRequestInfo.BrowserInfo.Browser,
                                ClientIpAddress = recordRequestInfo.ClientIpAddress,
                                Exceptions = recordRequestInfo.Exceptions.ExceptionInfo,
                                ExecutionDuration = recordRequestInfo.ExecutionDuration,
                                HttpMethod = recordRequestInfo.HttpMethod,
                                HttpStatusCode = recordRequestInfo.HttpStatusCode,
                                Id = recordRequestInfo.Id,
                                RequestParames = recordRequestInfo.RequestInfo.Input,
                                RequestTime = recordRequestInfo.RequestTime,
                                Sql = string.Join('|', sql.Select(x => x)),
                                SqlExecutionDuration = string.Join('|', sqlTotalTime.Select(x => x)),
                                Url = recordRequestInfo.Url,
                                ActionMethodName = recordRequestInfo.ActionMethodName,
                            };
                            var head = context.HttpContext.Request.Headers["Authorization"].ToString();
                            GlobalCurrentUser userInfo = null;
                            if (!string.IsNullOrWhiteSpace(head))
                            {
                                head = head.Replace("Bearer", "").Trim();
                                //if (userService != null)
                                //{
                                //     userInfo = userService.GetUserInfoAsync(head);
                                //    auditLogs.UserId = userInfo?.Id;
                                //    auditLogs.UserName = userInfo?.Name;
                                //}
                            }

                            if (db != null)
                                await db.Insertable<AuditLogs>(auditLogs).ExecuteCommandAsync();

                            #region 只有退出帐号后才会执行下面代码
                            if (userInfo != null && auditLogs.Url.IndexOf("/OperationLog/LogoutLog") > 0)
                            {
                                //删除当前用户在redis里面的缓存
                                if (await redis.ExistsAsync(userInfo.Account))
                                {
                                    await redis.DelAsync(userInfo.Account);
                                }
                            }
                            #endregion
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
                            await redis.DelAsync(context.HttpContext.TraceIdentifier.ToLower());
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("删除redis缓存出现错误", ex);
                        }
                    }
                }


            }
            #endregion
            context.HttpContext.Response.Headers.Add("AesKey", AppsettingsHelper.GetValue("AesKey"));
            await next.Invoke();
        }
        #endregion

        #region 全局异常处理
        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new Domain.Shared.ResponseAjaxResult<bool>();
            var obj = new ContentResult();
            try
            {
                RecordRequestInfo recordRequestInfo = new RecordRequestInfo();
                var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                //判断异常是否处理如果没有处理会走到这里
                //异常信息
                string? message = context.Exception.Message;
                string? exception = context.Exception.StackTrace;
                if (!context.ExceptionHandled)
                {
                    context.Result = new ContentResult()
                    {
                        Content = message + Environment.NewLine + exception,
                        StatusCode = (int)HttpStatusCode.Fail,
                        ContentType = "application/json charset=utf-8"
                    };
                    logger.LogError(message + Environment.NewLine + exception, "全局异常处理");
                }

                #region 是否开启审计日志
                if (Convert.ToBoolean(AppsettingsHelper.GetValue("AuditLogs:IsOpen")))
                {
                    var redis = RedisUtil.Instance;
                    var res = redis.Get(context.HttpContext.TraceIdentifier.ToLower());
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        recordRequestInfo = JsonConvert.DeserializeObject<RecordRequestInfo>(res);
                    }

                    if (recordRequestInfo != null)
                    {
                        if (string.IsNullOrWhiteSpace(recordRequestInfo.RequestTime))
                        {
                            //异常已经处理
                            context.ExceptionHandled = true;
                            //格式化错误信息
                            responseAjaxResult.Fail(ResponseMessage.SYSTEM_ERROR, HttpStatusCode.Fail);
                            obj = new ContentResult()
                            {
                                StatusCode = 500,
                                Content = responseAjaxResult.ToJson()
                            };
                            context.Result = obj;
                            return;
                        }
                        var stamp1 = TimeHelper.DateTimeToTimeStamp(Convert.ToDateTime(recordRequestInfo.RequestTime), TimeStampType.毫秒);
                        var stamp2 = TimeHelper.DateTimeToTimeStamp(Convert.ToDateTime(currentTime), TimeStampType.毫秒);
                        recordRequestInfo.ExecutionDuration = Math.Abs(stamp1 - stamp2).ToString();
                        if (context.Result != null && context.Result.ToString().IndexOf("ContentResult") >= 0)
                        {
                            recordRequestInfo.HttpStatusCode = (int)((ContentResult)context.Result).StatusCode;
                        }
                        else
                        {
                            recordRequestInfo.HttpStatusCode = context.HttpContext.Response.StatusCode;
                        }

                        var db = context.HttpContext.RequestServices.GetService<ISqlSugarClient>();
                        var sql = recordRequestInfo.SqlExecInfos.Select(x => x.Sql).ToList();
                        var sqlTotalTime = recordRequestInfo.SqlExecInfos.Select(x => x.SqlTotalTime).ToList();
                        AuditLogs auditLogs = new AuditLogs()
                        {
                            ApplicationName = recordRequestInfo.ApplicationName,
                            BrowserInfo = recordRequestInfo.BrowserInfo.Browser,
                            ClientIpAddress = recordRequestInfo.ClientIpAddress,
                            Exceptions = message + Environment.NewLine + exception,
                            ExecutionDuration = recordRequestInfo.ExecutionDuration,
                            HttpMethod = recordRequestInfo.HttpMethod,
                            HttpStatusCode = recordRequestInfo.HttpStatusCode,
                            Id = recordRequestInfo.Id,
                            RequestParames = recordRequestInfo.RequestInfo.Input,
                            RequestTime = recordRequestInfo.RequestTime,
                            Sql = string.Join('|', sql.Select(x => x)),
                            SqlExecutionDuration = string.Join('|', sqlTotalTime.Select(x => x)),
                            Url = recordRequestInfo.Url,
                            UserId = Guid.NewGuid(),
                            ActionMethodName = recordRequestInfo.ActionMethodName,
                            UserName = recordRequestInfo.UserName
                        };
                        try
                        {
                            var head = context.HttpContext.Request.Headers["Authorization"].ToString();
                            if (!string.IsNullOrWhiteSpace(head))
                            {
                                // var userService = context.HttpContext.RequestServices.GetService<IUserService>();
                                head = head.Replace("Bearer", "").Trim();
                                //if (userService != null)
                                //{
                                //    var userInfo = userService.GetUserInfoAsync(head);
                                //    auditLogs.UserId = userInfo?.Id;
                                //    auditLogs.UserName = userInfo?.Name;
                                //}
                            }
                            if (db != null)
                            {
                                await db.Insertable<AuditLogs>(auditLogs).ExecuteCommandAsync();
                                await redis.DelAsync(context.HttpContext.TraceIdentifier.ToLower());
                            }

                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning($"记录用户请求的参数如下:{auditLogs.RequestParames}");
                            logger.LogError(ex, "保存请求信息到数据库时出现错误");

                        }
                        finally
                        {
                            try
                            {
                                await redis.DelAsync(context.HttpContext.TraceIdentifier.ToLower());
                            }
                            catch (Exception ex)
                            {
                                logger.LogError("删除redis缓存出现错误", ex);
                            }
                        }
                    }
                }
                #endregion

                #region 更新接收数据日志
                if (context.HttpContext.Request.Path.Value.Contains("Receive"))
                {
                    var traceIdentifier = context.HttpContext.TraceIdentifier;
                    var db = context.HttpContext.RequestServices.GetService<ISqlSugarClient>();
                    var res = db.Queryable<ReceiveRecordLog>().Where(x => x.Traceidentifier == traceIdentifier).ToList();
                    foreach (var item in res)
                    {
                        item.Id = item.Id;
                        item.FailMessage = message + Environment.NewLine + exception;
                        item.SuccessNumber = 0;
                    }
                    await db.Updateable<ReceiveRecordLog>(res).ExecuteCommandAsync();
                }
               
                #endregion
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "全局异常处理");
            }
            //异常已经处理
            context.ExceptionHandled = true;
            //格式化错误信息
            responseAjaxResult.Fail(ResponseMessage.SYSTEM_ERROR, HttpStatusCode.Fail);
            obj = new ContentResult()
            {
                StatusCode = 500,
                Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                {
                    //首字母小写问题
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }),
            };
            context.Result = obj;
        }
        #endregion

    }
}
