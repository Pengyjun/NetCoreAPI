using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectYearPlan;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql.Replication;
using NPOI.SS.Formula.Functions;
using SkiaSharp;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace GHMonitoringCenterApi.Application.Service.ProjectYearPlan
{


    /// <summary>
    ///项目年初计划接口=实现层
    /// </summary>
    public class ProjectYearPlanService : IProjectYearPlanService
    {
        #region 依赖注入
        /// <summary>
        /// 匹配
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarClient _dbContext;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        public IBaseService baseService { get; set; }
        private readonly IBaseRepository<YearInitProjectPlan> yearInitProjectPlanRepository;
        public ProjectYearPlanService(IMapper _mapper, GlobalObject _globalObject, ISqlSugarClient _dbContext, IBaseService baseService,
            IBaseRepository<YearInitProjectPlan> yearInitProjectPlanRepository)
        {
            this._mapper = _mapper;
            this._globalObject = _globalObject;
            this._dbContext = _dbContext;
            this.baseService = baseService;
            this.yearInitProjectPlanRepository = yearInitProjectPlanRepository;


        }
        #endregion

        #region 获取项目年初计划列表
        /// <summary>
        /// 获取项目年初计划列表
        /// </summary>
        /// <param name="projectYearPlanRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<ProjectYearPlanResponseDto>> SearchProjectPlanAsync(ProjectYearPlanRequestDto projectYearPlanRequestDto)
        {
            #region 版本
            ResponseAjaxResult<ProjectYearPlanResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectYearPlanResponseDto>();
            ProjectYearPlanResponseDto projectYearPlanResponseDto = new ProjectYearPlanResponseDto();
            projectYearPlanResponseDto.ProjectYearPlanDetails = new List<ProjectYearPlanDetails>();
            #region 时间判断
            //当前用户机构
            var oids = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            var departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            //当前年份
            var year = 2024;
            var dataMonth = 0;
            var currentTimeInt = int.Parse(DateTime.Now.ToString("MMdd"));
            if (currentTimeInt > 1227)
            {
                year = DateTime.Now.AddYears(1).Year;
            }
            else
            {
                year = DateTime.Now.Year;
            }

            //当前月份
            //周期开始时间
            var startTime = string.Empty;
            if (DateTime.Now.Day >= 27)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            if (month.ToString().Length == 1)
            {
                dataMonth = int.Parse(year + "0" + month);
            }
            else
            {
                dataMonth = year + month;
            }
            #endregion

            #region 查询相关
            //项目状态ID 
            var projectStatusIds = CommonData.NoStatus.Split(",").Select(x => x.ToGuid()).ToList();
            RefAsync<int> total = 0;
            string[] months = new string[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
            decimal[] monthsProductionValue = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //项目相关
            projectYearPlanResponseDto.ProjectYearPlanDetails.AddRange(await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1)
                  .InnerJoin<Institution>((x, y) => x.CompanyId == y.PomId)
                 .InnerJoin<YearInitProjectPlan>((x, y, z) => x.Id == z.ProjectId)
                 .Where(x => !projectStatusIds.Contains(x.StatusId.Value) && x.ProjectDept.Value==null ||departmentIds.Contains(x.ProjectDept.Value))
                 .WhereIF(!string.IsNullOrWhiteSpace(projectYearPlanRequestDto.KeyWords), x => x.Name.Contains(projectYearPlanRequestDto.KeyWords))
                 .WhereIF(projectYearPlanRequestDto.CompanyId.HasValue && projectYearPlanRequestDto.CompanyId != Guid.Empty, x => x.CompanyId == projectYearPlanRequestDto.CompanyId)
                 .WhereIF(projectYearPlanRequestDto.ProjectDept.HasValue && projectYearPlanRequestDto.ProjectDept != Guid.Empty, x => x.ProjectDept.Value.ToString().Contains(projectYearPlanRequestDto.ProjectDept.ToString()))
                 .WhereIF(projectYearPlanRequestDto.StatusId != null && projectYearPlanRequestDto.StatusId != Guid.Empty, x => x.StatusId == (projectYearPlanRequestDto.StatusId))
                 .WhereIF(true, (x, y, z) => z.DataYear == projectYearPlanRequestDto.Year)
                 .Select((x, y, z) => new ProjectYearPlanDetails()
                 {
                      ExchangeRate=x.ExchangeRate.Value,
                     ProjectType=x.TypeId.Value,
                     CompanyId = x.CompanyId.Value,
                     //Id = z.Id,
                     ProjectStatus = x.StatusId == "0c686c96-889e-4c4d-b24d-fa2886d9dceb".ToGuid() ? "拟建" : "",
                     ProjectId = x.Id,
                     ContractAmount =x.Amount.Value,
                     ProjectName = x.Name,
                     CompanyName = y.Name,
                     //OneQuantity = z.OneQuantity.Value,
                     OneProductionValue = z.OneProductionValue.Value,
                     //TwoQuantity = z.TwoQuantity.Value,
                     TwoProductionValue = z.TwoProductionValue.Value,
                     //ThreeQuantity = z.ThreeQuantity.Value,
                     ThreeProductionValue = z.ThreeProductionValue.Value,
                     //FourQuantity = z.FourQuantity.Value,
                     FourProductionValue = z.FourProductionValue.Value,
                     //FiveQuantity = z.FiveQuantity.Value,
                     FiveProductionValue = z.FiveProductionValue.Value,
                     //SixQuantity = z.SixQuantity.Value,
                     SixProductionValue = z.SixProductionValue.Value,
                     //SevenQuantity = z.SevenQuantity.Value,
                     SevenProductionValue = z.SevenProductionValue.Value,
                     //EightQuantity = z.EightQuantity.Value,
                     EightProductionValue = z.EightProductionValue.Value,
                     //NineQuantity = z.NineQuantity.Value,
                     NineProductionValue = z.NineProductionValue.Value,
                     //TenQuantity = z.TenQuantity.Value,
                     TenProductionValue = z.TenProductionValue.Value,
                     //ElevenQuantity = z.ElevenQuantity.Value,
                     ElevenProductionValue = z.ElevenProductionValue.Value,
                     //TwelveQuantity = z.TwelveQuantity.Value,
                     TwelveProductionValue = z.TwelveProductionValue.Value,
                     YearTotalProductionValue = z.YearTotalProductionValue.Value,
                     //YearTotalQuantity = z.YearTotalQuantity.Value,
                 }).ToPageListAsync(projectYearPlanRequestDto.PageIndex, projectYearPlanRequestDto.PageSize, total));
            var dataMonthStr = dataMonth.ToString();
            var dayReportList = await _dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.DateDay.ToString().Contains(dataMonthStr)).ToListAsync();
            if (dayReportList.Any())
            {
                foreach (var item in projectYearPlanResponseDto.ProjectYearPlanDetails)
                {
                    item.ContractAmount = Math.Round((item.ExchangeRate * (item.ContractAmount) / 10000));
                    //item.ContractAmount = Math.Round(item.ContractAmount, 2);
                }
                //
                var projectIds = projectYearPlanResponseDto.ProjectYearPlanDetails.Select(x => x.ProjectId).ToList();

                var monthReportList = await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId))
                    .ToListAsync();
                var historyMonthReportList = await _dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId.Value))
                    .ToListAsync();

                foreach (var z in projectYearPlanResponseDto.ProjectYearPlanDetails)
                {
                    z.OneProductionValue = z.OneProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.OneProductionValue / 1), 2);
                    z.TwoProductionValue = z.TwoProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.TwoProductionValue / 1), 2);
                    z.ThreeProductionValue = z.ThreeProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.ThreeProductionValue / 1), 2);
                    z.FourProductionValue = z.FourProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.FourProductionValue / 1), 2);
                    z.FiveProductionValue = z.FiveProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.FiveProductionValue / 1), 2);
                    z.SixProductionValue = z.SixProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.SixProductionValue / 1), 2);
                    z.SevenProductionValue = z.SevenProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.SevenProductionValue / 1), 2);
                    z.EightProductionValue = z.EightProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.EightProductionValue / 1), 2);
                    z.NineProductionValue = z.NineProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.NineProductionValue / 1), 2);
                    z.TenProductionValue = z.TenProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.TenProductionValue / 1), 2);
                    z.ElevenProductionValue = z.ElevenProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.ElevenProductionValue / 1), 2);
                    z.TwelveProductionValue = z.TwelveProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.TwelveProductionValue / 1), 2);
                    z.YearTotalProductionValue = z.YearTotalProductionValue == null ? 0M : Math.Round(Convert.ToDecimal(z.YearTotalProductionValue / 10000), 2);
                    z.ContractAmount = Math.Round(z.ContractAmount / 1, 2);
                }

                foreach (var item in projectYearPlanResponseDto.ProjectYearPlanDetails)
                {
                    item.YData = new List<decimal>();
                    item.XData = months.ToList();
                    var currentProducitonMonth = dayReportList.Where(x => x.ProjectId == item.ProjectId)
                         .Select(x => new ProjectDayReportResponseDto
                         {
                             ProjectId = x.ProjectId,
                             DateDay = int.Parse(x.DateDay.ToString().Substring(0, 6)),
                             DayAmount = x.DayActualProductionAmount
                         }).Sum(x => x.DayAmount);
                    monthsProductionValue[month - 1] = currentProducitonMonth;
                    // item.YData = monthsProductionValue.ToList();
                    monthsProductionValue[0] = Math.Round(item.OneProductionValue/10000,2);
                    monthsProductionValue[1]=Math.Round(item.TwoProductionValue/10000,2);
                    monthsProductionValue[2]=Math.Round(item.ThreeProductionValue / 10000,2);
                    monthsProductionValue[3]=Math.Round(item.FourProductionValue / 10000, 2);
                    monthsProductionValue[4]=Math.Round(item.FiveProductionValue / 10000, 2);
                    monthsProductionValue[5]=Math.Round(item.SixProductionValue / 10000, 2);
                    monthsProductionValue[6]=Math.Round(item.SevenProductionValue / 10000, 2);
                    monthsProductionValue[7]=Math.Round(item.EightProductionValue / 10000, 2);
                    monthsProductionValue[8]=Math.Round(item.NineProductionValue / 10000, 2);
                    monthsProductionValue[9]=Math.Round(item.TenProductionValue / 10000, 2);
                    monthsProductionValue[10]= Math.Round(item.ElevenProductionValue / 10000, 2);
                    monthsProductionValue[11]= Math.Round(item.TwelveProductionValue / 10000, 2);
                    item.YData = monthsProductionValue.ToList();
                    var historyData = Sumxx(monthReportList, historyMonthReportList, item.ProjectId);
                    item.TotalProductionValue = Math.Round((historyData.Item1 + historyData.Item2 + currentProducitonMonth)/10000, 2);
                }
            }
            #endregion

            #region 合计相关
            if (projectYearPlanResponseDto.ProjectYearPlanDetails.Any())
            {

                var allProject = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1)
                  .InnerJoin<Institution>((x, y) => x.CompanyId == y.PomId)
                 .InnerJoin<YearInitProjectPlan>((x, y, z) => x.Id == z.ProjectId)
                 .Where(x => !projectStatusIds.Contains(x.StatusId.Value) && x.ProjectDept.Value == null || departmentIds.Contains(x.ProjectDept.Value))
                 .WhereIF(!string.IsNullOrWhiteSpace(projectYearPlanRequestDto.KeyWords), x => x.Name.Contains(projectYearPlanRequestDto.KeyWords))
                 .WhereIF(projectYearPlanRequestDto.CompanyId.HasValue && projectYearPlanRequestDto.CompanyId != Guid.Empty, x => x.CompanyId == projectYearPlanRequestDto.CompanyId)
                 .WhereIF(projectYearPlanRequestDto.ProjectDept.HasValue && projectYearPlanRequestDto.ProjectDept != Guid.Empty, x => x.ProjectDept.Value.ToString().Contains(projectYearPlanRequestDto.ProjectDept.ToString()))
                 .WhereIF(projectYearPlanRequestDto.StatusId != null && projectYearPlanRequestDto.StatusId != Guid.Empty, x => x.StatusId == (projectYearPlanRequestDto.StatusId))
                 .WhereIF(true, (x, y, z) => z.DataYear == projectYearPlanRequestDto.Year)
                 .Select((x, y, z) => new ProjectYearPlanDetails()
                 {
                     ExchangeRate=x.ExchangeRate.Value,
                     ProjectId = x.Id,
                     ContractAmount = x.Amount.Value
                 }).ToListAsync();
                var allIds = allProject.Select(x => x.ProjectId).ToList();
                var monthReportList = await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && allIds.Contains(x.ProjectId))
                   .ToListAsync();
                var historyMonthReportList = await _dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1 && allIds.Contains(x.ProjectId.Value))
                    .ToListAsync();

                var yearInitProjectList = await _dbContext.Queryable<YearInitProjectPlan>()
             .Where(x => x.IsDelete == 1 && allIds.Contains(x.ProjectId.Value))
             .ToListAsync();
                projectYearPlanResponseDto.TotalOneProductionValue = yearInitProjectList.Sum(x => x.OneProductionValue).Value;
                projectYearPlanResponseDto.TotalTwoProductionValue = yearInitProjectList.Sum(x => x.TwoProductionValue).Value;
                projectYearPlanResponseDto.TotalThreeProductionValue = yearInitProjectList.Sum(x => x.ThreeProductionValue).Value;
                projectYearPlanResponseDto.TotalFourProductionValue = yearInitProjectList.Sum(x => x.FourProductionValue).Value;
                projectYearPlanResponseDto.TotalFiveProductionValue = yearInitProjectList.Sum(x => x.FiveProductionValue).Value;
                projectYearPlanResponseDto.TotalSixProductionValue = yearInitProjectList.Sum(x => x.SixProductionValue).Value;
                projectYearPlanResponseDto.TotalSevenProductionValue = yearInitProjectList.Sum(x => x.SevenProductionValue).Value;
                projectYearPlanResponseDto.TotalEightProductionValue = yearInitProjectList.Sum(x => x.EightProductionValue).Value;
                projectYearPlanResponseDto.TotalNineProductionValue = yearInitProjectList.Sum(x => x.NineProductionValue).Value;
                projectYearPlanResponseDto.TotalTenProductionValue = yearInitProjectList.Sum(x => x.TenProductionValue).Value;
                projectYearPlanResponseDto.TotalElevenProductionValue = yearInitProjectList.Sum(x => x.ElevenProductionValue).Value;
                projectYearPlanResponseDto.TotalTwelveProductionValue = yearInitProjectList.Sum(x => x.TwelveProductionValue).Value;
                projectYearPlanResponseDto.TotalYearProductionValue = Math.Round((yearInitProjectList.Sum(x => x.YearTotalProductionValue).Value)/10000, 2);
                //var yearInitProjectList = await _dbContext.Queryable<YearInitProjectPlan>()
                //        .Where(x => x.IsDelete == 1 && allIds.Contains(x.ProjectId.Value))
                //        .ToListAsync();
                //projectYearPlanResponseDto.TotalOneProductionValue = yearInitProjectList.Sum(x => x.OneProductionValue.Value);
                //projectYearPlanResponseDto.TotalTwoProductionValue = yearInitProjectList.Sum(x => x.TwoProductionValue.Value);
                //projectYearPlanResponseDto.TotalThreeProductionValue = yearInitProjectList.Sum(x => x.ThreeProductionValue.Value);
                //projectYearPlanResponseDto.TotalFourProductionValue = yearInitProjectList.Sum(x => x.FourProductionValue.Value);
                //projectYearPlanResponseDto.TotalFiveProductionValue = yearInitProjectList.Sum(x => x.FiveProductionValue.Value);
                //projectYearPlanResponseDto.TotalSixProductionValue = yearInitProjectList.Sum(x => x.SixProductionValue.Value);
                //projectYearPlanResponseDto.TotalSevenProductionValue = yearInitProjectList.Sum(x => x.SevenProductionValue.Value);
                //projectYearPlanResponseDto.TotalEightProductionValue = yearInitProjectList.Sum(x => x.EightProductionValue.Value);
                //projectYearPlanResponseDto.TotalNineProductionValue = yearInitProjectList.Sum(x => x.NineProductionValue.Value);
                //projectYearPlanResponseDto.TotalTenProductionValue = yearInitProjectList.Sum(x => x.TenProductionValue.Value);
                //projectYearPlanResponseDto.TotalElevenProductionValue = yearInitProjectList.Sum(x => x.ElevenProductionValue.Value);
                //projectYearPlanResponseDto.TotalTwelveProductionValue = yearInitProjectList.Sum(x => x.TwelveProductionValue.Value);
                //projectYearPlanResponseDto.TotalYearProductionValue = Math.Round(yearInitProjectList.Sum(x => x.YearTotalProductionValue.Value), 2);
                //所有项目的合同产值-所有项目的开累产值
                //projectYearPlanResponseDto.TotalResidueProductionValue = Math.Round((allProject.Sum(x => x.ContractAmount*x.ExchangeRate) -
                //        (monthReportList.Sum(x => x.CompleteProductionAmount )
                //        + historyMonthReportList.Select(x => x.AccumulatedOutputValue ?? 0).Sum(x => x)))/10000, 2);
                var sum = 0;
                foreach (var item in allProject)
                {
                    var currentProducitonMonth = dayReportList.Where(x => x.ProjectId == item.ProjectId)
                         .Select(x => new ProjectDayReportResponseDto
                         {
                             ProjectId = x.ProjectId,
                             DateDay = int.Parse(x.DateDay.ToString().Substring(0, 6)),
                             DayAmount = x.DayActualProductionAmount
                         }).Sum(x => x.DayAmount);
                    var historyData = Sumxx(monthReportList, historyMonthReportList, item.ProjectId);
                    item.TotalProductionValue = Math.Round((historyData.Item1 + historyData.Item2 + currentProducitonMonth) / 10000, 2);
                }
                projectYearPlanResponseDto.TotalResidueProductionValue = Math.Round(
                    (allProject.Sum(x => x.ContractAmount * x.ExchangeRate)/10000 -
                        allProject.Sum(x => x.TotalProductionValue)
                        ),2);



     

               
            }
            #endregion


            responseAjaxResult.Count = total;
            responseAjaxResult.Data = projectYearPlanResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion

        }


        #endregion

        #region 新增编辑年初计划
        /// <summary>
        /// 新增编辑年初计划
        /// </summary>
        /// <param name="insertProjectYearPlanRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InsertPlanBuildProject(InsertProjectYearPlanRequestDto insertProjectYearPlanRequestDto)
        {
            ResponseAjaxResult<bool> response = new ResponseAjaxResult<bool>();
            var projectId = GuidUtil.Next();
            var str = new Random().Next(10000, 99999);
            var oids = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            var departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            //先查询一个项目
            var projectInfo = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1
            && x.StatusId == "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid()
            && x.CompanyId == _currentUser.CurrentLoginInstitutionId
            && departmentIds.Contains(x.ProjectDept.Value))
                .FirstAsync();
            if (projectInfo == null)
            {
                response.Fail("当前用户下无项目查询");
                response.Data = false;
                return response;
            }
            projectInfo.Id = projectId;
            projectInfo.StatusId = "0c686c96-889e-4c4d-b24d-fa2886d9dceb".ToGuid();
            projectInfo.PomId = projectInfo.PomId + str;
            projectInfo.Code = projectInfo.Code + str;
            projectInfo.CompanyId = _currentUser.CurrentLoginInstitutionId;
            projectInfo.Amount = insertProjectYearPlanRequestDto.ContractAmount;
            //projectInfo.ProjectDept = _currentUser.CurrentLoginDepartmentId;
            projectInfo.CreateTime = DateTime.Now;
            projectInfo.Name = insertProjectYearPlanRequestDto.ProjectName;
            projectInfo.ShortName = insertProjectYearPlanRequestDto.ProjectName;
            if (insertProjectYearPlanRequestDto.ProjectId == Guid.Empty)
            {
                //新增操作
                var flag = await _dbContext.Insertable<Project>(projectInfo).ExecuteCommandAsync();
            }
            else
            {
                //更新操作
                var project = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == insertProjectYearPlanRequestDto.ProjectId).FirstAsync();
                if (project != null)
                {
                    project.ShortName = insertProjectYearPlanRequestDto.ProjectName;
                    project.Name = insertProjectYearPlanRequestDto.ProjectName;
                    project.Amount = insertProjectYearPlanRequestDto.ContractAmount;
                    var flag = await _dbContext.Updateable<Project>(project).ExecuteCommandAsync();
                }

            }

            #region 时间判断
            //当前年份
            var year = 2024;
            var dataMonth = 0;
            var currentTimeInt = int.Parse(DateTime.Now.ToString("MMdd"));
            if (currentTimeInt > 1227)
            {
                year = DateTime.Now.AddYears(1).Year;
            }
            else
            {
                year = DateTime.Now.Year;
            }

            //当前月份
            //周期开始时间
            var startTime = string.Empty;
            if (DateTime.Now.Day >= 27)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            if (month.ToString().Length == 1)
            {
                dataMonth = int.Parse(year + "0" + month);
            }
            else
            {
                dataMonth = year + month;
            }
            #endregion

            var id = Guid.NewGuid();
            if (insertProjectYearPlanRequestDto.ProjectId != Guid.Empty)
            {
                var singleYearProject = await yearInitProjectPlanRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.ProjectId == insertProjectYearPlanRequestDto.ProjectId).FirstAsync();
                if (singleYearProject != null)
                    id = singleYearProject.Id;
            }

            var yearInitProjectPlan = _mapper.Map<InsertProjectYearPlanRequestDto, YearInitProjectPlan>(insertProjectYearPlanRequestDto);
            yearInitProjectPlan.DataMonth = dataMonth;
            yearInitProjectPlan.DataYear = year;
            if (insertProjectYearPlanRequestDto.ProjectId != Guid.Empty)
            {
                yearInitProjectPlan.Id = id;
                yearInitProjectPlan.ProjectId = insertProjectYearPlanRequestDto.ProjectId;
            }
            else
                yearInitProjectPlan.ProjectId = projectId;
            yearInitProjectPlan.YearTotalProductionValue = (
                insertProjectYearPlanRequestDto.OneProductionValue.Value +
                insertProjectYearPlanRequestDto.TwoProductionValue.Value
                + insertProjectYearPlanRequestDto.ThreeProductionValue.Value
                + insertProjectYearPlanRequestDto.FourProductionValue
                + insertProjectYearPlanRequestDto.FiveProductionValue +
                insertProjectYearPlanRequestDto.SixProductionValue +
                insertProjectYearPlanRequestDto.SevenProductionValue
                + insertProjectYearPlanRequestDto.EightProductionValue
                + insertProjectYearPlanRequestDto.NineProductionValue
                + insertProjectYearPlanRequestDto.TenProductionValue
                + insertProjectYearPlanRequestDto.ElevenProductionValue
                + insertProjectYearPlanRequestDto.TwelveProductionValue);
            //yearInitProjectPlan.YearTotalQuantity = Sumx(insertProjectYearPlanRequestDto).Item2;
            var isSuccess = await yearInitProjectPlanRepository.InsertOrUpdateAsync(yearInitProjectPlan);
            if (isSuccess)
            {
                response.Success("新增编辑成功");
                response.Data = true;
            }
            else
            {
                response.Fail("新增编辑失败");
                response.Data = false;
            }
            return response;
        }



        #endregion

        #region 获取每个项目的wbs
        /// <summary>
        /// 获取每个项目的wbs
        /// </summary>
        /// <param name="projectPlanWbsRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<ProjectPlanWbsResponseDto>> GetProjectPlanWbs(ProjectPlanWbsRequestDto projectPlanWbsRequestDto)
        {
            ResponseAjaxResult<ProjectPlanWbsResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectPlanWbsResponseDto>();
            List<ProjectYearPlanTree> projectYearPlanTrees = new List<ProjectYearPlanTree>();
            ProjectPlanWbsResponseDto projectPlanWbsResponseDto = new ProjectPlanWbsResponseDto();
            #region 时间判断
            //当前年份
            var year = 2024;
            var dataMonth = 0;
            var currentTimeInt = int.Parse(DateTime.Now.ToString("MMdd"));
            if (currentTimeInt > 1227)
            {
                year = DateTime.Now.AddYears(1).Year;
            }
            else
            {
                year = DateTime.Now.Year;
            }

            //当前月份
            //周期开始时间
            var startTime = string.Empty;
            if (DateTime.Now.Day >= 27)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            if (month.ToString().Length == 1)
            {
                dataMonth = int.Parse(year + "0" + month);
            }
            else
            {
                dataMonth = year + month;
            }
            #endregion

            #region 查询wbs相关

            //查询wbs相关
            var projectWbsList = await _dbContext.Queryable<HistoryProjectWbs>()
                .Where(x => x.IsDelete == 1 && x.ProjectId == projectPlanWbsRequestDto.ProjectId && x.DataMonth == dataMonth)
                .Select(x => new ProjectYearPlanTree
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId.Value,
                    ProjectWbsId = SqlFunc.ToGuid(x.Id),
                    Pid = x.Pid,
                    KeyId = x.KeyId,
                    _Quantity = x.EngQuantity.Value,
                    _UnitPrice = x.UnitPrice.Value,
                    Name = x.Name,

                }).ToListAsync();
            if (projectWbsList.Any())
            {
                var allIds = projectWbsList.Select(x => x.ProjectId).ToList();
                var allProjectYearInitList = await _dbContext.Queryable<YearInitProjectPlan>().Where(x => x.IsDelete == 1 && allIds.Contains(x.ProjectId.Value)).ToListAsync();
                foreach (var item in projectWbsList)
                {
                    var currentInfo = allProjectYearInitList.Where(x => x.ProjectId == item.ProjectId && x.ProjectWbsId == item.ProjectWbsId).FirstOrDefault();
                    if (currentInfo != null)
                    {
                        item.OneProductionValue = currentInfo.OneProductionValue.Value;
                        item.OneQuantity = currentInfo.OneQuantity.Value;
                        item.TwoProductionValue = currentInfo.TwoProductionValue.Value;
                        item.TwoQuantity = currentInfo.TwoQuantity.Value;
                        item.ThreeProductionValue = currentInfo.ThreeProductionValue.Value;
                        item.ThreeQuantity = currentInfo.ThreeQuantity.Value;
                        item.FourProductionValue = currentInfo.FourProductionValue.Value;
                        item.FourQuantity = currentInfo.FourQuantity.Value;
                        item.FiveProductionValue = currentInfo.FiveProductionValue.Value;
                        item.FiveQuantity = currentInfo.FiveQuantity.Value;
                        item.SixProductionValue = currentInfo.SixProductionValue.Value;
                        item.SixQuantity = currentInfo.SixQuantity.Value;
                        item.SevenProductionValue = currentInfo.SevenProductionValue.Value;
                        item.SevenQuantity = currentInfo.SevenQuantity.Value;
                        item.EightProductionValue = currentInfo.EightProductionValue.Value;
                        item.EightQuantity = currentInfo.EightQuantity.Value;
                        item.NineProductionValue = currentInfo.NineProductionValue.Value;
                        item.NineQuantity = currentInfo.NineQuantity.Value;
                        item.TenProductionValue = currentInfo.TenProductionValue.Value;
                        item.TenQuantity = currentInfo.TenQuantity.Value;
                        item.ElevenProductionValue = currentInfo.ElevenProductionValue.Value;
                        item.ElevenQuantity = currentInfo.ElevenQuantity.Value;
                        item.TwelveProductionValue = currentInfo.TwelveProductionValue.Value;
                        item.TwelveQuantity = currentInfo.TwelveQuantity.Value;
                    }

                }
            }

            //查询项目月报明细表 把月报明细添加到wbs相应节点中 如果没有找到对应的wbs则添加根节点
            var wbsRepostDetailList = await _dbContext.Queryable<YearProjectPlanDetail>().Where(x => x.IsDelete == 1 && projectPlanWbsRequestDto.ProjectId == x.ProjectId
             && dataMonth == x.DateMonth).ToListAsync();
            #endregion

            #region 查询最后节点的子项
            if (projectWbsList.Any())
            {
                var projectWbsListCopy = projectWbsList.DeepCopy();

                #region 查询最后节点的子项
                foreach (var project in projectWbsListCopy)
                {
                    var keyId = projectWbsList.Where(x => x.Id == project.Id).Select(x => x.KeyId).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(keyId))
                    {
                        var yearPlanTreesDetail = wbsRepostDetailList.Where(x => x.ProjectId == project.ProjectId && x.ProjectWBSId == project.Id)
                            .Select(x => new ProjectYearPlanTree
                            {
                                Id = x.Id,
                                ProjectWbsId = x.ProjectWBSId,
                                ProjectId = x.ProjectId,
                                Pid = keyId,
                                KeyId = GuidUtil.Next().ToString(),
                                _TotalQuantity = Math.Round(x.CompletedQuantity != 0 ? x.CompletedQuantity : 0, 2),
                                _TotalProductuinValue = Math.Round(x.CompleteProductionAmount != 0 ? x.CompleteProductionAmount : 0, 2),
                                _UnitPrice = Math.Round(x.UnitPrice != 0 ? x.UnitPrice : 0, 2),
                                _Quantity = 0,
                                IsDelete = true
                            }).ToList();
                        if (yearPlanTreesDetail.Any())
                        {
                            projectWbsList.AddRange(yearPlanTreesDetail);
                        }
                    }
                    else
                    {

                        var yearPlanTreesDetail = wbsRepostDetailList.Where(x => x.ProjectId == project.ProjectId && x.ProjectWBSId == project.Id)
                           .Select(x => new ProjectYearPlanTree
                           {
                               Id = x.Id,
                               ProjectId = x.ProjectId,
                               Pid = "0",
                               ProjectWbsId = x.ProjectWBSId,
                               KeyId = GuidUtil.Next().ToString(),
                               _TotalQuantity = Math.Round(x.CompletedQuantity != 0 ? x.CompletedQuantity : 0, 2),
                               _TotalProductuinValue = Math.Round(x.CompleteProductionAmount != 0 ? x.CompleteProductionAmount : 0, 2),
                               _UnitPrice = Math.Round(x.UnitPrice != 0 ? x.UnitPrice : 0, 2),
                               _Quantity = 0,
                               IsDelete = true
                           }).ToList();
                        if (yearPlanTreesDetail.Any())
                        {
                            projectWbsList.AddRange(yearPlanTreesDetail);
                        }
                    }

                }

                if (projectWbsList.Any())
                {
                    var copyData = projectWbsList.Where(x => x.IsDelete == true).ToList();
                    var allIds = projectWbsList.Select(x => x.ProjectId).ToList();
                    var allProjectYearInitList = await _dbContext.Queryable<YearInitProjectPlan>().Where(x => x.IsDelete == 1 && allIds.Contains(x.ProjectId.Value)).ToListAsync();
                    foreach (var item in copyData)
                    {
                        var currentInfo = allProjectYearInitList.Where(x => x.ProjectId == item.ProjectId && x.ProjectWbsId == item.ProjectWbsId).FirstOrDefault();
                        if (currentInfo != null)
                        {
                            item.OneProductionValue = currentInfo.OneProductionValue.Value;
                            item.OneQuantity = currentInfo.OneQuantity.Value;
                            item.TwoProductionValue = currentInfo.TwoProductionValue.Value;
                            item.TwoQuantity = currentInfo.TwoQuantity.Value;
                            item.ThreeProductionValue = currentInfo.ThreeProductionValue.Value;
                            item.ThreeQuantity = currentInfo.ThreeQuantity.Value;
                            item.FourProductionValue = currentInfo.FourProductionValue.Value;
                            item.FourQuantity = currentInfo.FourQuantity.Value;
                            item.FiveProductionValue = currentInfo.FiveProductionValue.Value;
                            item.FiveQuantity = currentInfo.FiveQuantity.Value;
                            item.SixProductionValue = currentInfo.SixProductionValue.Value;
                            item.SixQuantity = currentInfo.SixQuantity.Value;
                            item.SevenProductionValue = currentInfo.SevenProductionValue.Value;
                            item.SevenQuantity = currentInfo.SevenQuantity.Value;
                            item.EightProductionValue = currentInfo.EightProductionValue.Value;
                            item.EightQuantity = currentInfo.EightQuantity.Value;
                            item.NineProductionValue = currentInfo.NineProductionValue.Value;
                            item.NineQuantity = currentInfo.NineQuantity.Value;
                            item.TenProductionValue = currentInfo.TenProductionValue.Value;
                            item.TenQuantity = currentInfo.TenQuantity.Value;
                            item.ElevenProductionValue = currentInfo.ElevenProductionValue.Value;
                            item.ElevenQuantity = currentInfo.ElevenQuantity.Value;
                            item.TwelveProductionValue = currentInfo.TwelveProductionValue.Value;
                            item.TwelveQuantity = currentInfo.TwelveQuantity.Value;
                        }

                    }
                }
                if (projectWbsList.Any())
                {
                    projectYearPlanTrees = ListToTreeUtil.GetProjectPlanTree<ProjectYearPlanTree>("0", projectWbsList);

                }
                #endregion
            }
            #endregion

            #region 统计相关
            projectPlanWbsResponseDto.Statistics = new Statistics() ;
            var yearInitProjectList = await _dbContext.Queryable<YearInitProjectPlan>().Where(x => x.IsDelete == 1 && x.ProjectId == projectPlanWbsRequestDto.ProjectId).ToListAsync();
            if (yearInitProjectList.Any())
            {

                projectPlanWbsResponseDto.Statistics = new Statistics()
                {
                    TotalOneProductionValue = yearInitProjectList.Sum(x => x.OneProductionValue ?? 0),
                    TotalTwoProductionValue = yearInitProjectList.Sum(x => x.TwoProductionValue ?? 0),
                    TotalThreeProductionValue = yearInitProjectList.Sum(x => x.ThreeProductionValue ?? 0),
                    TotalFourProductionValue = yearInitProjectList.Sum(x => x.FourProductionValue ?? 0),
                    TotalFiveProductionValue = yearInitProjectList.Sum(x => x.FiveProductionValue ?? 0),
                    TotalSixProductionValue = yearInitProjectList.Sum(x => x.SixProductionValue ?? 0),
                    TotalSevenProductionValue = yearInitProjectList.Sum(x => x.SevenProductionValue ?? 0),
                    TotalEightProductionValue = yearInitProjectList.Sum(x => x.EightProductionValue ?? 0),
                    TotalNineProductionValue = yearInitProjectList.Sum(x => x.NineProductionValue ?? 0),
                    TotalTenProductionValue = yearInitProjectList.Sum(x => x.TenProductionValue ?? 0),
                    TotalElevenProductionValue = yearInitProjectList.Sum(x => x.ElevenProductionValue ?? 0),
                    TotalTwelveProductionValue = yearInitProjectList.Sum(x => x.TwelveProductionValue ?? 0),
                     

                };
                projectPlanWbsResponseDto.Statistics.TotalPlanProductionValue = Math.Round((projectPlanWbsResponseDto.Statistics.TotalOneProductionValue +
                    projectPlanWbsResponseDto.Statistics.TotalTwoProductionValue + projectPlanWbsResponseDto.Statistics.TotalThreeProductionValue +
                    projectPlanWbsResponseDto.Statistics.TotalFourProductionValue + projectPlanWbsResponseDto.Statistics.TotalFiveProductionValue +
                    projectPlanWbsResponseDto.Statistics.TotalSixProductionValue + projectPlanWbsResponseDto.Statistics.TotalSevenProductionValue +
                    projectPlanWbsResponseDto.Statistics.TotalEightProductionValue + projectPlanWbsResponseDto.Statistics.TotalNineProductionValue +
                    projectPlanWbsResponseDto.Statistics.TotalTenProductionValue + projectPlanWbsResponseDto.Statistics.TotalElevenProductionValue +
                    projectPlanWbsResponseDto.Statistics.TotalTwelveProductionValue) / 10000, 2) ;


                projectPlanWbsResponseDto.Statistics.TotalContractAmount = projectWbsList.Sum(x => x.ContractAmount);
                projectPlanWbsResponseDto.Statistics.TotalYearInitContractAmount = projectWbsList.Sum(x => x.TotalProductuinValue);
                projectPlanWbsResponseDto.Statistics.TotalResidueContractAmount = (projectPlanWbsResponseDto.Statistics.TotalContractAmount
                    - projectPlanWbsResponseDto.Statistics.TotalYearInitContractAmount);
            }

            #endregion

            #region 删除isdelete=false的  方便前端处理
            //删除isdelete=false的  方便前端处理
            var searializeJson = JsonConvert.SerializeObject(projectYearPlanTrees, new JsonSerializerSettings()
            {
                //首字母小写问题
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }).TrimAll().Replace("\"isDelete\":false,", " ").TrimAll();
            #endregion

            projectPlanWbsResponseDto.Json = searializeJson;
            responseAjaxResult.Data = projectPlanWbsResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 两个辅助方法
        public static Tuple<decimal, decimal> Sumx(ProjectYearPlanTreeRequestDto projectYearPlanTreeRequestDto)
        {

            var yearTotalProductionValue =
               projectYearPlanTreeRequestDto.OneProductionValue.Value +
               projectYearPlanTreeRequestDto.TwoProductionValue.Value +
               projectYearPlanTreeRequestDto.ThreeProductionValue.Value +
               projectYearPlanTreeRequestDto.FourProductionValue.Value +
               projectYearPlanTreeRequestDto.FiveProductionValue.Value +
               projectYearPlanTreeRequestDto.SixProductionValue.Value +
               projectYearPlanTreeRequestDto.SevenProductionValue.Value +
               projectYearPlanTreeRequestDto.EightProductionValue.Value +
               projectYearPlanTreeRequestDto.NineProductionValue.Value +
               projectYearPlanTreeRequestDto.TenProductionValue.Value +
               projectYearPlanTreeRequestDto.ElevenProductionValue.Value +
               projectYearPlanTreeRequestDto.TwelveProductionValue.Value;

            var yearTotalQuantity =
               projectYearPlanTreeRequestDto.OneQuantity.Value +
               projectYearPlanTreeRequestDto.TwoQuantity.Value +
               projectYearPlanTreeRequestDto.ThreeQuantity.Value +
               projectYearPlanTreeRequestDto.FourQuantity.Value +
               projectYearPlanTreeRequestDto.FiveQuantity.Value +
               projectYearPlanTreeRequestDto.SixQuantity.Value +
               projectYearPlanTreeRequestDto.SevenQuantity.Value +
               projectYearPlanTreeRequestDto.EightQuantity.Value +
               projectYearPlanTreeRequestDto.NineQuantity.Value +
               projectYearPlanTreeRequestDto.TenQuantity.Value +
               projectYearPlanTreeRequestDto.ElevenQuantity.Value +
               projectYearPlanTreeRequestDto.TwelveQuantity.Value;
            return Tuple.Create(yearTotalProductionValue, yearTotalQuantity);
        }

        /// <summary>
        /// 计算项目的开累产值
        /// </summary>
        /// <param name="monthReports"></param>
        /// <returns></returns>
        public static Tuple<decimal, decimal> Sumxx(List<MonthReport> monthReports, List<ProjectHistoryData> historyMonthReportList, Guid projectId)
        {
            var item1 = monthReports.Where(x => x.ProjectId == projectId).Sum(x => x.CompleteProductionAmount);
            var item2 = historyMonthReportList.Where(x => x.ProjectId == projectId).Select(x => x.AccumulatedOutputValue ?? 0)
                 .Sum(x => x);
            return Tuple.Create(item1, item2);
        }


        #endregion

        #region 保存项目年初计划wbs

        /// <summary>
        /// 保存项目年初计划wbs
        /// </summary>
        /// <param name="projectYearPlanTreeRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveProjectPlanWbsAsync(ProjectYearPlanTreeRequestDto projectYearPlanTreeRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            #region 时间判断
            //当前年份
            var year = 2024;
            var dataMonth = 0;
            var currentTimeInt = int.Parse(DateTime.Now.ToString("MMdd"));
            if (currentTimeInt > 1227)
            {
                year = DateTime.Now.AddYears(1).Year;
            }
            else
            {
                year = DateTime.Now.Year;
            }

            //当前月份
            //周期开始时间
            var startTime = string.Empty;
            if (DateTime.Now.Day >= 27)
            {
                startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            var month = Convert.ToDateTime(startTime).AddMonths(1).Month;
            if (month.ToString().Length == 1)
            {
                dataMonth = int.Parse(year + "0" + month);
            }
            else
            {
                dataMonth = year + month;
            }
            #endregion
            if (projectYearPlanTreeRequestDto.RequestType == false && projectYearPlanTreeRequestDto.IsDelete == true)
            {
                var projectWbsInfo = await _dbContext.Queryable<YearProjectPlanDetail>().FirstAsync(x => x.IsDelete == 1 && x.Id == projectYearPlanTreeRequestDto.Id);
                if (projectWbsInfo != null)
                {
                    projectWbsInfo.UnitPrice = projectYearPlanTreeRequestDto.Unitprice;
                    projectWbsInfo.CompletedQuantity = projectYearPlanTreeRequestDto.YearInitQuantity;
                    projectWbsInfo.CompleteProductionAmount = projectYearPlanTreeRequestDto.YearInitProductionValue;

                    var isExist = await yearInitProjectPlanRepository.AsQueryable().Where(x => x.IsDelete == 1
                     && x.DataMonth == dataMonth && x.ProjectId == projectYearPlanTreeRequestDto.ProjectId
                     && x.ProjectWbsId == projectYearPlanTreeRequestDto.ProjectWbsId).FirstAsync();
                    YearInitProjectPlan yearInitProjectPlan = null;
                    if (isExist == null)
                    {

                        yearInitProjectPlan = new YearInitProjectPlan()
                        {
                            YearTotalProductionValue = Sumx(projectYearPlanTreeRequestDto).Item1,
                            ProjectWbsId = projectYearPlanTreeRequestDto.ProjectWbsId,
                            ProjectId = projectYearPlanTreeRequestDto.ProjectId,
                            DataMonth = dataMonth,
                            DataYear = DateTime.Now.Year,
                            OneQuantity = projectYearPlanTreeRequestDto.OneQuantity,
                            OneProductionValue = projectYearPlanTreeRequestDto.OneProductionValue,
                            TwoQuantity = projectYearPlanTreeRequestDto.TwoQuantity,
                            TwoProductionValue = projectYearPlanTreeRequestDto.TwoProductionValue,
                            ThreeQuantity = projectYearPlanTreeRequestDto.ThreeQuantity,
                            ThreeProductionValue = projectYearPlanTreeRequestDto.ThreeProductionValue,
                            FourQuantity = projectYearPlanTreeRequestDto.FourQuantity,
                            FourProductionValue = projectYearPlanTreeRequestDto.FourProductionValue,
                            FiveQuantity = projectYearPlanTreeRequestDto.FiveQuantity,
                            FiveProductionValue = projectYearPlanTreeRequestDto.FiveProductionValue,
                            SixQuantity = projectYearPlanTreeRequestDto.SixQuantity,
                            SixProductionValue = projectYearPlanTreeRequestDto.SixProductionValue,
                            SevenQuantity = projectYearPlanTreeRequestDto.SevenQuantity,
                            SevenProductionValue = projectYearPlanTreeRequestDto.SevenProductionValue,
                            EightQuantity = projectYearPlanTreeRequestDto.EightQuantity,
                            EightProductionValue = projectYearPlanTreeRequestDto.EightProductionValue,
                            NineQuantity = projectYearPlanTreeRequestDto.NineQuantity,
                            NineProductionValue = projectYearPlanTreeRequestDto.NineProductionValue,
                            TenQuantity = projectYearPlanTreeRequestDto.TenQuantity,
                            TenProductionValue = projectYearPlanTreeRequestDto.TenProductionValue,
                            ElevenQuantity = projectYearPlanTreeRequestDto.ElevenQuantity,
                            ElevenProductionValue = projectYearPlanTreeRequestDto.ElevenProductionValue,
                            TwelveQuantity = projectYearPlanTreeRequestDto.TwelveQuantity,
                            TwelveProductionValue = projectYearPlanTreeRequestDto.TwelveProductionValue
                        };
                    }
                    else
                    {
                        yearInitProjectPlan = new YearInitProjectPlan()
                        {
                            YearTotalProductionValue = Sumx(projectYearPlanTreeRequestDto).Item1,
                            Id = isExist.Id,
                            ProjectWbsId = projectYearPlanTreeRequestDto.ProjectWbsId,
                            ProjectId = projectYearPlanTreeRequestDto.ProjectId,
                            DataMonth = dataMonth,
                            DataYear = DateTime.Now.Year,
                            OneQuantity = projectYearPlanTreeRequestDto.OneQuantity,
                            OneProductionValue = projectYearPlanTreeRequestDto.OneProductionValue,
                            TwoQuantity = projectYearPlanTreeRequestDto.TwoQuantity,
                            TwoProductionValue = projectYearPlanTreeRequestDto.TwoProductionValue,
                            ThreeQuantity = projectYearPlanTreeRequestDto.ThreeQuantity,
                            ThreeProductionValue = projectYearPlanTreeRequestDto.ThreeProductionValue,
                            FourQuantity = projectYearPlanTreeRequestDto.FourQuantity,
                            FourProductionValue = projectYearPlanTreeRequestDto.FourProductionValue,
                            FiveQuantity = projectYearPlanTreeRequestDto.FiveQuantity,
                            FiveProductionValue = projectYearPlanTreeRequestDto.FiveProductionValue,
                            SixQuantity = projectYearPlanTreeRequestDto.SixQuantity,
                            SixProductionValue = projectYearPlanTreeRequestDto.SixProductionValue,
                            SevenQuantity = projectYearPlanTreeRequestDto.SevenQuantity,
                            SevenProductionValue = projectYearPlanTreeRequestDto.SevenProductionValue,
                            EightQuantity = projectYearPlanTreeRequestDto.EightQuantity,
                            EightProductionValue = projectYearPlanTreeRequestDto.EightProductionValue,
                            NineQuantity = projectYearPlanTreeRequestDto.NineQuantity,
                            NineProductionValue = projectYearPlanTreeRequestDto.NineProductionValue,
                            TenQuantity = projectYearPlanTreeRequestDto.TenQuantity,
                            TenProductionValue = projectYearPlanTreeRequestDto.TenProductionValue,
                            ElevenQuantity = projectYearPlanTreeRequestDto.ElevenQuantity,
                            ElevenProductionValue = projectYearPlanTreeRequestDto.ElevenProductionValue,
                            TwelveQuantity = projectYearPlanTreeRequestDto.TwelveQuantity,
                            TwelveProductionValue = projectYearPlanTreeRequestDto.TwelveProductionValue
                        };
                    }
                    var flag = await _dbContext.Updateable<YearProjectPlanDetail>(projectWbsInfo).ExecuteCommandAsync();
                    var isSuccess = await yearInitProjectPlanRepository.InsertOrUpdateAsync(yearInitProjectPlan);
                    var deleteObj=await yearInitProjectPlanRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.ProjectId == projectYearPlanTreeRequestDto.ProjectId
                    && dataMonth == dataMonth && x.ProjectWbsId == null).FirstAsync();
                    if (deleteObj != null)
                    {
                        isSuccess = await yearInitProjectPlanRepository.DeleteAsync(deleteObj);
                    }
                    if (isSuccess)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                    }
                    else
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, Domain.Shared.Enums.HttpStatusCode.UpdateFail);
                    }

                }

            }
            else if (projectYearPlanTreeRequestDto.RequestType == false && projectYearPlanTreeRequestDto.IsDelete == false)
            {
                var projectWbsInfo = await _dbContext.Queryable<HistoryProjectWbs>().FirstAsync(x => x.IsDelete == 1 && x.Id == projectYearPlanTreeRequestDto.Id);
                if (projectWbsInfo != null)
                {
                    projectWbsInfo.EngQuantity = projectYearPlanTreeRequestDto.EightQuantity;
                    projectWbsInfo.ContractAmount = projectYearPlanTreeRequestDto.ContractAmount;
                    projectWbsInfo.UnitPrice = projectYearPlanTreeRequestDto.Unitprice;
                    var isExist = await yearInitProjectPlanRepository.AsQueryable().Where(x => x.IsDelete == 1
                     && x.DataMonth == dataMonth && x.ProjectId == projectYearPlanTreeRequestDto.ProjectId
                     && x.ProjectWbsId == projectYearPlanTreeRequestDto.ProjectWbsId).FirstAsync();
                    YearInitProjectPlan yearInitProjectPlan = null;
                    if (isExist == null)
                    {

                        yearInitProjectPlan = new YearInitProjectPlan()
                        {
                            YearTotalProductionValue = Sumx(projectYearPlanTreeRequestDto).Item1,
                            ProjectWbsId = projectYearPlanTreeRequestDto.ProjectWbsId,
                            ProjectId = projectYearPlanTreeRequestDto.ProjectId,
                            DataMonth = dataMonth,
                            DataYear = DateTime.Now.Year,
                            OneQuantity = projectYearPlanTreeRequestDto.OneQuantity,
                            OneProductionValue = projectYearPlanTreeRequestDto.OneProductionValue,
                            TwoQuantity = projectYearPlanTreeRequestDto.TwoQuantity,
                            TwoProductionValue = projectYearPlanTreeRequestDto.TwoProductionValue,
                            ThreeQuantity = projectYearPlanTreeRequestDto.ThreeQuantity,
                            ThreeProductionValue = projectYearPlanTreeRequestDto.ThreeProductionValue,
                            FourQuantity = projectYearPlanTreeRequestDto.FourQuantity,
                            FourProductionValue = projectYearPlanTreeRequestDto.FourProductionValue,
                            FiveQuantity = projectYearPlanTreeRequestDto.FiveQuantity,
                            FiveProductionValue = projectYearPlanTreeRequestDto.FiveProductionValue,
                            SixQuantity = projectYearPlanTreeRequestDto.SixQuantity,
                            SixProductionValue = projectYearPlanTreeRequestDto.SixProductionValue,
                            SevenQuantity = projectYearPlanTreeRequestDto.SevenQuantity,
                            SevenProductionValue = projectYearPlanTreeRequestDto.SevenProductionValue,
                            EightQuantity = projectYearPlanTreeRequestDto.EightQuantity,
                            EightProductionValue = projectYearPlanTreeRequestDto.EightProductionValue,
                            NineQuantity = projectYearPlanTreeRequestDto.NineQuantity,
                            NineProductionValue = projectYearPlanTreeRequestDto.NineProductionValue,
                            TenQuantity = projectYearPlanTreeRequestDto.TenQuantity,
                            TenProductionValue = projectYearPlanTreeRequestDto.TenProductionValue,
                            ElevenQuantity = projectYearPlanTreeRequestDto.ElevenQuantity,
                            ElevenProductionValue = projectYearPlanTreeRequestDto.ElevenProductionValue,
                            TwelveQuantity = projectYearPlanTreeRequestDto.TwelveQuantity,
                            TwelveProductionValue = projectYearPlanTreeRequestDto.TwelveProductionValue
                        };
                    }
                    else
                    {
                        yearInitProjectPlan = new YearInitProjectPlan()
                        {
                            YearTotalProductionValue = Sumx(projectYearPlanTreeRequestDto).Item1,
                            Id = isExist.Id,
                            ProjectWbsId = projectYearPlanTreeRequestDto.ProjectWbsId,
                            ProjectId = projectYearPlanTreeRequestDto.ProjectId,
                            DataMonth = dataMonth,
                            DataYear = DateTime.Now.Year,
                            OneQuantity = projectYearPlanTreeRequestDto.OneQuantity,
                            OneProductionValue = projectYearPlanTreeRequestDto.OneProductionValue,
                            TwoQuantity = projectYearPlanTreeRequestDto.TwoQuantity,
                            TwoProductionValue = projectYearPlanTreeRequestDto.TwoProductionValue,
                            ThreeQuantity = projectYearPlanTreeRequestDto.ThreeQuantity,
                            ThreeProductionValue = projectYearPlanTreeRequestDto.ThreeProductionValue,
                            FourQuantity = projectYearPlanTreeRequestDto.FourQuantity,
                            FourProductionValue = projectYearPlanTreeRequestDto.FourProductionValue,
                            FiveQuantity = projectYearPlanTreeRequestDto.FiveQuantity,
                            FiveProductionValue = projectYearPlanTreeRequestDto.FiveProductionValue,
                            SixQuantity = projectYearPlanTreeRequestDto.SixQuantity,
                            SixProductionValue = projectYearPlanTreeRequestDto.SixProductionValue,
                            SevenQuantity = projectYearPlanTreeRequestDto.SevenQuantity,
                            SevenProductionValue = projectYearPlanTreeRequestDto.SevenProductionValue,
                            EightQuantity = projectYearPlanTreeRequestDto.EightQuantity,
                            EightProductionValue = projectYearPlanTreeRequestDto.EightProductionValue,
                            NineQuantity = projectYearPlanTreeRequestDto.NineQuantity,
                            NineProductionValue = projectYearPlanTreeRequestDto.NineProductionValue,
                            TenQuantity = projectYearPlanTreeRequestDto.TenQuantity,
                            TenProductionValue = projectYearPlanTreeRequestDto.TenProductionValue,
                            ElevenQuantity = projectYearPlanTreeRequestDto.ElevenQuantity,
                            ElevenProductionValue = projectYearPlanTreeRequestDto.ElevenProductionValue,
                            TwelveQuantity = projectYearPlanTreeRequestDto.TwelveQuantity,
                            TwelveProductionValue = projectYearPlanTreeRequestDto.TwelveProductionValue
                        };
                    }
                    var flag = await _dbContext.Updateable<HistoryProjectWbs>(projectWbsInfo).ExecuteCommandAsync();
                    var isSuccess = await yearInitProjectPlanRepository.InsertOrUpdateAsync(yearInitProjectPlan);
                    var deleteObj =await yearInitProjectPlanRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.ProjectId == projectYearPlanTreeRequestDto.ProjectId
                    && dataMonth == dataMonth && x.ProjectWbsId == null).FirstAsync();
                    if (deleteObj != null)
                    {
                        isSuccess = await yearInitProjectPlanRepository.DeleteAsync(deleteObj);
                    }
                    if (isSuccess)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                    }
                    else
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, Domain.Shared.Enums.HttpStatusCode.UpdateFail);
                    }

                }
            }
            return responseAjaxResult;
        }
        #endregion
    }
}