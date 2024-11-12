using GDCDataSecurityApi.Application.Contracts.Dto.IncrementalData;
using GDCInterfaceApi.Application.Contracts.Dto.IncrementalData;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.IncrementalData;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using SqlSugar;
using SqlSugar.Extensions;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.SearchService
{
    /// <summary>
    /// 统计数据列表实现
    /// </summary>
    public class IncrementalDataSearchService : IIncrementalDataSearchService
    {

        #region 依赖注入
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public IncrementalDataSearchService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        /// <summary>
        /// 公用值
        /// </summary>
        public class TResult
        {
            public long Id { get; set; }
            public DateTime? CreateTime { get; set; }
            public DateTime? UpdateTime { get; set; }
        }

        #region 列表接口
        /// <summary>
        /// 获取每日增量数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<IncrementalDataDto>> GetIncrementalSearchAsync(IncrementalSearchRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<IncrementalDataDto>();
            var result = new IncrementalDataDto();
            var item = new List<IncrementalSearchResponse>();

            requestDto.ResetModelProperty();
            Utils.TryConvertDateTimeFromDateDay(requestDto.DateDay, out DateTime time);

            //前六+当日  七天
            var sevDay = time.AddDays(-6).ToDateDay();
            Utils.TryConvertDateTimeFromDateDay(sevDay, out DateTime sevTime);

            List<TResult> insertList = new();
            List<TResult> updateList = new();

            switch (requestDto.TableName)
            {
                case Domain.Shared.Enums.TableNameType.User:
                    // 获取新增的数据
                    insertList = await _dbContext.Queryable<User>()
                                        .Where(t => !string.IsNullOrWhiteSpace(t.CreateTime.ToString()) && t.IsDelete == 1 && t.CreateTime.Value.Date >= sevTime && t.CreateTime.Value.Date <= time)
                                        .Select(x => new TResult { CreateTime = x.CreateTime, Id = x.Id })
                                        .ToListAsync();
                    //获取修改的数据
                    updateList = await _dbContext.Queryable<User>()
                       .Where(t => !string.IsNullOrWhiteSpace(t.UpdateTime.ToString()) && t.IsDelete == 1 && t.UpdateTime.Value.Date >= sevTime && t.UpdateTime.Value.Date <= time)
                       .Select(x => new TResult { UpdateTime = x.UpdateTime, Id = x.Id })
                       .ToListAsync();
                    break;
                case Domain.Shared.Enums.TableNameType.Institution:
                    insertList = await _dbContext.Queryable<Institution>()
                                        .Where(t => !string.IsNullOrWhiteSpace(t.CreateTime.ToString()) && t.IsDelete == 1 && t.CreateTime.Value.Date >= sevTime && t.CreateTime.Value.Date <= time)
                                        .Select(x => new TResult { CreateTime = x.CreateTime, Id = x.Id })
                                        .ToListAsync();
                    //获取修改的数据
                    updateList = await _dbContext.Queryable<Institution>()
                       .Where(t => !string.IsNullOrWhiteSpace(t.UpdateTime.ToString()) && t.IsDelete == 1 && t.UpdateTime.Value.Date >= sevTime && t.UpdateTime.Value.Date <= time)
                       .Select(x => new TResult { UpdateTime = x.UpdateTime, Id = x.Id })
                       .ToListAsync();
                    break;
            }

            for (int i = 0; i < 7; i++)
            {
                //初始化
                List<string> incDetails = new();

                //数据
                var tabInsertList = insertList.Where(x => x.CreateTime.Value.Date == sevTime).Select(x => x.Id.ToString()).ToList();
                var tabUpdateList = updateList.Where(x => x.UpdateTime.Value.Date == sevTime).Select(x => x.Id.ToString()).ToList();
                incDetails.AddRange(tabInsertList);
                incDetails.AddRange(tabUpdateList);

                //创建
                item.Add(new IncrementalSearchResponse
                {
                    DetailsIds = incDetails,
                    ChangeNums = tabInsertList.Count + tabUpdateList.Count,
                    TimeValue = sevTime.ToString("yyyy-MM-dd")
                });
                sevTime = sevTime.AddDays(1);
            }

            result = new IncrementalDataDto
            {
                Item = item
            };

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }


        #endregion

        #region 首页统计主数据数量
        /// <summary>
        /// 首页统计主数据数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<EachMainDataCountResponseDto>> SearchEachMainDataCountAsync()
        {
            ResponseAjaxResult<EachMainDataCountResponseDto> responseAjaxResult = new ResponseAjaxResult<EachMainDataCountResponseDto>();
            var personCount = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1).CountAsync();
            var institutionCount = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).CountAsync();
            var corresUnitCount = await _dbContext.Queryable<CorresUnit>().Where(x => x.IsDelete == 1).CountAsync();
            var projectCount = await _dbContext.Queryable<DHProjects>().Where(x => x.IsDelete == 1).CountAsync();
            var financialInstitutionCount = await _dbContext.Queryable<FinancialInstitution>().Where(x => x.IsDelete == 1).CountAsync();
            var deviceClassCodeCount = await _dbContext.Queryable<DeviceClassCode>().Where(x => x.IsDelete == 1).CountAsync();
            var scientifiCNoProjectCount = await _dbContext.Queryable<DHResearch>().Where(x => x.IsDelete == 1).CountAsync();
            var businessNoCpportunityCount = await _dbContext.Queryable<DHOpportunity>().Where(x => x.IsDelete == 1).CountAsync();
            EachMainDataCountResponseDto eachMainDataCountResponseDto = new EachMainDataCountResponseDto()
            {
                MainBusinessNoCpportunityCount = businessNoCpportunityCount,
                MainCorresUnitCount = corresUnitCount,
                MainDeviceClassCodeCount = deviceClassCodeCount,
                MainFinancialInstitutionCount = financialInstitutionCount,
                MainInstitutionCount = institutionCount,
                MainPersonCount = personCount,
                MainProjectCount = projectCount,
                MainScientifiCNoProjectCount = scientifiCNoProjectCount
            };
            responseAjaxResult.Count = 1;
            responseAjaxResult.Data = eachMainDataCountResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion


        #region  首页各个公司主数据统计数量
        /// <summary>
        /// 首页各个公司主数据统计数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<EachCompanyMainDataCountResponseDto>>> SearchEachCompanyMainDataCountAsync(int type)
        {
            ResponseAjaxResult<List<EachCompanyMainDataCountResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<EachCompanyMainDataCountResponseDto>>();
            List<EachCompanyMainDataCountResponseDto> eachCompanyMainDataCountResponseDtos = new List<EachCompanyMainDataCountResponseDto>();
            var companyList = new CommonData().InitCompany();
            //获取机构数据
            var institutionList = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.STATUS == "1")
                .Select(x => new InstitutionTree()
                {
                    Oid = x.OID,
                    POid = x.POID,
                    ShortName = x.SHORTNAME,
                    Name = x.NAME,
                    Sno = x.SNO,
                    GPoid = x.GPOID
                }).ToListAsync();
            #region 人员数量
            if (type == 1)
            {
                //获取人员数据
                var userList = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1).ToListAsync();
                foreach (var item in companyList)
                {
                    var oids = new ListToTreeUtil().GetAllNodes(item.Key, institutionList);
                    var userCount = userList.Where(x => oids.Contains(x.OFFICE_DEPID)).Count();
                    EachCompanyMainDataCountResponseDto eachCompanyMainDataCountResponseDto = new EachCompanyMainDataCountResponseDto()
                    {
                        Type = type,
                        ConpanyName = item.Value,
                        XAxis = item.Value,
                        YAxis = userCount
                    };
                    eachCompanyMainDataCountResponseDtos.Add(eachCompanyMainDataCountResponseDto);
                }

            }
            #endregion

            #region 机构数量
            if (type == 2)
            {
                foreach (var item in companyList)
                {
                    var oids = new ListToTreeUtil().GetAllNodes(item.Key, institutionList);
                    EachCompanyMainDataCountResponseDto eachCompanyMainDataCountResponseDto = new EachCompanyMainDataCountResponseDto()
                    {
                        Type = type,
                        ConpanyName = item.Value,
                        XAxis = item.Value,
                        YAxis = oids.Count
                    };
                    eachCompanyMainDataCountResponseDtos.Add(eachCompanyMainDataCountResponseDto);
                }

            }
            #endregion

            #region 项目数量
            if (type == 3)
            {
                //获取机构数据
                var projectList = await _dbContext.Queryable<DHProjects>().Where(x => x.IsDelete == 1)
                    .ToListAsync();
                foreach (var item in companyList)
                {
                    var oids = new ListToTreeUtil().GetAllNodes(item.Key, institutionList);
                    var userCount = projectList.Where(x => oids.Contains(x.ZPRO_ORG)).Count();
                    EachCompanyMainDataCountResponseDto eachCompanyMainDataCountResponseDto = new EachCompanyMainDataCountResponseDto()
                    {
                        Type = type,
                        ConpanyName = item.Value,
                        XAxis = item.Value,
                        YAxis = userCount
                    };
                    eachCompanyMainDataCountResponseDtos.Add(eachCompanyMainDataCountResponseDto);
                }

            }
            #endregion

            responseAjaxResult.Data = eachCompanyMainDataCountResponseDtos.OrderBy(x => x.Type).ToList();
            responseAjaxResult.Count = eachCompanyMainDataCountResponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion


        #region 首页各个主数据增长数量
        /// <summary>
        /// 首页各个主数据增长数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<EachMainDataCountResponseDto>>> SearchEachDayMainDataCountAsync(string timeStr, string timeEnd)
        {
            ResponseAjaxResult<List<EachMainDataCountResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<EachMainDataCountResponseDto>>();
            List<EachMainDataCountResponseDto> eachMainDataCountResponseDtos = new List<EachMainDataCountResponseDto>();
            DateTime startTime = default(DateTime);
            DateTime endTime = default(DateTime);

            if (string.IsNullOrWhiteSpace(timeStr) || string.IsNullOrWhiteSpace(timeEnd))
            {
                startTime = DateTime.Now.AddDays(-6);
                endTime = DateTime.Now;
            }
            else
            {
                startTime = Convert.ToDateTime(timeStr);
                endTime = Convert.ToDateTime(Convert.ToDateTime(timeEnd).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            var personCount = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();


            var institutionCount = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();

            List<CorresUnitDetailsDto> corresUnitCount = new List<CorresUnitDetailsDto>();
            var redis = RedisUtil.Instance;
            var res = redis.Get("corresUnitCount");
            if (!string.IsNullOrWhiteSpace(res))
            {
                corresUnitCount = JsonConvert.DeserializeObject<List<CorresUnitDetailsDto>>(res);
            }
            else
            {
                corresUnitCount = await _dbContext.Queryable<CorresUnit>().Where(x => x.IsDelete == 1
              && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).Select(x => new CorresUnitDetailsDto { CreateTime = x.CreateTime }).ToListAsync();
                try
                {
                    redis.Set("corresUnitCount", corresUnitCount, 60 * 30);
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"出现异常:{ex}");

                }

            }


            var projectCount = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();

            var financialInstitutionCount = await _dbContext.Queryable<FinancialInstitution>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();

            var deviceClassCodeCount = await _dbContext.Queryable<DeviceClassCode>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();

            var scientifiCNoProjectCount = await _dbContext.Queryable<ScientifiCNoProject>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();

            var businessNoCpportunityCount = await _dbContext.Queryable<BusinessCpportunity>().Where(x => x.IsDelete == 1
            && SqlFunc.ToDate(x.CreateTime) >= startTime && SqlFunc.ToDate(x.CreateTime) <= endTime).ToListAsync();

            for (int i = 0; i <7; i++)
            {
                var startTimeStr = Convert.ToDateTime(startTime.ObjToDate().AddDays(i).ToString("yyyy-MM-dd 00:00:00.000001"));
                var endTimeStr = Convert.ToDateTime(startTime.AddDays(i).ToString("yyyy-MM-dd 23:59:59.999999"));

                EachMainDataCountResponseDto eachMainDataCountResponseDto = new EachMainDataCountResponseDto()
                {
                    Day = startTimeStr.ToString("yyyy-MM-dd"),
                    MainBusinessNoCpportunityCount = businessNoCpportunityCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainCorresUnitCount = corresUnitCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainDeviceClassCodeCount = deviceClassCodeCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainFinancialInstitutionCount = financialInstitutionCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainInstitutionCount = institutionCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainPersonCount = personCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainProjectCount = projectCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                    MainScientifiCNoProjectCount = scientifiCNoProjectCount.Where(x => SqlFunc.ToDate(x.CreateTime) >= startTimeStr && SqlFunc.ToDate(x.CreateTime) <= endTimeStr).Count(),
                };
                eachMainDataCountResponseDtos.Add(eachMainDataCountResponseDto);
            }
            responseAjaxResult.Count = eachMainDataCountResponseDtos.Count();
            responseAjaxResult.Data = eachMainDataCountResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion


        #region 首页API统计
        /// <summary>
        /// 首页API统计
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<EachAPIInterdaceCountResponseDto>>> SearchCallInterfaceCountAsync(string timeStr, string timeEnd, int type)
        {
            ResponseAjaxResult<List<EachAPIInterdaceCountResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<EachAPIInterdaceCountResponseDto>>();
            List<EachAPIInterdaceCountResponseDto> eachMainDataCountResponseDtos = new List<EachAPIInterdaceCountResponseDto>();

            #region 基本参数
            DateTime startTime = default(DateTime);
            DateTime endTime = default(DateTime);
            if (string.IsNullOrWhiteSpace(timeStr) || string.IsNullOrWhiteSpace(timeEnd))
            {
                startTime = DateTime.Now.AddDays(-6);
                endTime = DateTime.Now;
            }
            else
            {
                startTime = Convert.ToDateTime(timeStr);
                endTime = Convert.ToDateTime(timeEnd).AddDays(1);
            }
           new CacheHelper().Set("time1", startTime.ToString("yyyy-MM-dd HH:mm:ss"), int.MaxValue);
           new CacheHelper().Set("time2", endTime.ToString("yyyy-MM-dd HH:mm:ss"), int.MaxValue);
            #endregion

            //按接口
            var auditLogList = await _dbContext.Queryable<AuditLogs>()
                .Where(x => x.AppKey!=null
                 && SqlFunc.ToDate(x.RequestTime) >= startTime && SqlFunc.ToDate(x.RequestTime) < endTime && x.HttpStatusCode == 200)
                .ToListAsync();
            if (auditLogList.Count > 0)
            {
                //int max = 0;
                //if (type ==1)
                //{
                //    max = 7;
                //}
                //else {
                //    max = auditLogList.Select(x => x.AppKey).Distinct().Count();
                //}
                for (var i = 0; i < 7; i++)
                {
                    var startTimeStr = Convert.ToDateTime(startTime.ObjToDate().AddDays(i).ToString("yyyy-MM-dd 00:00:00.000001"));
                    var endTimeStr = Convert.ToDateTime(startTime.AddDays(i).ToString("yyyy-MM-dd 23:59:59.999999"));

                    #region http请求获取接口信息数据
                    var systemUrl = AppsettingsHelper.GetValue("API:SystemInfo");
                    var interfaceUrl = AppsettingsHelper.GetValue("API:SystemInterfaceInfo");
                    WebHelper webHelper = new WebHelper();
                    var responseSystem = await webHelper.DoGetAsync<ResponseAjaxResult<List<SystemResponseDto>>>(systemUrl);
                    Dictionary<string, object> parames = new Dictionary<string, object>();
                    parames.Add("keyWords", "B1C548CD-EFB0-4BC3-9224-877CA5A2715C");
                    var responseInterface = await webHelper.DoGetAsync<ResponseAjaxResult<List<DataInterfaceResponseDto>>>(interfaceUrl, parames);
                    var systemApiList = new List<SystemResponseDto>();
                    var systemINterfaceApiList = new List<DataInterfaceResponseDto>();
                    try
                    {
                        systemApiList = responseSystem.Result.Data;
                        systemINterfaceApiList = responseInterface.Result.Data;
                    }
                    catch (Exception ex)
                    {


                    }
                    #endregion

                    List<EachAPIInterdaceItem> eachMainDataCountResponseDto = new List<EachAPIInterdaceItem>();
                    var exp = auditLogList.Where(x => SqlFunc.ToDate(x.RequestTime) >= startTimeStr
                       && SqlFunc.ToDate(x.RequestTime) <= endTimeStr);
                    if (type == 1)
                    {
                        EachAPIInterdaceCountResponseDto eachInterdaceCountResponseDto = new EachAPIInterdaceCountResponseDto()
                        {
                            XAxis = startTimeStr.ToString("yyyy-MM-dd"),
                            YAxis = exp.Count(),
                        };
                        eachMainDataCountResponseDtos.Add(eachInterdaceCountResponseDto);
                    }
                    else if (type == 2)
                    {
                        foreach (var item in systemApiList)
                        {
                            EachAPIInterdaceCountResponseDto eachInterdaceCountResponseDto = new EachAPIInterdaceCountResponseDto()
                            {
                                AppKey = item.AppKey,
                                XAxis = item.SystemName,
                                YAxis = exp.Where(x=>x.AppKey==item.AppKey).Count(),

                            };
                            if (eachMainDataCountResponseDtos.Where(x => x.AppKey == item.AppKey).Count() > 0)
                            {
                                eachMainDataCountResponseDtos.Where(x => x.AppKey == item.AppKey).FirstOrDefault().YAxis += eachInterdaceCountResponseDto.YAxis;
                            }
                            else {
                                eachMainDataCountResponseDtos.Add(eachInterdaceCountResponseDto);
                            }
                        
                        }
                        
                    }
                }
            }
            responseAjaxResult.Count = eachMainDataCountResponseDtos.Count;
            responseAjaxResult.Data = eachMainDataCountResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion


        #region 首页API统计下钻
        /// <summary>
        /// 首页API统计下钻
        /// </summary>
        /// <param name="timeStr"></param>
        /// <param name="timeEnd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<EachAPIInterdaceItem>>> SearchRunHoleAsync(string timeStr, int type, string appKey)
        {
            ResponseAjaxResult<List<EachAPIInterdaceItem>> responseAjaxResult = new ResponseAjaxResult<List<EachAPIInterdaceItem>>();
            List<EachAPIInterdaceItem> eachMainDataCountResponseDtos = new List<EachAPIInterdaceItem>();
            var strtimeStr = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00.000001"));
            var endTimeStr = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59.999999"));
            #region 基本参数
            if (!string.IsNullOrWhiteSpace(timeStr))
            {
                strtimeStr = Convert.ToDateTime(timeStr.ObjToDate().ToString("yyyy-MM-dd 00:00:00.000001"));
                 endTimeStr = Convert.ToDateTime(timeStr.ObjToDate().ToString("yyyy-MM-dd 23:59:59.999999"));
            }
            if (type == 2)
            {
                strtimeStr=  new CacheHelper().Get("time1").Replace("\"","").ObjToDate();
                endTimeStr = new CacheHelper().Get("time2").Replace("\"", "").ObjToDate();
                await Console.Out.WriteLineAsync(strtimeStr.ToString());
                await Console.Out.WriteLineAsync(endTimeStr.ToString());
            }
            #endregion


            //按接口
            var auditLogList = await _dbContext.Queryable<AuditLogs>()
                .Where(x => x.AppKey != null && x.HttpStatusCode == 200
                 && SqlFunc.ToDate(x.RequestTime) >= strtimeStr
                 && SqlFunc.ToDate(x.RequestTime)<= endTimeStr)
                .WhereIF(type==2,x=>x.AppKey == appKey)
                .ToListAsync();
            if (auditLogList.Count > 0)
            {
                #region http请求获取接口信息数据
                var systemUrl = AppsettingsHelper.GetValue("API:SystemInfo");
                var interfaceUrl = AppsettingsHelper.GetValue("API:SystemInterfaceInfo");
                WebHelper webHelper = new WebHelper();
                var responseSystem = await webHelper.DoGetAsync<ResponseAjaxResult<List<SystemResponseDto>>>(systemUrl);
                Dictionary<string, object> parames = new Dictionary<string, object>();
                parames.Add("keyWords", "B1C548CD-EFB0-4BC3-9224-877CA5A2715C");
                var responseInterface = await webHelper.DoGetAsync<ResponseAjaxResult<List<DataInterfaceResponseDto>>>(interfaceUrl, parames);
                var systemApiList = new List<SystemResponseDto>();
                var systemINterfaceApiList = new List<DataInterfaceResponseDto>();
                try
                {
                    systemApiList = responseSystem.Result.Data;
                    systemINterfaceApiList = responseInterface.Result.Data;
                }
                catch (Exception ex)
                {


                }
                #endregion

                if (type == 1)
                {
                    var aa=auditLogList.Select(x => x.AppKey).ToList();
                    foreach (var item in systemApiList)
                    {

                        var systemList = auditLogList.Where(x => x.AppKey == item.AppKey).ToList();
                        foreach (var interfaces in systemList)
                        {
                            var interfaceName = systemINterfaceApiList.Where(x => x.AppinterfaceCode == interfaces.AppinterfaceCode).Select(x => x.InterfaceName).ToList();
                            if (interfaceName.Count== 0)
                            {
                                continue;
                            }
                            EachAPIInterdaceItem eachAPIInterdaceItem = new EachAPIInterdaceItem()
                            {
                                AppName = item.SystemName,
                                Count = interfaceName.Count,
                                InterfaceName = interfaceName[0],
                                RequestTime = string.IsNullOrWhiteSpace(timeStr) == true ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : timeStr,
                            };
                            if (eachMainDataCountResponseDtos.Where(x => x.AppName == item.SystemName&&x.InterfaceName== interfaceName[0]).Count() > 0)
                            {

                                eachMainDataCountResponseDtos.Where(x => x.AppName == item.SystemName && x.InterfaceName == interfaceName[0]).FirstOrDefault().Count += interfaceName.Count;
                            }
                            else
                            {
                                eachMainDataCountResponseDtos.Add(eachAPIInterdaceItem);
                            }
                           

                        }
                    }

                }
                else if (type == 2)
                {
                    var systemNameList = auditLogList.Where(x => x.AppKey == appKey).ToList();
                    var systemName= systemApiList.Where(x => x.AppKey == appKey).FirstOrDefault();
                    foreach (var interfaces in systemNameList)
                    {
                        var interfaceName = systemINterfaceApiList.Where(x => x.AppinterfaceCode == interfaces.AppinterfaceCode).Select(x => x.InterfaceName).ToList();
                        if (interfaceName.Count == 0)
                        {
                            continue;
                        }
                        EachAPIInterdaceItem eachAPIInterdaceItem = new EachAPIInterdaceItem()
                        {
                            AppName = systemApiList.Where(x => x.AppKey == appKey).FirstOrDefault()?.SystemName,
                            Count = interfaceName.Count,
                            InterfaceName = interfaceName[0],
                            RequestTime = interfaces.RequestTime,
                        };


                        if (eachMainDataCountResponseDtos.Where(x => x.AppName == systemName.SystemName && x.InterfaceName == interfaceName[0]).Count() > 0)
                        {

                            eachMainDataCountResponseDtos.Where(x => x.AppName == systemName.SystemName && x.InterfaceName == interfaceName[0]).FirstOrDefault().Count += interfaceName.Count;
                        }
                        else
                        {
                            eachMainDataCountResponseDtos.Add(eachAPIInterdaceItem);
                        }
                        
                    }
                }
            }
            responseAjaxResult.Count = eachMainDataCountResponseDtos.Count;
            responseAjaxResult.Data = eachMainDataCountResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion
    }
}
