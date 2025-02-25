using AutoMapper;
using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ApprovalUser;
using GHMonitoringCenterApi.Application.Contracts.Dto.AttributeLabelc;
using GHMonitoringCenterApi.Application.Contracts.Dto.Common;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionQualification;
using GHMonitoringCenterApi.Application.Contracts.Dto.Currency;
using GHMonitoringCenterApi.Application.Contracts.Dto.DictionaryTable;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.Dto.Holiday;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.Institution;
using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectArea;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload;
using GHMonitoringCenterApi.Application.Contracts.Dto.Province;
using GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.Subsidiary;
using GHMonitoringCenterApi.Application.Contracts.Dto.User;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTaste;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using SixLabors.ImageSharp;
using SqlSugar;
using SqlSugar.Extensions;
using UtilsSharp;
using UtilsSharp.Shared.Standard;
using static System.Net.Mime.MediaTypeNames;
using Model = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Service
{
    /// <summary>
    /// 基本service实现类
    /// </summary>
    public class BaseService : IBaseService
    {
        #region 依赖注入
        public ILogger<BaseService> logger { get; set; }

        public IBaseRepository<DealingUnit> baseDealingUnitRepository { get; set; }
        public IBaseRepository<Currency> baseCurrencyRepository { get; set; }
        public IBaseRepository<ProjectDepartment> baseProjectDepartmentRepository { get; set; }
        public IBaseRepository<AttributeLabelc> baseAttributeLabelcRepository { get; set; }
        public IBaseRepository<ConstructionQualification> baseConstructionQualificationRepository { get; set; }
        public IBaseRepository<IndustryClassification> baseIndustryClassificationRepository { get; set; }
        public IBaseRepository<WaterCarriage> baseWaterCarriageRepository { get; set; }
        public IBaseRepository<DictionaryTable> baseDictionaryTableRepository { get; set; }
        public IBaseRepository<ProjectScale> baseProjectScaleRepository { get; set; }
        public IBaseRepository<Province> baseProvinceRepository { get; set; }
        public IBaseRepository<ProjectType> baseProjectTypeRepository { get; set; }
        public IBaseRepository<Institution> baseInstitutionRepository { get; set; }
        public IBaseRepository<ProjectStatus> baseProjectStatusRepository { get; set; }
        public IBaseRepository<ProjectArea> baseProjectAreaRepository { get; set; }
        public IBaseRepository<ProjectLeader> baseProjectLeaderRepository { get; set; }
        public IBaseRepository<Model.User> baseUserRepository { get; set; }
        public IBaseRepository<OwnerShip> baseOwnerShipRepository { get; set; }
        public IBaseRepository<SubShip> baseSubShipRepository { get; set; }



        public IMapper mapper { get; set; }
        public ISqlSugarClient dbContext { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        public BaseService(ILogger<BaseService> logger,
            IBaseRepository<DealingUnit> baseDealingUnitRepository, IBaseRepository<Currency> baseCurrencyRepository, IBaseRepository<ProjectDepartment> baseProjectDepartmentRepository, IBaseRepository<AttributeLabelc> baseAttributeLabelcRepository, IBaseRepository<ConstructionQualification> baseConstructionQualificationRepository, IBaseRepository<IndustryClassification> baseIndustryClassificationRepository, IBaseRepository<WaterCarriage> baseWaterCarriageRepository, IBaseRepository<DictionaryTable> baseDictionaryTableRepository, IBaseRepository<ProjectScale> baseProjectScaleRepository, IBaseRepository<Province> baseProvinceRepository, IBaseRepository<ProjectType> baseProjectTypeRepository, IBaseRepository<Institution> baseInstitutionRepository, IBaseRepository<ProjectStatus> baseProjectStatusRepository, IBaseRepository<ProjectArea> baseProjectAreaRepository, IBaseRepository<ProjectLeader> baseProjectLeaderRepository, IBaseRepository<Model.User> baseUserRepository, IBaseRepository<OwnerShip> baseOwnerShipRepository, IBaseRepository<SubShip> baseSubShipRepository, IMapper mapper, ISqlSugarClient dbContext, GlobalObject globalObject)
        {
            this.logger = logger;

            this.baseDealingUnitRepository = baseDealingUnitRepository;
            this.baseCurrencyRepository = baseCurrencyRepository;
            this.baseProjectDepartmentRepository = baseProjectDepartmentRepository;
            this.baseAttributeLabelcRepository = baseAttributeLabelcRepository;
            this.baseConstructionQualificationRepository = baseConstructionQualificationRepository;
            this.baseIndustryClassificationRepository = baseIndustryClassificationRepository;
            this.baseWaterCarriageRepository = baseWaterCarriageRepository;
            this.baseDictionaryTableRepository = baseDictionaryTableRepository;
            this.baseProjectScaleRepository = baseProjectScaleRepository;
            this.baseProvinceRepository = baseProvinceRepository;
            this.baseProjectTypeRepository = baseProjectTypeRepository;
            this.baseInstitutionRepository = baseInstitutionRepository;
            this.baseProjectStatusRepository = baseProjectStatusRepository;
            this.baseProjectAreaRepository = baseProjectAreaRepository;
            this.baseProjectLeaderRepository = baseProjectLeaderRepository;
            this.baseUserRepository = baseUserRepository;
            this.baseOwnerShipRepository = baseOwnerShipRepository;
            this.baseSubShipRepository = baseSubShipRepository;

            this.mapper = mapper;
            this.dbContext = dbContext;
            _globalObject = globalObject;
        }




        #endregion

        #region 所属公司下拉列表接口
        /// <summary>
        /// 所属公司下拉列表
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyPullDownAsync(string oid, string keywords)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var result = new List<BasePullDownResponseDto>();

            #region 废弃不用代码
            ////if (oid != "101162350")//广航局
            ////{
            ////    var isContainsABC = await baseInstitutionRepository.AsQueryable().Where(x => x.Oid == oid).FirstAsync();//ocode是否包含abc  包含公司就不向上查 
            ////    if (isContainsABC != null && (isContainsABC.Ocode.Contains("00A") || isContainsABC.Ocode.Contains("00B") || isContainsABC.Ocode.Contains("00C")))
            ////    {
            ////        result = await baseInstitutionRepository.AsQueryable()
            ////                 .Where(x => x.IsDelete == 1 && x.Status != "4" && !string.IsNullOrWhiteSpace(x.Ocode) && (x.Ocode.Contains("00A") ||
            ////                 x.Ocode.Contains("00B") || x.Ocode.Contains("00C")) && (x.Oid == oid || x.Poid == oid))
            ////                 .WhereIF(!string.IsNullOrWhiteSpace(keywords), x => SqlFunc.Contains(x.Name, keywords))
            ////                 .OrderBy(x => x.Sno)
            ////                 .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name })
            ////                 .ToListAsync();
            ////    }
            ////    else
            ////    {
            ////        //拆分当前Grule
            ////        var orule = isContainsABC.Grule.Split('-').ToArray().Reverse().Where(x => x != oid && !string.IsNullOrWhiteSpace(x)).ToArray();
            ////        foreach (var item in orule)
            ////        {
            ////            if (result.Count() == 0)//有数据就不继续走循环
            ////            {
            ////                var isContains = await baseInstitutionRepository.AsQueryable().Where(x => x.Oid == item).FirstAsync();
            ////                if (isContains != null && (isContains.Ocode.Contains("00A") || isContains.Ocode.Contains("00B") || isContains.Ocode.Contains("00C")))
            ////                {
            ////                    var data = await baseInstitutionRepository.AsQueryable()
            ////                              .Where(x => x.IsDelete == 1 && x.Status != "4" && !string.IsNullOrWhiteSpace(x.Ocode) && (x.Ocode.Contains("00A") || x.Ocode.Contains("00B") || x.Ocode.Contains("00C")) && (x.Oid == isContains.Oid || x.Poid == isContains.Oid))
            ////                              .WhereIF(!string.IsNullOrWhiteSpace(keywords), x => SqlFunc.Contains(x.Name, keywords))
            ////                              .OrderBy(x => x.Sno)
            ////                              .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name })
            ////                              .ToListAsync();
            ////                    if (data.Count() != 0)
            ////                    {
            ////                        result = data;
            ////                    }
            ////                }
            ////            }
            ////        }
            ////    }
            ////}
            ////else
            ////{
            ////    result = await baseInstitutionRepository.AsQueryable()
            ////             .Where(x => x.IsDelete == 1 && x.Status != "4" && !string.IsNullOrWhiteSpace(x.Ocode) && (x.Ocode.Contains("00A") ||
            ////             x.Ocode.Contains("00B") || x.Ocode.Contains("00C")))
            ////             .WhereIF(!string.IsNullOrWhiteSpace(keywords), x => SqlFunc.Contains(x.Name, keywords))
            ////             .OrderBy(x => x.Sno)
            ////             .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name })
            ////             .ToListAsync();
            ////}



            //var institutionList = await baseInstitutionRepository.AsQueryable()
            //     .Where(x => x.IsDelete == 1 && x.Status != "4" && !string.IsNullOrWhiteSpace(x.Ocode) && (x.Ocode.Contains("00A") ||
            //     x.Ocode.Contains("00B") || x.Ocode.Contains("00C")))
            //     .WhereIF(oid == "101162350", x => (x.Oid == oid || x.Poid == oid))
            //     .WhereIF(oid != "101162350", x => x.Poid == oid || x.Oid == oid)
            //     .WhereIF(!string.IsNullOrWhiteSpace(keywords), x => SqlFunc.Contains(x.Name, keywords))
            //     .OrderBy(x => x.Sno)
            //     .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name })
            //     .ToListAsync();
            //responseAjaxResult.Data = institutionList;
            //responseAjaxResult.Count = institutionList.Count;
            //responseAjaxResult.Success();
            //return responseAjaxResult;
            #endregion

            var currentInstitutionList = await baseInstitutionRepository.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            if (currentInstitutionList != null && currentInstitutionList.Any())
            {
                if (currentInstitutionList.SingleOrDefault(x => x.Oid == oid
                && (x.Ocode.Contains("0000A") || x.Ocode.Contains("0000H")) && (x.Oid == "101162351" || x.Oid == "101162350")) != null)
                {
                    //判断当前机构是否是广航局总部的 如果是总部就是最高权限  查询广航局的所有公司 
                    result = currentInstitutionList.Where(x =>
                         x.Status != "4"
                         && !string.IsNullOrWhiteSpace(x.Ocode)
                         && (x.Ocode.Contains("0000A") || x.Ocode.Contains("0000B") || x.Ocode.Contains("0000C") || x.Ocode.Contains("0000E")))
                          .WhereIF(!string.IsNullOrWhiteSpace(keywords), x => SqlFunc.Contains(x.Name, keywords))
                          .OrderBy(x => x.Sno)
                          .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name })
                          .ToList();
                }
                else
                {
                    //说明是非广航局总部角色  获取当前角色所属的直级公司
                    var currentAffCompanyInfo = await GetCurrentInstitutionParent(oid);
                    result.Add(new BasePullDownResponseDto()
                    {
                        Name = currentAffCompanyInfo.AffCompanyFullName,
                        Id = currentAffCompanyInfo.CPomId,
                    });
                }
            }
            responseAjaxResult.Data = result;
            responseAjaxResult.Count = result.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;

        }

        #endregion
        /// <summary>
        /// 获取机构ids
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<List<Guid?>> GetCompanyIds()
        {
            var oids = await baseInstitutionRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            //获取机构数据
            var institutionId = await SearchCompanySubPullDownAsync(oids.PomId.Value);
            return institutionId.Data.Select(x => x.Id).ToList();
        }

        #region 获取当前公司的所有子公司  暂时不用
        /// <summary>
        /// 获取当前公司的所有子公司暂时不用
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SubordinateCompaniesDto>>> SearchSubsidiaryPullDownAsync(SubsidiaryRequsetDto subsidiaryRequsetDto)
        {
            ResponseAjaxResult<List<SubordinateCompaniesDto>> responseAjaxResult = new ResponseAjaxResult<List<SubordinateCompaniesDto>>();
            List<SubordinateCompaniesDto> SubordinateCompaniesDtos = new List<SubordinateCompaniesDto>();
            var SubordinateCompaniess = baseInstitutionRepository.AsQueryable().Where(x => x.PomId == subsidiaryRequsetDto.Id).Select(x => x.Oid).First();
            var SubordinateCompanies = await dbContext.Queryable<Institution>()
                .Select(x => new Institution { Name = x.Shortname, Oid = x.Oid, Poid = x.Poid }).ToListAsync();
            if (SubordinateCompanies.Any())
            {
                foreach (var item in SubordinateCompanies)
                {
                    SubordinateCompaniesDto classSubordinate = new SubordinateCompaniesDto()
                    {
                        KeyId = item.Oid,
                        Name = item.Shortname,
                        Pid = item.Poid
                    };
                    SubordinateCompaniesDtos.Add(classSubordinate);
                }
                if (SubordinateCompaniesDtos.Any())
                {
                    SubordinateCompaniesDtos = ListToTreeUtil.GetTree(SubordinateCompaniess, SubordinateCompaniesDtos);
                }
            }
            responseAjaxResult.Data = SubordinateCompaniesDtos;
            responseAjaxResult.Count = SubordinateCompaniesDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取当前公司下面的所有子公司的部门Id
        /// <summary>
        /// 获取当前公司下面的所有子公司的部门Id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userList">是否是查询当前部门下面的用户使用 默认不是</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanySubPullDownAsync(Guid companyId, bool userList = false, bool department = false)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            List<Guid> ids = new List<Guid>();
            List<BasePullDownResponseDto> departmentList = new List<BasePullDownResponseDto>();
            Institution? institutionSingle = await baseInstitutionRepository.AsQueryable().SingleAsync(x => x.Status != "4" && x.PomId == companyId);
            var institutionList = await baseInstitutionRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.Status != "4")
                 .Select(x => new InstitutionTreeResponseDto() { Id = x.PomId.Value, PomId = x.PomId.Value, Name = x.Shortname, KeyId = x.Oid, Pid = x.Poid, Ocode = x.Ocode, Grule = x.Grule, Oid = x.Oid, Poid = x.Poid, Sort = x.Sno })
                 .OrderBy(x => x.Sort).ToListAsync();

            if (institutionList.Any())
            {
                #region 新增逻辑
                var isCompany = false;//是否是公司角色进来的
                string? rootNode = "101114066";
                var currentInstitutionInfo = await GetCurrentInstitutionParent(institutionSingle.Oid);
                if (currentInstitutionInfo.Coid != "101162350" && !string.IsNullOrWhiteSpace(currentInstitutionInfo.AffDepartmentName))
                {
                    //说明不是直属广航局总部的 且是某个公司项目部进来的项目部进来的
                    institutionSingle.Poid = currentInstitutionInfo.Dpoid;
                    rootNode = currentInstitutionInfo.Doid;
                }
                else if (currentInstitutionInfo.Coid != "101162350" && string.IsNullOrWhiteSpace(currentInstitutionInfo.AffDepartmentName))
                {
                    //说明不是直属广航局总部的  且是某个公司进来的 
                    institutionSingle.Poid = currentInstitutionInfo.Cpoid;
                    rootNode = currentInstitutionInfo.Coid;
                    isCompany = true;
                }
                else if (currentInstitutionInfo.Coid == "101162350" && !string.IsNullOrWhiteSpace(currentInstitutionInfo.AffDepartmentName))
                {
                    //说明是直属广航局总部的  且是属于广航局直属项目部的
                    institutionSingle.Poid = currentInstitutionInfo.Dpoid;
                    rootNode = currentInstitutionInfo.Doid;

                }
                else if (currentInstitutionInfo.Coid == "101162350" && string.IsNullOrWhiteSpace(currentInstitutionInfo.AffDepartmentName))
                {
                    //说明是直属广航局总部的  且不是以项目部角色进来的
                    institutionSingle.Poid = currentInstitutionInfo.Cpoid;
                    rootNode = currentInstitutionInfo.Coid;
                    isCompany = true;
                }
                #endregion


                InstitutionTreeResponseDto? rootNodeItem = null;
                if (institutionSingle.Poid == "101114066")
                {
                    rootNodeItem = institutionList.FirstOrDefault(x => x.Poid == institutionSingle.Poid);
                }
                else
                {
                    //rootNode = institutionSingle?.Oid;
                    //rootNodeItem = institutionList.FirstOrDefault(x => x.Oid == institutionSingle.Oid);
                    rootNodeItem = institutionList.FirstOrDefault(x => x.Oid == rootNode);
                }
                if (rootNode == "101114066" || rootNode == "101162350")
                {
                    var departmentsKey = AppsettingsHelper.GetValue("InstitutionTreeKey:InstitutionSonNodeDepartments");
                    var redis = RedisUtil.Instance;
                    if (await redis.ExistsAsync(departmentsKey))
                    {
                        ids = await redis.GetAsync<List<Guid>>(departmentsKey);
                    }
                    //else {
                    //   //此地方不做任何逻辑   从redis里面获取一定会存在除非redis宕机

                    //}
                }
                else
                {
                    //如果是公司进来的  查询当前公司下面的所有节点
                    if (isCompany)
                    {
                        var len = rootNodeItem?.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length + 1;
                        ids = ListToTreeUtil.GetTree<InstitutionTreeResponseDto>(rootNode, institutionList, len.Value, rootNodeItem?.Grule).Item2;
                    }
                    else
                    {
                        if (department)
                        {
                            var len = rootNodeItem?.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length + 1;
                            ids = ListToTreeUtil.GetTree<InstitutionTreeResponseDto>(rootNode, institutionList, len.Value, rootNodeItem?.Grule).Item2;

                            if (ids.Any())
                            {
                                departmentList = await baseInstitutionRepository.AsQueryable()
                              .Where(x => x.IsDelete == 1)
                              .Where(x => ids.Contains((Guid)x.PomId))
                              .WhereIF(isCompany, x => x.Ocode.Contains("0000P") || x.Ocode.Contains("0000B") || x.Ocode.Contains("0000M") || x.Ocode.Contains("0000E") || x.Ocode.Contains("0000C"))
                               .Select(x => new BasePullDownResponseDto() { Id = x.PomId, Name = x.Shortname }).ToListAsync();
                            }
                        }

                    }

                }

                if (isCompany)
                {
                    // department  = true只针对获取部门下项目信息下拉列表接口
                    if (department)
                    {
                        departmentList = await baseInstitutionRepository.AsQueryable()
                          .Where(x => x.IsDelete == 1)
                          .Where(x => ids.Contains((Guid)x.PomId))
                          .WhereIF(isCompany, x => x.Ocode.Contains("0000P") || x.Ocode.Contains("0000B") || x.Ocode.Contains("0000M") || x.Ocode.Contains("0000E") || x.Ocode.Contains("0000C"))
                           .Select(x => new BasePullDownResponseDto() { Id = x.PomId, Name = x.Shortname }).ToListAsync();
                    }
                    else
                    {
                        departmentList = await baseInstitutionRepository.AsQueryable()
                          .Where(x => x.IsDelete == 1)
                          .Where(x => ids.Contains((Guid)x.PomId))
                          .WhereIF(isCompany, x => x.Ocode.Contains("0000P") || x.Ocode.Contains("0000M"))
                           .Select(x => new BasePullDownResponseDto() { Id = x.PomId, Name = x.Shortname }).ToListAsync();
                    }
                }
                else
                {
                    departmentList.Add(new BasePullDownResponseDto() { Id = currentInstitutionInfo.DPomId, Name = currentInstitutionInfo.AffDepartmentName });
                }
                if (userList)
                {
                    //此部分逻辑只有查询当前部门下面的用户使用其他地方暂时不使用  对应模块角色分配--》选择机构树的时候对应的部门用户
                    var len = rootNodeItem?.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length + 1;
                    ids = ListToTreeUtil.GetTree<InstitutionTreeResponseDto>(rootNode, institutionList, len.Value, rootNodeItem?.Grule).Item2;
                    if (ids.Any())
                    {
                        foreach (var item in ids)
                        {
                            departmentList.Add(new BasePullDownResponseDto() { Id = item });
                        }
                    }
                }

                var isDept = institutionList.SingleOrDefault(x => x.Oid == _currentUser.CurrentLoginInstitutionOid);
                if (isDept != null && isDept.Ocode.IndexOf("0000P") > 0)
                {
                    //说明是某个项目部进来的  这个时候 只需要看到自己的项目部信息即可
                    // departmentList = departmentList.Where(x => x.Id == isDept.Id).ToList();
                }


                //获取临时补录项目部
                //var depData = await dbContext.Queryable<ProjectDepartment>().ToListAsync();

                //depData.ForEach(x => departmentList.Add(new BasePullDownResponseDto
                //{
                //    Id = x.PomId,
                //    Name = x.Name
                //}));

                //if (departmentList.Count != 0)
                //{
                //    //说明不是交建公司  或者不是超级管理员 或者不是陈翠   就不让他看水工项目
                //    var len = _currentUser.CurrentLoginInstitutionGrule.IndexOf("101174265");
                //    if (!_currentUser.CurrentLoginIsAdmin && _currentUser.Account != "2016146340"&& len <= 0)
                //    {
                //        var res = departmentList.Where(x => x.Id == "00e9d13d-5678-47b5-90be-504985a31b1e".ToGuid()).FirstOrDefault();
                //        if (res != null)
                //        {
                //            departmentList.Remove(res);
                //        }
                //    }

                //}
                responseAjaxResult.Data = departmentList;
                responseAjaxResult.Count = departmentList.Count;
            }

            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 项目区域下拉列表接口
        /// <summary>
        /// 项目区域下拉列表接口
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectAreaPullDownAsync(ProjectAreaRequsetDto projectAreaRequsetDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ProjectAreaList = await baseProjectAreaRepository.AsQueryable()
               .WhereIF(!string.IsNullOrWhiteSpace(projectAreaRequsetDto.Name), x => SqlFunc.Contains(x.Name, projectAreaRequsetDto.Name))
               .Select(x => new BasePullDownResponseDto { Id = x.AreaId, Name = x.Name })
               .ToListAsync();

            responseAjaxResult.Data = ProjectAreaList;
            responseAjaxResult.Count = ProjectAreaList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取项目省份下拉列表接口
        /// <summary>
        /// 获取项目省份列表数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectProvincePullDownAsync(ProvinceRequsetDto baseprovinceRequestDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var provinceList = await baseProvinceRepository.AsQueryable()
                .WhereIF(true, x => x.RegionId == baseprovinceRequestDto.Regionid)
                .WhereIF(!string.IsNullOrWhiteSpace(baseprovinceRequestDto.Name), x => SqlFunc.Contains(x.Zaddvsname, baseprovinceRequestDto.Name))
                .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Zaddvsname }).ToListAsync();
            responseAjaxResult.Data = provinceList;
            responseAjaxResult.Count = provinceList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取项目状态下拉列表接口
        /// <summary>
        /// 获取项目状态下拉列表接口
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectStatusPullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var projectStatusList = await baseProjectStatusRepository.AsQueryable()
                .OrderBy(x => x.Sequence)
                  .Select(x => new BasePullDownResponseDto { Id = x.StatusId, Name = x.Name })
                  .ToListAsync();
            responseAjaxResult.Data = projectStatusList;
            responseAjaxResult.Count = projectStatusList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 行业分类标准下拉列表接口
        /// <summary>
        /// 行业分类标准下拉列表接口
        /// </summary>
        /// <returns></returns>

        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchIndustryClassificationPullDownAsync(IndustryClassificationRequsetDto IndustryClassificationResponseDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var IndustryClassificationList = await baseIndustryClassificationRepository.AsQueryable()
               .WhereIF(IndustryClassificationResponseDto.Id == null || IndustryClassificationResponseDto.Id == Guid.Empty, x => string.IsNullOrWhiteSpace(x.ParentId.ToString()))
               .WhereIF(IndustryClassificationResponseDto.Id.HasValue, x => x.ParentId == IndustryClassificationResponseDto.Id)// x => x.ParentId == null)//x =>x.ParentId == IndustryClassificationResponseDto.Id)
               .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name }).ToListAsync();
            responseAjaxResult.Data = IndustryClassificationList;
            responseAjaxResult.Count = IndustryClassificationList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 项目施工资质下拉列表接口
        /// <summary>
        ///  项目施工资质
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchConstructionQualificationPullDownAsync(ConstructionQualificationRequsetDto qualificationRequsetDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ConstructionQualificationList = await baseConstructionQualificationRepository.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(qualificationRequsetDto.Name), x => SqlFunc.Contains(x.Name, qualificationRequsetDto.Name))
                .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name }).ToListAsync();
            responseAjaxResult.Data = ConstructionQualificationList;
            responseAjaxResult.Count = ConstructionQualificationList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 水运工况级别下拉列表接口
        /// <summary>
        /// 水运工况级别
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchWaterCarriagePullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var WaterCarriageList = await baseWaterCarriageRepository.AsQueryable()
                .OrderBy(x => x.Grade)
                .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Remarks, Type = Convert.ToInt32(x.Grade) }).ToListAsync();
            responseAjaxResult.Data = WaterCarriageList;
            responseAjaxResult.Count = WaterCarriageList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取项目类型下拉列表接口

        /// <summary>
        /// 获取项目类型下拉列表接口
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectTypePullDownAsync(ProjectTypRequsetDto projectTypRequsetDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ProjectTypeList = await baseProjectTypeRepository.AsQueryable()
             .Where(x => x.BusinessRemark != null)
            .WhereIF(!string.IsNullOrWhiteSpace(projectTypRequsetDto.Name), x => SqlFunc.Contains(x.Name, projectTypRequsetDto.Name))
                .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Code = x.BusinessRemark }).ToListAsync();
            if (projectTypRequsetDto.EnableRemark == 1)
            {
                responseAjaxResult.Data = ProjectTypeList.Select(x => new BasePullDownResponseDto() { Id = x.Id, Name = x.Name + x.Code }).ToList();
            }
            else
            {
                responseAjaxResult.Data = ProjectTypeList;
            }
            responseAjaxResult.Count = ProjectTypeList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 干系单位类型下拉列表接口
        /// <summary>
        /// 干系单位类型
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchDictionaryTableTreeAsyncc(DictionaryTableRequestDto baseDictionaryTableRequestDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var DictionaryTableList = await baseDictionaryTableRepository.AsQueryable()
                .Where(x => x.TypeNo == baseDictionaryTableRequestDto.Type)
                .Select(x => new BasePullDownResponseDto { Type = x.Type, Name = x.Name, Code = x.Remark })
                .OrderBy(x => x.Type).ToListAsync();
            responseAjaxResult.Data = DictionaryTableList;
            responseAjaxResult.Count = DictionaryTableList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        # region 类型项目规模下拉列表接口
        /// <summary>
        /// 类型项目规模 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectScalePullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ProjectScaleList = await baseProjectScaleRepository.AsQueryable()
                .OrderBy(x => x.Sequence)
                 .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name }).ToListAsync();
            responseAjaxResult.Data = ProjectScaleList;
            responseAjaxResult.Count = ProjectScaleList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 属性标签下拉列表接口
        /// <summary>
        /// 属性标签
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchAttributeLabelcPullDownAsync(AttributeLabelcResqusetDto attributeLabelcResqusetDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var AttributeLabelcList = await baseAttributeLabelcRepository.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(attributeLabelcResqusetDto.Name), x => SqlFunc.Contains(x.Attribute, attributeLabelcResqusetDto.Name))
                 .Select(x => new BasePullDownResponseDto { Name = x.Attribute }).ToListAsync();
            responseAjaxResult.Data = AttributeLabelcList;
            responseAjaxResult.Count = AttributeLabelcList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 项目部下拉列表接口 暂时不用
        /// <summary>
        /// 项目部  暂时不用
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectDepartmentPullDownAsync(ProjectDepartmentRuqusetDto projectDepartmentRuqusetDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ProjectDepartmentList = await baseProjectDepartmentRepository.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(projectDepartmentRuqusetDto.Name), x => SqlFunc.Contains(x.Name, projectDepartmentRuqusetDto.Name))
                 .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name }).ToListAsync();
            responseAjaxResult.Data = ProjectDepartmentList;
            responseAjaxResult.Count = ProjectDepartmentList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 币种
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCurrencyPullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            //人民币、美元、欧元
            var code = new string[] { "156", "840", "947" };
            var CurrencyList = await baseCurrencyRepository.AsQueryable()
                .Where(x => code.Contains(x.Zcurrencycode))
                 .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Zcurrencyname }).ToListAsync();
            responseAjaxResult.Data = CurrencyList;
            responseAjaxResult.Count = CurrencyList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;

        }
        #endregion

        #region 获取组织机构数  懒加载模式
        /// <summary>
        /// 获取组织机构数
        /// </summary>
        /// <param name="poid"></param>
        /// <param name="defaultNode">默认根节点的父节点</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchInstitutionTreeAsync(string poid, string defaultNode = "101114066")
        {
            ResponseAjaxResult<List<InstitutionTreeResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<InstitutionTreeResponseDto>>();
            List<InstitutionTreeResponseDto> institutionTreeResponseDtos = new List<InstitutionTreeResponseDto>();
            var currentNodeLength = 0;
            var currentNodeOid = string.Empty;
            Institution institutionSingle = new Institution();
            if (poid == defaultNode)
            {
                institutionSingle = await baseInstitutionRepository.AsQueryable().SingleAsync(x => x.Poid == poid);
            }
            else
            {
                institutionSingle = await baseInstitutionRepository.AsQueryable().SingleAsync(x => x.Oid == poid);
            }
            currentNodeOid = institutionSingle?.Oid;
            currentNodeLength = institutionSingle.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length + 1;
            institutionTreeResponseDtos = await baseInstitutionRepository.AsQueryable()
                .WhereIF(poid == defaultNode, x => x.Poid == poid)
                .WhereIF(poid != defaultNode, x => SqlFunc.Contains(x.Grule, institutionSingle.Grule))
                .Select(x => new InstitutionTreeResponseDto() { Id = x.PomId.Value, Name = x.Shortname, Grule = x.Grule, Oid = x.Oid, Poid = x.Poid, Sort = x.Sno })
                .ToListAsync();
            //过滤掉不符合的数据
            if (poid != defaultNode)
            {
                institutionTreeResponseDtos = institutionTreeResponseDtos.Where(x => x.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length == currentNodeLength).ToList();
            }

            //判断是否存在叶子节点
            if (institutionTreeResponseDtos.Any())
            {
                var institutionList = await baseInstitutionRepository.AsQueryable().ToListAsync();
                if (institutionList.Any())
                {
                    foreach (var currentNode in institutionTreeResponseDtos)
                    {
                        currentNodeLength = currentNode.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length + 1;
                        currentNode.IsNode = institutionList.Count(x => SqlFunc.Contains(x.Grule, currentNode.Grule) && x.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length == currentNodeLength) > 1 ? false : true;
                    }
                    institutionTreeResponseDtos = institutionTreeResponseDtos.OrderBy(x => x.Sort).ToList();
                }
            }
            responseAjaxResult.Data = institutionTreeResponseDtos;
            responseAjaxResult.Count = institutionTreeResponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取组织机构树  非懒加载模式
        /// <summary>
        /// 获取机构组织树  非懒加载模式 
        /// </summary>
        /// <param name="defaultRootNode"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchInstitutionNoLazyLoadingTreeAsync(string rootNode)
        {
            ResponseAjaxResult<List<InstitutionTreeResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<InstitutionTreeResponseDto>>();
            List<InstitutionTreeResponseDto> institutionTreeResponseDtos = new List<InstitutionTreeResponseDto>();
            var institutionList = await baseInstitutionRepository.AsQueryable()
                .Where(x => x.IsDelete == 1 && x.Status != "4"
                //&& (
                // string.IsNullOrEmpty(x.Ocode) ||
                // x.Ocode.Contains("0000A") ||
                // x.Ocode.Contains("0000B") ||
                // x.Ocode.Contains("0000C") ||
                // x.Ocode.Contains("0000P"))
                 )
                 .Select(x => new InstitutionTreeResponseDto() { PomId = x.PomId.Value, Id = x.PomId.Value, Name = x.Shortname, KeyId = x.Oid, Pid = x.Poid, Grule = x.Grule, Oid = x.Oid, Poid = x.Poid, Sort = x.Sno })
                 .OrderBy(x => x.Sort).ToListAsync();
            if (institutionList.Any())
            {
                #region 特殊处理  如果是广航局总部进来的进行机构提级  提级为广航局
                //只要grule里面含有101162351  他就必是广航局总部的  就应该提级为广航局机构
                var institution = institutionList.SingleOrDefault(x => x.Oid == rootNode && x.Grule.IndexOf("101162351") >= 0);
                if (institution != null)
                {
                    rootNode = "101114066";
                }
                #endregion

                InstitutionTreeResponseDto rootNodeItem = null;
                if (rootNode == "101114066")
                {
                    rootNodeItem = institutionList.FirstOrDefault(x => x.Poid == rootNode);
                }
                else
                {
                    rootNodeItem = institutionList.FirstOrDefault(x => x.Oid == rootNode);
                }
                var len = rootNodeItem?.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length + 1;
                if (rootNode != null && rootNode == "101114066")
                {
                    var redis = RedisUtil.Instance;
                    var key = AppsettingsHelper.GetValue("InstitutionTreeKey:InstitutionKey");
                    var departmentsKey = AppsettingsHelper.GetValue("InstitutionTreeKey:InstitutionSonNodeDepartments");
                    if (await redis.ExistsAsync(key))
                    {
                        institutionTreeResponseDtos = await redis.GetAsync<List<InstitutionTreeResponseDto>>(key);

                    }
                    else
                    {
                        var treeResult = ListToTreeUtil.GetTree<InstitutionTreeResponseDto>(rootNode, institutionList, len.Value, rootNodeItem?.Grule);
                        await redis.SetAsync(key, treeResult.Item1, -1);
                        await redis.SetAsync(departmentsKey, treeResult.Item2, -1);
                        institutionTreeResponseDtos = await redis.GetAsync<List<InstitutionTreeResponseDto>>(key);
                    }
                }
                else
                {
                    var treeResult = ListToTreeUtil.GetTree<InstitutionTreeResponseDto>(rootNode, institutionList, len.Value, rootNodeItem?.Grule).Item1;
                    if (rootNode != "101114066")
                    {
                        var data = await GetCurrentAllPreNodeAsync(rootNodeItem?.Grule, treeResult);
                        institutionTreeResponseDtos.Clear();
                        institutionTreeResponseDtos.Add(data);
                    }
                }

                responseAjaxResult.Data = institutionTreeResponseDtos;
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取人员类型的人员信息
        /// <summary>
        /// 获取职位类型的人员信息
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchPositionTypeUserDownAsync(PositionUserRequestDto positionUserRequestDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            List<BasePullDownResponseDto> userList = new List<BasePullDownResponseDto>();
            userList = await baseUserRepository.AsQueryable().Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(positionUserRequestDto.KeyWords), x => SqlFunc.Contains(x.Name, positionUserRequestDto.KeyWords)
                || SqlFunc.Contains(x.Phone, positionUserRequestDto.KeyWords))
                .Select(x => new BasePullDownResponseDto() { Name = x.Name + "(" + x.Phone + ")", Type = positionUserRequestDto.Type, Id = x.PomId })
                .ToListAsync();
            responseAjaxResult.Data = userList;
            responseAjaxResult.Count = userList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 根据名称或手机号获取人员信息
        /// <summary>
        /// 根据名称或手机号获取人员信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchUserDownAsync(BaseRequestDto baseRequestDto)
        {
            int Quantity = 0;
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            List<BasePullDownResponseDto> userList = new List<BasePullDownResponseDto>();
            userList = baseUserRepository.AsQueryable().Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => SqlFunc.Contains(x.Name, baseRequestDto.KeyWords)
                || SqlFunc.Contains(x.Phone, baseRequestDto.KeyWords))
                .Select(x => new BasePullDownResponseDto() { Name = x.Name + "(" + x.Phone + ")", Code = x.Phone, Id = x.PomId })
                .ToPageList(baseRequestDto.PageIndex, baseRequestDto.PageSize, ref Quantity);

            responseAjaxResult.Data = userList;
            responseAjaxResult.Count = Quantity;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 获取行业分类标准
        public async Task<ResponseAjaxResult<List<ClassifyStandardResponseDto>>> SerchClassifyTreeListAsync()
        {
            ResponseAjaxResult<List<ClassifyStandardResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ClassifyStandardResponseDto>>();
            List<ClassifyStandardResponseDto> classResponseDtos = new List<ClassifyStandardResponseDto>();
            var classList = await dbContext.Queryable<IndustryClassification>().Select(x => new IndustryClassification { PomId = x.PomId, ParentId = x.ParentId, Name = x.Name }).ToListAsync();
            if (classList.Any())
            {
                foreach (var item in classList)
                {
                    ClassifyStandardResponseDto classResponseDto = new ClassifyStandardResponseDto()
                    {
                        KeyId = item.PomId.ToString(),
                        Pid = item.ParentId.ToString(),
                        Name = item.Name,
                    };
                    classResponseDtos.Add(classResponseDto);
                }
                if (classResponseDtos.Any())
                {
                    classResponseDtos = ListToTreeUtil.GetTree("", classResponseDtos);
                }
            }

            responseAjaxResult.Data = classResponseDtos;
            responseAjaxResult.Count = classResponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 往来单位名称名称下拉表接口
        /// <summary>
        /// 往来单位名称名称
        /// </summary>
        /// <param name="dealingUnitRequseDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchDealingUnitPullDownAsync(BaseRequestDto dealingUnitRequseDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ProjectDealingUnitList = await baseDealingUnitRepository.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(dealingUnitRequseDto.KeyWords), x => SqlFunc.Contains(x.ZBPNAME_ZH, dealingUnitRequseDto.KeyWords))
                // .Where(x=>x.ZBPNAME_ZH.Contains("机关") || x.ZBPNAME_ZH.Contains("本部") || x.ZBPNAME_ZH.Contains("纳税人"))
                .Where(x => !x.ZBPNAME_ZH.Contains("汇总）") && !x.ZBPNAME_ZH.Contains("合并）"))
                // .Where(x => x.ZBPTYPE == "01"||x.ZBPTYPE == "02" || x.ZBPTYPE == "03")
                .OrderBy(x => x.ZBPNAME_ZH)
                 .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.ZBPNAME_ZH })
                 .ToListAsync();
            ProjectDealingUnitList = ProjectDealingUnitList.Where(x => !Utils.EndsWithParenthesis(x.Name)).OrderBy(x => x.Name).ToList();

            responseAjaxResult.Data = ProjectDealingUnitList;
            responseAjaxResult.Count = ProjectDealingUnitList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 境外区域地点模糊查询
        /// <summary>
        /// 境外区域地点模糊查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectAreaRegionResponseDto>>> SearchOverAreaRegionAsync(BaseKeyWordsRequestDto requestDto)
        {
            ResponseAjaxResult<List<ProjectAreaRegionResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProjectAreaRegionResponseDto>>();
            //获取所有境外地点
            var areaList = await baseProvinceRepository.AsQueryable()
                .LeftJoin<ProjectArea>((x, y) => x.RegionId == y.AreaId)
                .Where(x => x.IsDelete == 1 && x.Overseas == "1")
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Zaddvsname, requestDto.KeyWords))
                .Select((x, y) => new ProjectAreaRegionResponseDto { AreaId = x.PomId, AreaName = x.Zaddvsname, RegionId = x.RegionId, RegionName = y.Name }).ToListAsync();
            responseAjaxResult.Data = areaList;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 自有分包船舶 往来单位模糊查询
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchOwnerSubShipAsync(ShipRequestDto requestDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            RefAsync<int> total = 0;
            if (requestDto.Type == 1)
            {
                //获取自有船舶数据
                var ownerList = await baseOwnerShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.OwnerShip })
                    .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                responseAjaxResult.Data = ownerList;
                responseAjaxResult.Count = ownerList.Count;
                responseAjaxResult.Success();
            }
            if (requestDto.Type == 2)
            {
                //获取往来单位数据
                var ProjectDealingUnitList = await baseDealingUnitRepository.AsQueryable()
                     .Where(x => !string.IsNullOrWhiteSpace(x.ZUSCC))
                     .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.ZBPNAME_ZH, requestDto.KeyWords))
                     .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.ZBPNAME_ZH, Type = (int)ShipType.SubBusinessUnit })
                     .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                //获取分包船舶数据
                var subList = await baseSubShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.SubShip })
                    .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                var data = ProjectDealingUnitList.Union(subList).ToList();

                responseAjaxResult.Data = data;
                responseAjaxResult.Count = data.Count;
                responseAjaxResult.Success();
            }
            if (requestDto.Type == 3)
            {
                //获取分包船舶数据
                var subList = await baseSubShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.SubShip })
                    .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                responseAjaxResult.Data = subList;
                responseAjaxResult.Count = total;
                responseAjaxResult.Success();
            }
            if (requestDto.Type == 4)
            {
                //自有船舶  且船舶类型 是耙吸船    绞吸  抓斗船三种
                List<Guid> gUIDs = new List<Guid>();
                gUIDs.Add("06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf".ToGuid());
                gUIDs.Add("f1718922-c213-4409-a59f-fdaf3d6c5e23".ToGuid());
                gUIDs.Add("6959792d-27a4-4f2b-8fa4-a44222f08cb2".ToGuid());
                //获取自有船舶数据
                var ownerList = await baseOwnerShipRepository.AsQueryable()
                    .Where(x => gUIDs.Contains(x.TypeId.Value))
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.OwnerShip })
                    .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                responseAjaxResult.Data = ownerList;
                responseAjaxResult.Count = ownerList.Count;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
        #endregion

        #region 获取当前节点的所有上级
        /// <summary>
        /// 获取当前节点的所有上级
        /// </summary>
        /// <param name="grule"></param>
        /// <returns></returns>
        public async Task<InstitutionTreeResponseDto> GetCurrentAllPreNodeAsync(string grule, List<InstitutionTreeResponseDto> data)
        {

            var rootNode = await baseInstitutionRepository.AsQueryable().SingleAsync(x => x.Oid == "101162350");
            List<InstitutionTreeResponseDto> institutionTreeResponseDto = new List<InstitutionTreeResponseDto>();
            InstitutionTreeResponseDto institutionRootTreeResponseDto = new InstitutionTreeResponseDto()
            {
                PomId = rootNode.PomId.Value,
                Id = rootNode.PomId.Value,
                Oid = rootNode.Oid,
                Poid = rootNode.Poid,
                Name = rootNode.Shortname,
                Grule = rootNode.Grule,
                Node = new List<InstitutionTreeResponseDto>()
            };

            var baseGrule = "-104396-104400-101114066-101114070-101162350-";
            if (grule == baseGrule)
            {
                return institutionRootTreeResponseDto;
            }
            else
            {
                var gruleArray = grule.Replace(baseGrule, "-").Split("-", StringSplitOptions.RemoveEmptyEntries).Select(x => x).ToList();
                var institutionList = await baseInstitutionRepository.AsQueryable()
                      .Where(x => x.IsDelete == 1 && gruleArray.Contains(x.Oid))
                      .Select(x => new InstitutionTreeResponseDto() { Id = x.PomId.Value, PomId = x.PomId.Value, Name = x.Shortname, KeyId = x.Oid, Pid = x.Poid, Grule = x.Grule, Oid = x.Oid, Poid = x.Poid, Sort = x.Sno })
                      .ToListAsync();
                institutionList = institutionList.OrderByDescending(x => x.Grule).ToList();
                if (institutionList.Any())
                {
                    for (int i = 0; i < institutionList.Count; i++)
                    {
                        List<InstitutionTreeResponseDto> institutionTreeResponseDtos = new List<InstitutionTreeResponseDto>();
                        institutionTreeResponseDtos.Add(institutionList[i]);
                        if (i == 0)
                        {
                            institutionList[i].Node = data;
                        }
                        if (i == institutionList.Count - 1)
                        {

                            institutionRootTreeResponseDto.Node = institutionTreeResponseDtos;
                        }
                        else
                        {
                            institutionList[i + 1].Node = institutionTreeResponseDtos;
                        }
                    }
                }
            }
            return institutionRootTreeResponseDto;
        }

        #endregion

        #region 项目列表导出字段查询
        /// <summary>
        /// 项目列表导出字段查询
        /// </summary>
        /// <returns></returns>
        public ResponseAjaxResult<List<BasePullDownResponseDto>> SearchProjectImportColumns()
        {

            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            List<BasePullDownResponseDto> basePullDownResponseDtos = new List<BasePullDownResponseDto>();
            var filedList = Utils.GetClassFiledAttribute<ProjectExcelImportResponseDto>();
            if (filedList.Any())
            {
                foreach (var item in filedList)
                {
                    basePullDownResponseDtos.Add(new BasePullDownResponseDto()
                    {
                        Code = item.Key,
                        Name = item.Value
                    });
                }
                responseAjaxResult.Data = basePullDownResponseDtos;
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 获取实体字段备注说明
        /// <summary>
        /// 获取实体字段备注说明
        /// </summary>
        /// <param name="entityDtoName">Dto的类名称</param>
        /// <returns></returns>
        public async Task<List<EntityFieldRemark>> SearchEntityFieldRemarkAsync(string entityDtoName)
        {
            List<EntityFieldRemark> list = new List<EntityFieldRemark>();
            try
            {
                string jsonResult = string.Empty;
                var redisKey = "SwaggerUIJsonUrl";
                WebHelper webHelper = new WebHelper();
                var redis = RedisUtil.Instance;
                var isExist = await redis.ExistsAsync(redisKey);
                if (!isExist)
                {
                    var url = AppsettingsHelper.GetValue(redisKey);
                    var res = await webHelper.DoGetAsync(url);
                    if (res.Code == 200 && !string.IsNullOrWhiteSpace(res.Result))
                    {
                        jsonResult = res.Result;
                        await redis.SetAsync(redisKey, res.Result);
                    }

                }
                else
                {
                    jsonResult = await redis.GetAsync(redisKey);
                }

                if (!string.IsNullOrWhiteSpace(jsonResult))
                {
                    JObject jobject = JObject.Parse(jsonResult);
                    var entityInfoList = jobject["components"]["schemas"]["" + entityDtoName + ""]["properties"].ToList();
                    foreach (var item in entityInfoList)
                    {
                        var description = ((Newtonsoft.Json.Linq.JValue)JObject.Parse(((Newtonsoft.Json.Linq.JProperty)item).Value.ToString())["description"])?.Value.ToString();
                        var name = ((Newtonsoft.Json.Linq.JProperty)item).Name;
                        list.Add(new EntityFieldRemark()
                        {
                            Field = name,
                            Name = description
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("获取实体字段和备注出现错误", ex);
            }
            return list;
        }

        #endregion

        #region 获取首页菜单列表
        ///<summary>
        ///获取首页菜单列表
        ///</summary>
        ///<returns></returns>
        ///<exception cref = "NotImplementedException" ></exception>
        public async Task<ResponseAjaxResult<SearchHomePageMenuResponseDto>> SearchHomeMenuAsync(Guid? projectId)
        {
            //非施工类业务
            Guid projectType = "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid();
            //项目状态Id
            List<Guid> projectStatusList = new List<Guid>()
            {
                "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(),
                "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(),
                "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(),
                "75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid()
            };
            ResponseAjaxResult<SearchHomePageMenuResponseDto> responseAjaxResult = new();
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == projectId).SingleAsync();
            var result = new SearchHomePageMenuResponseDto();
            //projectid为Guid的default值时，直接返回船舶动态日报
            if (string.IsNullOrWhiteSpace(projectId.ToString()))
            {
                responseAjaxResult.Data = null;
                responseAjaxResult.Success(ResponseMessage.OPERATION_SUCCESS);
                return responseAjaxResult;
            }
            //判断项目是否存在
            var projectInfo = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && x.Id == projectId).SingleAsync();
            if (projectInfo == null)
            {
                return responseAjaxResult.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            else
            {
                var menuDisplayTime = new List<string?>();
                // 获取该用户授权的业务菜单
                var authMenus = await GetUserAuthMenuAsync(_currentUser.Id);
                if (authMenus.Any())
                {
                    //全部显示
                    menuDisplayTime = await dbContext.Queryable<HomeMenu>()
                    .OrderBy(h => h.Sort)
                    .Select(h => h.MenuName)
                    .ToListAsync();
                }
                else
                {
                    var today = DateTime.Now.Day;
                    var todayInt = DateTime.Now.Month * 100 + today;
                    menuDisplayTime = await dbContext.Queryable<HomeMenu>()
                         .LeftJoin<DisplayTime>((h, d) => h.Id == d.MenuId)
                         .Where((h, d) => (h.IsDelete == 1) && ((d.StartTime <= todayInt && d.EndTime >= todayInt) || (d.StartTime <= today && d.EndTime >= today)))
                         .OrderBy(h => h.Sort)
                         .Select(h => h.MenuName)
                         .ToListAsync();

                }
                //筛选需要展示的菜单名称

                menuDisplayTime = menuDisplayTime.Distinct().ToList();
                foreach (var item in menuDisplayTime)
                {
                    switch (item)
                    {
                        //case "施工项目档案电子化系统":
                        //    result.OutputColumn.Add(new ColumnInfo()
                        //    {
                        //        ProjectId = projectId,
                        //        MenuName = "施工项目档案电子化系统",
                        //        Url = "http://www.hnkcfives.cn/fileweb/"
                        //    }) ;
                        //    break;
                        //case "任务督办系统":
                        //    result.OutputColumn.Add(new ColumnInfo()
                        //    {
                        //        ProjectId = projectId,
                        //        MenuName = "任务督办系统",
                        //        Url= "http://www.hnkcfives.cn/taskweb/"
                        //    });
                        //    break;
                        case "船舶日报":
                            if (projectList.TypeId == projectType)
                            {
                                continue;
                            }
                            //获取当前项目当日填报船舶日报
                            bool filling = false;
                            var nowDay = DateTime.Now.AddDays(-1).ToDateDay();
                            //var project = await dbContext.Queryable<Project>()
                            //                    .LeftJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
                            //                    .Where((x, y) => x.IsDelete == 1 && projectStatusList.Contains(x.StatusId.Value))
                            //                    .Select(x=>x.Id)
                            //                    .ToListAsync();
                            EnterShipsRequestDto model = new EnterShipsRequestDto()
                            {
                                ProjectId = projectId,
                                DateDayTime = DateTime.Now.AddDays(-1),
                                AssociationProject = 0,
                                PageSize = 1000
                            };
                            var projectShipMovementsService = (IProjectShipMovementsService)HttpContentAccessFactory.Current.RequestServices.GetService(typeof(IProjectShipMovementsService));
                            var projectShipMovements = await projectShipMovementsService.SearchEnterShipsAsync(model);
                            var columnInfo = new ColumnInfo()
                            {
                                ProjectId = projectId,
                                MenuName = "船舶日报",
                                FillingStatus = projectShipMovements.Count == 0 ? 3 : projectShipMovements.Data.Ships.Where(x => x.FillReportStatus == 2).ToList().Count == projectShipMovements.Count ? 1 : 2,
                            };
                            if (!filling)
                            {
                                columnInfo.Prompt = "填完所有船舶日报时会显示已填报";
                            }
                            result.ShipColumn.Add(columnInfo);
                            break;
                        case "产值日报":
                            if (projectList.TypeId == projectType)
                            {
                                continue;
                            }
                            //获取当前项目当日填报产值日报
                            var dayReports = await dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1
                            && x.ProjectId == projectId && x.DateDay == DateTime.Now.AddDays(-1).ToDateDay() && x.ProcessStatus == DayReportProcessStatus.Submited)
                                .ToListAsync();
                            result.OutputColumn.Add(new ColumnInfo()
                            {
                                ProjectId = projectId,
                                MenuName = "产值日报",
                                FillingStatus = dayReports.Any() ? 1 : 2
                            });
                            break;
                        case "安监日报":
                            if (projectList.TypeId == projectType)
                            {
                                continue;
                            }
                            //获取当前项目当日已填安监日报信息
                            var safeSupervisionDayReports = await dbContext.Queryable<SafeSupervisionDayReport>()
                                .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateDay == DateTime.Now.AddDays(-1).ToDateDay())
                                .ToListAsync();
                            result.SafetySupervision.Add(new ColumnInfo()
                            {
                                ProjectId = projectId,
                                MenuName = "安监日报",
                                FillingStatus = safeSupervisionDayReports.Any() ? 1 : 2
                            });
                            break;
                        case "项目月报":
                            int day = DateTime.Now.Day;
                            int time = 0;
                            if (1 <= day && day < 26)
                            {
                                time = DateTime.Now.AddMonths(-1).ToDateMonth();
                            }
                            else
                            {
                                time = DateTime.Now.ToDateMonth();
                            }
                            var outputMonthReports = await dbContext.Queryable<MonthReport>()
                                .Where(x => x.IsDelete == 1 && x.ProjectId == projectId & x.DateMonth == time)
                                .ToListAsync();
                            result.OutputColumn.Add(new ColumnInfo()
                            {
                                ProjectId = projectId,
                                MenuName = "项目月报",
                                FillingStatus = outputMonthReports.Any() ? 1 : 2,
                                TypePrompt = projectList.TypeId == projectType ? true : false
                            });
                            break;
                        case "船舶月报":
                            if (projectList.TypeId == projectType)
                            {
                                continue;
                            }
                            int datetime = 0;
                            if (DateTime.Now.Day < 26)
                            {
                                datetime = DateTime.Now.AddMonths(-1).ToDateMonth();
                            }
                            else
                            {
                                datetime = DateTime.Now.ToDateMonth();
                            }
                            ShipsForReportRequestDto models = new ShipsForReportRequestDto()
                            {
                                ProjectId = projectId.Value,
                                DateMonth = datetime,
                                PageSize = 1000
                            };
                            var iprojectShipMovementsService = (IProjectReportService)HttpContentAccessFactory.Current.RequestServices.GetService(typeof(IProjectReportService));
                            var shipMonthReport = await iprojectShipMovementsService.SearchShipsForMonthReportAsync(models);
                            var shipMonthReportCount = shipMonthReport.Data.Ships.Where(x => x.FillReportStatus == Contracts.Dto.Enums.FillReportStatus.Reported).ToList().Count();
                            result.ShipColumn.Add(new ColumnInfo()
                            {
                                ProjectId = projectId,
                                MenuName = "船舶月报",
                                FillingStatus = shipMonthReport.Count == 0 ? 2 : shipMonthReport.Count == shipMonthReportCount ? 1 : 2
                            });
                            break;
                        case "非自有设备":
                            result.SafetySupervision.Add(new ColumnInfo()
                            {
                                ProjectId = projectId,
                                MenuName = "非自有设备",
                                FillingStatus = 3
                            });
                            break;
                    }
                }
            }

            responseAjaxResult.Data = result;
            responseAjaxResult.Count = result.OutputColumn.Count + result.ShipColumn.Count + result.SafetySupervision.Count;
            responseAjaxResult.Success(ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }

        /// <summary>
        /// 获取用户授权的菜单名称
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetUserAuthMenuAsync(Guid userId)
        {
            var menus = new List<string>();
            var bizAuths = await dbContext.Queryable<AuthorizedUser>().Where(t => t.IsDelete == 1 && t.UserId == userId).ToListAsync();
            bizAuths.ForEach(item =>
            {
                if (item.AuthorizeMode == AuthorizeMode.AuthorizeForever)
                {
                    menus.Add(item.BizModule.ToDescription());
                }
                else if (item.AuthorizeMode == AuthorizeMode.AuthorizeOneDay && DateTime.Now <= item.ExpireTime)
                {
                    menus.Add(item.BizModule.ToDescription());
                }
            });
            return menus;
        }

        #endregion

        #region 获取审批用户列表
        /// <summary>
        /// 获取审批用户列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ApprovalUser>>> SearchApprovalUserAsync(Guid? Id, Guid ProjectId)
        {
            ResponseAjaxResult<List<ApprovalUser>> responseAjaxResult = new ResponseAjaxResult<List<ApprovalUser>>();
            var project = await dbContext.Queryable<Project>().Where(x => x.Id == ProjectId).SingleAsync();
            var institutions = await dbContext.Queryable<Institution>().Where(x => x.PomId == project.ProjectDept).SingleAsync();
            if (institutions != null)
            {
                var institutionKeyValue = await GetCurrentInstitutionParent(institutions.Oid);
                #region
                var roleList = await dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 && (x.InstitutionId == institutionKeyValue.CPomId || x.InstitutionId == institutionKeyValue.DPomId)).ToListAsync();
                var roleLists = roleList.Select(x => x.RoleId).ToList();
                if (roleLists.Any())
                {
                    var roleId = await dbContext.Queryable<Model.Role>().Where(x => x.IsDelete == 1 && roleLists.Contains(x.Id) && x.IsApprove == true).Select(x => x.Id).Distinct().ToListAsync();
                    if (roleId.Any())
                    {
                        var institutionRole = await dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 && roleId.Contains(x.RoleId.Value)).ToListAsync();
                        if (institutionRole.Any())
                        {
                            var institution = await dbContext.Queryable<Institution>().ToListAsync();
                            var role = await dbContext.Queryable<Model.Role>().ToListAsync();
                            var user = await baseUserRepository.AsQueryable().ToListAsync();
                            List<ApprovalUser> informationresponseDto = new List<ApprovalUser>();
                            foreach (var item in institutionRole)
                            {
                                if (item.UserId != null)
                                {
                                    ApprovalUser ApprovalUser = new ApprovalUser()
                                    {

                                        UserId = item.UserId,
                                        UserName = user.SingleOrDefault(x => x.Id == item.UserId)?.Name,
                                        Phone = user.SingleOrDefault(x => x.Id == item.UserId)?.Phone,
                                        DepartmentId = item.InstitutionId,
                                        DepartmentName = institution.SingleOrDefault(x => x.PomId == item.InstitutionId)?.Name,
                                        RoleId = item.RoleId.Value,
                                        RoleName = role.SingleOrDefault(x => x.Id == item.RoleId)?.Name,
                                    };
                                    var informationres = informationresponseDto.Where(x => x.UserId == ApprovalUser.UserId).FirstOrDefault();
                                    if (informationres == null)
                                    {
                                        informationresponseDto.Add(ApprovalUser);
                                    }
                                }
                            }
                            responseAjaxResult.Data = informationresponseDto;
                            responseAjaxResult.Count = informationresponseDto.Count;
                        }
                    }
                }

                #endregion
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取未填报日报 、 安监 、船舶 下拉集合
        /// <summary>
        /// 获取未填报日报 、 安监 、船舶 下拉集合
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetNotFillDayRepsSearch(CurrentUser currentUser, BaseKeyWordsRequestDto requestDto)
        {
            //项目状态Id
            List<Guid> projectStatusList = new List<Guid>()
            {
                "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(),
                "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(),
                "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(),
                "75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid()
            };
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            List<BasePullDownResponseDto> dayRepNotFillSearch = new List<BasePullDownResponseDto>();
            List<BasePullDownResponseDto> projects = new List<BasePullDownResponseDto>();//项目Id
            List<Guid?> proIds = new List<Guid?>();//当前登陆用户未填报项目Id
            //过滤类型符合条件的项目
            var project = await dbContext.Queryable<Project>()
              .LeftJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
               .Where((x, y) => x.IsDelete == 1 && projectStatusList.Contains(x.StatusId.Value))
               .ToListAsync();
            //根据当前登陆人 获取所需要填写的项目日报
            if (currentUser.CurrentLoginInstitutionPoid == "101114066")
            {
                projects = project.Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name }).ToList();
                //获取项目ids
                proIds = projects.Select(x => x.Id).ToList();
            }
            else
            {
                projects = project.Where(x => x.ProjectDept == currentUser.CurrentLoginDepartmentId).Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name }).ToList();
                //获取项目ids
                proIds = projects.Select(x => x.Id).ToList();
            }
            var noFillNums = 0;//未报数量
            if (projects.Count() > 0)
            {
                if (requestDto.KeyWords == "dayrep")
                {
                    //获取已填报日报
                    var dayContains = await dbContext.Queryable<DayReport>().Where(x => proIds.Contains(x.ProjectId) && Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date && x.ProcessStatus == DayReportProcessStatus.Submited).ToListAsync();
                    if (requestDto.Type == 1)
                    {
                        dayContains.ForEach(y => dayRepNotFillSearch.Add(new BasePullDownResponseDto
                        {
                            Id = projects.Where(x => x.Id == y.ProjectId).SingleOrDefault()?.Id,
                            Name = projects.Where(x => x.Id == y.ProjectId).SingleOrDefault()?.Name
                        }));
                    }
                    else if (requestDto.Type == 0)
                    {
                        foreach (var item in projects)
                        {
                            var isExistDayRepResult = dayContains.Where(x => x.ProjectId == item.Id).FirstOrDefault();
                            if (isExistDayRepResult == null)
                            {
                                dayRepNotFillSearch.Add(new BasePullDownResponseDto
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                });
                                noFillNums++;
                            }
                        }
                    }
                }
                else if (requestDto.KeyWords == "shiprep")
                {
                    //获取已填报船舶日报
                    var shipDayContains = await dbContext.Queryable<ShipDayReport>().Where(x => proIds.Contains(x.ProjectId) && Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date).ToListAsync();
                    shipDayContains = shipDayContains.GroupBy(x => x.ProjectId).Select(y => y.First()).ToList();
                    if (requestDto.Type == 1)
                    {
                        shipDayContains.ForEach(y => dayRepNotFillSearch.Add(new BasePullDownResponseDto
                        {
                            Id = projects.Where(x => x.Id == y.ProjectId).SingleOrDefault()?.Id,
                            Name = projects.Where(x => x.Id == y.ProjectId).SingleOrDefault()?.Name
                        }));
                    }
                    else if (requestDto.Type == 0)
                    {
                        noFillNums = 0;
                        foreach (var item in projects)
                        {
                            var isExistDayRepResult = shipDayContains.Where(x => x.ProjectId == item.Id).FirstOrDefault();
                            if (isExistDayRepResult == null)
                            {
                                dayRepNotFillSearch.Add(new BasePullDownResponseDto
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                });
                                noFillNums++;
                            }
                        }
                    }
                }
                else if (requestDto.KeyWords == "saferep")
                {
                    //获取已填报安监日报
                    var safeDayContains = await dbContext.Queryable<SafeSupervisionDayReport>().Where(x => proIds.Contains(x.ProjectId) && Convert.ToDateTime(x.CreateTime).Date == DateTime.Now.Date).ToListAsync();
                    if (requestDto.Type == 1)
                    {
                        safeDayContains.ForEach(y => dayRepNotFillSearch.Add(new BasePullDownResponseDto
                        {
                            Id = projects.Where(x => x.Id == y.ProjectId).SingleOrDefault()?.Id,
                            Name = projects.Where(x => x.Id == y.ProjectId).SingleOrDefault()?.Name
                        }));
                    }
                    else if (requestDto.Type == 0)
                    {
                        noFillNums = 0;
                        foreach (var item in projects)
                        {
                            var isExistDayRepResult = safeDayContains.Where(x => x.ProjectId == item.Id).FirstOrDefault();
                            if (isExistDayRepResult == null)
                            {
                                dayRepNotFillSearch.Add(new BasePullDownResponseDto
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                });
                                noFillNums++;
                            }
                        }
                    }
                }
            }
            //foreach (var item in projects)
            //{
            //    var type = 0;//1已报  0 未报
            //    var isExistDayRepResult = dayContains.Where(x => x.ProjectId == item.Id).FirstOrDefault();
            //    if (isExistDayRepResult != null) type = 1; else type = 0;
            //    pDayRepNotFillSearch.Add(new BasePullDownResponseDto
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Type = type,
            //        Code = "dayrep"
            //    });
            //    var isShipDayRepResult = shipDayContains.Where(x => x.ProjectId == item.Id).FirstOrDefault();
            //    if (isShipDayRepResult != null) type = 1; else type = 0;
            //    pShipDayRepNotFillSearch.Add(new BasePullDownResponseDto
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Type = type,
            //        Code = "shiprep"
            //    });
            //    var isSafeDayRepResult = safeDayContains.Where(x => x.ProjectId == item.Id).FirstOrDefault();
            //    if (isSafeDayRepResult != null) type = 1; else type = 0;
            //    pSafeDayRepNotFillSearch.Add(new BasePullDownResponseDto
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Type = type,
            //        Code = "saferep"
            //    });
            //}
            responseAjaxResult.Data = dayRepNotFillSearch;
            responseAjaxResult.Count = noFillNums;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        #endregion

        #region 获取船舶名称
        /// <summary>
        /// 获取船舶名称模糊查询
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipPingNameAsync(BaseRequestDto baseRequestDto)
        {
            //组合
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var subShipList = await baseSubShipRepository.AsQueryable().Where(x => x.IsDelete == 1).Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = 2 }).ToListAsync();
            var ownerShipList = await baseOwnerShipRepository.AsQueryable().Where(x => x.IsDelete == 1).Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = 1 }).ToListAsync();
            foreach (var item in ownerShipList)
            {
                subShipList.Add(item);
            }
            var ShipList = subShipList.WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => x.Name.Contains(baseRequestDto.KeyWords)).ToList();
            int skipCount = (baseRequestDto.PageIndex - 1) * baseRequestDto.PageSize;
            var pageData = subShipList.Skip(skipCount).Take(baseRequestDto.PageSize).ToList();
            responseAjaxResult.Data = pageData;
            responseAjaxResult.Count = ShipList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }




        #endregion

        #region 获取当前机构的所属项目部以及所属公司

        /// <summary>
        /// 获取当前机构的父亲节点
        /// </summary>
        /// <param name="oid">当前机构OID</param>
        /// <returns></returns>
        public async Task<InstitutionKeyValueResponseDto> GetCurrentInstitutionParent(string oid)
        {
            InstitutionKeyValueResponseDto institutionKeyValueResponseDto = new InstitutionKeyValueResponseDto();
            var allInstitutionList = await baseInstitutionRepository.AsQueryable().ToListAsync();
            if (allInstitutionList.Any())
            {
                var currentInstitution = allInstitutionList.SingleOrDefault(x => x.Oid == oid);
                if (currentInstitution != null)
                {
                    var currentOrult = currentInstitution.Orule?.Split("-", StringSplitOptions.RemoveEmptyEntries).Reverse();
                    if (currentOrult != null && currentOrult.Any())
                    {
                        foreach (var item in currentOrult)
                        {
                            //所属项目部信息
                            var institutionInfo = allInstitutionList.SingleOrDefault(x => x.Oid == item && x.Ocode?.ToUpper().LastIndexOf("000P") > 0);
                            if (institutionInfo != null)
                            {
                                institutionKeyValueResponseDto.DPomId = institutionInfo.PomId.Value;
                                institutionKeyValueResponseDto.Doid = institutionInfo.Oid;
                                institutionKeyValueResponseDto.AffDepartmentName = institutionInfo.Shortname;
                                institutionKeyValueResponseDto.Dpoid = institutionInfo.Poid;
                            }
                            //所属公司信息
                            var companyInstitutionInfo = allInstitutionList.SingleOrDefault(x => x.Oid == item);
                            if (companyInstitutionInfo != null &&
                                (companyInstitutionInfo.Ocode.IndexOf("00000A") > 0
                                || companyInstitutionInfo.Ocode.IndexOf("00000B") > 0
                                || companyInstitutionInfo.Ocode.IndexOf("00000C") > 0
                                || companyInstitutionInfo.Ocode.IndexOf("00000E") > 0))
                            {
                                institutionKeyValueResponseDto.CPomId = companyInstitutionInfo.PomId.Value;
                                institutionKeyValueResponseDto.Coid = companyInstitutionInfo.Oid;
                                institutionKeyValueResponseDto.Cpoid = companyInstitutionInfo.Poid;
                                institutionKeyValueResponseDto.AffCompanyFullName = companyInstitutionInfo.Name;
                                institutionKeyValueResponseDto.AffCompanName = companyInstitutionInfo.Shortname;
                                break;
                            }
                        }
                    }
                }
            }
            return institutionKeyValueResponseDto;
        }
        #endregion

        #region  如果是广航局总部的机构提级为广航局的 返回广航局的机构
        /// <summary>
        /// 如果是广航局总部的机构提级为广航局的 返回广航局的机构
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public async Task<Guid> IsGHJInstitution(Guid institutionId)
        {
            var currentInstitutionInfo = await baseInstitutionRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.PomId == institutionId);
            if (currentInstitutionInfo != null)
            {
                if (currentInstitutionInfo.Orule?.IndexOf("101162351") >= 0)
                {
                    //说明是广航局总部的   这里为了不必再查数据库可以把机构的id直接写死
                    return "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid();
                }

            }
            return Guid.Empty;
        }



        #endregion

        #region 获取所属公司下拉接口
        /// <summary>
        /// 获取所属公司下拉接口
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyAsync(BaseKeyWordsRequestDto baseKeyWordsRequestDto, string oid)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var institutionList = await baseInstitutionRepository.AsQueryable()
                 .Where(x => x.IsDelete == 1 && x.Status != "4" && !string.IsNullOrWhiteSpace(x.Ocode) && (x.Ocode.Contains("0000A") ||
                 x.Ocode.Contains("0000B") || x.Ocode.Contains("0000C") || x.Ocode.Contains("0000E")))
                 .WhereIF(oid == "101162350", x => (x.Oid == oid || x.Poid == oid))
                 .WhereIF(oid != "101162350", x => x.Poid == oid || x.Oid == oid)
                 .WhereIF(!string.IsNullOrWhiteSpace(baseKeyWordsRequestDto.KeyWords), x => SqlFunc.Contains(x.Name, baseKeyWordsRequestDto.KeyWords))
                 .OrderBy(x => x.Sno)
                 .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name })
                 .ToListAsync();
            responseAjaxResult.Data = institutionList;
            responseAjaxResult.Count = institutionList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
            throw new NotImplementedException();
        }
        #endregion

        #region 获取部门下项目信息下拉列表
        /// <summary>
        /// 获取部门下项目信息下拉列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ProjectInformationResponseDto>>> SearchProjectInformationAsync(BaseRequestDto baseRequestDto)
        {
            //非施工类业务
            Guid projectType = "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid();
            //项目状态Id
            List<Guid> projectStatusList = new List<Guid>()
            {
                "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(),//在建（间歇性停工）
                "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(),//在建（短暂性停工）
                "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(),//在建
                //"75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid(),//中标已签的   
            };
            ResponseAjaxResult<List<ProjectInformationResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProjectInformationResponseDto>>();
            var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            //var InstitutionId = await dbContext.Queryable<Institution>()
            //    .Where(x => x.Grule.Contains(oids.Grule)).ToListAsync();
            var InstitutionId = await SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            var InstitutionIdList = InstitutionId.Data.Select(x => x.Id).ToList();
            var project = await dbContext.Queryable<Project>()
               .LeftJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
               .Where((x, y) => x.IsDelete == 1
               && InstitutionIdList.Contains(x.ProjectDept)
               && x.IsDelete == 1 && x.StatusId != "0c686c96-889e-4c4d-b24d-fa2886d9dceb".ToGuid())
               //&& projectStatusList.Contains(x.StatusId.Value))
               .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => x.Name.Contains(baseRequestDto.KeyWords))
               .OrderBy((x, y) => new { y.Sequence, x.CreateTime, OrderByType.Desc })
               .Select((x, y) => new ProjectInformationResponseDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   Status = y.Name,
                   Rate = x.Rate * 100,
                   Required = projectStatusList.Contains(x.StatusId.Value) ? true : false
               }).ToListAsync();
            responseAjaxResult.Data = project;
            responseAjaxResult.Count = project.Count;
            #region 暂时不用
            //if (_currentUser.CurrentLoginInstitutionPoid == "101114066")
            //{
            //    var project = await dbContext.Queryable<Project>()
            //   .LeftJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
            //    .Where((x, y) => x.IsDelete == 1 && y.Sequence == "16" || y.Sequence == "17" || y.Sequence == "6" || y.Sequence == "9")
            //   .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => x.Name.Contains(baseRequestDto.KeyWords))
            //   .Select((x, y) => new ProjectInformationResponseDto { Id = x.Id, Name = x.Name, Status = y.Name, Rate = x.Rate*100 }).ToListAsync();
            //    responseAjaxResult.Data = project;
            //    responseAjaxResult.Count = project.Count;
            //}
            //else
            //{
            //    var project = await dbContext.Queryable<Project>()
            //   .LeftJoin<ProjectStatus>((x, y) => x.StatusId == y.StatusId)
            //   .Where((x,y) => x.IsDelete == 1 && x.ProjectDept == _currentUser.CurrentLoginDepartmentId && x.IsDelete == 1&&(y.Sequence == "16" || y.Sequence == "17" || y.Sequence == "6" || y.Sequence == "9"))
            //   .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => x.Name.Contains(baseRequestDto.KeyWords))
            //   .Select((x, y) => new ProjectInformationResponseDto { Id = x.Id, Name = x.Name, Status = y.Name, Rate = x.Rate*100 }).ToListAsync();
            //    responseAjaxResult.Data = project;
            //    responseAjaxResult.Count = project.Count;
            //}
            #endregion
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取船舶港口数据
        /// <summary>
        /// 获取船舶港口数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetSearchShipPortAsync(BaseRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var resultResponse = new List<BasePullDownResponseDto>();
            RefAsync<int> total = 0;
            var data = await dbContext.Queryable<Domain.Models.PortData>().Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), (x) => x.Name.Contains(requestDto.KeyWords)).Select(x => new
                {
                    Id = x.PomId,
                    Name = x.Name
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
            data.ForEach(item =>
            {
                resultResponse.Add(new BasePullDownResponseDto() { Id = item.Id.ToGuid(), Name = item.Name });
            });
            responseAjaxResult.Data = resultResponse;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取施工性质模糊搜索
        /// <summary>
        /// 获取施工性质模糊搜索
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetConstructionNatureAsync(BaseRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var constructionNature = await dbContext.Queryable<DictionaryTable>()
                .Where(x => x.IsDelete == 1 && x.TypeNo == 7)
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => x.Name.Contains(requestDto.KeyWords))
                .OrderBy(x => x.Type)
                .Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name })
                .ToListAsync();
            responseAjaxResult.Success();
            responseAjaxResult.Count = constructionNature.Count;
            responseAjaxResult.Data = constructionNature;
            return responseAjaxResult;
        }


        #endregion

        #region 产值日报自有分包船舶模糊查询
        /// <summary>
        /// 产值日报自有分包船舶模糊查询
        /// </summary>       
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchLogOwnerSubShipAsync(ShipRequestDto requestDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            RefAsync<int> total = 0;
            if (requestDto.Type == 1)
            {
                //获取自有船舶数据
                var ownerList = await baseOwnerShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.OwnerShip }).ToListAsync();
                var ownerLists = await baseDictionaryTableRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.TypeNo == 8)
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name }).ToListAsync();
                foreach (var item in ownerLists)
                {
                    ownerList.Add(item);
                }
                responseAjaxResult.Data = ownerList;
                responseAjaxResult.Count = ownerList.Count;
                responseAjaxResult.Success();
            }
            if (requestDto.Type == 2)
            {
                //获取往来单位数据
                var ProjectDealingUnitList = await baseDealingUnitRepository.AsQueryable()
                     .Where(x => !string.IsNullOrWhiteSpace(x.ZUSCC))
                     .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.ZBPNAME_ZH, requestDto.KeyWords))
                     .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.ZBPNAME_ZH, Type = (int)ShipType.SubBusinessUnit })
                     .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                //获取分包船舶数据
                var subList = await baseSubShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.SubShip })
                    .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                //获取用户添加的分包船舶数据

                var data = ProjectDealingUnitList.Union(subList).ToList();
                var ownerLists = await baseDictionaryTableRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.TypeNo == 8 && x.Type == 2)
                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                .Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name }).SingleAsync();


                if (ownerLists != null)
                {
                    data.Add(ownerLists);
                }
                responseAjaxResult.Data = data;
                responseAjaxResult.Count = data.Count;
                responseAjaxResult.Success();
            }
            if (requestDto.Type == 3)
            {
                //获取分包船舶数据
                var subList = await baseSubShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.SubShip })
                    .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                responseAjaxResult.Data = subList;
                responseAjaxResult.Count = total;

            }
            if (requestDto.Type == 4)
            {
                //获取自有船舶数据
                var ownerList = await baseOwnerShipRepository.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.PomId, Name = x.Name, Type = (int)ShipType.SubOwe }).ToListAsync();
                var ownerLists = await baseDictionaryTableRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.TypeNo == 8)
                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.KeyWords), x => SqlFunc.Contains(x.Name, requestDto.KeyWords))
                    .Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name }).ToListAsync();
                foreach (var item in ownerLists)
                {
                    ownerList.Add(item);
                }
                responseAjaxResult.Data = ownerList;
                responseAjaxResult.Count = ownerList.Count;
                responseAjaxResult.Success();
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 疏浚吹填分类

        /// <summary>
        /// 疏浚吹填分类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipWorkTypePullDownAsync()
        {
            var result = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var shipWorkTypes = await dbContext.Queryable<ShipWorkType>().Where(t => t.IsDelete == 1)
                .Select(t => new BasePullDownResponseDto { Id = t.PomId, Name = t.Name }).ToListAsync();
            return result.SuccessResult(shipWorkTypes, shipWorkTypes.Count);
        }

        #endregion

        #region 工艺方式

        /// <summary>
        /// 工艺方式
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipWorkModePullDownAsync()
        {
            var result = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var shipWorkModes = await dbContext.Queryable<ShipWorkMode>().Where(t => t.IsDelete == 1)
                .Select(t => new BasePullDownResponseDto { Id = t.PomId, Name = t.Name }).ToListAsync();
            return result.SuccessResult(shipWorkModes, shipWorkModes.Count);
        }

        #endregion 

        #region 疏浚土分类

        /// <summary>
        /// 疏浚土分类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchSoilPullDownAsync()
        {
            var result = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var soils = await dbContext.Queryable<Soil>().Where(t => t.IsDelete == 1)
                .Select(t => new BasePullDownResponseDto { Id = t.PomId, Name = t.Name }).ToListAsync();
            return result.SuccessResult(soils, soils.Count);
        }

        #endregion

        #region 疏浚岩土分级

        /// <summary>
        /// 疏浚岩土分级
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchSoilGradePullDownAsync()
        {
            var result = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var soils = await dbContext.Queryable<SoilGrade>().Where(t => t.IsDelete == 1)
                .OrderBy(t => t.Sequence)
                .Select(t => new BasePullDownResponseDto { Id = t.PomId, Name = t.Grade }).ToListAsync();
            return result.SuccessResult(soils, soils.Count);
        }


        #endregion

        #region 新增项目组织结构
        /// <summary>
        /// 获取项目组织结构
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ProjectWBSUpload>>> InsertProjectWBSAsync(ProjectWBSUploadRequestDto projectWBSUploadRequestDto)
        {
            ResponseAjaxResult<List<ProjectWBSUpload>> responseAjaxResult = new ResponseAjaxResult<List<ProjectWBSUpload>>();

            var projectWBSUploadsList = projectWBSUploadRequestDto.projectWBSUploads.ToList();
            foreach (var item in projectWBSUploadsList)
            {
                item.SId = item.Number;

                var aa = item.Number.Split(".").ToList();
                if (aa.Count != 1)
                {
                    for (int i = 0; i < aa.Count - 1; i++)
                    {
                        item.Poid += aa.Count - 2 == i ? aa[i] : aa[i] + ".";
                    }
                }
                else
                {
                    item.Poid = "0";
                }
            }
            var projectWBSUploads = ListToTreeUtil.GetTree(0, "0", projectWBSUploadsList).ToList();
            responseAjaxResult.Data = projectWBSUploads;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion

        #region 获取预览和下载网址
        /// <summary>
        /// 获取预览和下载网址
        /// </summary>
        /// <param name="baseKeyWordsRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<SearchWebsiteResponseDto>> SearchWebsiteAsync(string websites)
        {
            ResponseAjaxResult<SearchWebsiteResponseDto> responseAjaxResult = new ResponseAjaxResult<SearchWebsiteResponseDto>();
            DictionaryTableRequestDto dictionaryTableRequestDto = new DictionaryTableRequestDto()
            {
                Type = 11
            };
            var dictionaryTable = await SearchDictionaryTableTreeAsyncc(dictionaryTableRequestDto);
            if (dictionaryTable == null)
            {
                responseAjaxResult.Fail(ResponseMessage.NOT_FIND_FILE);
                return null;
            }
            var website = dictionaryTable.Data.Where(x => x.Name == websites).Select(x => x.Code).SingleOrDefault();
            var basePullDownResponseDto = new SearchWebsiteResponseDto()
            {
                UserManual = website
            };
            responseAjaxResult.Data = basePullDownResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取省份
        public async Task<string> GetProvince(List<Province> provinces, Guid? pomId)
        {
            var fIsFirst = provinces.FirstOrDefault(x => x.PomId == pomId);
            if (fIsFirst != null)
            {
                if (fIsFirst.Zaddvslevel == 1 || fIsFirst.Zaddvslevel == 0)
                {
                    return fIsFirst.Zaddvsname;
                }
                else
                {
                    var sIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == fIsFirst.Zaddvsup);
                    if (sIsExist != null)
                    {
                        if (sIsExist.Zaddvslevel == 1 || sIsExist.Zaddvslevel == 0)
                        {
                            return sIsExist.Zaddvsname;
                        }
                        else
                        {
                            var tIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == sIsExist.Zaddvsup);
                            if (tIsExist != null)
                            {
                                if (tIsExist.Zaddvslevel == 1 || tIsExist.Zaddvslevel == 0)
                                {
                                    return tIsExist.Zaddvsname;
                                }
                                else
                                {
                                    var foIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == tIsExist.Zaddvsup);
                                    if (foIsExist != null)
                                    {
                                        if (foIsExist.Zaddvslevel == 1 || foIsExist.Zaddvslevel == 0)
                                        {
                                            return foIsExist.Zaddvsname;
                                        }
                                        else
                                        {
                                            var fivIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == foIsExist.Zaddvsup);
                                            if (fivIsExist != null)
                                            {
                                                if (fivIsExist.Zaddvslevel == 1 || fivIsExist.Zaddvslevel == 0)
                                                {
                                                    return fivIsExist.Zaddvsname;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;

        }


        #endregion

        #region 获取市
        public async Task<string> GetProvincemarket(List<Province> provinces, Guid? pomId)
        {
            var fIsFirst = provinces.FirstOrDefault(x => x.PomId == pomId);
            if (fIsFirst != null)
            {
                if (fIsFirst.Zaddvslevel == 2 || fIsFirst.Zaddvslevel == 1)
                {
                    return fIsFirst.Zaddvsname;
                }
                else
                {
                    var sIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == fIsFirst.Zaddvsup);
                    if (sIsExist != null)
                    {
                        if (sIsExist.Zaddvslevel == 2)
                        {
                            return sIsExist.Zaddvsname;
                        }
                        else
                        {
                            var tIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == sIsExist.Zaddvsup);
                            if (tIsExist != null)
                            {
                                if (tIsExist.Zaddvslevel == 2)
                                {
                                    return tIsExist.Zaddvsname;
                                }
                                else
                                {
                                    var foIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == tIsExist.Zaddvsup);
                                    if (foIsExist != null)
                                    {
                                        if (foIsExist.Zaddvslevel == 2)
                                        {
                                            return foIsExist.Zaddvsname;
                                        }
                                        else
                                        {
                                            var fivIsExist = provinces.FirstOrDefault(x => x.Zaddvscode == foIsExist.Zaddvsup);
                                            if (fivIsExist != null)
                                            {
                                                if (fivIsExist.Zaddvslevel == 2)
                                                {
                                                    return fivIsExist.Zaddvsname;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;

        }
        #endregion

        #region 船级社  船舶状态  船舶类型
        /// <summary>
        /// 船级社
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipClassicPullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();

            responseAjaxResult.Data = await dbContext.Queryable<ShipClassic>().Where(x => x.IsDelete == 1)
                .Select(x => new BasePullDownResponseDto() { Id = x.Id, Name = x.Name }).ToListAsync();
            responseAjaxResult.Success();
            return responseAjaxResult;

        }

        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipTypePullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();

            responseAjaxResult.Data = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1)
                .Select(x => new BasePullDownResponseDto() { Id = x.PomId, Name = x.Name }).ToListAsync();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipStatusPullDownAsync()
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();

            responseAjaxResult.Data = await dbContext.Queryable<ShipStatus>().Where(x => x.IsDelete == 1)
                .Select(x => new BasePullDownResponseDto() { Id = x.Id, Name = x.Name }).ToListAsync();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion

        #region 获取设备信息
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetDeviceInformationAsync(GetDeviceInformationRequesDto getDeviceInformationResponseDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            if (getDeviceInformationResponseDto.Type == 1) // 水上设备
            {
                var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1 && x.Oid == 1).Select(x => new BasePullDownResponseDto
                {
                    Id = x.Id,
                    Name = x.DeviceName
                })
                 .ToListAsync();
                if (equipmentList != null)
                {
                    responseAjaxResult.Count = equipmentList.Count;
                    responseAjaxResult.Success();
                    responseAjaxResult.Data = equipmentList;
                }

            }
            else if (getDeviceInformationResponseDto.Type == 2)// 陆域设备
            {
                if (getDeviceInformationResponseDto.Id != null)
                {
                    var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1 && x.Id == getDeviceInformationResponseDto.Id).SingleAsync();
                    if (equipmentList != null)
                    {
                        var equipmentLists = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1 && x.Oid == equipmentList.Poid).Select(x => new BasePullDownResponseDto
                        {
                            Id = x.Id,
                            Name = x.DeviceName
                        })
                        .ToListAsync();
                        if (equipmentList != null)
                        {
                            responseAjaxResult.Count = equipmentLists.Count;
                            responseAjaxResult.Success();
                            responseAjaxResult.Data = equipmentLists;
                            return responseAjaxResult;
                        }
                    }
                }
                else
                {
                    var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1 && x.Oid == 3).Select(x => new BasePullDownResponseDto
                    {
                        Id = x.Id,
                        Name = x.DeviceName
                    })
                       .ToListAsync();
                    if (equipmentList != null)
                    {
                        responseAjaxResult.Count = equipmentList.Count;
                        responseAjaxResult.Success();
                        responseAjaxResult.Data = equipmentList;
                    }
                }
            }
            else if (getDeviceInformationResponseDto.Type == 3) // 特种设备
            {
                var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1 && x.Oid == 2).Select(x => new BasePullDownResponseDto
                {
                    Id = x.Id,
                    Name = x.DeviceName
                })
                .ToListAsync();
                if (equipmentList != null)
                {
                    responseAjaxResult.Count = equipmentList.Count;
                    responseAjaxResult.Success();
                    responseAjaxResult.Data = equipmentList;

                }
            }
            else if (getDeviceInformationResponseDto.Type == 4) // 航区
            {
                var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1 && x.Oid == 4).Select(x => new BasePullDownResponseDto
                {
                    Id = x.Id,
                    Name = x.DeviceName
                })
                .ToListAsync();
                if (equipmentList != null)
                {
                    responseAjaxResult.Count = equipmentList.Count;
                    responseAjaxResult.Success();
                    responseAjaxResult.Data = equipmentList;
                }
            }
            return responseAjaxResult;
        }
        #endregion

        #region 根据名称或手机号获取人员信息(ID)
        /// <summary>
        /// 根据名称或手机号获取人员信息(ID)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchUserInformationAsync(BaseRequestDto baseRequestDto)
        {
            RefAsync<int> total = 0;
            //大东
            Guid Dadong = "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid();
            //int Quantity = 0;
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            List<BasePullDownResponseDto> userList = new List<BasePullDownResponseDto>();
            userList = await baseUserRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.Id != Dadong)
                .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => SqlFunc.Contains(x.Name, baseRequestDto.KeyWords)
                || SqlFunc.Contains(x.Phone, baseRequestDto.KeyWords))
                .Select(x => new BasePullDownResponseDto() { Name = x.Name + "(" + x.Phone + ")", Code = x.Phone, Id = x.Id })
                .ToListAsync();
            responseAjaxResult.Data = userList;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 获取首页菜单权限用户列表
        /// <summary>
        /// 获取首页菜单权限用户列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<InformationResponseDto>>> GetHomeMenuPermissionUserAsync(BaseRequestDto baseRequestDto)
        {
            RefAsync<int> total = 0;
            var responseAjaxResult = new ResponseAjaxResult<List<InformationResponseDto>>();
            var institutionRole = await dbContext.Queryable<InstitutionRole>().ToListAsync();
            var roleUserList = institutionRole.Select(x => x.UserId).Distinct().ToList();
            var institution = await dbContext.Queryable<Institution>().ToListAsync();
            DictionaryTableRequestDto baseDictionaryTableRequestDto = new DictionaryTableRequestDto()
            {
                Type = 11
            };
            var userInformation = await SearchDictionaryTableTreeAsyncc(baseDictionaryTableRequestDto);
            if (userInformation.Data.Any())
            {
                var userAccount = userInformation.Data.Select(x => x.Name).ToList();
                var userList = await baseUserRepository.AsQueryable().Where(x => userAccount.Contains(x.GroupCode))
                    .WhereIF(baseRequestDto.KeyWords != null, x => x.Name.Contains(baseRequestDto.KeyWords) || x.Phone.Contains(baseRequestDto.KeyWords))
                    .OrderByDescending(x => x.CreateTime)
                    .Select(x => new InformationResponseDto
                    {
                        UserId = x.Id,
                        Name = x.Name + "(" + x.Phone + ")",
                        LoginAccount = x.GroupCode,
                        Company = x.CompanyId.ToString(),
                        DepartmentName = x.DepartmentId.ToString(),
                        Phone = x.Phone
                    }).ToListAsync();

                var userList1 = await baseUserRepository.AsQueryable().Where(x => !userAccount.Contains(x.GroupCode) && roleUserList.Contains(x.Id))
                    .WhereIF(baseRequestDto.KeyWords != null, x => x.Name.Contains(baseRequestDto.KeyWords) || x.Phone.Contains(baseRequestDto.KeyWords))
                    .Select(x => new InformationResponseDto
                    {
                        UserId = x.Id,
                        Name = x.Name + "(" + x.Phone + ")",
                        LoginAccount = "2",
                        Company = x.CompanyId.ToString(),
                        DepartmentName = x.DepartmentId.ToString(),
                        Phone = x.Phone
                    }).ToListAsync();
                userList = userList.Concat(userList1).ToList();
                int skipCount = (baseRequestDto.PageIndex - 1) * baseRequestDto.PageSize;
                var user = userList.Skip(skipCount).Take(baseRequestDto.PageSize).ToList();
                foreach (var item in user)
                {
                    if (item.Company != null)
                    {
                        item.Company = institution.Where(y => y.PomId == item.Company.ToGuid()).Select(x => x.Name).FirstOrDefault();
                    }
                    if (item.DepartmentName != null)
                    {
                        item.DepartmentName = institution.Where(y => y.PomId == item.DepartmentName.ToGuid()).Select(x => x.Name).FirstOrDefault();
                    }
                    if (item.LoginAccount != null && item.LoginAccount != "2")
                    {
                        item.LoginAccount = userInformation.Data.FirstOrDefault(y => y.Name == item.LoginAccount)?.Type.ToString();
                    }
                }
                responseAjaxResult.Data = user;
                responseAjaxResult.Count = userList.Count;
                responseAjaxResult.Success();
            }
            else
            {
                var userList = await baseUserRepository.AsQueryable()
                    .Where(x => roleUserList.Contains(x.Id))
                    .WhereIF(baseRequestDto.KeyWords != null, x => x.Name.Contains(baseRequestDto.KeyWords) || x.Phone.Contains(baseRequestDto.KeyWords))
                    .Select(x => new InformationResponseDto
                    {
                        UserId = x.Id,
                        Name = x.Name + "(" + x.Phone + ")",
                        LoginAccount = "2",
                        Company = x.CompanyId.ToString(),
                        DepartmentName = x.DepartmentId.ToString(),
                    }).ToPageListAsync(baseRequestDto.PageIndex, baseRequestDto.PageSize, total);
                foreach (var item in userList)
                {
                    if (item.Company != null)
                    {
                        item.Company = institution.Where(y => y.PomId == item.Company.ToGuid()).Select(x => x.Name).FirstOrDefault();
                    }
                    if (item.DepartmentName != null)
                    {
                        item.DepartmentName = institution.Where(y => y.PomId == item.DepartmentName.ToGuid()).Select(x => x.Name).FirstOrDefault();
                    }
                }
                responseAjaxResult.Data = userList;
                responseAjaxResult.Count = total;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectAsync(BaseRequestDto baseRequestDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            responseAjaxResult.Data = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => x.Name.Contains(baseRequestDto.KeyWords) ||
                x.ShortName.Contains(baseRequestDto.KeyWords))
                .Select(x => new BasePullDownResponseDto() { Code = x.MasterCode, Name = x.Name, Id = x.Id }).ToListAsync();
            responseAjaxResult.Count = responseAjaxResult.Data.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 记录每年的节假日日期  每年写一次
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RecordHolidayAsync(int year)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            List<HolidayConfig> holidayConfigs = new List<HolidayConfig>();
            WebHelper webHelper = new WebHelper();
            var url = AppsettingsHelper.GetValue("Holidays").Replace("@year", year.ToString());
            var responseData = await webHelper.DoGetAsync(url);

            if (responseData.Code == 200 && responseData.Result != null)
            {
                //解析json
                var parseData = JObject.Parse(responseData.Result).Values();
                foreach (var item in parseData)
                {
                    holidayConfigs.Add(new HolidayConfig()
                    {
                        Id = GuidUtil.Next(),
                        Title = item["name"].ToString(),
                        IsHoliday = item["isOffDay"].ToString() == "True" ? 1 : 0,
                        HolidayTime = item["date"].ToString().ObjToDate(),
                        DateDay = int.Parse(item["date"].ToString().ObjToDate().ToString("yyyyMMdd"))

                    });
                }

                if (holidayConfigs.Count > 0)
                {
                    await dbContext.Insertable<HolidayConfig>(holidayConfigs).ExecuteCommandAsync();
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        /// <summary>
        /// 管理类型
        /// </summary>
        /// <param name="projectTypRequsetDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchManagerTypeAsync(ProjectTypRequsetDto projectTypRequsetDto)
        {
            ResponseAjaxResult<List<BasePullDownResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<BasePullDownResponseDto>>();
            var ProjectTypeList = await dbContext.Queryable<ProjectMangerType>()
             .Where(x => x.IsDelete == 1)
            .WhereIF(!string.IsNullOrWhiteSpace(projectTypRequsetDto.Name), x => SqlFunc.Contains(x.Name, projectTypRequsetDto.Name))
             .Select(x => new BasePullDownResponseDto { Id = x.Id, Name = x.Name, Type = SqlFunc.ToInt32(x.Code), Code = x.BusinessRemark }).OrderBy(x => x.Type).ToListAsync();
            if (projectTypRequsetDto.EnableRemark == 1)
            {
                responseAjaxResult.Data = ProjectTypeList.Select(x => new BasePullDownResponseDto() { Id = x.Id, Type = x.Type, Name = x.Name + x.Code }).ToList();
            }
            else
            {
                responseAjaxResult.Data = ProjectTypeList;
            }
            responseAjaxResult.Count = ProjectTypeList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 日报审批推送提醒
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DayReportApprovePushAsync()
        {
            var phonePage = AppsettingsHelper.GetValue("PhoneDayUrl");
            var pushJjtUserList = await dbContext.Queryable<DayReportJjtPushConfi>().Where(x => x.IsDelete == 1 && x.Type == 7).Select(x => x.PushAccount).ToListAsync();
            foreach (var item in pushJjtUserList)
            {
                if (item != "2022002687")
                {
                   var userIdCrypt=CryptoStringExtension.EncryptAsync(item);
                    phonePage = phonePage.Replace("@vali", userIdCrypt).TrimAll();
                    //九点第一批人员发送
                    var obj = new SingleMessageTemplateRequestDto()
                    {
                        MessageType = "text",
                        TextContent = $"您好:" + DateTime.Now.AddDays(-1).ToString("MM月dd") + "日报还未审核,请登录智慧运营中心-日报推送审核页面进行审核;移动端请访问:<br>" + phonePage,
                        UserIds = new List<string>() { item }
                    };
                    var pushResult = JjtUtils.SinglePushMessage(obj, false);
                }
            }
            return true;
        }

        /// <summary>
        /// 页面审核
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DayReportApproveAsync(bool isApprove,string? vali)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (!string.IsNullOrWhiteSpace(vali))
            {
                var account = CryptoStringExtension.DecryptAsync(vali);
                if (!string.IsNullOrWhiteSpace(account))
                {
                    var obj = new DayPushApprove()
                    {
                        ApproveId = _currentUser.Id,
                        ApproveName = (account== "L20132106"? "姜世珂": account == "2018015149" ? "刘国银": account == "2022002687"?"管理员": "于亚南"),
                        Status = isApprove ? "已审批" : "未审批",
                        DayTime = DateTime.Now.AddDays(-1).ToDateDay(),
                    };
                    await dbContext.Insertable<DayPushApprove>(obj).ExecuteCommandAsync();
                    responseAjaxResult.Data = true;
                    #region 触发定时任务
                    if (DateTime.Now.Hour >= 10)
                    {
                        var res = AppsettingsHelper.GetValue("jjtPush");
                        WebHelper webHelper = new WebHelper();
                        webHelper.DoGetAsync(res);
                    }
                   
                    #endregion
                    responseAjaxResult.Success();
                }
            }
            else {

                var token= HttpContentAccessFactory.GetUserToken;
                var userService= HttpContentAccessFactory.Current.Request.HttpContext.RequestServices.GetService<IUserService>();
                if (userService == null)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success("审核错误", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
               var userInfo= userService.GetUserInfoAsync(token);
                if (userInfo.CurrentLoginIsAdmin ||
                   userInfo.Account == "2018015149" ||
                   userInfo.Account == "2016146439" ||
                   userInfo.Account == "L20132106")
                {
                    var obj = new DayPushApprove()
                    {
                        ApproveId = _currentUser.Id,
                        ApproveName = _currentUser.Name,
                        Status = isApprove ? "已审批" : "未审批",
                        DayTime = DateTime.Now.AddDays(-1).ToDateDay(),
                    };
                    await dbContext.Insertable<DayPushApprove>(obj).ExecuteCommandAsync();
                    #region 触发定时任务
                    if (DateTime.Now.Hour >= 10)
                    {
                        var res = AppsettingsHelper.GetValue("jjtPush");
                        WebHelper webHelper = new WebHelper();
                        webHelper.DoGetAsync(res);
                    }

                    #endregion
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                }
                else
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success("您没有权限审核", HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
            }



            return responseAjaxResult;
        }
        public async Task<ResponseAjaxResult<string>> SearchDayReportApproveAsync()
        {
            ResponseAjaxResult<string> responseAjaxResult = new ResponseAjaxResult<string>();
            var time = DateTime.Now.AddDays(-1).ToDateDay();
            var staus = await dbContext.Queryable<DayPushApprove>().Where(x => x.DayTime == time).Select(x => x.Status).FirstAsync();
            responseAjaxResult.Data = staus;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 排除一些项目  如菲律宾帕塞吹填开发项目（1期）水工工程   只有交建公司能看到这个项目   
        /// 其他公司包括局管理员如陈翠  他们也看不到这个项目 ，并且项目导出  月报 日报 生产日报统计等等 都不在范围内，交建公司要计算在内的  原因是  他是属于某个项目的分项，
        /// 所以不需要看到这个项目
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public async Task UpdateProjectAsync()
        //{
        //    try
        //    {
        //        var project = await dbContext.Queryable<Project>().Where(x =>x.Id == "08dcdec4-4d90-4802-80fe-1293e55fbdff".ToGuid())
        //                           .FirstAsync();
        //        if (_currentUser.CurrentLoginIsAdmin||_currentUser.CurrentLoginInstitutionGrule.IndexOf("101174265") > 0  )
        //        {
        //            //说明是交建公司  
        //            project.IsDelete = 1;
        //            await dbContext.Updateable<Project>(project).ExecuteCommandAsync();
        //        }
        //        else
        //        {
        //            project.IsDelete = 0;
        //            await dbContext.Updateable<Project>(project).ExecuteCommandAsync();
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        await Console.Out.WriteLineAsync($"激活登录用户删除项目出现问题:{ex}");

        //    }

        //}
        #endregion

        #region 控制项目隐藏 日报推送使用

        ///控制项目隐藏 日报推送使用
        public async Task<ResponseAjaxResult<bool>> UpdateShowProjectAsync(Guid projectId)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var project = await dbContext.Queryable<ProjectOpen>().Where(x => x.ProjectId == projectId).FirstAsync();
            if (project == null)
            {
                await dbContext.Insertable<ProjectOpen>(new ProjectOpen()
                {
                    ProjectId = projectId,
                    IsShow = 0
                }).ExecuteCommandAsync();
            }
            if (project != null && project.IsShow == 1)
            {
                project.IsShow = 0;
            }
            else if (project != null && project.IsShow == 0)
            {
                project.IsShow = 1;
            }
            await dbContext.Updateable<ProjectOpen>(project).ExecuteCommandAsync();
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        public async Task<ResponseAjaxResult<List<ProjectOpen>>> SelectShowProjectAsync()
        {
            ResponseAjaxResult<List<ProjectOpen>> responseAjaxResult = new ResponseAjaxResult<List<ProjectOpen>>();
            responseAjaxResult.Data = await dbContext.Queryable<ProjectOpen>().Where(x => x.IsDelete == 1).ToListAsync();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion
    }
}
