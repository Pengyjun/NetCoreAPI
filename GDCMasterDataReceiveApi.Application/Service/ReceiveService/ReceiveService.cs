using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.Extensions.Logging;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.ReceiveService
{
    /// <summary>
    /// 接收主数据推送接口实现
    /// </summary>
    public class ReceiveService : IReceiveService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ReceiveService> logger;
        private readonly IBaseService baseService;
        /// <summary>
        /// 注入上下文
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="baseService"></param>
        public ReceiveService(ISqlSugarClient dbContext, IMapper mapper, ILogger<ReceiveService> logger, IBaseService baseService)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this.logger = logger;
            this.baseService = baseService;

        }
        /// <summary>
        /// 获取通用字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CommonDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CorresUnitDataAsync(BaseReceiveDataRequestDto<CorresUnitReceiveDto> baseReceiveDataRequestDto)
        {

            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            try
            {
                ////处理曾用名
                List<BankCard> insertzMDGS_OLDNAMEs = new();
                List<BankCard> updatezMDGS_OLDNAMEs = new();
                //查询项目表
                var projectCodeList = await _dbContext.Queryable<CorresUnit>().Where(x => x.IsDelete == 1).ToListAsync();
                //需要新增的数据
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !projectCodeList.Select(x => x.ZBP).ToList().Contains(x.ZBP)).ToList();
                //需要更新的数据
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => projectCodeList.Select(x => x.ZBP).ToList().Contains(x.ZBP)).ToList();
                //新增操作
                if (insertOids.Any())
                {
                    foreach (var itemItem in insertOids)
                    {
                        itemItem.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var items in itemItem.ZBANK.Item)
                        {
                            BankCard projectUsedName = new BankCard()
                            {
                                Id = itemItem.Id.Value,
                                ZBP = itemItem.ZBP,
                                ZBANK = items.ZBANK,
                                ZBANKN = items.ZBANKN,
                                ZBANKSTA = items.ZBANKSTA,
                                ZCITY2 = items.ZCITY2,
                                ZCOUNTY2 = items.ZCOUNTY2,
                                ZCURR = items.ZCURR,
                                ZFINAME = items.ZFINAME,
                                ZFINC = items.ZFINC,
                                ZKOINH = items.ZKOINH,
                                ZPROVINC2 = items.ZPROVINC2,
                                ZZCOUNTR2 = items.ZZCOUNTR2,
                                ZZSERIAL = items.ZZSERIAL
                            };
                            insertzMDGS_OLDNAMEs.Add(projectUsedName);
                        }
                    }
                    var projectList = _mapper.Map<List<CorresUnitReceiveDto>, List<CorresUnit>>(insertOids);
                    projectList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<CorresUnit>().BulkCopyAsync(projectList);
                    await _dbContext.Fastest<BankCard>().BulkCopyAsync(insertzMDGS_OLDNAMEs);
                }
                if (updateOids.Any())
                {
                    List<BankCard> deleteData = new List<BankCard>();
                    //更新曾用名
                    var projectUsedNameList = await _dbContext.Queryable<BankCard>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = projectCodeList.Where(x => x.ZBP == itemItem.ZBP).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZBANK.Item)
                        {
                            BankCard projectUsedName = new BankCard()
                            {
                                Id = itemItem.Id.Value,
                                ZBP = itemItem.ZBP,
                                ZBANK = items.ZBANK,
                                ZBANKN = items.ZBANKN,
                                ZBANKSTA = items.ZBANKSTA,
                                ZCITY2 = items.ZCITY2,
                                ZCOUNTY2 = items.ZCOUNTY2,
                                ZCURR = items.ZCURR,
                                ZFINAME = items.ZFINAME,
                                ZFINC = items.ZFINC,
                                ZKOINH = items.ZKOINH,
                                ZPROVINC2 = items.ZPROVINC2,
                                ZZCOUNTR2 = items.ZZCOUNTR2,
                                ZZSERIAL = items.ZZSERIAL
                            };
                            updatezMDGS_OLDNAMEs.Add(projectUsedName);
                        }
                        deleteData.AddRange(projectUsedNameList.Where(x => x.ZBP == itemItem.ZBP).ToList());
                    }
                    var projectList = _mapper.Map<List<CorresUnitReceiveDto>, List<CorresUnit>>(updateOids);
                    await _dbContext.Updateable(projectList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<BankCard>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updatezMDGS_OLDNAMEs).ExecuteCommandAsync();

                }
                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> EscrowOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> BusinessProjectDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CountryRegionDataAsync(BaseReceiveDataRequestDto<CountryRegionReceiveDto> baseReceiveDataRequest)
        {
            var responseAjaxResult = new MDMResponseResult();
            //处理多语言描述表类型
            List<CountryLanguage> insertzMDGS_OLDNAMEs = new();
            List<CountryLanguage> updatezMDGS_OLDNAMEs = new();
            //查询项目表
            var dataCodeList = await _dbContext.Queryable<CountryRegion>().Where(x => x.IsDelete == 1).ToListAsync();
            //需要新增的数据
            var insertOids = baseReceiveDataRequest.IT_DATA.item.Where(x => !dataCodeList.Select(x => x.ZCOUNTRYCODE).ToList().Contains(x.ZCOUNTRYCODE)).ToList();
            //需要更新的数据
            var updateOids = baseReceiveDataRequest.IT_DATA.item.Where(x => dataCodeList.Select(x => x.ZCOUNTRYCODE).ToList().Contains(x.ZCOUNTRYCODE)).ToList();
            //新增操作
            if (insertOids.Any())
            {
                foreach (var itemItem in insertOids)
                {
                    itemItem.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    foreach (var lang in itemItem.ZLANG_LIST.Item)
                    {

                        CountryLanguage dataItem = new CountryLanguage()
                        {
                            Id = itemItem.Id.Value,
                            ZLANGCODE = lang.ZLANGCODE,
                            ZCODE_DESC = lang.ZCODE_DESC,
                        };
                        insertzMDGS_OLDNAMEs.Add(dataItem);
                    }
                }
                var projectList = _mapper.Map<List<CountryRegionReceiveDto>, List<CountryRegion>>(insertOids);
                projectList.ForEach(x => x.CreateTime = DateTime.Now);
                await _dbContext.Fastest<CountryRegion>().BulkCopyAsync(projectList);
                await _dbContext.Fastest<CountryLanguage>().BulkCopyAsync(insertzMDGS_OLDNAMEs);
            }
            if (updateOids.Any())
            {
                List<CountryLanguage> deleteData = new List<CountryLanguage>();
                var projectUsedNameList = await _dbContext.Queryable<CountryLanguage>().ToListAsync();
                foreach (var itemItem in updateOids)
                {
                    var id = dataCodeList.Where(x => x.ZCOUNTRYCODE == itemItem.ZCOUNTRYCODE).Select(x => x.Id).First();
                    itemItem.Id = id;
                    foreach (var items in itemItem.ZLANG_LIST.Item)
                    {
                        CountryLanguage dataItem = new CountryLanguage()
                        {
                            Id = itemItem.Id.Value,
                            ZLANGCODE = items.ZLANGCODE,
                            ZCODE_DESC = items.ZCODE_DESC,
                        };
                        updatezMDGS_OLDNAMEs.Add(dataItem);
                    }
                    deleteData.AddRange(projectUsedNameList.Where(x => x.Id == itemItem.Id).ToList());
                }
                var projectList = _mapper.Map<List<CountryRegionReceiveDto>, List<CountryRegion>>(updateOids);
                await _dbContext.Updateable(projectList).ExecuteCommandAsync();
                await _dbContext.Deleteable<CountryLanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                await _dbContext.Insertable(updatezMDGS_OLDNAMEs).ExecuteCommandAsync();

            }
            responseAjaxResult.Success();

            return responseAjaxResult;
        }
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CountryContinentDataAsync(BaseReceiveDataRequestDto<CountryContinentReceiveDto> baseReceiveDataRequestDto)
        {

            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            //处理多语言描述表类型
            List<CountryContinentLanguage> insertzMDGS_OLDNAMEs = new();
            List<CountryContinentLanguage> updatezMDGS_OLDNAMEs = new();
            //查询项目表
            var dataCodeList = await _dbContext.Queryable<CountryContinent>().Where(x => x.IsDelete == 1).ToListAsync();
            //需要新增的数据
            var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !dataCodeList.Select(x => x.ZCONTINENTCODE).ToList().Contains(x.ZCONTINENTCODE)).ToList();
            //需要更新的数据
            var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => dataCodeList.Select(x => x.ZCONTINENTCODE).ToList().Contains(x.ZCONTINENTCODE)).ToList();
            //新增操作
            if (insertOids.Any())
            {
                foreach (var itemItem in insertOids)
                {
                    itemItem.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    foreach (var lang in itemItem.ZLANG_LIST.item)
                    {

                        CountryContinentLanguage dataItem = new CountryContinentLanguage()
                        {
                            Id = itemItem.Id.Value,
                            ZLANGCODE = lang.ZLANGCODE,
                            ZAREA_DESC = lang.ZAREA_DESC,
                            ZCODE_DESC = lang.ZCODE_DESC,
                        };
                        insertzMDGS_OLDNAMEs.Add(dataItem);
                    }
                }
                var projectList = _mapper.Map<List<CountryContinentReceiveDto>, List<CountryContinent>>(insertOids);
                await _dbContext.Fastest<CountryContinent>().BulkCopyAsync(projectList);
                await _dbContext.Fastest<CountryContinentLanguage>().BulkCopyAsync(insertzMDGS_OLDNAMEs);
            }
            if (updateOids.Any())
            {
                List<CountryContinentLanguage> deleteData = new List<CountryContinentLanguage>();
                var projectUsedNameList = await _dbContext.Queryable<CountryContinentLanguage>().ToListAsync();
                foreach (var itemItem in updateOids)
                {
                    var id = dataCodeList.Where(x => x.ZCONTINENTCODE == itemItem.ZCONTINENTCODE).Select(x => x.Id).First();
                    itemItem.Id = id;
                    foreach (var items in itemItem.ZLANG_LIST.item)
                    {
                        CountryContinentLanguage dataItem = new CountryContinentLanguage()
                        {
                            Id = itemItem.Id.Value,
                            ZLANGCODE = items.ZLANGCODE,
                            ZAREA_DESC = items.ZAREA_DESC,
                            ZCODE_DESC = items.ZCODE_DESC,
                        };
                        updatezMDGS_OLDNAMEs.Add(dataItem);
                    }
                    deleteData.AddRange(projectUsedNameList.Where(x => x.Id == itemItem.Id).ToList());
                }
                var projectList = _mapper.Map<List<CountryContinentReceiveDto>, List<CountryContinent>>(updateOids);
                await _dbContext.Updateable(projectList).ExecuteCommandAsync();
                await _dbContext.Deleteable<CountryContinentLanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                await _dbContext.Insertable(updatezMDGS_OLDNAMEs).ExecuteCommandAsync();

            }
            responseAjaxResult.Success();

            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RegionalDataAsync(BaseReceiveDataRequestDto<RegionalItem> baseReceiveDataRequestDto)
        {
            var responseAjaxResult = new MDMResponseResult();

            try
            {
                List<RegionLanguage> insertItem = new();
                List<RegionLanguage> updateItem = new();

                var rlList = await _dbContext.Queryable<Regional>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !rlList.Select(x => x.ZCRHCODE).ToList().Contains(x.ZCRHCODE)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => rlList.Select(x => x.ZCRHCODE).ToList().Contains(x.ZCRHCODE)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.ZLANG_LIST.Item)
                        {
                            var ul = new RegionLanguage
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZCODE_DESC = cc.ZCODE_DESC,
                                ZLANGCODE = cc.ZLANGCODE,
                                ZSCRTEXT_S = cc.ZSCRTEXT_S,
                                Code = ic.ZCRHCODE,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(ul);
                        }
                    }

                    var mapList = _mapper.Map<List<RegionalItem>, List<Regional>>(insertOids);
                    mapList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<Regional>().BulkCopyAsync(mapList);
                }
                if (updateOids.Any())
                {
                    List<RegionLanguage> deleteData = new();
                    //更新
                    var lgeList = await _dbContext.Queryable<RegionLanguage>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = rlList.Where(x => x.ZCRHCODE == itemItem.ZCRHCODE).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZLANG_LIST.Item)
                        {
                            RegionLanguage ldetails = new RegionLanguage()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZCRHCODE,
                                ZSCRTEXT_S = items.ZSCRTEXT_S,
                                ZLANGCODE = items.ZLANGCODE,
                                ZCODE_DESC = items.ZCODE_DESC,
                                CreateTime = DateTime.Now
                            };
                            updateItem.Add(ldetails);
                        }
                        deleteData.AddRange(lgeList.Where(x => x.Code == itemItem.ZCRHCODE).ToList());
                    }
                    var mapList = _mapper.Map<List<RegionalItem>, List<Regional>>(updateOids);
                    await _dbContext.Updateable(mapList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<RegionLanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> UnitMeasurementDataAsync(BaseReceiveDataRequestDto<UnitMeasurementItem> baseReceiveDataRequestDto)
        {
            var responseAjaxResult = new MDMResponseResult();

            try
            {
                List<UnitMeasurementLanguage> insertItem = new();
                List<UnitMeasurementLanguage> updateItem = new();

                var ulList = await _dbContext.Queryable<UnitMeasurement>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !ulList.Select(x => x.ZUNITCODE).ToList().Contains(x.ZUNITCODE)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => ulList.Select(x => x.ZUNITCODE).ToList().Contains(x.ZUNITCODE)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.ZUNIT_LANG.Item)
                        {
                            var ul = new UnitMeasurementLanguage
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZSPRAS = cc.ZSPRAS,
                                ZSPTXT = cc.ZSPTXT,
                                ZUNITCODE = ic.ZUNITCODE,
                                ZUNITDESCR = cc.ZUNITDESCR,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(ul);
                        }
                    }

                    var mapList = _mapper.Map<List<UnitMeasurementItem>, List<UnitMeasurement>>(insertOids);
                    mapList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<UnitMeasurement>().BulkCopyAsync(mapList);
                }
                if (updateOids.Any())
                {
                    List<UnitMeasurementLanguage> deleteData = new();
                    //更新
                    var lgeList = await _dbContext.Queryable<UnitMeasurementLanguage>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = ulList.Where(x => x.ZUNITCODE == itemItem.ZUNITCODE).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZUNIT_LANG.Item)
                        {
                            UnitMeasurementLanguage ldetails = new UnitMeasurementLanguage()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZUNITDESCR = items.ZUNITDESCR,
                                ZUNITCODE = itemItem.ZUNITCODE,
                                ZSPTXT = items.ZSPTXT,
                                ZSPRAS = items.ZSPRAS,
                                CreateTime = DateTime.Now
                            };
                            updateItem.Add(ldetails);
                        }
                        deleteData.AddRange(lgeList.Where(x => x.ZUNITCODE == itemItem.ZUNITCODE).ToList());
                    }
                    var mapList = _mapper.Map<List<UnitMeasurementItem>, List<UnitMeasurement>>(updateOids);
                    await _dbContext.Updateable(mapList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<UnitMeasurementLanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ProjectClassificationDataAsync(BaseReceiveDataRequestDto<ProjectClassificationItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                var aaMapperList = await _dbContext.Queryable<ProjectClassification>().Where(x => x.IsDelete == 1).ToListAsync();
                //var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !aaMapperList.Select(x => x.ZID).ToList().Contains(x.ZID)).ToList();
                //var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => aaMapperList.Select(x => x.ZID).ToList().Contains(x.ZID)).ToList();

                //新增操作
                //if (insertOids.Any())
                //{
                //    foreach (var ic in insertOids)
                //    {
                //        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                //    }

                //    var mapRmList = _mapper.Map<List<ProjectClassificationItem>, List<ProjectClassification>>(insertOids);
                //    mapRmList.ForEach(x => x.CreateTime = DateTime.Now);
                //    await _dbContext.Fastest<ProjectClassification>().BulkCopyAsync(mapRmList);
                //}
                //if (updateOids.Any())
                //{
                //    ////更新
                //    //foreach (var itemItem in updateOids)
                //    //{
                //    //    var id = aaMapperList.Where(x => x.ZID == itemItem.ZID).Select(x => x.Id).First();
                //    //    itemItem.Id = id;
                //    //}
                //    //var mapRmList = _mapper.Map<List<ProjectClassificationItem>, List<ProjectClassification>>(updateOids);
                //    //await _dbContext.Updateable(mapRmList).ExecuteCommandAsync();
                //}

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> FinancialInstitutionDataAsync(BaseReceiveDataRequestDto<FinancialInstitutionReceiveDto> baseReceiveDataRequestDto)
        {

            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.FinancialInstitution,
                RequestParame = baseReceiveDataRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = baseReceiveDataRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                var projectList = _mapper.Map<List<FinancialInstitutionReceiveDto>, List<FinancialInstitution>>(baseReceiveDataRequestDto.IT_DATA.item);
                foreach (var item in projectList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                var projectCodeList = await _dbContext.Queryable<FinancialInstitution>().Where(x => x.IsDelete == 1).Select(x => x.ZFINC).ToListAsync();
                var insertOids = projectList.Where(x => !projectCodeList.Contains(x.ZFINC)).Select(x => x.ZFINC).ToList();
                var updateOids = projectList.Where(x => projectCodeList.Contains(x.ZFINC)).Select(x => x.ZFINC).ToList();
                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = projectList.Where(x => insertOids.Contains(x.ZFINC)).ToList();
                    await _dbContext.Fastest<FinancialInstitution>().BulkCopyAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = projectList.Where(x => updateOids.Contains(x.ZFINC)).ToList();
                    await _dbContext.Fastest<FinancialInstitution>().BulkUpdateAsync(batchData);
                }
                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {

                throw;
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> DeviceClassCodeDataAsync(BaseReceiveDataRequestDto<DeviceClassCodeItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                List<DeviceClassAttribute> insertItem = new();//分类属性
                List<DeviceClassAttribute> updateItem = new();//分类属性
                List<DeviceClassAttributeValue> insertItem2 = new();//分类属性值
                List<DeviceClassAttributeValue> updateItem2 = new();//分类属性值

                var dcCodes = await _dbContext.Queryable<DeviceClassCode>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !dcCodes.Select(x => x.ZCLASS).ToList().Contains(x.ZCLASS)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => dcCodes.Select(x => x.ZCLASS).ToList().Contains(x.ZCLASS)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.ZPROPERTY_LIST.Item)
                        {
                            var invoiceLanguage = new DeviceClassAttribute
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZATTRCODE = cc.ZATTRCODE,
                                ZATTRNAME = cc.ZATTRNAME,
                                ZATTRUNIT = cc.ZATTRUNIT,
                                ZCLASS = cc.ZCLASS,
                                ZREMARK = cc.ZREMARK,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(invoiceLanguage);
                        }
                        foreach (var cc in ic.ZVALUE_LIST.Item)
                        {
                            var invoiceLanguage = new DeviceClassAttributeValue
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZATTRCODE = cc.ZATTRCODE,
                                ZCLASS = cc.ZCLASS,
                                ZVALUECODE = cc.ZVALUECODE,
                                ZVALUENAME = cc.ZVALUENAME,
                                CreateTime = DateTime.Now
                            };
                            insertItem2.Add(invoiceLanguage);
                        }
                    }

                    var dCList = _mapper.Map<List<DeviceClassCodeItem>, List<DeviceClassCode>>(insertOids);
                    dCList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<DeviceClassAttribute>().BulkCopyAsync(insertItem);
                    await _dbContext.Fastest<DeviceClassAttributeValue>().BulkCopyAsync(insertItem2);
                }
                if (updateOids.Any())
                {
                    List<DeviceClassAttribute> deleteData = new();
                    List<DeviceClassAttributeValue> deleteData2 = new();
                    //更新
                    var dcAList = await _dbContext.Queryable<DeviceClassAttribute>().ToListAsync();
                    var dcAVlueList = await _dbContext.Queryable<DeviceClassAttributeValue>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = dcCodes.Where(x => x.ZCLASS == itemItem.ZCLASS).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZPROPERTY_LIST.Item)
                        {
                            DeviceClassAttribute dca = new DeviceClassAttribute()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZCLASS = itemItem.ZCLASS,
                                ZREMARK = items.ZREMARK,
                                ZATTRCODE = items.ZATTRCODE,
                                ZATTRNAME = items.ZATTRNAME,
                                ZATTRUNIT = items.ZATTRUNIT,
                                CreateTime = DateTime.Now
                            };
                            updateItem.Add(dca);
                        }
                        foreach (var items in itemItem.ZVALUE_LIST.Item)
                        {
                            DeviceClassAttributeValue dcav = new DeviceClassAttributeValue()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZCLASS = itemItem.ZCLASS,
                                ZATTRCODE = items.ZATTRCODE,
                                ZVALUECODE = items.ZVALUECODE,
                                ZVALUENAME = items.ZVALUENAME,
                                CreateTime = DateTime.Now
                            };
                            updateItem2.Add(dcav);
                        }
                        deleteData.AddRange(dcAList.Where(x => x.ZCLASS == itemItem.ZCLASS).ToList());
                    }
                    var dcCodeList = _mapper.Map<List<DeviceClassCodeItem>, List<DeviceClassCode>>(updateOids);
                    await _dbContext.Updateable(dcCodeList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<DeviceClassAttribute>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<DeviceClassAttributeValue>().WhereColumns(deleteData2, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem2).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {

                throw;
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AccountingDepartmentDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RegionalCenterDataAsync(BaseReceiveDataRequestDto<RegionalCenterItem> baseReceiveDataRequestDto)
        {
            var responseAjaxResult = new MDMResponseResult();

            try
            {
                List<RegionalCenterLanguage> insertItem = new();
                List<RegionalCenterLanguage> updateItem = new();

                var rlList = await _dbContext.Queryable<RegionalCenter>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !rlList.Select(x => x.ZCRCCODE).ToList().Contains(x.ZCRCCODE)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => rlList.Select(x => x.ZCRCCODE).ToList().Contains(x.ZCRCCODE)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.ZLANG_LIST.Item)
                        {
                            var ul = new RegionalCenterLanguage
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZCODE_DESC = cc.ZCODE_DESC,
                                ZLANGCODE = cc.ZLANGCODE,
                                Code = ic.ZCRCCODE,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(ul);
                        }
                    }

                    var mapList = _mapper.Map<List<RegionalCenterItem>, List<RegionalCenter>>(insertOids);
                    mapList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<RegionalCenter>().BulkCopyAsync(mapList);
                }
                if (updateOids.Any())
                {
                    List<RegionalCenterLanguage> deleteData = new();
                    //更新
                    var rcList = await _dbContext.Queryable<RegionalCenterLanguage>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = rlList.Where(x => x.ZCRCCODE == itemItem.ZCRCCODE).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZLANG_LIST.Item)
                        {
                            RegionalCenterLanguage rcdetails = new RegionalCenterLanguage()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZCRCCODE,
                                ZLANGCODE = items.ZLANGCODE,
                                ZCODE_DESC = items.ZCODE_DESC,
                                CreateTime = DateTime.Now
                            };
                            updateItem.Add(rcdetails);
                        }
                        deleteData.AddRange(rcList.Where(x => x.Code == itemItem.ZCRCCODE).ToList());
                    }
                    var mapList = _mapper.Map<List<RegionalCenterItem>, List<RegionalCenter>>(updateOids);
                    await _dbContext.Updateable(mapList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<RegionalCenterLanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }

            return responseAjaxResult;
        }
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> NationalEconomyDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AdministrativeOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> InvoiceTypeDataAsync(BaseReceiveDataRequestDto<InvoiceTypeItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                List<InvoiceLanguage> insertItem = new();
                List<InvoiceLanguage> updateItem = new();

                var invoiceCodes = await _dbContext.Queryable<InvoiceType>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !invoiceCodes.Select(x => x.ZINVTCODE).ToList().Contains(x.ZINVTCODE)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => invoiceCodes.Select(x => x.ZINVTCODE).ToList().Contains(x.ZINVTCODE)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.ZLANG_LIST.Item)
                        {
                            var invoiceLanguage = new InvoiceLanguage
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                InvoiceCode = ic.ZINVTCODE,
                                ZCODE_DESC = cc.ZCODE_DESC,
                                ZLANGCODE = cc.ZLANGCODE,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(invoiceLanguage);
                        }
                    }

                    var invoiceList = _mapper.Map<List<InvoiceTypeItem>, List<InvoiceType>>(insertOids);
                    invoiceList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<InvoiceType>().BulkCopyAsync(invoiceList);
                    await _dbContext.Fastest<InvoiceLanguage>().BulkCopyAsync(insertItem);
                }
                if (updateOids.Any())
                {
                    List<InvoiceLanguage> deleteData = new();
                    //更新
                    var invoiceLanguageList = await _dbContext.Queryable<InvoiceLanguage>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = invoiceCodes.Where(x => x.ZINVTCODE == itemItem.ZINVTCODE).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZLANG_LIST.Item)
                        {
                            InvoiceLanguage invoiceLanguage = new InvoiceLanguage()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                InvoiceCode = itemItem.ZINVTCODE,
                                ZCODE_DESC = items.ZCODE_DESC,
                                ZLANGCODE = items.ZLANGCODE,
                                CreateTime = DateTime.Now
                            };
                            updateItem.Add(invoiceLanguage);
                        }
                        deleteData.AddRange(invoiceLanguageList.Where(x => x.InvoiceCode == itemItem.ZINVTCODE).ToList());
                    }
                    var invoiceTypeItemList = _mapper.Map<List<InvoiceTypeItem>, List<InvoiceType>>(updateOids);
                    await _dbContext.Updateable(invoiceTypeItemList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<InvoiceLanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> CurrencyDataAsync(BaseReceiveDataRequestDto<CurrencyReceiveDto> baseReceiveDataRequestDto)
        {

            var responseAjaxResult = new MDMResponseResult();
            ////处理曾用名
            List<Currencylanguage> insertzMDGS_OLDNAMEs = new();
            List<Currencylanguage> updatezMDGS_OLDNAMEs = new();
            //查询项目表
            var projectCodeList = await _dbContext.Queryable<Currency>().Where(x => x.IsDelete == 1).ToListAsync();
            //需要新增的数据
            var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !projectCodeList.Select(x => x.ZCURRENCYCODE).ToList().Contains(x.ZCURRENCYCODE)).ToList();
            //需要更新的数据
            var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => projectCodeList.Select(x => x.ZCURRENCYCODE).ToList().Contains(x.ZCURRENCYCODE)).ToList();
            //新增操作
            if (insertOids.Any())
            {
                foreach (var itemItem in insertOids)
                {
                    itemItem.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    foreach (var items in itemItem.ZLANG_LIST.Item)
                    {
                        Currencylanguage projectUsedName = new Currencylanguage()
                        {
                            Id = itemItem.Id.Value,
                            ZCODE_DESC = items.ZCODE_DESC,
                            ZLANGCODE = items.ZLANGCODE
                        };
                        insertzMDGS_OLDNAMEs.Add(projectUsedName);
                    }
                }
                var projectList = _mapper.Map<List<CurrencyReceiveDto>, List<Currency>>(insertOids);
                await _dbContext.Fastest<Currency>().BulkCopyAsync(projectList);
                await _dbContext.Fastest<Currencylanguage>().BulkCopyAsync(insertzMDGS_OLDNAMEs);
            }
            if (updateOids.Any())
            {
                List<Currencylanguage> deleteData = new List<Currencylanguage>();
                //更新曾用名
                var projectUsedNameList = await _dbContext.Queryable<Currencylanguage>().ToListAsync();
                foreach (var itemItem in updateOids)
                {
                    var id = projectCodeList.Where(x => x.ZCURRENCYCODE == itemItem.ZCURRENCYCODE).Select(x => x.Id).First();
                    itemItem.Id = id;
                    foreach (var items in itemItem.ZLANG_LIST.Item)
                    {
                        Currencylanguage projectUsedName = new Currencylanguage()
                        {
                            Id = itemItem.Id.Value,
                            ZLANGCODE = items.ZLANGCODE,
                            ZCODE_DESC = items.ZCODE_DESC
                        };
                        updatezMDGS_OLDNAMEs.Add(projectUsedName);
                    }
                    deleteData.AddRange(projectUsedNameList.Where(x => x.Id == itemItem.Id).ToList());
                }
                var projectList = _mapper.Map<List<CurrencyReceiveDto>, List<Currency>>(updateOids);
                await _dbContext.Updateable(projectList).ExecuteCommandAsync();
                await _dbContext.Deleteable<Currencylanguage>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                await _dbContext.Insertable(updatezMDGS_OLDNAMEs).ExecuteCommandAsync();

            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AdministrativeAccountingMapperDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ProjectDataAsync(BaseReceiveDataRequestDto<ProjectItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                ////处理曾用名
                List<ProjectUsedName> insertzMDGS_OLDNAMEs = new();
                List<ProjectUsedName> updatezMDGS_OLDNAMEs = new();
                //查询项目表
                var projectCodeList = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
                //需要新增的数据
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !projectCodeList.Select(x => x.ZPROJECT).ToList().Contains(x.ZPROJECT)).ToList();
                //需要更新的数据
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => projectCodeList.Select(x => x.ZPROJECT).ToList().Contains(x.ZPROJECT)).ToList();
                //新增操作
                if (insertOids.Any())
                {
                    foreach (var itemItem in insertOids)
                    {
                        itemItem.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var items in itemItem.ZOLDNAME_LIST.item)
                        {
                            ProjectUsedName projectUsedName = new ProjectUsedName()
                            {
                                Id = itemItem.Id.Value,
                                ZOLDNAME = items.ZOLDNAME,
                                ZPROJECT = items.ZPROJECT
                            };
                            insertzMDGS_OLDNAMEs.Add(projectUsedName);
                        }
                    }
                    var projectList = _mapper.Map<List<ProjectItem>, List<Project>>(insertOids);
                    projectList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<Project>().BulkCopyAsync(projectList);
                    await _dbContext.Fastest<ProjectUsedName>().BulkCopyAsync(insertzMDGS_OLDNAMEs);
                }
                if (updateOids.Any())
                {
                    List<ProjectUsedName> deleteData = new List<ProjectUsedName>();
                    //更新曾用名
                    var projectUsedNameList = await _dbContext.Queryable<ProjectUsedName>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = projectCodeList.Where(x => x.ZPROJECT == itemItem.ZPROJECT).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZOLDNAME_LIST.item)
                        {
                            ProjectUsedName projectUsedName = new ProjectUsedName()
                            {
                                Id = itemItem.Id.Value,
                                ZOLDNAME = items.ZOLDNAME,
                                ZPROJECT = items.ZPROJECT
                            };
                            updatezMDGS_OLDNAMEs.Add(projectUsedName);
                        }
                        deleteData.AddRange(projectUsedNameList.Where(x => x.ZPROJECT == itemItem.ZPROJECT).ToList());
                    }
                    var projectList = _mapper.Map<List<ProjectItem>, List<Project>>(updateOids);
                    await _dbContext.Updateable(projectList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<ProjectUsedName>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updatezMDGS_OLDNAMEs).ExecuteCommandAsync();

                }
                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ScientifiCNoProjectDataAsync(BaseReceiveDataRequestDto<ScientifiCNoProjectItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                List<KySecUnit> insertSuItem = new();
                List<KySecUnit> updateSuItem = new();//二级单位
                List<KyCDUnit> insertCuItem = new();
                List<KyCDUnit> updateCuItem = new();//承担单位
                List<KyNameCeng> insertNcItem = new();
                List<KyNameCeng> updateNCItem = new();//曾用名
                List<KyCanYUnit> insertCyItem = new();
                List<KyCanYUnit> updateCyItem = new();//参与单位
                List<KyWeiTUnit> insertWtItem = new();
                List<KyWeiTUnit> updateWtItem = new();//委托单位
                List<KyPLeader> insertPlItem = new();
                List<KyPLeader> updatePlItem = new();//项目负责人
                List<KyCanYDep> insertCdItem = new();
                List<KyCanYDep> updateCdItem = new();//参与部门

                var sNpList = await _dbContext.Queryable<ScientifiCNoProject>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !sNpList.Select(x => x.ZSRP).ToList().Contains(x.ZSRP)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => sNpList.Select(x => x.ZSRP).ToList().Contains(x.ZSRP)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    #region 数据处理
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.IT_AI.Item)
                        {
                            var res = new KySecUnit
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                Z2NDORG = cc.Z2NDORG,
                                Z2NDORGN = cc.Z2NDORGN,
                                CreateTime = DateTime.Now
                            };
                            insertSuItem.Add(res);
                        }
                        foreach (var cc in ic.IT_AG.Item)
                        {
                            var res = new KyCDUnit
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                ZIOSIDE = cc.ZIOSIDE,
                                ZUDTK = cc.ZUDTK,
                                ZUDTKN = cc.ZUDTKN,
                                CreateTime = DateTime.Now
                            };
                            insertCuItem.Add(res);
                        }
                        foreach (var cc in ic.IT_ONAME.Item)
                        {
                            var res = new KyNameCeng
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                ZITEM = cc.ZITEM,
                                ZOLDNAME = cc.ZOLDNAME,
                                CreateTime = DateTime.Now
                            };
                            insertNcItem.Add(res);
                        }
                        foreach (var cc in ic.IT_AH.Item)
                        {
                            var res = new KyCanYUnit
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                ZIOSIDE = cc.ZIOSIDE,
                                ZPU = cc.ZPU,
                                ZPUN = cc.ZPUN,
                                CreateTime = DateTime.Now
                            };
                            insertCyItem.Add(res);
                        }
                        foreach (var cc in ic.IT_AK.Item)
                        {
                            var res = new KyWeiTUnit
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                ZAUTHORISE = cc.ZAUTHORISE,
                                ZAUTHORISEN = cc.ZAUTHORISEN,
                                ZIOSIDE = cc.ZIOSIDE,
                                CreateTime = DateTime.Now
                            };
                            insertWtItem.Add(res);
                        }
                        foreach (var cc in ic.IT_AJ.Item)
                        {
                            var res = new KyPLeader
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                ZPRINCIPAL = cc.ZPRINCIPAL,
                                ZPRINCIPALN = cc.ZPRINCIPALN,
                                CreateTime = DateTime.Now
                            };
                            insertPlItem.Add(res);
                        }
                        foreach (var cc in ic.IT_DE.Item)
                        {
                            var res = new KyCanYDep
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZSRP,
                                ZKZDEPART = cc.ZKZDEPART,
                                ZKZDEPARTNM = cc.ZKZDEPARTNM,
                                CreateTime = DateTime.Now
                            };
                            insertCdItem.Add(res);
                        }
                    }

                    var sNproList = _mapper.Map<List<ScientifiCNoProjectItem>, List<ScientifiCNoProject>>(insertOids);
                    await _dbContext.Fastest<ScientifiCNoProject>().BulkCopyAsync(sNproList);
                    await _dbContext.Fastest<KySecUnit>().BulkCopyAsync(insertSuItem);
                    await _dbContext.Fastest<KyCDUnit>().BulkCopyAsync(insertCuItem);
                    await _dbContext.Fastest<KyNameCeng>().BulkCopyAsync(insertNcItem);
                    await _dbContext.Fastest<KyCanYUnit>().BulkCopyAsync(insertCyItem);
                    await _dbContext.Fastest<KyWeiTUnit>().BulkCopyAsync(insertWtItem);
                    await _dbContext.Fastest<KyPLeader>().BulkCopyAsync(insertPlItem);
                    await _dbContext.Fastest<KyCanYDep>().BulkCopyAsync(insertCdItem);
                    #endregion
                }
                if (updateOids.Any())
                {
                    #region 数据处理
                    List<KySecUnit> deleteSuData = new();
                    List<KyCDUnit> deleteCuData = new();
                    List<KyNameCeng> deleteNcData = new();
                    List<KyCanYUnit> deleteCyData = new();
                    List<KyWeiTUnit> deleteWtData = new();
                    List<KyPLeader> deletePlData = new();
                    List<KyCanYDep> deleteCdData = new();
                    //更新
                    var suList = await _dbContext.Queryable<KySecUnit>().ToListAsync();
                    var cuList = await _dbContext.Queryable<KyCDUnit>().ToListAsync();
                    var ncList = await _dbContext.Queryable<KyNameCeng>().ToListAsync();
                    var cyList = await _dbContext.Queryable<KyCanYUnit>().ToListAsync();
                    var wtList = await _dbContext.Queryable<KyWeiTUnit>().ToListAsync();
                    var plList = await _dbContext.Queryable<KyPLeader>().ToListAsync();
                    var cdList = await _dbContext.Queryable<KyCanYDep>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = sNpList.Where(x => x.ZSRP == itemItem.ZSRP).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.IT_AI.Item)
                        {
                            KySecUnit res = new KySecUnit()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                Z2NDORG = items.Z2NDORG,
                                Z2NDORGN = items.Z2NDORGN,
                                CreateTime = DateTime.Now
                            };
                            updateSuItem.Add(res);
                        }
                        deleteSuData.AddRange(suList.Where(x => x.Code == itemItem.ZSRP).ToList());
                        foreach (var items in itemItem.IT_AG.Item)
                        {
                            KyCDUnit res = new KyCDUnit()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                ZIOSIDE = items.ZIOSIDE,
                                ZUDTK = items.ZUDTK,
                                ZUDTKN = items.ZUDTKN,
                                CreateTime = DateTime.Now
                            };
                            updateCuItem.Add(res);
                        }
                        deleteCuData.AddRange(cuList.Where(x => x.Code == itemItem.ZSRP).ToList());
                        foreach (var items in itemItem.IT_ONAME.Item)
                        {
                            KyNameCeng res = new KyNameCeng()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                ZITEM = items.ZITEM,
                                ZOLDNAME = items.ZOLDNAME,
                                CreateTime = DateTime.Now
                            };
                            updateNCItem.Add(res);
                        }
                        deleteNcData.AddRange(ncList.Where(x => x.Code == itemItem.ZSRP).ToList());
                        foreach (var items in itemItem.IT_AH.Item)
                        {
                            KyCanYUnit res = new KyCanYUnit()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                ZIOSIDE = items.ZIOSIDE,
                                ZPU = items.ZPU,
                                ZPUN = items.ZPUN,
                                CreateTime = DateTime.Now
                            };
                            updateCyItem.Add(res);
                        }
                        deleteCyData.AddRange(cyList.Where(x => x.Code == itemItem.ZSRP).ToList());
                        foreach (var items in itemItem.IT_AK.Item)
                        {
                            KyWeiTUnit res = new KyWeiTUnit()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                ZAUTHORISE = items.ZAUTHORISE,
                                ZAUTHORISEN = items.ZAUTHORISEN,
                                ZIOSIDE = items.ZIOSIDE,
                                CreateTime = DateTime.Now
                            };
                            updateWtItem.Add(res);
                        }
                        deleteWtData.AddRange(wtList.Where(x => x.Code == itemItem.ZSRP).ToList());
                        foreach (var items in itemItem.IT_AJ.Item)
                        {
                            KyPLeader res = new KyPLeader()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                ZPRINCIPAL = items.ZPRINCIPAL,
                                ZPRINCIPALN = items.ZPRINCIPALN,
                                CreateTime = DateTime.Now
                            };
                            updatePlItem.Add(res);
                        }
                        deletePlData.AddRange(plList.Where(x => x.Code == itemItem.ZSRP).ToList());
                        foreach (var items in itemItem.IT_DE.Item)
                        {
                            KyCanYDep res = new KyCanYDep()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = itemItem.ZSRP,
                                ZKZDEPART = items.ZKZDEPART,
                                ZKZDEPARTNM = items.ZKZDEPARTNM,
                                CreateTime = DateTime.Now
                            };
                            updateCdItem.Add(res);
                        }
                        deleteCdData.AddRange(cdList.Where(x => x.Code == itemItem.ZSRP).ToList());
                    }
                    var mapList = _mapper.Map<List<ScientifiCNoProjectItem>, List<ScientifiCNoProject>>(updateOids);
                    await _dbContext.Updateable(mapList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KySecUnit>().WhereColumns(deleteSuData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KyCDUnit>().WhereColumns(deleteCuData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KyNameCeng>().WhereColumns(deleteNcData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KyCanYUnit>().WhereColumns(deleteCyData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KyWeiTUnit>().WhereColumns(deleteWtData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KyPLeader>().WhereColumns(deletePlData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Deleteable<KyCanYDep>().WhereColumns(deleteCdData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateSuItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateCuItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateNCItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateCyItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateWtItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updatePlItem).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateCdItem).ExecuteCommandAsync();
                }
                #endregion

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {

                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> BusinessNoCpportunityDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RelationalContractsDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ManagementOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> LouDongDataAsync(BaseReceiveDataRequestDto<LouDongItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                var louDongList = await _dbContext.Queryable<LouDong>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !louDongList.Select(x => x.ZBLDG).ToList().Contains(x.ZBLDG)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => louDongList.Select(x => x.ZBLDG).ToList().Contains(x.ZBLDG)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    }

                    var mapRmList = _mapper.Map<List<LouDongItem>, List<LouDong>>(insertOids);
                    mapRmList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<LouDong>().BulkCopyAsync(mapRmList);
                }
                if (updateOids.Any())
                {
                    //更新
                    foreach (var itemItem in updateOids)
                    {
                        var id = louDongList.Where(x => x.ZBLDG == itemItem.ZBLDG).Select(x => x.Id).First();
                        itemItem.Id = id;
                    }
                    var mapRmList = _mapper.Map<List<LouDongItem>, List<LouDong>>(updateOids);
                    await _dbContext.Updateable(mapRmList).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RoomNumberDataAsync(BaseReceiveDataRequestDto<RoomNumberItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                var rmList = await _dbContext.Queryable<RoomNumber>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !rmList.Select(x => x.ZROOM).ToList().Contains(x.ZROOM)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => rmList.Select(x => x.ZROOM).ToList().Contains(x.ZROOM)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    }

                    var mapRmList = _mapper.Map<List<RoomNumberItem>, List<RoomNumber>>(insertOids);
                    mapRmList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<RoomNumber>().BulkCopyAsync(mapRmList);
                }
                if (updateOids.Any())
                {
                    //更新
                    foreach (var itemItem in updateOids)
                    {
                        var id = rmList.Where(x => x.ZROOM == itemItem.ZROOM).Select(x => x.Id).First();
                        itemItem.Id = id;
                    }
                    var mapRmList = _mapper.Map<List<RoomNumberItem>, List<RoomNumber>>(updateOids);
                    await _dbContext.Updateable(mapRmList).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AdministrativeDivisionDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> LanguageDataAsync(BaseReceiveDataRequestDto<LanguageItem> baseReceiveDataRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();

            try
            {
                List<LanguageDetails> insertItem = new();
                List<LanguageDetails> updateItem = new();

                var lgList = await _dbContext.Queryable<Language>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => !lgList.Select(x => x.ZLANG_TER).ToList().Contains(x.ZLANG_TER)).ToList();
                var updateOids = baseReceiveDataRequestDto.IT_DATA.item.Where(x => lgList.Select(x => x.ZLANG_TER).ToList().Contains(x.ZLANG_TER)).ToList();

                //新增操作
                if (insertOids.Any())
                {
                    foreach (var ic in insertOids)
                    {
                        ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        foreach (var cc in ic.ZLANG_LIST.Item)
                        {
                            var lg = new LanguageDetails
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                Code = ic.ZLANG_TER,
                                ZVALUE_DESC = cc.ZVALUE_DESC,
                                ZCODE_DESC = cc.ZCODE_DESC,
                                ZLANGCODE = cc.ZLANGCODE,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(lg);
                        }
                    }

                    var mapList = _mapper.Map<List<LanguageItem>, List<Language>>(insertOids);
                    mapList.ForEach(x => x.CreateTime = DateTime.Now);
                    await _dbContext.Fastest<Language>().BulkCopyAsync(mapList);
                }
                if (updateOids.Any())
                {
                    List<LanguageDetails> deleteData = new();
                    //更新
                    var lgeList = await _dbContext.Queryable<LanguageDetails>().ToListAsync();
                    foreach (var itemItem in updateOids)
                    {
                        var id = lgList.Where(x => x.ZLANG_TER == itemItem.ZLANG_TER).Select(x => x.Id).First();
                        itemItem.Id = id;
                        foreach (var items in itemItem.ZLANG_LIST.Item)
                        {
                            LanguageDetails ldetails = new LanguageDetails()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ZCODE_DESC = items.ZCODE_DESC,
                                ZLANGCODE = items.ZLANGCODE,
                                Code = itemItem.ZLANG_TER,
                                ZVALUE_DESC = items.ZVALUE_DESC,
                                CreateTime = DateTime.Now
                            };
                            updateItem.Add(ldetails);
                        }
                        deleteData.AddRange(lgeList.Where(x => x.Code == itemItem.ZLANG_TER).ToList());
                    }
                    var mapList = _mapper.Map<List<LanguageItem>, List<Language>>(updateOids);
                    await _dbContext.Updateable(mapList).ExecuteCommandAsync();
                    await _dbContext.Deleteable<LanguageDetails>().WhereColumns(deleteData, it => new { it.Id }).ExecuteCommandAsync();
                    await _dbContext.Insertable(updateItem).ExecuteCommandAsync();
                }

                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> DeviceDetailCodeDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> AccountingOrganizationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 人员主数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> PersonDataAsync(ReceiveUserRequestDto receiveUserRequestDto)
        {
            var responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.Person,
                RequestParame = receiveUserRequestDto.user.ToJson(),
                ReceiveNumber = 1,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {

                await _dbContext.Insertable(receiveRecordLog).ExecuteCommandAsync();
                //创建用户
                if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "CREATE")
                {
                    var isExistUser = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.EMP_CODE == receiveUserRequestDto.user.EMP_CODE).SingleAsync();
                    if (isExistUser == null)
                    {
                        var user = _mapper.Map<User>(receiveUserRequestDto.user);
                        user.Enable = 1;
                        user.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                        await _dbContext.Insertable<User>(user).ExecuteCommandAsync();

                        responseAjaxResult.Success();
                    }
                    else
                    {
                        if (isExistUser.Enable == 0)
                        {
                            isExistUser.Enable = 1;
                        }
                        await _dbContext.Updateable<User>(isExistUser).ExecuteCommandAsync();
                        responseAjaxResult.UpdateSuccess();
                    }
                }
                //修改用户   禁用  启用用户
                else if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "EDIT"
                    || receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "DISABLE"
                    || receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "ENABLE")
                {

                    var isExistUser = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.EMP_CODE == receiveUserRequestDto.user.EMP_CODE).SingleAsync();
                    if (isExistUser == null)
                    {
                        responseAjaxResult.UserNoExist();
                        return responseAjaxResult;
                    }
                    if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "EDIT")
                    {
                        var user = _mapper.Map<User>(receiveUserRequestDto.user);
                        await _dbContext.Updateable<User>(user).Where(x => x.EMP_CODE == isExistUser.EMP_CODE).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                    //禁用
                    else if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "DISABLE")
                    {
                        isExistUser.Enable = 0;
                    }
                    //启用
                    else if (receiveUserRequestDto.OP_TYPE != null && receiveUserRequestDto.OP_TYPE.ToUpper() == "ENABLE")
                    {
                        isExistUser.Enable = 1;
                    }

                    await _dbContext.Updateable<User>(isExistUser).Where(x => x.EMP_CODE == isExistUser.EMP_CODE).IgnoreColumns(x => x.Id).ExecuteCommandAsync();
                    responseAjaxResult.Success();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> InstitutionDataAsync(ReceiveInstitutionRequestDto receiveInstitutionRequestDto)
        {

            var responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.Institution,
                RequestParame = receiveInstitutionRequestDto.OrganizeItem.ToJson(),
                ReceiveNumber = receiveInstitutionRequestDto.OrganizeItem.Count
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                var institutions = _mapper.Map<List<InstitutionItem>, List<Institution>>(receiveInstitutionRequestDto.OrganizeItem);
                foreach (var item in institutions)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                var institutiontOids = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).Select(x => x.OID).ToListAsync();
                var insertOids = institutions.Where(x => !institutiontOids.Contains(x.OID)).Select(x => x.OID).ToList();
                var updateOids = institutions.Where(x => institutiontOids.Contains(x.OID)).Select(x => x.OID).ToList();
                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = institutions.Where(x => insertOids.Contains(x.OID)).ToList();
                    await _dbContext.Fastest<Institution>().BulkCopyAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = institutions.Where(x => updateOids.Contains(x.OID)).ToList();
                    await _dbContext.Fastest<Institution>().BulkUpdateAsync(batchData);
                }
                responseAjaxResult.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseAjaxResult;
        }
    }
}
