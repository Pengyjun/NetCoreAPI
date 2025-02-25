using Aspose.Words.Drawing;
using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Common;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using NPOI.SS.Formula.Atp;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using SqlSugar.Extensions;
using System.Text;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.Projects
{
    /// <summary>
    /// 项目业务接口层的实现层
    /// </summary>
    public class ProjectService : IProjectService
    {

        #region 依赖注入
        public IBaseRepository<ProjectWBS> baseProjectWBSRepository { get; set; }
        public IBaseRepository<Project> baseProjects { get; set; }
        public IBaseRepository<ProjectWbsHistoryMonth> baseProjectWbsHistory { get; set; }

        public ISqlSugarClient dbContext { get; set; }
        public IMapper mapper { get; set; }
        public IBaseRepository<Files> baseFilesRepository { get; set; }
        public IBaseRepository<ProjectOrg> baseProjectOrgRepository { get; set; }
        public IBaseRepository<ProjectLeader> baseProjectLeaderRepository { get; set; }
        public IPushPomService _pushPomService { get; set; }
        public IBaseService baseService { get; set; }
        public ILogService logService { get; set; }
        public IEntityChangeService entityChangeService { get; set; }

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 当前用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        //public ActionExecutingContext context { get; set; }
        public ProjectService(IBaseRepository<ProjectWbsHistoryMonth> baseProjectWbsHistory, IBaseRepository<ProjectWBS> baseProjectWBSRepository, ISqlSugarClient dbContext, IMapper mapper, IBaseRepository<Files> baseFilesRepository, IBaseRepository<ProjectOrg> baseProjectOrgRepository, IBaseRepository<ProjectLeader> baseProjectLeaderRepository, IPushPomService pushPomService, IBaseService baseService, ILogService logService, IEntityChangeService entityChangeService, GlobalObject globalObject, IBaseRepository<Project> baseProjects)
        {
            this.baseProjectWBSRepository = baseProjectWBSRepository;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.baseFilesRepository = baseFilesRepository;
            this.baseProjectOrgRepository = baseProjectOrgRepository;
            this.baseProjectLeaderRepository = baseProjectLeaderRepository;
            this._pushPomService = pushPomService;
            this.baseService = baseService;
            this.logService = logService;
            this.entityChangeService = entityChangeService;
            this._globalObject = globalObject;
            this.baseProjectWbsHistory = baseProjectWbsHistory;
            this.baseProjects = baseProjects;
            //this.context = context;
        }
        #endregion

        /// <summary>
        /// 分页获取项目列表
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectResponseDto>>> SearchProjectAsync(ProjectSearchRequestDto searchRequestDto)
        {
            ResponseAjaxResult<List<ProjectResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProjectResponseDto>>();
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
            // var departList = await baseService.SearchCompanySubPullDownAsync(_currentUser.CurrentLoginInstitutionId);
            List<Guid> departmentIds = new List<Guid>();
            //if (departList.Data != null && departList.Data.Any())
            //{
            //获取登陆用户角色信息
            //var uRoles = _currentUser.RoleInfos.Select(x => x.Type).ToList();
            //获取所有机构
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            //if (uRoles.Contains(1) || uRoles.Contains(2))//管理员 或 广航总部管理员
            //{
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
            //}
            //else departmentIds.Add(_currentUser.CurrentLoginDepartmentId.Value);
            //}
            //过滤特殊字符
            searchRequestDto.ProjectName = Utils.ReplaceSQLChar(searchRequestDto.ProjectName);

            departmentIds.Clear();
            var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            var a = departmentIds.Where(x => x == "e187aa5b-003d-4598-ad86-14fb7e69a53b".ToGuid()).ToList();
            var project = dbContext.Queryable<Project>()
                .LeftJoin<ProjectStatus>((p, ps) => p.StatusId == ps.StatusId)
                .Where(p => departmentIds.Contains(p.ProjectDept.Value))
                .Where(p => p.StatusId != "0c686c96-889e-4c4d-b24d-fa2886d9dceb".ToGuid())
                .WhereIF(true, p => p.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ManagerType), p => p.ManagerType == searchRequestDto.ManagerType)
                .WhereIF(searchRequestDto.CompanyId != null, p => p.CompanyId == searchRequestDto.CompanyId)
                .WhereIF(searchRequestDto.ProjectDept != null, p => p.ProjectDept == searchRequestDto.ProjectDept)
                .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), p => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                .WhereIF(searchRequestDto.ProjectRegionId != null, p => p.RegionId == searchRequestDto.ProjectRegionId)
                .WhereIF(searchRequestDto.ProjectTypeId != null, p => p.TypeId == searchRequestDto.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), p => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                .WhereIF(searchRequestDto.ProjectAreaId! != null, p => p.AreaId == searchRequestDto.ProjectAreaId)
                .WhereIF(categoryList != null && categoryList.Any(), p => categoryList.Contains(p.Category))
                .WhereIF(tagList != null && tagList.Any(), p => tagList.Contains(p.Tag))
                .WhereIF(tag2List != null && tag2List.Any(), p => tag2List.Contains(p.Tag2))

                .Select((p, ps) => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortName = p.ShortName,
                    Code = p.Code,
                    AreaName = p.AreaId.ToString(),
                    Category = p.Category,
                    TypeId = p.TypeId,
                    Amount = p.Amount,
                    CurrencyId = p.CurrencyId,
                    CompanyId = p.CompanyId,
                    ProjectDept = p.ProjectDept,
                    StatusId = ps.StatusId,
                    Status = ps.Name,
                    Tag = p.Tag,
                    Tag2 = p.Tag2,
                    CreateTime = p.CreateTime,
                    Sequence = ps.Sequence.Value,
                    CommencementTime = p.CommencementTime,
                    CompletionTime = p.CompletionTime,
                    StartContractDuration = p.StartContractDuration,
                    EndContractDuration = p.EndContractDuration,
                    Rate = p.Rate * 100,
                    ExchangeRate = p.ExchangeRate
                });

            //是否全量导出
            var projectList = new List<ProjectResponseDto>();
            if (!searchRequestDto.IsFullExport)
            {
                projectList = await project.OrderBy("ps.Sequence asc,p.CreateTime desc ").ToPageListAsync(searchRequestDto.PageIndex, searchRequestDto.PageSize, total);
            }
            else
            {
                projectList = await project.OrderBy("ps.Sequence asc,p.CreateTime desc ").ToListAsync();
                total = projectList.Count;
            }

            //获取项目Id 
            var projectIds = projectList.Select(x => x.Id).ToList();
            //获取项目状态Id
            var projectStatusIds = projectList.Select(x => x.StatusId).ToList();
            ///获取项目类型Id
            var projectTypeIds = projectList.Select(x => x.TypeId).ToList();
            //获取项目币种Id
            var projectCurrencyIds = projectList.Select(x => x.CurrencyId).ToList();
            //获取项目公司Id
            var projectCompanyIds = projectList.Select(x => x.CompanyId).ToList();
            //获取币种汇率
            var rate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.Year == DateTime.Now.Year).ToListAsync();
            //获取省份
            var provinceList = await dbContext.Queryable<Province>().Where(x => x.IsDelete == 1).ToListAsync();
            //获取项目状态
            var projectStatusList = await dbContext.Queryable<ProjectStatus>().Where(x => x.IsDelete == 1 && projectStatusIds.Contains(x.StatusId)).ToListAsync();
            //获取项目类型
            var projectTypeList = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && projectTypeIds.Contains(x.PomId)).ToListAsync();
            //获取项目币种
            var projectCurrencyList = await dbContext.Queryable<Currency>().Where(x => x.IsDelete == 1 && projectCurrencyIds.Contains(x.PomId)).ToListAsync();
            //获取项目所属公司
            var projectCompanyList = institution.Where(x => x.IsDelete == 1 && projectCompanyIds.Contains(x.PomId)).ToList();
            //获取日报信息
            var dayReportList = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId) && x.DateDay == DateTime.Now.ToDateDay() && x.ProcessStatus == DayReportProcessStatus.Submited).ToListAsync();

            foreach (var item in projectList)
            {
                //item.Status = projectStatusList.FirstOrDefault(x => x.StatusId == item.StatusId)?.Name;
                item.TypeName = projectTypeList.FirstOrDefault(x => x.PomId == item.TypeId)?.Name;
                item.ZCURRENCYNAME = searchRequestDto.IsConvert == true ? "CNY-人民币" : projectCurrencyList.FirstOrDefault(x => x.PomId == item.CurrencyId)?.Zcurrencyname;
                if (item.ExchangeRate == null || string.IsNullOrWhiteSpace(item.ExchangeRate.ToString()))
                {
                    var exrate = rate.Where(x => x.CurrencyId == item.CurrencyId.ToString()).FirstOrDefault();
                    if (exrate != null)
                    {
                        item.ExchangeRate = exrate.ExchangeRate;
                    }
                }
                var exrateReplace = rate.Where(x => x.CurrencyId == item.CurrencyId.ToString()).FirstOrDefault();
                if (exrateReplace != null)
                {
                    item.ExchangeRate = exrateReplace.ExchangeRate;
                }
                item.CompanyName = projectCompanyList.FirstOrDefault(x => x.PomId == item.CompanyId)?.Name;
                item.AreaName = item.AreaName == null ? null : await baseService.GetProvincemarket(provinceList, item.AreaName.ToGuid());
                item.Amount = searchRequestDto.IsConvert == true ?
                    item.CurrencyId.ToString() == "edea1eb4-936f-465a-8ffa-1d1669bc3776" ?
                    Math.Round(Convert.ToDecimal(item.Amount * rate.Single(x => x.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776").ExchangeRate), 2)
                    : item.CurrencyId.ToString() == "43e52b6c-f41f-48c8-822c-103732f81cc1" ?
                    Math.Round(Convert.ToDecimal(item.Amount * rate.Single(x => x.CurrencyId == "43e52b6c-f41f-48c8-822c-103732f81cc1").ExchangeRate), 2)
                    : item.Amount : item.Amount;
                item.IsAddOrUpdate = dayReportList.Where(x => projectIds.Contains(x.ProjectId)).Any() == true ? true : false;
            }

            responseAjaxResult.Data = projectList;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 币种切换判断
        /// </summary>
        /// <returns></returns>
        public decimal GetAmount(bool IsConvert, Guid? currencyId, decimal? amount, decimal? exchangeRate)
        {
            //获取币种汇率
            var rate = dbContext.Queryable<CurrencyConverter>().Where(x => x.Year == DateTime.Now.Year).ToList();
            if (IsConvert)
            {
                if (string.IsNullOrWhiteSpace(exchangeRate.ToString()))
                {
                    if (currencyId.ToString() == "edea1eb4-936f-465a-8ffa-1d1669bc3776")
                    {
                        amount = Math.Round(Convert.ToDecimal(amount * rate.Single(x => x.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776").ExchangeRate), 2);
                    }
                    else if (currencyId.ToString() == "43e52b6c-f41f-48c8-822c-103732f81cc1")
                    {
                        amount = Math.Round(Convert.ToDecimal(amount * rate.Single(x => x.CurrencyId == "43e52b6c-f41f-48c8-822c-103732f81cc1").ExchangeRate), 2);
                    }
                }
            }
            return amount.Value;
        }

        /// <summary>
        /// 获取项目列表导出缺失字段数据
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        public async Task<List<ProjectResponseDto>> GetAllSearchProjectAsync(ProjectSearchRequestDto searchRequestDto)
        {
            //获取项目数据列表
            var dataResult = await SearchProjectAsync(searchRequestDto);
            //获取项目主键ids
            var proIds = dataResult.Data.Select(x => x.Id).ToList();
            var s = DateTime.Now;
            var mergeTableData = await dbContext.Queryable<Project>()
                .LeftJoin<WaterCarriage>((p, wc) => p.ConditionGradeId == wc.PomId.ToString())//水运工况
                .LeftJoin<ProjectScale>((p, wc, ps) => p.GradeId == ps.PomId)//项目规模
                .LeftJoin<Province>((p, wc, ps, pro) => p.AreaId == pro.PomId)//省份市区地区乡镇
                .LeftJoin<ConstructionQualification>((p, wc, ps, pro, cq) => p.ProjectConstructionQualificationId == cq.PomId)//施工资质
                                                                                                                              //.LeftJoin<Model.User>((p, wc, ps, pro, cq, uu) => p.ReportFormer == uu.PomId.ToString())//报表负责人
                .LeftJoin<ProjectArea>((p, wc, ps, pro, cq, pa) => p.RegionId == pa.AreaId)//所属区域
                .Select((p, wc, ps, pro, cq, pa) => new ProjectResponseDto
                {
                    Id = p.Id,
                    ConditiongradeName = wc.Remarks,
                    GradeName = ps.Name,
                    Category = p.Category,
                    AreaName = pro.Zaddvsname,
                    RegionName = pa.Name,
                    Ecamount = p.ECAmount,
                    ContractmeapayProp = p.ContractMeaPayProp,
                    Rate = p.Rate,
                    ProjectConstructionQualification = cq.Name,
                    ProjectDeptAddress = p.ProjectDeptAddress,
                    Quantity = p.Quantity,
                    QuantityRemarks = p.QuantityRemarks,
                    Completiondate = p.CompletionDate,
                    Longitude = p.Longitude,
                    Latitude = p.Latitude,
                    IsStrength = p.IsStrength == true ? "是" : "否",
                    CompleteQuantity = p.CompleteQuantity,
                    CompleteOutput = p.CompleteOutput,
                    BudgetInterestRate = p.BudgetInterestRate,
                    CompilationTime = p.CompilationTime.ToString(),
                    BudgetaryReasons = p.BudgetaryReasons,
                    SocietySpeceffect = p.SocietySpecEffect == false ? "否" : "是",
                    ReclamationArea = p.ReclamationArea,
                    Administrator = p.Administrator,
                    Constructor = p.Constructor,
                    Remarks = p.Remarks,
                    ClassifyStandard = p.ClassifyStandard,
                    Reportformer = p.ReportFormer,
                    ReportformerTel = p.ReportForMertel
                })
                .MergeTable()
                .Where(x => proIds.Contains(x.Id))
                .ToListAsync();
            //反转获取第一个行业分类标准 的所有项目
            foreach (var item in mergeTableData)
            {
                if (!string.IsNullOrWhiteSpace(item.ClassifyStandard))
                {
                    item.ClassifyStandard = item.ClassifyStandard.Split(',').ToArray().Reverse().First();
                }
            }
            //获取行业分类ids
            var icIds = mergeTableData.Select(x => x.ClassifyStandard).ToList();
            var icData = dbContext.Queryable<IndustryClassification>().Where(x => icIds.Contains(x.PomId.ToString())).ToList();
            //循环取出名字
            foreach (var item in mergeTableData)
            {
                item.ClassifyStandard = icData.Where(x => x.PomId.ToString() == item.ClassifyStandard).SingleOrDefault()?.Name;
            }
            var ssss = DateTime.Now;
            return mergeTableData;

        }

        /// <summary>
        /// 获取项目列表导出数据
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectExcelSearchsResponseDto>> GetProjectExcelAsync(ProjectSearchRequestDto searchRequestDto)
        {
            ResponseAjaxResult<ProjectExcelSearchsResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectExcelSearchsResponseDto>();
            ProjectExcelSearchsResponseDto responseDto = new ProjectExcelSearchsResponseDto();
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
            //if (departList.Data != null && departList.Data.Any())
            //{
            //获取登陆用户角色信息
            //var uRoles = _currentUser.RoleInfos.Select(x => x.Type).ToList();
            //获取所有机构
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            //if (uRoles.Contains(1) || uRoles.Contains(2))//管理员 或 广航总部管理员
            //{
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

            var project = dbContext.Queryable<Project>()
                .LeftJoin<ProjectStatus>((p, ps) => p.StatusId == ps.StatusId)
                .Where(p => departmentIds.Contains(p.ProjectDept.Value))
                .WhereIF(true, p => p.IsDelete == 1)
                .WhereIF(searchRequestDto.CompanyId != null, p => p.CompanyId == searchRequestDto.CompanyId)
                .WhereIF(searchRequestDto.ProjectDept != null, p => p.ProjectDept == searchRequestDto.ProjectDept)
                .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), p => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                .WhereIF(searchRequestDto.ProjectRegionId != null, p => p.RegionId == searchRequestDto.ProjectRegionId)
                .WhereIF(searchRequestDto.ProjectTypeId != null, p => p.TypeId == searchRequestDto.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), p => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                .WhereIF(searchRequestDto.ProjectAreaId! != null, p => p.AreaId == searchRequestDto.ProjectAreaId)
                .WhereIF(categoryList != null && categoryList.Any(), p => categoryList.Contains(p.Category))
                .WhereIF(tagList != null && tagList.Any(), p => tagList.Contains(p.Tag))
                .WhereIF(tag2List != null && tag2List.Any(), p => tag2List.Contains(p.Tag2))
                .Select((p, ps) => new ProjectExcelSearchResponseDto
                {
                    Id = p.Id,
                    ManagerType = p.ManagerType,
                    MasterProjectId = p.MasterProjectId.Value,
                    Name = p.Name,
                    Category = p.Category,
                    TypeName = p.TypeId.ToString(),
                    Amount = p.Amount,
                    CurrencyId = p.CurrencyId.ToString(),
                    ZCURRENCYNAME = p.CurrencyId.ToString(),
                    CompanyName = p.CompanyId.ToString(),
                    Status = p.StatusId.ToString(),
                    ConditiongradeName = p.ConditionGradeId,
                    GradeName = p.GradeId.ToString(),
                    AreaName = p.AreaId.ToString(),
                    RegionName = p.RegionId.ToString(),
                    Ecamount = p.ECAmount,
                    ContractmeapayProp = p.ContractMeaPayProp,
                    Rate = p.Rate,
                    ProjectConstructionQualification = p.ProjectConstructionQualificationId.ToString(),
                    ProjectDeptAddress = p.ProjectDeptAddress,
                    Quantity = p.Quantity,
                    QuantityRemarks = p.QuantityRemarks,
                    Longitude = p.Longitude,
                    Latitude = p.Latitude,
                    IsStrength = p.IsStrength == true ? "是" : "否",
                    CompleteQuantity = p.CompleteQuantity,
                    CompleteOutput = p.CompleteOutput,
                    BudgetInterestRate = p.BudgetInterestRate,
                    CompilationTime = p.CompilationTime.ToString(),
                    BudgetaryReasons = p.BudgetaryReasons,
                    ClassifyStandardId = p.ClassifyStandard,
                    SocietySpeceffect = p.SocietySpecEffect == true ? "是" : "否",
                    ReclamationArea = p.ReclamationArea,
                    Administrator = p.Administrator,
                    Constructor = p.Constructor,
                    Remarks = p.Remarks,
                    Reportformer = p.ReportFormer,
                    ReportformerTel = p.ReportForMertel,
                    CreateTime = p.CreateTime,
                    Sequence = ps.Sequence.Value,
                    CommencementTime = p.CommencementTime.Value.ToString("yyyy-MM-dd"),
                    CompletionTime = p.CompletionTime,
                    StartContractDuration = p.StartContractDuration,
                    EndContractDuration = p.EndContractDuration,
                    ProjectLocation = p.ProjectLocation,
                    BidWinningDate = p.BidWinningDate.Value.ToString("yyyy-MM-dd"),
                    ShutdownDate = p.ShutdownDate.Value.ToString("yyyy-MM-dd"),
                    DurationInformation = p.DurationInformation,
                    ShutDownReason = p.ShutDownReason,
                    ContractChangeInfo = p.ContractChangeInfo,
                    MasterCode = p.MasterCode,
                    ProjectDeptId = p.ProjectDept.Value,
                });





            //是否全量导出
            var projectList = new List<ProjectExcelSearchResponseDto>();
            if (!searchRequestDto.IsFullExport)
            {
                projectList = await project.OrderBy("ps.Sequence asc,p.CreateTime desc ").ToPageListAsync(searchRequestDto.PageIndex, searchRequestDto.PageSize, total);
            }
            else
            {
                projectList = await project.OrderBy("ps.Sequence asc,p.CreateTime desc ").ToListAsync();
                total = projectList.Count;
            }
            var managerType = await dbContext.Queryable<ProjectMangerType>().Where(x => x.IsDelete == 1).ToListAsync();
            foreach (var item in projectList)
            {
                var ids = item.ManagerType.Split(",").ToList();
                var res = managerType.Where(x => ids.Contains(x.Code)).Select(x => x.Name).ToList();
                item.ManagerTypeName = string.Join(",", res);
                item.ProjectDept = institution.Where(x => x.PomId == item.ProjectDeptId).Select(x => x.Name).FirstOrDefault();
            }
            #region 获取所有类型的id
            //获取项目Id 
            var projectIds = projectList.Select(x => x.Id).ToList();
            //获取项目状态Id
            var projectStatusIds = projectList.Select(x => x.Status).ToList();
            //获取项目类型Id
            var projectTypeIds = projectList.Select(x => x.TypeName).ToList();
            //获取项目币种Id
            var projectCurrencyIds = projectList.Select(x => x.ZCURRENCYNAME).ToList();
            //获取项目公司Id
            var projectCompanyIds = projectList.Select(x => x.CompanyName).ToList();
            //获取工况级数Id
            var conditiongradeIds = projectList.Select(x => x.ConditiongradeName).ToList();
            //获取工况规模Id
            var gradeIds = projectList.Select(x => x.GradeName).ToList();
            //获取施工地点Id
            var areaIds = projectList.Select(x => x.AreaName).ToList();
            //获取施工区域Id
            var regionIds = projectList.Select(x => x.RegionName).ToList();
            //获取项目施工资质
            var projectConstructionQualificationIds = projectList.Select(x => x.ProjectConstructionQualification).ToList();
            //获取行业分类标准
            var classifyStandardIds = projectList.Select(x => x.ClassifyStandardId).ToList();
            #endregion

            #region 获取相关数据集
            //获取币种汇率
            var rate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.Year == DateTime.Now.Year).ToListAsync();
            //获取项目状态
            var projectStatusList = await dbContext.Queryable<ProjectStatus>().Where(x => x.IsDelete == 1 && projectStatusIds.Contains(x.StatusId.ToString())).ToListAsync();
            //获取项目类型
            var projectTypeList = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && projectTypeIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取项目币种
            var projectCurrencyList = await dbContext.Queryable<Currency>().Where(x => x.IsDelete == 1 && projectCurrencyIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取项目所属公司
            var projectCompanyList = institution.Where(x => x.IsDelete == 1 && projectCompanyIds.Contains(x.PomId.ToString())).ToList();
            //获取工况级数
            var conditiongradeList = await dbContext.Queryable<WaterCarriage>().Where(x => x.IsDelete == 1 && conditiongradeIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取工况规模
            var gradeList = await dbContext.Queryable<ProjectScale>().Where(x => x.IsDelete == 1 && gradeIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取施工地点
            var areaList = await dbContext.Queryable<Province>().Where(x => x.IsDelete == 1 && areaIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取省份
            var provinceList = await dbContext.Queryable<Province>().Where(x => x.IsDelete == 1).ToListAsync();
            //获取施工区域
            var regionList = await dbContext.Queryable<ProjectArea>().Where(x => x.IsDelete == 1 && regionIds.Contains(x.AreaId.ToString())).ToListAsync();
            //获取项目施工资质
            var projectConstructionQualificationList = await dbContext.Queryable<ConstructionQualification>().Where(x => x.IsDelete == 1 && projectConstructionQualificationIds.Contains(x.PomId.ToString())).ToListAsync();
            //获取行业分类标准
            var classifyStandardList = await dbContext.Queryable<IndustryClassification>().Where(x => x.IsDelete == 1).ToListAsync();
            #endregion

            foreach (var item in projectList)
            {
                item.CategoryName = item.Category == 0 ? "境内" : "境外";
                item.Sequence = Convert.ToInt32(projectStatusList.FirstOrDefault(x => x.StatusId.ToString() == item.Status).Sequence);
                item.Status = projectStatusList.FirstOrDefault(x => x.StatusId.ToString() == item.Status)?.Name;
                item.TypeName = projectTypeList.FirstOrDefault(x => x.PomId.ToString() == item.TypeName)?.Name;
                item.ZCURRENCYNAME = searchRequestDto.IsConvert == true ? "CNY-人民币" : projectCurrencyList.FirstOrDefault(x => x.PomId.ToString() == item.ZCURRENCYNAME)?.Zcurrencyname;
                item.CompanyName = projectCompanyList.FirstOrDefault(x => x.PomId.ToString() == item.CompanyName)?.Name;
                item.Ecamount = searchRequestDto.IsConvert == true ?
                    item.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776" ?
                    Math.Round(Convert.ToDecimal(item.Ecamount * rate.Single(x => x.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776").ExchangeRate), 2)
                    : item.CurrencyId == "43e52b6c-f41f-48c8-822c-103732f81cc1" ?
                    Math.Round(Convert.ToDecimal(item.Ecamount * rate.Single(x => x.CurrencyId == "43e52b6c-f41f-48c8-822c-103732f81cc1").ExchangeRate), 2)
                    : item.Ecamount : item.Ecamount;
                item.Amount = searchRequestDto.IsConvert == true ?
                    item.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776" ?
                    Math.Round(Convert.ToDecimal(item.Amount * rate.Single(x => x.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776").ExchangeRate), 2)
                    : item.CurrencyId == "43e52b6c-f41f-48c8-822c-103732f81cc1" ?
                    Math.Round(Convert.ToDecimal(item.Amount * rate.Single(x => x.CurrencyId == "43e52b6c-f41f-48c8-822c-103732f81cc1").ExchangeRate), 2)
                    : item.Amount : item.Amount;
                item.ConditiongradeName = conditiongradeList.FirstOrDefault(x => x.PomId.ToString() == item.ConditiongradeName)?.Remarks;
                item.GradeName = gradeList.FirstOrDefault(x => x.PomId.ToString() == item.GradeName)?.Name;
                item.ProvinceName = await baseService.GetProvince(provinceList, item.AreaName.ToGuid());
                item.AreaName = await baseService.GetProvincemarket(provinceList, item.AreaName.ToGuid());
                //item.AreaName = areaList.FirstOrDefault(x => x.PomId.ToString() == item.AreaName)?.Zaddvsname;
                item.RegionName = regionList.FirstOrDefault(x => x.AreaId.ToString() == item.RegionName)?.Name;
                item.ProjectConstructionQualification = projectConstructionQualificationList.FirstOrDefault(x => x.PomId.ToString() == item.ProjectConstructionQualification)?.Name;
                var classifyArray = item.ClassifyStandardId.Split(',');
                foreach (var item2 in classifyArray)
                {
                    if (!string.IsNullOrWhiteSpace(classifyStandardList.FirstOrDefault(x => x.PomId.ToString() == item2)?.Name))
                    {
                        item.ClassifyStandard += classifyStandardList.FirstOrDefault(x => x.PomId.ToString() == item2)?.Name + "/";
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.ClassifyStandard))
                {
                    item.ClassifyStandard = item.ClassifyStandard.Substring(0, item.ClassifyStandard.Length - 1);
                }
            }

            #region 新增逻辑
            if (searchRequestDto.IsFullExport)
            {
                List<int> types = new List<int>();
                types.Add(1);
                types.Add(6);
                types.Add(4);
                List<int> users = new List<int>();
                types.Add(1);
                types.Add(3);
                types.Add(4);
                types.Add(5);
                types.Add(8);
                users.Add(1);
                users.Add(3);
                users.Add(4);
                users.Add(5);
                users.Add(8);
                var ids = projectList.Select(x => x.Id).ToList();
                var idss = projectList.Where(x => x.MasterProjectId.HasValue == true).Select(x => x.MasterProjectId.Value).ToList();
                ids.AddRange(idss);
                ids = ids.Distinct().ToList();
                //获取项目干系单位
                var orgList = await dbContext.Queryable<ProjectOrg>()
                    .Where(p => p.IsDelete == 1 && ids.Contains(p.ProjectId.Value)).ToListAsync();
                //查询项目干系单位名称
                var orgTypeName = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && (x.TypeNo == 2
                || x.TypeNo == 1) && types.Contains(x.Type)).ToListAsync();
                //查询往来单位名称
                var unitName = await dbContext.Queryable<DealingUnit>()
                    .Where(x => x.IsDelete == 1).ToListAsync();

                var dutyList = await dbContext.Queryable<ProjectLeader>()
                .Where(pl => pl.IsDelete == 1 && ids.Contains(pl.ProjectId.Value))
                .ToListAsync();
                var userList = await dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.User>()
                    .Where(x => x.IsDelete == 1).ToListAsync();
                foreach (var item in projectList)
                {
                    #region 干系单位类型
                    var typeList = orgTypeName.Where(x => x.TypeNo == 2).Select(x => x.Type.ToString()).ToList();
                    var orgs = orgList.Where(x => x.ProjectId == item.Id || x.ProjectId == item.MasterProjectId && typeList.Contains(x.Type)).ToList();
                    var oid = orgs.FirstOrDefault(x => x.Type == "1")?.OrganizationId;
                    if (oid.HasValue)
                    {
                        item.YeZhuUnitName = unitName.FirstOrDefault(x => x.PomId == oid)?.ZBPNAME_ZH;
                    }
                    oid = orgs.FirstOrDefault(x => x.Type == "6")?.OrganizationId;
                    if (oid.HasValue)
                    {
                        item.JianLiUnitName = unitName.SingleOrDefault(x => x.PomId == oid)?.ZBPNAME_ZH;
                    }
                    oid = orgs.FirstOrDefault(x => x.Type == "4")?.OrganizationId;
                    if (oid.HasValue)
                    {
                        item.SheJiUnitName = unitName.SingleOrDefault(x => x.PomId == oid)?.ZBPNAME_ZH;
                    }
                    #endregion

                    #region 人员职位类型
                    var typeUserList = orgTypeName.Where(x => x.TypeNo == 1 && users.Contains(x.Type)).Select(x => x.Type.ToString()).ToList();
                    var userTypeList = dutyList.Where(x => x.ProjectId == item.Id || x.ProjectId == item.MasterProjectId && users.Contains(x.Type)).ToList();
                    var assistantManagerId = userTypeList.FirstOrDefault(x => x.Type == 1)?.AssistantManagerId;
                    if (assistantManagerId != null)
                        item.XmJlName = userList.FirstOrDefault(x => x.PomId == assistantManagerId)?.Name;
                    assistantManagerId = userTypeList.FirstOrDefault(x => x.Type == 3)?.AssistantManagerId;
                    if (assistantManagerId != null)
                        item.XmSjName = userList.FirstOrDefault(x => x.PomId == assistantManagerId)?.Name;
                    assistantManagerId = userTypeList.FirstOrDefault(x => x.Type == 4)?.AssistantManagerId;
                    if (assistantManagerId != null)
                        item.XmZgName = userList.FirstOrDefault(x => x.PomId == assistantManagerId)?.Name;
                    assistantManagerId = userTypeList.FirstOrDefault(x => x.Type == 5)?.AssistantManagerId;
                    if (assistantManagerId != null)
                        item.XmZjName = userList.FirstOrDefault(x => x.PomId == assistantManagerId)?.Name;
                    assistantManagerId = userTypeList.FirstOrDefault(x => x.Type == 8)?.AssistantManagerId;
                    if (assistantManagerId != null)
                        item.XmZyFerName = userList.FirstOrDefault(x => x.PomId == assistantManagerId)?.Name;
                    #endregion
                }
            }


            #endregion
            //排序
            projectList = projectList.OrderBy(x => x.Sequence).ThenByDescending(x => x.CreateTime).ToList();

            responseDto.projectExcelData = projectList;

            responseAjaxResult.Data = responseDto;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<ProjectDetailResponseDto>> SearchProjectDetailAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            ResponseAjaxResult<ProjectDetailResponseDto> responseAjaxResult = new();
            var projectDeteilSingle = await dbContext.Queryable<Project>()
                 .Where((p) => p.Id == basePrimaryRequestDto.Id && p.IsDelete == 1)
                 .Select((p) => new ProjectDetailResponseDto
                 {
                     ManagerType = p.ManagerType,
                     PProjectName = SqlFunc.Subqueryable<Project>().Where(x => p.PProjectMasterCode == x.MasterCode).Select(x => x.Name),
                     PProjectMasterCode = p.PProjectMasterCode,
                     IsSubContractProject = p.IsSubContractProject,
                     Id = p.Id,
                     Name = p.Name,
                     MasterCode = p.MasterCode,
                     Code = p.Code,
                     ShortName = p.ShortName,
                     AreaName = p.AreaId.ToString(),
                     CompanyId = p.CompanyId,
                     ProjectDept = p.ProjectDept,
                     ProjectDeptAddress = p.ProjectDeptAddress,
                     AreaId = p.AreaId,
                     RegionId = p.RegionId,
                     Category = p.Category,
                     TypeId = p.TypeId,
                     ClassifyStandard = p.ClassifyStandard,
                     CompilationTime = p.CompilationTime,
                     Rate = p.Rate * 100,
                     ExchangeRate = p.ExchangeRate,
                     Amount = p.Amount,
                     ECAmount = p.ECAmount,
                     ContractMeaPayProp = p.ContractMeaPayProp,
                     Longitude = p.Longitude,
                     Latitude = p.Latitude,
                     ProjectConstructionQualificationId = p.ProjectConstructionQualificationId,
                     CurrencyId = p.CurrencyId,
                     StatusId = p.StatusId,
                     GradeId = p.GradeId,
                     IsStrength = p.BudgetInterestRate > 0 ? true : false,//p.IsStrength,
                     Tag = p.Tag,
                     Tag2 = p.Tag2,
                     BudgetInterestRate = p.BudgetInterestRate * 100,
                     BudgetaryReasons = p.BudgetaryReasons,
                     ReclamationArea = p.ReclamationArea,
                     ConditionGradeId = p.ConditionGradeId,
                     Administrator = p.Administrator,
                     ConstructorNum = p.Constructor,
                     ReportFormer = p.ReportFormer,
                     ReportForMertel = p.ReportForMertel,
                     SocietySpecEffect = p.SocietySpecEffect,
                     QuantityRemarks = p.QuantityRemarks,
                     MasterProjectId = p.MasterProjectId,
                     CommencementTime = p.CommencementTime,
                     CompletionTime = p.CompletionTime,
                     StartContractDuration = p.StartContractDuration,
                     EndContractDuration = p.EndContractDuration,
                     ProjectLocation = p.ProjectLocation,
                     BidWinningDate = p.BidWinningDate,
                     ShutdownDate = p.ShutdownDate,
                     DurationInformation = p.DurationInformation,
                     ShutDownReason = p.ShutDownReason,
                     ContractChangeInfo = p.ContractChangeInfo,
                     WorkDay = p.WorkDay,
                     ContractSignDate = p.ContractSignDate,
                     ContractStipulationEndDate = p.ContractStipulationEndDate,
                     ContractStipulationStartDate = p.ContractStipulationStartDate
                 }).SingleAsync();



            if (projectDeteilSingle == null)
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                return responseAjaxResult;
            }

            //项目管理类型
            var managerTypeName = await dbContext.Queryable<ProjectMangerType>().Where(x => x.IsDelete == 1 && x.Code == SqlFunc.ToString(projectDeteilSingle.ManagerType)).Select(x => x.Name).FirstAsync();
            projectDeteilSingle.ManagerTypeName = managerTypeName;
            //获取机构信息
            var intitutionList = dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1);
            projectDeteilSingle.CompanyName = intitutionList.SingleAsync(x => x.PomId == projectDeteilSingle.CompanyId).Result.Name;

            projectDeteilSingle.ProjectDeptName = intitutionList.Where(x => x.PomId == projectDeteilSingle.ProjectDept).Select(x => x.Shortname).Single();
            if (projectDeteilSingle.ProjectDeptName == null)
            {
                projectDeteilSingle.ProjectDeptName = await dbContext.Queryable<ProjectDepartment>().Where(x => x.Name == "临时补录项目部" && x.PomId == projectDeteilSingle.ProjectDept).Select(x => x.Name).SingleAsync();
            }
            //获取省份
            var provinceList = await dbContext.Queryable<Province>().Where(x => x.IsDelete == 1).ToListAsync();
            //获取地点名称
            projectDeteilSingle.AreaName = await baseService.GetProvincemarket(provinceList, projectDeteilSingle.AreaName.ToGuid());
            //获取区域名称
            projectDeteilSingle.RegionName = await dbContext.Queryable<ProjectArea>().Where(x => x.IsDelete == 1 && x.AreaId == projectDeteilSingle.RegionId).Select(x => x.Name).SingleAsync();
            //类型
            projectDeteilSingle.TypeName = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && x.PomId == projectDeteilSingle.TypeId).Select(x => x.Name).SingleAsync();
            //币种
            projectDeteilSingle.CurrencyName = await dbContext.Queryable<Currency>().Where(x => x.IsDelete == 1 && x.PomId == projectDeteilSingle.CurrencyId).Select(x => x.Zcurrencyname).SingleAsync();
            //获取币种汇率
            projectDeteilSingle.ExchangeRate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.IsDelete == 1 && x.CurrencyId == projectDeteilSingle.CurrencyId.ToString() && x.Year == DateTime.Now.Year).Select(x => x.ExchangeRate).SingleAsync();
            //项目状态
            projectDeteilSingle.StatusName = await dbContext.Queryable<ProjectStatus>().Where(x => x.IsDelete == 1 && x.StatusId == projectDeteilSingle.StatusId).Select(x => x.Name).SingleAsync();
            //项目规模
            projectDeteilSingle.GradeName = await dbContext.Queryable<ProjectScale>().Where(x => x.IsDelete == 1 && x.PomId == projectDeteilSingle.GradeId).Select(x => x.Name).SingleAsync();
            //境内外
            projectDeteilSingle.CategoryName = projectDeteilSingle.Category == 0 ? "境内" : "境外";
            //是否完成编制
            projectDeteilSingle.IsStrengthName = projectDeteilSingle.IsStrength == true ? "是" : "否";
            //传统新兴标签
            projectDeteilSingle.TagName = projectDeteilSingle.Tag == 0 ? "传统" : "新兴";
            //现汇投资标签
            projectDeteilSingle.Tag2Name = projectDeteilSingle.Tag2 == 0 ? "现汇" : "投资";
            //是否具有特殊社会效应
            projectDeteilSingle.SocietySpecEffectName = projectDeteilSingle.SocietySpecEffect == true ? "是" : "否";
            //判断行业分类标准是否存在
            var classList = await dbContext.Queryable<IndustryClassification>()
                .Select(x => new { x.PomId, x.ParentId, x.Name }).ToListAsync();
            //行业分类标准分割
            if (projectDeteilSingle.ClassifyStandard != null)
            {
                var classifyArrays = projectDeteilSingle.ClassifyStandard.Split(',');
                projectDeteilSingle.ClassifyArray = classifyArrays;
                #region 暂时不使用的逻辑
                //for (int i = 0; i < projectDeteilSingle.ClassifyArray.Length; i++)
                //{
                //    if (!classList.Any(x => x.PomId.ToString() == projectDeteilSingle.ClassifyArray[i] || x.ParentId.ToString() == projectDeteilSingle.ClassifyArray[i]))
                //    {
                //        projectDeteilSingle.ClassifyArray = new string[] { };
                //    }
                //    else
                //    {
                //        projectDeteilSingle.ClassifyArrayName += classList.Where(x => x.PomId.ToString() == projectDeteilSingle.ClassifyArray[i]).FirstOrDefault().Name + "/";
                //    }
                //}
                //if (projectDeteilSingle.ClassifyArray.Length == 0)
                //{
                //    projectDeteilSingle.ClassifyArrayName = "";
                //}
                //else
                //{
                //    projectDeteilSingle.ClassifyArrayName = projectDeteilSingle.ClassifyArrayName.Substring(0, projectDeteilSingle.ClassifyArrayName.Length - 2);
                //}
                #endregion

                var classIds = classifyArrays.Select(x => x.ToGuid()).ToList();
                projectDeteilSingle.ClassifyStandard = string.Join(',', classIds);
                for (int i = 0; i < classIds.Count; i++)
                {
                    projectDeteilSingle.ClassifyArray[i] = classIds[i].ToString();
                }
                if (classIds.Any())
                {
                    var classInfoList = classList.Where(x => classIds.Contains(x.PomId)).GroupBy(x => new { x.PomId, x.ParentId, x.Name }).Select(x => new { x.Key.PomId, x.Key.ParentId, x.Key.Name }).ToList();
                    if (classInfoList.Any())
                    {
                        string className = string.Empty;
                        Guid currentNode = Guid.Empty;
                        var oneFirst = classInfoList.Where(x => x.ParentId == null).SingleOrDefault();
                        if (oneFirst != null)
                        {
                            className = oneFirst?.Name + "/";
                            currentNode = oneFirst.PomId;
                        }
                        if (!string.IsNullOrWhiteSpace(className))
                        {
                            classInfoList = classInfoList.Where(x => x.ParentId != null).ToList();
                            foreach (var item in classInfoList)
                            {
                                var classInfo = classInfoList.Where(x => x.ParentId == currentNode).SingleOrDefault();
                                if (classInfo != null)
                                {
                                    className += classInfo.Name + "/";
                                    currentNode = classInfo.PomId;
                                }

                            }
                            projectDeteilSingle.ClassifyArrayName = className.Substring(0, className.Length - 1);

                        }
                    }
                }
            }

            //获取水运工况级别
            projectDeteilSingle.ConditionGradeName = await dbContext.Queryable<WaterCarriage>().Where(x => x.PomId.ToString() == projectDeteilSingle.ConditionGradeId).Select(x => x.Remarks).SingleAsync();
            //获取项目施工资质名称
            projectDeteilSingle.ProjectConstructionQualificationName = await dbContext.Queryable<ConstructionQualification>()
                .Where(x => x.PomId == projectDeteilSingle.ProjectConstructionQualificationId).Select(x => x.Name).SingleAsync();

            //获取项目干系单位
            var orgList = await dbContext.Queryable<ProjectOrg>()
                .Where(p => (p.ProjectId == projectDeteilSingle.Id || p.ProjectId == projectDeteilSingle.MasterProjectId) && p.IsDelete == 1 && p.OrganizationId != null).ToListAsync();
            if (orgList != null && orgList.Count > 0)
            {
                projectDeteilSingle.projectOrgDtos = mapper.Map<List<ProjectOrg>, List<ProjectOrgDto>>(orgList);
            }
            if (projectDeteilSingle.projectOrgDtos != null && projectDeteilSingle.projectOrgDtos.Count > 0)
            {
                //查询往来单位名称
                var unitName = await dbContext.Queryable<DealingUnit>()
                    .Where(x => orgList.Select(x => x.OrganizationId).Contains(x.PomId)).Select(x => new { x.PomId, x.ZBPNAME_ZH }).ToListAsync();
                //查询项目干系单位名称
                var orgTypeName = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && x.TypeNo == 2).ToListAsync();
                foreach (var item in projectDeteilSingle.projectOrgDtos)
                {
                    item.OrganizationName = unitName.Where(x => x.PomId == item.OrganizationId).Select(x => x.ZBPNAME_ZH).FirstOrDefault();
                    item.TypeName = orgTypeName.Where(x => x.Type == item.Type).Select(x => x.Name).FirstOrDefault();
                }
                projectDeteilSingle.projectOrgDtos = projectDeteilSingle.projectOrgDtos.OrderBy(x => x.Type).ToList();
            }
            //获取项目干系人员信息
            var dutyList = await dbContext.Queryable<ProjectLeader>()
                .Where(pl => (pl.ProjectId == projectDeteilSingle.Id || pl.ProjectId == projectDeteilSingle.MasterProjectId) && pl.IsDelete == 1)
                .ToListAsync();
            if (dutyList != null && dutyList.Count > 0)
            {
                projectDeteilSingle.projectDutyDtos = mapper.Map<List<ProjectLeader>, List<ProjectDutyDto>>(dutyList);
            }
            if (projectDeteilSingle.projectDutyDtos != null && projectDeteilSingle.projectDutyDtos.Count > 0)
            {
                //查询人员名称
                var dutyName = await dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.User>().Where(x => dutyList.Select(x => x.AssistantManagerId).Contains(x.PomId)).Select(x => new { x.PomId, x.Name, x.Phone }).ToListAsync();
                //查询项目干系人员名称
                var orgTypeName = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && x.TypeNo == 1).ToListAsync();
                foreach (var item in projectDeteilSingle.projectDutyDtos)
                {
                    item.AssistantManagerName = dutyName.Where(x => x.PomId == item.AssistantManagerId).Select(x => $"{x.Name}({x.Phone})").FirstOrDefault();
                    item.TypeName = orgTypeName.Where(x => x.Type == item.Type).Select(x => x.Name).FirstOrDefault();
                }
                projectDeteilSingle.projectDutyDtos = projectDeteilSingle.projectDutyDtos.OrderBy(x => x.Type).ThenByDescending(x => x.IsPresent).ToList();
            }
            if (projectDeteilSingle.projectDutyDtos != null && !projectDeteilSingle.projectDutyDtos.Any())
            {
                projectDeteilSingle.projectDutyDtos = projectDeteilSingle.projectDutyDtos.DistinctBy(t => new { t.AssistantManagerId, t.Type, t.IsPresent }).ToList();
            }
            #region 去重人员单位信息
            List<ProjectDutyDto> projectDutyDtoList = new List<ProjectDutyDto>();
            if (projectDeteilSingle != null && projectDeteilSingle.projectDutyDtos != null && projectDeteilSingle.projectDutyDtos.Any())
            {

                foreach (var item in projectDeteilSingle.projectDutyDtos)
                {
                    var projectDutyList = projectDeteilSingle.projectDutyDtos.Where(x => x.Type == item.Type).ToList();
                    if (projectDutyList.Any() && projectDutyList.Count >= 2)
                    {
                        foreach (var pro in projectDutyList)
                        {
                            if (pro.ProjectId == projectDeteilSingle.Id && !projectDutyDtoList.Exists(x => x.Type == item.Type))
                            {
                                projectDutyDtoList.Add(pro);
                            }
                        }
                    }
                    else
                    {
                        if (!projectDutyDtoList.Exists(x => x.Type == item.Type))
                        {
                            projectDutyDtoList.Add(item);
                        }

                    }

                }
            }

            projectDeteilSingle.projectDutyDtos = projectDutyDtoList;
            #endregion

            #region 获取月报开累完成产值匹配实际合同额
            //获取项目月报 (原币种计算  非全是人民币的情况)
            var mpReport = await dbContext.Queryable<MonthReport>().Where(t => t.IsDelete == 1 && t.ProjectId == basePrimaryRequestDto.Id).ToListAsync();
            if (mpReport != null && mpReport.Any())
            {
                projectDeteilSingle.CompleteOutputValue = mpReport.Sum(x => x.CurrencyCompleteProductionAmount);
            }
            else projectDeteilSingle.CompleteOutputValue = 0M;

            #endregion
            responseAjaxResult.Data = projectDeteilSingle;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 新增或修改项目
        /// </summary>
        /// <param name="addOrUpdateProjectRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddORUpdateProjectDetailAsync(AddOrUpdateProjectRequestDto addOrUpdateProjectRequestDto, LogInfo logDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();

            #region 条件判断
            List<int> orgs = new List<int>() { 1, 6, 2 };
            var validCount = addOrUpdateProjectRequestDto.projectOrgDtos.Count(x => orgs.Contains(x.Type));
            if (validCount < 3)
            {
                responseAjaxResult.Fail("业主、甲方、监理单位必须填写", HttpStatusCode.VerifyFail);
                responseAjaxResult.Data = false;
                return responseAjaxResult;
            }
            #endregion


            //获取项目数据
            var projectList = await dbContext.Queryable<Project>().ToListAsync();
            //增改项目信息
            var projectObject = new Project();
            //增改项目干系单位
            var projectDutys = new List<ProjectOrg>();
            //增改项目干系人员
            var proLeaderList = new List<ProjectLeader>();
            Guid projectId = GuidUtil.Next();
            var startWorkRecord = new StartWorkRecord();
            if (addOrUpdateProjectRequestDto.RequestType == true)
            {
                #region 开停工记录
                //开停工记录
                startWorkRecord = new StartWorkRecord()
                {
                    Id = GuidUtil.Next(),
                    CompanyId = addOrUpdateProjectRequestDto.CompanyId,
                    ProjectId = projectId,
                    Name = addOrUpdateProjectRequestDto.Name,
                    StartWorkTime = addOrUpdateProjectRequestDto.CommencementTime.HasValue ? addOrUpdateProjectRequestDto.CommencementTime.Value : null,
                    EndWorkTime = addOrUpdateProjectRequestDto.ShutdownDate.HasValue ? addOrUpdateProjectRequestDto.ShutdownDate.Value : null,
                    BeforeStatus = addOrUpdateProjectRequestDto.StatusId.ToString(),
                    StopWorkReson = addOrUpdateProjectRequestDto.ShutDownReason,
                    CreateTime = DateTime.Now,
                    UpdateUser = _currentUser.Name + "新增项目",
                    AfterStatus = addOrUpdateProjectRequestDto.StatusId.ToString(),
                };
                await dbContext.Insertable<StartWorkRecord>(startWorkRecord).ExecuteCommandAsync();
                #endregion
            }
            else
            {
                var isFirst = projectList.FirstOrDefault(x => x.Id == addOrUpdateProjectRequestDto.Id);
                if (isFirst != null)
                {
                    startWorkRecord = new StartWorkRecord()
                    {
                        Id = GuidUtil.Next(),
                        CompanyId = addOrUpdateProjectRequestDto.CompanyId,
                        ProjectId = addOrUpdateProjectRequestDto.Id.Value,
                        Name = addOrUpdateProjectRequestDto.Name,
                        StartWorkTime = addOrUpdateProjectRequestDto.CommencementTime,
                        EndWorkTime = addOrUpdateProjectRequestDto.ShutdownDate,
                        BeforeStatus = isFirst.StatusId.ToString(),
                        StopWorkReson = addOrUpdateProjectRequestDto.ShutDownReason,
                        CreateTime = DateTime.Now,
                        UpdateUser = _currentUser.Name + "修改项目",
                        AfterStatus = addOrUpdateProjectRequestDto.StatusId.ToString()
                    };
                    await dbContext.Insertable<StartWorkRecord>(startWorkRecord).ExecuteCommandAsync();
                }
            }


            #region 计算项目停工天数 供交建通每天发消息使用

            ////计算当前周期已过多少天
            //var nowDay = DateTime.Now.Day;
            ////当前月的最后一天
            //DateTime currentDate = DateTime.Now;
            //var stopDay = 0;
            //var monthLastDay = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1).AddDays(-1).Day;

            //if (nowDay == 27)
            //{
            //    stopDay = 1;
            //}
            //else if (nowDay != 26)
            //{
            //    //stopDay = nowDay + (monthLastDay-26);
            //    var startDay = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26").ObjToDate();
            //    var endDay = DateTime.Now.ToString("yyyy-MM-dd").ObjToDate();
            //    stopDay = TimeHelper.GetTimeSpan(startDay, endDay).Days;
            //}
            ////获取周期
            //var startTime = 0;
            //var endTime = 0;
            //Utils.GetDateRange(DateTime.Now, out startTime, out endTime);
            //var dayCount = 0;
            //if (addOrUpdateProjectRequestDto.Id.HasValue)
            //{
            //    dayCount = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.ProjectId == addOrUpdateProjectRequestDto.Id.Value && x.DateDay >= startTime && x.DateDay <= endTime).CountAsync();
            //}
            //stopDay = stopDay - dayCount >= 0 ? (stopDay - dayCount) : 0;
            //if (addOrUpdateProjectRequestDto.RequestType)
            //{
            //    //新增操作（如果新增的是在建项目的就会记录表里面）
            //    if (CommonData.PConstruc == addOrUpdateProjectRequestDto.StatusId.ToString())
            //    {
            //        ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
            //        {
            //            Id = projectId,
            //            OldStatus = addOrUpdateProjectRequestDto.StatusId.Value,
            //            NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
            //            ChangeTime = DateTime.Now,
            //            IsValid = 1,
            //            StopDay = stopDay
            //        };
            //        var isExist = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == projectId).CountAsync();
            //        if (isExist == 0)
            //        {
            //            await dbContext.Insertable(projectStatusChangeRecord).ExecuteCommandAsync();
            //        }

            //    }

            //}
            //else
            //{
            //    var projectStatusChangeSingle = await dbContext.Queryable<ProjectStatusChangeRecord>().FirstAsync(x => x.IsValid == 1
            //     && x.Id == addOrUpdateProjectRequestDto.Id);

            //    //修改操作  修改状态之前还是在建状态  所以停工天数不计算  生产日报也不会推送 等下再改为在建的时候 就会记录停工天数
            //    if (projectStatusChangeSingle != null && projectStatusChangeSingle.NewStatus == CommonData.PConstruc.ToGuid() && projectStatusChangeSingle.NewStatus != addOrUpdateProjectRequestDto.StatusId)
            //    {
            //        //说明停工 不算每天生产日报推送数据里面了 
            //        ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
            //        {
            //            ItemId = projectStatusChangeSingle.ItemId,
            //            Id = addOrUpdateProjectRequestDto.Id.Value,
            //            OldStatus = CommonData.PConstruc.ToGuid(),
            //            NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
            //            ChangeTime = DateTime.Now,
            //            IsValid = 1,
            //            StopDay = 0
            //        };
            //        await dbContext.Updateable(projectStatusChangeRecord).ExecuteCommandAsync();
            //    }
            //    //修改状态之前  原来状态不是在建状态了 这个时候要计算停工天数
            //    else if (projectStatusChangeSingle != null && projectStatusChangeSingle.NewStatus != CommonData.PConstruc.ToGuid() && addOrUpdateProjectRequestDto.StatusId == CommonData.PConstruc.ToGuid())
            //    {
            //        var diffDays = TimeHelper.GetTimeSpan(projectStatusChangeSingle.ChangeTime, DateTime.Now).Days;
            //        stopDay = projectStatusChangeSingle.StopDay.Value + diffDays;
            //        ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
            //        {
            //            ItemId = projectStatusChangeSingle.ItemId,
            //            Id = addOrUpdateProjectRequestDto.Id.Value,
            //            OldStatus = CommonData.PConstruc.ToGuid(),
            //            NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
            //            ChangeTime = DateTime.Now,
            //            IsValid = 1,
            //            StopDay = stopDay
            //        };
            //        await dbContext.Updateable(projectStatusChangeRecord).ExecuteCommandAsync();
            //    }
            //    else if (projectStatusChangeSingle == null && addOrUpdateProjectRequestDto.StatusId == CommonData.PConstruc.ToGuid())
            //    {
            //        //说明是有原来的非在建状态改为在建状态的
            //        var projectSingle = await dbContext.Queryable<Project>().SingleAsync(x => x.IsDelete == 1 && x.Id == addOrUpdateProjectRequestDto.Id);

            //        var currentMonth = String.Empty;
            //        if (DateTime.Now.Day > 26 && DateTime.Now.Day <= monthLastDay)
            //        {
            //            currentMonth = DateTime.Now.ToString("MM");
            //        }
            //        else
            //        {
            //            currentMonth = DateTime.Now.AddMonths(-1).ToString("MM");
            //        }
            //        var initDay = Convert.ToDateTime(DateTime.Now.ToString("yyyy-") + currentMonth + "-26");
            //        var diffDays = TimeHelper.GetTimeSpan(initDay, DateTime.Now).Days - 1;
            //        ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
            //        {
            //            ItemId = GuidUtil.Next(),
            //            Id = projectSingle.Id,
            //            OldStatus = projectSingle != null ? projectSingle.StatusId.Value : Guid.Empty,
            //            NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
            //            ChangeTime = DateTime.Now,
            //            IsValid = 1,
            //            StopDay = diffDays
            //        };
            //        await dbContext.Insertable(projectStatusChangeRecord).ExecuteCommandAsync();
            //    }
            //}
            #endregion

            #region 计算项目停工天数 供交建通每天发消息使用 新版本
            var startYearTime = DateTime.Now.ToDateDay() > int.Parse(DateTime.Now.ToString("yyyy1226")) ? DateTime.Now.ToString("yyyy-12-26 00:00:00") : DateTime.Now.AddYears(-1).ToString("yyyy-12-26 00:00:00");
            if (addOrUpdateProjectRequestDto.RequestType)//新增
            {
                //计算停工天数
                var stopDay = TimeHelper.GetTimeSpan(startYearTime.ObjToDate(), DateTime.Now).Days - 1;
                //新增项目
                ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                {
                    Id = projectId,
                    Status = addOrUpdateProjectRequestDto.StatusId.Value,
                    IsValid = 1,
                    ItemId = projectId,
                    ChangeTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00").ObjToDate(),
                    StopDay = stopDay
                };
                await dbContext.Insertable<ProjectStatusChangeRecord>(projectStatusChangeRecord).ExecuteCommandAsync();
            }
            else
            {
                var projectStatusSingle = await dbContext.Queryable<ProjectStatusChangeRecord>().Where(x => x.IsValid == 1 && x.Id == addOrUpdateProjectRequestDto.Id).FirstAsync();
                if (projectStatusSingle.Status != CommonData.PConstruc.ToGuid() && addOrUpdateProjectRequestDto.StatusId.Value == CommonData.PConstruc.ToGuid())
                {
                    //计算停工天数
                    var stopDay = TimeHelper.GetTimeSpan(projectStatusSingle.ChangeTime, DateTime.Now).Days - 1;
                    projectStatusSingle.StopDay += stopDay;
                }
                else if (projectStatusSingle.Status == CommonData.PConstruc.ToGuid() && addOrUpdateProjectRequestDto.StatusId.Value != CommonData.PConstruc.ToGuid())
                {
                    //计算停工天数
                    var stopDay = TimeHelper.GetTimeSpan(projectStatusSingle.ChangeTime, DateTime.Now).Days - 1;
                    projectStatusSingle.StopDay += stopDay;
                }
                projectStatusSingle.ChangeTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00").ObjToDate();
                await dbContext.Updateable<ProjectStatusChangeRecord>(projectStatusSingle).ExecuteCommandAsync();
            }

            #endregion


            if (addOrUpdateProjectRequestDto.RequestType)
            {


                projectObject = mapper.Map<AddOrUpdateProjectRequestDto, Project>(addOrUpdateProjectRequestDto);
                lock (projectObject)
                {
                    projectObject.PomId = Convert.ToInt32(TimeHelper.DateTimeToTimeStamp(DateTime.Now));
                }

                projectObject.Id = projectId;
                //projectObject.Id = GuidUtil.Next();
                projectObject.MasterProjectId = Guid.NewGuid();
                projectObject.Rate = projectObject.Rate / 100;
                projectObject.ExchangeRate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.Year == DateTime.Now.Year && x.CurrencyId == projectObject.CurrencyId.ToString()).Select(x => x.ExchangeRate).SingleAsync();
                projectObject.BudgetInterestRate = projectObject.BudgetInterestRate / 100;
                projectObject.Score = 0;
                //判断新增的项目名称是否相同
                var count = projectList.Where(x => x.Name == projectObject.Name && x.CompanyId == projectObject.CompanyId && x.IsDelete == 1).Count();
                if (count > 0)
                {
                    responseAjaxResult.FailResult(HttpStatusCode.CompanyIdentical, ResponseMessage.OPERATION_COMPANY_IDENTICAL);
                    return responseAjaxResult;
                }
                //判断项目编码是否存在
                var projectCode = projectList.Where(x => x.Code == projectObject.Code);
                if (!projectCode.Any())
                {
                    var code = projectList.Where(x => x.Code.Contains("PGHJ" + DateTime.Now.Year + "_".ToString()))
                        .OrderByDescending(x => Convert.ToInt32(x.Code.Substring(x.Code.IndexOf("_") + 1))).Select(x => x.Code).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        projectObject.Code = "PGHJ" + DateTime.Now.Year + "_" + 1.ToString();
                    }
                    else
                    {
                        var num = code.Substring(code.IndexOf("_") + 1);
                        projectObject.Code = "PGHJ" + DateTime.Now.Year + "_" + (Convert.ToInt32(num) + 1).ToString();
                    }
                }
                //插入项目干系单位
                projectDutys = mapper.Map<List<ProjectOrgDtos>, List<ProjectOrg>>(addOrUpdateProjectRequestDto.projectOrgDtos);
                foreach (var item in projectDutys)
                {
                    if (item.OrganizationId != null && item.OrganizationId != Guid.Empty)
                    {
                        item.Id = GuidUtil.Next();
                        item.ProjectId = projectObject.Id;
                    }
                }
                var oCount = projectDutys.GroupBy(x => new { x.Type, x.OrganizationId }).Count();
                if (projectDutys.Count() != oCount)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.FailResult(HttpStatusCode.SametypeIdentical, ResponseMessage.OPERATION_SAMETYPE_IDENTICAL);
                    return responseAjaxResult;
                }
                //插入项目干系人员相关信息
                proLeaderList = mapper.Map<List<ProjectDutyDtos>, List<ProjectLeader>>(addOrUpdateProjectRequestDto.projectDutyDtos);
                var isRepeatLeaders = proLeaderList.GroupBy(x => new { x.IsPresent, x.AssistantManagerId, x.Type }).Count();
                if (isRepeatLeaders != proLeaderList.Count())
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.FailResult(HttpStatusCode.RoleEmployed, ResponseMessage.OPERATION_ROLE_EMPLOYED);
                    return responseAjaxResult;
                }
                foreach (var item in proLeaderList)
                {
                    if (item.AssistantManagerId != null && item.AssistantManagerId != Guid.Empty)
                    {
                        item.Id = GuidUtil.Next();
                        item.ProjectId = projectObject.Id;
                        item.ProjectCode = projectObject.Code;
                    }
                }

                #region 记录在建的项目状态
                //if (CommonData.status.Contains(addOrUpdateProjectRequestDto.StatusId.ToString()))
                //{
                //    ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                //    {
                //        Id = projectObject.Id,
                //        OldStatus = projectObject.StatusId.Value,
                //        NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                //        ChangeTime = DateTime.Now,
                //        IsValid = 1,
                //        StopDay = 0
                //    };
                //    await dbContext.Insertable(projectStatusChangeRecord).ExecuteCommandAsync();
                //}
                #endregion

                #region 记录停工的项目状态
                if (CommonData.status2.Contains(addOrUpdateProjectRequestDto.StatusId.ToString()))
                {
                    ProjectChangeRecord projectChangeRecord = new ProjectChangeRecord();
                    projectChangeRecord.Id = GuidUtil.Next();
                    projectChangeRecord.ProjectId = projectObject.Id;
                    projectChangeRecord.Status = 0;
                    projectChangeRecord.StartTime = DateTime.Now.Date;
                    projectChangeRecord.EndTime = DateTime.MaxValue.Date;
                    await dbContext.Insertable(projectChangeRecord).ExecuteCommandAsync();
                }
                #endregion


                //新增干系单位
                await dbContext.Insertable(projectDutys).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                //新增干系人员
                await dbContext.Insertable(proLeaderList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                if (addOrUpdateProjectRequestDto.IsSubContractProject == 1)
                {
                    projectObject.PProjectMasterCode = addOrUpdateProjectRequestDto.PProjectMasterCode;
                }
                //新增项目
                await dbContext.Insertable(projectObject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                //项目变更记录
                await entityChangeService.RecordEntitysChangeAsync(EntityType.Project, projectObject.Id);

                //新增项目加入重点项目表
                await dbContext.Insertable<KeyProject>(new KeyProject()
                {
                    Id = GuidUtil.Next(),
                    Interval = 60,
                    ProjectId = projectObject.Id,
                    IsDelete = 1,
                    ProjectName = projectObject.ShortName,
                    IsNew = true
                }).ExecuteCommandAsync();

                //新增后直接推送项目信息
                await _pushPomService.PushProjectAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
                return responseAjaxResult;
            }
            else  //修改项目
            {

                projectObject = await dbContext.Queryable<Project>().SingleAsync(x => x.IsDelete == 1 && x.Id == addOrUpdateProjectRequestDto.Id);
                #region 业务判断
                if (projectObject == null)
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                #endregion

                #region 记录项目状态改变
                //if (addOrUpdateProjectRequestDto.TypeId != CommonData.NoConstrutionProjectType)
                //{
                //    int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
                //    var ofdays = DateTime.Now.Day <= 26 ? (DateTime.Now.Day + ((days - 26))) : DateTime.Now.Day - 26;
                //    var stopDay = 0;
                //    List<ProjectStatusChangeRecord> result = new List<ProjectStatusChangeRecord>();
                //    ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                //    {
                //        Id = projectObject.Id,
                //        OldStatus = projectObject.StatusId.Value,
                //        NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                //        ChangeTime = DateTime.Now,
                //        IsValid = 1,
                //        StopDay = 0
                //    };
                //    //只有把项目状态更改为在建状态时才会触发   其他状态不计算 （此逻辑是交建通每天推送日报数据使用  其他逻辑暂无使用）
                //    if (addOrUpdateProjectRequestDto.StatusId.Value == CommonData.PConstruc.ToGuid())
                //    {
                //        var startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
                //        var endTime = DateTime.Now.ToString("yyyy-MM-25 23:59:59");
                //        result = await dbContext.Queryable<ProjectStatusChangeRecord>()
                //            .Where(x => x.IsValid == 1 && x.Id == projectObject.Id && x.ChangeTime >= SqlFunc.ToDate(startTime)
                //            && x.ChangeTime <= SqlFunc.ToDate(endTime)).OrderBy(x => x.ChangeTime).ToListAsync();
                //        var count = result.Count;
                //        if (result != null && count > 1)
                //        {
                //            var time1 = result[count - 1].ChangeTime.ToDateDay();//最新一次一定是在建
                //                                                                 //求其他状态的时间 
                //            var lastStatusTime = result.Where(x => x.NewStatus != CommonData.PConstruc.ToGuid()).OrderBy(x => x.ChangeTime).ToList();
                //            if (lastStatusTime != null && lastStatusTime.Any())
                //            {
                //                var time2 = lastStatusTime[0].ChangeTime.ToDateDay();//取暂停的时间
                //                if (result[0].StopDay.Value + (time1 - time2) <= ofdays)
                //                {
                //                    stopDay = result[0].StopDay.Value + (time1 - time2);
                //                }
                //                else {
                //                    stopDay = result[0].StopDay.Value;
                //                }

                //            }
                //            else
                //            {
                //                if (DateTime.Now.Day >= 26)
                //                {
                //                    stopDay = (DateTime.Now.Day - 26);
                //                }
                //                else
                //                {
                //                    stopDay = DateTime.Now.Day + ((days - 26));
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (DateTime.Now.Day >= 26)
                //            {
                //                stopDay = (DateTime.Now.Day - 26);
                //            }
                //            else
                //            {
                //                stopDay = DateTime.Now.Day + ((days - 26));
                //            }
                //        }

                //    }
                //    else
                //    {
                //        if (DateTime.Now.Day >= 26)
                //        {
                //            stopDay = (DateTime.Now.Day - 26);
                //        }
                //        else
                //        {
                //            stopDay = DateTime.Now.Day + ((days - 26));
                //        }
                //    }
                //    projectStatusChangeRecord.StopDay = stopDay;
                //     await dbContext.Insertable<ProjectStatusChangeRecord>(projectStatusChangeRecord).ExecuteCommandAsync();
                //    foreach (var item in result)
                //    {
                //        item.StopDay = stopDay;
                //    }

                //    if (result != null && result.Count > 0)
                //    {
                //        //更新暂停天数
                //        await dbContext.Updateable<ProjectStatusChangeRecord>(result).Where(x => x.IsValid.Value == 1).WhereColumns(x => new { x.Id }).UpdateColumns(x => new { x.StopDay })
                //            .ExecuteCommandAsync();
                //    }
                //}
                #endregion
                #region 记录状态变更 停工记录 日报未填报使用
                //判断项目状态是否修改
                if (projectObject.StatusId != addOrUpdateProjectRequestDto.StatusId)
                {
                    ProjectChangeRecord projectChangeRecord = new ProjectChangeRecord();
                    var changeList = await dbContext.Queryable<ProjectChangeRecord>().Where(x => x.ProjectId == addOrUpdateProjectRequestDto.Id).OrderBy(x => x.EndTime).ToListAsync();
                    //判断是否为改为在建
                    if (CommonData.status.Contains(addOrUpdateProjectRequestDto.StatusId.ToString()))
                    {
                        var isExist = changeList.Where(x => x.EndTime == DateTime.MaxValue.Date).FirstOrDefault();
                        if (isExist != null)
                        {
                            isExist.EndTime = DateTime.Now.Date;
                            await dbContext.Updateable(isExist).WhereColumns(x => x.Id).ExecuteCommandAsync();
                        }
                    }
                    //判断是否是改为停工状态
                    else if (CommonData.status2.Contains(addOrUpdateProjectRequestDto.StatusId.ToString()))
                    {
                        var changeFirst = changeList.FirstOrDefault();
                        projectChangeRecord.Id = GuidUtil.Next();
                        projectChangeRecord.ProjectId = addOrUpdateProjectRequestDto.Id.Value;
                        projectChangeRecord.Status = 0;
                        projectChangeRecord.StartTime = DateTime.Now.Date;
                        projectChangeRecord.EndTime = DateTime.MaxValue.Date;
                        if (changeList.Count == 0)
                        {
                            await dbContext.Insertable(projectChangeRecord).ExecuteCommandAsync();
                        }
                        else if (changeFirst != null)
                        {
                            if (changeFirst.EndTime != DateTime.MaxValue.Date)
                            {
                                await dbContext.Insertable(projectChangeRecord).ExecuteCommandAsync();
                            }
                        }
                    }
                    projectObject.IsChangeStatus = 1;
                }
                #endregion

                mapper.Map(addOrUpdateProjectRequestDto, projectObject);
                //税率
                projectObject.ManagerType = addOrUpdateProjectRequestDto.ManagerType;
                projectObject.Rate = projectObject.Rate / 100;
                projectObject.ExchangeRate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.Year == DateTime.Now.Year && x.CurrencyId == projectObject.CurrencyId.ToString()).Select(x => x.ExchangeRate).SingleAsync();
                //标后预算毛利率
                projectObject.BudgetInterestRate = projectObject.BudgetInterestRate / 100;

                #region 项目干系单位信息
                var addOrgList = new List<ProjectOrg>();
                var updateOrgList = new List<ProjectOrg>();
                var removeOrgList = new List<ProjectOrg>();
                //查询干系单位id
                var existProjectId = addOrUpdateProjectRequestDto.projectOrgDtos.Select(x => x.Id).ToList();
                var oldOrgList = await dbContext.Queryable<ProjectOrg>().Where(x => x.IsDelete == 1 && (x.ProjectId == projectObject.Id || x.ProjectId == projectObject.MasterProjectId)).ToListAsync();
                //判断干系单位是否有值
                if (addOrUpdateProjectRequestDto.projectOrgDtos.Any())
                {
                    var addList = addOrUpdateProjectRequestDto.projectOrgDtos.Where(x => x.Id == null).ToList();
                    addOrgList = mapper.Map(addList, addOrgList);
                    updateOrgList = oldOrgList.Where(x => existProjectId.Contains(x.Id)).ToList();
                    removeOrgList = oldOrgList.Where(x => !existProjectId.Contains(x.Id)).ToList();
                }
                else
                {
                    removeOrgList = oldOrgList.Where(x => !existProjectId.Contains(x.Id)).ToList();
                }
                //数据填充
                addOrgList.ForEach(x =>
                {
                    x.Id = GuidUtil.Next();
                    x.ProjectId = addOrUpdateProjectRequestDto.Id;
                });
                updateOrgList.ForEach(x =>
                {
                    var orgSingle = addOrUpdateProjectRequestDto.projectOrgDtos.Where(s => s.Id == x.Id).SingleOrDefault();
                    mapper.Map(orgSingle, x);
                });
                removeOrgList.ForEach(x =>
                {
                    x.IsDelete = 0;
                });
                //机构种类判断
                var addOrUpdateOrgList = new List<ProjectOrg>();
                addOrUpdateOrgList.AddRange(addOrgList);
                addOrUpdateOrgList.AddRange(updateOrgList);
                var isRepeat = addOrUpdateOrgList.GroupBy(x => new { x.Type, x.OrganizationId }).Count();
                if (isRepeat != addOrUpdateOrgList.Count())
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.FailResult(HttpStatusCode.SametypeIdentical, ResponseMessage.OPERATION_SAMETYPE_IDENTICAL);
                    return responseAjaxResult;
                }
                if (addOrgList.Any())
                {
                    await dbContext.Insertable(addOrgList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                if (updateOrgList.Any())
                {
                    await dbContext.Updateable(updateOrgList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                if (removeOrgList.Any())
                {
                    await dbContext.Updateable(removeOrgList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                #endregion

                #region 项目人员信息
                var addLeaderList = new List<ProjectLeader>();
                var updateLeaderList = new List<ProjectLeader>();
                var removeLeaderList = new List<ProjectLeader>();
                var oldLeaderList = await dbContext.Queryable<ProjectLeader>().Where(x => x.IsDelete == 1 && (x.ProjectId == projectObject.Id || x.ProjectId == projectObject.MasterProjectId)).ToListAsync();
                //获取提交的干系人员id
                var existLeaderIds = addOrUpdateProjectRequestDto.projectDutyDtos.Select(x => x.Id).ToList();
                if (addOrUpdateProjectRequestDto.projectDutyDtos.Any())
                {
                    var addList = addOrUpdateProjectRequestDto.projectDutyDtos.Where(x => x.Id == null).ToList();
                    mapper.Map(addList, addLeaderList);
                    updateLeaderList = oldLeaderList.Where(x => existLeaderIds.Contains(x.Id)).ToList();
                    removeLeaderList = oldLeaderList.Where(x => !existLeaderIds.Contains(x.Id)).ToList();
                }
                else
                {
                    removeLeaderList = oldLeaderList.Where(x => !existLeaderIds.Contains(x.Id)).ToList();
                }
                addLeaderList.ForEach(x =>
                {
                    x.Id = GuidUtil.Next();
                    x.ProjectId = addOrUpdateProjectRequestDto.Id;
                    x.ProjectCode = addOrUpdateProjectRequestDto.Code;
                });
                updateLeaderList.ForEach(x =>
                {
                    var leaderSingle = addOrUpdateProjectRequestDto.projectDutyDtos.Where(s => s.Id == x.Id).SingleOrDefault();
                    mapper.Map(leaderSingle, x);
                });
                removeLeaderList.ForEach(x =>
                {
                    x.IsDelete = 0;
                });
                //人员职位判断
                var addOrUpdateLeaderList = new List<ProjectLeader>();
                addOrUpdateLeaderList.AddRange(addLeaderList);
                addOrUpdateLeaderList.AddRange(updateLeaderList);
                var isRepeatLeaders = addOrUpdateLeaderList.Where(x => x.IsPresent == true).GroupBy(x => new { x.Type, x.IsPresent }).Count();
                var groupList = addOrUpdateLeaderList.Where(x => x.IsPresent == true).GroupBy(x => new { x.Type, x.IsPresent }).ToList();
                if (isRepeatLeaders != addOrUpdateLeaderList.Where(x => x.IsPresent == true).Count())
                {
                    //获取各职位的相关名称
                    var dicTable = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && x.TypeNo == 1)
                        .Select(x => new DictionaryTable { Type = x.Type, Name = x.Name }).ToListAsync();
                    var strBuilder = new StringBuilder();
                    var typeName = new DictionaryTable();
                    if (dicTable != null)
                    {
                        foreach (var item in groupList)
                        {
                            typeName = dicTable.Where(x => x.Type == item.Key.Type).FirstOrDefault();
                            if (typeName != null)
                            {
                                strBuilder.Append(typeName.Name + "、");
                            }
                        }
                    }
                    strBuilder = strBuilder.Remove(strBuilder.Length - 1, 1);
                    responseAjaxResult.Data = false;
                    responseAjaxResult.FailResult(HttpStatusCode.RoleEmployed, strBuilder + ResponseMessage.OPERATION_ROLE_EMPLOYED);
                    return responseAjaxResult;
                }

                if (addLeaderList.Any())
                {
                    await dbContext.Insertable(addLeaderList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                if (updateLeaderList.Any())
                {
                    await dbContext.Updateable(updateLeaderList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                if (removeLeaderList.Any())
                {
                    await dbContext.Updateable(removeLeaderList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                #endregion
                //重新查询数据是否有开工日期
                //var newProjectObject = await dbContext.Queryable<Project>().SingleAsync(x => x.IsDelete == 1 && x.Id == addOrUpdateProjectRequestDto.Id);
                //if (newProjectObject.CommencementTime != DateTime.MinValue && !string.IsNullOrEmpty(newProjectObject.CommencementTime.ToString()))
                //{
                //    //修改项目信息
                //    await dbContext.Updateable(projectObject).IgnoreColumns(x => x.CommencementTime).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                //}
                //else
                //{
                //    //修改项目信息
                //    await dbContext.Updateable(projectObject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                //}
                if (addOrUpdateProjectRequestDto.IsSubContractProject == 1)
                {
                    projectObject.PProjectMasterCode = addOrUpdateProjectRequestDto.PProjectMasterCode;
                }
                if (addOrUpdateProjectRequestDto.CommencementTime != null && addOrUpdateProjectRequestDto.CommencementTime.HasValue)
                {
                    //修改项目信息
                    await dbContext.Updateable(projectObject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                else
                {
                    //修改项目信息
                    await dbContext.Updateable(projectObject).IgnoreColumns(x => x.CommencementTime).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }


                //项目变更记录
                await entityChangeService.RecordEntitysChangeAsync(EntityType.Project, projectObject.Id);
                //更改后直接推送项目信息
                await _pushPomService.PushProjectAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                return responseAjaxResult;
            }




        }
        #region 删除项目
        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> RemoveProjectAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var projecta = await dbContext.Queryable<Project>().SingleAsync(a => a.Id == basePrimaryRequestDto.Id);

            if (projecta != null)
            {
                projecta.IsDelete = 0;
                await dbContext.Updateable<Project>(projecta).Where(x => x.Id == basePrimaryRequestDto.Id).ExecuteCommandAsync();

                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
                return responseAjaxResult;
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                return responseAjaxResult;
            }
        }
        #endregion

        /// <summary>
        ///  根据项目ID查询projectwbs树
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectWBSResponseDto>>> SearchProjectWBSTree(Guid ProjectId)
        {
            ResponseAjaxResult<List<ProjectWBSResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProjectWBSResponseDto>>();
            List<ProjectWBSResponseDto> WBSResponseDtos = new List<ProjectWBSResponseDto>();
            if (ProjectId != null || ProjectId != Guid.Empty)
            {
                var projectwbsList = await dbContext.Queryable<ProjectWBS>().Where(x => x.IsDelete == 1 && x.ProjectId == ProjectId.ToString()).ToListAsync();
                if (projectwbsList.Any())
                {
                    foreach (var item in projectwbsList)
                    {
                        ProjectWBSResponseDto WBSResponseDto = new ProjectWBSResponseDto()
                        {
                            KeyId = item.KeyId,
                            Pid = item.Pid,
                            Id = item.Id,
                            ProjectId = item.ProjectId,
                            Name = item.Name,
                            EngQuantity = item.EngQuantity,
                            ContractAmount = item.ContractAmount,
                            UnitPrice = item.UnitPrice,
                            ProjectNum = item.ProjectNum
                        };
                        WBSResponseDtos.Add(WBSResponseDto);
                    }

                    if (WBSResponseDtos.Any())
                    {
                        WBSResponseDtos = ListToTreeUtil.GetTree(Guid.Empty, "0", WBSResponseDtos);
                    }
                }
            }

            responseAjaxResult.Data = WBSResponseDtos;
            responseAjaxResult.Count = WBSResponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 新增或修改ProjectWbBS树
        /// </summary>
        /// <param name="addOrUpdateProjectWBSRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveProjectWBSTreeAsync(AddOrUpdateProjectWBSRequsetDto addOrUpdateProjectWBSRequestDto, LogInfo logDto)
        {
            ProjectWBS projectWBS = new ProjectWBS();
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (addOrUpdateProjectWBSRequestDto.RequestType)
            {
                string? PId = string.Empty;
                if (addOrUpdateProjectWBSRequestDto.ParentId != null)
                {
                    PId = await baseProjectWBSRepository.AsQueryable()
                   .Where(x => x.Id == addOrUpdateProjectWBSRequestDto.ParentId && x.IsDelete == 1)
                   .Select(x => x.KeyId).SingleAsync();
                }
                else
                {
                    PId = "0";
                }
                var Ids = await baseProjectWBSRepository.AsQueryable().Where(x => x.ProjectId == addOrUpdateProjectWBSRequestDto.ProjectId && x.IsDelete == 1)
                    .Select(x => Convert.ToInt32(x.KeyId)).ToListAsync();
                projectWBS = mapper.Map<AddOrUpdateProjectWBSRequsetDto, ProjectWBS>(addOrUpdateProjectWBSRequestDto);

                projectWBS.Id = GuidUtil.Next();
                projectWBS.Pid = PId;
                if (Ids.Any())
                {
                    projectWBS.KeyId = (Ids.Max() + 1).ToString();
                }
                else
                {
                    projectWBS.KeyId = "1";
                }
                await baseProjectWBSRepository.AsInsertable(projectWBS).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
                responseAjaxResult.Data = true;
            }
            else
            {
                if (addOrUpdateProjectWBSRequestDto.ParentId == null)
                {
                    var Single = await baseProjectWBSRepository.AsQueryable().Where(x => x.Id == addOrUpdateProjectWBSRequestDto.Id && x.IsDelete == 1).SingleAsync();
                    var oldData = Single;
                    Single.Name = addOrUpdateProjectWBSRequestDto.Name;
                    await baseProjectWBSRepository.AsUpdateable(Single).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                }
                else
                {
                    LogInfo logRequestDto = new LogInfo();
                    //该项目下的所有数据
                    var projectwbsList = await dbContext.Queryable<ProjectWBS>().Where(x => x.ProjectId == addOrUpdateProjectWBSRequestDto.ProjectId && x.IsDelete == 1).ToListAsync();
                    //当前要修改的原数据
                    var Single = await baseProjectWBSRepository.AsQueryable().Where(x => x.Id == addOrUpdateProjectWBSRequestDto.Id && x.IsDelete == 1).SingleAsync();
                    var oldData = Single;
                    //所选父级菜单的数据
                    var parentSingle = projectwbsList.Where(x => x.Id == addOrUpdateProjectWBSRequestDto.ParentId && x.IsDelete == 1).FirstOrDefault();
                    //查询出该父级下的所有子集
                    var childList = projectwbsList.Where(x => x.Pid == parentSingle.KeyId).ToList();
                    //判断当前存在的值是否存在其中  若存在则只修改名称
                    if (childList.Any(x => x.Id == Single.Id))
                    {
                        Single.Name = addOrUpdateProjectWBSRequestDto.Name;


                        await baseProjectWBSRepository.AsUpdateable(Single).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                        return responseAjaxResult;
                    }
                    //获取树的所有一级
                    var firstList = projectwbsList.Where(x => x.IsDelete == 1 && x.Pid == "0").Select(x => x.KeyId).ToList();
                    //获取树的所有二级
                    var secondList = projectwbsList.Where(x => firstList.Contains(x.Pid)).Select(x => x.Id).ToList();
                    //判断所有父级菜单是否为树的二级
                    if (secondList.Any(x => x == parentSingle.Id))
                    {
                        //该项目下所有的keyid
                        var keyIds = projectwbsList.Where(x => x.ProjectId == addOrUpdateProjectWBSRequestDto.ProjectId && x.IsDelete == 1)
                            .Select(x => Convert.ToInt32(x.KeyId)).ToList();
                        Single.Name = addOrUpdateProjectWBSRequestDto.Name;
                        Single.KeyId = (keyIds.Max() + 1).ToString();
                        Single.Pid = parentSingle.KeyId;



                        await baseProjectWBSRepository.UpdateAsync(Single);
                    }
                    else
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_NOTSHIFT_STRUCTURE, HttpStatusCode.NotShiftStructure);
                        return responseAjaxResult;
                    }
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
            return responseAjaxResult;
        }

        /// <summary>
        /// 根据地址获取地点区域
        /// </summary>
        /// <param name="requestDto">经纬度</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectAreaRegionResponseDto>> SearchProjectAreaRegion(ProjectLonLatRequestDto requestDto)
        {
            ResponseAjaxResult<ProjectAreaRegionResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectAreaRegionResponseDto>();
            ProjectAreaRegionResponseDto responseDto = new ProjectAreaRegionResponseDto();
            //根据地址获取省市县
            Dictionary<string, string> address = Utils.GetAddress(requestDto.Address);
            string province = "";
            string city = "";
            string country = "";
            //省
            if (address.ContainsKey("province"))
            {
                province = address["province"];
            }
            //市
            if (address.ContainsKey("city"))
            {
                city = address["city"];
            }
            //县
            if (address.ContainsKey("country"))
            {
                country = address["country"];
            }

            var substr = "";
            if (!string.IsNullOrWhiteSpace(province))
            {
                if (province.Contains("黑龙江") || province.Contains("内蒙古"))
                {
                    substr = province.Substring(0, 3);
                }
                else
                {
                    substr = province.Substring(0, 2);
                }
            }
            else
            {
                if (city.Length != 0)
                {
                    substr = city.Substring(0, city.Length - 1);
                }
                else
                {
                    substr = country;
                }
            }

            //查询所在区域  排除无用数据
            var regionList = await dbContext.Queryable<ProjectArea>().Where(x => x.IsDelete == 1 && x.Id.ToString() != "08db6bad-fc86-4f42-86e9-4722003c4d49" && x.Remarks.Contains(substr)).ToListAsync();
            if (regionList.Count != 0)
            {
                var regionSingle = regionList.FirstOrDefault();
                responseDto.RegionId = regionSingle.AreaId;
                responseDto.RegionName = regionSingle.Name;
                //查询所在省份
                var areaList = await dbContext.Queryable<Province>().Where(x => x.IsDelete == 1 && x.RegionId == regionSingle.AreaId).ToListAsync();

                var areaFirst = new Province();
                //查询出第一级的省或市
                if (!string.IsNullOrWhiteSpace(province))
                {
                    areaFirst = areaList.Where(x => x.Zaddvsname.Equals(province) && x.Zaddvsup == "0").FirstOrDefault();
                }
                else
                {
                    areaFirst = areaList.Where(x => x.Zaddvsname.Equals(city) && x.Zaddvsup == "0").FirstOrDefault();
                }
                if (areaFirst != null)
                {
                    //查询出第二级的市或区
                    var areaSecondUp = areaList.Where(x => x.Zaddvsup == areaFirst.Zaddvscode).ToList();
                    if (areaSecondUp.Count == 1)
                    {
                        if (areaSecondUp.FirstOrDefault().Zaddvsname == "市辖区")
                        {
                            var areaThirdUp = areaList.Where(x => x.Zaddvsup == areaSecondUp.FirstOrDefault().Zaddvscode).ToList();
                            var areaThirdName = areaThirdUp.Where(x => x.Zaddvsname.Equals(country)).FirstOrDefault();
                            responseDto.AreaId = areaThirdName.PomId;
                            responseDto.AreaName = country;
                        }
                    }
                    else
                    {
                        var areaSecond = areaSecondUp.Where(x => x.Zaddvsname.Equals(city)).FirstOrDefault();
                        if (areaSecond != null)
                        {
                            //查询出第三级的区
                            var areaThirdUp = areaList.Where(x => x.Zaddvsup == areaSecond.Zaddvscode).ToList();
                            var areaThird = areaThirdUp.Where(x => x.Zaddvsname.Equals(country)).FirstOrDefault();
                            if (string.IsNullOrWhiteSpace(country))
                            {
                                responseDto.AreaId = areaList.Where(x => x.Zaddvsname.Equals(city)).Select(x => x.PomId).FirstOrDefault();
                                responseDto.AreaName = city;
                            }
                            else
                            {
                                responseDto.AreaId = areaList.Where(x => x.Zaddvsname.Equals(country)).Select(x => x.PomId).FirstOrDefault();
                                responseDto.AreaName = country;
                            }
                        }
                    }
                }
            }

            responseAjaxResult.Data = responseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;

            ////根据经纬度获取省市县
            //var positionInfo = await Utils.GetPositionInfoAsync(requestDto.Lon, requestDto.Lat);
            //         //查询省市县信息
            //         var areaSingle = await dbContext.Queryable<Province>().Select(x => new Province { PomId = x.PomId, Zaddvsname = x.Zaddvsname, RegionId = x.RegionId }).SingleAsync(x => x.Zaddvscode == positionInfo.AdCode);
            //         if (areaSingle != null)
            //         {
            //             //获取区域名称
            //             var regionName = await dbContext.Queryable<ProjectArea>().Where(x => x.AreaId == areaSingle.RegionId).Select(x => x.Name).SingleAsync();
            //             responseDto.AreaId = areaSingle.PomId;
            //             responseDto.AreaName = areaSingle.Zaddvsname;
            //             responseDto.RegionId = areaSingle.RegionId;
            //             responseDto.RegionName = regionName;

            //             responseAjaxResult.Data = responseDto;
            //             responseAjaxResult.Success();
            //             return responseAjaxResult;
            //         }
            //         //数据不存在时
            //         responseAjaxResult.Data = responseDto;
            //         responseAjaxResult.Success(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
            //         return responseAjaxResult;
        }

        /// <summary>
        /// 删除ProjectWbBS树
        /// </summary>
        /// <param name="deleteProjectWBSRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeleteProjectWBSTreeAsync(Guid Id)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var projectWbsSingle = await dbContext.Queryable<ProjectWBS>().Where(x => x.IsDelete == 1 && x.Id == Id).SingleAsync();
            var oldData = projectWbsSingle;
            if (projectWbsSingle != null)
            {
                projectWbsSingle.IsDelete = 0;
                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/项目结构设置/删除",
                    BusinessRemark = "/系统管理/项目结构设置/删除",
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                };
                #endregion
                await baseProjectWBSRepository.AsUpdateable(projectWbsSingle).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
                responseAjaxResult.Data = true;
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL);
                responseAjaxResult.Data = false;
            }
            return responseAjaxResult;
        }

        /// <summary>
        /// 保存项目-项目结构树
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveProjectWBSTreeBizAsync(SaveProjectWBSRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();

            //var month = DateTime.Now.ToDateMonth();//原来的
            //当前日期>=26 & <=addmonth(1) 1  下月1号
            var month = DateTime.Now.ToDateMonth();

            //校验是否在可更改的时间范围内
            var hMenu = await dbContext.Queryable<HomeMenu>().Where(x => x.IsDelete == 1 && x.Display == true).FirstAsync();
            if (hMenu == null)
            {
                //关闭修改结构权限
                if (DateTime.Now.Day == 1)//如果是1号   是上月数据最后修改的一天 在填报范围内 不可更改项目结构
                {
                    return result.FailResult(HttpStatusCode.NoAuthorityOperateFail, "填报时间内不可调整项目结构");
                }
                else if (DateTime.Now.Day >= 26)//当月的数据  在填报范围内 不可更改项目结构
                {
                    return result.FailResult(HttpStatusCode.NoAuthorityOperateFail, "填报时间内不可调整项目结构");
                }
            }

            if (DateTime.Now.Day == 1)//如果是1号  
            {
                month = DateTime.Now.AddMonths(-1).ToDateMonth();
            }
            else if (DateTime.Now.Day >= 26)//当月 
            { }
            else
            {
                //减掉一个月
                month = DateTime.Now.AddMonths(-1).ToDateMonth();
            }

            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var wbsList = await GetProjectWBSListAsync(project.Id.ToString());
            var reqWBSNodeList = new List<SaveProjectWBSRequestDto.ReqProjectWBSNode>();
            var wbsIds = wbsList.Where(t => int.TryParse(t.KeyId, out int r)).Select(t => int.Parse(t.KeyId)).ToArray();
            int maxKeyId = wbsIds.Any() ? wbsIds.Max() : 0;
            MapReqNodeList(null, model.Nodes, reqWBSNodeList, wbsList, ref maxKeyId);
            var addReqWBSNodes = reqWBSNodeList.Where(t => t.State == ModelState.Add).ToList();
            var exsitsWBSIds = reqWBSNodeList.Where(t => t.Id != null).Select(t => (Guid)t.Id).ToArray();
            var updateWBSIds = reqWBSNodeList.Where(t => t.Id != null && t.State == ModelState.Update).Select(t => (Guid)t.Id).ToArray();
            var removeWBSList = wbsList.Where(t => !exsitsWBSIds.Contains(t.Id)).ToList();
            var addWBSList = new List<ProjectWBS>();
            var updateWBSList = wbsList.Where(t => updateWBSIds.Contains(t.Id)).ToList();
            foreach (var removeWBS in removeWBSList)
            {
                removeWBS.IsDelete = 0;
            }
            foreach (var reqWBSNode in addReqWBSNodes)
            {
                var wbs = new ProjectWBS();
                wbs = mapper.Map(reqWBSNode, wbs);
                wbs.Id = GuidUtil.Next();
                wbs.ProjectId = project.Id.ToString();
                wbs.ProjectWBSId = project.PomId.ToString();
                wbs.ProjectNum = Utils.ListEncoding(project.Id);
                addWBSList.Add(wbs);
            }
            foreach (var updateWBS in updateWBSList)
            {
                var reqWBSNode = reqWBSNodeList.Single(t => t.Id == updateWBS.Id);
                mapper.Map(reqWBSNode, updateWBS);
            }
            #region 日志信息
            LogInfo logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
                DataId = model.ProjectId,
                BusinessModule = "/系统管理/项目结构设置/编辑wbs树",
                BusinessRemark = "/系统管理/项目结构设置/编辑wbs树"
            };

            #endregion
            // 删除
            if (removeWBSList.Any())
            {
                await dbContext.Updateable(removeWBSList).UpdateColumns(t => new { t.IsDelete, t.DeleteId, t.DeleteTime }).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                #region 无用代码
                //List<ProjectWbsHistoryMonth> projectWbsHistoryMonths = new List<ProjectWbsHistoryMonth>();
                //foreach (var item in removeWBSList)
                //{
                //    var projectWbsHistoryMonth = new ProjectWbsHistoryMonth()
                //    {
                //        UpdateTime = item.UpdateTime,
                //        ContractAmount = item.ContractAmount,
                //        CreateId = item.CreateId,
                //        CreateTime = item.CreateTime,
                //        DeleteTime = item.DeleteTime,
                //        Def = item.Def,
                //        DownOne = item.DownOne,
                //        EngQuantity = item.EngQuantity,
                //        ItemNum = item.ItemNum,
                //        KeyId = item.KeyId,
                //        Name = item.Name,
                //        Prev = item.Prev,
                //        Pid = item.Pid,
                //        ProjectNum = item.ProjectNum,
                //        ProjectWBSId = item.ProjectWBSId,
                //        UnitPrice = item.UnitPrice,
                //        ProjectId = item.ProjectId,
                //        Id = item.Id,
                //        IsDelete = 0,
                //        DeleteId = item.DeleteId,
                //        UpdateId = item.UpdateId,
                //    };
                //    projectWbsHistoryMonth.DateMonth = month;
                //    projectWbsHistoryMonths.Add(projectWbsHistoryMonth);
                //}
                //await dbContext.Updateable<ProjectWbsHistoryMonth>(projectWbsHistoryMonths).Where(x => x.DateMonth == month).UpdateColumns(t => new { t.IsDelete, t.DeleteId, t.DeleteTime }).ExecuteCommandAsync();
                #endregion
            }
            // 更新
            if (updateWBSList.Any())
            {
                await dbContext.Updateable(updateWBSList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                #region 无用代码
                //List<ProjectWbsHistoryMonth> projectWbsHistoryMonths = new List<ProjectWbsHistoryMonth>();
                //foreach (var item in updateWBSList)
                //{
                //    var projectWbsHistoryMonth = new ProjectWbsHistoryMonth()
                //    {
                //        UpdateTime = item.UpdateTime,
                //        ContractAmount = item.ContractAmount,
                //        CreateId = item.CreateId,
                //        CreateTime = item.CreateTime,
                //        DeleteTime = item.DeleteTime,
                //        Def = item.Def,
                //        DownOne = item.DownOne,
                //        EngQuantity = item.EngQuantity,
                //        ItemNum = item.ItemNum,
                //        KeyId = item.KeyId,
                //        Name = item.Name,
                //        Prev = item.Prev,
                //        Pid = item.Pid,
                //        ProjectNum = item.ProjectNum,
                //        ProjectWBSId = item.ProjectWBSId,
                //        UnitPrice = item.UnitPrice,
                //        ProjectId = item.ProjectId,
                //        Id = item.Id,
                //        IsDelete = item.IsDelete,
                //        DeleteId = item.DeleteId,
                //        UpdateId = item.UpdateId


                //    };
                //    projectWbsHistoryMonth.DateMonth = month;
                //    projectWbsHistoryMonths.Add(projectWbsHistoryMonth);
                //}
                //await dbContext.Updateable<ProjectWbsHistoryMonth>(projectWbsHistoryMonths).Where(x => x.DateMonth == month).ExecuteCommandAsync();
                #endregion
            }
            // 新增
            if (addWBSList.Any())
            {
                await dbContext.Insertable(addWBSList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                addWBSList.AddRange(updateWBSList);
                //List<ProjectWbsHistoryMonth> projectWbsHistoryMonths = new List<ProjectWbsHistoryMonth>();
                //foreach (var item in addWBSList)
                //{
                //    var projectWbsHistoryMonth = new ProjectWbsHistoryMonth()
                //    {
                //        UpdateTime = item.UpdateTime,
                //        ContractAmount = item.ContractAmount,
                //        CreateId = item.CreateId,
                //        CreateTime = item.CreateTime,
                //        DeleteTime = item.DeleteTime,
                //        Def = item.Def,
                //        DownOne = item.DownOne,
                //        EngQuantity = item.EngQuantity,
                //        ItemNum = item.ItemNum,
                //        KeyId = item.KeyId,
                //        Name = item.Name,
                //        Prev = item.Prev,
                //        Pid = item.Pid,
                //        ProjectNum = item.ProjectNum,
                //        ProjectWBSId = item.ProjectWBSId,
                //        UnitPrice = item.UnitPrice,
                //        ProjectId = item.ProjectId,
                //        Id = item.Id,
                //        IsDelete = item.IsDelete,
                //        DeleteId = item.DeleteId,
                //        UpdateId = item.UpdateId


                //    };
                //    projectWbsHistoryMonth.DateMonth = month;
                //    projectWbsHistoryMonths.Add(projectWbsHistoryMonth);
                //}
                var existData = await dbContext.Queryable<ProjectWbsHistoryMonth>().Where(x => x.DateMonth == month).ToListAsync();
                foreach (var item in existData)
                {
                    item.IsDelete = 0;
                }
                await dbContext.Updateable<ProjectWbsHistoryMonth>(existData).ExecuteCommandAsync();
                //await baseProjectWbsHistory.InsertOrUpdateAsync(projectWbsHistoryMonths);
                //await dbContext.Insertable<ProjectWbsHistoryMonth>(projectWbsHistoryMonths).ExecuteCommandAsync();
            }
            #region 新增wbs历史

            if (wbsList.Any())
            {
                List<ProjectWbsHistoryMonth> projectWbsHistoryMonths = new List<ProjectWbsHistoryMonth>();
                foreach (var item in wbsList)
                {
                    var projectWbsHistoryMonth = new ProjectWbsHistoryMonth()
                    {
                        ContractAmount = item.ContractAmount,
                        CreateTime = DateTime.Now,
                        Def = item.Def,
                        DownOne = item.DownOne,
                        EngQuantity = item.EngQuantity,
                        ItemNum = item.ItemNum,
                        KeyId = item.KeyId,
                        Name = item.Name,
                        Prev = item.Prev,
                        Pid = item.Pid,
                        ProjectNum = item.ProjectNum,
                        ProjectWBSId = item.ProjectWBSId,
                        UnitPrice = item.UnitPrice,
                        ProjectId = item.ProjectId,
                        IsDelete = item.IsDelete,
                        Id = item.Id
                    };
                    projectWbsHistoryMonth.DateMonth = month;
                    projectWbsHistoryMonths.Add(projectWbsHistoryMonth);
                }
                await baseProjectWbsHistory.InsertOrUpdateAsync(projectWbsHistoryMonths);
            }
            #endregion
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 查询项目-项目结构树
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectWBSComposeResponseDto>> SearchProjectWBSTreeBizAsync(Guid projectId)
        {
            var result = new ResponseAjaxResult<ProjectWBSComposeResponseDto>();
            var resNodes = new List<ProjectWBSComposeResponseDto.ResProjectWBSNode>();
            var wbsList = await GetProjectWBSListAsync(projectId.ToString());
            var level1WBSNodeList = wbsList.Where(t => t.Pid == "0").ToList();
            foreach (var level1WBSNode in level1WBSNodeList)
            {
                resNodes.Add(MapResNode(level1WBSNode, wbsList));
            }
            return result.SuccessResult(new ProjectWBSComposeResponseDto() { ProjectId = projectId, Nodes = resNodes.ToArray() });
        }

        /// <summary>
        /// 转化为新的请求项目结构对象
        /// </summary>
        /// <returns></returns>
        private ProjectWBSComposeResponseDto.ResProjectWBSNode MapResNode(ProjectWBS node, List<ProjectWBS> wbsList)
        {
            var resNode = new ProjectWBSComposeResponseDto.ResProjectWBSNode();
            resNode = mapper.Map(node, resNode);
            var resChildNodes = new List<ProjectWBSComposeResponseDto.ResProjectWBSNode>();
            if (node.KeyId != node.Pid)
            {
                var childNodes = wbsList.Where(t => t.Pid == node.KeyId).ToList();
                foreach (var childNode in childNodes)
                {
                    resChildNodes.Add(MapResNode(childNode, wbsList));
                }
            }
            resNode.Children = resChildNodes.ToArray();
            return resNode;
        }

        /// <summary>
        /// 转化为新的请求项目结构对象集合
        /// </summary>
        /// <returns></returns>
        private void MapReqNodeList(SaveProjectWBSRequestDto.ReqProjectWBSNode? parentNode, SaveProjectWBSRequestDto.ReqProjectWBSNode[] nodes, List<SaveProjectWBSRequestDto.ReqProjectWBSNode> newNodeList, List<ProjectWBS> oldWBSList, ref int maxKeyId)
        {
            foreach (var node in nodes)
            {
                var newNode = (SaveProjectWBSRequestDto.ReqProjectWBSNode)node.Clone();
                newNode.Children = null;
                // 作为更新的对象，保证KeyId，Pid,ProjectNum与库一致
                var oldWBS = oldWBSList.FirstOrDefault(t => t.Id == newNode.Id);
                newNode.State = oldWBS == null ? ModelState.Add : node.State;
                if (newNode.State == ModelState.Add)
                {
                    newNode.Id = null;
                    maxKeyId = maxKeyId + 1;
                    newNode.KeyId = maxKeyId.ToString();
                    newNode.Pid = parentNode == null ? "0" : parentNode.KeyId;
                }
                else if (newNode.State == ModelState.Update)
                {
                    if (oldWBS != null)
                    {
                        newNode.ProjectNum = oldWBS.ProjectNum;
                        newNode.KeyId = oldWBS.KeyId;
                        newNode.Pid = oldWBS.Pid;
                    }
                }
                newNodeList.Add(newNode);
                if (node.Children != null && node.Children.Any())
                {
                    MapReqNodeList(newNode, node.Children, newNodeList, oldWBSList, ref maxKeyId);
                }
            }
        }

        /// <summary>
        /// 获取一个项目(部分字段（Id,Name,PomId）)
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns></returns>
        private async Task<Project?> GetProjectPartAsync(Guid projectId)
        {
            return await dbContext.Queryable<Project>().Where(t => t.Id == projectId && t.IsDelete == 1).Select(t => new Project() { Id = t.Id, Name = t.Name, PomId = t.PomId }).FirstAsync();
        }

        /// <summary>
        /// 获取项目结构集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectWBS>> GetProjectWBSListAsync(string? projectId)
        {
            return await dbContext.Queryable<ProjectWBS>().Where(x => x.ProjectId == projectId && x.IsDelete == 1).OrderBy(x => Convert.ToInt32(x.Prev)).ToListAsync();
        }

        /// <summary>
        /// 获取登陆人未填日报弹框信息
        /// </summary>
        /// <param name="currentUser">当前登陆人</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BasePullDownResponseDto>> GetNotFillBulletBoxAsync(CurrentUser currentUser)
        {
            //项目状态Id
            List<Guid> projectStatusList = new List<Guid>()
            {
                "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(),
                "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(),
                "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(),
                "75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid()
            };
            ResponseAjaxResult<BasePullDownResponseDto> responseAjaxResult = new ResponseAjaxResult<BasePullDownResponseDto>();
            var basePullResponseDto = new BasePullDownResponseDto();
            var notFillMsg = string.Empty;//为填报返回内容
            List<Project> fillProjects = new List<Project>();
            List<Guid> proIds = new List<Guid>();
            var ownShipsCount = 0;//自有船舶数量
                                  //过滤类型符合条件的项目
            var project = await dbContext.Queryable<Project>()
              .LeftJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
               .Where((x, y) => x.IsDelete == 1 && projectStatusList.Contains(x.StatusId.Value))
               .ToListAsync();

            var curRoleInfo = _currentUser.RoleInfos.Where(role => role.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
            var nowDay = DateTime.Now.ToDateDay();
            if (_currentUser.CurrentLoginIsAdmin || (curRoleInfo != null && curRoleInfo.Type == 2))//管理员   超级管理员
            {
                fillProjects = project;
                //获取应填报项目ids
                proIds = fillProjects.Select(x => x.Id).ToList();
                //已填报船舶
                var fillShipCount = dbContext.Queryable<ShipDayReport>().Where(x => Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date && nowDay == x.DateDay && x.IsDelete == 1).Select(x => x.ShipId).Distinct().Count();
                //船舶特殊处理  获取所有自有船舶-当日已填船舶
                if (!curRoleInfo.IsAdmin && curRoleInfo.Oid != "101162350")//公司管理员
                {
                    //获取当前机构下所有自有船舶
                    var institutionFirst = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == curRoleInfo.Oid).FirstAsync();
                    var institutionIdResponse = await baseService.SearchCompanySubPullDownAsync(institutionFirst.PomId.Value);
                    var institutionIds = institutionIdResponse.Data.Select(x => x.Id.Value).ToList();
                    //追加当前角色公司
                    institutionIds.Add(institutionFirst.PomId.Value);
                    ownShipsCount = dbContext.Queryable<OwnerShip>().Where(x => institutionIds.Contains(x.CompanyId.Value)).Count() - fillShipCount;
                }
                else
                {
                    //超级管理员
                    ownShipsCount = dbContext.Queryable<OwnerShip>().Count() - fillShipCount;
                }
            }
            else
            {
                var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).FirstAsync();
                var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value);
                var InstitutionIdList = InstitutionId.Data.Select(x => x.Id).ToList();
                //获取当前登陆人所属项目部所有项目信息   获取应填报项目数据
                fillProjects = project.Where(x => InstitutionIdList.Contains(x.ProjectDept) && x.IsDelete == 1).ToList();
                //获取应填报项目ids
                proIds = fillProjects.Select(x => x.Id).ToList();
                //获取当前项目所有进场船舶
                var shipMovmentCount = dbContext.Queryable<ShipMovement>().Where(x => proIds.Contains(x.ProjectId) && x.Status == ShipMovementStatus.Enter && x.IsDelete == 1 && x.ShipType == ShipType.OwnerShip).Select(x => x.ShipId).Count();
                //项目部人员进来  只能看见当前项目部关联某个项目的船舶数量
                //获取已报自有船舶日报
                var fillShipCount = dbContext.Queryable<ShipDayReport>().Where(x => proIds.Contains(x.ProjectId) && Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date && nowDay == x.DateDay && x.IsDelete == 1 && x.ShipDayReportType == ShipDayReportType.ProjectShip).Select(x => x.ShipId).Distinct().Count();
                ownShipsCount = shipMovmentCount - fillShipCount;

            }

            //获取当前登陆人所在部门当日已填报所有项目
            var uProjects = await dbContext.Queryable<DayReport>().Where(x => proIds.Contains(x.ProjectId) && Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date && x.DateDay == nowDay && x.ProcessStatus == DayReportProcessStatus.Submited && x.IsDelete == 1).ToListAsync();
            //获取当前登陆人所在部门当日已填安监日报信息
            var uSafeSupers = await dbContext.Queryable<SafeSupervisionDayReport>().Where(x => proIds.Contains(x.ProjectId) && x.DateDay == nowDay && Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date && x.IsDelete == 1).ToListAsync();

            var shipMovementList = await dbContext.Queryable<ShipMovement>().Where(x => x.Status == ShipMovementStatus.Enter && x.IsDelete == 1).ToListAsync();
            string isMsg = "true";//是否弹框  false 否
            if (fillProjects.Count() != 0)//当日应填报项目不能为空
            {
                //如果项目日报今日已经填完
                if (uProjects.Count() == fillProjects.Count()) { }
                else if (uProjects.Count() != fillProjects.Count())//如果应填报项目数量与实际项目填报数量不匹配  那就是当前项目部有项目未填当日日报
                {
                    notFillMsg = DateTime.Now.ToString("yyyy年MM月dd日") + "您需填报：产值日报" + "（" + (fillProjects.Count() - uProjects.Count()) + "个)";
                }
                //如果项目日报今日已经填完   如果应填报项目数量与实际项目填报数量不匹配  那就是当前项目部有项目未填当日安监日报
                if (uSafeSupers.Count() == fillProjects.Count()) { }
                else
                {
                    if (!string.IsNullOrEmpty(notFillMsg)) notFillMsg += "、安监日报";
                    else notFillMsg = DateTime.Now.ToString("yyyy年MM月dd日") + "您需填报：安监日报";
                    notFillMsg = notFillMsg + "（" + (fillProjects.Count() - uSafeSupers.Count()) + "个)";
                }
                if (shipMovementList.Any())
                {
                    if (ownShipsCount != 0)
                    {
                        if (!string.IsNullOrEmpty(notFillMsg)) notFillMsg += "、船舶日报（" + ownShipsCount + "艘）";
                        else notFillMsg = DateTime.Now.ToString("yyyy年MM月dd日") + "您需填报：船舶日报（" + ownShipsCount + "艘）";
                    }
                }
                else
                {
                    notFillMsg += "、暂无船舶进场";
                }
            }
            if (string.IsNullOrEmpty(notFillMsg)) isMsg = "false";
            basePullResponseDto = new BasePullDownResponseDto
            {
                Name = notFillMsg,
                Code = isMsg
            };
            responseAjaxResult.Data = basePullResponseDto;
            responseAjaxResult.Success(ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }

        /// <summary>
        /// 获取币种汇率
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<ProjectCurrencyResponseDto>> GetCurrentcyRate(Guid currencyId)
        {
            var year = DateTime.Now.Year;
            ResponseAjaxResult<ProjectCurrencyResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectCurrencyResponseDto>();
            ProjectCurrencyResponseDto projectCurrencyResponseDto = new ProjectCurrencyResponseDto();
            //获取汇率表中的数据
            var rate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.CurrencyId == currencyId.ToString()
            && x.Year == year).Select(x => x.ExchangeRate).SingleAsync();
            projectCurrencyResponseDto.ExchangeRate = rate;
            responseAjaxResult.Data = projectCurrencyResponseDto;
            responseAjaxResult.Success(ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }

        /// <summary>
        ///  项目与报表负责人列表
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectResponseDto>>> ProjectAboutStatementAsync(ProjectAboutStatmentRequestDto searchRequestDto)
        {
            ResponseAjaxResult<List<ProjectResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProjectResponseDto>>();
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
            //获取临时补录项目部id
            var depIds = await dbContext.Queryable<ProjectDepartment>().Where(x => x.Name == "临时补录项目部" && departmentIds.Contains(x.CompanyId.Value)).Select(x => x.PomId.Value).ToListAsync();
            departmentIds.AddRange(depIds);
            //过滤特殊字符
            searchRequestDto.ProjectName = Utils.ReplaceSQLChar(searchRequestDto.ProjectName);

            departmentIds.Clear();
            var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();

            var projects = await dbContext.Queryable<Project>()
                .LeftJoin<ProjectStatus>((p, ps) => p.StatusId == ps.StatusId)
                .Where(p => departmentIds.Contains(p.ProjectDept.Value))
                .WhereIF(true, p => p.IsDelete == 1)
                .WhereIF(searchRequestDto.CompanyId != null, p => p.CompanyId == searchRequestDto.CompanyId)
                .WhereIF(searchRequestDto.ProjectDept != null, p => p.ProjectDept == searchRequestDto.ProjectDept)
                .WhereIF(searchRequestDto.ProjectStatusId != null && searchRequestDto.ProjectStatusId.Any(), p => searchRequestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                .WhereIF(searchRequestDto.ProjectRegionId != null, p => p.RegionId == searchRequestDto.ProjectRegionId)
                .WhereIF(searchRequestDto.ProjectTypeId != null, p => p.TypeId == searchRequestDto.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(searchRequestDto.ProjectName), p => SqlFunc.Contains(p.Name, searchRequestDto.ProjectName))
                .WhereIF(searchRequestDto.ProjectAreaId! != null, p => p.AreaId == searchRequestDto.ProjectAreaId)
                .WhereIF(categoryList != null && categoryList.Any(), p => categoryList.Contains(p.Category))
                .WhereIF(tagList != null && tagList.Any(), p => tagList.Contains(p.Tag))
                .WhereIF(tag2List != null && tag2List.Any(), p => tag2List.Contains(p.Tag2))
                .WhereIF(searchRequestDto.Reportformer != null && searchRequestDto.IsReportformerNull == false, p => p.ReportFormer.Contains(searchRequestDto.Reportformer))
                .WhereIF(searchRequestDto.IsReportformerNull, p => p.ReportFormer == "")
                .Select((p, ps) => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortName = p.ShortName,
                    Code = p.Code,
                    Category = p.Category,
                    TypeId = p.TypeId,
                    Amount = p.Amount,
                    CurrencyId = p.CurrencyId,
                    CompanyId = p.CompanyId,
                    ProjectDept = p.ProjectDept,
                    Status = ps.Name,
                    Tag = p.Tag,
                    Tag2 = p.Tag2,
                    CreateTime = p.CreateTime,
                    Sequence = ps.Sequence.Value,
                    CommencementTime = p.CommencementTime,
                    Reportformer = p.ReportFormer,
                    ReportformerTel = p.ReportForMertel
                }).OrderBy("ps.Sequence asc,p.CreateTime desc ").ToPageListAsync(searchRequestDto.PageIndex, searchRequestDto.PageSize, total);

            //获取项目类型Id
            var projectTypeIds = projects.Select(x => x.TypeId).ToList();
            //获取项目币种Id
            var projectCurrencyIds = projects.Select(x => x.CurrencyId).ToList();
            //获取项目公司Id
            var projectCompanyIds = projects.Select(x => x.CompanyId).ToList();

            //获取项目类型
            var projectTypeList = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && projectTypeIds.Contains(x.PomId)).ToListAsync();
            //获取项目币种
            var projectCurrencyList = await dbContext.Queryable<Currency>().Where(x => x.IsDelete == 1 && projectCurrencyIds.Contains(x.PomId)).ToListAsync();
            //获取项目所属公司
            var projectCompanyList = institution.Where(x => x.IsDelete == 1 && projectCompanyIds.Contains(x.PomId)).ToList();

            foreach (var item in projects)
            {
                item.TypeName = projectTypeList.FirstOrDefault(x => x.PomId == item.TypeId)?.Name;
                item.ZCURRENCYNAME = searchRequestDto.IsConvert == true ? "CNY-人民币" : projectCurrencyList.FirstOrDefault(x => x.PomId == item.CurrencyId)?.Zcurrencyname;
                item.CompanyName = projectCompanyList.FirstOrDefault(x => x.PomId == item.CompanyId)?.Name;
            }

            responseAjaxResult.Data = projects;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 修改项目与报表负责人关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UpdateProjectAboutStatementAsync(RetortformerRequestDto searchRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            //var projects = await dbContext.Queryable<Project>().SingleAsync(x => x.IsDelete == 1 && x.ReportFormer == searchRequestDto.Reportformer);
            //if (projects == null)
            //{
            var project = await dbContext.Queryable<Project>().SingleAsync(x => x.IsDelete == 1 && x.Id == searchRequestDto.ProjectId);
            if (project != null)
            {
                project.ReportFormer = searchRequestDto.Reportformer;
                project.ReportForMertel = searchRequestDto.ReportformerTel;
                var result = await dbContext.Updateable(project).ExecuteCommandAsync();
                if (result > 0)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                }
                else
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.UpdateFail);
                }
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
            }
            //}
            //else
            //{
            //    responseAjaxResult.Message = $"{projects.ReportFormer}已在{projects.Name}项目存在,选择其他负责人";
            //    responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_SUCCESS);
            //}
            return responseAjaxResult;
        }

        /// <summary>
        /// 维护分包船舶信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SubShipUserResponseDto>>> SearchSubShipAndUserAsync(SubShipUserRequestDto sub)
        {
            ResponseAjaxResult<List<SubShipUserResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SubShipUserResponseDto>>();
            RefAsync<int> total = 0;
            //分包船舶
            var sublist = await dbContext.Queryable<SubShip>()
             .LeftJoin<ShipPingType>((x, y) => x.TypeId == y.PomId)
             .LeftJoin<ShipClassic>((x, y, z) => x.ClassicId == z.Id)
             .LeftJoin<ShipStatus>((x, y, z, ship) => x.StatusId == ship.Id)

             .WhereIF(!string.IsNullOrWhiteSpace(sub.Name), (x, y, z, ship) => SqlFunc.Contains(x.Name, sub.Name))
             .WhereIF(sub.TypeId != null, (x, y, z, ship) => x.TypeId == sub.TypeId).Where((x, y) => x.IsDelete == 1)
            //.OrderBy(x => x.CreateId, OrderByType.Desc)
            .OrderBy(x => x.CreateTime, OrderByType.Asc)
            .Select((x, y, z, ship) => new SubShipUserResponseDto
            {
                Id = x.Id,
                SubId = x.Id,
                Name = x.Name,
                TypeId = x.TypeId,
                TypeName = y.Name,
                ClassicId = z.Id,
                ClassicName = z.Name,
                CallSign = x.CallSign,
                Mmsi = x.Mmsi,
                Imo = x.Imo,
                RegisterNumber = x.RegisterNumber,
                FisrtRegisterNumber = x.FisrtRegisterNumber,
                IdentNumber = x.IdentNumber,
                RadioNumber = x.RadioNumber,
                InspectNumber = x.InspectNumber,
                InspectType = x.InspectType,
                NationType = x.NationType,
                RegistryPort = x.RegistryPort,
                NavigateArea = x.NavigateArea,
                ConstructionArea = x.ConstructionArea,
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                UserName = x.UserName,
                StatusId = ship.Id,
                StatusName = ship.Name,
                Designer = x.Designer,
                Builder = x.Builder,
                LengthOverall = x.LengthOverall,
                LengthBpp = x.LengthBpp,
                Breadth = x.Breadth,
                Depth = x.Depth,
                LoadedDraft = x.LoadedDraft,
                LightDraft = x.LightDraft,
                LoadedDisplacement = x.LoadedDisplacement,
                LightDisplacement = x.LightDisplacement,
                GrossTonnage = x.GrossTonnage,
                NetTonnage = x.NetTonnage,
                TotalPower = x.TotalPower,
                Endurance = x.Endurance,
                Speed = x.Speed,
                Height = x.Height,
                FinishDate = x.FinishDate,
                Remarks = x.Remarks,
                BusinessUnit = x.BusinessUnit,
                IsUserShip = x.CreateId.HasValue,
            }).ToPageListAsync(sub.PageIndex, sub.PageSize, total);

            if (sublist.Any())
            {
                var ids = sublist.Select(x => x.CompanyId).ToList();
                var allDealngUnitList = await dbContext.Queryable<DealingUnit>().Where(x => ids.Contains(x.PomId.Value.ToString())).ToListAsync();
                foreach (var item in sublist)
                {
                    item.CompanyName = allDealngUnitList.SingleOrDefault(x => x.PomId.Value.ToString() == item.CompanyId)?.ZBPNAME_ZH;
                }
            }

            responseAjaxResult.Data = sublist;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 新增或修改分包船舶信息
        /// </summary>
        /// <param name="subShip"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveSubShipUserAsync(AddSubShipUserRequestDto subShip)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var sub = mapper.Map<AddSubShipUserRequestDto, SubShip>(subShip);
            if (subShip.IsInsert)
            {
                sub.Id = GuidUtil.Next();
                sub.PomId = Guid.NewGuid();//为了不生成36个0是因为前端资源限定唯一性作区分
                var result = await dbContext.Insertable(sub).ExecuteCommandAsync();
                if (result == 1)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
                }
                else
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_FAIL);
                }
            }
            else
            {
                var sinleSubShip = await dbContext.Queryable<SubShip>().SingleAsync(x => x.IsDelete == 1 && x.Id == sub.Id);
                if (sinleSubShip != null)
                {
                    #region 日志信息
                    LogInfo logDto = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        OperationId = _currentUser.Id,
                        BusinessModule = "/装备管理/分包船舶管理/编辑",
                        OperationName = _currentUser.Name,
                        DataId = sinleSubShip.Id
                    };
                    #endregion
                    sub.PomId = sinleSubShip.PomId;
                    var result = await dbContext.Updateable(sub).IgnoreColumns(x => x.CreateId).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                    if (result > 0)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.DeleteFail);
                    }
                }
                else
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                }

            }
            //记录分包船舶的变更
            await entityChangeService.RecordEntitysChangeAsync(EntityType.SubShip, sub.Id);
            return responseAjaxResult;
        }

        /// <summary>
        /// 删除分包船舶
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RemoveSubShipUserAsync(SubShipRequestDto sub)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var sinleSubShip = await dbContext.Queryable<SubShip>().SingleAsync(x => x.IsDelete == 1 && x.Id == sub.Id);
            if (sinleSubShip != null)
            {
                sinleSubShip.IsDelete = 0;
                var result = await dbContext.Updateable(sinleSubShip).ExecuteCommandAsync();
                if (result > 0)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
                }
                else
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DeleteFail);
                }
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
            }

            return responseAjaxResult;
        }

        /// <summary>
		/// 保存项目产值计划
		/// </summary>
		/// <param name="projectPlanRequestDto"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveProjectPlanAsync(SaveProjectPlanRequestDto projectPlanRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (projectPlanRequestDto.Year == 0)
            {
                projectPlanRequestDto.Year = DateTime.Now.Year;
            }
            var model = mapper.Map<SaveProjectPlanRequestDto, ProjectPlanProduction>(projectPlanRequestDto);
            #region 日志信息

            #endregion
            if (projectPlanRequestDto.RequestType)
            {

                model.Id = GuidUtil.Next();
                LogInfo logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    OperationId = _currentUser.Id,
                    BusinessModule = "/进度与成本管控/月度计划产值管理/编辑",
                    OperationName = _currentUser.Name,
                    DataId = model.Id
                };
                var isSave = await dbContext.Insertable(model).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                responseAjaxResult.Data = isSave > 0 ? true : false;
                if (isSave > 0)
                    responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_SUCCESS);
                else
                    responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_SUCCESS, HttpStatusCode.SaveFail);
            }
            else
            {
                var projectPlan = await dbContext.Queryable<ProjectPlanProduction>()
                    .Where(x => x.IsDelete == 1 && x.CompanyId == projectPlanRequestDto.CompanyId && x.ProjectId == projectPlanRequestDto.ProjectId && x.Year == projectPlanRequestDto.Year).FirstAsync();
                if (projectPlan != null)
                {
                    var singleEntity = mapper.Map<SaveProjectPlanRequestDto, ProjectPlanProduction>(projectPlanRequestDto);
                    singleEntity.Id = projectPlan.Id;
                    #region 日志信息
                    LogInfo logDto = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        OperationId = _currentUser.Id,
                        BusinessModule = "/进度与成本管控/月度计划产值管理/编辑",
                        OperationName = _currentUser.Name,
                        DataId = singleEntity.Id
                    };
                    #endregion
                    var isSave = await dbContext.Updateable(singleEntity).EnableDiffLogEvent(logDto).WhereColumns(it => new { it.ProjectId, it.CompanyId, it.Year }).ExecuteCommandAsync();
                    responseAjaxResult.Data = isSave > 0 ? true : false;
                    if (isSave > 0)
                        responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                    else
                        responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.UpdateFail);
                }
                else
                {
                    model.Id = GuidUtil.Next();
                    LogInfo logDto = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        OperationId = _currentUser.Id,
                        BusinessModule = "/进度与成本管控/月度计划产值管理/编辑",
                        OperationName = _currentUser.Name,
                        DataId = model.Id
                    };
                    var isSave = await dbContext.Insertable(model).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                    responseAjaxResult.Data = isSave > 0 ? true : false;
                    if (isSave > 0)
                        responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_SUCCESS);
                    else
                        responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_SUCCESS, HttpStatusCode.SaveFail);
                }
            }

            return responseAjaxResult;
        }

        /// <summary>
        /// 搜索项目计划列表
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<ProjectPlanResponseDto>> SearchProjectPlanAsync(MonthlyPlanRequestDto monthlyPlanRequestDto)
        {
            ResponseAjaxResult<ProjectPlanResponseDto> ajaxResult = new ResponseAjaxResult<ProjectPlanResponseDto>();
            ProjectPlanResponseDto projectPlans = new ProjectPlanResponseDto();
            RefAsync<int> total = 0;
            var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            var departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            var institutionList = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();

            var result = await dbContext.Queryable<Project>()
               .LeftJoin<ProjectPlanProduction>((x, y) => x.Id == y.ProjectId && y.Year == DateTime.Now.Year)
               .Where(x => x.IsDelete == 1 && departmentIds.Contains(x.ProjectDept.Value))
               .WhereIF(monthlyPlanRequestDto.CompanyId != null, (x, y) => x.CompanyId == monthlyPlanRequestDto.CompanyId)
               .WhereIF(!string.IsNullOrWhiteSpace(monthlyPlanRequestDto.KeyWords), (x, y) => x.Name.Contains(monthlyPlanRequestDto.KeyWords))
               .OrderBy(x => x.CompanyId)
               .Select((x, y) => new ProjectPlanSearchInfo()
               {
                   ProjectId = x.Id,
                   Year = y.Year,
                   CompanyId = x.CompanyId.Value,
                   Name = x.Name,
                   OnePlanProductionValue = y.OnePlanProductionValue.Value,
                   TwoPlanProductionValue = y.TwoPlanProductionValue.Value,
                   ThreePlanProductionValue = y.ThreePlanProductionValue.Value,
                   FourPlanProductionValue = y.FourPlanProductionValue.Value,
                   FivePlanProductionValue = y.FivePlanProductionValue.Value,
                   SixPlanProductionValue = y.SixPlanProductionValue.Value,
                   SevenPlanProductionValue = y.SevenPlanProductionValue.Value,
                   EightPlanProductionValue = y.EightPlanProductionValue.Value,
                   NinePlanProductionValue = y.NinePlanProductionValue.Value,
                   TenPlanProductionValue = y.TenPlanProductionValue.Value,
                   ElevenPlanProductionValue = y.ElevenPlanProductionValue.Value,
                   TwelvePlanProductionValue = y.TwelvePlanProductionValue.Value
               }).ToListAsync();
            foreach (var item in result)
            {
                item.CompanyName = institutionList.Where(x => x.PomId == item.CompanyId).FirstOrDefault() == null ? "" : institutionList.Where(x => x.PomId == item.CompanyId).FirstOrDefault().Shortname;
            }

            var list = new List<ProjectPlanSearchInfo>();
            if (monthlyPlanRequestDto.IsFullExport)
            {
                list = result;
                total = list.Count;
            }
            else
            {
                var planList = result;
                list = result.Skip((monthlyPlanRequestDto.PageIndex - 1) * monthlyPlanRequestDto.PageSize).Take(monthlyPlanRequestDto.PageSize).ToList();
                total = planList.Count;
            }
            projectPlans.projectSearchInfos = list;
            projectPlans.sumProjectPlanInfo = GetSumProjectPlan(result);
            ajaxResult.Count = total;
            ajaxResult.Data = projectPlans;
            ajaxResult.Success();
            return ajaxResult;
        }

        /// <summary>
        /// 获取项目月度产值计划合计
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public SumProjectPlanInfo GetSumProjectPlan(List<ProjectPlanSearchInfo> infos)
        {
            SumProjectPlanInfo sumProjectPlanInfo = new SumProjectPlanInfo();
            sumProjectPlanInfo.SumOnePlanProductionValue = infos.Sum(x => x.OnePlanProductionValue);
            sumProjectPlanInfo.SumTwoPlanProductionValue = infos.Sum(x => x.TwoPlanProductionValue);
            sumProjectPlanInfo.SumThreePlanProductionValue = infos.Sum(x => x.ThreePlanProductionValue);
            sumProjectPlanInfo.SumFourPlanProductionValue = infos.Sum(x => x.FourPlanProductionValue);
            sumProjectPlanInfo.SumFivePlanProductionValue = infos.Sum(x => x.FivePlanProductionValue);
            sumProjectPlanInfo.SumSixPlanProductionValue = infos.Sum(x => x.SixPlanProductionValue);
            sumProjectPlanInfo.SumSevenPlanProductionValue = infos.Sum(x => x.SevenPlanProductionValue);
            sumProjectPlanInfo.SumEightPlanProductionValue = infos.Sum(x => x.EightPlanProductionValue);
            sumProjectPlanInfo.SumNinePlanProductionValue = infos.Sum(x => x.NinePlanProductionValue);
            sumProjectPlanInfo.SumTenPlanProductionValue = infos.Sum(x => x.TenPlanProductionValue);
            sumProjectPlanInfo.SumElevenPlanProductionValue = infos.Sum(x => x.ElevenPlanProductionValue);
            sumProjectPlanInfo.SumTwelvePlanProductionValue = infos.Sum(x => x.TwelvePlanProductionValue);

            return sumProjectPlanInfo;
        }

        /// <summary>
        /// 查询项目计划详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectPlanDetailResponseDto>> SearchProjectPlanDetailAsync(ProjectPlanSearchRequestDto requestDto)
        {
            ResponseAjaxResult<ProjectPlanDetailResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectPlanDetailResponseDto>();
            ProjectPlanDetailResponseDto projectPlans = new ProjectPlanDetailResponseDto();
            RefAsync<int> total = 0;
            if (requestDto.Year == 0)
            {
                requestDto.Year = DateTime.Now.Year;
            }
            //查询该项目下的年度计划
            var projectPlanList = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1 && x.ProjectId == requestDto.ProjectId).OrderByDescending(x => x.Year).ToListAsync();
            if (projectPlanList.Any())
            {
                projectPlans.ProjectId = projectPlanList.First().ProjectId;
                foreach (var item in projectPlanList)
                {
                    ProjectYearInfo projectYearInfo = new ProjectYearInfo();
                    projectYearInfo.Year = item.Year;
                    projectYearInfo.YearPlanActual =
                         (item.OnePlanProductionValue ?? 0) + (item.TwoPlanProductionValue ?? 0) + (item.ThreePlanProductionValue ?? 0) +
                         (item.FourPlanProductionValue ?? 0) + (item.FivePlanProductionValue ?? 0) + (item.SixPlanProductionValue ?? 0) +
                         (item.SevenPlanProductionValue ?? 0) + (item.EightPlanProductionValue ?? 0) + (item.NinePlanProductionValue ?? 0) +
                         (item.TenPlanProductionValue ?? 0) + (item.ElevenPlanProductionValue ?? 0) + (item.TwelvePlanProductionValue ?? 0);
                    projectPlans.projectYearInfos.Add(projectYearInfo);
                }
                var projectPlan = projectPlanList.Where(x => x.Year == requestDto.Year).FirstOrDefault();
                if (projectPlan != null)
                {
                    projectPlans.projectMonthInfos.Year = projectPlan.Year;
                    projectPlans.projectMonthInfos.OnePlanProductionValue = projectPlan.OnePlanProductionValue;
                    projectPlans.projectMonthInfos.TwoPlanProductionValue = projectPlan.TwoPlanProductionValue;
                    projectPlans.projectMonthInfos.ThreePlanProductionValue = projectPlan.ThreePlanProductionValue;
                    projectPlans.projectMonthInfos.FourPlanProductionValue = projectPlan.FourPlanProductionValue;
                    projectPlans.projectMonthInfos.FivePlanProductionValue = projectPlan.FivePlanProductionValue;
                    projectPlans.projectMonthInfos.SixPlanProductionValue = projectPlan.SixPlanProductionValue;
                    projectPlans.projectMonthInfos.SevenPlanProductionValue = projectPlan.SevenPlanProductionValue;
                    projectPlans.projectMonthInfos.EightPlanProductionValue = projectPlan.EightPlanProductionValue;
                    projectPlans.projectMonthInfos.NinePlanProductionValue = projectPlan.NinePlanProductionValue;
                    projectPlans.projectMonthInfos.TenPlanProductionValue = projectPlan.TenPlanProductionValue;
                    projectPlans.projectMonthInfos.ElevenPlanProductionValue = projectPlan.ElevenPlanProductionValue;
                    projectPlans.projectMonthInfos.TwelvePlanProductionValue = projectPlan.TwelvePlanProductionValue;
                }
            }
            projectPlans.projectYearInfos = projectPlans.projectYearInfos.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Data = projectPlans;
            responseAjaxResult.Count = projectPlanList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 保存项目产值计划excel导入的数据
        /// </summary>
        /// <param name="projectPlanInfos"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveProjectPlanExcelImportAsync(List<ProjectPlanInfo> projectPlanInfos)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            List<ProjectPlanProduction> addList = new List<ProjectPlanProduction>();
            List<ProjectPlanProduction> updateList = new List<ProjectPlanProduction>();
            if (projectPlanInfos.Any())
            {
                var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();
                var projectPlan = await dbContext.Queryable<ProjectPlanProduction>().Where(x => x.IsDelete == 1).ToListAsync();
                foreach (var item in projectPlanInfos)
                {
                    ProjectPlanProduction model = new ProjectPlanProduction();
                    mapper.Map(item, model);
                    model.ProjectId = projectList.Where(x => x.Id == item.ProjectId).FirstOrDefault().Id;
                    model.CompanyId = projectList.Where(x => x.Id == item.ProjectId).FirstOrDefault().CompanyId.Value;
                    if (model.Year == 0)
                    {
                        model.Year = DateTime.Now.Year;
                    }
                    var projectPlanFirst = projectPlan.Where(x => x.ProjectId == model.ProjectId && x.Year == model.Year).FirstOrDefault();
                    if (projectPlanFirst != null)
                    {
                        model.Id = projectPlanFirst.Id;
                        updateList.Add(model);
                    }
                    else
                    {
                        model.Id = GuidUtil.Next();
                        addList.Add(model);
                    }
                }
                if (addList.Any())
                {
                    await dbContext.Insertable(addList).InsertColumns(it => new { it.Id, it.ProjectId, it.CompanyId, it.Year, it.OnePlanProductionValue, it.TwoPlanProductionValue, it.ThreePlanProductionValue, it.FourPlanProductionValue, it.FivePlanProductionValue, it.SixPlanProductionValue, it.SevenPlanProductionValue, it.EightPlanProductionValue, it.NinePlanProductionValue, it.TenPlanProductionValue, it.ElevenPlanProductionValue, it.TwelvePlanProductionValue }).ExecuteCommandAsync();
                }
                if (updateList.Any())
                {
                    await dbContext.Updateable(updateList).UpdateColumns(it => new { it.ProjectId, it.CompanyId, it.Year, it.OnePlanProductionValue, it.TwoPlanProductionValue, it.ThreePlanProductionValue, it.FourPlanProductionValue, it.FivePlanProductionValue, it.SixPlanProductionValue, it.SevenPlanProductionValue, it.EightPlanProductionValue, it.NinePlanProductionValue, it.TenPlanProductionValue, it.ElevenPlanProductionValue, it.TwelvePlanProductionValue }).ExecuteCommandAsync();
                }
            }
            responseAjaxResult.Data = true;
            return responseAjaxResult;
        }


        /// <summary>
        /// 查询公司在手项目清单
        /// </summary>
        /// <param name="companyProjectDetailsdRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>> SearchCompanyProjectListAsync(CompanyProjectDetailsdRequestDto companyProjectDetailsdRequestDto)
        {
            ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>();
            //项目状态ID 
            var projectStatusIds = CommonData.NoStatus.Split(",").Select(x => x.ToGuid()).ToList();
            var result = await dbContext.Queryable<Project>()
                .LeftJoin<ProductionMonitoringOperationDayReport>((x, y) => x.CompanyId == y.ItemId &&
                y.Type == 1 && y.Collect == 0 && !string.IsNullOrWhiteSpace(y.Name))
                .LeftJoin<ProjectType>((x, y, z) => x.TypeId == z.PomId)
                .LeftJoin<ProjectStatus>((x, y, z, a) => x.StatusId == a.StatusId)
                 .WhereIF(!string.IsNullOrWhiteSpace(companyProjectDetailsdRequestDto.ProjectName), (x, y, z, a) => x.Name.Contains(companyProjectDetailsdRequestDto.ProjectName))
                .WhereIF(companyProjectDetailsdRequestDto.StatusId != null, (x, y, z, a) => a.StatusId == companyProjectDetailsdRequestDto.StatusId)
                .WhereIF(companyProjectDetailsdRequestDto.TypeId != null, (x, y, z, a) => x.TypeId == companyProjectDetailsdRequestDto.TypeId)
                .WhereIF(companyProjectDetailsdRequestDto.CompanyId != null, (x, y, z, a) => x.CompanyId == companyProjectDetailsdRequestDto.CompanyId)
                //.WhereIF(companyProjectDetailsdRequestDto.CommencementDate!=null, (x, y, z, a)=>x.CommencementTime== companyProjectDetailsdRequestDto.CompanyId)
                .Where(x => x.IsDelete == 1 && !projectStatusIds.Contains(x.StatusId.Value) && x.TypeId != "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid())
                .OrderBy((x, y, z, a) => new { y.Sort, a.Sequence })
                .Select((x, y, z, a) => new CompanyProjectDetailedResponseDto
                {
                    Id = x.Id,
                    ProjectName = x.Name,
                    CompanyId = y.ItemId.Value,
                    CompanyName = y.Name,
                    StatusSort = a.Sequence.Value,
                    StatusName = a.Name,
                    TypeName = z.Name,
                    ContractAmount = x.ECAmount.Value * x.ExchangeRate.Value,
                    CommencementDate = x.CommencementTime.Value.ToString(),

                }).ToListAsync();
            #region 计算项目累计产值
            //查询历史产值
            var historyProjectList = await dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //2023-7-至今
            var currentYear = DateTime.Now.Year;
            var projectIds = result.Select(x => x.Id).ToList();
            var currentTotalYearOffirmProductionValue = await dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId) && x.DateYear <= currentYear).ToListAsync();
            foreach (var item in result)
            {
                item.Remark = item.CommencementDate?.IndexOf(currentYear.ToString()) >= 0 ? "新中标" : "";
                if (item.ContractAmount != 0)
                {
                    //计算工程进度
                    item.ProjectProgress = Math.Round(
                        (GetProjectTotalProductionValue(item.Id, historyProjectList, currentTotalYearOffirmProductionValue) / item.ContractAmount) * 100, 2);
                }
                item.ContractAmount = Math.Round(item.ContractAmount / 10000, 2);

            }
            #endregion

            responseAjaxResult.Data = result;
            responseAjaxResult.Count = result.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 计算项目累计产值
        /// </summary>
        /// <param name="id">项目iD</param>
        /// <param name="data">历史数据</param>
        /// <param name="currentTotalYearOffirmProductionValue">每月完成产值</param>
        /// <returns></returns>
        public static decimal GetProjectTotalProductionValue(Guid id, List<ProjectHistoryData> data, List<MonthReport> currentTotalYearOffirmProductionValue)
        {
            //var projectSingleProductionValue = data.Where(x => x.ProjectId == id).SingleOrDefault();
            //var totalYearKaileaOffirmProductionValue = projectSingleProductionValue?.AccumulatedOutputValue ?? 0 +
            //    currentTotalYearOffirmProductionValue.Where(x => x.ProjectId == id).Sum(x => x.CurrencyCompleteProductionAmount);
            //return totalYearKaileaOffirmProductionValue;
            var totalYearKaileaOffirmProductionValue =
                currentTotalYearOffirmProductionValue.Where(x => x.ProjectId == id).Sum(x => x.CompleteProductionAmount);
            return totalYearKaileaOffirmProductionValue;
        }

        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyProjectPullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            responseAjaxResult.Data = (await dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(x => x.IsDelete == 1 && x.Type == 1 && x.Collect == 0 && x.Name != null)
                .OrderBy(x => x.Sort.Value)
                .Select(x => new BasePullDownResponseDto()
                {
                    Id = x.ItemId,
                    Name = x.Name,
                    Type = x.Sort.Value
                }).ToListAsync());
            responseAjaxResult.Count = responseAjaxResult.Data.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        public async Task<ResponseAjaxResult<List<StartWorkResponseDto>>> SearchStartListAsync(Guid projectid, int pageIndex, int pageSize)
        {
            ResponseAjaxResult<List<StartWorkResponseDto>> responseAjaxResult = new
                ();
            List<StartWorkResponseDto> startWorkResponseDtos = new();
            RefAsync<int> total = 0;
            //项目状态表
            var projectStatus = await dbContext.Queryable<ProjectStatus>().Where(x => x.IsDelete == 1).Select(x => new { x.StatusId, x.Name }).ToListAsync();
            startWorkResponseDtos = await dbContext.Queryable<StartWorkRecord>()
                .LeftJoin<Project>((sw, pro) => sw.ProjectId == pro.Id)
                .LeftJoin<Institution>((sw, pro, ins) => pro.CompanyId == ins.PomId)
                .Where((sw, pro, ins) => sw.IsDelete == 1 && projectid == sw.ProjectId)
                .OrderByDescending((sw, pro, ins) => sw.CreateTime)
                .Select((sw, pro, ins) => new StartWorkResponseDto()
                {
                    EndWorkTime = sw.EndWorkTime,
                    Name = sw.Name,
                    ProjectId = sw.ProjectId,
                    StartWorkTime = sw.StartWorkTime,
                    StopWorkReason = sw.StopWorkReson,
                    UpdateUser = sw.UpdateUser,
                    CompanyName = ins.Shortname,
                    AfterStatusId = sw.AfterStatus,
                    BeforeStatusId = sw.BeforeStatus,
                    UpdateTime = sw.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                })
                 .ToPageListAsync(pageIndex, pageSize, total);

            foreach (var item in startWorkResponseDtos)
            {
                item.AfterStatus = projectStatus.FirstOrDefault(x => x.StatusId.ToString() == item.AfterStatusId)?.Name;
                item.BeforeStatus = projectStatus.FirstOrDefault(x => x.StatusId.ToString() == item.BeforeStatusId)?.Name;
            }

            responseAjaxResult.Data = startWorkResponseDtos;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 查询船舶进退场记录
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ShipMovementRecordResponseDto>>> SearchShipMovementAsync(Guid shipId, int pageIndex, int pageSize)
        {
            ResponseAjaxResult<List<ShipMovementRecordResponseDto>> responseAjaxResult = new();
            RefAsync<int> total = 0;
            var subShipList = await dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1 && x.PomId == shipId).Select(x => new { ShipId = x.PomId, Name = x.Name }).ToListAsync();
            var ownerShipList = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && x.PomId == shipId).Select(x => new { ShipId = x.PomId, Name = x.Name }).ToListAsync();
            foreach (var item in ownerShipList)
            {
                var obj = new
                {
                    ShipId = item.ShipId,
                    Name = item.Name
                };
                subShipList.Add(obj);
            }
            var res = await dbContext.Queryable<ShipMovementRecord>().Where(x => x.IsDelete == 1 && x.ShipId == shipId)
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .Where((x, y) => y.IsDelete == 1)
                .Select((x, y) => new ShipMovementRecordResponseDto()
                {
                    ProjectId = x.ProjectId,
                    ProjectName = y.ShortName,
                    EnterTime = x.EnterTime,
                    QuitTime = x.QuitTime,
                    Status = x.Status,
                    ShipMovementId = x.ShipMovementId,
                    ShipId = x.ShipId,
                    Remark = x.Remark

                }).ToPageListAsync(pageIndex, pageSize, total);

            foreach (var item in res)
            {
                item.ShipName = subShipList.Where(x => x.ShipId == x.ShipId).Select(x => x.Name).FirstOrDefault();
            }
            responseAjaxResult.Data = res;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        /// <summary>
        /// 撤回项目月报
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> RevocationProjectMonthAsync(Guid id)
        {
            ResponseAjaxResult<bool> ajaxResult = new ResponseAjaxResult<bool>();
            //#region 时间判断
            ////查询当前是第几月
            //var nowYear = DateTime.Now.Year;
            //var nowMonth = DateTime.Now.Month;
            //var nowDay = DateTime.Now.Day;
            //var monthDay = 0;
            //if (nowDay > 26)
            //{
            //    nowMonth += 1;
            //}
            //if (int.Parse(DateTime.Now.ToString("MMdd")) > 1226)
            //{
            //    nowYear += 1;
            //}
            //if (nowMonth.ToString().Length == 1)
            //{
            //    monthDay = int.Parse(nowYear + $"0{nowMonth}");
            //}
            //else
            //{
            //    monthDay = int.Parse(nowYear + $"{nowMonth}");
            //}
            //#endregion

            var projectMonth = await dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.Id == id).FirstAsync();

            if (projectMonth == null)
            {
                ajaxResult.Data = false;
                ajaxResult.FailResult(HttpStatusCode.DataNotEXIST, "数据不存在");
                return ajaxResult;
            }

            #region 新
            var monthDay = 0;
            if (projectMonth != null)
            {
                //今天
                ConvertHelper.TryConvertDateTimeFromDateDay(Convert.ToInt32(projectMonth.DateMonth.ToString() + DateTime.Now.Day), out DateTime nowDateTime);
                //如果日期>=26  小于等于下月一号
                if (nowDateTime.Day >= 26 || (nowDateTime.Day >= 26 && nowDateTime <= Convert.ToDateTime(nowDateTime.AddMonths(1).ToString("yyyy-MM-01"))))
                {
                    monthDay = nowDateTime.ToDateMonth();
                }
            }
            #endregion

            var job = await dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.Job>().Where(x => x.IsDelete == 1
             && x.ProjectId == projectMonth.ProjectId && x.DateMonth == monthDay && x.ApproveLevel != ApproveLevel.Level2).FirstAsync();
            if (job == null)
            {
                ajaxResult.Data = false;
                ajaxResult.Success();
                return ajaxResult;
            }

            var jobApprover = await dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.JobApprover>().Where(x => x.IsDelete == 1
          && x.JobId == job.Id).FirstAsync();
            if (jobApprover == null)
            {
                ajaxResult.Data = false;
                ajaxResult.Success();
                return ajaxResult;
            }
            //
            var jobRecord = await dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.JobRecord>().Where(x => x.IsDelete == 1
           && x.JobId == job.Id).FirstAsync();
            if (jobRecord == null)
            {
                ajaxResult.Data = false;
                ajaxResult.Success();
                return ajaxResult;
            }

            //修改审批状态
            //job.IsFinish = true;
            //job.FinishApproveLevel = ApproveLevel.Level2;
            //job.ApproveStatus = JobApproveStatus.Revoca;
            //jobRecord.ApproveStatus= JobApproveStatus.Revoca;
            job.IsDelete = 0;
            jobApprover.IsDelete = 1;
            jobRecord.IsDelete = 1;
            projectMonth.Status = MonthReportStatus.Revoca;
            projectMonth.StatusText = "已撤回";
            await dbContext.Updateable(projectMonth).ExecuteCommandAsync();
            await dbContext.Updateable(job).ExecuteCommandAsync();
            await dbContext.Updateable(jobRecord).ExecuteCommandAsync();
            await dbContext.Updateable(jobApprover).ExecuteCommandAsync();
            ajaxResult.Data = true;
            ajaxResult.Success();
            return ajaxResult;
        }

        public bool aa()
        {
            var str = "087db4dd-5366-48f8-96ac-a7675b2fcd71,660;08db3b35-fb38-4bba-8e23-9070f6a03203,365;08db3b35-fb38-4bbb-867b-f4c153c956af,420;08db3b35-fb38-4bc0-8c22-01063508e54f,1080;08db3b35-fb38-4bc1-8cff-b6b61baf8d53,910;08db3b35-fb38-4bc8-83b9-c067667a0662,365;08db3b35-fb38-4bc8-871c-83f3ed8494d1,730;08db3b35-fb38-4bca-83df-b295051faa1e,2922;08db3b35-fb38-4bce-8861-d043bc2448aa,540;08db3b35-fb38-4bcf-8e64-6381009800a0,730;08db3b35-fb38-4bd0-81c5-256ccfaf1104,720;08db3b35-fb38-4bd2-84ca-4d9c8e2767ea,300;08db3b35-fb38-4bd2-87f4-92d45fb1b837,730;08db3b35-fb38-4bd3-85a1-a99172fb7884,1095;08db3b35-fb38-4bd4-8005-0772d187e107,730;08db3b35-fb38-4bd4-8c9d-e1ca444a050e,395;08db3b35-fb38-4bd5-8358-3db344bfa4dc,490;08db3b35-fb38-4bd5-8b6f-17b516463587,1095;08db3b35-fb38-4bd5-8ea2-3df8a3a49e1c,1095;08db3b35-fb38-4bd7-8839-a36e77dfa19a,540;08db3b35-fb38-4bd8-83d2-9621a5467d6f,130;08db3b35-fb38-4bda-806c-52bf562a880a,365;08db3b35-fb38-4bda-8f1e-6445cdeeed5a,600;08db3b35-fb38-4bdb-8edc-1baf7dd64191,365;08db3b35-fb38-4bdc-82a7-a3fd1867dfd1,540;08db3b35-fb38-4bdc-893d-c3b42e68cd9b,1080;08db3b35-fb38-4bdd-85d8-573caadc18b1,1333;08db3b35-fb38-4bdd-8c93-aafd432a41f5,190;08db3b35-fb38-4bde-80f5-eae129aa8e85,3650;08db3b35-fb38-4bde-8b60-c9cb444c66f7,481;08db3b35-fb38-4bdf-8d59-bb5f13e280d0,429;08db3b35-fb38-4be1-85ed-31b49d26438f,600;08db3b35-fb38-4be2-8000-f46bb5d64fd8,360;08db3b35-fb38-4be2-8845-449842bf7d93,196;08db3b35-fb38-4be3-896d-b174b155113e,1080;08db3b35-fb38-4be4-8549-985b98c7fdb7,365;08db3b35-fb38-4be4-8972-791700b544d2,900;08db3b35-fb38-4be4-8d14-33581ab8f6ad,1050;08db3b35-fb38-4be5-89c7-a4ef00f21167,402;08db3b35-fb38-4be5-8db1-39db1ef97f9f,1080;08db3b35-fb38-4be6-8482-2e99c2ae1bff,395;08db3b35-fb38-4be6-88f7-10fa459fd124,245;08db3b35-fb38-4be8-813b-1942519277c6,640;08db3b35-fb38-4be9-805f-9bfbce952fb8,730;08db3b35-fb38-4be9-84b8-5b90c10f9060,913;08db3b35-fb38-4be9-8815-483744dec8e4,540;08db3b35-fb38-4be9-8f98-365fff1041e1,1095;08db3b35-fb38-4bea-82b1-48dc8cce7ca5,240;08db3b35-fb38-4bea-86ae-8892facf0d33,180;08db3b35-fb38-4bea-8ab5-3d7b1aabc967,1440;08db3b35-fb38-4bea-8ef7-94a1cd48e4bd,730;08db3b35-fb38-4beb-856e-c061e00b7e57,365;08db3b35-fb38-4beb-8b0a-be38c637a1c0,90;08db3b35-fb38-4beb-8f31-7a1654ce60d2,1080;08db3b35-fb38-4bec-83ab-07f8acd848b1,385;08db7eaf-15db-47f3-8df2-1f46fea0e881,500;08db80ea-6e06-4b54-8455-061f6f1eef90,90;08db8665-81e2-4fdf-8099-e57bdde04e90,600;08db8ce3-8f90-49f6-8c69-56587a073e1d,1095;08db8e02-22fc-446e-8995-f420831653c4,720;08db9c8c-c2b3-4aef-8398-c423594da07a,20;08db9dfc-ca98-43c0-84c0-d50b028a9c7c,80;08db9f9a-77b9-4414-87dd-46ac7cc0c27b,300;08dbaf6e-40cc-4164-869a-6fb218965f44,213;08dbb260-b460-49bd-8006-07f40c301b95,730;08dbc93a-4536-4ee3-83f2-d63866bbdd1e,179;08dbc96f-e397-468f-87ef-4e76075ee535,120;08dbd92f-d27d-4cbf-896d-73ef7dc82740,77;08dbd930-8d07-4ae9-8cc6-be1601c03947,730;08dbde9a-0611-410d-8fc7-c45c9637e236,60;08dbea6e-58e8-44b6-8671-41922a3d1989,500;08dbf0b7-347a-49c7-8934-94f61d2e7d35,180;08dbf0b8-fa3c-497d-8aca-c8be3a4f869d,120;08dbf175-f7fc-4085-8843-cf9842c2cb4b,70;08dbf60d-8127-4a71-84d8-85979c96192d,720;08dc02d2-cb09-48a8-8afc-89416b1aff6a,183;08dc03ba-a326-43a2-8f53-3e0276894d73,240;08dc04fe-2b63-4636-816e-5b967d881645,120;08dc051f-abad-49ab-88a7-33d97c084c8e,90;08dc0bf9-e917-412e-8fcd-03efc8158bda,150;08dc10ba-292f-4e35-8919-393ec7692fa8,730;08dc1676-2c4a-4b34-833f-293646af3418,720;08dc1c87-0efa-4d20-87cf-d7f7de24f4f9,2008;08dc1d72-d0ea-4fc4-890f-fd791594a13a,240;08dc1d82-f77a-45d7-82fb-25ec62271d45,180;08dc1d85-d5e2-4d2e-89f6-dceedf6a3440,240;08dc1d86-947c-49e6-827e-019528d46a32,120;08dc1fdf-3c11-4785-8c98-34ca4e3663c2,380;08dc1fdf-e3c6-463f-8c70-d56dcbc557e4,20;08dc217c-3fbd-4be6-8783-efa481396268,364;08dc3dac-49e1-4b30-87fe-c13b3efa19d9,90;08dc4169-09d3-466c-859f-6b52b5999bfe,1440;08dc4242-4a24-4e72-8ec1-70ac66af0d5f,344;08dc4312-78ff-4c4c-8c36-0cd586b6fcd5,540;08dc432c-5057-477e-8554-183f7a3cf4c4,540;08dc4358-579b-4bd5-823c-186e946f2c17,365;08dc4632-246e-4852-84fe-82031c6f34b9,30;08dc4892-6112-4b5d-8a02-53b9da0ccdfb,1095;08dc48d2-81c6-477d-8d6e-914015773e32,360;08dc4a10-c76f-4fd6-8e6f-1e511ded8dc7,150;08dc4ec7-9698-407b-8309-051c37d6edc3,420;08dc4f8e-aa88-401b-8fe0-175d383b297b,200;08dc507d-9d6f-4a8e-841b-e0a881791414,420;08dc52ef-c1bc-4002-80ec-9b9acf41a294,60;08dc6e42-8a87-4395-865d-bf4d84c38078,390;08dc7667-136c-426d-8bc3-2e78b502cf77,30;08dc7e21-2e0a-4cdd-8e81-2350000a5037,720;08dc7f80-a163-4d06-8954-9ff7f9019784,365;08dc8c50-8e3a-4738-8e78-25c6b003d479,60;08dca18c-759b-4e80-8210-cd7fd485073d,913;08dca9f7-c85c-4a1f-81f5-03c03c233b7b,210;08dcad3a-4ced-4f37-8b4f-e78d7894374e,360;08dcb6a2-50ae-4fcd-80b8-233a5ea54f66,1825;08dcb850-871a-437e-89b5-d5325ce03486,365;08dcbb3b-bb96-4bd0-83ef-f37a5b2582db,30;08dcbb45-2fea-4ec2-865d-c39988e1dd77,480;219e55e0-1237-44bf-8752-311ede2ec6e8,365;2cb8d03c-9f6d-4339-9e4c-c21ff68a3aa8,150;4c0c9417-e4cb-4d73-9c21-80cdfabfcee4,120;adf6e450-7b05-49ba-b27b-eff4e207c804,150;b4003549-4fe6-48b6-8b77-9e77ecc43e07,730;d00376b9-f050-4f9e-a16b-7e4f79ba6da1,300;f3320af2-c127-4f2e-8baf-74ae8d0dccd5,540;";

            var dic = new List<Dictionary<Guid, string>>();
            var strs = str.Split(';').ToList();
            var gid = new List<Guid>();
            foreach (var item in strs)
            {
                if (item != "")
                {
                    var d = item.Split(',');
                    dic.Add(new Dictionary<Guid, string> { { d[0].ToGuid(), d[1] } });
                    gid.Add(d[0].ToGuid());
                }
            }
            var pt = dbContext.Queryable<Project>().Where(x => gid.Contains(x.Id)).ToList();

            var projes = new List<Project>();
            foreach (var item in dic)
            {
                var d = item.ToList();
                foreach (var item2 in d)
                {
                    var ex = pt.FirstOrDefault(x => x.Id == item2.Key);
                    if (ex != null)
                    {
                        ex.WorkDay = Convert.ToInt32(item2.Value);
                        projes.Add(ex);
                    }
                }
            }
            baseProjects.InsertOrUpdateAsync(projes);
            return true;
        }
        /// <summary>
        /// 删除wbs   校验 是否填写过 月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeleteProjectWBSTreeValidatableAsync(DeleteProjectWBSValidatableDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            if (requestDto.Ids != null && requestDto.Ids.Any())
            {
                if (!string.IsNullOrWhiteSpace(requestDto.ProjectId))
                {
                    var pId = requestDto.ProjectId.ToGuid();

                    //查询月报
                    var mRep = await dbContext.Queryable<MonthReportDetail>()
                        .Where(x => x.IsDelete == 1 && x.ProjectId == pId && requestDto.Ids.Contains(x.ProjectWBSId.ToString()))
                        .ToListAsync();

                    if (mRep != null && mRep.Any())
                    {
                        //存在月报 不允许删除
                        //查询wbs树
                        var pwbsTree = await dbContext.Queryable<ProjectWBS>()
                            .Where(x => x.IsDelete == 1 && x.ProjectId == requestDto.ProjectId && mRep.Select(m => m.ProjectWBSId).Distinct().Contains(x.Id))
                            .ToListAsync();

                        string msg = string.Empty;
                        foreach (var item in pwbsTree)
                        {
                            msg += item.Name + "</br>";
                        }
                        responseAjaxResult.FailResult(HttpStatusCode.DeleteFail, "wbs：</br>" + msg + "</br>曾经填过月报，不可删除！！！");
                        return responseAjaxResult;
                    }
                }
            }

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 历史产值月报列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<HistoryProjectMonthReportResponseDto>>> SearchHistoryProjectMonthRepAsync(HistoryProjectMonthReportRequestDto requestBody)
        {
            ResponseAjaxResult<List<HistoryProjectMonthReportResponseDto>> rt = new();
            RefAsync<int> total = 0;

            //complete
            var completeData = await dbContext.Queryable<ExcelProductionConvertTable>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.KeyWords), x => x.Name.Contains(requestBody.KeyWords))
                .WhereIF(requestBody.DateYear != 0, x => x.Year == requestBody.DateYear)
                .Select(x => new HistoryProjectMonthReportResponseDto
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.Name,
                    Year = x.Year
                })
                .OrderByDescending(x => x.Year)
                .Distinct()
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            var pIds = completeData.Select(x => x.ProjectId).ToList();
            var data = await dbContext.Queryable<ExcelProductionConvertTable>()
                .Where(x => pIds.Contains(x.ProjectId))
                .ToListAsync();
            data = data.OrderByDescending(x => x.Year).ThenBy(x => x.Name).ToList();
            foreach (var item in completeData)
            {
                decimal? totalCompleteValue = 0M;//年累
                decimal? one = 0M;
                decimal? two = 0M;
                decimal? three = 0M;
                decimal? four = 0M;
                decimal? five = 0M;
                decimal? six = 0M;
                decimal? seven = 0M;
                decimal? eight = 0M;
                decimal? nine = 0M;
                decimal? ten = 0M;
                decimal? ele = 0M;
                decimal? twe = 0M;
                for (int i = 1; i <= 12; i++)
                {
                    var md = int.Parse(item.Year + (i < 10 ? "0" + i : i.ToString()));
                    item.CompleteId = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId).Id;
                    switch (i)
                    {
                        case 1:
                            one = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.OneCompleteValue;
                            item.OneCompleteValue = one;
                            totalCompleteValue += one;
                            break;
                        case 2:
                            two = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.TwoCompleteValue;
                            item.TwoCompleteValue = two;
                            totalCompleteValue += two;
                            break;
                        case 3:
                            three = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.ThreeCompleteValue;
                            item.ThreeCompleteValue = three;
                            totalCompleteValue += three;
                            break;
                        case 4:
                            four = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.FourCompleteValue;
                            item.FourCompleteValue = four;
                            totalCompleteValue += four;
                            break;
                        case 5:
                            five = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.FiveCompleteValue;
                            item.FiveCompleteValue = five;
                            totalCompleteValue += five;
                            break;
                        case 6:
                            six = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.SixCompleteValue;
                            item.SixCompleteValue = six;
                            totalCompleteValue += six;
                            break;
                        case 7:
                            seven = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.SevenCompleteValue;
                            item.SevenCompleteValue = seven;
                            totalCompleteValue += seven;
                            break;
                        case 8:
                            eight = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.EightCompleteValue;
                            item.EightCompleteValue = eight;
                            totalCompleteValue += eight;
                            break;
                        case 9:
                            nine = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.NineCompleteValue;
                            item.NineCompleteValue = nine;
                            totalCompleteValue += nine;
                            break;
                        case 10:
                            ten = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.TenCompleteValue;
                            item.TenCompleteValue = ten;
                            totalCompleteValue += ten;
                            break;
                        case 11:
                            ele = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.ElevenCompleteValue;
                            item.ElevenCompleteValue = ele;
                            totalCompleteValue += ele;
                            break;
                        case 12:
                            twe = data.FirstOrDefault(x => x.DateMonth == md && x.ProjectId == item.ProjectId)?.TwelveCompleteValue;
                            item.TwelveCompleteValue = twe;
                            totalCompleteValue += twe;
                            break;
                    }
                    item.TotalCompleteValue = totalCompleteValue;
                }
                //开累
                var accumulateuCompleteValue = data
                    .Where(x => x.Year <= item.Year && x.ProjectId == item.ProjectId)
                    .Sum(x => SqlFunc.ToDecimal(x.OneCompleteValue) + SqlFunc.ToDecimal(x.TwoCompleteValue) + SqlFunc.ToDecimal(x.ThreeCompleteValue) + SqlFunc.ToDecimal(x.FourCompleteValue) + SqlFunc.ToDecimal(x.FiveCompleteValue) + SqlFunc.ToDecimal(x.SixCompleteValue) + SqlFunc.ToDecimal(x.SevenCompleteValue) + SqlFunc.ToDecimal(x.EightCompleteValue) + SqlFunc.ToDecimal(x.NineCompleteValue) + SqlFunc.ToDecimal(x.TenCompleteValue) + SqlFunc.ToDecimal(x.ElevenCompleteValue) + SqlFunc.ToDecimal(x.TwelveCompleteValue));
                item.AccumulateuCompleteValue = accumulateuCompleteValue;
            }
            rt.Count = total;
            rt.SuccessResult(completeData);
            return rt;
        }
        /// <summary>
        /// 保存历史产值月报
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveHistoryProjectMonthReportAsync(HistoryProjectMonthReportRequestParam requestBody)
        {
            ResponseAjaxResult<bool> rt = new();
            List<ExcelProductionConvertTable> updateTable = new();
            var rr = await dbContext.Queryable<ExcelProductionConvertTable>().FirstAsync(x => x.Id == requestBody.CompleteId);
            if (rr != null)
            {
                var data = await dbContext.Queryable<ExcelProductionConvertTable>().Where(x => x.ProjectId == rr.ProjectId && x.Year == rr.Year).ToListAsync();
                for (int i = 1; i <= 12; i++)
                {
                    var md = int.Parse(rr.Year + (i < 10 ? "0" + i : i.ToString()));
                    var fr = data.FirstOrDefault(x => x.DateMonth == md);
                    switch (i)
                    {
                        case 1:
                            if (fr != null) { fr.OneCompleteValue = requestBody.OneCompleteValue; updateTable.Add(fr); }
                            break;
                        case 2:
                            if (fr != null) { fr.TwoCompleteValue = requestBody.TwoCompleteValue; updateTable.Add(fr); }
                            break;
                        case 3:
                            if (fr != null) { fr.ThreeCompleteValue = requestBody.ThreeCompleteValue; updateTable.Add(fr); }
                            break;
                        case 4:
                            if (fr != null) { fr.FourCompleteValue = requestBody.FourCompleteValue; updateTable.Add(fr); }
                            break;
                        case 5:
                            if (fr != null) { fr.FiveCompleteValue = requestBody.FiveCompleteValue; updateTable.Add(fr); }
                            break;
                        case 6:
                            if (fr != null) { fr.SixCompleteValue = requestBody.SixCompleteValue; updateTable.Add(fr); }
                            break;
                        case 7:
                            if (fr != null) { fr.SevenCompleteValue = requestBody.SevenCompleteValue; updateTable.Add(fr); }
                            break;
                        case 8:
                            if (fr != null) { fr.EightCompleteValue = requestBody.EightCompleteValue; updateTable.Add(fr); }
                            break;
                        case 9:
                            if (fr != null) { fr.NineCompleteValue = requestBody.NineCompleteValue; updateTable.Add(fr); }
                            break;
                        case 10:
                            if (fr != null) { fr.TenCompleteValue = requestBody.TenCompleteValue; updateTable.Add(fr); }
                            break;
                        case 11:
                            if (fr != null) { fr.ElevenCompleteValue = requestBody.ElevenCompleteValue; updateTable.Add(fr); }
                            break;
                        case 12:
                            if (fr != null) { fr.TwelveCompleteValue = requestBody.TwelveCompleteValue; updateTable.Add(fr); }
                            break;
                    }
                }
                await dbContext.Updateable(updateTable)
                    .WhereColumns(x => x.Id)
                    .UpdateColumns(x => new
                    {
                        x.OneCompleteValue,
                        x.TwoCompleteValue,
                        x.ThreeCompleteValue,
                        x.FourCompleteValue,
                        x.FiveCompleteValue,
                        x.SixCompleteValue,
                        x.SevenCompleteValue,
                        x.EightCompleteValue,
                        x.NineCompleteValue,
                        x.TenCompleteValue,
                        x.ElevenCompleteValue,
                        x.TwelveCompleteValue,
                    }).ExecuteCommandAsync();
                rt.SuccessResult(true);
            }
            else rt.FailResult(HttpStatusCode.UpdateFail, "无数据");
            return rt;
        }

        #region 月报编辑按钮控制权限
        /// <summary>
        /// 月报编辑按钮控制权限 1月报 2 日报
        /// </summary>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public bool BtnEditMonthlyReport(int type, int date, List<BtnEditMonthlyReportPermission> permissions)
        {
            bool rs = false;
            //获取登录用户按钮权限控制表
            var permission = permissions.FirstOrDefault(t => t.IsDelete == 1 && t.UserId == _currentUser.Id);
            if (_currentUser.CurrentLoginIsAdmin || _currentUser.Account == "2016146340") rs = true;
            else if (type == 1)
            {
                if (permission != null && permission.MonthReportEnable == true)//如果设置了权限  优先级最大
                {
                    //获取可以修改的月份
                    var months = string.IsNullOrEmpty(permission.MonthTime)
                        ? new List<int>()
                        : permission.MonthTime.Split(',')
                        .Where(s => int.TryParse(s, out _))
                        .Select(int.Parse)
                        .ToList();
                    if (months.Any() && months.Contains(date) && permission.MonthEffectiveTime >= DateTime.Now)//并且在有效期内
                        rs = true;
                }
                else
                {
                    //没设置权限
                    //月报 默认加载情况
                    //日期判断 是否在当月26-次月6日期间 可以修改当月的月报 
                    var tMonth = 0;
                    if (DateTime.Now.Day >= 26) tMonth = ConvertHelper.ToDateMonth(DateTime.Now);
                    else if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 6) tMonth = ConvertHelper.ToDateMonth(DateTime.Now.AddMonths(-1));
                    if (tMonth != 0 && tMonth == date) rs = true;
                }
            }
            else if (type == 2)
            {
                //设置了权限 优先级最大
                if (permission != null && permission.DailyReportEnable == true)
                {
                    //只能更改在这个时间范围内的日报
                    if (date >= permission.DailyStartTime.ToDateDay() && date <= permission.DailyEndTime.ToDateDay()) rs = true;
                }
                else
                {
                    //默认只能修改当日日报 并且在十点前可以修改 否则不可以修改
                    var nowDay = Convert.ToDateTime(DateTime.Now.AddDays(-1)).ToDateDay();
                    if (DateTime.Now <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 10:00:00")))
                    {
                        //当天日报可以修改
                        if (date == nowDay) rs = true;
                    }
                }
            }
            return rs;
        }
        /// <summary>
        /// 月报编辑按钮权限控制 列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BtnEditMonthlyReportSearch>>> BtnEditMonthlyReportSearchAsync(BaseRequestDto requestBody)
        {
            ResponseAjaxResult<List<BtnEditMonthlyReportSearch>> rt = new();
            List<BtnEditMonthlyReportSearch> rs = new();
            RefAsync<int> total = 0;
            var users = await dbContext.Queryable<Domain.Models.User>()
                .Where(t => t.IsDelete == 1 && t.LoginAccount != "2016146340" && t.LoginAccount != "2022002687")
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.KeyWords), t => t.Name.Contains(requestBody.KeyWords) || t.Phone.Contains(requestBody.KeyWords))
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            //所有启用的用户
            var permissions = await dbContext.Queryable<BtnEditMonthlyReportPermission>().Where(t => t.IsDelete == 1).ToListAsync();
            foreach (var item in users)
            {
                BtnEditMonthlyReportSearch rr = new();
                rr.UserName = item.Name + "(" + item.Phone + ")";
                rr.UserId = item.Id;
                var per = permissions.FirstOrDefault(x => x.UserId == item.Id);
                if (per != null)
                {
                    rr.Id = per.Id;
                    //月报
                    if (per.MonthReportEnable == true)
                    {
                        rr.MonthReportEnableStatus = per.MonthReportEnable;
                        //月报可修改的月份
                        var months = string.IsNullOrEmpty(per.MonthTime)
                           ? new List<int>()
                           : per.MonthTime.Split(',')
                           .Where(s => int.TryParse(s, out _))
                           .Select(int.Parse)
                           .OrderBy(x => x)
                           .ToList();
                        var monthTime = string.Empty;
                        if (months.Any())
                        {
                            List<string> resultList = new();
                            foreach (var item2 in months)
                            {
                                if (ConvertHelper.TryParseFromDateMonth(item2, out DateTime nowTime))
                                {
                                    resultList.Add(nowTime.ToString("yyyy年MM月"));
                                }
                            }
                            monthTime = string.Join("、", resultList);
                        }
                        rr.MonthTime = monthTime;
                        //月报修改有效日期
                        rr.MonthEffectiveTime = per.MonthEffectiveTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    //日报
                    if (per.DailyReportEnable == true)
                    {
                        rr.DailyReportEnableStatus = per.DailyReportEnable;
                        rr.DailyStartTime = per.DailyStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                        rr.DailyEndTime = per.DailyEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                rs.Add(rr);
            }
            rt.Count = total;
            return rt.SuccessResult(rs);
        }

        /// <summary>
        /// 保存编辑按钮权限
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveBtnEditReportAsync(SaveBtnEditMonthlyReport requestBody)
        {
            ResponseAjaxResult<bool> rt = new();
            bool status = false;

            if (!string.IsNullOrWhiteSpace(requestBody.Id.ToString()) && requestBody.Id != Guid.Empty)
            {
                //修改保存
                var per = await dbContext.Queryable<BtnEditMonthlyReportPermission>().Where(t => t.IsDelete == 1 && t.Id == requestBody.Id).FirstAsync();
                if (per != null)
                {
                    if (requestBody.PermissionType == 1)//月报
                    {
                        if (requestBody.MonthTime != null && requestBody.MonthTime.Any())
                        {
                            per.MonthTime = string.Join(',', requestBody.MonthTime);
                        }
                        else
                        {
                            per.MonthTime = string.Empty;
                        }
                        per.MonthEffectiveTime = Convert.ToDateTime(DateTime.Now.AddDays(2).ToString("23:59:59"));//往后加三天
                        per.MonthReportEnable = requestBody.MonthReportEnable;
                        await dbContext.Updateable(per).UpdateColumns(x => new
                        {
                            x.MonthTime,
                            x.MonthEffectiveTime,
                            x.MonthReportEnable
                        })
                        .ExecuteCommandAsync();
                        status = true;
                    }
                    else if (requestBody.PermissionType == 2)//日报
                    {
                        per.DailyStartTime = requestBody.DailyStartTime;
                        per.DailyEndTime = requestBody.DailyEndTime;
                        per.DailyReportEnable = requestBody.DailyReportEnable;
                        await dbContext.Updateable(per).UpdateColumns(x => new
                        {
                            x.DailyStartTime,
                            x.DailyEndTime,
                            x.DailyReportEnable
                        })
                        .ExecuteCommandAsync();
                        status = true;
                    }
                    else status = false;
                }
                else status = false;
            }
            else
            {
                //新增
                if (requestBody.PermissionType == 1)//月报
                {
                    var monthTime = string.Empty;
                    if (requestBody.MonthTime != null && requestBody.MonthTime.Any())
                    {
                        monthTime = string.Join(',', requestBody.MonthTime);
                    }
                    else
                    {
                        monthTime = string.Empty;
                    }
                    var add = new BtnEditMonthlyReportPermission
                    {
                        Id = GuidUtil.Next(),
                        DailyEndTime = DateTime.Now,//默认当前时间 不做处理
                        DailyStartTime = DateTime.Now,//默认当前时间 不做处理
                        MonthEffectiveTime = requestBody.MonthEffectiveTime,
                        MonthReportEnable = requestBody.MonthReportEnable,
                        MonthTime = monthTime,
                        UserId = requestBody.UserId,
                        DailyReportEnable = false
                    };
                    await dbContext.Insertable(add).ExecuteCommandAsync();
                    status = true;
                }
                else if (requestBody.PermissionType == 2)//日报
                {
                    var add = new BtnEditMonthlyReportPermission
                    {
                        Id = GuidUtil.Next(),
                        DailyEndTime = requestBody.DailyEndTime,
                        DailyStartTime = requestBody.DailyStartTime,
                        MonthEffectiveTime = DateTime.Now,
                        MonthReportEnable = false,
                        MonthTime = string.Empty,
                        UserId = requestBody.UserId,
                        DailyReportEnable = requestBody.DailyReportEnable
                    };
                    await dbContext.Insertable(add).ExecuteCommandAsync();
                    status = true;
                }
                else status = false;
            }
            if (status == true)
                return rt.SuccessResult(status, "保存成功");
            else
                return rt.SuccessResult(status, "保存失败");
        }
        #endregion

        #region 项目年初计划
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SearchProjectAnnualProduction>> SearchProjectAnnualProductionDetailsAsync(SearchProjectAnnualProductionRequest requestBody)
        {
            ResponseAjaxResult<SearchProjectAnnualProduction> rt = new();
            SearchProjectAnnualProduction rr = new();
            List<SearchProjectAnnualProductionDto> rsList = new();

            //初始化
            List<ProjectAnnualPlanProduction> pPlanProduction = new();

            var annualProductionShips = await dbContext.Queryable<AnnualProductionShips>()
                .WhereIF(!requestBody.IsProjectAnnualProduction, x => x.ShipType == 1)
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            pPlanProduction = await dbContext.Queryable<ProjectAnnualPlanProduction>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()), t => t.ProjectId == requestBody.ProjectId)
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id))
                .ToListAsync();

            var pIds = pPlanProduction.Select(x => x.ProjectId).ToList();

            //项目基本信息
            var projects = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && pIds.Contains(t.Id)).Select(x => new { x.Id, x.CurrencyId, x.ShortName, x.ECAmount, x.ContractStipulationEndDate }).ToListAsync();

            var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();

            //项目开累产值   24年调整最终版本+至今为止月报数据
            var monthReport = await dbContext.Queryable<MonthReport>().Where(t => t.IsDelete == 1 && pIds.Contains(t.ProjectId)).ToListAsync();
            var monthReport24 = await dbContext.Queryable<MonthReportDetailHistory>().Where(t => pIds.Contains(t.ProjectId)).ToListAsync();

            if (!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()))
            {
                decimal completeAmount = 0M;//完成产值
                decimal accumulatedOutputValue = 0M;//开累产值
                var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.Id == requestBody.ProjectId && x.IsDelete == 1);
                if (pp != null)
                {
                    var dateMonth = DateTime.Now.ToDateMonth();
                    var yearMonth = Convert.ToInt32(DateTime.Now.Year + "01");
                    if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                    {
                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.ActualCompAmount));//开累
                    }
                    else
                    {
                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.RMBHValue));//开累
                    }
                    //截止到当月的完成产值+历史调整开累产值=开累产值
                    accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                    //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                    completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                    rr.ProjectName = pp.ShortName;
                    rr.EffectiveAmount = Convert.ToDecimal(pp.ECAmount);
                    rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                    rr.RemainingAmount = Convert.ToDecimal(pp.ECAmount) - completeAmount;
                    rr.AccumulatedOutputValue = accumulatedOutputValue;
                }
            }

            foreach (var item in pPlanProduction)
            {
                SearchProjectAnnualProductionDto ppChild = new();
                ppChild.Id = item.Id;
                ppChild.JanuaryProductionQuantity = item.JanuaryProductionQuantity;
                ppChild.JanuaryProductionValue = item.JanuaryProductionValue;
                ppChild.FebruaryProductionQuantity = item.FebruaryProductionQuantity;
                ppChild.FebruaryProductionValue = item.FebruaryProductionValue;
                ppChild.MarchProductionQuantity = item.MarchProductionQuantity;
                ppChild.MarchProductionValue = item.MarchProductionValue;
                ppChild.AprilProductionQuantity = item.AprilProductionQuantity;
                ppChild.AprilProductionValue = item.AprilProductionValue;
                ppChild.MayProductionQuantity = item.MayProductionQuantity;
                ppChild.MayProductionValue = item.MayProductionValue;
                ppChild.JuneProductionQuantity = item.JuneProductionQuantity;
                ppChild.JuneProductionValue = item.JuneProductionValue;
                ppChild.JulyProductionQuantity = item.JulyProductionQuantity;
                ppChild.JulyProductionValue = item.JulyProductionValue;
                ppChild.AugustProductionValue = item.AugustProductionValue;
                ppChild.AugustProductionQuantity = item.AugustProductionQuantity;
                ppChild.SeptemberProductionQuantity = item.SeptemberProductionQuantity;
                ppChild.SeptemberProductionValue = item.SeptemberProductionValue;
                ppChild.OctoberProductionQuantity = item.OctoberProductionQuantity;
                ppChild.OctoberProductionValue = item.OctoberProductionValue;
                ppChild.NovemberProductionQuantity = item.NovemberProductionQuantity;
                ppChild.NovemberProductionValue = item.NovemberProductionValue;
                ppChild.DecemberProductionQuantity = item.DecemberProductionQuantity;
                ppChild.DecemberProductionValue = item.DecemberProductionValue;
                ppChild.AnnualProductionShips = annualProductionShips
                    .Where(x => x.ProjectAnnualProductionId == item.Id)
                    .Select(x => new AnnualPlanProductionShips { ShipId = x.ShipId, ShipType = x.ShipType, ShipName = x.ShipName })
                    .ToList();
                ppChild.ProjectId = item.ProjectId;
                ppChild.CompanyId = item.CompanyId;
                rsList.Add(ppChild);
            }

            rt.Count = rsList.Count;
            rr.SearchProjectAnnualProductionDto = rsList;
            return rt.SuccessResult(rr);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchProjectAnnualProductionDto>>> SearchProjectAnnualProductionAsync(SearchProjectAnnualProductionRequest requestBody)
        {
            ResponseAjaxResult<List<SearchProjectAnnualProductionDto>> rt = new();
            List<SearchProjectAnnualProductionDto> rr = new();

            //初始化
            List<ProjectAnnualPlanProduction> pPlanProduction = new();

            var annualProductionShips = await dbContext.Queryable<AnnualProductionShips>()
                .WhereIF(!requestBody.IsProjectAnnualProduction, x => x.ShipType == 1)
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            pPlanProduction = await dbContext.Queryable<ProjectAnnualPlanProduction>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()), t => t.ProjectId == requestBody.ProjectId)
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id))
                .ToListAsync();

            foreach (var item in pPlanProduction)
            {
                SearchProjectAnnualProductionDto ppChild = new();
                ppChild.Id = item.Id;
                ppChild.JanuaryProductionQuantity = item.JanuaryProductionQuantity;
                ppChild.JanuaryProductionValue = item.JanuaryProductionValue;
                ppChild.FebruaryProductionQuantity = item.FebruaryProductionQuantity;
                ppChild.FebruaryProductionValue = item.FebruaryProductionValue;
                ppChild.MarchProductionQuantity = item.MarchProductionQuantity;
                ppChild.MarchProductionValue = item.MarchProductionValue;
                ppChild.AprilProductionQuantity = item.AprilProductionQuantity;
                ppChild.AprilProductionValue = item.AprilProductionValue;
                ppChild.MayProductionQuantity = item.MayProductionQuantity;
                ppChild.MayProductionValue = item.MayProductionValue;
                ppChild.JuneProductionQuantity = item.JuneProductionQuantity;
                ppChild.JuneProductionValue = item.JuneProductionValue;
                ppChild.JulyProductionQuantity = item.JulyProductionQuantity;
                ppChild.JulyProductionValue = item.JulyProductionValue;
                ppChild.AugustProductionValue = item.AugustProductionValue;
                ppChild.AugustProductionQuantity = item.AugustProductionQuantity;
                ppChild.SeptemberProductionQuantity = item.SeptemberProductionQuantity;
                ppChild.SeptemberProductionValue = item.SeptemberProductionValue;
                ppChild.OctoberProductionQuantity = item.OctoberProductionQuantity;
                ppChild.OctoberProductionValue = item.OctoberProductionValue;
                ppChild.NovemberProductionQuantity = item.NovemberProductionQuantity;
                ppChild.NovemberProductionValue = item.NovemberProductionValue;
                ppChild.DecemberProductionQuantity = item.DecemberProductionQuantity;
                ppChild.DecemberProductionValue = item.DecemberProductionValue;
                ppChild.AnnualProductionShips = annualProductionShips
                    .Where(x => x.ProjectAnnualProductionId == item.Id)
                    .Select(x => new AnnualPlanProductionShips { ShipId = x.ShipId, ShipType = x.ShipType, ShipName = x.ShipName })
                    .ToList();
                ppChild.ProjectId = item.ProjectId;
                ppChild.CompanyId = item.CompanyId;
                rr.Add(ppChild);
            }

            rt.Count = rr.Count;
            return rt.SuccessResult(rr);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>

        public async Task<ResponseAjaxResult<bool>> SaveProjectAnnualProductionAsync(List<SearchProjectAnnualProductionDto>? requestBody)
        {
            ResponseAjaxResult<bool> rt = new();

            List<ProjectAnnualPlanProduction> addTables = new();
            List<ProjectAnnualPlanProduction> upTables = new();
            List<AnnualProductionShips> addShipTables = new();
            List<Guid> Ids = new();

            if (requestBody != null && requestBody.Any())
            {
                var tables = await dbContext.Queryable<ProjectAnnualPlanProduction>().Where(t => t.IsDelete == 1 && requestBody.Select(x => x.Id).Contains(t.Id)).ToListAsync();
                foreach (var item in requestBody)
                {
                    if (!string.IsNullOrWhiteSpace(item.Id.ToString()) && item.Id != Guid.Empty)
                    {
                        var first = tables.FirstOrDefault(x => x.Id == item.Id);
                        if (first != null)
                        {
                            first.JanuaryProductionQuantity = item.JanuaryProductionQuantity;
                            first.JanuaryProductionValue = item.JanuaryProductionValue;
                            first.FebruaryProductionQuantity = item.FebruaryProductionQuantity;
                            first.FebruaryProductionValue = item.FebruaryProductionValue;
                            first.MarchProductionQuantity = item.MarchProductionQuantity;
                            first.MarchProductionValue = item.MarchProductionValue;
                            first.AprilProductionQuantity = item.AprilProductionQuantity;
                            first.AprilProductionValue = item.AprilProductionValue;
                            first.MayProductionQuantity = item.MayProductionQuantity;
                            first.MayProductionValue = item.MayProductionValue;
                            first.JuneProductionQuantity = item.JuneProductionQuantity;
                            first.JuneProductionValue = item.JuneProductionValue;
                            first.JulyProductionQuantity = item.JulyProductionQuantity;
                            first.JulyProductionValue = item.JulyProductionValue;
                            first.AugustProductionValue = item.AugustProductionValue;
                            first.AugustProductionQuantity = item.AugustProductionQuantity;
                            first.SeptemberProductionQuantity = item.SeptemberProductionQuantity;
                            first.SeptemberProductionValue = item.SeptemberProductionValue;
                            first.OctoberProductionQuantity = item.OctoberProductionQuantity;
                            first.OctoberProductionValue = item.OctoberProductionValue;
                            first.NovemberProductionQuantity = item.NovemberProductionQuantity;
                            first.NovemberProductionValue = item.NovemberProductionValue;
                            first.DecemberProductionQuantity = item.DecemberProductionQuantity;
                            first.DecemberProductionValue = item.DecemberProductionValue;
                            upTables.Add(first);
                            Ids.Add(first.Id);

                            foreach (var ship in item.AnnualProductionShips)
                            {
                                addShipTables.Add(new AnnualProductionShips
                                {
                                    Id = GuidUtil.Next(),
                                    ShipType = ship.ShipType,
                                    ShipId = ship.ShipId,
                                    ShipName = ship.ShipName,
                                    ProjectAnnualProductionId = first.Id
                                });
                            }
                        }
                    }
                    else
                    {
                        var id = GuidUtil.Next();
                        addTables.Add(new ProjectAnnualPlanProduction
                        {
                            JanuaryProductionQuantity = item.JanuaryProductionQuantity,
                            JanuaryProductionValue = item.JanuaryProductionValue,
                            FebruaryProductionQuantity = item.FebruaryProductionQuantity,
                            FebruaryProductionValue = item.FebruaryProductionValue,
                            MarchProductionQuantity = item.MarchProductionQuantity,
                            MarchProductionValue = item.MarchProductionValue,
                            AprilProductionQuantity = item.AprilProductionQuantity,
                            AprilProductionValue = item.AprilProductionValue,
                            MayProductionQuantity = item.MayProductionQuantity,
                            MayProductionValue = item.MayProductionValue,
                            JuneProductionQuantity = item.JuneProductionQuantity,
                            JuneProductionValue = item.JuneProductionValue,
                            JulyProductionQuantity = item.JulyProductionQuantity,
                            JulyProductionValue = item.JulyProductionValue,
                            AugustProductionValue = item.AugustProductionValue,
                            AugustProductionQuantity = item.AugustProductionQuantity,
                            SeptemberProductionQuantity = item.SeptemberProductionQuantity,
                            SeptemberProductionValue = item.SeptemberProductionValue,
                            OctoberProductionQuantity = item.OctoberProductionQuantity,
                            OctoberProductionValue = item.OctoberProductionValue,
                            NovemberProductionQuantity = item.NovemberProductionQuantity,
                            NovemberProductionValue = item.NovemberProductionValue,
                            DecemberProductionQuantity = item.DecemberProductionQuantity,
                            DecemberProductionValue = item.DecemberProductionValue,
                            ProjectId = item.ProjectId,
                            CompanyId = item.CompanyId,
                            Year = DateTime.Now.Year,
                            Id = id
                        });

                        foreach (var ship in item.AnnualProductionShips)
                        {
                            addShipTables.Add(new AnnualProductionShips
                            {
                                Id = GuidUtil.Next(),
                                ShipType = ship.ShipType,
                                ShipId = ship.ShipId,
                                ShipName = ship.ShipName,
                                ProjectAnnualProductionId = id
                            });
                        }
                    }
                }
                if (Ids.Any())
                {
                    var delete = await dbContext.Queryable<AnnualProductionShips>().Where(x => Ids.Contains(x.ProjectAnnualProductionId)).ToListAsync();
                    await dbContext.Deleteable(delete).ExecuteCommandAsync();
                }
                if (upTables.Any())
                {
                    await dbContext.Updateable(upTables).ExecuteCommandAsync();
                }
                if (addTables.Any())
                {
                    await dbContext.Insertable(addTables).ExecuteCommandAsync();
                }
                if (addShipTables.Any())
                {
                    await dbContext.Insertable(addShipTables).ExecuteCommandAsync();
                }
                rt.SuccessResult(true, "保存成功");
            }
            else rt.FailResult(HttpStatusCode.SaveFail, "保存失败");

            return rt;
        }

        /// <summary>
        /// 基本数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BaseAnnualProduction>> BaseAnnualProductionAsync()
        {
            ResponseAjaxResult<BaseAnnualProduction> rt = new();
            BaseAnnualProduction rr = new();

            var bProjects = await baseService.SearchProjectInformationAsync(new BaseRequestDto());
            var ppIds = bProjects.Data?.Select(x => x.Id).ToList();

            List<ProjectInfosForAnnualProduction> projectInfosForAnnualProductions = new();
            List<OwnShipsForAnnualProduction> ownShipsForAnnualProductions = new();

            projectInfosForAnnualProductions = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && ppIds.Contains(t.Id))
                .Select(x => new ProjectInfosForAnnualProduction
                {
                    CompanyId = x.CompanyId,
                    ProjectId = x.Id,
                    ProjectName = x.Name
                }).ToListAsync();

            var owns = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1)
                .Select(x => new OwnShipsForAnnualProduction
                {
                    Id = x.PomId,
                    Name = x.Name,
                    SubOrOwn = 1
                }).ToListAsync();
            var subs = await dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1)
                .Select(x => new OwnShipsForAnnualProduction
                {
                    Id = x.PomId,
                    Name = x.Name,
                    SubOrOwn = 2
                }).ToListAsync();
            ownShipsForAnnualProductions.AddRange(owns);
            ownShipsForAnnualProductions.AddRange(subs);

            rr.ProjectInfosForAnnualProductions = projectInfosForAnnualProductions;
            rr.OwnShipsForAnnualProductions = ownShipsForAnnualProductions;
            return rt.SuccessResult(rr);
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> ExcelProjectAnnualProductionAsync(ProjectAnnualProductionDto requestBody)
        {
            ResponseAjaxResult<bool> rt = new();
            List<ProjectAnnualPlanProduction> add = new();

            var rr = requestBody.ExcelImport.ToList();

            if (rr != null && rr.Any())
            {
                var tables = await dbContext.Queryable<ProjectAnnualPlanProduction>().Where(t => t.IsDelete == 1 && t.Year == DateTime.Now.Year).ToListAsync();
                tables.ForEach(x => x.IsDelete = 0);
                await dbContext.Updateable(tables).UpdateColumns(x => x.IsDelete).ExecuteCommandAsync();
                var projects = await dbContext.Queryable<Project>().Select(x => new { x.Id, x.CompanyId, x.Name }).ToListAsync();
                var names = new List<string>();
                foreach (var item in rr)
                {
                    var name = "";
                    if (item.ProjectName.Length > 10)
                    {
                        name = item.ProjectName.Substring(0, 10);
                    }
                    var pp = projects.FirstOrDefault(x => x.Name.Contains(name));
                    add.Add(new ProjectAnnualPlanProduction
                    {
                        JanuaryProductionValue = item.JanuaryProductionValue,
                        FebruaryProductionValue = item.FebruaryProductionValue,
                        MarchProductionValue = item.MarchProductionValue,
                        AprilProductionValue = item.AprilProductionValue,
                        MayProductionValue = item.MayProductionValue,
                        JuneProductionValue = item.JuneProductionValue,
                        JulyProductionValue = item.JulyProductionValue,
                        AugustProductionValue = item.AugustProductionValue,
                        SeptemberProductionValue = item.SeptemberProductionValue,
                        OctoberProductionValue = item.OctoberProductionValue,
                        NovemberProductionValue = item.NovemberProductionValue,
                        DecemberProductionValue = item.DecemberProductionValue,
                        ProjectId = pp?.Id ?? null,
                        CompanyId = Guid.Empty,
                        Year = DateTime.Now.Year,
                        Id = GuidUtil.Next()
                    });
                    if (pp == null) names.Add(item.ProjectName);
                }
                add = add.Where(x => x.ProjectId != Guid.Empty && x.ProjectId != null).ToList();
                foreach (var item in add)
                {
                    var compId = projects.FirstOrDefault(x => x.Id == item.ProjectId)?.CompanyId;
                    item.CompanyId = compId.Value;
                }
                await dbContext.Insertable(add).ExecuteCommandAsync();

                rt.Data = true;
                rt.Success();
            }
            return rt;
        }

        /// <summary>
        /// 公司初始化
        /// </summary>
        /// <returns></returns>
        private Dictionary<Guid, (string Name, int Sequence)> KeyValuePairs()
        {
            return new Dictionary<Guid, (string, int)>
            {
                { "c0d51e81-03dd-4ef8-bd83-6eb1355879e1".ToGuid(), ("五公司", 5) },
                { "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid(), ("直营项目", 9) },
                { "5a8f39de-8515-456b-8620-c0d01c012e04".ToGuid(), ("四公司", 4) },
                { "3c5b138b-601a-442a-9519-2508ec1c1eb2".ToGuid(), ("疏浚公司", 1) },
                { "65052a94-6ea7-44ba-96b4-cf648de0d28a".ToGuid(), ("福建公司", 6) },
                { "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid(), ("交建公司", 2) },
                { "11c9c978-9ef3-411d-ba70-0d0eed93e048".ToGuid(), ("三公司", 3) },
                { "01ff7a0e-e827-4b46-9032-0a540ce1fba3".ToGuid(), ("菲律宾公司", 7) },
                { "9930ca6d-7131-4a2a-8b75-ced9c0579b8c".ToGuid(), ("广航水利", 8) }
            };
        }
        #endregion


        #region 获取每年的节假日
        public bool GetHolidays()
        {
            var url = AppsettingsHelper.GetValue("Holidays");
            url = url.Replace("year", DateTime.Now.Year.ToString());
            return false;
        }


        #endregion
    }
}
