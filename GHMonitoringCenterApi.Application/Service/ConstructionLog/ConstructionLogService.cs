using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Service.ConstructionLog
{
    /// <summary>
    /// 施工日志实现层
    /// </summary>
    public class ConstructionLogService : IConstructionLogService
    {

        #region 依赖注入
        public IBaseService baseService { get; set; }
        public IBaseRepository<Project> baseProjectRepository { get; set; }
        public IBaseRepository<DayReport> baseDayReportRepository { get; set; }
        public IProjectReportService projectReportService1 { get; set; }
        public ISqlSugarClient dbContent { get; set; }
        private readonly IMapper _mapper;

        public ConstructionLogService(IBaseService baseService, IBaseRepository<Project> baseProjectRepository, IBaseRepository<DayReport> baseDayReportRepository, IProjectReportService projectReportService1, ISqlSugarClient dbContent, IMapper mapper)
        {
            this.baseService = baseService;
            this.baseProjectRepository = baseProjectRepository;
            this.baseDayReportRepository = baseDayReportRepository;
            this.projectReportService1 = projectReportService1;
            this.dbContent = dbContent;
            _mapper = mapper;
        }


        #endregion
        /// <summary>
        /// 获取施工日志列表
        /// </summary>
        /// <param name="constructionLogRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ConstructionLogResponseDto>>> SearchConstructionLogAsync(ConstructionLogRequestDto constructionLogRequestDto, string oid)
        {
           
            ResponseAjaxResult<List<ConstructionLogResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ConstructionLogResponseDto>>();
            List<ConstructionLogResponseDto> constructionLog = new List<ConstructionLogResponseDto>();
            var currentInstitutionList = await dbContent.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
            RefAsync<int> total = 0;
            //var a = currentInstitutionList.Where(x => x.Oid == oid).Select(x => x.Orule).Single();
            if (currentInstitutionList.SingleOrDefault(x => x.Oid == oid && (x.Oid == "101162350" || x.Oid == "101162351")) == null)
            {
                var currentAffCompanyInfo = await baseService.GetCurrentInstitutionParent(oid);
                constructionLog = await dbContent.Queryable<Project>()
                .InnerJoin<DayReport>((x, y) => x.Id == y.ProjectId && y.ProcessStatus == DayReportProcessStatus.Submited)
                .InnerJoin<ProjectStatus>((x, y, z) => x.StatusId == z.StatusId)
            .Where((x,y) => y.IsDelete == 1)
            .WhereIF(currentAffCompanyInfo != null, x => x.ProjectDept == currentAffCompanyInfo.DPomId)
            .WhereIF(!string.IsNullOrWhiteSpace(constructionLogRequestDto.ProjectName), x => x.Name.Contains(constructionLogRequestDto.ProjectName))
            .WhereIF(constructionLogRequestDto.CompanyId.HasValue, x => x.CompanyId == constructionLogRequestDto.CompanyId)
            .WhereIF(constructionLogRequestDto.Time.HasValue, (x, y) => y.DateDay == constructionLogRequestDto.Time.Value.ToDateDay())
            .WhereIF(constructionLogRequestDto.ProjectDepartmentId.HasValue, x => x.ProjectDept == constructionLogRequestDto.ProjectDepartmentId)
            .WhereIF(constructionLogRequestDto.StatusId != null && constructionLogRequestDto.StatusId.Any(), (x, y, z) => constructionLogRequestDto.StatusId.Contains(x.StatusId.ToString()))
            .OrderByDescending((x, y, z)=> y.DateDay)
            .Select((x, y, z) => new ConstructionLogResponseDto
            {
                Id = y.Id,
                ProjectId = x.Id,
                ProjectName = x.Name,
                CompanyId = x.CompanyId,
                Status = z.StatusId,
                StatusName = z.Name,
                DateDay = y.DateDay
            }).ToPageListAsync(constructionLogRequestDto.PageIndex, constructionLogRequestDto.PageSize, total);
            }
            else
            {
                constructionLog = await dbContent.Queryable<Project>()
                  .InnerJoin<DayReport>((x, y) => x.Id == y.ProjectId )
                  .InnerJoin<ProjectStatus>((x, y, z) => x.StatusId == z.StatusId)
                  .Where((x, y, z) => y.IsDelete == 1 && y.ProcessStatus == DayReportProcessStatus.Submited)
                  .WhereIF(!string.IsNullOrWhiteSpace(constructionLogRequestDto.ProjectName), (x, y, z) => x.Name.Contains(constructionLogRequestDto.ProjectName))
                  .WhereIF(constructionLogRequestDto.CompanyId.HasValue, (x, y, z) => x.CompanyId == constructionLogRequestDto.CompanyId)
                  .WhereIF(constructionLogRequestDto.Time.HasValue, (x, y, z) => y.DateDay == constructionLogRequestDto.Time.Value.ToDateDay())
                  .WhereIF(constructionLogRequestDto.ProjectDepartmentId.HasValue, (x, y, z) => x.ProjectDept == constructionLogRequestDto.ProjectDepartmentId)
                  .WhereIF(constructionLogRequestDto.StatusId != null && constructionLogRequestDto.StatusId.Any(), (x, y, z) => constructionLogRequestDto.StatusId.Contains(x.StatusId.ToString()))
                  .OrderByDescending((x, y, z) => y.DateDay)
                                .Select((x, y, z) => new ConstructionLogResponseDto
                                {
                                    Id = y.Id,
                                    ProjectId = x.Id,
                                    ProjectName = x.Name,
                                    CompanyId = x.CompanyId,
                                    Status = z.StatusId,
                                    StatusName = z.Name,
                                    DateDay = y.DateDay
                                })
          .ToPageListAsync(constructionLogRequestDto.PageIndex, constructionLogRequestDto.PageSize, total);
            }
            var company = await dbContent.Queryable<Institution>().ToListAsync();
            if (constructionLog.Any())
            {
                foreach (var item in constructionLog)
                {
                    if (!string.IsNullOrWhiteSpace(item.DateDay.ToString()))
                    {
                        ConvertHelper.TryConvertDateTimeFromDateDay(item.DateDay.Value, out DateTime dayTimes);
                        item.SubmitTime = dayTimes;
                    }
                    item.CompanyName = company.Where(x => x.PomId == item.CompanyId).Select(x => x.Name).FirstOrDefault();
                    
                }
            }
            responseAjaxResult.Data = constructionLog;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;

        }
        /// <summary>
        /// 获取已填报日志日期
        /// </summary>
        /// <returns></returns>                                  
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SearchCompletedConstructionLogRequestDto>>> SearchCompletedConstructionLogAsync(DateTime? dateTime, Guid projectid)
        {

            ResponseAjaxResult<List<SearchCompletedConstructionLogRequestDto>> responseAjaxResult = new ResponseAjaxResult<List<SearchCompletedConstructionLogRequestDto>>();
            var times = await dbContent.Queryable<DayReport>()
               .Where(x => x.IsDelete == 1 && x.DateDay >= dateTime.Value.ToDateDay() && x.DateDay <= dateTime.Value.ToDateDay() + 30 && x.ProjectId == projectid&&x.ProcessStatus == DayReportProcessStatus.Submited)
                .Select(x => x.DateDay).ToListAsync();
            List<SearchCompletedConstructionLogRequestDto> constructionLog = new List<SearchCompletedConstructionLogRequestDto>();
            DateTime dateTimes;
            foreach (var item in times)
            {
                if(ConvertHelper.TryConvertDateTimeFromDateDay(item, out dateTimes))
                {
                    SearchCompletedConstructionLogRequestDto searchCompletedConstructionLogRequestDto = new SearchCompletedConstructionLogRequestDto()
                    {
                        Time = dateTimes.ToShortDateString(),
                        DateDayTime = item
                    };
                    constructionLog.Add(searchCompletedConstructionLogRequestDto);
                }
            };
               
            responseAjaxResult.Data = constructionLog;
            responseAjaxResult.Count = constructionLog.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取施工日志详情
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SearchConstructionLoDetailsgResponseDto>> SearchConstructionLogDetailsgAsync(Guid? Id, int? dateTime)
        {
            ResponseAjaxResult<SearchConstructionLoDetailsgResponseDto> responseAjaxResult = new ResponseAjaxResult<SearchConstructionLoDetailsgResponseDto>();
            if (dateTime == null)
            {
                dateTime = DateTime.Now.ToDateDay()-1;
            }
            var constructionLogs = await dbContent.Queryable<DayReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .LeftJoin<DictionaryTable>((x, y, s) => s.TypeNo == 4 && s.Type == x.Weather)
                .LeftJoin<Domain.Models.User>((x, y, s, z) => z.Id == x.CreateId)
                .Where(x => x.IsDelete == 1 && x.ProjectId == Id&& x.ProcessStatus == DayReportProcessStatus.Submited)
                .Select((x, y, s, z) => new SearchConstructionLoDetailsgResponseDto
                {
                    Id = x.Id,
                    Name = y.Name,
                    ProjectId = x.ProjectId,
                    ConstructionRemarks = x.ConstructionRemarks,
                    Weather = s.Name,
                    RecorderName = z.Name,
                    Management = x.SiteManagementPersonNum,
                    ConstructionPersonnel = x.SiteConstructionPersonNum,
                    ConstructionEquipment = x.ConstructionDevice,
                    OtherRecords = x.OtherRecord,
                    ConstructionDeviceNum = x.ConstructionDeviceNum,
                    HazardousConstructionNum = x.HazardousConstructionNum,
                    TeamLeader = x.TeamLeader,
                    LandWorkplace = x.LandWorkplace,
                    ShiftLeader = x.ShiftLeader,
                    ShiftLeaderPhone = x.ShiftLeaderPhone,
                    FewLandWorkplace= x.FewLandWorkplace,
                    SiteShipNum= x.SiteShipNum,
                    OnShipPersonNum= x.OnShipPersonNum,
                    HazardousConstructionDescription= x.HazardousConstructionDescription,
                    IsHoliday= x.IsHoliday,
                    FillingDateTime = x.DateDay
                })
               .ToListAsync();
            var  constructionLog = constructionLogs.Where(x => x.FillingDateTime == dateTime).FirstOrDefault();
            //var dayReportIds = await dbContent.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.CreateTime.ToDateDay() == dateTime.Value.ToDateDay() && x.ProjectId == Id).Select(x => x.Id).SingleAsync();
            if (constructionLog == null || string.IsNullOrWhiteSpace(constructionLog.ProjectId.ToString()) || constructionLog.ProjectId == Guid.Empty)
            {
                var dayReportConstructions = await dbContent.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.ProjectId == Id&& x.ProcessStatus == DayReportProcessStatus.Submited).OrderByDescending(x => x.CreateTime).ToListAsync();

                if (!dayReportConstructions.Any())
                {
                    responseAjaxResult.Fail(ResponseMessage.DATA_NOTEXIST_BUILDDAYREPORT, Domain.Shared.Enums.HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                return await SearchConstructionLogDetailsgAsync(dayReportConstructions[0].ProjectId, dayReportConstructions[0].DateDay);
            }

            ConvertHelper.TryConvertDateTimeFromDateDay(dateTime.Value, out DateTime dayTimes);
            constructionLog.DateTime = dayTimes;
            constructionLog.DayReportConstruction = new List<DayReportConstructions>();
            var dayReportConstruction = await dbContent.Queryable<DayReportConstruction>().Where(x => x.ProjectId == constructionLog.ProjectId && x.DateDay == dateTime && x.IsDelete == 1).ToListAsync();
            var projectWBS = await dbContent.Queryable<ProjectWBS>().Where(x => x.IsDelete == 1 && x.ProjectId == constructionLog.ProjectId.ToString()).ToListAsync();
            foreach (var item in dayReportConstruction)
            {
                var projectwbsList = projectWBS.FirstOrDefault(x => x.Id == item.ProjectWBSId);
                DayReportConstructions dayReportConstructions = new DayReportConstructions();
                dayReportConstructions.ActualDailyProduction = item.ActualDailyProduction;
                dayReportConstructions.ActualDailyProductionAmount = item.ActualDailyProductionAmount;
                if (projectwbsList != null)
                {
                    dayReportConstructions.ConstructionContent = GetProjectWBSLevelName(projectWBS, projectwbsList.Name, projectwbsList.Pid);
                    dayReportConstructions.ConstructionRecord = GetProjectWBSLevel1Name(projectWBS, projectwbsList);
                }


                constructionLog.DayReportConstruction.Add(dayReportConstructions);
            }
            responseAjaxResult.Count = 1;
            responseAjaxResult.Data = constructionLog;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 递归获取施工分类层级名称
        /// </summary>
        /// <returns></returns>
        private string? GetProjectWBSLevelName(List<ProjectWBS> projectWBSList, string? levelName, string? pid)
        {
            if (string.IsNullOrWhiteSpace(pid) || pid.TrimAll() == "0")
            {
                return levelName;
            }
            var partentProjectWBS = projectWBSList.FirstOrDefault(t => t.KeyId == pid);
            if (partentProjectWBS == null || partentProjectWBS.KeyId == partentProjectWBS.Pid)
            {
                return levelName;
            }
            return GetProjectWBSLevelName(projectWBSList, partentProjectWBS.Name + "/" + levelName, partentProjectWBS.Pid);
        }
        /// <summary>
        /// 递归获取施工分类一级层级名称
        /// </summary>
        /// <returns></returns>
        private string? GetProjectWBSLevel1Name(List<ProjectWBS> projectWBSList, ProjectWBS projectWBS)
        {
            if (string.IsNullOrWhiteSpace(projectWBS.Pid) || projectWBS.Pid.TrimAll() == "0")
            {
                return projectWBS.Name;
            }
            var partentProjectWBS = projectWBSList.FirstOrDefault(t => t.KeyId == projectWBS.Pid);
            if (partentProjectWBS == null)
            {
                return projectWBS.Name;
            }
            if (partentProjectWBS.KeyId == partentProjectWBS.Pid)
            {
                return partentProjectWBS.Name;
            }
            return GetProjectWBSLevel1Name(projectWBSList, partentProjectWBS);
        }
    }
}
