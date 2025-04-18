﻿using Castle.DynamicProxy;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.POPManagOrg;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using UtilsSharp;

namespace GHElectronicFileApi.AopInterceptor
{

    /// <summary>
    /// 接收数据拦截器AOP
    /// </summary>
    public class ReceiveServiceInterceptor : IInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public async void Intercept(IInvocation invocation)
        {
            //请求唯一
            var traceIdentifier = HttpContentAccessFactory.Current.TraceIdentifier;
            //日志实例
            ReceiveRecordLog receiveRecordLog = null;
            //雪花ID
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            //注入baseService  
            var baseService = (IBaseService)HttpContentAccessFactory.Current.RequestServices.GetService<IBaseService>();
            var logger = (ILogger<ReceiveServiceInterceptor>)HttpContentAccessFactory.Current.RequestServices.GetService<ILogger<ReceiveServiceInterceptor>>();
            //接收参数
            var requestParame = string.Empty;
            //参数数量
            var parameCount = 0;
            //请求的方法名称
            var methodName = invocation.Method.Name;
            //接收数据类型
            ReceiveDataType receiveDataType = 0;
            //目标方法
            try
            {
                #region 接收类型数据判断
                if (methodName == "ProjectDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<ProjectItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Project;
                }
                else if (methodName == "CountryContinentDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<CountryContinentReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.CountryContinent;
                }
                else if (methodName == "CountryRegionDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<CountryRegionReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.CountryRegion;
                }
                else if (methodName == "CorresUnitDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<CorresUnitReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.CrossUnit;
                }
                else if (methodName == "CurrencyDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<CurrencyReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Currency;
                }
                else if (methodName == "InvoiceTypeDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<InvoiceTypeItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Invoice;
                }
                else if (methodName == "DeviceClassCodeDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<DeviceClassCodeItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.ClassDevice;
                }
                else if (methodName == "ScientifiCNoProjectDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<ScientifiCNoProjectItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Rcientific;
                }
                else if (methodName == "RoomNumberDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<RoomNumberItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Room;
                }
                else if (methodName == "LanguageDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<LanguageItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Language;
                }
                else if (methodName == "LouDongDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<LouDongItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.LouDong;
                }
                else if (methodName == "UnitMeasurementDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<UnitMeasurementItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.UnitMeasurement;
                }
                else if (methodName == "RegionalDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<RegionalItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Regional;
                }
                else if (methodName == "RegionalCenterDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<RegionalCenterItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.RegionalCenter;
                }
                else if (methodName == "NationalEconomyDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<NationalEconomyItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.NationalEconomy;
                }
                else if (methodName == "ProjectClassificationDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<ProjectClassificationItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.ProjectClassification;
                }
                else if (methodName == "RelationalContractsDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<RelationalContractsItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Regional;
                }
                else if (methodName == "AccountingDepartmentDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<AccountingDepartmentReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.AccountingDepartment;
                }
                else if (methodName == "AdministrativeOrganizationDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<AdministrativeOrganizationReceiveRequestDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.AdministrativeOrganization;
                }
                else if (methodName == "CommonDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<ValueDomainReceiveRequestDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.Common;
                }
                else if (methodName == "ManagementOrganizationDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<POPMangOrgItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.POPManger;
                }
                else if (methodName == "AdministrativeDivisionDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<AdministrativeDivisionItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.AdministrativeDivision;
                }
                else if (methodName == "PersonDataAsync")
                {
                    var receiveParame = ((ReceiveUserRequestDto)invocation.Arguments[0]).user;
                    parameCount = 1;
                    requestParame = receiveParame.ToJson();
                    receiveDataType = ReceiveDataType.Person;
                }
                else if (methodName == "InstitutionDataAsync")
                {
                    var receiveParame = ((ReceiveInstitutionRequestDto)invocation.Arguments[0]).OrganizeItem;
                    parameCount = receiveParame.Count;
                    requestParame = receiveParame.ToJson();
                    receiveDataType = ReceiveDataType.Institution;
                }
                else if (methodName == "BusinessProjectDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<BusinessCpportunityItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.BusinessCpportunity;
                }
                else if (methodName == "DeviceDetailCodeDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<DeviceDetailCodeItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.DeviceDetailCode;
                }
                else if (methodName == "AccountingOrganizationDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<AccountingOrganizationReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.AccountingOrganization;
                }
                else if (methodName == "EscrowOrganizationDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<EscrowOrganizationItem>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.EscrowOrganization;
                }

                else if (methodName == "FinancialInstitutionDataAsync")
                {
                    var receiveParame = ((BaseReceiveDataRequestDto<FinancialInstitutionReceiveDto>)invocation.Arguments[0]).IT_DATA;
                    parameCount = receiveParame.item.Count;
                    requestParame = receiveParame.item.ToJson();
                    receiveDataType = ReceiveDataType.FinancialInstitution;
                }
                #endregion

                #region 记录接收数据的请求日志
                if (!string.IsNullOrWhiteSpace(requestParame))
                {
                    receiveRecordLog = new ReceiveRecordLog()
                    {
                        Id = receiceRecordId,
                        ReceiveNumber = parameCount,
                        RequestParame = requestParame,
                        Traceidentifier = traceIdentifier,
                        ReceiveType = receiveDataType,
                    };
                   await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
                }
                #endregion

                invocation.Proceed();
                var res = (Task)invocation.ReturnValue;
                res.Wait();//如果任务有异常 直接报错
                #region 异步通知主数据  
                RecordRequestInfo recordRequestInfo = new RecordRequestInfo();
                var redis = RedisUtil.Instance;
                var result = redis.Get(traceIdentifier.ToLower());
                recordRequestInfo =JsonConvert.DeserializeObject<RecordRequestInfo>(result);
                if (recordRequestInfo != null && recordRequestInfo.RequestInfo != null && !string.IsNullOrWhiteSpace(recordRequestInfo.RequestInfo.Input))
                {
                    var parseParame = JObject.Parse(recordRequestInfo.RequestInfo.Input);

                    #region 拼接参数
                    var headParame = string.Empty;
                    var businessParame = string.Empty;
                    if (
                        parseParame["IS_REQ_HEAD_ASYNC"]!=null&&(
                        parseParame["IS_REQ_HEAD_ASYNC"]["ZZREQTIME"] != null ||
                        parseParame["IS_REQ_HEAD_ASYNC"]["ZINSTID"] != null ||
                        parseParame["IS_REQ_HEAD_ASYNC"]["ZZOBJECT"] != null ||
                        parseParame["IS_REQ_HEAD_ASYNC"]["ZZATTR1"] != null ||
                        parseParame["IS_REQ_HEAD_ASYNC"]["ZZATTR3"] != null)    
                        )
                    {
                        IS_REQ_HEAD_ASYNC  iS_REQ_HEAD_ASYNC = new IS_REQ_HEAD_ASYNC()
                        {
                            ZINSTID = parseParame["IS_REQ_HEAD_ASYNC"]["ZINSTID"].ToString(),
                            ZZATTR1 = parseParame["IS_REQ_HEAD_ASYNC"]["ZZATTR1"].ToString(),
                            ZZATTR2 = "MDM",
                            ZZATTR3 = parseParame["IS_REQ_HEAD_ASYNC"]["ZZATTR3"].ToString(),
                            ZZOBJECT = parseParame["IS_REQ_HEAD_ASYNC"]["ZZOBJECT"].ToString(),
                            ZZSRC_SYS = "GHJ-MDG",
                        };

                        if (parseParame["IS_REQ_HEAD_ASYNC"]["ZZREQTIME"] != null)
                        {
                           var timeRes= parseParame["IS_REQ_HEAD_ASYNC"]["ZZREQTIME"].ToString();
                            if (timeRes.IndexOf(".") >= 0)
                            {
                                iS_REQ_HEAD_ASYNC.ZZREQTIME = timeRes.Split(".")[0];
                            }
                            else {
                                iS_REQ_HEAD_ASYNC.ZZREQTIME = parseParame["IS_REQ_HEAD_ASYNC"]["ZZREQTIME"].ToString();

                            }

                        }

                        var time = DateTime.Now;
                        var isTime=DateTime.TryParse(parseParame["IS_REQ_HEAD_ASYNC"]["ZZATTR1"].ToString(),out time);
                        if (isTime)
                        {
                            iS_REQ_HEAD_ASYNC.ZZATTR1= time.ToString("yyyy-MM-ddTHH:mm:ss");
                         
                        }
                        if (parseParame["IT_DATA"] != null && parseParame["IT_DATA"]["item"] != null)
                        {
                            XmlSerializer serializer = null;
                            var count = parseParame["IT_DATA"]["item"].Count();
                            for (int i = 0; i < count; i++)
                            {

                                item item = new item() {
                                    ZZMSG = "成功",
                                  ZZSTAT=GDCMasterDataReceiveApi.Domain.Shared.Const.ResponseStatus.SUCCESS,
                                };
                                if (parseParame["IT_DATA"]["item"][i]["ZZSERIAL"] != null)
                                {
                                    item.ZZSERIAL = parseParame["IT_DATA"]["item"][i]["ZZSERIAL"].ToString();
                                }

                               businessParame+=FormortXml(typeof(item), null, item);
                                 
                            }
                         headParame+= FormortXml(typeof(IS_REQ_HEAD_ASYNC), iS_REQ_HEAD_ASYNC, null);
                        }
                      
                    }
                    #endregion

                    #region 异步通知
                    if (methodName != "PersonDataAsync" ||methodName!= "InstitutionDataAsync")
                    {
                        NotifyAsync(headParame, businessParame, logger);
                    }
                    #endregion
            }
                #endregion

                #region 更新请求日志

                if (!string.IsNullOrWhiteSpace(requestParame))
                {
                    receiveRecordLog = new ReceiveRecordLog()
                    {
                        Id = receiceRecordId,
                        ReceiveNumber = parameCount,
                        RequestParame = requestParame,
                        SuccessNumber = parameCount,
                        Traceidentifier = traceIdentifier,
                        ReceiveType = receiveDataType,
                    };
                   await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Update);
                }
                #endregion

            }
                catch (Exception ex)
            {
                //这里不处理  会自动向上抛

            }

        }


        /// <summary>
        /// 处理json
        /// </summary>
        /// <param name="t"></param>
        /// <param name="iS_REQ_HEAD_ASYNC"></param>
        /// <param name="item"></param>
        /// <returns></returns>

        public static string FormortXml(Type t, IS_REQ_HEAD_ASYNC iS_REQ_HEAD_ASYNC,item item)
        {
            XmlSerializer serializer = new XmlSerializer(t);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                if (iS_REQ_HEAD_ASYNC != null)
                {
                    serializer.Serialize(memoryStream, iS_REQ_HEAD_ASYNC);
                }

                if (item != null)
                {
                    serializer.Serialize(memoryStream, item);
                }

                // 将MemoryStream的指针重置到开头  
                memoryStream.Position = 0;
                using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    string xml = reader.ReadToEnd();
                    if (xml.StartsWith("<?xml"))
                    {
                        int endIndex = xml.IndexOf("?>") + 2;   
                        xml = xml.Substring(endIndex).Trim(); 
                    }
                    return xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"","");
                }
            }
        }

        /// <summary>
        /// 模拟证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // 永远返回true以允许所有证书
            return true;
        }


        /// <summary>
        /// 异步通知
        /// </summary>
        /// <param name="headParame"></param>
        /// <param name="businessParame"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        private async Task NotifyAsync(string headParame,string businessParame, ILogger<ReceiveServiceInterceptor> logger)
        {
            var requestBody = Utils.SoapFormat(headParame, businessParame);
            //请求地址
            var url = AppsettingsHelper.GetValue("MDMAsyncResultApi");

            #region RestClient写法  有问题
            RestClientOptions restClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri(url),
                RemoteCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; }
            };
            using (var client = new RestClient(restClientOptions))
            {
                var resultRequest = new RestRequest("", Method.Post);
                resultRequest.AddHeader("Content-Type", "application/xml");
                resultRequest.AddParameter("application/xml", requestBody, ParameterType.RequestBody);
                var apiResponse = await client.ExecutePostAsync(resultRequest);
                if (logger != null)
                {
                    logger.LogInformation($"接口异步通知回调结果:{apiResponse.Content.ToJson()}");
                }
            }
            #endregion

            #region WebHelper写法

            //   WebHelper webHelper = new WebHelper();
            //   webHelper.Headers.Add("Content-Type", "application/xml");
            //   webHelper.UseDefaultCredentials= true;
            //   //webHelper.Credentials = true;
            //   var a= await webHelper.DoPostAsync(url);
            //// var a= await webHelper.DoPostAsync("https://localhost:8040/api/DataSecurity/AuthSystemInterface");
            #endregion


            #region HttpClient
            //using (var client = new HttpClient())
            //{
            //    //// 你的XML内容
            //    //string xmlContent = "<note><to>Tove</to><from>Jani</from><heading>Reminder</heading><body>Don't forget me this weekend!</body></note>";

            //    //// 创建StringContent实例，并设置MediaType
            //    //var content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

            //    // 发送POST请求
            //    client.GetAsync("https://localhost:8040/api/DataSecurity/AuthSystemInterface");


            //}
            #endregion
        }
    }
}
