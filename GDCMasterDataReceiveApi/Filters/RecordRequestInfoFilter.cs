using GDCMasterDataReceiveApi.Application.Contracts.Dto.DataInterface;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using GDCMasterDataReceiveApi.Domain.Shared.Const;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UtilsSharp;
using HttpStatusCode = GDCMasterDataReceiveApi.Domain.Shared.Enums.HttpStatusCode;

namespace GDCMasterDataReceiveApi.Filters
{

    /// <summary>
    /// 记录请求信息拦截器
    /// </summary>
    public class RecordRequestInfoFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var method = actionDescriptor.MethodInfo;
            var httpContext = context.HttpContext;
            #region 拦截验证查看接口基本信息是否允许
            var isAllowInterfaceIntercept = context.ActionDescriptor.EndpointMetadata.OfType<InterfaceInterceptAttribute>().Any();
            if (isAllowInterfaceIntercept)
            {
                WebHelper webHelper = new WebHelper();
                var interfaceAuthApi = AppsettingsHelper.GetValue("API:InterfaceAuthApi");
                var appKey = "appKey";
                var appinterfaceCode = "appinterfaceCode";
                var headers = context.HttpContext.Request.Headers;
                if (headers.ContainsKey(appKey) && headers.ContainsKey(appinterfaceCode))
                {
                    var sKey = context.HttpContext.Request.Headers[appKey].ToString();
                    var iKey = context.HttpContext.Request.Headers[appinterfaceCode].ToString();
                    webHelper.Headers.Add("appKey", sKey);
                    webHelper.Headers.Add("appinterfaceCode", iKey);
                    var interfaceAuth = await webHelper.DoGetAsync<string>(interfaceAuthApi);
                    if (interfaceAuth.Code == 200 && interfaceAuth.Result == "true")
                    {
                        var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                        if (!string.IsNullOrWhiteSpace(actionName))
                        {
                            actionName = actionName + "Async";
                        }
                        var systemInterfaceInfoApi = AppsettingsHelper.GetValue("API:SystemInterfaceInfoApi");
                        systemInterfaceInfoApi= systemInterfaceInfoApi.Replace("$systemApi", sKey).Replace("$interfaceApi", iKey);
                        var interfaceInfoList = await webHelper.DoGetAsync<ResponseAjaxResult<List<DataInterfaceResponseDto>>>(systemInterfaceInfoApi);
                        //如果返回空
                        if (interfaceInfoList.Code==500||(interfaceInfoList.Code == 200 && interfaceInfoList.Result == null && !interfaceInfoList.Result.Data.Any()))
                        {
                            ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>()
                            {
                            };

                            responseAjaxResult.Fail(message: ResponseMessage.ACCESSINTERFACE_ERROR, httpStatusCode: HttpStatusCode.InterfaceAuth);
                            context.HttpContext.Response.Clear();
                            var obj = new ContentResult()
                            {
                                StatusCode = 200,
                                Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                                {
                                    //首字母小写问题
                                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                                }),
                            };
                            context.Result = obj;
                            return;
                        }
                        var intefaceInfo = interfaceInfoList.Result.Data!=null?interfaceInfoList.Result.Data.Where(x => x.InterfaceName == actionName && x.IsEnable == 1).FirstOrDefault():null;
                        //验证接口是否存在
                        if (intefaceInfo == null)
                        {
                            ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>()
                            {
                            };

                            responseAjaxResult.Fail(message: ResponseMessage.ACCESSINTERFACE_ERROR, httpStatusCode: HttpStatusCode.InterfaceAuth);
                            context.HttpContext.Response.Clear();
                            var obj = new ContentResult()
                            {
                                StatusCode = 200,
                                Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                                {
                                    //首字母小写问题
                                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                                }),
                            };
                            context.Result = obj;
                            return;
                        }
                        //验证访问IP
                        #region 限制IP
                        var splitStr = ";";
                        var accessIp = Utils.GetIP();
                        // await Console.Out.WriteLineAsync($"接口访问IP地址是:{accessIp}");
                        if (!string.IsNullOrWhiteSpace(intefaceInfo.AccessRestrictedIP) && intefaceInfo.AccessRestrictedIP.IndexOf("splitStr") >=-1)
                        {
                            var ipList = intefaceInfo.AccessRestrictedIP.Split(splitStr, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var isExist = ipList.Where(x => x == "*").ToList();
                            if (!isExist.Any() && ipList.Where(x => x == accessIp).Count() < 0)
                            {
                                ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>()
                                {
                                };

                                responseAjaxResult.Fail(message: ResponseMessage.IPACCESS_ERROR, httpStatusCode: HttpStatusCode.InterfaceAuth);
                                context.HttpContext.Response.Clear();
                                var obj = new ContentResult()
                                {
                                    StatusCode = 200,
                                    Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                                    {
                                        //首字母小写问题
                                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                                    }),
                                };
                                context.Result = obj;
                                return;
                            }

                        }
                        #endregion
                        CacheHelper cacheHelper = new CacheHelper();
                        cacheHelper.Set(httpContext.TraceIdentifier, intefaceInfo, 30);
                    }
                    else
                    {
                        ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>()
                        {
                        };
                        var value = interfaceAuth.Result!= null?JObject.Parse(interfaceAuth.Result)["message"]: interfaceAuth.Msg;
                        responseAjaxResult.Fail(message: value.ToString(), httpStatusCode: HttpStatusCode.InterfaceAuth);
                        context.HttpContext.Response.Clear();
                        var obj = new ContentResult()
                        {
                            StatusCode = 200,
                            Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                            {
                                //首字母小写问题
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            }),
                        };
                        context.Result = obj;
                        return;
                    }
                }
                else
                {
                    ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>()
                    {
                    };

                    responseAjaxResult.Fail(message: ResponseMessage.APPKEY_ERROR, httpStatusCode: HttpStatusCode.InterfaceAuth);
                    context.HttpContext.Response.Clear();
                    var obj = new ContentResult()
                    {
                        StatusCode = 200,
                        Content = JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                        {
                            //首字母小写问题
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }),
                    };
                    context.Result = obj;
                    return;
                }
            }

            #endregion

            //获取请求参数
            #region 请求参数
            RecordRequestInfo recordRequestInfo = new RecordRequestInfo()
            {
                RequestInfo = new RequestInfo(),
                SqlExecInfos = new List<SqlExecInfo>(),
                BrowserInfo = new BrowserInfo(),
                Exceptions = new Exceptions()
            };
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
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
                            recordRequestInfo.RequestInfo.Input = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion

            #region 是否开启审计日志
            if (Convert.ToBoolean(AppsettingsHelper.GetValue("AuditLogs:IsOpen")))
            {
                recordRequestInfo.Id = Guid.NewGuid();
                recordRequestInfo.ClientIpAddress = Utils.GetIP();
                recordRequestInfo.HttpMethod = requestMethod;
                recordRequestInfo.ApplicationName = "GDCMasterDataReceiveApi";
                recordRequestInfo.Url = context.HttpContext.Request.Path;
                recordRequestInfo.RequestTime = currentTime;
                recordRequestInfo.ActionMethodName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                recordRequestInfo.BrowserInfo.Browser = context.HttpContext.Request.Headers["User-Agent"].ToString();
                CacheHelper cache = new CacheHelper();
                cache.Set(context.HttpContext.TraceIdentifier.ToLower(), JsonConvert.SerializeObject(recordRequestInfo), 60);
                int cacheSeconds = int.Parse(AppsettingsHelper.GetValue("Redis:DefaultKeyCacheSeconds"));
                await RedisUtil.Instance.SetAsync(context.HttpContext.TraceIdentifier.ToLower(), JsonConvert.SerializeObject(recordRequestInfo), cacheSeconds);
            }
            #endregion
            await next();
        }
    }
}
