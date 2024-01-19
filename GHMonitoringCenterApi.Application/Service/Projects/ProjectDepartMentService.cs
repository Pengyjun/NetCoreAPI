using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using UtilsSharp;
using System.Globalization;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System.Reflection;
using System.Drawing;
using Spire.Pdf.Exporting.XPS.Schema;
using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Service.Projects
{
    public class ProjectDepartMentService : IProjectDepartMentService
    {
        #region 依赖注入
        public ISqlSugarClient dbContext { get; set; }
        public IMapper mapper { get; set; }
        public IBaseRepository<ProjectPlanProduction> baseAnnualPlanRepository { get; set; }
        public IBaseRepository<Project> baseProjectRepository { get; set; }
        public IBaseRepository<DayReportConstruction> baseDayReportRepository { get; set; }
        public IBaseRepository<DayReport> dayReportRepository { get; set; }
        public IBaseRepository<ProjectOutPutValue> baseProjectOutPutValueRepository { get; set; }
        public IBaseService baseService { get; set; }
        public ILogService logService { get; set; }

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 当前用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        public ProjectDepartMentService(ISqlSugarClient dbContext, IMapper mapper, IBaseRepository<ProjectPlanProduction> baseAnnualPlanRepository, IBaseRepository<Project> baseProjectRepository, IBaseRepository<DayReport> dayReportRepository, IBaseRepository<DayReportConstruction> baseDayReportRepository, IBaseRepository<ProjectOutPutValue> baseProjectOutPutValueRepository, IBaseService baseService, ILogService logService, GlobalObject globalObject)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.baseAnnualPlanRepository = baseAnnualPlanRepository;
            this.baseProjectRepository = baseProjectRepository;
            this.dayReportRepository = dayReportRepository;
            this.baseDayReportRepository = baseDayReportRepository;
            this.baseProjectOutPutValueRepository = baseProjectOutPutValueRepository;
            this.baseService = baseService;
            this.logService = logService;
            this._globalObject = globalObject;
        }
        #endregion


        /// <summary>
        /// 获取项目部的相关产值
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<OutputValueResponseDto>> GetOutputValueV2Async(OutputValueRequestDto model)
        {
            var responseAjaxResult = new ResponseAjaxResult<OutputValueResponseDto>();
            //应前端要求没有项目Id传空对象 保证页面数据正常展示
            if (model.ProjectId == null)
            {
                return responseAjaxResult.SuccessResult(new OutputValueResponseDto() { });
            }
            //获取项目
            var project = await baseProjectRepository.GetFirstAsync(x => x.IsDelete == 1 && x.Id == model.ProjectId);
            if (project == null)
            {
                return responseAjaxResult.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var now = DateTime.Now;
            // 计划月份时间节点，上月26-本月25为本月时间范围
            var planMonthTime = now.Day > 25 ? now.AddMonths(1) : now;
            // 计划月份
            var planMonth = planMonthTime.Month;
            // 计划年份
            var planYear = planMonthTime.Year;
            //年度计划
           var  planProject = (await GetProjectAnnualPlansAsync((Guid)model.ProjectId, new int[] { planYear })).OrderByDescending(t=>t.CreateTime).FirstOrDefault();
            // 今天
            var todayDay = now.ToDateDay();
            // 昨天
            var yesterDay = now.AddDays(-1).ToDateDay();
            // 7天时间范围
            var sevenStartDay = now.AddDays(-6).ToDateDay();
            var sevenEndDay = todayDay;
            //7天的内的日报产值集合 todo 理论拆分查询统计产值更具有可读性，目前先采用查出来再统计对应的时间节点产值
            var sevenDayReports = await dayReportRepository.AsQueryable().Where(t => t.IsDelete == 1 && t.ProcessStatus== DayReportProcessStatus.Submited&& t.ProjectId == project.Id && t.DateDay >= sevenStartDay && t.DateDay <= sevenEndDay).Select(t => new { t.DateDay, t.DayActualProductionAmount }).ToListAsync();
            // 今天产值(元)
            var todayAmount = sevenDayReports.Where(t => t.DateDay == todayDay).Sum(t => t.DayActualProductionAmount);
            // 昨天产值（元）
            var yesterdayAmount = sevenDayReports.Where(t => t.DateDay == yesterDay).Sum(t => t.DayActualProductionAmount);
            // 7天产值（元）
            var sevenDayAmount = sevenDayReports.Sum(t => t.DayActualProductionAmount);
            //今天日计划产值（元）
            decimal todayDayPlanAmount = 0;
            if (planProject != null)
            {
                var monthOutputValue = GetMonthPlannedOutputValue(planMonth, planProject);
                //今日计划产值（逻辑：月份天数的平均值）
                todayDayPlanAmount =monthOutputValue / 30.5m;
            }
            var result = new OutputValueResponseDto()
            {
                TodayActualOutput = Math.Round(todayAmount / 10000, 2),
                yesterdayOutputTotal = Math.Round(yesterdayAmount / 10000, 2),
                SevenDayOutputTotal = Math.Round(sevenDayAmount / 10000, 2),
                TodayPlanOutput = Math.Round(todayDayPlanAmount/10000,2)
            };
            return responseAjaxResult.SuccessResult(result);
        }

        /// <summary>
        /// 获取年度计划
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="years">年份集合</param>
        /// <returns></returns>
        private async Task<List<ProjectPlanProduction>> GetProjectAnnualPlansAsync(Guid  projectId, int[] years)
        {
            return await baseAnnualPlanRepository.GetListAsync(t => t.IsDelete == 1 && t.ProjectId == projectId && years.Contains( t.Year));
        }

        /// <summary>
        /// 获取月份计划产值(元)
        /// </summary>
        /// <param name="month">月份</param>
        /// <param name="projectAnnualPlan">计划产值</param>
        /// <returns></returns>
        private decimal GetMonthPlannedOutputValue(int month, ProjectPlanProduction projectAnnualPlan)
        {
            decimal? planOutValue=null ;
            switch (month)
            {
                case 1: planOutValue = projectAnnualPlan.OnePlanProductionValue; break;
                case 2: planOutValue = projectAnnualPlan.TwoPlanProductionValue; break;
                case 3: planOutValue = projectAnnualPlan.ThreePlanProductionValue; break;
                case 4: planOutValue = projectAnnualPlan.FourPlanProductionValue; break;
                case 5: planOutValue = projectAnnualPlan.FivePlanProductionValue; break;
                case 6: planOutValue = projectAnnualPlan.SixPlanProductionValue; break;
                case 7: planOutValue = projectAnnualPlan.SevenPlanProductionValue; break;
                case 8: planOutValue = projectAnnualPlan.EightPlanProductionValue; break;
                case 9: planOutValue = projectAnnualPlan.NinePlanProductionValue; break;
                case 10: planOutValue = projectAnnualPlan.TenPlanProductionValue; break;
                case 11: planOutValue = projectAnnualPlan.ElevenPlanProductionValue; break;
                case 12: planOutValue = projectAnnualPlan.TwelvePlanProductionValue; break;
            }
            return planOutValue??0;
        }

        /// <summary>
        /// 项目部产值chart图
        /// </summary>
        /// <param name="outputChartRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<OutputValueChartResponseDto>>> GetOutputValueChartAsync(OutputChartRequestDto outputChartRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<OutputValueChartResponseDto>>();
            //应前端要求没有项目Id传空对象 保证页面数据正常展示
            if (outputChartRequestDto.ProjectId == null)
            {
                return responseAjaxResult.SuccessResult(new List<OutputValueChartResponseDto>() { });
            }
            //获取项目
            var project = await baseProjectRepository.GetFirstAsync(x => x.IsDelete == 1 && x.Id == outputChartRequestDto.ProjectId);
            if (project == null)
            {
                return responseAjaxResult.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var startTime = outputChartRequestDto.StartTime.Date;
            var endTime = outputChartRequestDto.EndTime.Date;
            var planYears = GetPlanYears(startTime, endTime);
            //年度产值计划
            var planProjects = await GetProjectAnnualPlansAsync((Guid)outputChartRequestDto.ProjectId, planYears);
            var spanDays = (endTime - startTime).Days + 1;
            var startDay = startTime.ToDateDay();
            var endDay = endTime.ToDateDay();
            var dayReports = await dayReportRepository.AsQueryable().Where(t => t.IsDelete == 1 && t.ProcessStatus == DayReportProcessStatus.Submited&&t.ProjectId == project.Id && t.DateDay >= startDay && t.DateDay <= endDay).Select(t => new { t.DateDay, t.DayActualProductionAmount }).ToListAsync();
            var resOutputValues = new List<OutputValueChartResponseDto>();
            for (var i = 0; i < spanDays; i++)
            {
                var thisTime = startTime.AddDays(i);
                // 当前时间点的对应的日期产值金额（元）
                var thisTimeDayAmount = dayReports.Where(t => t.DateDay == thisTime.ToDateDay()).Sum(t=>t.DayActualProductionAmount);
                var outputChart = new OutputValueChartResponseDto()
                {
                    ActualValue = Math.Round( thisTimeDayAmount/10000,2),
                    DateValue = string.Format("{0}-{1}", thisTime.Month, thisTime.Day)
                };
                // 上月26-本月25为本月时间范围
                var thisMonthTime = thisTime.Day > 25 ? thisTime.AddMonths(1) : thisTime;
                var planProject = planProjects.Where(t => t.Year >= thisMonthTime.Year).FirstOrDefault();
                if (planProject!=null)
                {
                    var thisTimePlannedMonthOutputValue = GetMonthPlannedOutputValue(thisMonthTime.Month, planProject);
                    //当前时间计划产值 （逻辑：月份天数的平均值）
                    outputChart.PlanValue = Math.Round(thisTimePlannedMonthOutputValue/10000/ 30.5m, 2);
                }
                resOutputValues.Add(outputChart);
            }
            return responseAjaxResult.SuccessResult(resOutputValues, resOutputValues.Count);
        }

        /// <summary>
        /// 根据时间范围获取计划年份集合
        /// </summary>
        /// <returns></returns>
        private int[] GetPlanYears(DateTime startTime,DateTime endTime)
        {
            var years = new List<int>();
            for(var i= startTime.Year; i<= endTime.Year; i++)
            {
                years.Add(i);
            }
            //判断结束时间是否跨年
            if(endTime.Month == 12&&endTime.Day>25 )
            {
                years.Add(endTime.AddMonths(1).Year);
            }
            return years.ToArray();
        }

        /// <summary>
        /// 获取用户的项目Id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<GetUserProjectIdResponseDto>> GetUserProjectIdAsync()
        {
            ResponseAjaxResult<GetUserProjectIdResponseDto> responseAjaxResult = new ResponseAjaxResult<GetUserProjectIdResponseDto>();
            GetUserProjectIdResponseDto responseDto = new GetUserProjectIdResponseDto();
            //获取该用户最新的一条项目信息
            var projectId = await baseProjectRepository.AsQueryable().OrderByDescending(x => x.CreateTime).FirstAsync(x => x.ProjectDept.Equals(_currentUser.CurrentLoginDepartmentId));
            if (projectId == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, Domain.Shared.Enums.HttpStatusCode.DataNotEXIST);
                return responseAjaxResult;
            }
            responseDto.ProjectId = projectId.Id;
            responseAjaxResult.Data = responseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}
