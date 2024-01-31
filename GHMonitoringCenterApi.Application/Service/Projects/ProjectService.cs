using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
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
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Pkcs;
using SqlSugar;
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
        public ProjectService(IBaseRepository<ProjectWBS> baseProjectWBSRepository, ISqlSugarClient dbContext, IMapper mapper, IBaseRepository<Files> baseFilesRepository, IBaseRepository<ProjectOrg> baseProjectOrgRepository, IBaseRepository<ProjectLeader> baseProjectLeaderRepository, IPushPomService pushPomService, IBaseService baseService, ILogService logService, IEntityChangeService entityChangeService, GlobalObject globalObject)
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

            var project = dbContext.Queryable<Project>()
                .LeftJoin<ProjectStatus>((p, ps) => p.StatusId == ps.StatusId)
                .Where(p => departmentIds.Contains(p.ProjectDept.Value))
                .Where(p =>p.StatusId != "0c686c96-889e-4c4d-b24d-fa2886d9dceb".ToGuid())
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
                    Name = p.Name,
                    Category = p.Category,
                    TypeName = p.TypeId.ToString(),
                    Amount = p.Amount,
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
                    ContractChangeInfo = p.ContractChangeInfo
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
                item.Amount = searchRequestDto.IsConvert == true ?
                    item.ZCURRENCYNAME == "edea1eb4-936f-465a-8ffa-1d1669bc3776" ?
                    Math.Round(Convert.ToDecimal(item.Amount * rate.Single(x => x.CurrencyId == "edea1eb4-936f-465a-8ffa-1d1669bc3776").ExchangeRate), 2)
                    : item.ZCURRENCYNAME == "43e52b6c-f41f-48c8-822c-103732f81cc1" ?
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
            ResponseAjaxResult<ProjectDetailResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectDetailResponseDto>();
            var projectDeteilSingle = await dbContext.Queryable<Project>()
                 .Where((p) => p.Id == basePrimaryRequestDto.Id && p.IsDelete == 1)
                 .Select((p) => new ProjectDetailResponseDto
                 {
                     Id = p.Id,
                     Name = p.Name,
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
                     ContractChangeInfo = p.ContractChangeInfo
                 }).SingleAsync();
            if (projectDeteilSingle == null)
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                return responseAjaxResult;
            }
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
            projectDeteilSingle.AreaName =  await baseService.GetProvincemarket(provinceList, projectDeteilSingle.AreaName.ToGuid());
            //获取区域名称
            projectDeteilSingle.RegionName = await dbContext.Queryable<ProjectArea>().Where(x => x.IsDelete == 1 && x.AreaId == projectDeteilSingle.RegionId).Select(x => x.Name).SingleAsync();
            //类型
            projectDeteilSingle.TypeName = await dbContext.Queryable<ProjectType>().Where(x => x.IsDelete == 1 && x.PomId == projectDeteilSingle.TypeId).Select(x => x.Name).SingleAsync();
            //币种
            projectDeteilSingle.CurrencyName = await dbContext.Queryable<Currency>().Where(x => x.IsDelete == 1 && x.PomId == projectDeteilSingle.CurrencyId).Select(x => x.Zcurrencyname).SingleAsync();
            //获取币种汇率
            projectDeteilSingle.ExchangeRate = await dbContext.Queryable<CurrencyConverter>().Where(x => x.IsDelete == 1 && x.CurrencyId == projectDeteilSingle.CurrencyId.ToString()&&x.Year==DateTime.Now.Year).Select(x => x.ExchangeRate).SingleAsync();
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
                .Where(p => (p.ProjectId == projectDeteilSingle.Id || p.ProjectId == projectDeteilSingle.MasterProjectId) && p.IsDelete == 1).ToListAsync();
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
                var dutyName = await dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.User>().Where(x => dutyList.Select(x => x.AssistantManagerId).Contains(x.PomId)).Select(x => new { x.PomId, x.Name,x.Phone }).ToListAsync();
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
            if (projectDeteilSingle!=null&&projectDeteilSingle.projectDutyDtos!=null&&projectDeteilSingle.projectDutyDtos.Any())
            {
                
                foreach (var item in projectDeteilSingle.projectDutyDtos)
                {
                    var  projectDutyList= projectDeteilSingle.projectDutyDtos.Where(x => x.Type == item.Type).ToList();
                    if (projectDutyList.Any() && projectDutyList.Count >= 2)
                    {
                        foreach (var pro in projectDutyList)
                        {
                            if (pro.ProjectId == projectDeteilSingle.Id&& !projectDutyDtoList.Exists(x => x.Type == item.Type))
                            {
                                projectDutyDtoList.Add(pro);
                            }
                        }
                    }
                    else {
                        if (!projectDutyDtoList.Exists(x => x.Type == item.Type))
                        {
                            projectDutyDtoList.Add(item);
                        }
                        
                    }
                   
                }
            }

            projectDeteilSingle.projectDutyDtos = projectDutyDtoList;
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
            //获取项目数据
            var projectList = await dbContext.Queryable<Project>().ToListAsync();
            //增改项目信息
            var projectObject = new Project();
            //增改项目干系单位
            var projectDutys = new List<ProjectOrg>();
            //增改项目干系人员
            var proLeaderList = new List<ProjectLeader>();
            #region 计算项目停工天数 供交建通每天发消息使用
            //项目ID
            Guid projectId = GuidUtil.Next();
            //计算当前周期已过多少天
            var nowDay = DateTime.Now.Day;
            //当前月的最后一天
            DateTime currentDate = DateTime.Now;
            var stopDay = 0;
            var monthLastDay = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1).AddDays(-1).Day;
            if (nowDay > 26 && nowDay <= monthLastDay)
            {
                stopDay = monthLastDay-26;
            }
            else
            {
                stopDay = nowDay + monthLastDay;

            }

            if (addOrUpdateProjectRequestDto.RequestType)
            {
                //新增操作（如果新增的是在建项目的就会记录表里面）
                if (CommonData.PConstruc == addOrUpdateProjectRequestDto.StatusId.ToString())
                {
                    ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                    {
                        Id = projectId,
                        OldStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                        NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                        ChangeTime = DateTime.Now,
                        IsValid = 1,
                        StopDay = stopDay
                    };
                    var isExist = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == projectId).CountAsync();
                    if (isExist == 0)
                    {
                        await dbContext.Insertable(projectStatusChangeRecord).ExecuteCommandAsync();
                    }

                }

            }
            else 
            {
               var  projectStatusChangeSingle = await dbContext.Queryable<ProjectStatusChangeRecord>().FirstAsync(x => x.IsValid == 1 
                && x.Id == addOrUpdateProjectRequestDto.Id);

                //修改操作  修改状态之前还是在建状态  所以停工天数不计算  生产日报也不会推送 等下再改为在建的时候 就会记录停工天数
                if (projectStatusChangeSingle != null && projectStatusChangeSingle.NewStatus == CommonData.PConstruc.ToGuid() && projectStatusChangeSingle.NewStatus != addOrUpdateProjectRequestDto.StatusId)
                {
                    //说明停工 不算每天生产日报推送数据里面了 
                    ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                    {
                        Id = addOrUpdateProjectRequestDto.Id.Value,
                        OldStatus = CommonData.PConstruc.ToGuid(),
                        NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                        ChangeTime = DateTime.Now,
                        IsValid = 1,
                        StopDay = 0
                    };
                    await dbContext.Updateable(projectStatusChangeRecord).Where(x=>x.IsValid==1).ExecuteCommandAsync();
                }
                //修改状态之前  原来状态不是在建状态了 这个时候要计算停工天数
                else if (projectStatusChangeSingle != null && projectStatusChangeSingle.NewStatus != CommonData.PConstruc.ToGuid() && addOrUpdateProjectRequestDto.StatusId == CommonData.PConstruc.ToGuid())
                {
                    var diffDays = TimeHelper.GetTimeSpan(projectStatusChangeSingle.ChangeTime, DateTime.Now).Days;
                    stopDay = projectStatusChangeSingle.StopDay.Value + diffDays;
                    ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                    {
                        Id = addOrUpdateProjectRequestDto.Id.Value,
                        OldStatus = CommonData.PConstruc.ToGuid(),
                        NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                        ChangeTime = DateTime.Now,
                        IsValid = 1,
                        StopDay = stopDay
                    };
                    await dbContext.Updateable(projectStatusChangeRecord).Where(x => x.IsValid == 1).ExecuteCommandAsync();
                }
                else if (projectStatusChangeSingle == null && addOrUpdateProjectRequestDto.StatusId == CommonData.PConstruc.ToGuid())
                {
                    //说明是有原来的非在建状态改为在建状态的
                    var projectSingle = await dbContext.Queryable<Project>().SingleAsync(x => x.IsDelete == 1 && x.Id == addOrUpdateProjectRequestDto.Id);
                    
                    var currentMonth = String.Empty;
                    if (DateTime.Now.Day > 26 && DateTime.Now.Day <= monthLastDay)
                    {
                        currentMonth =DateTime.Now.ToString("MM");
                    }
                    else {
                        currentMonth = DateTime.Now.AddMonths(-1).ToString("MM");
                    }
                    var initDay=Convert.ToDateTime(DateTime.Now.ToString("yyyy-") + currentMonth + "-26");
                    var diffDays = TimeHelper.GetTimeSpan(initDay, DateTime.Now).Days-1;
                    ProjectStatusChangeRecord projectStatusChangeRecord = new ProjectStatusChangeRecord()
                    {
                        Id = projectSingle.Id,
                        OldStatus = projectSingle != null ? projectSingle.StatusId.Value : Guid.Empty,
                        NewStatus = addOrUpdateProjectRequestDto.StatusId.Value,
                        ChangeTime = DateTime.Now,
                        IsValid = 1,
                        StopDay = diffDays
                    };
                    await dbContext.Insertable(projectStatusChangeRecord).ExecuteCommandAsync();
                }
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
                //新增项目
                await dbContext.Insertable(projectObject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                //项目变更记录
                await entityChangeService.RecordEntitysChangeAsync(EntityType.Project, projectObject.Id);
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
                }
                #endregion

                mapper.Map(addOrUpdateProjectRequestDto, projectObject);
                //税率
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

                //修改项目信息
                await dbContext.Updateable(projectObject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
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
            }
            // 更新
            if (updateWBSList.Any())
            {
                await dbContext.Updateable(updateWBSList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            }
            // 新增
            if (addWBSList.Any())
            {
                await dbContext.Insertable(addWBSList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            }
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
            &&x.Year== year).Select(x => x.ExchangeRate).SingleAsync();
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
            .OrderBy(x => x.CreateId, OrderByType.Desc)
            .OrderBy(x => x.CreateTime, OrderByType.Desc)
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
               .WhereIF(monthlyPlanRequestDto.CompanyId != null , (x, y) => x.CompanyId == monthlyPlanRequestDto.CompanyId)
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
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>> SearchCompanyProjectListAsync()
        {
            ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>();
            //List<CompanyProjectDetailedResponseDto> companyProjectDetailedResponseDtos = new List<CompanyProjectDetailedResponseDto>();
           // var companyList = await dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(x => x.IsDelete == 1 && x.Type == 1).OrderBy(x=>x.Sort).ToListAsync();
            //项目状态ID 
            var projectStatusIds = CommonData.NoStatus.Split(",").Select(x => x.ToGuid()).ToList();
            var result =await dbContext.Queryable<Project>()
                .LeftJoin<ProductionMonitoringOperationDayReport>((x, y) => x.CompanyId == y.ItemId && y.Type==1&&y.Collect==0&&!string.IsNullOrWhiteSpace(y.Name))
                .LeftJoin<ProjectType>((x, y, z) => x.TypeId == z.PomId)
                .LeftJoin<ProjectStatus>((x, y, z, a) => x.StatusId == a.StatusId)
                .Where(x => x.IsDelete == 1&&!projectStatusIds.Contains(x.StatusId.Value))
                .OrderBy((x, y, z, a)=>new { y.Sort ,a.Sequence})
                .Select((x, y, z, a) => new CompanyProjectDetailedResponseDto
                {
                    Id = x.Id,
                    ProjectName = x.Name,
                    CompanyId=y.ItemId.Value,
                    CompanyName = y.Name,
                    StatusSort=a.Sequence.Value,
                    StatusName = a.Name,
                    TypeName = z.Name,
                    ContractAmount = x.Amount.Value,
                    CommencementDate = x.CommencementTime.Value,
                    //ProjectProgress=
                }).ToListAsync();
            //foreach (var item in companyList)
            //{
            //    companyProjectDetailedResponseDtos.AddRange(result.Where(x => x.CompanyId == item.ItemId).OrderBy(x => x.StatusSort).ToList());
            //}
            responseAjaxResult.Data = result;
            responseAjaxResult.Count = result.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}
