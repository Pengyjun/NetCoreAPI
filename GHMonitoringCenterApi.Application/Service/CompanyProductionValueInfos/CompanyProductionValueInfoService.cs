using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using Model = GHMonitoringCenterApi.Domain.Models;



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

        public IBaseRepository<Project> baseProjectRepository { get; set; }

        public IBaseRepository<MonthReport> baseMonthReportRepository { get; set; }

        public IBaseRepository<CompanyAdjustmentValue> baseCompanyAdjustmentValueRepository { get; set; }

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
        public CompanyProductionValueInfoService(IBaseRepository<Model.CompanyProductionValueInfo> baseCompanyProductionValueInfoRepository, IBaseRepository<Company> baseCompanyRepository, ISqlSugarClient dbContext, IMapper mapper, IBaseService baseService, ILogService logService, IEntityChangeService entityChangeService, GlobalObject globalObject, IBaseRepository<Institution> dbInstitution, IBaseRepository<ProductionMonitoringOperationDayReport> productionMonitoringOperationDayReport, IBaseRepository<Project> baseProjectRepository, IBaseRepository<MonthReport> baseMonthReportRepository, IBaseRepository<CompanyAdjustmentValue> baseCompanyAdjustmentValueRepository)
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
            this.baseProjectRepository = baseProjectRepository;
            this.baseMonthReportRepository = baseMonthReportRepository;
            this.baseCompanyAdjustmentValueRepository = baseCompanyAdjustmentValueRepository;
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


            companyProductionValueInfoRequestDto.OnePlanProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.OnePlanProductionValue);

            companyProductionValueInfoRequestDto.EightPlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.EightPlaProductionValue);

            companyProductionValueInfoRequestDto.ElevenPlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.ElevenPlaProductionValue);

            companyProductionValueInfoRequestDto.FourPlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.FourPlaProductionValue);

            companyProductionValueInfoRequestDto.FivePlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.FivePlaProductionValue);

            companyProductionValueInfoRequestDto.NinePlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.NinePlaProductionValue);

            companyProductionValueInfoRequestDto.SevenPlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.SevenPlaProductionValue);

            companyProductionValueInfoRequestDto.SixPlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.SixPlaProductionValue);

            companyProductionValueInfoRequestDto.TenPlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.TenPlaProductionValue);

            companyProductionValueInfoRequestDto.ThreePlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.ThreePlaProductionValue);

            companyProductionValueInfoRequestDto.TwelvePlaProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.TwelvePlaProductionValue);

            companyProductionValueInfoRequestDto.TwoPlanProductionValue = Setnumericalconversiontwo(companyProductionValueInfoRequestDto.TwoPlanProductionValue);

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
            var List = await baseCompanyProductionValueInfoRepository.AsQueryable().InnerJoin<ProductionMonitoringOperationDayReport>((x, c) => x.CompanyId == c.ItemId && c.IsDelete == 1 && c.Type == 1)
            .Where(x => x.IsDelete == 1)
            .Where((x, c) => c.Name != "广航局总体" && !SqlFunc.IsNullOrEmpty(c.Name))
            .WhereIF(requsetDto.Dateday.HasValue, x => x.DateDay == requsetDto.Dateday)
            .OrderBy(x => x.Sort)
            .Select((x, c) => new CompanyProductionValueInfoResponseDto
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                CompanyName = c.Name,
                DateDay = x.DateDay,
                OnePlanProductionValue = x.OnePlanProductionValue,
                TwoPlanProductionValue = x.TwoPlanProductionValue,
                ThreePlaProductionValue = x.ThreePlaProductionValue,
                FourPlaProductionValue = x.FourPlaProductionValue,
                FivePlaProductionValue = x.FivePlaProductionValue,
                SixPlaProductionValue = x.SixPlaProductionValue,
                SevenPlaProductionValue = x.SevenPlaProductionValue,
                EightPlaProductionValue = x.EightPlaProductionValue,
                NinePlaProductionValue = x.NinePlaProductionValue,
                TenPlaProductionValue = x.TenPlaProductionValue,
                ElevenPlaProductionValue = x.ElevenPlaProductionValue,
                TwelvePlaProductionValue = x.TwelvePlaProductionValue,
                Sort = x.Sort
            }).ToPageListAsync(requsetDto.PageIndex, requsetDto.PageSize, total);

            var institutionIds = new List<Guid?>();
            institutionIds.AddRange(List.Select(t => t.CompanyId).Distinct().ToArray());

            var list = await baseProjectRepository.AsQueryable().Where(x => institutionIds.Contains(x.CompanyId) && x.IsDelete == 1).Select(it => new Project()
            {
                CompanyId = it.CompanyId,
                Name = it.Name,
                Id = it.Id
            }).ToListAsync();

            var planIds = new List<Guid?>();
            planIds.AddRange(List.Select(t => t.Id).Distinct().ToArray());
            var adjustmentValues = await baseCompanyAdjustmentValueRepository.AsQueryable().Where(x => planIds.Contains(x.PlanId)).Select(it => new CompanyAdjustmentValue()
            {
                PlanId = it.PlanId,
                Id = it.Id,
                AdjustmentValue = it.AdjustmentValue,
                Month = it.Month
            }).ToListAsync();

            foreach (var item in List)
            {
                item.adjustmentValues = adjustmentValues.Where(x => x.PlanId == item.Id).OrderBy(x => x.Month).ToList();

                item.OnePlanProductionValue = Setnumericalconversion(item.OnePlanProductionValue);

                item.EightPlaProductionValue = Setnumericalconversion(item.EightPlaProductionValue);

                item.ElevenPlaProductionValue = Setnumericalconversion(item.ElevenPlaProductionValue);

                item.FourPlaProductionValue = Setnumericalconversion(item.FourPlaProductionValue);

                item.FivePlaProductionValue = Setnumericalconversion(item.FivePlaProductionValue);

                item.NinePlaProductionValue = Setnumericalconversion(item.NinePlaProductionValue);

                item.SevenPlaProductionValue = Setnumericalconversion(item.SevenPlaProductionValue);

                item.SixPlaProductionValue = Setnumericalconversion(item.SixPlaProductionValue);

                item.TenPlaProductionValue = Setnumericalconversion(item.TenPlaProductionValue);

                item.ThreePlaProductionValue = Setnumericalconversion(item.ThreePlaProductionValue);

                item.TwelvePlaProductionValue = Setnumericalconversion(item.TwelvePlaProductionValue);

                item.TwoPlanProductionValue = Setnumericalconversion(item.TwoPlanProductionValue);

                var clist = list.Where(x => x.CompanyId == item.CompanyId).ToList();

                int year = item.DateDay.GetValueOrDefault();
                var targetYear = year;
                var months = new List<int>();

                months = Enumerable.Range(1, 12)
               .Select(m => targetYear * 100 + m)
               .ToList();


                item.MonthlyDatas = await baseMonthReportRepository.AsQueryable().Where(x => clist.Select(it => it.Id).ToList().Contains(x.ProjectId) && months.Contains(x.DateMonth) && x.IsDelete == 1 && x.Status != MonthReportStatus.ApproveReject).GroupBy(x => new { x.DateMonth })
                    .Select(x => new MonthlyDataProductionValue
                    {
                        Month = x.DateMonth,
                        Total = SqlFunc.AggregateSum(x.RollingPlanForNextMonth)
                    }).ToListAsync();

                foreach (var item1 in item.MonthlyDatas)
                {
                    item1.Total = Setnumericalconversion(item1.Total);

                    item1.ActualDate = AddMonthToIntDate(item1.Month);
                }

                foreach (var itema in item.adjustmentValues)
                {
                    itema.AdjustmentValue = Setnumericalconversion(itema.AdjustmentValue);
                }

                months.RemoveAll(x => item.MonthlyDatas.Select(it => it.Month).Contains(x));
                item.MonthlyDatas = item.MonthlyDatas.Union(months.Select(it => new MonthlyDataProductionValue() { Month = it, Total = 0 })).OrderBy(x => x.Month).ToList();

                foreach (var item1 in item.MonthlyDatas)
                {
                    item1.ActualDate = AddMonthToIntDate(item1.Month);
                }
            }

            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
        }

        /// <summary>
        /// 去除小数点及转换成万元
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal Setnumericalconversion(decimal? value)
        {
            if (value.GetValueOrDefault() == 0)
            {
                return 0;
            }
            else
            {
                var val = Math.Floor((value.GetValueOrDefault() / 10000) * 100) / 100;

                return Convert.ToDecimal(val.ToString("0"));
            }
        }

        /// <summary>
        /// 去除小数点及转换成元
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal Setnumericalconversiontwo(decimal? value)
        {
            if (value.GetValueOrDefault() == 0)
            {
                return 0;
            }
            else
            {
                var val = value.GetValueOrDefault() * 10000;
                return val;
            }
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
                        .OrderBy(x => x.Sort)
                        .Select(x => new ProductionMonitoringOperationDayReport
                        {
                            ItemId = x.ItemId,
                            Name = x.Name

                        }).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddORUpdateCompanyAdjustmentValueAsync(AddORUpdateCompanyAdjustmentValueRequestDto requestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            Guid id = GuidUtil.Next();

            var model = await baseCompanyAdjustmentValueRepository.AsQueryable().Where(x => x.PlanId == requestDto.PlanId && x.Month == requestDto.Month).FirstAsync();
            if (model == null)
            {
                requestDto.Adjustmentvalue = Setnumericalconversiontwo(requestDto.Adjustmentvalue);
                var mappermodel = mapper.Map<AddORUpdateCompanyAdjustmentValueRequestDto, CompanyAdjustmentValue>(requestDto);
                mappermodel.Id = id;
                mappermodel.IsDelete = 1;
                mappermodel.CreateId = _currentUser.Id;
                mappermodel.CreateTime = DateTime.Now;
                var result = await dbContext.Insertable<Model.CompanyAdjustmentValue>(mappermodel).ExecuteCommandAsync();
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
                if (model != null)
                {
                    model.AdjustmentValue = Setnumericalconversiontwo(requestDto.Adjustmentvalue);
                    model.UpdateId = _currentUser.Id;
                    model.UpdateTime = DateTime.Now;
                    var save = await dbContext.Updateable<Model.CompanyAdjustmentValue>(model).UpdateColumns(it => new
                    {
                        it.AdjustmentValue,
                        it.UpdateId,
                        it.UpdateTime
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
        /// 添加月份
        /// </summary>
        /// <param name="dateInt"></param>
        /// <returns></returns>
        public static int AddMonthToIntDate(int dateInt)
        {
            string dateStr = dateInt.ToString("000000") + "01";
            DateTime date = DateTime.ParseExact(dateStr, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            date = date.AddMonths(1);
            return int.Parse(date.ToString("yyyyMM"));
        }
    }
}
