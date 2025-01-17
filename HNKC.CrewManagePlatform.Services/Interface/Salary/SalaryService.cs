using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.Sms.Interfaces;
using HNKC.CrewManagePlatform.Sms.Model;
using HNKC.CrewManagePlatform.SqlSugars.Extensions;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Xml.Linq;
using UtilsSharp;
using salary = HNKC.CrewManagePlatform.SqlSugars.Models;

namespace HNKC.CrewManagePlatform.Services.Interface.Salary
{
    public class SalaryService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, ISalaryService
    {
        #region 依赖注入
        private readonly ISqlSugarClient dbContext;
        private readonly IMapper mapper;
        public ISmsService smsService { get; set; }
        public SalaryService(ISqlSugarClient dbContext, IMapper mapper, ISmsService smsService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.smsService = smsService;
        }

        #endregion

        /// <summary>
        /// 工资列表查询
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageResult<SalaryResponse>> SearchSalaryListAsync(SalaryRequest salaryRequest)
        {
            if (!salaryRequest.Month.HasValue)
            {
                salaryRequest.Month = DateTime.Now.Month;
            }
            RefAsync<int> total = 0;
            return await dbContext.Queryable<User>()
                 .InnerJoin<salary.Salary>((x, y) => x.Id == y.UserId)
                  .WhereIF(salaryRequest.Year.HasValue, (x, y) => y.Year == salaryRequest.Year)
                  .WhereIF(salaryRequest.Month.HasValue, (x, y) => y.Month == salaryRequest.Month)
                  .WhereIF(!string.IsNullOrWhiteSpace(salaryRequest.Name), (x, y) => x.Name.Contains(salaryRequest.Name))
                  .WhereIF(!string.IsNullOrWhiteSpace(salaryRequest.WorkNumber), (x, y) => x.WorkNumber.Contains(salaryRequest.WorkNumber))
                  .WhereIF(!string.IsNullOrWhiteSpace(salaryRequest.Phone), (x, y) => x.Phone.Contains(salaryRequest.Phone))
                  .Where((x, y) => x.IsDelete == 1 && y.IsDelete == 1)
                 .Select((x, y) => new SalaryResponse
                 {
                     BId=x.BusinessId,
                     Id = x.Id.ToString(),
                     BaseWage = y.BaseWage,
                     CardId = x.CardId,
                     CertificateSubsidy = y.CertificateSubsidy,
                     Year = y.Year,
                     Month = y.Month,
                     DepartmentName = x.Oid,
                     HolidaysWage = y.HolidaysWage,
                     MasterApprenticeSubsidy = y.MasterApprenticeAllowance,
                     MonthPerformance = y.MonthPerformance,
                     Name = x.Name,
                     NameSubsidy = y.NameSubsidy,
                     OtherWage = y.OtherWage,
                     OvertimeWage = y.OvertimeWage,
                     Phone = x.Phone,
                     PostWage = y.PostWage,
                     QuarterPerformance = y.QuarterPerformance,
                     RankWage = y.RankWage,
                     RegularWage = y.RegularWage,
                     ReissueBuckleMoney = y.ReissueBuckleMoney,
                     SkillSubsidy = y.SkillSubsidy,
                     SkillWage = y.SkillWage,
                     TrainPerformance = y.TrainPerformance,
                     WorkAgeWage = y.WorkAgeWage,
                     WorkNumber = x.WorkNumber
                 }).MergeTable()
                 .OrderByDescending(z => new { z.Year, z.Month })
                .ToPageResultAsync(salaryRequest.PageIndex, salaryRequest.PageSize, total);
        }


        /// <summary>
        /// 获取全部短信推送记录
        /// </summary>
        /// <param name="salaryPushRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageResult<SalaryPushResponse>> SearchSalaryPushRecordAsync(SalaryPushRequest salaryPushRequest)
        {
            RefAsync<int> total = 0;
            return await dbContext.Queryable<SalaryPushRecord>()
               .LeftJoin<User>((x, y) => x.UserId == y.Id)
               .WhereIF(!string.IsNullOrWhiteSpace(salaryPushRequest.Name), (x, y) => y.Name.Contains(salaryPushRequest.Name))
               .WhereIF(!string.IsNullOrWhiteSpace(salaryPushRequest.WorkNumber), (x, y) => y.WorkNumber.Contains(salaryPushRequest.WorkNumber))
               .WhereIF(!string.IsNullOrWhiteSpace(salaryPushRequest.Phone), (x, y) => y.Phone.Contains(salaryPushRequest.Phone))
               .WhereIF(!string.IsNullOrWhiteSpace(salaryPushRequest.Oid), (x, y) => y.Oid.Contains(salaryPushRequest.Oid))
               .WhereIF(salaryPushRequest.PushTime.HasValue, (x, y) => x.Created == salaryPushRequest.PushTime.Value)
               .WhereIF(salaryPushRequest.PushResult.HasValue, (x, y) => x.Result == salaryPushRequest.PushResult.Value)
               .WhereIF(salaryPushRequest.BusinessType.HasValue, (x, y) => x.BusinessType == salaryPushRequest.BusinessType.Value)
               .Where((x, y) => x.IsDelete == 1)
               .Select((x, y) => new SalaryPushResponse()
               {
                   BusinessType = x.BusinessType,
                   CreateTime = x.Created,
                   DepartmentName = y.Oid,
                   Id = x.Id.ToString(),
                   Name = y.Name,
                   Phone = y.Phone,
                   PushResult = x.Result,
                   WorkNumber = y.WorkNumber
               })
              .ToPageResultAsync(salaryPushRequest.PageIndex, salaryPushRequest.PageSize, total);
        }
        /// <summary>
        /// 获取个人所有短信推送记录
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageResult<SalaryPushResponse>> GetSalaryPushRecordByUserAsync(SalaryPushRequest salaryPushRequest)
        {
            RefAsync<int> total = 0;
            var id = long.Parse(salaryPushRequest.Id);
            return await dbContext.Queryable<User>()
               .LeftJoin<SalaryPushRecord>((x, y) => x.Id == y.UserId)
               .WhereIF(salaryPushRequest.PushTime.HasValue, (x, y) => y.Created == salaryPushRequest.PushTime.Value)
               .WhereIF(salaryPushRequest.PushResult.HasValue, (x, y) => y.Result == salaryPushRequest.PushResult.Value)
               .WhereIF(salaryPushRequest.BusinessType.HasValue, (x, y) => y.BusinessType == salaryPushRequest.BusinessType.Value)
               .Where((x, y) => x.IsDelete == 1 && y.UserId == id)
               .Select((x, y) => new SalaryPushResponse()
               {
                   BusinessType = y.BusinessType,
                   CreateTime = y.Created,
                   DepartmentName = x.Oid,
                   Id = x.Id.ToString(),
                   Name = x.Name,
                   Phone = x.Phone,
                   PushResult = y.Result,
                   WorkNumber = x.WorkNumber
               })
              .ToPageResultAsync(salaryPushRequest.PageIndex, salaryPushRequest.PageSize, total);
        }


        /// <summary>
        /// 短信通知 点击链接查询
        /// </summary>
        /// <param name="sgin"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SalaryAsExcelResponse> FindUserInfoAsync(string sign)
        {
            SalaryAsExcelResponse salaryAsExcelResponse = new SalaryAsExcelResponse();
            if (string.IsNullOrWhiteSpace(sign))
            {
                return salaryAsExcelResponse;
            }
            
            salary.Salary salary = new salary.Salary();
            #region 信息校验
            var pushTimeCalc = await dbContext.Queryable<SalaryPushRecord>().Where(x => x.IsDelete == 1 && x.RandomUrl == sign).FirstAsync();
            if (pushTimeCalc==null)
            {
                return salaryAsExcelResponse;
            }
            var parseSign = WebUtility.UrlDecode(pushTimeCalc.PhoneUrl);
            if (!pushTimeCalc.Created.HasValue || (pushTimeCalc.Created.Value.AddDays(3) - DateTime.Now).Seconds <= 0)
            {
                return null;
            }
            #endregion

            #region 基础信息
            DateTime pushTime = default(DateTime);
            var decrRes = (CryptoStringExtension.DecryptAsync(parseSign)).Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            if (decrRes.Count != 3)
            {
                return salaryAsExcelResponse;
            }
            var workNumber = decrRes[0];
            var year = decrRes[1].ObjToInt();
            var month = decrRes[2].ObjToInt();
            #endregion

            //用户信息
            var user = await dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.WorkNumber == workNumber).FirstAsync();
            if (user != null)
            {
                //推送时间
                pushTime = await dbContext.Queryable<SalaryPushRecord>().Where(x => x.UserId == user.Id && x.Result == (int)PushResultEnum.Success && x.IsDelete == 1 && x.Year == year && x.Month == month).Select(x => x.Created.Value).FirstAsync();
                //工资信息
                salary = await dbContext.FirstAsync<salary.Salary>(x => x.IsDelete == 1 && x.UserId == user.Id && x.Month == month && x.Year == year);

            }
            salaryAsExcelResponse = mapper.Map<salary.Salary, SalaryAsExcelResponse>(salary);
            if (salaryAsExcelResponse != null)
            {
                var endTime = Convert.ToDateTime(pushTime).AddDays(3);
                salaryAsExcelResponse.CutOffTime = Math.Round(TimeHelper.GetTimeSpan(DateTime.Now, endTime).TotalSeconds, 0);
                salaryAsExcelResponse.Name = user?.Name;
                salaryAsExcelResponse.Phone = user?.Phone;
                salaryAsExcelResponse.WorkNumber = user?.WorkNumber;
                salaryAsExcelResponse.DepartmentName = user?.Oid;
            }
            return salaryAsExcelResponse;
        }

        /// <summary>
        /// 群发 单发
        /// </summary>
        /// <param name="baseRequest">为空群发  否则单发</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> SendSmsAllAsync(BaseRequest baseRequest)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            if (baseRequest.BId == Guid.Empty)
            {
                return Result.Fail("发送失败", (int)ResponseHttpCode.SendFail);
            }
            var salarieList = await dbContext.Queryable<salary.Salary>().Where(x => x.IsDelete == 1&&x.Year==year&&x.Month==month).ToListAsync();
            if (salarieList.Count == 0)
            {
                return Result.Fail("没有工资信息无法发送", (int)ResponseHttpCode.SendFail);
            }

            //工资推送记录
            List<SalaryPushRecord> salaryPushRecords = new List<SalaryPushRecord>();
            var salaryIds= salarieList.Select(x=>x.WorkNumber).ToList();
            //所有用户
            var allUser= await dbContext.Queryable<User>()
                 .Where(x => x.IsDelete == 1&&!SqlFunc.IsNullOrEmpty(x.Phone)&& salaryIds.Contains(x.WorkNumber))
                 .WhereIF(baseRequest != null && baseRequest.BId != null, x => x.BusinessId == baseRequest.BId).ToListAsync();

            //查询推送记录
            //所有未推送的用户
            var allPushUserId = await dbContext.Queryable<SalaryPushRecord>()
                 .Where(x => x.IsDelete == 1 && x.Year==year&&x.Month==month)
                 .ToListAsync();
            var allNoPushUserId= allPushUserId.Where(x=>x.Result==1).Select(x=>x.UserId).ToList();
            var pushUserList= allUser.Where(x =>!allNoPushUserId.Contains(x.Id)).ToList();
            if (pushUserList.Count == 0)
            {
                return Result.Fail("本月短信已发送，没有可发送的短信", (int)ResponseHttpCode.SendFail);
            }
            var len = int.Parse(AppsettingsHelper.GetValue("Length"));
            foreach (var item in pushUserList)
            {
                if (allPushUserId.Where(x => x.UserId == item.Id).Count() == 0)
                {
                    salaryPushRecords.Add(new SalaryPushRecord()
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        BusinessId = Guid.Empty,
                        Month = month,
                        Year = year,
                        UserId = item.Id,
                        Result = 0,
                        RandomUrl = RandomHelper.NumberAndLetters(len),
                        BusinessType = (baseRequest == null || baseRequest.BId == null) ? 1 : 0,
                        PhoneUrl = WebUtility.UrlEncode(CryptoStringExtension.EncryptAsync($"{item.WorkNumber},{year},{month}"))
                    });
                }
               
            }
            if (salaryPushRecords.Count > 0)
            {
                await dbContext.Insertable<SalaryPushRecord>(salaryPushRecords).ExecuteCommandAsync();
            }
            //不等待
            if (baseRequest!=null&&baseRequest.BId==null)
            {
                //群发
                SendSmsAsync(null);
            }
            else {
                if (allUser.Count > 0)
                {
                    //单发
                    SendSmsAsync(allUser[0].Phone);
                }
               
            }
              
            return Result.Success("发送成功");
       }

        /// <summary>
        /// 发短信
        /// </summary>
        /// <param name="phone">为空 群发  否则单独发</param>
        /// <returns></returns>
        public async Task<Result> SendSmsAsync(string phone)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var allNoPushUserId = await dbContext.Queryable<SalaryPushRecord>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month && x.Result != 1)
                .ToListAsync();
            if (allNoPushUserId.Count == 0)
            {
                return Result.Fail("没有可发送的短信", (int)ResponseHttpCode.SendFail);
            }
            var userIds = allNoPushUserId.Select(x => x.UserId).ToList();
            var userList=await dbContext.Queryable<User>()
                .Where(x => x.IsDelete == 1 && userIds.Contains(x.Id))
                .WhereIF(!string.IsNullOrWhiteSpace(phone),x=>x.Phone==phone)
                .ToListAsync();
            try
            {
                
                foreach (var user in allNoPushUserId)
                {
                    var userInfo = userList.Where(x => x.Id == user.UserId).FirstOrDefault();
                    if (userInfo != null)
                    {
                        var url = AppsettingsHelper.GetValue("CtyunSms:SendSmsPhoneUrl");
                        url = url.Replace("@sign", user.RandomUrl);
                        var parame = new Sms.Model.SmsRequest()
                        {
                            PhoneNumber = userInfo.Phone
                        };
                        ParameTemplate parameTemplate = new ParameTemplate()
                        {
                            Name = userInfo.Name,
                            Url = url,
                            Time = int.Parse(year.ToString() + month)
                        };
                        parame.TemplateParam = parameTemplate.ToJson();
                        var responseResult = await smsService.SendSmsAsync(parame);
                        user.Result= responseResult.IsSuccess? 1 : 0;
                        if (user.Result != 1)
                        {
                            user.Fail = responseResult.Data;
                        }

                    }
                }
                await dbContext.Updateable<SalaryPushRecord>(allNoPushUserId).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                //如果失败 把之前发送成功的 更新 防止发送重复
                 await dbContext.Updateable<SalaryPushRecord>(allNoPushUserId).ExecuteCommandAsync();
            }
            return Result.Success();
        }
    }
}
