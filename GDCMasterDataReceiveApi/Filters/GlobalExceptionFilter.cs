using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DataInterface;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Const;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using System.Text;
using System.Text.Json.Nodes;
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
        private readonly IMapper mapper;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
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

            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            var routeData = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller).ControllerContext.RouteData.Values.ToArray();
            var db = context.HttpContext.RequestServices.GetService<ISqlSugarClient>();
            var isRecord = true;
            #region 获取请求参数
            RecordRequestInfo recordRequestInfo = new RecordRequestInfo()
            {
                RequestInfo = new RequestInfo() {},
            };
            var requestMethod = context.HttpContext.Request.Method.ToUpper();
            if (requestMethod == "GET")
            {
                if (context.HttpContext.Request.QueryString.HasValue && !string.IsNullOrWhiteSpace(context.HttpContext.Request.QueryString.Value))
                {
                    recordRequestInfo.RequestInfo.Input = context.HttpContext.Request.QueryString.Value.Replace("?", "").TrimAll();

                }
            }
            else if (requestMethod == "POST")
            {
                try
                {
                    if (context.HttpContext.Request.ContentType != null && context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                    {
                        context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                        {
                            var parame = await reader.ReadToEndAsync();
                            recordRequestInfo.RequestInfo = new RequestInfo() { Input = parame != null ? parame : "" };
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion

            #region 400的错误
            try
            {
                
                ResponseAjaxResult<List<ErrorMessage>> responseAjaxResult = new();
                //格式化请求参数错误提示0HMOVSRFRALDE:00000019
                if (context.Result != null && context.Result.ToString().IndexOf("FileContentResult") < 0 && context.Result.ToString().IndexOf("FileStreamResult") < 0 && context.Result != null && context.Result.ToString().IndexOf("EmptyResult") < 0 && context.Result.ToString().IndexOf("ContentResult") < 0 && ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).StatusCode == 400)
                {
                    isRecord = false;
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
                    #region 请求参数400记录审计日志  只记录接收集团主数据请求的错误  
                    //if (routeData.Count() > 0 && routeData[1].ToString().IndexOf("Receive") >= 0)
                    //{
                        #region 暂时不用
                        //RecordRequestInfo recordRequestInfos = new RecordRequestInfo()
                        //{
                        //    RequestInfo = new RequestInfo(),
                        //};
                        //var requestMethod = context.HttpContext.Request.Method.ToUpper();
                        //if (requestMethod == "GET")
                        //{
                        //    if (context.HttpContext.Request.QueryString.HasValue && !string.IsNullOrWhiteSpace(context.HttpContext.Request.QueryString.Value))
                        //    {
                        //        recordRequestInfo.RequestInfo.Input = context.HttpContext.Request.QueryString.Value.Replace("?", "").TrimAll();

                        //    }
                        //}
                        //else if (requestMethod == "POST")
                        //{
                        //    try
                        //    {
                        //        if (context.HttpContext.Request.ContentType != null && context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                        //        {
                        //            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                        //            using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                        //            {
                        //                var parame = await reader.ReadToEndAsync();
                        //                recordRequestInfo.RequestInfo = new RequestInfo() { Input = parame != null ? parame : "" };
                        //            }
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {

                        //    }
                        //}
                        #endregion

                        AuditLogs auditLogs = new AuditLogs()
                        {
                            ApplicationName = "GDCMasterDataReceiveApi",
                            BrowserInfo = context.HttpContext.Request.Headers["User-Agent"].ToString(),
                            ClientIpAddress = Utils.GetIP(),
                            Exceptions = errors.ToJson(),
                            HttpMethod = requestMethod,
                            HttpStatusCode = 400,
                            Id = recordRequestInfo.Id,
                            RequestParames = recordRequestInfo.RequestInfo.Input,
                            RequestTime = currentTime,
                            Url = context.HttpContext.Request.Path,
                            ActionMethodName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName
                        };

                        if (db != null)
                        {
                            await db.Insertable<AuditLogs>(auditLogs).ExecuteCommandAsync();
                        }
                        logger.LogWarning($"用户请求{context.HttpContext.Request.Path}方法出现400的错误信息:{responseAjaxResult.ToJson()}\r\n 请求参数:{recordRequestInfo.RequestInfo.Input}");
                   // }
                    #endregion
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
                else {

                    #region 暂时不用
                    //var requestMethod = context.HttpContext.Request.Method.ToUpper();
                    //if (requestMethod == "GET")
                    //{
                    //    if (context.HttpContext.Request.QueryString.HasValue && !string.IsNullOrWhiteSpace(context.HttpContext.Request.QueryString.Value))
                    //    {
                    //        recordRequestInfo.RequestInfo = new RequestInfo() { Input = context.HttpContext.Request.QueryString.Value.Replace("?", "").TrimAll() };

                    //    }
                    //}
                    //else if (requestMethod == "POST")
                    //{
                    //    try
                    //    {
                    //        if (context.HttpContext.Request.ContentType != null && context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                    //        {
                    //            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                    //            using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                    //            {
                    //                var parame = await reader.ReadToEndAsync();
                    //                recordRequestInfo.RequestInfo = new RequestInfo() { Input = parame != null ? parame : "" };
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                recordRequestInfo.Exceptions.ExceptionInfo = ex.Message + Environment.NewLine + ex.StackTrace;
                logger.LogError(ex, "自定义接口入参格式化错误信息出现错误");
            }
            #endregion

            #region 判断接口是否做拦截
            
            //接口返回值
            
            if (routeData[1].ToString().IndexOf("Receive") <=0)
            { 
                #region 过滤接口返回值
                CacheHelper cacheHelper = new CacheHelper();
                var cacheResult = cacheHelper.Get<DataInterfaceResponseDto>(context.HttpContext.TraceIdentifier);
                var IsEncrypt = 1;//是否加密   0  不是加密 

                #region  接口返回值字段规则设置
                WebHelper webHelper = new WebHelper();
                var systemInterfaceFiledRuleApi = AppsettingsHelper.GetValue("API:SystemInterfaceFiledRuleApi");
                Dictionary<string, object> parames = new Dictionary<string, object>();
                string setupResult = string.Empty;
                if (cacheResult != null)
                {
                    var returnRes = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
                    webHelper.Headers.Add("appKey", cacheResult.AppKey);
                    webHelper.Headers.Add("appinterfaceCode", cacheResult.AppinterfaceCode);
                    parames.Add("JsonObj", returnRes.ToJson(true));
                    parames.Add("InterfaceApiId", cacheResult.Id);
                    parames.Add("AppSystemId", cacheResult.AppSystemId);
                    var   responseResult = await webHelper.DoPostAsync<string>(systemInterfaceFiledRuleApi, parames);
                    setupResult = responseResult.Result;
                    context.Result =new JsonResult(responseResult.Result);
                }

                #endregion

                #region 过滤接口加密设置
                if (cacheResult != null && cacheResult.IsEncrypt == 1)
                {
                    IsEncrypt = 1;//加密
                    //接口返回值
                    //var returnRes = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
                    if (setupResult!=null)
                    {
                        var interfaceEncryptApi = AppsettingsHelper.GetValue("API:InterfaceEncryptApi");
                        Dictionary<string, object> parame = new Dictionary<string, object>();
                        parame.Add("item", setupResult.ToJson(true));
                        var dateEncrypt = await webHelper.DoPostAsync(interfaceEncryptApi, parame);
                        var obj = new ContentResult()
                        {
                            StatusCode = 200,
                            Content = JsonConvert.SerializeObject(dateEncrypt.Result, new JsonSerializerSettings()
                            {
                                //首字母小写问题
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            }),
                        };
                        context.Result = obj;

                    }
                }
                else  if(cacheResult != null && cacheResult.IsEncrypt == 0)//不加密  且返回的是json字符串
                {
                    IsEncrypt = 2;
                }
                else if (cacheResult == null) //不加密  返回json对象
                {
                    IsEncrypt = 3;
                }
                #endregion

                context.HttpContext.Response.Headers.Append("IsEncryption", IsEncrypt.ToString());

                #endregion
            }

            #endregion


            #region 针对人员 机构 返回值处理
            if (routeData[0].ToString().IndexOf("Person") >= 0)
            {
                var jsonObj=JsonConvert.DeserializeObject<MDMResponseResult>(((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value.ToJson());
                context.Result= new ObjectResult(jsonObj);
            }

            #endregion

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
                           
                            // var userService = context.HttpContext.RequestServices.GetService<IUserService>();
                            var sql = recordRequestInfo.SqlExecInfos.Select(x => x.Sql).ToList();
                            var sqlTotalTime = recordRequestInfo.SqlExecInfos.Select(x => x.SqlTotalTime).ToList();
                            AuditLogs auditLogs = new AuditLogs()
                            {
                                ApplicationName = recordRequestInfo.ApplicationName,
                                BrowserInfo = recordRequestInfo.BrowserInfo.Browser,
                                ClientIpAddress = Utils.GetIP(),
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
                                AppKey = recordRequestInfo.RequestHeads.AppKey,
                                AppinterfaceCode = recordRequestInfo.RequestHeads.AppinterfaceCode,
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
                            await redis.DelAsync(context.HttpContext.TraceIdentifier.ToLower());
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("删除redis缓存出现错误", ex);
                        }
                    }
                }
                else if(isRecord) {
                    
                    var returnRes = ((Microsoft.AspNetCore.Mvc.ContentResult)context.Result);
                    recordRequestInfo.Exceptions= new Exceptions();
                    recordRequestInfo.Exceptions.ExceptionInfo = returnRes?.Content.ToJson();
                    recordRequestInfo.BrowserInfo = new BrowserInfo();
                    AuditLogs auditLogs = new AuditLogs()
                    {
                        ApplicationName = recordRequestInfo.ApplicationName,
                        BrowserInfo = recordRequestInfo.BrowserInfo.Browser= context.HttpContext.Request.Headers["User-Agent"].ToString(),
                        ClientIpAddress = Utils.GetIP(),
                        Exceptions = recordRequestInfo.Exceptions.ExceptionInfo,
                        ExecutionDuration =string.Empty,
                        HttpMethod = context.HttpContext.Request.Method,
                        HttpStatusCode = 10000,//非法请求
                        RequestParames = recordRequestInfo.RequestInfo.Input,
                        RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"),
                        Sql = string.Empty,
                        SqlExecutionDuration = string.Empty,
                        Url = context.HttpContext.Request.Path,
                        ActionMethodName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName,
                        AppKey = context.HttpContext.Request.Headers["AppKey"].ToString(),
                        AppinterfaceCode = context.HttpContext.Request.Headers["AppinterfaceCode"].ToString(),
                    };
                  
                    if (db != null)
                    {
                        await db.Insertable<AuditLogs>(auditLogs).ExecuteCommandAsync();
                    }

                }

            }
            #endregion


          
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
                            ClientIpAddress = Utils.GetIP(),
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
                            UserName = recordRequestInfo.UserName,
                            AppKey = recordRequestInfo.RequestHeads.AppKey,
                            AppinterfaceCode = recordRequestInfo.RequestHeads.AppinterfaceCode,
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
                if (context.HttpContext.Request.RouteValues.Where(x => x.Value.Equals("Receive")).Any())
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
