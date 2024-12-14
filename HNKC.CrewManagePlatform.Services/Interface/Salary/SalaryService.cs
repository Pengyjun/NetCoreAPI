using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Extensions;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using SqlSugar.Extensions;
using UtilsSharp;
using salary = HNKC.CrewManagePlatform.SqlSugars.Models;

namespace HNKC.CrewManagePlatform.Services.Interface.Salary
{
    public class SalaryService :HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService,ISalaryService
    {
        #region 依赖注入
        private readonly ISqlSugarClient dbContext;
        private readonly IMapper mapper;
        public SalaryService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
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
                 .InnerJoin<salary.Salary>((x, y) =>  x.Id == y.UserId)
                  .WhereIF(salaryRequest.Year.HasValue,(x,y)=>y.Year== salaryRequest.Year)
                  .WhereIF(salaryRequest.Month.HasValue,(x,y)=>y.Month== salaryRequest.Month)
                  .WhereIF(!string.IsNullOrWhiteSpace(salaryRequest.Name),(x,y)=>x.Name.Contains(salaryRequest.Name))
                  .WhereIF(!string.IsNullOrWhiteSpace(salaryRequest.WorkNumber),(x,y)=>x.WorkNumber.Contains(salaryRequest.WorkNumber))
                  .WhereIF(!string.IsNullOrWhiteSpace(salaryRequest.Phone),(x,y)=>x.Phone.Contains(salaryRequest.Phone))
                  .Where((x,y)=>x.IsDelete==1&&y.IsDelete==1)
                 .Select((x, y) => new SalaryResponse {
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
                 .OrderByDescending(z => new {z.Year,z.Month })
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
               .Select((x, y) => new SalaryPushResponse() {
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
               .Where((x, y) => x.IsDelete == 1&&y.UserId==id)
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
            salary.Salary salary = new salary.Salary();
            #region 基础信息
            string pushTime =string.Empty;
            var decrRes = (CryptoStringExtension.DecryptAsync(sign)).Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
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
               pushTime=await dbContext.Queryable<SalaryPushRecord>().Where(x => x.UserId == user.Id && x.Result == (int)PushResultEnum.Success && x.IsDelete == 1&& x.Year == year && x.Month == month).Select(x=>x.CreatedBy).FirstAsync();
                //工资信息
                 salary = await dbContext.FirstAsync<salary.Salary>(x => x.IsDelete == 1 && x.UserId == user.Id && x.Month == month && x.Year == year);

             }
            salaryAsExcelResponse = mapper.Map<salary.Salary, SalaryAsExcelResponse>(salary);
            if (salaryAsExcelResponse != null)
            {
               var endTime= Convert.ToDateTime(pushTime).AddDays(3);
                salaryAsExcelResponse.CutOffTime =Math.Round(TimeHelper.GetTimeSpan(DateTime.Now, endTime).TotalSeconds,0);
                salaryAsExcelResponse.Name = user?.Name;
                salaryAsExcelResponse.Phone = user?.Phone;
                salaryAsExcelResponse.WorkNumber = user?.WorkNumber;
                salaryAsExcelResponse.DepartmentName = user?.Oid;
            }
            return salaryAsExcelResponse;
        }
    }
}
