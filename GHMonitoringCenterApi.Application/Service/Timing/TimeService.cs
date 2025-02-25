using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Timing;
using GHMonitoringCenterApi.Application.Contracts.IService.Timing;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.Extensions.Logging;
using SqlSugar;
using SqlSugar.Extensions;
using UtilsSharp;
using Model = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Service.Timing
{
    /// <summary>
    /// 定时向POM系统拉取数据 实现层
    /// </summary>
    public class TimeService : ITimeService
    {


        #region 依赖注入
        public IBaseRepository<Model.ProjectMasterData> baseProjectMasterDataRepository { get; set; }
        public IBaseRepository<SubShip> baseSubShipRepository { get; set; }
        public IBaseRepository<OwnerShip> baseOwnerShipRepository { get; set; }
        public IBaseRepository<ShipPingType> baseShipPingTypeRepository { get; set; }
        public IBaseRepository<ProjectDepartment> baseProjectDepartmentRepository { get; set; }
        public IBaseRepository<WaterCarriage> baseWaterCarriageRepository { get; set; }
        public IBaseRepository<IndustryClassification> baseIndustryClassificationRepository { get; set; }
        public IBaseRepository<ConstructionQualification> baseConstructionQualificationRepository { get; set; }
        public IBaseRepository<ProjectScale> baseProjectScaleRepository { get; set; }
        public IBaseRepository<ProjectType> baseProjectTypeRepository { get; set; }
        public IBaseRepository<Province> baseProvinceRepository { get; set; }
        public IBaseRepository<ProjectLeader> baseProjectLeaderRepository { get; set; }
        public IBaseRepository<Company> baseCompanyRepository { get; set; }
        public IBaseRepository<Model.User> baseUserRepository { get; set; }
        public IBaseRepository<Currency> baseCurrencyRepository { get; set; }
        public IBaseRepository<DealingUnitCache> baseDealingUnitCacheRepository { get; set; }
        public IBaseRepository<DealingUnit> baseDealingUnitRepository { get; set; }
        public IBaseRepository<Project> baseProjectRepository { get; set; }
        public IBaseRepository<ProjectStatus> baseProjectStatusRepository { get; set; }
        public IBaseRepository<ProjectArea> baseProjectAreaRepository { get; set; }
        public IBaseRepository<Institution> basePomInstitutionRepository { get; set; }
        public IBaseRepository<ProjectOrg> basePomProjectOrgRepository { get; set; }
        public IBaseRepository<ProjectAnnualPlan> basePomProjectYearPlan { get; set; }
        public IBaseRepository<PortData> basePortDataRepository { get; set; }

        public IBaseRepository<Soil> baseSoilRepository { get; set; }
        public IBaseRepository<ShipWorkType> baseShipWorkTypeRepository { get; set; }
        public IBaseRepository<ShipWorkMode> baseShipWorkModeRepository { get; set; }
        public IBaseRepository<SoilGrade> baseSoilGradeRepository { get; set; }

        public IBaseRepository<ShipClassic> baseShipClassicRepository { get; set; }
        public IMapper mapper { get; set; }
        public ISqlSugarClient dbContext { get; set; }

        private readonly ILogger<TimeService> _logger;
        public TimeService(IBaseRepository<Model.ProjectMasterData> baseProjectMasterDataRepository, IBaseRepository<SubShip> baseSubShipRepository, IBaseRepository<OwnerShip> baseOwnerShipRepository, IBaseRepository<ShipPingType> baseShipPingTypeRepository, IBaseRepository<ProjectDepartment> baseProjectDepartmentRepository, IBaseRepository<WaterCarriage> baseWaterCarriageRepository, IBaseRepository<IndustryClassification> baseIndustryClassificationRepository, IBaseRepository<ConstructionQualification> baseConstructionQualificationRepository, IBaseRepository<ProjectScale> baseProjectScaleRepository, IBaseRepository<ProjectType> baseProjectTypeRepository, IBaseRepository<Province> baseProvinceRepository, IBaseRepository<ProjectLeader> baseProjectLeaderRepository, IBaseRepository<Company> baseCompanyRepository, IBaseRepository<Model.User> baseUserRepository, IBaseRepository<Currency> baseCurrencyRepository, IBaseRepository<DealingUnitCache> baseDealingUnitCacheRepository, IBaseRepository<Project> baseProjectRepository, IBaseRepository<ProjectStatus> baseProjectStatusRepository, IBaseRepository<ProjectArea> baseProjectAreaRepository, IBaseRepository<Institution> basePomInstitutionRepository, IBaseRepository<ProjectOrg> basePomProjectOrgRepository, IBaseRepository<ProjectAnnualPlan> basePomProjectYearPlan, IBaseRepository<PortData> basePortDataRepository,
            IBaseRepository<Soil> baseSoilRepository,
            IBaseRepository<ShipWorkType> baseShipWorkTypeRepository,
            IBaseRepository<ShipWorkMode> baseShipWorkModeRepository,
            IBaseRepository<SoilGrade> baseSoilGradeRepository,
            IBaseRepository<ShipClassic> baseShipClassicRepository,
            IMapper mapper, ISqlSugarClient dbContext, ILogger<TimeService> logger,
            IBaseRepository<DealingUnit> baseUnitRepository)
        {
            this.baseProjectMasterDataRepository = baseProjectMasterDataRepository;
            this.baseSubShipRepository = baseSubShipRepository;
            this.baseOwnerShipRepository = baseOwnerShipRepository;
            this.baseShipPingTypeRepository = baseShipPingTypeRepository;
            this.baseProjectDepartmentRepository = baseProjectDepartmentRepository;
            this.baseWaterCarriageRepository = baseWaterCarriageRepository;
            this.baseIndustryClassificationRepository = baseIndustryClassificationRepository;
            this.baseConstructionQualificationRepository = baseConstructionQualificationRepository;
            this.baseProjectScaleRepository = baseProjectScaleRepository;
            this.baseProjectTypeRepository = baseProjectTypeRepository;
            this.baseProvinceRepository = baseProvinceRepository;
            this.baseProjectLeaderRepository = baseProjectLeaderRepository;
            this.baseCompanyRepository = baseCompanyRepository;
            this.baseUserRepository = baseUserRepository;
            this.baseCurrencyRepository = baseCurrencyRepository;
            this.baseDealingUnitCacheRepository = baseDealingUnitCacheRepository;
            this.baseDealingUnitRepository = baseUnitRepository;
            this.baseProjectRepository = baseProjectRepository;
            this.baseProjectStatusRepository = baseProjectStatusRepository;
            this.baseProjectAreaRepository = baseProjectAreaRepository;
            this.basePomInstitutionRepository = basePomInstitutionRepository;
            this.basePomProjectOrgRepository = basePomProjectOrgRepository;
            this.basePomProjectYearPlan = basePomProjectYearPlan;
            this.basePortDataRepository = basePortDataRepository;
            this.baseShipWorkModeRepository = baseShipWorkModeRepository;
            this.baseSoilGradeRepository = baseSoilGradeRepository;
            this.baseShipWorkTypeRepository = baseShipWorkTypeRepository;
            this.baseSoilRepository = baseSoilRepository;
            this.baseShipClassicRepository = baseShipClassicRepository;
            this.mapper = mapper;
            this.dbContext = dbContext;
            _logger = logger;
        }



        #endregion

        #region 获取远程服务响应数据
        /// <summary>
        /// 获取远程服务响应数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parame">根据不同参数获取不同类型的数据</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> GetServiceResponse<T>(int parame = 1)
        {
            Console.WriteLine("111:" + parame);
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var url = AppsettingsHelper.GetValue("InitDbDataItem:CompanyDataItem:Url");
            var token = AppsettingsHelper.GetValue("InitDbDataItem:CompanyDataItem:Token");
            var systemIdentificationCode = string.Empty;
            var functionAuthorizationCode = string.Empty;
            var type = string.Empty;
            var companyType = string.Empty;
            Dictionary<string, object> dirParame = new Dictionary<string, object>();
            if (parame == 2)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PersonDataItem:Url");
                systemIdentificationCode = AppsettingsHelper.GetValue("InitDbDataItem:PersonDataItem:SystemIdentificationCode");
                functionAuthorizationCode = AppsettingsHelper.GetValue("InitDbDataItem:PersonDataItem:FunctionAuthorizationCode");

                url += "?systemIdentificationCode=" + systemIdentificationCode + "&functionAuthorizationCode=" + functionAuthorizationCode;
                dirParame.Add("pageSize", "10000");
            }
            else if (parame == 3)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:InstitutionDataItem:Url");
                systemIdentificationCode = AppsettingsHelper.GetValue("InitDbDataItem:InstitutionDataItem:SystemIdentificationCode");
                functionAuthorizationCode = AppsettingsHelper.GetValue("InitDbDataItem:InstitutionDataItem:FunctionAuthorizationCode");

                url += "?systemIdentificationCode=" + systemIdentificationCode + "&functionAuthorizationCode=" + functionAuthorizationCode;
                dirParame.Add("pageSize", "10000");
            }
            else if (parame == 4)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:CurrencyDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:CurrencyDataItem:Token");
            }
            else if (parame == 5)
            {
                //url = AppsettingsHelper.GetValue("InitDbDataItem:DealingUnitDataItem:Url");
                //token = AppsettingsHelper.GetValue("InitDbDataItem:DealingUnitDataItem:Token");
                //跟改为获取主数据往来单位
                url = AppsettingsHelper.GetValue("MDM:BusinessRelatedUnit:Url");
                //url += DateTime.Now.ToString("yyyy-MM-dd");
                url += Convert.ToDateTime("2000-01-01");
                systemIdentificationCode = AppsettingsHelper.GetValue("MDM:BusinessRelatedUnit:SystemIdentificationCode");
                functionAuthorizationCode = AppsettingsHelper.GetValue("MDM:BusinessRelatedUnit:FunctionAuthorizationCode");
                url += "&systemIdentificationCode=" + systemIdentificationCode + "&functionAuthorizationCode=" + functionAuthorizationCode;
                dirParame.Add("pageSize", "1000000");
            }
            else if (parame == 6)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:ProjectDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:ProjectDataItem:Token");
                dirParame.Add("CompanyName", "GHJ");
            }
            else if (parame == 7)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:ProjectStatusDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:ProjectStatusDataItem:Token");
            }
            else if (parame == 8)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:ProjectAreaDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:ProjectAreaDataItem:Token");
            }
            else if (parame == 9)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:ProjectLeaderDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:ProjectLeaderDataItem:Token");
            }
            else if (parame == 10)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomInstitutionDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomInstitutionDataItem:Token");
                dirParame.Add("CompanyId", "GHJ");
            }
            else if (parame == 11)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomUserDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomUserDataItem:Token");
                dirParame.Add("CompanyId", "GHJ");
            }
            else if (parame == 12)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomProvinceDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomProvinceDataItem:Token");
                dirParame.Add("CompanyId", "GHJ");
            }
            else if (parame == 13)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectTypeDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectTypeDataItem:Token");
            }
            else if (parame == 14)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectScaleDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectScaleDataItem:Token");
            }
            else if (parame == 15)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomConstructionQualificationDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomConstructionQualificationDataItem:Token");
            }
            else if (parame == 16)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomIndustryClassificationDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomIndustryClassificationDataItem:Token");
            }
            else if (parame == 17)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomWaterCarriageDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomWaterCarriageDataItem:Token");
            }
            else if (parame == 18)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectDepartmentDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectDepartmentDataItem:Token");
            }
            else if (parame == 19)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:ProjectOrgDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:ProjectOrgDataItem:Token");
            }
            else if (parame == 20)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomOwnerShipDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomOwnerShipDataItem:Token");
                dirParame.Add("companyType", "3");
                dirParame.Add("type", "1");
            }
            else if (parame == 21)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomSubShipDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomSubShipDataItem:Token");
                dirParame.Add("companyType", "3");
                dirParame.Add("type", "2");
            }
            else if (parame == 22)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectMasterDataDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectMasterDataDataItem:Token");
            }
            else if (parame == 23)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectYearPlanDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomProjectYearPlanDataItem:Token");
                dirParame.Add("CompanyName", "GHJ");
            }
            // 交建通发消息提前写入数据中间表 （针对监控日报） 
            if (parame == 24)
            {
                await JjtWriteDataRecordAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
                return responseAjaxResult;
            }
            else if (parame == 25)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomPortDataDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomPortDataDataItem:Token");
            }
            else if (parame == 26)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Token");
            }
            else if (parame == 27)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Token");
            }
            else if (parame == 28)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Token");
            }
            else if (parame == 29)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Token");
            }
            else if (parame == 30)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomShipPingTypeDataItem:Token");
            }
            else if (parame == 31)
            {
                url = AppsettingsHelper.GetValue("InitDbDataItem:PomShipClassicDataItem:Url");
                token = AppsettingsHelper.GetValue("InitDbDataItem:PomShipClassicDataItem:Token");
            }
            WebHelper webHelper = new WebHelper();
            webHelper.Timeout = 300;
            if (parame == 1 || parame >= 4 && parame <= 31 && parame != 5)
            {
                webHelper.Headers.Add("token", token);
            }
            Console.WriteLine("url:" + url);
            Console.WriteLine("dirParame:" + dirParame);
            var responseResult = await webHelper.DoGetAsync<HttpBaseResponseDto<T>>(url, dirParame);
            //Console.WriteLine("结果:" + responseResult.ToJson());
            if (responseResult.Code == 200 && responseResult.Result != null && responseResult.Result.Data.Any())
            {
                //公司数据信息
                if (parame == 1)
                {
                    try
                    {
                        List<PomCompanyResponseDto> companyResponseDtos = new List<PomCompanyResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            companyResponseDtos.Add(item as PomCompanyResponseDto);
                        }
                        var companyList = mapper.Map<List<PomCompanyResponseDto>, List<Company>>(companyResponseDtos);
                        if (companyList.Any())
                        {
                            //获取公司数据信息表中所有pomids
                            var pomidList = baseCompanyRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的公司数据信息ids
                            var obtainIds = companyList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的公司数据信息
                                var noExistProjects = companyList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseCompanyRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {
                                //获取不存在的公司数据信息
                                var noExistProjects = companyList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseCompanyRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"公司数据信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }

                }
                //公司人员数据信息暂时不用
                if (parame == 2)
                {
                    //try
                    //{
                    //    List<PomUserResponseDto> userResponseDtos = new List<PomUserResponseDto>();
                    //    foreach (var item in responseResult.Result.Data)
                    //    {
                    //        userResponseDtos.Add(item as PomUserResponseDto);
                    //    }
                    //    var userList = mapper.Map<List<PomUserResponseDto>, List<Model.User>>(userResponseDtos);
                    //    if (userList.Any())
                    //    {
                    //        //获取公司数据信息表中所有pomids
                    //        var pomidList = baseUserRepository.AsQueryable().ToList();
                    //        var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                    //        //获取拉取的公司数据信息ids
                    //        var obtainIds = userList.Select(x => x.PomId).ToList();
                    //        var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                    //        var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                    //        //相同的修改
                    //        if (intersectIds.Count() != 0)
                    //        {

                    //            //获取存在的公司数据信息
                    //            var noExistProjects = userList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                    //            if (noExistProjects.Any())
                    //            {
                    //                await baseUserRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                    //            }
                    //        }
                    //        //不存在的新增
                    //        if (exceptIds.Count() != 0)
                    //        {

                    //            //获取不存在的公司数据信息
                    //            var noExistProjects = userList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                    //            foreach (var item in noExistProjects)
                    //            {
                    //                item.Id = GuidUtil.Next();
                    //            }
                    //            await baseUserRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    _logger.LogInformation($"公司数据信息拉取失败原因：{ex.Message}");
                    //    throw ex;
                    //}
                }
                //机构信息暂时不用
                if (parame == 3)
                {
                    //try
                    //{
                    //    List<PomInstitutionResponseDto> institutionResponseDtos = new List<PomInstitutionResponseDto>();
                    //    foreach (var item in responseResult.Result.Data)
                    //    {
                    //        institutionResponseDtos.Add(item as PomInstitutionResponseDto);
                    //    }
                    //    var institutionList = mapper.Map<List<PomInstitutionResponseDto>, List<Institution>>(institutionResponseDtos);
                    //    if (institutionList.Any())
                    //    {
                    //        var pomidList = basePomInstitutionRepository.AsQueryable().ToList();
                    //        var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                    //        //获取拉取的机构信息ids
                    //        var obtainIds = institutionList.Select(x => x.PomId).ToList();
                    //        var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                    //        var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                    //        //相同的修改
                    //        if (intersectIds.Count() != 0)
                    //        {
                    //            _logger.LogInformation($"币种信息拉取相同数据为：{intersectIds.Count()}");
                    //            //获取存在的机构信息
                    //            var noExistProjects = institutionList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                    //            if (noExistProjects.Any())
                    //            {
                    //                await basePomInstitutionRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                    //            }
                    //        }
                    //        //不存在的新增
                    //        if (exceptIds.Count() != 0)
                    //        {
                    //            _logger.LogInformation($"币种信息拉取不相同数据为：{exceptIds.Count()}");
                    //            //获取不存在的机构信息
                    //            var noExistProjects = institutionList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                    //            foreach (var item in noExistProjects)
                    //            {
                    //                item.Id = GuidUtil.Next();
                    //            }
                    //            await basePomInstitutionRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    _logger.LogInformation($"机构信息拉取失败原因：{ex.Message}");
                    //    throw ex;
                    //}
                }
                //币种信息
                if (parame == 4)
                {
                    try
                    {
                        List<PomCurrencyResponseDto> currencyResponseDtos = new List<PomCurrencyResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            currencyResponseDtos.Add(item as PomCurrencyResponseDto);
                        }
                        var currencyList = mapper.Map<List<PomCurrencyResponseDto>, List<Currency>>(currencyResponseDtos);
                        if (currencyList.Any())
                        {
                            //获取币种信息表中所有pomids
                            var pomidList = baseCurrencyRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的币种信息ids
                            var obtainIds = currencyList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                _logger.LogInformation($"币种信息拉取相同数据为：{intersectIds.Count()}");
                                //获取存在的币种信息
                                var noExistProjects = currencyList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseCurrencyRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {
                                _logger.LogInformation($"币种信息拉取不相同数据为：{exceptIds.Count()}");
                                //获取不存在的币种信息
                                var noExistProjects = currencyList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseCurrencyRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"币种信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //往来单位信息
                if (parame == 5)
                {
                    await baseDealingUnitCacheRepository.AsDeleteable().ExecuteCommandAsync();
                    //广航所有往来单位数据
                    var dealingUnits = await baseDealingUnitCacheRepository.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
                    var responseData = responseResult.Result.Data as List<PomDealingUnitResponseDto>;
                    await Console.Out.WriteLineAsync($"拉取数据有:{responseData.Count}");
                    List<DealingUnitCache> dealingUnitCaches = new List<DealingUnitCache>();
                    foreach (var item in responseData)
                    {
                        var res = dealingUnits.Where(x => x.ZBP == item.ZBP && x.ZBPNAME_ZH == item.ZBPNAME_ZH).SingleOrDefault();
                        if (res == null)
                        {
                            dealingUnitCaches.Add(new DealingUnitCache()
                            {
                                ZBPNAME_ZH = item.ZBPNAME_ZH,
                                Id = Guid.NewGuid(),
                                PomId = Guid.NewGuid(),
                                ZBPNAME_EN = item.ZBPNAME_EN,
                                ZBP = item.ZBP,
                                ZBRNO = item.ZBRNO,
                                ZBPSTATE = item.ZBPSTATE,
                                ZIDNO = item.ZIDNO,
                                ZUSCC = item.ZUSCC,
                                CreateTime = DateTime.Now,
                                ZBPTYPE = item.Fzbpkinds,

                            });
                        }
                    }
                    var count = await dbContext.Fastest<DealingUnitCache>().BulkCopyAsync(dealingUnitCaches);
                    await Console.Out.WriteLineAsync($"保存成功有多少条:{count}");
                    ////循环次数
                    //var forCount = responseData.Count() % 1000M == 0 ? responseData.Count() / 1000M : Math.Floor(responseData.Count() / 1000M) + 1;

                    //if (responseData.Count() > 0)//获取到了数据 删掉原来的所有的数据 重新全部写入
                    //{
                    //    await dbContext.Deleteable<DealingUnit>().ExecuteCommandAsync();

                    //    for (int i = 0; i < forCount; i++)
                    //    {
                    //        var responsePagesData = responseData.Skip(i * 1000).Take(1000).ToList();
                    //        foreach (var item in responsePagesData)
                    //        {
                    //            item.PomId = GuidUtil.Next();
                    //            item.Id = GuidUtil.Next();
                    //            item.CreateTime = DateTime.Now;
                    //        }

                    //        var insdealingUnitList = mapper.Map<List<PomDealingUnitResponseDto>, List<DealingUnit>>(responsePagesData);
                    //        await dbContext.Fastest<DealingUnit>().BulkCopyAsync(insdealingUnitList);


                    //    }
                    //}

                    #region 原代码
                    //try
                    //{
                    //    List<PomDealingUnitResponseDto> dealingUnitResponseDtos = new List<PomDealingUnitResponseDto>();
                    //    foreach (var item in responseResult.Result.Data)
                    //    {
                    //        var newObj = item as PomDealingUnitResponseDto;
                    //        if (newObj != null)
                    //        {
                    //            newObj.PomId = newObj.Id;
                    //            newObj.Id = Guid.Empty;
                    //            dealingUnitResponseDtos.Add(newObj);
                    //        }
                    //    }
                    //    var dealingUnitList = mapper.Map<List<PomDealingUnitResponseDto>, List<DealingUnit>>(dealingUnitResponseDtos).OrderBy(x => x.PomId).ToList();
                    //    //所有往来单位数据
                    //    var dealingUnits = await baseDealingUnitRepository.AsQueryable().Where(x => x.IsDelete == 1).OrderBy(x => x.PomId).ToListAsync();
                    //    //分页
                    //    int pageIndex = 1, pageSize = 10000;
                    //    var totalPage = dealingUnits.Count / pageSize + 1;
                    //    //页码计数
                    //    var index = 0;
                    //    Console.WriteLine("开始计算:" + DateTime.Now);
                    //    if (!dealingUnits.Any())
                    //    {
                    //        foreach (var item in dealingUnitList)
                    //        {
                    //            item.Id = GuidUtil.Next();
                    //            item.CreateTime = DateTime.Now;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        while (pageIndex <= totalPage)
                    //        {
                    //            //每页显示多少
                    //            var batchDealUnit = dealingUnitList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    //            var batchIds = batchDealUnit.Select(y => y.PomId).ToList();
                    //            var batchData = dealingUnits.Where(x => batchIds.Contains(x.PomId)).ToList();
                    //            if (!batchData.Any())
                    //            {
                    //                break;
                    //            }
                    //            foreach (var item in batchDealUnit)
                    //            {
                    //                var singleDealUnit = batchData.SingleOrDefault(x => item.PomId == x.PomId);
                    //                if (singleDealUnit != null)
                    //                {
                    //                    item.Id = singleDealUnit.Id;
                    //                    item.CreateTime = DateTime.Now;
                    //                }
                    //                if (index == pageSize)
                    //                {
                    //                    Console.WriteLine("当前页码:" + pageIndex);
                    //                    pageIndex += 1;
                    //                    index = 0;
                    //                    break;
                    //                }
                    //                else
                    //                {
                    //                    index += 1;
                    //                }
                    //            }

                    //        }
                    //    }
                    //    //选择Id为空的情况(新增操作)
                    //    var nullDataList = dealingUnits.Where(x => string.IsNullOrWhiteSpace(x.Id.ToString())).ToList();
                    //    if (nullDataList.Any())
                    //    {
                    //        foreach (var item in dealingUnitList)
                    //        {
                    //            item.Id = GuidUtil.Next();
                    //            item.CreateTime = DateTime.Now;
                    //        }
                    //    }
                    //    Console.WriteLine("完成计算开始删除和插入数据:" + DateTime.Now);
                    //    await dbContext.Ado.BeginTranAsync();
                    //    await baseDealingUnitRepository.AsDeleteable().ExecuteCommandAsync();
                    //    await dbContext.Fastest<DealingUnit>().BulkCopyAsync(dealingUnitList);
                    //    await dbContext.Ado.CommitTranAsync();
                    //    Console.WriteLine(DateTime.Now);
                    //}
                    //catch (Exception ex)
                    //{
                    //    await dbContext.Ado.RollbackTranAsync();
                    //}

                    #endregion

                }
                //项目信息
                if (parame == 6)
                {
                    try
                    {
                        List<PomProjectResponseDto> projectResponseDtos = new List<PomProjectResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            projectResponseDtos.Add(item as PomProjectResponseDto);
                        }
                        var projectList = mapper.Map<List<PomProjectResponseDto>, List<Project>>(projectResponseDtos);
                        if (projectList.Any())
                        {
                            //获取项目信息表中所有pomids
                            var pomidList = baseProjectRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目信息ids
                            var obtainIds = projectList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目信息
                                var noExistProjects = projectList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    //var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseProjectRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => new { x.Id, x.CreateTime }).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目信息
                                var noExistProjects = projectList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目状态信息
                if (parame == 7)
                {
                    try
                    {
                        List<PomProjectStatusResponseDto> projectStatusResponseDtos = new List<PomProjectStatusResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            projectStatusResponseDtos.Add(item as PomProjectStatusResponseDto);
                        }
                        var projectStatusList = mapper.Map<List<PomProjectStatusResponseDto>, List<ProjectStatus>>(projectStatusResponseDtos);
                        if (projectStatusList.Any())
                        {
                            //获取项目状态信息表中所有pomids
                            var pomidList = baseProjectStatusRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.StatusId).ToList();
                            //获取拉取的项目状态信息ids
                            var obtainIds = projectStatusList.Select(x => x.StatusId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目状态信息
                                var noExistProjects = projectStatusList.Where(x => intersectIds.Contains(x.StatusId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseProjectStatusRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.StatusId).IgnoreColumns(x => new { x.Id, x.Sequence }).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目状态信息
                                var noExistProjects = projectStatusList.Where(x => exceptIds.Contains(x.StatusId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectStatusRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目状态信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目区域
                if (parame == 8)
                {
                    try
                    {
                        List<PomProjectAreaResponseDto> projectAreaResponseDtos = new List<PomProjectAreaResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            projectAreaResponseDtos.Add(item as PomProjectAreaResponseDto);
                        }
                        var projectAreaList = mapper.Map<List<PomProjectAreaResponseDto>, List<ProjectArea>>(projectAreaResponseDtos);
                        if (projectAreaList.Any())
                        {
                            //获取项目区域表中所有pomids
                            var pomidList = baseProjectAreaRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.AreaId).ToList();
                            //获取拉取的项目区域ids
                            var obtainIds = projectAreaList.Select(x => x.AreaId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目区域
                                var noExistProjects = projectAreaList.Where(x => intersectIds.Contains(x.AreaId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseProjectAreaRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.AreaId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目区域
                                var noExistProjects = projectAreaList.Where(x => exceptIds.Contains(x.AreaId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectAreaRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目区域信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目领导班子
                if (parame == 9)
                {
                    try
                    {
                        List<PomProjectLeaderResponseDto> projectLeaderResponseDtos = new List<PomProjectLeaderResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            projectLeaderResponseDtos.Add(item as PomProjectLeaderResponseDto);
                        }
                        var projectLeaderList = mapper.Map<List<PomProjectLeaderResponseDto>, List<ProjectLeader>>(projectLeaderResponseDtos);
                        if (projectLeaderList.Any())
                        {
                            //获取项目领导班子表中所有pomids
                            var pomidList = baseProjectLeaderRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目领导班子ids
                            var obtainIds = projectLeaderList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目领导班子
                                var noExistProjects = projectLeaderList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseProjectLeaderRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目领导班子
                                var noExistProjects = projectLeaderList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectLeaderRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目领导班子信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //pom机构数据
                if (parame == 10)
                {
                    try
                    {
                        List<PomInstitutionResponseDto> pomInstitutionResponseDtos = new List<PomInstitutionResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomInstitutionResponseDtos.Add(item as PomInstitutionResponseDto);
                        }
                        var pomInstitutionList = mapper.Map<List<PomInstitutionResponseDto>, List<Institution>>(pomInstitutionResponseDtos);
                        if (pomInstitutionList.Any())
                        {
                            //获取机构数据表中所有pomids
                            var pomidList = basePomInstitutionRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的机构数据ids
                            var obtainIds = pomInstitutionList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的机构数据
                                var noExistProjects = pomInstitutionList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await basePomInstitutionRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => new { x.Id, x.Status }).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的机构数据
                                var noExistProjects = pomInstitutionList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await basePomInstitutionRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"机构数据信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //pom人员数据
                if (parame == 11)
                {
                    try
                    {
                        List<PomUserResponseDto> pomUserResponseDtos = new List<PomUserResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomUserResponseDtos.Add(item as PomUserResponseDto);
                        }
                        var pomUserList = mapper.Map<List<PomUserResponseDto>, List<Model.User>>(pomUserResponseDtos);
                        if (pomUserList.Any())
                        {
                            //获取pom人员数据表中所有pomids
                            var pomids = baseUserRepository.AsQueryable().Select(x => x.IdentityCard.ToLower()).ToList();
                            //获取拉取的pom人员数据ids
                            var obtainIds = pomUserList.Select(x => x.IdentityCard.ToLower()).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的pom人员数据
                                var noExistProjects = pomUserList.Where(x => intersectIds.Contains(x.IdentityCard)).ToList();
                                if (noExistProjects.Any())
                                {
                                    var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseUserRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.IdentityCard).IgnoreColumns(x => new { x.Id, x.PomId, x.CompanyId, x.DepartmentId }).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的pom人员数据
                                var noExistProjects = pomUserList.Where(x => exceptIds.Contains(x.IdentityCard)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseUserRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"pom人员数据信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目所属省份
                if (parame == 12)
                {
                    try
                    {
                        List<PomProvinceResponseDto> ProvinceResponseDtos = new List<PomProvinceResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ProvinceResponseDtos.Add(item as PomProvinceResponseDto);
                        }
                        var ProvinceList = mapper.Map<List<PomProvinceResponseDto>, List<Province>>(ProvinceResponseDtos);
                        if (ProvinceList.Any())
                        {
                            //获取项目所属省份表中所有pomids
                            var pomidList = baseProvinceRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目所属省份ids
                            var obtainIds = ProvinceList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目所属省份
                                var noExistProjects = ProvinceList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseProvinceRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目所属省份
                                var noExistProjects = ProvinceList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProvinceRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目所属省份信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目类型
                if (parame == 13)
                {
                    try
                    {
                        List<PomProjectTypeResponseDto> ProjectTypeResponseDtos = new List<PomProjectTypeResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ProjectTypeResponseDtos.Add(item as PomProjectTypeResponseDto);
                        }
                        var ProjectTypeList = mapper.Map<List<PomProjectTypeResponseDto>, List<ProjectType>>(ProjectTypeResponseDtos);
                        if (ProjectTypeList.Any())
                        {
                            //获取项目类型表中所有pomids
                            var pomids = baseProjectTypeRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目类型ids
                            var obtainIds = ProjectTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目类型
                                var noExistProjects = ProjectTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseProjectTypeRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目类型
                                var noExistProjects = ProjectTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectTypeRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目类型信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目规模
                if (parame == 14)
                {
                    try
                    {
                        List<PomProjectScaleResponseDto> ProjectScaleResponseDtos = new List<PomProjectScaleResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ProjectScaleResponseDtos.Add(item as PomProjectScaleResponseDto);
                        }
                        var ProjectScaleList = mapper.Map<List<PomProjectScaleResponseDto>, List<ProjectScale>>(ProjectScaleResponseDtos);
                        if (ProjectScaleList.Any())
                        {
                            //获取项目规模表中所有pomids
                            var pomidList = baseProjectScaleRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目规模ids
                            var obtainIds = ProjectScaleList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目规模
                                var noExistProjects = ProjectScaleList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    //var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseProjectScaleRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目规模
                                var noExistProjects = ProjectScaleList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectScaleRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目规模信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目施工资质
                if (parame == 15)
                {
                    try
                    {
                        List<PomConstructionQualificationResponseDto> ConstructionQualificationResponseDtos = new List<PomConstructionQualificationResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ConstructionQualificationResponseDtos.Add(item as PomConstructionQualificationResponseDto);
                        }
                        var ConstructionQualificationleList = mapper.Map<List<PomConstructionQualificationResponseDto>, List<ConstructionQualification>>(ConstructionQualificationResponseDtos);
                        if (ConstructionQualificationleList.Any())
                        {
                            //获取项目施工资质表中所有pomids
                            var pomids = baseConstructionQualificationRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目施工资质ids
                            var obtainIds = ConstructionQualificationleList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目施工资质
                                var noExistProjects = ConstructionQualificationleList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseConstructionQualificationRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目施工资质
                                var noExistProjects = ConstructionQualificationleList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseConstructionQualificationRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目施工资质信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目行业分类标准
                if (parame == 16)
                {
                    try
                    {
                        List<PomIndustryClassificationResponseDto> IndustryClassificationResponseDtos = new List<PomIndustryClassificationResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            IndustryClassificationResponseDtos.Add(item as PomIndustryClassificationResponseDto);
                        }
                        var IndustryClassificationList = mapper.Map<List<PomIndustryClassificationResponseDto>, List<IndustryClassification>>(IndustryClassificationResponseDtos);
                        if (IndustryClassificationList.Any())
                        {
                            //获取项目行业分类标准表中所有pomids
                            var pomids = baseIndustryClassificationRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目行业分类标准ids
                            var obtainIds = IndustryClassificationList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目行业分类标准
                                var noExistProjects = IndustryClassificationList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseIndustryClassificationRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目行业分类标准
                                var noExistProjects = IndustryClassificationList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseIndustryClassificationRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目行业分类标准信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //水运工况级别
                if (parame == 17)
                {
                    try
                    {
                        List<PomWaterCarriageResponseDto> WaterCarriageResponseDtos = new List<PomWaterCarriageResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            WaterCarriageResponseDtos.Add(item as PomWaterCarriageResponseDto);
                        }
                        var WaterCarriageList = mapper.Map<List<PomWaterCarriageResponseDto>, List<WaterCarriage>>(WaterCarriageResponseDtos);
                        if (WaterCarriageList.Any())
                        {
                            //获取水运工况级别表中所有pomids
                            var pomids = baseWaterCarriageRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的水运工况级别ids
                            var obtainIds = WaterCarriageList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的水运工况级别
                                var noExistProjects = WaterCarriageList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await baseWaterCarriageRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的水运工况级别
                                var noExistProjects = WaterCarriageList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseWaterCarriageRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"水运工况级别信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目部
                if (parame == 18)
                {
                    try
                    {
                        List<PomProjectDepartmentResponseDto> ProjectDepartmentResponseDtos = new List<PomProjectDepartmentResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ProjectDepartmentResponseDtos.Add(item as PomProjectDepartmentResponseDto);
                        }
                        var ProjectDepartmentList = mapper.Map<List<PomProjectDepartmentResponseDto>, List<ProjectDepartment>>(ProjectDepartmentResponseDtos);
                        if (ProjectDepartmentList.Any())
                        {
                            //获取项目部表中所有pomids
                            var pomidList = baseProjectDepartmentRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目部ids
                            var obtainIds = ProjectDepartmentList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目部
                                var noExistProjects = ProjectDepartmentList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseProjectDepartmentRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目部
                                var noExistProjects = ProjectDepartmentList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseProjectDepartmentRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目部信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目干系单位
                if (parame == 19)
                {
                    try
                    {
                        List<PomProjectOrgResponseDto> ProjectOrgResponseDtos = new List<PomProjectOrgResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ProjectOrgResponseDtos.Add(item as PomProjectOrgResponseDto);
                        }
                        var ProjectOrgList = mapper.Map<List<PomProjectOrgResponseDto>, List<ProjectOrg>>(ProjectOrgResponseDtos);
                        if (ProjectOrgList.Any())
                        {
                            //获取项目干系单位表中所有pomids
                            var pomidList = basePomProjectOrgRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的项目干系单位ids
                            var obtainIds = ProjectOrgList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的项目干系单位
                                var noExistProjects = ProjectOrgList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    var pomIds = noExistProjects.Select(x => x.PomId).ToList();
                                    await basePomProjectOrgRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的项目干系单位
                                var noExistProjects = ProjectOrgList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await basePomProjectOrgRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目干系单位信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //自有船舶
                if (parame == 20)
                {
                    try
                    {
                        List<PomOwnerShipResponseDto> PomOwnerShipResponseDtos = new List<PomOwnerShipResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            PomOwnerShipResponseDtos.Add(item as PomOwnerShipResponseDto);
                        }
                        var PomOwnerShipList = mapper.Map<List<PomOwnerShipResponseDto>, List<OwnerShip>>(PomOwnerShipResponseDtos);
                        if (PomOwnerShipList.Any())
                        {
                            //获取自有船舶表中所有pomids
                            var pomids = baseOwnerShipRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的自有船舶ids
                            var obtainIds = PomOwnerShipList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的自有船舶
                                var noExistProjects = PomOwnerShipList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseOwnerShipRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的自有船舶
                                var noExistProjects = PomOwnerShipList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseOwnerShipRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"自有船舶信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //分包船舶
                if (parame == 21)
                {
                    try
                    {
                        List<PomSubShipResponseDto> PomSubShipResponseDtos = new List<PomSubShipResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            PomSubShipResponseDtos.Add(item as PomSubShipResponseDto);
                        }
                        var SubShipList = mapper.Map<List<PomSubShipResponseDto>, List<SubShip>>(PomSubShipResponseDtos);
                        if (SubShipList.Any())
                        {
                            //获取分包船舶表中所有pomids
                            var pomids = baseSubShipRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的分包船舶ids
                            var obtainIds = SubShipList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的分包船舶
                                var noExistProjects = SubShipList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseSubShipRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的分包船舶
                                var noExistProjects = SubShipList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseSubShipRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"分包船舶信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目主数据
                if (parame == 22)
                {
                    try
                    {
                        List<PomProjectMasterDataResponseDto> PomProjectMasterDataResponseDtos = new List<PomProjectMasterDataResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            PomProjectMasterDataResponseDtos.Add(item as PomProjectMasterDataResponseDto);
                        }
                        var ProjectMasterDataList = mapper.Map<List<PomProjectMasterDataResponseDto>, List<Model.ProjectMasterData>>(PomProjectMasterDataResponseDtos);
                        if (ProjectMasterDataList.Any())
                        {
                            foreach (var item in ProjectMasterDataList)
                            {
                                item.Id = GuidUtil.Next();
                            }
                            await baseProjectMasterDataRepository.InsertOrUpdateAsync(ProjectMasterDataList);
                            ////获取项目主数据表中所有pomids
                            //var pomidList = baseProjectMasterDataRepository.AsQueryable().ToList();
                            //var pomids = pomidList.AsQueryable().Select(x => x.ProjectMasterCode).ToList();
                            ////获取拉取的项目主数据ids
                            //var obtainIds = ProjectMasterDataList.Select(x => x.ProjectMasterCode).ToList();
                            //var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            //var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            ////相同的修改
                            //if (intersectIds.Count() != 0)
                            //{
                            //    //获取存在的项目主数据
                            //    var noExistProjects = ProjectMasterDataList.Where(x => intersectIds.Contains(x.ProjectMasterCode)).ToList();
                            //    if (noExistProjects.Any())
                            //    {
                            //        await baseProjectMasterDataRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.ProjectMasterCode).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                            //    }
                            //}
                            ////不存在的新增
                            //if (exceptIds.Count() != 0)
                            //{   //获取不存在的项目主数据
                            //    var noExistProjects = ProjectMasterDataList.Where(x => exceptIds.Contains(x.ProjectMasterCode)).ToList();
                            //    foreach (var item in noExistProjects)
                            //    {
                            //        item.Id = GuidUtil.Next();
                            //    }
                            //    await baseProjectMasterDataRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目主数据信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //项目年度计划产值
                if (parame == 23)
                {
                    try
                    {
                        List<ProjectYearPlanDto> pomYearPlanResponse = new List<ProjectYearPlanDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomYearPlanResponse.Add(item as ProjectYearPlanDto);
                        }
                        var data = mapper.Map<List<ProjectYearPlanDto>, List<Model.ProjectAnnualPlan>>(pomYearPlanResponse);
                        if (data.Any())
                        {
                            foreach (var item in data)
                            {
                                item.Id = GuidUtil.Next();
                            }
                            //获取存在的项目年度计划产值
                            if (data.Any())
                            {
                                await dbContext.Deleteable<ProjectAnnualPlan>().ExecuteCommandAsync();
                                await basePomProjectYearPlan.AsInsertable(data).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"项目年度计划产值拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //港口数据
                if (parame == 25)
                {
                    try
                    {
                        List<PomPortDataResponseDto> pomPortDataResponseDto = new List<PomPortDataResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomPortDataResponseDto.Add(item as PomPortDataResponseDto);
                        }
                        var pomPortDataList = mapper.Map<List<PomPortDataResponseDto>, List<PortData>>(pomPortDataResponseDto);
                        if (pomPortDataList.Any())
                        {
                            //获取港口数据表中所有pomids
                            var pomids = basePortDataRepository.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的港口数据ids
                            var obtainIds = pomPortDataList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的港口数据
                                var noExistProjects = pomPortDataList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await basePortDataRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的港口数据
                                var noExistProjects = pomPortDataList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await basePortDataRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"港口数据信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //船舶类型
                if (parame == 26)
                {
                    try
                    {
                        List<PomShipPingTypeResponseDto> pomShipPingTypeResponseDto = new List<PomShipPingTypeResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomShipPingTypeResponseDto.Add(item as PomShipPingTypeResponseDto);
                        }
                        var pomShipPingTypeList = mapper.Map<List<PomShipPingTypeResponseDto>, List<ShipPingType>>(pomShipPingTypeResponseDto);
                        if (pomShipPingTypeList.Any())
                        {
                            //获取船舶类型表中所有pomids
                            var pomidList = baseShipPingTypeRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的船舶类型ids
                            var obtainIds = pomShipPingTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseShipPingTypeRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseShipPingTypeRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"船舶类型信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //疏浚土分类
                if (parame == 27)
                {
                    try
                    {
                        List<PomSoilResponseDto> pomShipPingTypeResponseDto = new List<PomSoilResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomShipPingTypeResponseDto.Add(item as PomSoilResponseDto);
                        }
                        var pomShipPingTypeList = mapper.Map<List<PomSoilResponseDto>, List<Soil>>(pomShipPingTypeResponseDto);
                        if (pomShipPingTypeList.Any())
                        {
                            //获取船舶类型表中所有pomids
                            var pomidList = baseSoilRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的船舶类型ids
                            var obtainIds = pomShipPingTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseSoilRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseSoilRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"船舶类型信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //疏浚吹填分类
                if (parame == 28)
                {
                    try
                    {
                        List<PomShipWorkTypeResponseDto> pomShipPingTypeResponseDto = new List<PomShipWorkTypeResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomShipPingTypeResponseDto.Add(item as PomShipWorkTypeResponseDto);
                        }
                        var pomShipPingTypeList = mapper.Map<List<PomShipWorkTypeResponseDto>, List<ShipWorkType>>(pomShipPingTypeResponseDto);
                        if (pomShipPingTypeList.Any())
                        {
                            //获取船舶类型表中所有pomids
                            var pomidList = baseShipWorkTypeRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的船舶类型ids
                            var obtainIds = pomShipPingTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseShipWorkTypeRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseShipWorkTypeRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"疏浚吹填分类信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //工艺方式
                if (parame == 29)
                {
                    try
                    {
                        List<PomShipWorkModeResponseDto> pomShipPingTypeResponseDto = new List<PomShipWorkModeResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomShipPingTypeResponseDto.Add(item as PomShipWorkModeResponseDto);
                        }
                        var pomShipPingTypeList = mapper.Map<List<PomShipWorkModeResponseDto>, List<ShipWorkMode>>(pomShipPingTypeResponseDto);
                        if (pomShipPingTypeList.Any())
                        {
                            //获取船舶类型表中所有pomids
                            var pomidList = baseShipWorkModeRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的船舶类型ids
                            var obtainIds = pomShipPingTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseShipWorkModeRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseShipWorkModeRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"工艺方式信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //疏浚土分级
                if (parame == 30)
                {
                    try
                    {
                        List<PomSoilGradeResponseDto> pomShipPingTypeResponseDto = new List<PomSoilGradeResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            pomShipPingTypeResponseDto.Add(item as PomSoilGradeResponseDto);
                        }
                        var pomShipPingTypeList = mapper.Map<List<PomSoilGradeResponseDto>, List<SoilGrade>>(pomShipPingTypeResponseDto);
                        if (pomShipPingTypeList.Any())
                        {
                            //获取船舶类型表中所有pomids
                            var pomidList = baseSoilGradeRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的船舶类型ids
                            var obtainIds = pomShipPingTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseSoilGradeRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseSoilGradeRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"疏浚土分级信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
                //船级社
                if (parame == 31)
                {
                    try
                    {
                        List<PomShipClassicResponseDto> ShipClassicResponseDto = new List<PomShipClassicResponseDto>();
                        foreach (var item in responseResult.Result.Data)
                        {
                            ShipClassicResponseDto.Add(item as PomShipClassicResponseDto);
                        }
                        var pomShipPingTypeList = mapper.Map<List<PomShipClassicResponseDto>, List<ShipClassic>>(ShipClassicResponseDto);
                        if (pomShipPingTypeList.Any())
                        {
                            //获取船级社表中所有pomids
                            var pomidList = baseShipClassicRepository.AsQueryable().ToList();
                            var pomids = pomidList.AsQueryable().Select(x => x.PomId).ToList();
                            //获取拉取的船舶类型ids
                            var obtainIds = pomShipPingTypeList.Select(x => x.PomId).ToList();
                            var intersectIds = obtainIds.Intersect(pomids).ToList(); //相同的
                            var exceptIds = obtainIds.Except(pomids).ToList();//不相同的

                            //相同的修改
                            if (intersectIds.Count() != 0)
                            {
                                //获取存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => intersectIds.Contains(x.PomId)).ToList();
                                if (noExistProjects.Any())
                                {
                                    await baseShipClassicRepository.AsUpdateable(noExistProjects).WhereColumns(x => x.PomId).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                                }
                            }
                            //不存在的新增
                            if (exceptIds.Count() != 0)
                            {   //获取不存在的船舶类型
                                var noExistProjects = pomShipPingTypeList.Where(x => exceptIds.Contains(x.PomId)).ToList();
                                foreach (var item in noExistProjects)
                                {
                                    item.Id = GuidUtil.Next();
                                }
                                await baseShipClassicRepository.AsInsertable(noExistProjects).ExecuteCommandAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"船级社信息拉取失败原因：{ex.Message}");
                        throw ex;
                    }
                }
            }
            else
            {
                responseAjaxResult.Message = responseResult.Msg;
                return responseAjaxResult;
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 定时写入数据
        /// <summary>
        /// jjt发送消息数值 写入中间表  每日写一次
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> JjtWriteDataRecordAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            //在建项目  = 正常施工 +  暂停
            var pInStatusIds = new List<Guid?>() { "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(), "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(), "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(), "169c8afe-be81-4cbc-bfe7-61a40da0597b".ToGuid() };
            // 正常施工 = 在建  + 在建短暂性停工 +  在建间歇性停工
            var pConstruc = new List<Guid?>() { "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(), "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(), "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid() };
            //暂停 = 暂停长期停工
            var pSuspend = new List<Guid?>() { "169c8afe-be81-4cbc-bfe7-61a40da0597b".ToGuid() };

            //获取所有项目信息
            var projects = await dbContext.Queryable<Project>().Where(x => x.TypeId != "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid()).Select(x => new { x.Id, x.StatusId, x.PomId }).ToListAsync();
            //在建项目
            var pInBuildProjects = projects.Where(x => pInStatusIds.Contains(x.StatusId)).Count();
            //正常施工项目
            var pConstrucProjects = projects.Where(x => pConstruc.Contains(x.StatusId)).Count();
            //暂停项目
            var pSuspendProjects = projects.Where(x => pSuspend.Contains(x.StatusId)).Count();
            //取所有项目ids
            var pIds = projects.Where(x => pInStatusIds.Contains(x.StatusId) || pConstruc.Contains(x.StatusId) || pSuspend.Contains(x.StatusId)).Select(x => x.Id).ToList();
            //日期转换
            int nowDay = ConvertHelper.ToDateDay(DateTime.Now.AddDays(-1));
            //获取项目当日总产值
            var pOutputVal = dbContext.Queryable<DayReport>().Where(x => pIds.Contains(x.ProjectId) && x.DateDay == nowDay).Sum(x => x.DayActualProductionAmount);
            //本年项目开累产值
            var pAccumulateOutput = dbContext.Queryable<DayReport>().Where(x => pIds.Contains(x.ProjectId) && Convert.ToDateTime(x.CreateTime).Year == DateTime.Now.AddDays(-1).Year).Sum(x => x.DayActualProductionAmount);
            //获取日均计划产值   取年度计划当前月 123月取1  456取3  789取6 101112 取9
            //获取pom年度计划项目ids
            var yearPlanPids = projects.Select(x => x.PomId).ToList();
            var quarter = (DateTime.Now.AddDays(-1).Month + 2) / 3;//获取当前月份所属季度
            var inDate = new DateTime(DateTime.Now.AddDays(-1).Year, quarter == 1 ? 1 : quarter == 2 ? 3 : quarter == 3 ? 6 : 9, 1);
            //var days = 1;           //Math.Abs(DateTime.Now.Day - 25);绝对值
            //if (DateTime.Now.Day > 25)
            //{
            //    //当日大于25号  获取当月自然月天数-当天+25=剩余天数
            //    days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
            //    if (days == 0) days = days + 26; //28-28=0  31-31=0 当月最后一天   1+25
            //    else days = days + 25;//25号之后未知剩余多少天  如当天为26号  当月大月31天  剩余天数=days+25
            //}
            //else if (DateTime.Now.Day < 25)
            //{
            //    //当日小于25号  25-当日天数=剩余天数
            //    days = 25 - DateTime.Now.Day;
            //}
            //else
            //{
            //    //当日为25号  只剩当天
            //    days = 1;
            //}
            //已经过了多少天
            //var days = 0;
            //当天
            //var nowDateDay = DateTime.Now.AddDays(-1).Day;
            //if (nowDateDay > 25)
            //{
            //    days = nowDateDay - 25;
            //}
            //else
            //{
            //    //获取上月自然月天数
            //    var lDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month) - 25;//上月过了多少天
            //    //截至到今天过了多少天
            //    days = nowDateDay + lDays;
            //}
            var proYearPlanData = await dbContext.Queryable<ProjectAnnualPlan>().Where(x => x.ProjectId.Length != 36 && yearPlanPids.Contains(Convert.ToInt32(x.ProjectId)) && x.SubmitYear == inDate).ToListAsync();
            //获取昨日项目累计日均计划产值
            var IsexistlastDayUptoOutputVal = await dbContext.Queryable<JjtWriteDataRecord>().Where(x => x.NowDay == nowDay).FirstAsync();
            //项目日均计划产值
            var uptoDayNowOutputVal = decimal.Zero;
            //当日日均加上昨日累计日均计划产值为今日项目计划产值累计
            var uptoNowDayPlanAccumulateOutputVal = decimal.Zero;
            if (quarter == 1)//一季度获取数据1-3月计划产值
            {
                if (DateTime.Now.AddDays(-1).Month == 1)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.OnePlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 2)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.TwoPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 3)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.ThreePlannedOutputValue);
            }
            else if (quarter == 2)//二季度获取数据4-6月计划产值
            {
                if (DateTime.Now.AddDays(-1).Month == 4)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.FourPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 5)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.FivPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 6)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.SixPlannedOutputValue);
            }
            else if (quarter == 3)//三季度获取数据7-9月计划产值
            {
                if (DateTime.Now.AddDays(-1).Month == 7)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.SevPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 8)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.EigPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 9)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.NinPlannedOutputValue);
            }
            else//四季度获取数据10-12月计划产值
            {
                if (DateTime.Now.AddDays(-1).Month == 10)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.TenPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 11)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.ElePlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 12)
                    uptoNowDayPlanAccumulateOutputVal = proYearPlanData.Sum(x => x.TwePlannedOutputValue);
            }
            //新的月
            if (DateTime.Now.AddDays(-1).Day == 26)
            {
                pAccumulateOutput = 0M;
            }
            //项目日均计划产值
            uptoDayNowOutputVal = uptoNowDayPlanAccumulateOutputVal * 10000 / 30.5M;
            //当日日均加上昨日累计日均计划产值为今日项目计划产值累计   已过天数*日均=累计计划产值
            //uptoNowDayPlanAccumulateOutputVal = DateTime.Now.AddDays(-1).Day == 26 ? uptoDayNowOutputVal : uptoDayNowOutputVal * days;
            // 如果当天为新的月总日均计划产值为当日日均计划产值
            uptoNowDayPlanAccumulateOutputVal = DateTime.Now.AddDays(-1).Day == 26 ? uptoDayNowOutputVal : IsexistlastDayUptoOutputVal == null ? uptoDayNowOutputVal : IsexistlastDayUptoOutputVal.UptoNowDayPlanAccumulateOutputVal + uptoDayNowOutputVal;
            //船舶类型
            var shipTypes = CommonData.ShipType.Split(',').ToList();
            //获取广航局所有自有船舶
            var ownShips = await dbContext.Queryable<OwnerShip>().Select(x => new { x.Id, x.PomId, x.StatusId, x.TypeId }).ToListAsync();
            // 获取当日自有船舶总产值
            var shipIds = ownShips.Where(x => shipTypes.Contains(x.TypeId.ToString())).ToList().Select(x => x.PomId);
            var ownShipData = await dbContext.Queryable<ShipDayReport>().Where(x => shipIds.Contains(x.ShipId) && x.DateDay == nowDay).ToListAsync();
            var ownShipOutputVal = ownShipData.Sum(x => Convert.ToDecimal(x.EstimatedOutputAmount));
            //获取当日正常施工船舶
            var inWorkShips = ownShipData.Where(x => x.ShipState != ProjectShipState.Standby).Count();
            //获取当日停置船舶
            var stopShips = ownShipData.Where(x => x.ShipState == ProjectShipState.Standby).Count();
            //获取自有船舶开累产值
            var ownShipAccumulateOutputVal = dbContext.Queryable<ShipDayReport>().Where(x => shipIds.Contains(x.ShipId) && Convert.ToDateTime(x.CreateTime).Year == DateTime.Now.AddDays(-1).Year).Sum(x => Convert.ToDecimal(x.EstimatedOutputAmount));
            var shipYearPlanData = await dbContext.Queryable<ShipYearPlan>().Where(x => shipIds.Contains(x.ShipId.Value) && x.SubmitYear == inDate).ToListAsync();
            //船舶日均计划产值
            var shipDayNowOutputVal = decimal.Zero;
            //当日日均加上昨日累计日均计划产值为今日船舶计划产值累计
            var shipUptoNowDayPlanAccumulateOutputVal = decimal.Zero;
            if (quarter == 1)//   船舶取当月 
            {
                if (DateTime.Now.AddDays(-1).Month == 1)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.OnePlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 2)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.TwoPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 3)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.ThreePlannedOutputValue);
            }
            else if (quarter == 2)
            {
                if (DateTime.Now.AddDays(-1).Month == 4)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.FourPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 5)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.FivPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 6)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.SixPlannedOutputValue);
            }
            else if (quarter == 3)
            {
                if (DateTime.Now.AddDays(-1).Month == 7)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.SevPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 8)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.EigPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 9)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.NinPlannedOutputValue);
            }
            else
            {
                if (DateTime.Now.AddDays(-1).Month == 10)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.TenPlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 11)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.ElePlannedOutputValue);
                if (DateTime.Now.AddDays(-1).Month == 12)
                    shipUptoNowDayPlanAccumulateOutputVal = shipYearPlanData.Sum(x => x.TwePlannedOutputValue);
            }
            //新的一月
            if (DateTime.Now.AddDays(-1).Day == 26)
            {
                ownShipOutputVal = 0M;//完工产值为0
            }
            // 船舶日均计划产值 =计划合计/天数
            shipDayNowOutputVal = shipUptoNowDayPlanAccumulateOutputVal * 10000 / 30.5M;
            //跨年暂不考虑
            //当日日均加上昨日累计日均计划产值为今日船舶计划产值累计 已过天数* 日均=累计计划产值
            //shipUptoNowDayPlanAccumulateOutputVal = DateTime.Now.AddDays(-1).Day == 26 ? shipDayNowOutputVal : shipDayNowOutputVal * days;
            //如果当天为新的月总日均计划产值为当日日均计划产值
            shipUptoNowDayPlanAccumulateOutputVal = DateTime.Now.AddDays(-1).Day == 26 ? shipDayNowOutputVal : IsexistlastDayUptoOutputVal == null ? shipDayNowOutputVal : IsexistlastDayUptoOutputVal.ShipUptoNowDayPlanAccumulateOutputVal + shipDayNowOutputVal;

            //当日数据是否已经写过
            int nowTime = ConvertHelper.ToDateDay(DateTime.Now);
            var isNowDayData = await dbContext.Queryable<JjtWriteDataRecord>().Where(x => x.NowDay == nowTime).FirstAsync();
            if (isNowDayData != null)
            {
                isNowDayData.PAccumulateOutput = pAccumulateOutput;
                isNowDayData.POutputVal = pOutputVal;
                isNowDayData.UptoDayNowOutputVal = uptoDayNowOutputVal;
                isNowDayData.UptoNowDayPlanAccumulateOutputVal = uptoNowDayPlanAccumulateOutputVal;
                isNowDayData.OwnShipAccumulateOutputVal = ownShipAccumulateOutputVal;
                isNowDayData.OwnShipOutputVal = ownShipOutputVal;
                isNowDayData.PInbuildNums = pInBuildProjects;
                isNowDayData.PConstrucNums = pConstrucProjects;
                isNowDayData.pSuspendNums = pSuspendProjects;
                isNowDayData.OwnShipConstrucNums = inWorkShips;
                isNowDayData.OwnShipNums = ownShips.Count();
                isNowDayData.OwnShipStopNums = stopShips;
                isNowDayData.ShipUptoDayNowOutputVal = shipDayNowOutputVal;
                isNowDayData.ShipUptoNowDayPlanAccumulateOutputVal = shipUptoNowDayPlanAccumulateOutputVal;
                await dbContext.Updateable(isNowDayData).WhereColumns(x => new { x.Id }).ExecuteCommandAsync();
            }
            else
            {
                var data = new JjtWriteDataRecordResponseDto
                {
                    CreateTime = DateTime.Now,
                    NowDay = DateTime.Now.ToDateDay(),
                    Id = GuidUtil.Next(),
                    PAccumulateOutput = pAccumulateOutput,
                    POutputVal = pOutputVal,
                    UptoDayNowOutputVal = uptoDayNowOutputVal,
                    UptoNowDayPlanAccumulateOutputVal = uptoNowDayPlanAccumulateOutputVal,
                    OwnShipAccumulateOutputVal = ownShipAccumulateOutputVal,
                    OwnShipOutputVal = ownShipOutputVal,
                    PInbuildNums = pInBuildProjects,
                    PConstrucNums = pConstrucProjects,
                    pSuspendNums = pSuspendProjects,
                    OwnShipConstrucNums = inWorkShips,
                    OwnShipNums = ownShips.Count(),
                    OwnShipStopNums = stopShips,
                    ShipUptoDayNowOutputVal = shipDayNowOutputVal,
                    ShipUptoNowDayPlanAccumulateOutputVal = shipUptoNowDayPlanAccumulateOutputVal,
                    TextType = 1
                };
                var result = mapper.Map<JjtWriteDataRecordResponseDto, JjtWriteDataRecord>(data);
                await dbContext.Insertable(result).ExecuteCommandAsync();
            }
            responseAjaxResult.Data = true;
            return responseAjaxResult;
        }



        #endregion


        #region 定时同步每个公司的每天的完成产值汇总 
        /// <summary>
        /// 定时同步每个公司的每天的完成产值汇总 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SynchronizationDayProductionAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            #region 筛选条件
            var year = 2025;
            var currentDay = DateTime.Now.Day;
            var startTime = string.Empty;
            var endTime = string.Empty;
            var month = 1;
            if (currentDay >= 26)
            {
                startTime = DateTime.Now.ToString("yyyyMM26");
                endTime = DateTime.Now.AddMonths(1).ToString("yyyyMM26");
                month = DateTime.Now.AddMonths(1).Month;
            }
            else
            {
                startTime = DateTime.Now.AddMonths(-1).ToString("yyyyMM26");
                endTime = DateTime.Now.ToString("yyyyMM26");
                month = DateTime.Now.Month;
            }

            #endregion

            var companyProducitonValueList = await dbContext.Queryable<CompanyProductionValueInfo>().Where(x => x.IsDelete == 1 && x.DateDay == year
            && x.Sort != 0).ToListAsync();
            var companyIds = companyProducitonValueList.Select(x => x.CompanyId).ToList();
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            if (projectList.Any())
            {
                //查询当前中期的日报
                var dayProjectProductionList = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1 && x.ProcessStatus == DayReportProcessStatus.Submited
                 && x.DateDay >= SqlFunc.ToInt32(startTime)
                 && x.DateDay <= SqlFunc.ToInt32(endTime)
                 ).ToListAsync();
                if (dayProjectProductionList.Any())
                {
                    //SumAsync(x => x.DayActualProductionAmount)
                    List<CompanyProductionValueInfo> updateDatas = new List<CompanyProductionValueInfo>();
                    foreach (var item in companyProducitonValueList)
                    {
                        var ids = projectList.Where(x => x.CompanyId == item.CompanyId).Select(x => x.Id).ToList();
                        //添加交建公司水工项目
                        //if (ids.Count != 0 && item.CompanyId == "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid())
                        //{
                        //    ids.Add("08dcdec4-4d90-4802-80fe-1293e55fbdff".ToGuid());
                        //}
                        var dayProjectProduction = dayProjectProductionList.Where(x => ids.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);
                        var currentCompany = companyProducitonValueList.Where(x => x.CompanyId == item.CompanyId).FirstOrDefault();
                        currentCompany = UpdateCompanyMonthProduction(currentCompany, month, dayProjectProduction);
                        updateDatas.Add(currentCompany);
                    }
                    if (updateDatas.Any())
                    {
                        var flag = await dbContext.Updateable<CompanyProductionValueInfo>(updateDatas).Where(x => x.DateDay == year).ExecuteCommandAsync();
                        if (flag > 0)
                        {
                            responseAjaxResult.Data = true;
                            responseAjaxResult.Success();
                        }
                        else
                        {
                            responseAjaxResult.Data = false;
                            responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, Domain.Shared.Enums.HttpStatusCode.UpdateFail);
                        }
                    }
                }
            }
            return responseAjaxResult;
        }


        /// <summary>
        /// 更新公司每个月的产值数据
        /// </summary>
        /// <param name="companyProductionValueInfos"></param>
        /// <param name="month"></param>
        /// <param name="monthProduction"></param>
        /// <returns></returns>
        private CompanyProductionValueInfo UpdateCompanyMonthProduction(CompanyProductionValueInfo companyProductionValueInfos, int month, decimal monthProduction)
        {

            if (month == 1)
            {
                companyProductionValueInfos.OneCompleteProductionValue = monthProduction;
            }
            else if (month == 2)
            {
                companyProductionValueInfos.TwoCompleteProductionValue = monthProduction;
            }
            else if (month == 3)
            {
                companyProductionValueInfos.ThreeCompleteProductionValue = monthProduction;
            }
            else if (month == 4)
            {
                companyProductionValueInfos.FourCompleteProductionValue = monthProduction;
            }
            else if (month == 5)
            {
                companyProductionValueInfos.FiveCompleteProductionValue = monthProduction;
            }
            else if (month == 6)
            {
                companyProductionValueInfos.SixCompleteProductionValue = monthProduction;
            }
            else if (month == 7)
            {
                companyProductionValueInfos.SevenCompleteProductionValue = monthProduction;
            }
            else if (month == 8)
            {
                companyProductionValueInfos.EightCompleteProductionValue = monthProduction;
            }
            else if (month == 9)
            {
                companyProductionValueInfos.NineCompleteProductionValue = monthProduction;
            }
            else if (month == 10)
            {
                companyProductionValueInfos.TenCompleteProductionValue = monthProduction;
            }
            else if (month == 11)
            {
                companyProductionValueInfos.ElevenCompleteProductionValue = monthProduction;
            }
            else if (month == 12)
            {
                companyProductionValueInfos.TwelveCompleteProductionValue = monthProduction;
            }
            return companyProductionValueInfos;

        }
        #endregion



        /// <summary>
        /// 定时同步往来单位数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> SynchronizationDealUnitsync()
        {
            try
            {
                //中间表
                var baseDealCache = await baseDealingUnitCacheRepository.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
                //目标表
                var baseDeal = await baseDealingUnitRepository.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
                List<DealingUnit> dealingUnits = new List<DealingUnit>();
                var zbp = baseDeal.Select(x => x.ZBP).ToList();
                var baseDealCachea = baseDealCache.Where(x => !zbp.Contains(x.ZBP)).ToList();
                foreach (var item in baseDealCachea)
                {
                    dealingUnits.Add(new DealingUnit()
                    {
                        ZBPNAME_ZH = item.ZBPNAME_ZH,
                        Id = Guid.NewGuid(),
                        PomId = Guid.NewGuid(),
                        ZBPNAME_EN = item.ZBPNAME_EN,
                        ZBP = item.ZBP,
                        ZBRNO = item.ZBRNO,
                        ZBPSTATE = item.ZBPSTATE,
                        ZIDNO = item.ZIDNO,
                        ZUSCC = item.ZUSCC,
                        CreateTime = DateTime.Now,
                        ZBPTYPE = item.ZBPTYPE,
                    });
                }
                // var flag= await dbContext.Insertable<DealingUnit>(dealingUnits).ExecuteCommandAsync();
                var flag = await dbContext.Fastest<DealingUnit>().BulkCopyAsync(dealingUnits);
                return flag > 0 ? "成功" : "失败";
            }
            catch (Exception ex)
            {
                return (ex.Message + Environment.NewLine + ex.StackTrace).ToString();
            }
        }


        /// <summary>
        /// 临时代码   
        /// </summary>
        /// <returns></returns>

        public async Task<string> SynchronizationDealUnitNewsync()
        {
            try
            {
                //中间表
                var baseDealCache = await baseDealingUnitCacheRepository.AsQueryable().Where(x => x.IsDelete == 1 && (x.ZBPTYPE == "02" || x.ZBPTYPE == "03")).ToListAsync();
                //目标表   
                var aa = baseDealCache.Select(x => x.ZBP).ToList();
                var baseDeal = await baseDealingUnitRepository.AsQueryable().Where(x => x.IsDelete == 1 && aa.Contains(x.ZBP)).ToListAsync();
                List<DealingUnit> dealingUnits = new List<DealingUnit>();
                var a = 0;
                foreach (var item in baseDeal)
                {
                    var sing = baseDealCache.Where(x => x.ZBP == item.ZBP).SingleOrDefault();
                    if (sing != null)
                    {
                        item.ZBPTYPE = sing.ZBPTYPE;
                        dealingUnits.Add(item);
                    }
                    a += 1;
                }
                // var flag= await dbContext.Insertable<DealingUnit>(dealingUnits).ExecuteCommandAsync();
                var flag = await dbContext.Updateable<DealingUnit>(dealingUnits).ExecuteCommandAsync();
                return flag > 0 ? "成功" : "失败";
            }
            catch (Exception ex)
            {
                return (ex.Message + Environment.NewLine + ex.StackTrace).ToString();
            }
        }
        /// <summary>
        /// 获取技术危大施工方案信息
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSpecialConstruction()
        {
            var url = AppsettingsHelper.GetValue("JingWeiSpecialConstruction");
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJVc2VyTmFtZSI6InNwYWRtaW5AY2NnZGMuY29tJDE2MzczMDEzOTUwMDBcdTAwMDAiLCJSb2xlcyI6WyJhZG1pbiIsImNvbW1vbnJvbGUiXSwiSXNBZG1pbiI6ZmFsc2UsIkV4cGlyeURhdGVUaW1lIjoiMjI1MC0wMS0yMVQxOTowMDo1Mi45NTY1ODcrMDg6MDAifQ.tg4JhClRQX2HxuIsjUzNo-Vissn4UBdskWnSDj0uZY0";

            WebHelper webHelper = new();
            webHelper.Headers.Add("auth", token);

            Dictionary<string, object> di = new();
            di.Add("Code", "");
            var responseResult = await webHelper.DoPostAsync<SpecialConstructionResponse>(url, di);
            if (responseResult.Result.Code == "20000")//请求成功
            {
                List<DangerousDetails> insertTable = new();
                foreach (var item in responseResult.Result.Data.RspBody.List)
                {
                    insertTable.Add(new DangerousDetails
                    {
                        Id = GuidUtil.Next(),
                        ProjectId = item.ProjectId,
                        FangAnName = item.FangAnName,
                        FangAnStatus = item.FangAnStatus,
                        ProjectCode = item.ProjectCode,
                        ApprovalState = item.ApprovalState,
                        EndTime = item.EndTime,
                        ErrorOrNot = item.ErrorOrNot,
                        ExpertOpinionFileObjectID = item.ExpertOpinionFileObjectID,
                        FangAnFileObjectID = item.FangAnFileObjectID,
                        CreatedTime = item.CreatedTime,
                        FangAnType = item.FangAnType,
                        FinishTime = item.FinishTime,
                        ManageDepartment = item.ManageDepartment,
                        StartTime = item.StartTime,
                        CreateTime = DateTime.Now,
                        FangAnKind = item.FangAnKind
                    });
                }

                if (insertTable.Any()) await dbContext.Fastest<DangerousDetails>().BulkCopyAsync(insertTable);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 刷项目日报填报信息
        /// </summary>
        /// <param name="daytime"></param>
        /// <param name="isAll">如果等于treu 刷新最近一个月的数据</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SynchronizationProjectReportAsync(int daytime, bool isAll=false)
        {
            //本月周期开始时间（统计周期  月开始和结束时间 用于过滤日报数据的条件）
            var startMonthTime = DateTime.Now.ToDateDay() > int.Parse(DateTime.Now.ToString("yyyyMM26")) ? DateTime.Now.ToString("yyyyMM26").ObjToInt() : DateTime.Now.AddMonths(-1).ToString("yyyyMM26").ObjToInt();
            var endMonthTime = DateTime.Now.ToDateDay() > int.Parse(DateTime.Now.ToString("yyyyMM26")) ? DateTime.Now.AddYears(1).ToString("yyyyMM25").ObjToInt() : DateTime.Now.ToString("yyyyMM25").ObjToInt();

            var dayList= await dbContext.Queryable<DayWriteReportRecord>().Where(x => x.IsDelete == 1 && x.DateDay >= startMonthTime && x.DateDay <= endMonthTime).ToListAsync();
            foreach (var item in dayList)
            {
                item.IsDelete = 0;
            }
            if (dayList.Count > 0)
            {
                await dbContext.Updateable<DayWriteReportRecord>(dayList).ExecuteCommandAsync();
            }
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            List<DayWriteReportRecord> dayWriteReportRecords = new List<DayWriteReportRecord>();
           var dayReports=await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1)
                .WhereIF(isAll,x=>x.DateDay >= startMonthTime && x.DateDay <= endMonthTime)
                .WhereIF(!isAll&&daytime != 0, x => x.DateDay == daytime)
                .WhereIF(!isAll && daytime == 0, x => x.DateDay == DateTime.Now.AddDays(-1).ToDateDay())
                .Select(x=>new DayWriteReportRecord() { ProjectId=x.ProjectId,DateDay=x.DateDay })
                .ToListAsync();
            foreach (var item in dayReports)
            {
                var dayRecord = new DayWriteReportRecord()
                {
                    Id = GuidUtil.Next(),
                    ProjectId=item.ProjectId,
                    CreateTime = DateTime.Now,
                    CreateId = Guid.Empty,
                    DateDay = item.DateDay,
                };
                #region 日期计算
                var value =item.DateDay.ToString().Substring(6, 2).ObjToInt();
                if (value == 26)
                {
                    dayRecord.TwentySix = 1;
                }
                else if (value == 27)
                {
                    dayRecord.TwentySeven = 1;
                }
                else if (value == 28)
                {
                    dayRecord.TwentyEight = 1;
                }
                else if (value == 29)
                {
                    dayRecord.TwentyNine = 1;
                }
                else if (value == 30)
                {
                    dayRecord.Thirty = 1;
                }
                else if (value == 31)
                {
                    dayRecord.TwentyOne = 1;
                }
                else if (value == 1)
                {
                    dayRecord.One = 1;
                }
                else if (value == 2)
                {
                    dayRecord.Two = 1;
                }
                else if (value == 3)
                {
                    dayRecord.Three = 1;
                }
                else if (value == 4)
                {
                    dayRecord.Four = 1;
                }
                else if (value == 5)
                {
                    dayRecord.Five = 1;
                }
                else if (value == 6)
                {
                    dayRecord.Six = 1;
                }
                else if (value == 7)
                {
                    dayRecord.Seven = 1;
                }
                else if (value == 8)
                {
                    dayRecord.Eight = 1;
                }
                else if (value == 9)
                {
                    dayRecord.Nine = 1;
                }
                else if (value == 10)
                {
                    dayRecord.Ten = 1;
                }
                else if (value == 11)
                {
                    dayRecord.Eleven = 1;
                }
                else if (value == 12)
                {
                    dayRecord.Twelve = 1;
                }
                else if (value == 13)
                {
                    dayRecord.Thirteen = 1;
                }
                else if (value == 14)
                {
                    dayRecord.Fourteen = 1;
                }
                else if (value == 15)
                {
                    dayRecord.Fifteen = 1;
                }
                else if (value == 16)
                {
                    dayRecord.Sixteen = 1;
                }
                else if (value == 17)
                {
                    dayRecord.Seventeen = 1;
                }
                else if (value == 18)
                {
                    dayRecord.Eighteen = 1;
                }
                else if (value == 19)
                {
                    dayRecord.Nineteen = 1;
                }
                else if (value == 20)
                {
                    dayRecord.Twenty = 1;
                }
                else if (value == 21)
                {
                    dayRecord.TwentyOne = 1;
                }
                else if (value == 22)
                {
                    dayRecord.TwentyTwo = 1;
                }
                else if (value == 23)
                {
                    dayRecord.TwentyThree = 1;
                }
                else if (value == 24)
                {
                    dayRecord.TwentyFour = 1;
                }
                else if (value == 25)
                {
                    dayRecord.TwentyFive = 1;
                }
                #endregion
                dayWriteReportRecords.Add(dayRecord);
            }
            //await dbContext.Fastest<DayWriteReportRecord>().BulkCopyAsync(dayWriteReportRecords);
            await dbContext.Insertable<DayWriteReportRecord>(dayWriteReportRecords).ExecuteCommandAsync();
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        
    }
}
