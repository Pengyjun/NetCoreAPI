using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Linq.Expressions;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.ProjectProductionReport
{
    /// <summary>
    /// 项目生产报表
    /// </summary>
    public class ProjectProductionReportService : IProjectProductionReportService
    {
        #region 依赖注入
        public ISqlSugarClient dbContext { get; set; }
        public IMapper mapper { get; set; }
        public IBaseService baseService { get; set; }
        /// <summary>
        /// 机构
        /// </summary>
        private readonly IBaseRepository<Institution> _dbInstitution;
        public ILogService logService { get; set; }
        /// <summary>
        /// 产值日报
        /// </summary>
        public IBaseRepository<DayReport> _dbDayReport;
        /// <summary>
        /// 船舶日报
        /// </summary>
        public IBaseRepository<ShipDayReport> _dbShipDayReport;
        /// <summary>
        /// 安监日报
        /// </summary>
        public IBaseRepository<SafeSupervisionDayReport> _dbSafeDayReport;
        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 当前用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        public ProjectProductionReportService(ISqlSugarClient dbContext, IMapper mapper, IBaseService baseService, ILogService logService, IBaseRepository<DayReport> _dbDayReport, IBaseRepository<ShipDayReport> _dbShipDayReport, IBaseRepository<Institution> _dbInstitution, IBaseRepository<SafeSupervisionDayReport> _dbSafeDayReport, GlobalObject globalObject)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.baseService = baseService;
            this.logService = logService;
            this._dbDayReport = _dbDayReport;
            this._dbShipDayReport = _dbShipDayReport;
            this._dbSafeDayReport = _dbSafeDayReport;
            this._globalObject = globalObject;
            this._dbInstitution = _dbInstitution;
        }
        #endregion

        /// <summary>
        /// 获取产值日报列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<DayReportResponseDto>> SearchDayReportAsync(ProductionSafetyRequestDto searchRequestDto)
        {
            ResponseAjaxResult<DayReportResponseDto> responseAjaxResult = new ResponseAjaxResult<DayReportResponseDto>();
            DayReportResponseDto responseDtos = new DayReportResponseDto();
            //获取所有机构
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            //获取产值日报的项目基础数据
            var list = await BaseSearchDatReportListAsync(searchRequestDto, institution);

            if (list.Data.dayReportInfos == null)
            {
                responseAjaxResult.Data = responseDtos;
                responseAjaxResult.Count = list.Count;
                responseAjaxResult.Success();
                return responseAjaxResult;
            }
            var result = await GetDayResponseDtosAsync(list.Data.dayReportInfos, institution);
            // result = result.OrderByDescending(x => x.DateDay).ThenByDescending(x=>x.ProSumActualDailyProductionAmount).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();

            responseDtos.dayReportInfos = result;
            responseDtos.TimeValue = list.Data.TimeValue;
            responseDtos.sumDayValue = list.Data.sumDayValue;

            responseAjaxResult.Data = responseDtos;
            responseAjaxResult.Count = list.Count;
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
        /// 获取产值日报需要计算的合计值
        /// </summary>
        /// <param name="dayReports"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SumDayValue> GetSumDayValueAsync(List<DayReportInfo> dayReports)
        {
            SumDayValue sumInfo = new SumDayValue();
            var dayIds = dayReports.Select(x => x.DayId).ToList();
            //查询项目日报下的施工记录集合
            var dayConstructionList = await dbContext.Queryable<DayReportConstruction>().Where(x => x.IsDelete == 1 && dayIds.Contains(x.DayReportId)).ToListAsync();
            #region 处理分包产值问题
            if (_currentUser.CurrentLoginIsAdmin || _currentUser.CurrentLoginInstitutionOid == "101162350")
            {
                foreach (var dayCons in dayConstructionList)
                {
                    var isExist = dayReports.Where(x => x.DayId == dayCons.DayReportId && x.IsSubContractProject == 1).FirstOrDefault();
                    if (isExist != null)
                    {
                        dayCons.ActualDailyProduction = 0m; dayCons.ActualDailyProductionAmount = 0M;
                    }
                }
            }
          
            #endregion
            sumInfo.SumOutsourcingExpensesAmount = Math.Round(dayReports.Sum(x => x.ProSumOutsourcingExpensesAmount.Value), 2);
            sumInfo.SumActualDailyProduction = Math.Round(dayConstructionList.Sum(x => x.ActualDailyProduction), 3);
            sumInfo.SumActualDailyProductionAmount = Math.Round(dayConstructionList.Sum(x => x.ActualDailyProductionAmount), 2);
            return sumInfo;
        }

        /// <summary>
        /// 产值日报列表excel导出用
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<DayReportExcelResponseDto>> SearchDayReportExcelAsync(ProductionSafetyRequestDto searchRequestDto)
        {
            ResponseAjaxResult<DayReportExcelResponseDto> responseAjaxResult = new ResponseAjaxResult<DayReportExcelResponseDto>();
            DayReportExcelResponseDto responseDtos = new DayReportExcelResponseDto();
            //获取所有机构
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            //获取产值日报的项目基础数据
            var list = await BaseSearchDatReportListAsync(searchRequestDto, institution);
            var result = await GetDayExcelResponseDtosAsync(list.Data.dayReportInfos, institution);
            responseDtos.dayReportExcels = result;
            responseDtos.TimeValue = list.Data.TimeValue;

            responseAjaxResult.Data = responseDtos;
            responseAjaxResult.Count = list.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 产值日报基础查询
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<DayReportResponseDto>> BaseSearchDatReportListAsync(ProductionSafetyRequestDto searchRequestDto, List<Institution> institution)
        {

            ResponseAjaxResult<DayReportResponseDto> responseAjaxResult = new ResponseAjaxResult<DayReportResponseDto>();
            DayReportResponseDto responseDtos = new DayReportResponseDto();
            RefAsync<int> total = 0;
            //属性标签
            var categoryList = new List<int>();
            var tagList = new List<int>();
            var tag2List = new List<int>();
            if (searchRequestDto.TagName != null && searchRequestDto.TagName.Any())
            {
                foreach (var item in searchRequestDto.TagName)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            List<Guid> departmentIds = new List<Guid>();
            //获取所有机构
            //var institution = await dbContext.Queryable<Institution>().ToListAsync();
            if (searchRequestDto.IsDuiWai)
            {
                _currentUser.CurrentLoginIsAdmin = true; _currentUser.CurrentLoginInstitutionOid = "101162350";
                _currentUser.CurrentLoginInstitutionGrule = "-104396-104400-101114066-101114070-101162350-";
            }
            if (_currentUser.CurrentLoginIsAdmin)
            {
                departmentIds = institution.Select(x => x.PomId.Value).ToList();
                departmentIds.Add(_currentUser.CurrentLoginInstitutionId);
            }
            else
            {
                //获取当前登陆人是否属于公司登陆系统 
                var curLogin = institution.Where(x => x.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
                if (curLogin != null)
                {
                    if (curLogin.Ocode.Contains("0000A") || curLogin.Ocode.Contains("0000B") || curLogin.Ocode.Contains("0000C") || curLogin.Ocode.Contains("0000E"))
                    {
                        var reverse = curLogin.Grule.Split('-').Reverse().ToArray()[1];
                        departmentIds = institution.Where(x => x.Grule.Contains(reverse)).Select(x => x.PomId.Value).ToList();
                    }
                    else
                    {
                        departmentIds.Add(_currentUser.CurrentLoginDepartmentId.Value);
                    }
                    departmentIds.Add(curLogin.PomId.Value);
                }
            }
            //获取临时补录项目部id
            var depIds = await dbContext.Queryable<ProjectDepartment>().Where(x => x.Name == "临时补录项目部" && departmentIds.Contains(x.CompanyId.Value)).Select(x => x.PomId.Value).ToListAsync();
            departmentIds.AddRange(depIds);
            //过滤特殊字符
            searchRequestDto.ProjectName = Utils.ReplaceSQLChar(searchRequestDto.ProjectName);

            var time = 0;
            if (searchRequestDto.StartTime != null && searchRequestDto.EndTime != null)
            {
                if (searchRequestDto.StartTime == searchRequestDto.EndTime)
                {
                    responseDtos.TimeValue = searchRequestDto.StartTime.Value.Year + "年" + searchRequestDto.StartTime.Value.Month + "月" + searchRequestDto.StartTime.Value.Day + "日";
                }
                else
                {
                    responseDtos.TimeValue = searchRequestDto.StartTime.Value.Year + "年" + searchRequestDto.StartTime.Value.Month + "月" + searchRequestDto.StartTime.Value.Day + "日--" + searchRequestDto.EndTime.Value.Year + "年" + searchRequestDto.EndTime.Value.Month + "月" + searchRequestDto.EndTime.Value.Day + "日";
                }
            }
            else
            {
                time = DateTime.Now.Date.AddDays(-1).ToDateDay();
                responseDtos.TimeValue = DateTime.Now.Date.AddDays(-1).Year + "年" + DateTime.Now.Date.AddDays(-1).Month + "月" + DateTime.Now.Date.AddDays(-1).Day + "日";

            }
            departmentIds.Clear();
            var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            //分页查询
            var dayReport = await dbContext.Queryable<DayReportConstruction>().Where(x => x.IsDelete == 1)
                .WhereIF(time != 0, x => x.DateDay == time)
                .Select(x => new { x.DayReportId, x.ActualDailyProductionAmount, x.OutsourcingExpensesAmount, x.ActualDailyProduction, x.UnitPrice })
                .ToListAsync();
            var a = departmentIds.Where(x => x == "63115206-38d9-4e3a-8c5a-39abf7e5ea29".ToGuid()).ToList();
            var list = new List<DayReportInfo>();
            if (!searchRequestDto.IsDuiWai)//非对外查询接口  监控中心自己用
            {
               

                list = await dbContext.Queryable<Project, DayReport>(
                   (p, d) =>
                   new JoinQueryInfos(
                       JoinType.Inner, p.Id == d.ProjectId && d.ProcessStatus == DayReportProcessStatus.Submited
                      
                   ))
                   .WhereIF(true, (p, d) => p.IsDelete == 1)
                   .WhereIF(true, p => departmentIds.Contains(p.ProjectDept.Value))
                   .WhereIF(searchRequestDto.StartTime == null && searchRequestDto.EndTime == null, (p, d) => d.DateDay == time)
                   .WhereIF(searchRequestDto.StartTime != null && searchRequestDto.EndTime != null, (p, d) => d.DateDay >= searchRequestDto.StartTime.Value.ToDateDay() && d.DateDay <= searchRequestDto.EndTime.Value.ToDateDay())
                   .WhereIF(searchRequestDto.CompanyId != null, (p, d) => p.CompanyId == searchRequestDto.CompanyId)
                   .WhereIF(searchRequestDto.ProjectDept != null, (p, d) => p.ProjectDept == searchRequestDto.ProjectDept)
                   .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), (p, d) => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                   .WhereIF(searchRequestDto.ProjectTypeId != null, (p, d) => p.TypeId == searchRequestDto.ProjectTypeId)
                    .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), (p, d) => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                   .WhereIF(categoryList != null && categoryList.Any(), (p, d) => categoryList.Contains(p.Category))
                   .WhereIF(tagList != null && tagList.Any(), (p, d) => tagList.Contains(p.Tag))
                   .WhereIF(tag2List != null && tag2List.Any(), (p, d) => tag2List.Contains(p.Tag2))
                   //.WhereIF(_currentUser.CurrentLoginIsAdmin||_currentUser.CurrentLoginInstitutionOid== "101162350")
                   .Select((p, d) => new DayReportInfo
                   {
                       
                       Id = d.Id,
                       ProjectId = p.Id,
                       DayId = d.Id,
                       LandWorkplace = d.LandWorkplace,
                       ShiftLeader = d.ShiftLeader,
                       ShiftLeaderPhone = d.ShiftLeaderPhone,
                       FewLandWorkplace = d.FewLandWorkplace,
                       SiteShipNum = d.SiteShipNum,
                       OnShipPersonNum = d.OnShipPersonNum,
                       HazardousConstructionDescription = d.HazardousConstructionDescription,
                       ProjectName = p.Name,
                       IsHoliday = d.IsHoliday,
                       CompanyName = p.CompanyId.ToString(),
                       ProjectCategory = SqlFunc.IIF(p.Category == 0, "境内", "境外"),
                       CurrencyExchangeRate = d.CurrencyExchangeRate,
                       ProjectType = p.TypeId.ToString(),
                       ProjectStatus = p.StatusId.ToString(),
                       Amount = p.Amount,
                       CreateUser = d.CreateId.ToString(),
                       DayActualProductionAmount = d.DayActualProductionAmount,
                       DayActualPlanAmount = d.MonthPlannedProductionAmount / 30.5M,
                       DayActualProductionQuantity = d.DayActualProduction,
                       DateDay = d.DateDay,
                       UpdateTime = d.UpdateTime,
                       CreateTime = d.CreateTime,
                       Sitemanagementpersonnum = d.SiteManagementPersonNum,
                       Siteconstructionpersonnum = d.SiteConstructionPersonNum,
                       Hazardousconstructionnum = d.HazardousConstructionNum,
                       HazardousConstructionDetails = d.HazardousConstructionDetails,
                       IsSubContractProject = p.IsSubContractProject
                   }).ToListAsync();

               

            }
            else
            {
                list = await dbContext.Queryable<Project, DayReport>(
                   (p, d) =>
                   new JoinQueryInfos(
                       JoinType.Inner, p.Id == d.ProjectId && d.ProcessStatus == DayReportProcessStatus.Submited
                   ))
                   .WhereIF(true, (p, d) => p.IsDelete == 1)
                   .WhereIF(true, p => departmentIds.Contains(p.ProjectDept.Value))
                   .WhereIF(searchRequestDto.CompanyId != null, (p, d) => p.CompanyId == searchRequestDto.CompanyId)
                   .WhereIF(searchRequestDto.ProjectDept != null, (p, d) => p.ProjectDept == searchRequestDto.ProjectDept)
                   .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), (p, d) => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                   .WhereIF(searchRequestDto.ProjectTypeId != null, (p, d) => p.TypeId == searchRequestDto.ProjectTypeId)
                   .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), (p, d) => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                   .WhereIF(categoryList != null && categoryList.Any(), (p, d) => categoryList.Contains(p.Category))
                   .WhereIF(tagList != null && tagList.Any(), (p, d) => tagList.Contains(p.Tag))
                   .WhereIF(tag2List != null && tag2List.Any(), (p, d) => tag2List.Contains(p.Tag2))
                   .Select((p, d) => new DayReportInfo
                   {
                       Id = d.Id,
                       ProjectId = p.Id,
                       DayId = d.Id,
                       LandWorkplace = d.LandWorkplace,
                       ShiftLeader = d.ShiftLeader,
                       ShiftLeaderPhone = d.ShiftLeaderPhone,
                       FewLandWorkplace = d.FewLandWorkplace,
                       SiteShipNum = d.SiteShipNum,
                       OnShipPersonNum = d.OnShipPersonNum,
                       HazardousConstructionDescription = d.HazardousConstructionDescription,
                       ProjectName = p.Name,
                       IsHoliday = d.IsHoliday,
                       CompanyName = p.CompanyId.ToString(),
                       ProjectCategory = SqlFunc.IIF(p.Category == 0, "境内", "境外"),
                       CurrencyExchangeRate = d.CurrencyExchangeRate,
                       ProjectType = p.TypeId.ToString(),
                       ProjectStatus = p.StatusId.ToString(),
                       Amount = p.Amount,
                       CreateUser = d.CreateId.ToString(),
                       DayActualProductionAmount = d.DayActualProductionAmount,
                       DayActualPlanAmount = d.MonthPlannedProductionAmount / 30.5M,
                       DayActualProductionQuantity = d.DayActualProduction,
                       DateDay = d.DateDay,
                       UpdateTime = d.UpdateTime,
                       CreateTime = d.CreateTime,
                       Sitemanagementpersonnum = d.SiteManagementPersonNum,
                       Siteconstructionpersonnum = d.SiteConstructionPersonNum,
                       Hazardousconstructionnum = d.HazardousConstructionNum,
                       HazardousConstructionDetails = d.HazardousConstructionDetails,
                   }).ToListAsync();
                //对外接口日期筛选
                list = list
                    .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                     x.CreateTime >= searchRequestDto.StartTime && x.CreateTime <= searchRequestDto.EndTime
                    : x.UpdateTime >= searchRequestDto.StartTime && x.UpdateTime <= searchRequestDto.EndTime)
                    .ToList();
            }
            if (!list.Any())
            {
                responseAjaxResult.Data = responseDtos;
                responseAjaxResult.Count = total;
                return responseAjaxResult;
            }
         
            var dayIds = list.Select(x => x.DayId).ToList();
            dayReport = dayReport.Where(x => dayIds.Contains(x.DayReportId)).ToList();
            for (int i = 0; i < list.Count; i++)
            {
               
                for (int j = 0; j < searchRequestDto.Sort.Length; j++)
                {
                    list[i].Sort[j] = searchRequestDto.Sort[j];
                }
                #region 处理分包项目  不计算产值 工程量  外包支出
                //list[i].IsSubContractProject == 1
                #endregion
                if ((_currentUser.CurrentLoginIsAdmin|| _currentUser.CurrentLoginInstitutionOid== "101162350")&& list[i].IsSubContractProject == 1)
                {
                    //list[i].ProSumActualDailyProductionAmount = 0M;
                    list[i].ProSumOutsourcingExpensesAmount = 0M;
                    list[i].DayActualProductionAmount = 0M;
                    list[i].DayActualProductionQuantity = 0M;
                    //list[i].ProSumActualDailyProduction = 0M;

                }
                else {
                    list[i].ProSumActualDailyProductionAmount = Math.Round(dayReport.Where(x => x.DayReportId == list[i].DayId).Sum(x => x.ActualDailyProductionAmount), 2);
                    list[i].ProSumOutsourcingExpensesAmount = Math.Round(dayReport.Where(x => x.DayReportId == list[i].DayId).Sum(x => x.OutsourcingExpensesAmount) * list[i].CurrencyExchangeRate, 2);
                    list[i].ProSumActualDailyProduction = Math.Round(dayReport.Where(x => x.DayReportId == list[i].DayId).Sum(x => x.ActualDailyProduction), 2);
                    list[i].ProSumUnitPrice = Math.Round(dayReport.Where(x => x.DayReportId == list[i].DayId).Sum(x => x.UnitPrice) * list[i].CurrencyExchangeRate, 2);
                }
               
            }

            #region 获取所有数据  合计值
            responseDtos.sumDayValue = await GetSumDayValueAsync(list);
            #endregion
            #region 是否全量导出
            //是否全量导出
            var projectList = new List<DayReportInfo>();
            if (!searchRequestDto.IsFullExport)
            {
                total = list.Count;
                if (searchRequestDto.Sort.Any())
                {
                    if (searchRequestDto.Sort[1] == "descending")
                    {
                        if (searchRequestDto.Sort[0] == "actualDailyProductionAmount")
                        {
                            projectList = list.OrderByDescending(x => x.ProSumActualDailyProductionAmount).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "actualDailyProduction")
                        {
                            projectList = list.OrderByDescending(x => x.ProSumActualDailyProduction).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();

                        }
                        else if (searchRequestDto.Sort[0] == "outsourcingExpensesAmount")
                        {
                            projectList = list.OrderByDescending(x => x.ProSumOutsourcingExpensesAmount).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "unitPrice")
                        {
                            projectList = list.OrderByDescending(x => x.ProSumUnitPrice).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                        }
                    }
                    else if (searchRequestDto.Sort[1] == "ascending")
                    {
                        if (searchRequestDto.Sort[0] == "actualDailyProductionAmount")
                        {
                            projectList = list.OrderBy(x => x.ProSumActualDailyProductionAmount).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "actualDailyProduction")
                        {
                            projectList = list.OrderBy(x => x.ProSumActualDailyProduction).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();

                        }
                        else if (searchRequestDto.Sort[0] == "outsourcingExpensesAmount")
                        {
                            projectList = list.OrderBy(x => x.ProSumOutsourcingExpensesAmount).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "unitPrice")
                        {
                            projectList = list.OrderBy(x => x.ProSumUnitPrice).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                        }
                    }
                }
                else
                {
                    projectList = list.OrderByDescending(x => x.DateDay).Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
                }
            }
            else
            {
                if (searchRequestDto.Sort.Any())
                {
                    if (searchRequestDto.Sort[1] == "descending")
                    {
                        if (searchRequestDto.Sort[0] == "actualDailyProductionAmount")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenByDescending(x => x.ProSumActualDailyProductionAmount).ThenByDescending(x => x.DayActualProductionAmount).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "actualDailyProduction")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenByDescending(x => x.ProSumActualDailyProduction).ThenByDescending(x => x.DayActualProductionAmount).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "outsourcingExpensesAmount")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenByDescending(x => x.ProSumOutsourcingExpensesAmount).ThenByDescending(x => x.DayActualProductionAmount).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "unitPrice")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenByDescending(x => x.ProSumUnitPrice).ThenByDescending(x => x.DayActualProductionAmount).ToList();
                        }
                    }
                    else if (searchRequestDto.Sort[1] == "ascending")
                    {
                        if (searchRequestDto.Sort[0] == "actualDailyProductionAmount")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenBy(x => x.ProSumActualDailyProductionAmount).ThenBy(x => x.DayActualProductionAmount).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "actualDailyProduction")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenBy(x => x.ProSumActualDailyProduction).ThenBy(x => x.DayActualProductionAmount).ToList();

                        }
                        else if (searchRequestDto.Sort[0] == "outsourcingExpensesAmount")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenBy(x => x.ProSumOutsourcingExpensesAmount).ThenBy(x => x.DayActualProductionAmount).ToList();
                        }
                        else if (searchRequestDto.Sort[0] == "unitPrice")
                        {
                            projectList = list.OrderByDescending(x => x.DateDay).ThenBy(x => x.ProSumUnitPrice).ThenBy(x => x.DayActualProductionAmount).ToList();
                        }
                    }

                }
                else
                {
                    projectList = list.OrderByDescending(x => x.DateDay).ToList();
                }
                #endregion
            }

            responseDtos.dayReportInfos = projectList;

            responseAjaxResult.Data = responseDtos;
            responseAjaxResult.Count = total;
            return responseAjaxResult;
        }


        /// <summary>
        /// 获取产值日报处理过的数据
        /// </summary>
        /// <param name="safeDayReports"></param>
        /// <param name="institution"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<DayReportInfo>> GetDayResponseDtosAsync(List<DayReportInfo> list, List<Institution> institution)
        {
            //获取项目id与日报id
            var projectIds = list.Select(x => x.ProjectId.ToString()).ToList();
            var dayIds = list.Select(x => x.DayId).ToList();
            //类型Id
            var typeIds = list.Select(x => x.ProjectType).ToList();
            //状态Id
            var statusIds = list.Select(x => x.ProjectStatus).ToList();
            //用户Id
            var userIds = list.Select(x => x.CreateUser).ToList();
            //公司Id
            var companyIds = list.Select(x => x.CompanyName).ToList();

            //获取项目类型
            var typeList = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && typeIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取项目状态
            var statusList = await dbContext.Queryable<ProjectStatus>().Where(x => x.IsDelete == 1 && statusIds.Contains(x.StatusId.ToString())).ToListAsync();
            //获取用户信息
            var userList = await dbContext.Queryable<Domain.Models.User>().Where(x => x.IsDelete == 1 && userIds.Contains(x.Id.ToString())).ToListAsync();
            //获取公司信息
            var companyList = institution.Where(x => x.IsDelete == 1 && companyIds.Contains(x.PomId.ToString())).ToList();

            //查询项目日报下的施工记录集合
            var dayConstructionList = await dbContext.Queryable<DayReportConstruction>().Where(x => x.IsDelete == 1 && dayIds.Contains(x.DayReportId)).ToListAsync();
            var ownerIds = dayConstructionList.Where(x => x.OwnerShipId != null).Select(x => x.OwnerShipId).ToList();
            var subIds = dayConstructionList.Where(x => x.SubShipId != null).Select(x => x.SubShipId).ToList();
            //获取wbs数据
            var projectWbsList = await dbContext.Queryable<ProjectWBS>().Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId)).ToListAsync();
            //获取自有船舶数据
            var ownerShipList = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && ownerIds.Contains(x.PomId)).ToListAsync();
            //获取分包船舶数据
            var subShipList = await dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1 && subIds.Contains(x.PomId)).ToListAsync();
            //获取往来单位数据
            var dealingUnit = await dbContext.Queryable<DealingUnit>().Where(x => x.IsDelete == 1 && subIds.Contains(x.PomId)).ToListAsync();

            foreach (var item in list)
            {
                item.CompanyName = companyList.FirstOrDefault(x => x.PomId.ToString() == item.CompanyName)?.Name;
                item.ProjectType = typeList.FirstOrDefault(x => x.PomId.ToString() == item.ProjectType)?.Name;
                item.ProjectStatus = statusList.FirstOrDefault(x => x.StatusId.ToString() == item.ProjectStatus)?.Name;
                item.CreateUser = userList.FirstOrDefault(x => x.Id.ToString() == item.CreateUser)?.Name;
                item.DayReportConstructions = dayConstructionList.Where(x => x.DayReportId == item.DayId).Select(x => new DayResConstruction
                {
                    ConstructionId = x.Id,
                    PId = item.ProjectId,
                    ProjectWBSId = x.ProjectWBSId,
                    OutPutType = x.OutPutType,
                    OwnerShipId = x.OwnerShipId,
                    SubShipId = x.SubShipId,
                    UnitPrice = Math.Round(x.UnitPrice * item.CurrencyExchangeRate, 2),
                    OutsourcingExpensesAmount = (_currentUser.CurrentLoginIsAdmin || _currentUser.CurrentLoginInstitutionOid == "101162350") && item .IsSubContractProject == 1 ? 0 : Math.Round(x.OutsourcingExpensesAmount * item.CurrencyExchangeRate, 2),//处理分包项目
                    ActualDailyProduction = (_currentUser.CurrentLoginIsAdmin || _currentUser.CurrentLoginInstitutionOid == "101162350") && item.IsSubContractProject == 1 ? 0:Math.Round(x.ActualDailyProduction, 3),//处理分包项目
                    ActualDailyProductionAmount = (_currentUser.CurrentLoginIsAdmin || _currentUser.CurrentLoginInstitutionOid == "101162350") && item.IsSubContractProject == 1 ? 0 : Math.Round(x.ActualDailyProductionAmount, 2),//处理分包项目
                    ConstructionNature = x.ConstructionNature,
                    OwnerShipName = ownerShipList.FirstOrDefault(t => t.PomId == x.OwnerShipId)?.Name,
                    SubShipName = subShipList.FirstOrDefault(t => t.PomId == x.SubShipId)?.Name == null ? dealingUnit.FirstOrDefault(t => t.PomId == x.SubShipId)?.ZBPNAME_ZH : subShipList.FirstOrDefault(t => t.PomId == x.SubShipId)?.Name,
                    ProjectWBSName = GetProjectWBSLevelName(projectWbsList.Where(x => x.ProjectId == item.ProjectId.ToString()).ToList(), projectWbsList.FirstOrDefault(t => t.Id == x.ProjectWBSId)?.Name, projectWbsList.FirstOrDefault(t => t.Id == x.ProjectWBSId)?.Pid)
                }).ToArray();
                if (item.Sort.Any())
                {
                    if (item.Sort[1] == "descending")
                    {
                        if (item.Sort[0] == "actualDailyProductionAmount")
                        {
                            var a = "";
                            item.DayReportConstructions = item.DayReportConstructions.OrderByDescending(x => x.ActualDailyProductionAmount).ToArray();
                        }
                        else if (item.Sort[0] == "actualDailyProduction")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderByDescending(x => x.ActualDailyProduction).ToArray();

                        }
                        else if (item.Sort[0] == "outsourcingExpensesAmount")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderByDescending(x => x.OutsourcingExpensesAmount).ToArray();
                        }
                        else if (item.Sort[0] == "unitPrice")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderByDescending(x => x.UnitPrice).ToArray();
                        }
                    }
                    else
                    {
                        if (item.Sort[0] == "actualDailyProductionAmount")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderBy(x => x.ActualDailyProductionAmount).ToArray();
                        }
                        else if (item.Sort[0] == "actualDailyProduction")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderBy(x => x.ActualDailyProduction).ToArray();

                        }
                        else if (item.Sort[0] == "outsourcingExpensesAmount")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderBy(x => x.OutsourcingExpensesAmount).ToArray();
                        }
                        else if (item.Sort[0] == "unitPrice")
                        {
                            item.DayReportConstructions = item.DayReportConstructions.OrderBy(x => x.UnitPrice).ToArray();
                        }
                    }
                }
                //item.ProSumOutsourcingExpensesAmount = Math.Round(Convert.ToDecimal(item.DayReportConstructions.Sum(x => x.OutsourcingExpensesAmount)), 2);
                //item.ProSumActualDailyProduction = Math.Round(Convert.ToDecimal(item.DayReportConstructions.Sum(x => x.ActualDailyProduction)), 2);
                //item.ProSumActualDailyProductionAmount = Math.Round(Convert.ToDecimal(item.DayReportConstructions.Sum(x => x.ActualDailyProductionAmount)), 2);
            }
            return list;
        }

        /// <summary>
        /// 获取产值日报excel处理过的数据
        /// </summary>
        /// <param name="safeDayReports"></param>
        /// <returns></returns>
        public async Task<List<DayReportExcel>> GetDayExcelResponseDtosAsync(List<DayReportInfo> list, List<Institution> institution)
        {
            var data = await GetDayResponseDtosAsync(list, institution);
            var constructionList = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && x.TypeNo == 7).ToListAsync();
            List<DayReportExcel> dayReportExcelList = new List<DayReportExcel>();
            foreach (var item in data)
            {
                foreach (var item2 in item.DayReportConstructions)
                {
                    DayReportExcel dayReportExcel = new DayReportExcel();
                    dayReportExcel.IsHoliday = item.IsHoliday;
                    dayReportExcel.ProjectName = item.ProjectName;
                    dayReportExcel.CompanyName = item.CompanyName;
                    dayReportExcel.ProjectCategory = item.ProjectCategory;
                    dayReportExcel.ProjectType = item.ProjectType;
                    dayReportExcel.DateDay = item.DateDay;
                    dayReportExcel.LandWorkplace = item.LandWorkplace;
                    dayReportExcel.ShiftLeader = item.ShiftLeader;
                    dayReportExcel.ShiftLeaderPhone = item.ShiftLeaderPhone;
                    dayReportExcel.FewLandWorkplace = item.FewLandWorkplace;
                    dayReportExcel.SiteShipNum = item.SiteShipNum;
                    dayReportExcel.OnShipPersonNum = item.OnShipPersonNum;
                    dayReportExcel.HazardousConstructionDescription = item.HazardousConstructionDescription;
                    dayReportExcel.ProjectStatus = item.ProjectStatus;
                    dayReportExcel.Amount = item.Amount;
                    dayReportExcel.ProjectWBSName = item2.ProjectWBSName;
                    dayReportExcel.ConstructionNature = constructionList.FirstOrDefault(x => x.Type == item2.ConstructionNature)?.Name;
                    dayReportExcel.OutPutType = EnumExtension.GetEnumDescription(item2.OutPutType);
                    if (item2.OutPutType == ConstructionOutPutType.Self || item2.OutPutType == ConstructionOutPutType.SubOwner)
                    {
                        dayReportExcel.Resources = item2.OwnerShipName;
                    }
                    else if (item2.OutPutType == ConstructionOutPutType.SubPackage)
                    {
                        dayReportExcel.Resources = item2.SubShipName;
                    }
                    dayReportExcel.UnitPrice = item2.UnitPrice;
                    dayReportExcel.OutsourcingExpensesAmount = item2.OutsourcingExpensesAmount;
                    dayReportExcel.ActualDailyProduction = item2.ActualDailyProduction;
                    dayReportExcel.ActualDailyProductionAmount = item2.ActualDailyProductionAmount;
                    dayReportExcelList.Add(dayReportExcel);
                }
            }

            return dayReportExcelList;
        }

        /// <summary>
        /// 船舶日报
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<ShipsDayReportResponseDto>> SearchShipDayReportAsync(ShipDailyRequestDto searchRequestDto)
        {
            ResponseAjaxResult<ShipsDayReportResponseDto> responseAjaxResult = new ResponseAjaxResult<ShipsDayReportResponseDto>();
            ShipsDayReportResponseDto shipsDayReportResponseDto = new ShipsDayReportResponseDto();
            RefAsync<int> total = 0;
            //属性标签
            var categoryList = new List<int>();
            var tagList = new List<int>();
            var tag2List = new List<int>();
            if (searchRequestDto.TagName != null && searchRequestDto.TagName.Any())
            {
                foreach (var item in searchRequestDto.TagName)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            List<Guid> departmentIds = new List<Guid>();
            //获取所有机构
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            if (searchRequestDto.IsDuiWai == true)
            {
                _currentUser.CurrentLoginIsAdmin = true;
            }
            if (_currentUser.CurrentLoginIsAdmin)
            {
                departmentIds = institution.Select(x => x.PomId.Value).ToList();
                departmentIds.Add(_currentUser.CurrentLoginInstitutionId);
            }
            else
            {
                //获取当前登陆人是否属于公司登陆系统 
                var curLogin = institution.Where(x => x.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
                if (curLogin != null)
                {
                    if (curLogin.Ocode.Contains("0000A") || curLogin.Ocode.Contains("0000B") || curLogin.Ocode.Contains("0000C") || curLogin.Ocode.Contains("0000E"))
                    {
                        var reverse = curLogin.Grule.Split('-').Reverse().ToArray()[1];
                        departmentIds = institution.Where(x => x.Grule.Contains(reverse)).Select(x => x.PomId.Value).ToList();
                    }
                    else
                    {
                        departmentIds.Add(_currentUser.CurrentLoginDepartmentId.Value);
                    }
                    departmentIds.Add(curLogin.PomId.Value);
                }
            }
            var departmentIdss = departmentIds.Select(x => (Guid?)x);
            //过滤特殊字符
            searchRequestDto.ProjectName = Utils.ReplaceSQLChar(searchRequestDto.ProjectName);

            var time = 0;
            if (searchRequestDto.StartTime != null && searchRequestDto.EndTime != null)
            {
                if (searchRequestDto.StartTime == searchRequestDto.EndTime)
                {
                    shipsDayReportResponseDto.TimeValue = searchRequestDto.StartTime.Value.Year + "年" + searchRequestDto.StartTime.Value.Month + "月" + searchRequestDto.StartTime.Value.Day + "日";
                }
                else
                {
                    shipsDayReportResponseDto.TimeValue = searchRequestDto.StartTime.Value.Year + "年" + searchRequestDto.StartTime.Value.Month + "月" + searchRequestDto.StartTime.Value.Day + "日--" + searchRequestDto.EndTime.Value.Year + "年" + searchRequestDto.EndTime.Value.Month + "月" + searchRequestDto.EndTime.Value.Day + "日";
                }
            }
            else
            {
                time = DateTime.Now.Date.AddDays(-1).ToDateDay();
                shipsDayReportResponseDto.TimeValue = DateTime.Now.Date.AddDays(-1).Year + "年" + DateTime.Now.Date.AddDays(-1).Month + "月" + DateTime.Now.Date.AddDays(-1).Day + "日";

            }
            //获取当前角色信息  判断是否是管理员或者公司管理员  不是的话 只展示关联项目的数据
            var IsAdmin = true;
            if (_currentUser.RoleInfos != null)
            {
                var curRoleInfo = _currentUser.RoleInfos.Where(role => role.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
                if (curRoleInfo.IsAdmin || (curRoleInfo != null && curRoleInfo.Type == 2))//超级管理员  或者  管理员
                {
                    IsAdmin = false;
                }
            }
            if (searchRequestDto.IsDuiWai == true)
            {
                IsAdmin = false;
            }
            //船舶进退场（新逻辑）
            var shiMovenmentList = await dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1 && x.Status == ShipMovementStatus.Enter).ToListAsync();
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();

            //获取需要查询的字段
            var selectExpr = GetSelectExp();
            var list = new List<ShipsDayReportInfo>();
            if (!searchRequestDto.IsDuiWai)//不是对外接口  属于监控中心列表
            {
                list = await dbContext.Queryable<ShipDayReport, Project, OwnerShip>(
                    (d, p, s) =>
                    new JoinQueryInfos(
                        JoinType.Left, d.ProjectId == p.Id && departmentIdss.Contains(p.ProjectDept),
                        JoinType.Left, d.ShipId == s.PomId
                    ))
                    .Where((d, p, s) => d.IsDelete == 1)
                    .OrderBy((d, p) => new { DateDay = SqlFunc.Desc(d.DateDay), Name = SqlFunc.Desc(p.Name) })
                    .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ShipPingId.ToString()), (d, p) => d.ShipId == searchRequestDto.ShipPingId)
                    .WhereIF(searchRequestDto.StartTime == null && searchRequestDto.EndTime == null, (d, p) => d.DateDay == time)
                    .WhereIF(searchRequestDto.StartTime != null && searchRequestDto.EndTime != null, (d, p) => d.DateDay >= searchRequestDto.StartTime.Value.Date.ToDateDay() && d.DateDay <= searchRequestDto.EndTime.Value.Date.ToDateDay())
                    .WhereIF(IsAdmin, (d, p) => d.ShipDayReportType == ShipDayReportType.ProjectShip)
                    .WhereIF(searchRequestDto.ShipState != null, (d, p) => d.ShipState == searchRequestDto.ShipState)
                    .WhereIF(searchRequestDto.CompanyId != null, (d, p) => p.CompanyId == searchRequestDto.CompanyId)
                    .WhereIF(searchRequestDto.ProjectDept != null, (d, p) => p.ProjectDept == searchRequestDto.ProjectDept)
                    .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), (d, p) => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                    .WhereIF(searchRequestDto.ProjectTypeId != null, (d, p) => p.TypeId == searchRequestDto.ProjectTypeId)
                    .WhereIF(searchRequestDto.ShipTypeId != null, (d, p, s) => s.TypeId == searchRequestDto.ShipTypeId)
                    .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), (d, p) => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                    .WhereIF(categoryList != null && categoryList.Any(), (d, p) => categoryList.Contains(p.Category))
                    .WhereIF(tagList != null && tagList.Any(), (d, p) => tagList.Contains(p.Tag))
                    .WhereIF(tag2List != null && tag2List.Any(), (d, p) => tag2List.Contains(p.Tag2))
                    .Select(selectExpr).ToListAsync();
            }
            else
            {
                list = await dbContext.Queryable<ShipDayReport, Project, OwnerShip>(
                    (d, p, s) =>
                    new JoinQueryInfos(
                        JoinType.Left, d.ProjectId == p.Id && departmentIdss.Contains(p.ProjectDept),
                        JoinType.Left, d.ShipId == s.PomId
                    ))
                    .OrderBy((d, p) => new { DateDay = SqlFunc.Desc(d.DateDay), Name = SqlFunc.Desc(p.Name) })
                    .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ShipPingId.ToString()), (d, p) => d.ShipId == searchRequestDto.ShipPingId)
                    .WhereIF(IsAdmin, (d, p) => d.ShipDayReportType == ShipDayReportType.ProjectShip)
                    .WhereIF(searchRequestDto.ShipState != null, (d, p) => d.ShipState == searchRequestDto.ShipState)
                    .WhereIF(searchRequestDto.CompanyId != null, (d, p) => p.CompanyId == searchRequestDto.CompanyId)
                    .WhereIF(searchRequestDto.ProjectDept != null, (d, p) => p.ProjectDept == searchRequestDto.ProjectDept)
                    .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), (d, p) => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                    .WhereIF(searchRequestDto.ProjectTypeId != null, (d, p) => p.TypeId == searchRequestDto.ProjectTypeId)
                    .WhereIF(searchRequestDto.ShipTypeId != null, (d, p, s) => s.TypeId == searchRequestDto.ShipTypeId)
                    .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), (d, p) => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                    .WhereIF(categoryList != null && categoryList.Any(), (d, p) => categoryList.Contains(p.Category))
                    .WhereIF(tagList != null && tagList.Any(), (d, p) => tagList.Contains(p.Tag))
                    .WhereIF(tag2List != null && tag2List.Any(), (d, p) => tag2List.Contains(p.Tag2))
                    .Select(selectExpr).ToListAsync();
                //对外接口日期条件筛选
                list = list
                    .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                     x.CreateTime >= searchRequestDto.StartTime && x.CreateTime <= searchRequestDto.EndTime
                    : x.UpdateTime >= searchRequestDto.StartTime && x.UpdateTime <= searchRequestDto.EndTime)
                    .ToList();
            }

            #region 新逻辑
            //新逻辑
            var primaryIds = list.Where(t => t.ProjectId == Guid.Empty).Select(t => new { id = t.Id, shipId = t.ShipId, DateDay = t.DateDay, ProjectId = t.ProjectId }).ToList();
            foreach (var item in primaryIds)
            {
                //var isExistProject = shiMovenmentList.Where(x => x.IsDelete == 1 && x.ShipId == item.shipId && x.EnterTime.HasValue == true && (x.EnterTime.Value.ToDateDay() <= item.DateDay || x.QuitTime.HasValue == true && x.QuitTime.Value.ToDateDay() >= item.DateDay)).OrderByDescending(x => x.EnterTime).ToList();
                var isExistProject = shiMovenmentList.Where(x => x.IsDelete == 1 && x.ShipId == item.shipId).OrderByDescending(x => x.EnterTime).ToList();
                if (isExistProject.Count > 0)
                {
                    foreach (var project in isExistProject)
                    {

                        if (project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                        {

                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;
                                oldValue.ProjectName = projectList.Where(x => x.Id == project.ProjectId).FirstOrDefault()?.Name;
                                oldValue.shipDayReportType = 1;
                            }
                        }
                        if (project.EnterTime.HasValue && project.QuitTime.HasValue == false && project.EnterTime.Value.ToDateDay() <= item.DateDay)
                        {

                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;
                                oldValue.ProjectName = projectList.Where(x => x.Id == project.ProjectId).FirstOrDefault()?.Name;
                                oldValue.shipDayReportType = 1;
                            }
                        }
                        if (project.EnterTime.HasValue && project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                        {

                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;
                                oldValue.ProjectName = projectList.Where(x => x.Id == project.ProjectId).FirstOrDefault()?.Name;
                                oldValue.shipDayReportType = 1;
                            }
                        }
                        if (project.Status == ShipMovementStatus.Enter && project.EnterTime == null && project.QuitTime == null)
                        {
                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;
                                oldValue.ProjectName = projectList.Where(x => x.Id == project.ProjectId).FirstOrDefault()?.Name;
                                oldValue.shipDayReportType = 1;
                            }
                        }

                    }
                }
                else
                {
                    var oldValue = list.Where(t => t.ShipId == item.shipId && t.DateDay == item.DateDay).FirstOrDefault();
                    if (oldValue != null)
                    {

                        oldValue.shipDayReportType = 2;
                    }
                }

            }

            #endregion

            #region 获取所有的数据
            shipsDayReportResponseDto.sumShipDayValue = await GetSumShipValueAsync(list);
            #endregion
            //是否全量导出
            var shipList = new List<ShipsDayReportInfo>();
            if (!searchRequestDto.IsFullExport)
            {
                shipList = list.OrderBy(x => x.ShipId).ToList();
                total = shipList.Count;
                shipList = shipList.Skip((searchRequestDto.PageIndex - 1) * searchRequestDto.PageSize).Take(searchRequestDto.PageSize).ToList();
            }
            else
            {
                shipList = list.OrderBy(x => x.ShipId).ToList();
                total = shipList.Count;
            }
            //土质
            var soilData = await dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 5).Select(x => new { x.Type, x.Name }).ToListAsync();
            foreach (var item in shipList)
            {
                var types = item.SoilQuality.Split(',').ToList();
                //若数组中存在空元素则直接跳过
                bool emptyFlag = types.Any(x => string.IsNullOrWhiteSpace(x));
                if (emptyFlag)
                {
                    continue;
                }
                var typeName = string.Empty;
                types.ForEach(x =>
                {
                    typeName += soilData.FirstOrDefault(y => y.Type == Convert.ToInt32(x))?.Name + ',';
                });
                if (!string.IsNullOrWhiteSpace(typeName))
                {
                    typeName = typeName.Substring(0, typeName.Length - 1);
                }
                item.SoilQuality = typeName;

            }
            //获取处理完的数据
            var result = await GetShipResponseDtosAsync(shipList, institution);
            var removeList = result.Where(t => t.ShipState == 0 && t.ProjectName == "非关联项目").Select(t => t.Id).ToList();
            result = result.Where(t => !removeList.Contains(t.Id)).ToList();
            shipsDayReportResponseDto.shipsDayReportInfos = result;
            responseAjaxResult.Data = shipsDayReportResponseDto;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        public Expression<Func<ShipDayReport, Project, ShipsDayReportInfo>> GetSelectExp()
        {
            Expression<Func<ShipDayReport, Project, ShipsDayReportInfo>> selectExpr = (d, p) => new ShipsDayReportInfo()
            {
                Id = d.Id,
                ProjectId = d.ProjectId,
                ShipId = d.ShipId,
                ProjectName = Convert.ToInt32(d.ShipDayReportType) == (int)ShipDayReportType.ProjectShip ? p.Name : CommonData.ShipProjectName,
                AreaName = d.ConstructionArea,
                AverageEDepth = d.AverageExcavationDepth,
                FloatingLength = d.FloatingTubeLength,
                ForwardDistanceCabinNumberOfBarges = d.ForwardNumber,
                AverageExcavationWidth = d.AverageExcavationWidth,
                LengthOfShorePipe = d.ShorePipeLength,
                ImmersedTubeLength = d.ImmersedTubeLength,
                ConstructionAndLayout = d.ConstructionLayout,
                StandbyMachine = d.StandbyMachine,
                DownMoveAnchorView = d.DownAnchor,
                MovingAShip = d.MovingShip,
                WeldingCutterToothSeat = d.WeldingCutter,
                GearChange = d.ChangeGear,
                Supply = d.Supply,
                MeasurementImpact = d.MeasurementImpact,
                LayingPipelines = d.LayingPipelines,
                AdjustingThePipeline = d.AdjustingPipeline,
                MudCleaningPump = d.CleaningPump,
                GrabRefueling = d.GrabRefueling,
                WaitingForTransfer = d.WaitingReplaceRefueling,
                ReplaceTheSteelWire = d.ReplaceWire,
                AvoidingShips = d.AvoidingShip,
                WeatherEffect = d.WeatherImpact,
                TidalInfluence = d.TideImpact,
                SuddenFailure = d.SuddenFailure,
                WaitingForSparePartsToBeRepaired = d.WaitingSparePartRepair,
                OilAndWaterEquivalentMaterials = d.WaitingOilWaterMaterial,
                WaitingForBargeAndTowing = d.WaitingBargeAndTowing,
                NotifyShutdown = d.NotifyShutdown,
                EquipmentModificationAndMaintenance = d.EquipmentModificationMaintenance,
                FloatingShorePipeFailure = d.FSPipeFailure,
                SunkenTubeFailure = d.SunkenTubeFailure,
                SocialInterference = d.SocialInterference,
                CofferdamDrainageIssues = d.CofferdamDrainageIssues,
                NonProductiveDowntime_Other = d.Other,
                ExecuteDispatch = d.ExecuteDispatch,
                ShipConversion = d.ShipConversion,
                SealedWarehouseAtSea = d.SealedForGoSea,
                ApplyForInspection = d.ApplyForInspection,
                ShipyardBaseRepair = d.ShipyardRepair,
                MonthlyPreventiveMaintenance = d.MonthlyPreventiveMaintenance,
                Standby = d.Standby,
                OffSiteStandby = d.OffSiteStandby,
                CustomsClearanceAndCustomsDeclaration = d.CustomsClearanceAndDeclaration,
                CleanTheCutterRakeHeadSuctionPort = d.CleaningCRS,
                PrepareForDispatch = d.PrepareDispatch,
                EstimatingOilPrices = d.FuelUnitPrice,
                EstimatedUnitPrice = d.EstimatedUnitPrice,
                SoilQuality = d.SoilQuality,
                ShipState = Convert.ToInt32(d.ShipState),
                shipDayReportType = Convert.ToInt32(d.ShipDayReportType),
                ShipReportedProduction = d.ShipReportedProduction,
                PipeLineLength = d.PipelineLength,
                OilConsumption = d.OilConsumption,
                EstimatedCostAmount = d.EstimatedCostAmount,
                EstimatedOutputAmount = d.EstimatedOutputAmount,
                Dredge = d.Dredge,
                Sail = d.Sail,
                SedimentDisposal = d.SedimentDisposal,
                BlowShore = d.BlowShore,
                BlowingWater = d.BlowingWater,
                DateDay = d.DateDay,
                Remarks = d.Remarks,
                UpdateTime = d.UpdateTime,
                CreateTime = d.CreateTime,
                ProductionOperatingTime = SqlFunc.IsNull(d.Dredge, 0) + SqlFunc.IsNull(d.Sail, 0) + SqlFunc.IsNull(d.BlowingWater, 0) + SqlFunc.IsNull(d.BlowShore, 0) + SqlFunc.IsNull(d.SedimentDisposal, 0),
                ProductionStoppage = SqlFunc.IsNull(d.ConstructionLayout, 0) + SqlFunc.IsNull(d.StandbyMachine, 0) + SqlFunc.IsNull(d.DownAnchor, 0) + SqlFunc.IsNull(d.MovingShip, 0) + SqlFunc.IsNull(d.WeldingCutter, 0) + SqlFunc.IsNull(d.ChangeGear, 0) + SqlFunc.IsNull(d.Supply, 0) + SqlFunc.IsNull(d.MeasurementImpact, 0) + SqlFunc.IsNull(d.LayingPipelines, 0) + SqlFunc.IsNull(d.AdjustingPipeline, 0) + SqlFunc.IsNull(d.CleaningPump, 0) + SqlFunc.IsNull(d.CleaningCRS, 0) + SqlFunc.IsNull(d.WaitingBargeAndTowing, 0) + SqlFunc.IsNull(d.ReplaceWire, 0) + SqlFunc.IsNull(d.AvoidingShip, 0),
                NonProductionStoppage = SqlFunc.IsNull(d.WeatherImpact, 0) + SqlFunc.IsNull(d.TideImpact, 0) + SqlFunc.IsNull(d.SuddenFailure, 0) + SqlFunc.IsNull(d.WaitingSparePartRepair, 0) + SqlFunc.IsNull(d.WaitingOilWaterMaterial, 0) + SqlFunc.IsNull(d.WaitingBargeAndTowing, 0) + SqlFunc.IsNull(d.NotifyShutdown, 0) + SqlFunc.IsNull(d.EquipmentModificationMaintenance, 0) + SqlFunc.IsNull(d.Standby, 0) + SqlFunc.IsNull(d.FSPipeFailure, 0) + SqlFunc.IsNull(d.SunkenTubeFailure, 0) + SqlFunc.IsNull(d.SocialInterference, 0) + SqlFunc.IsNull(d.CofferdamDrainageIssues, 0) + SqlFunc.IsNull(d.Other, 0),
                Dispatch = SqlFunc.IsNull(d.ExecuteDispatch, 0) + SqlFunc.IsNull(d.PrepareDispatch, 0) + SqlFunc.IsNull(d.ShipConversion, 0) + SqlFunc.IsNull(d.SealedForGoSea, 0) + SqlFunc.IsNull(d.ApplyForInspection, 0),
                TimedPause = SqlFunc.IsNull(d.ShipyardRepair, 0) + SqlFunc.IsNull(d.MonthlyPreventiveMaintenance, 0),
                OtherTime = SqlFunc.IsNull(d.OffSiteStandby, 0) + SqlFunc.IsNull(d.CustomsClearanceAndDeclaration, 0),
                PipeLineManageMent = SqlFunc.IsNull(d.LayingPipelines, 0) + SqlFunc.IsNull(d.AdjustingPipeline, 0) + SqlFunc.IsNull(d.FSPipeFailure, 0) + SqlFunc.IsNull(d.SunkenTubeFailure, 0),
                ShipEngineFailure = SqlFunc.IsNull(d.SuddenFailure, 0) + SqlFunc.IsNull(d.WaitingSparePartRepair, 0) + SqlFunc.IsNull(d.EquipmentModificationMaintenance, 0),
                FaultsRepairs = SqlFunc.IsNull(d.SuddenFailure, 0) + SqlFunc.IsNull(d.WaitingSparePartRepair, 0) + SqlFunc.IsNull(d.EquipmentModificationMaintenance, 0) + SqlFunc.IsNull(d.ShipyardRepair, 0) + SqlFunc.IsNull(d.MonthlyPreventiveMaintenance, 0)
            };
            return selectExpr;
        }



        /// <summary>
        /// 获取船舶日报需要计算的合计值
        /// </summary>
        /// <param name="dayReports"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SumShipDayValue> GetSumShipValueAsync(List<ShipsDayReportInfo> dayReports)
        {
            SumShipDayValue sumShipDayValue = new SumShipDayValue();
            sumShipDayValue.SumShipReportedProduction = Math.Round(Convert.ToDecimal(dayReports.Sum(x => x.ShipReportedProduction)), 2);
            sumShipDayValue.SumOilConsumption = Math.Round(Convert.ToDecimal(dayReports.Sum(x => x.OilConsumption)), 2);
            sumShipDayValue.SumEstimatedCostAmount = Math.Round(Convert.ToDecimal(dayReports.Sum(x => x.EstimatedCostAmount)), 2);
            sumShipDayValue.SumEstimatedOutputAmount = Math.Round(Convert.ToDecimal(dayReports.Sum(x => x.EstimatedOutputAmount)), 2);
            sumShipDayValue.SumOperatingTime = dayReports.Sum(x => x.ProductionOperatingTime);
            sumShipDayValue.SumProductionStoppage = dayReports.Sum(x => x.ProductionStoppage);
            sumShipDayValue.SumNonProductionStoppage = dayReports.Sum(x => x.NonProductionStoppage);
            sumShipDayValue.SumDispatch = dayReports.Sum(x => x.Dispatch);
            sumShipDayValue.SumTimedPause = dayReports.Sum(x => x.TimedPause);
            sumShipDayValue.SumOtherTime = dayReports.Sum(x => x.OtherTime);
            return sumShipDayValue;
        }

        /// <summary>
        /// 获取船舶日报处理过的数据
        /// </summary>
        /// <param name="safeDayReports"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ShipsDayReportInfo>> GetShipResponseDtosAsync(List<ShipsDayReportInfo> list, List<Institution> institution)
        {
            //获取项目Id
            var projectIds = list.Select(x => x.ProjectId).ToList();
            //获取船舶Id
            var shipIds = list.Select(x => x.ShipId).ToList();

            //获取船舶信息
            var shipList = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && shipIds.Contains(x.PomId)).ToListAsync();
            //获取船舶的公司id
            var companyIds = shipList.Select(x => x.CompanyId).ToList();
            //获取公司信息
            var companyList = institution.Where(x => x.IsDelete == 1 && companyIds.Contains(x.PomId)).ToList();
            //获取船舶进退场信息
            var shipMoveMent = await dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1 && x.ShipType == ShipType.OwnerShip).ToListAsync();

            foreach (var item in list)
            {
                var shipCompanyId = shipList.FirstOrDefault(x => x.PomId == item.ShipId)?.CompanyId;
                item.ShipCompany = companyList.FirstOrDefault(x => x.PomId == shipCompanyId)?.Name;
                item.ShipName = shipList.FirstOrDefault(x => x.PomId == item.ShipId)?.Name;
                item.TypeClass = shipList.FirstOrDefault(x => x.PomId == item.ShipId)?.TypeClass;
                item.OperatingTime = (item.ProductionOperatingTime ?? 0) + (item.ProductionStoppage ?? 0) + (item.NonProductionStoppage ?? 0) + (item.Dispatch ?? 0) + (item.TimedPause ?? 0) + (item.OtherTime ?? 0);
                if (item.OperatingTime != 0)
                    //item.TimeAvailability = Math.Round((Convert.ToDecimal(item.ProductionOperatingTime / item.OperatingTime) * 100), 2);
                    item.TimeAvailability = Math.Round((Convert.ToDecimal(item.ProductionOperatingTime / 24) * 100), 2);
                if (item.ProductionOperatingTime != 0)
                    item.ConstructionEfficiency = Math.Round(Convert.ToDecimal(item.ShipReportedProduction / item.ProductionOperatingTime), 2);
                item.EnterTime = shipMoveMent.FirstOrDefault(x => x.ProjectId == item.ProjectId && x.ShipId == item.ShipId)?.EnterTime;
                item.Quittime = shipMoveMent.FirstOrDefault(x => x.ProjectId == item.ProjectId && x.ShipId == item.ShipId)?.QuitTime;
                item.AverageEDepth = Math.Round(Convert.ToDecimal(item.AverageEDepth), 2);
                item.FloatingLength = Math.Round(Convert.ToDecimal(item.FloatingLength), 2);
                item.ForwardDistanceCabinNumberOfBarges = Math.Round(Convert.ToDecimal(item.ForwardDistanceCabinNumberOfBarges), 2);
                item.AverageExcavationWidth = Math.Round(Convert.ToDecimal(item.AverageExcavationWidth), 2);
                item.LengthOfShorePipe = Math.Round(Convert.ToDecimal(item.LengthOfShorePipe), 2);
                item.ImmersedTubeLength = Math.Round(Convert.ToDecimal(item.ImmersedTubeLength), 2);
                item.ConstructionAndLayout = Math.Round(Convert.ToDecimal(item.ConstructionAndLayout), 2);
                item.StandbyMachine = Math.Round(Convert.ToDecimal(item.StandbyMachine), 2);
                item.DownMoveAnchorView = Math.Round(Convert.ToDecimal(item.DownMoveAnchorView), 2);
                item.MovingAShip = Math.Round(Convert.ToDecimal(item.MovingAShip), 2);
                item.WeldingCutterToothSeat = Math.Round(Convert.ToDecimal(item.WeldingCutterToothSeat), 2);
                item.GearChange = Math.Round(Convert.ToDecimal(item.GearChange), 2);
                item.Supply = Math.Round(Convert.ToDecimal(item.Supply), 2);
                item.MeasurementImpact = Math.Round(Convert.ToDecimal(item.MeasurementImpact), 2);
                item.LayingPipelines = Math.Round(Convert.ToDecimal(item.LayingPipelines), 2);
                item.AdjustingThePipeline = Math.Round(Convert.ToDecimal(item.AdjustingThePipeline), 2);
                item.MudCleaningPump = Math.Round(Convert.ToDecimal(item.MudCleaningPump), 2);
                item.GrabRefueling = Math.Round(Convert.ToDecimal(item.GrabRefueling), 2);
                item.WaitingForTransfer = Math.Round(Convert.ToDecimal(item.WaitingForTransfer), 2);
                item.ReplaceTheSteelWire = Math.Round(Convert.ToDecimal(item.ReplaceTheSteelWire), 2);
                item.AvoidingShips = Math.Round(Convert.ToDecimal(item.AvoidingShips), 2);
                item.WeatherEffect = Math.Round(Convert.ToDecimal(item.WeatherEffect), 2);
                item.TidalInfluence = Math.Round(Convert.ToDecimal(item.TidalInfluence), 2);
                item.SuddenFailure = Math.Round(Convert.ToDecimal(item.SuddenFailure), 2);
                item.WaitingForSparePartsToBeRepaired = Math.Round(Convert.ToDecimal(item.WaitingForSparePartsToBeRepaired), 2);
                item.OilAndWaterEquivalentMaterials = Math.Round(Convert.ToDecimal(item.OilAndWaterEquivalentMaterials), 2);
                item.WaitingForBargeAndTowing = Math.Round(Convert.ToDecimal(item.WaitingForBargeAndTowing), 2);
                item.NotifyShutdown = Math.Round(Convert.ToDecimal(item.NotifyShutdown), 2);
                item.EquipmentModificationAndMaintenance = Math.Round(Convert.ToDecimal(item.EquipmentModificationAndMaintenance), 2);
                item.FloatingShorePipeFailure = Math.Round(Convert.ToDecimal(item.FloatingShorePipeFailure), 2);
                item.SunkenTubeFailure = Math.Round(Convert.ToDecimal(item.SunkenTubeFailure), 2);
                item.SocialInterference = Math.Round(Convert.ToDecimal(item.SocialInterference), 2);
                item.CofferdamDrainageIssues = Math.Round(Convert.ToDecimal(item.CofferdamDrainageIssues), 2);
                item.NonProductiveDowntime_Other = Math.Round(Convert.ToDecimal(item.NonProductiveDowntime_Other), 2);
                item.ExecuteDispatch = Math.Round(Convert.ToDecimal(item.ExecuteDispatch), 2);
                item.ShipConversion = Math.Round(Convert.ToDecimal(item.ShipConversion), 2);
                item.SealedWarehouseAtSea = Math.Round(Convert.ToDecimal(item.SealedWarehouseAtSea), 2);
                item.ApplyForInspection = Math.Round(Convert.ToDecimal(item.ApplyForInspection), 2);
                item.ShipyardBaseRepair = Math.Round(Convert.ToDecimal(item.ShipyardBaseRepair), 2);
                item.MonthlyPreventiveMaintenance = Math.Round(Convert.ToDecimal(item.MonthlyPreventiveMaintenance), 2);
                item.OffSiteStandby = Math.Round(Convert.ToDecimal(item.OffSiteStandby), 2);
                item.CustomsClearanceAndCustomsDeclaration = Math.Round(Convert.ToDecimal(item.CustomsClearanceAndCustomsDeclaration), 2);
                item.CleanTheCutterRakeHeadSuctionPort = Math.Round(Convert.ToDecimal(item.CleanTheCutterRakeHeadSuctionPort), 2);
                item.PrepareForDispatch = Math.Round(Convert.ToDecimal(item.PrepareForDispatch), 2);
                item.EstimatingOilPrices = Math.Round(Convert.ToDecimal(item.EstimatingOilPrices), 2);
                item.EstimatedUnitPrice = Math.Round(Convert.ToDecimal(item.EstimatedUnitPrice), 2);
                item.ShipReportedProduction = Math.Round(Convert.ToDecimal(item.ShipReportedProduction), 2);
                item.PipeLineLength = Math.Round(Convert.ToDecimal(item.PipeLineLength), 2);
                item.OilConsumption = Math.Round(Convert.ToDecimal(item.OilConsumption), 2);
                item.EstimatedCostAmount = Math.Round(Convert.ToDecimal(item.EstimatedCostAmount), 2);
                item.EstimatedOutputAmount = Math.Round(Convert.ToDecimal(item.EstimatedOutputAmount), 2);
                item.Dredge = Math.Round(Convert.ToDecimal(item.Dredge), 2);
                item.Sail = Math.Round(Convert.ToDecimal(item.Sail), 2);
                item.SedimentDisposal = Math.Round(Convert.ToDecimal(item.SedimentDisposal), 2);
                item.BlowShore = Math.Round(Convert.ToDecimal(item.BlowShore), 2);
                item.BlowingWater = Math.Round(Convert.ToDecimal(item.BlowingWater), 2);
                item.ShipStateName = Utils.GetDescription((ProjectShipState)item.ShipState);
                if (item.ShipReportedProduction != 0)
                {
                    item.WanfangOilConsumption = Math.Round(Convert.ToDecimal(item.OilConsumption * 10000 / item.ShipReportedProduction), 2);
                }
                else
                {
                    item.WanfangOilConsumption = 0;
                }
                //            if (item.EstimatedOutputAmount != 0)
                //            {
                //	item.DayGrossMargin = (item.EstimatedOutputAmount - item.EstimatedCostAmount) / item.EstimatedOutputAmount;
                //            }
                //            else
                //            {
                //	item.DayGrossMargin = 0;

                //}
            }
            return list;
        }



        /// <summary>
        /// 安监日报
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<SafeDayReportResponseDto>> SearchSafeDayReportAsync(ProductionSafetyRequestDto searchRequestDto)
        {
            ResponseAjaxResult<SafeDayReportResponseDto> responseAjaxResult = new ResponseAjaxResult<SafeDayReportResponseDto>();
            SafeDayReportResponseDto reportResponseDto = new SafeDayReportResponseDto();
            RefAsync<int> total = 0;
            //属性标签
            var categoryList = new List<int>();
            var tagList = new List<int>();
            var tag2List = new List<int>();
            if (searchRequestDto.TagName != null && searchRequestDto.TagName.Any())
            {
                foreach (var item in searchRequestDto.TagName)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            List<Guid> departmentIds = new List<Guid>();
            //获取所有机构
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            if (_currentUser.CurrentLoginIsAdmin)
            {
                departmentIds = institution.Select(x => x.PomId.Value).ToList();
                departmentIds.Add(_currentUser.CurrentLoginInstitutionId);
            }
            else
            {
                //获取当前登陆人是否属于公司登陆系统 
                var curLogin = institution.Where(x => x.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
                if (curLogin != null)
                {
                    if (curLogin.Ocode.Contains("0000A") || curLogin.Ocode.Contains("0000B") || curLogin.Ocode.Contains("0000C") || curLogin.Ocode.Contains("0000E"))
                    {
                        var reverse = curLogin.Grule.Split('-').Reverse().ToArray()[1];
                        departmentIds = institution.Where(x => x.Grule.Contains(reverse)).Select(x => x.PomId.Value).ToList();
                    }
                    else
                    {
                        departmentIds.Add(_currentUser.CurrentLoginDepartmentId.Value);
                    }
                    departmentIds.Add(curLogin.PomId.Value);
                }
            }

            //过滤特殊字符
            searchRequestDto.ProjectName = Utils.ReplaceSQLChar(searchRequestDto.ProjectName);
            var time = 0;
            if (searchRequestDto.StartTime != null && searchRequestDto.EndTime != null)
            {
                if (searchRequestDto.StartTime == searchRequestDto.EndTime)
                {
                    reportResponseDto.TimeValue = searchRequestDto.StartTime.Value.Year + "年" + searchRequestDto.StartTime.Value.Month + "月" + searchRequestDto.StartTime.Value.Day + "日";
                }
                else
                {
                    reportResponseDto.TimeValue = searchRequestDto.StartTime.Value.Year + "年" + searchRequestDto.StartTime.Value.Month + "月" + searchRequestDto.StartTime.Value.Day + "日--" + searchRequestDto.EndTime.Value.Year + "年" + searchRequestDto.EndTime.Value.Month + "月" + searchRequestDto.EndTime.Value.Day + "日";
                }
            }
            else
            {
                time = DateTime.Now.Date.AddDays(-1).ToDateDay();
                reportResponseDto.TimeValue = DateTime.Now.Date.AddDays(-1).Year + "年" + DateTime.Now.Date.AddDays(-1).Month + "月" + DateTime.Now.Date.AddDays(-1).Day + "日";
            }
            var list = dbContext.Queryable<Project>()
                .InnerJoin<SafeSupervisionDayReport>((x, y) => x.Id == y.ProjectId)
                .OrderBy((x, y) => new { Name = SqlFunc.Desc(x.Name), DateDay = SqlFunc.Desc(y.DateDay) })
                .WhereIF(true, x => x.IsDelete == 1)
                .WhereIF(true, x => departmentIds.Contains(x.ProjectDept.Value))
                .WhereIF(searchRequestDto.StartTime == null && searchRequestDto.EndTime == null, (x, y) => y.DateDay == time)
                .WhereIF(searchRequestDto.StartTime != null && searchRequestDto.EndTime != null, (x, y) => y.DateDay >= searchRequestDto.StartTime.Value.ToDateDay() && y.DateDay <= searchRequestDto.EndTime.Value.ToDateDay())
                .WhereIF(searchRequestDto.CompanyId != null, x => x.CompanyId == searchRequestDto.CompanyId)
                .WhereIF(searchRequestDto.ProjectDept != null, x => x.ProjectDept == searchRequestDto.ProjectDept)
                .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), x => searchRequestDto.ProjectStatusId.Contains(x.StatusId.Value.ToString()))
                .WhereIF(searchRequestDto.ProjectTypeId != null, x => x.TypeId == searchRequestDto.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), x => SqlFunc.Contains(x.Name, searchRequestDto.ProjectName))
                .WhereIF(categoryList != null && categoryList.Any(), x => categoryList.Contains(x.Category))
                .WhereIF(tagList != null && tagList.Any(), x => tagList.Contains(x.Tag))
                .WhereIF(tag2List != null && tag2List.Any(), x => tag2List.Contains(x.Tag2))
                .Select((x, y) => new SafeDayReportInfo
                {
                    Id = y.Id,
                    ProjectId = x.Id,
                    SafeId = y.Id,
                    ProjectName = x.Name,
                    ProjectCompany = x.CompanyId.ToString(),
                    ProjectCategory = x.Category.ToString(),
                    ProjectStatus = x.StatusId.ToString(),
                    ProjectType = x.TypeId.ToString(),
                    Iswork = Convert.ToInt32(y.IsWork).ToString(),
                    Iscompanywork = Convert.ToInt32(y.IsCompanyWork).ToString(),
                    Workstatus = y.WorkStatus.ToString(),
                    Masknum = y.MaskNum,
                    Thermometernum = y.ThermometerNum,
                    Disinfectantnum = y.DisinfectantNum,
                    Measures = y.Measures,
                    Situation = y.Situation.ToString(),
                    Issuperiorsupervision = Convert.ToInt32(y.IsSuperiorSupervision).ToString(),
                    Superiorsupervisioncount = y.SuperiorSupervisionCount,
                    Superiorsupervisionform = y.SuperiorSupervisionForm.ToString(),
                    Superiorsupervisiondate = y.SuperiorSupervisionDate,
                    Supervisionunit = y.SupervisionUnit,
                    Supervisionleader = y.SupervisionLeader,
                    Supervisionother = y.SupervisionOther,
                    Other = y.Other,
                    DateDay = y.DateDay
                });

            //是否全量导出
            var safeList = new List<SafeDayReportInfo>();
            if (!searchRequestDto.IsFullExport)
            {
                safeList = await list.ToPageListAsync(searchRequestDto.PageIndex, searchRequestDto.PageSize, total);
            }
            else
            {
                safeList = await list.ToListAsync();
                total = safeList.Count;
            }
            //获取处理完的数据
            var result = await GetSafeResponseDtosAsync(safeList);
            reportResponseDto.safeDayReportInfos = result;

            responseAjaxResult.Data = reportResponseDto;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 获取安监日报处理过的数据
        /// </summary>
        /// <param name="safeDayReports"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SafeDayReportInfo>> GetSafeResponseDtosAsync(List<SafeDayReportInfo> list)
        {

            //获取项目的Id
            var projectIds = list.Select(x => x.ProjectId).ToList();
            //获取分页后项目的类型id
            var typeIds = list.Select(x => x.ProjectType).ToList();
            //获取分页后项目的状态Id
            var statusIds = list.Select(x => x.ProjectStatus).ToList();
            //获取分页后项目的公司Id
            var companyId = list.Select(x => x.ProjectCompany).ToList();

            //查询项目下的类型与状态
            var typeList = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && typeIds.Contains(x.PomId.ToString())).ToListAsync();
            var statusList = await dbContext.Queryable<ProjectStatus>().Where(x => x.IsDelete == 1 && statusIds.Contains(x.StatusId.ToString())).ToListAsync();

            ////获取安监日报数据
            //var safeList = await dbContext.Queryable<SafeSupervisionDayReport>()
            //	.Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId) && x.DateDay == time).ToListAsync();
            //获取机构数据
            var companyList = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && companyId.Contains(x.PomId.ToString())).ToListAsync();

            foreach (var item in list)
            {
                item.ProjectType = typeList.FirstOrDefault(x => x.PomId.ToString() == item.ProjectType)?.Name;
                item.ProjectStatus = statusList.FirstOrDefault(x => x.StatusId.ToString() == item.ProjectStatus)?.Name;
                item.ProjectCompany = companyList.FirstOrDefault(x => x.PomId.ToString() == item.ProjectCompany)?.Name;
                item.ProjectCategory = item.ProjectCategory == "0" ? "境内" : "境外";
                item.Iswork = item.Iswork == "1" ? "明确" : item.Iswork == "0" ? "不明确" : "";
                item.Iscompanywork = item.Iscompanywork == "1" ? "是" : item.Iscompanywork == "0" ? "否" : "";
                item.Workstatus = item.Workstatus == "1" ? "春节未复工" : item.Workstatus == "2" ? "未开工或停工未复工" : item.Workstatus == "3" ? "已复工" : item.Workstatus == "4" ? "已完工" : "";
                item.Situation = item.Situation == "1" ? "安全" : item.Situation == "2" ? "事故" : "";
                item.Issuperiorsupervision = item.Issuperiorsupervision == "1" ? "是" : item.Issuperiorsupervision == "0" ? "否" : "";
                item.Superiorsupervisionform = item.Superiorsupervisionform == "1" ? "远程督查" : item.Superiorsupervisionform == "2" ? "现场督查" : "";
            }

            return list;
        }
        /// <summary>
        /// 获取当日计划产值
        /// </summary>
        /// <param name="DateDay"></param>
        /// <param name="dayReports"></param>
        /// <returns></returns>
        public decimal SearchOutput(int DateDay, Guid projectId, List<DayReport> dayReports)
        {
            var a = dayReports.Where(x => x.DateDay == DateDay && x.ProjectId == projectId).ToList();
            decimal b = 0;
            foreach (var item in a)
            {
                b += item.DayActualProductionAmount;
            }
            return b;
        }
    }
}
