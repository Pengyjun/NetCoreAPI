using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Domain.IRepository;
using Model = GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.SearchUser;
using Spire.Doc.Documents;
using UtilsSharp;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.SqlSugarCore;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Service.Push;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace GHMonitoringCenterApi.Application.Service.CompanyProductionValueInfos
{
    /// <summary>
    /// 公司产值信息
    /// </summary>
    public class CompanyProductionValueInfoService : ICompanyProductionValueInfoService
    {

        #region 依赖注入
        public IBaseRepository<Model.CompanyProductionValueInfo> baseCompanyProductionValueInfoRepository { get; set; }
        public IBaseRepository<Company> baseCompanyRepository { get; set; }

        public ISqlSugarClient dbContext { get; set; }
        public IMapper mapper { get; set; }
        public IBaseService baseService { get; set; }
        public ILogService logService { get; set; }
        public IEntityChangeService entityChangeService { get; set; }

        /// <summary>
        /// 机构（公司）
        /// </summary>
        private readonly IBaseRepository<Institution> _dbInstitution;

        /// <summary>
        /// 当前用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        public IBaseRepository<ProductionMonitoringOperationDayReport> productionMonitoringOperationDayReport;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseCompanyProductionValueInfoRepository"></param>
        /// <param name="baseCompanyRepository"></param>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="baseService"></param>
        /// <param name="logService"></param>
        /// <param name="entityChangeService"></param>
        /// <param name="globalObject"></param>
        public CompanyProductionValueInfoService(IBaseRepository<Model.CompanyProductionValueInfo> baseCompanyProductionValueInfoRepository, IBaseRepository<Company> baseCompanyRepository, ISqlSugarClient dbContext, IMapper mapper, IBaseService baseService, ILogService logService, IEntityChangeService entityChangeService, GlobalObject globalObject, IBaseRepository<Institution> dbInstitution, IBaseRepository<ProductionMonitoringOperationDayReport> productionMonitoringOperationDayReport)
        {
            this.baseCompanyProductionValueInfoRepository = baseCompanyProductionValueInfoRepository;
            this.baseCompanyRepository = baseCompanyRepository;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.baseService = baseService;
            this.logService = logService;
            this.entityChangeService = entityChangeService;
            _globalObject = globalObject;
            _dbInstitution = dbInstitution;
            this.productionMonitoringOperationDayReport = productionMonitoringOperationDayReport;
        }


        #endregion

        /// <summary>
        /// 添加或修改公司产值信息
        /// </summary>
        /// <param name="companyProductionValueInfoRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddORUpdateCompanyProductionValueInfoAsync(AddOrUpdateCompanyProductionValueInfoRequestDto companyProductionValueInfoRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            Guid id = GuidUtil.Next();
            var model = new Model.CompanyProductionValueInfo();
            if (companyProductionValueInfoRequestDto.RequestType == true)
            {
                model = mapper.Map<AddOrUpdateCompanyProductionValueInfoRequestDto, Model.CompanyProductionValueInfo>(companyProductionValueInfoRequestDto);
                model.Id = id;
                model.IsDelete = 1;
                model.CreateId = _currentUser.Id;
                model.CreateTime = DateTime.Now;
                var result = await dbContext.Insertable<Model.CompanyProductionValueInfo>(model).ExecuteCommandAsync();
                if (result > 0)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                }
                else
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, Domain.Shared.Enums.HttpStatusCode.InsertFail);
                }
            }
            else
            {
                model = await baseCompanyProductionValueInfoRepository.AsQueryable().Where(x => x.Id == companyProductionValueInfoRequestDto.Id).FirstAsync();
                if (model != null)
                {
                    var mappermodel = mapper.Map<AddOrUpdateCompanyProductionValueInfoRequestDto, Model.CompanyProductionValueInfo>(companyProductionValueInfoRequestDto);
                    model.UpdateId = _currentUser.Id;
                    model.UpdateTime = DateTime.Now;
                    var save = await baseCompanyProductionValueInfoRepository.AsUpdateable(mappermodel).UpdateColumns(it => new
                    {
                        it.DateDay,
                        it.UpdateId,
                        it.UpdateTime,
                        it.OnePlanProductionValue,
                        it.TwoPlanProductionValue,
                        it.ThreePlaProductionValue,
                        it.FourPlaProductionValue,
                        it.FivePlaProductionValue,
                        it.SixPlaProductionValue,
                        it.SevenPlaProductionValue,
                        it.EightPlaProductionValue,
                        it.NinePlaProductionValue,
                        it.TenPlaProductionValue,
                        it.ElevenPlaProductionValue,
                        it.TwelvePlaProductionValue
                    }).ExecuteCommandAsync();
                    if (save > 0)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success();
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, Domain.Shared.Enums.HttpStatusCode.UpdateFail);
                    }
                }
            }
            return responseAjaxResult;
        }

        /// <summary>
        /// 删除公司产值信息
        /// </summary>
        /// <param name="deleteCompanyProductionValueInfoRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DelectCompanyProductionValueInfoAsync(DeleteCompanyProductionValueInfoRequestDto deleteCompanyProductionValueInfoRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();

            var model = await baseCompanyProductionValueInfoRepository.AsQueryable().Where(x => x.Id == deleteCompanyProductionValueInfoRequestDto.Id).FirstAsync();
            if (model != null)
            {
                model.IsDelete = 0;
                var save = await baseCompanyProductionValueInfoRepository.AsUpdateable(model).ExecuteCommandAsync();
                if (save > 0)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                }
                else
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL, Domain.Shared.Enums.HttpStatusCode.DeleteFail);
                }
            }
            else
            {
                //
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, Domain.Shared.Enums.HttpStatusCode.SaveFail);
            }
            return responseAjaxResult;
        }

        /// <summary>
        /// 查询公司产值信息
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<CompanyProductionValueInfoResponseDto>>> SearchCompanyProductionValueInfoAsync(CompanyProductionValueInfoRequsetDto requsetDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CompanyProductionValueInfoResponseDto>>();
            RefAsync<int> total = 0;
            var List = await baseCompanyProductionValueInfoRepository.AsQueryable()
            .Where(x => x.IsDelete == 1)
            .WhereIF(requsetDto.Dateday.HasValue, x => x.DateDay == requsetDto.Dateday)
            .OrderByDescending(x => x.Sort)
            .Select(x => new CompanyProductionValueInfoResponseDto
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                DateDay = x.DateDay,
                EightPlaProductionValue = x.EightPlaProductionValue,
                ElevenPlaProductionValue = x.ElevenPlaProductionValue,
                FourPlaProductionValue = x.FourPlaProductionValue,
                FivePlaProductionValue = x.FivePlaProductionValue,
                NinePlaProductionValue = x.NinePlaProductionValue,
                OnePlanProductionValue = x.OnePlanProductionValue,
                SevenPlaProductionValue = x.SevenPlaProductionValue,
                SixPlaProductionValue = x.SixPlaProductionValue,
                Sort = x.Sort,
                TenPlaProductionValue = x.TenPlaProductionValue,
                ThreePlaProductionValue = x.ThreePlaProductionValue,
                TwelvePlaProductionValue = x.TwelvePlaProductionValue,
                TwoPlanProductionValue = x.TwoPlanProductionValue
            }).ToPageListAsync(requsetDto.PageIndex, requsetDto.PageSize, total);

            var institutionIds = new List<Guid?>();
            //institutionIds.AddRange(List.Select(t => t.CompanyId).Distinct().ToArray());
            var institutions = await GetCompanyList();

            List.ForEach(item =>
            {

                item.CompanyName = institutions.FirstOrDefault(t => t.ItemId == item.CompanyId)?.Name;
            });
            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
        }

        private async Task<List<Institution>> GetInstitutionsAsync(Guid?[] institutionIds)
        {
            if (institutionIds == null || !institutionIds.Any())
            {
                return new List<Institution>();
            }
            return await _dbInstitution.GetListAsync(t => institutionIds.Contains(t.PomId) && t.IsDelete == 1);
        }

        /// <summary>
        /// 查询公司下拉框
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ProductionMonitoringOperationDayReport>>> SearchCompanyAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<List<ProductionMonitoringOperationDayReport>>();
            RefAsync<int> total = 0;
            List<ProductionMonitoringOperationDayReport> List = await GetCompanyList();
            responseAjaxResult.Data = List;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        private async Task<List<ProductionMonitoringOperationDayReport>> GetCompanyList()
        {
            return await productionMonitoringOperationDayReport.AsQueryable()
                        .Where(x => x.IsDelete == 1 && x.Name != "广航局总体" && !SqlFunc.IsNullOrEmpty(x.Name) && x.Type == 1)
                        .OrderByDescending(x => x.Sort)
                        .Select(x => new ProductionMonitoringOperationDayReport
                        {
                            ItemId = x.ItemId,
                            Name = x.Name

                        }).ToListAsync();
        }
    }
}
