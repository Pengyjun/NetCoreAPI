using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.OtherModels;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Collections.Generic;
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
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.CrossUnit,
                RequestParame = baseReceiveDataRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = baseReceiveDataRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                var projectList = _mapper.Map<List<CorresUnitReceiveDto>, List<CorresUnit>>(baseReceiveDataRequestDto.IT_DATA.item);
                foreach (var item in projectList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                var projectCodeList = await _dbContext.Queryable<CorresUnit>().Where(x => x.IsDelete == 1).Select(x => x.ZBP).ToListAsync();
                var insertOids = projectList.Where(x => !projectCodeList.Contains(x.ZBP)).Select(x => x.ZBP).ToList();
                var updateOids = projectList.Where(x => projectCodeList.Contains(x.ZBP)).Select(x => x.ZBP).ToList();
                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = projectList.Where(x => insertOids.Contains(x.ZBP)).ToList();
                    await _dbContext.Fastest<CorresUnit>().BulkCopyAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = projectList.Where(x => updateOids.Contains(x.ZBP)).ToList();
                    await _dbContext.Fastest<CorresUnit>().BulkUpdateAsync(batchData);
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
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.CountryRegion,
                RequestParame = baseReceiveDataRequest.IT_DATA.item.ToJson(),
                ReceiveNumber = baseReceiveDataRequest.IT_DATA.item.Count
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                List<CountryLanguage> insertzMDGS_OLDNAMEs = new();
                List<CountryLanguage> updatezMDGS_OLDNAMEs = new();
                var countryList = _mapper.Map<List<CountryRegionReceiveDto>, List<CountryRegion>>(baseReceiveDataRequest.IT_DATA.item);
                foreach (var item in countryList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                var CountryRegionList = await _dbContext.Queryable<CountryRegion>().Where(x => x.IsDelete == 1).Select(x => x.ZCOUNTRYCODE).ToListAsync();
                var insertData = countryList.Where(x => !CountryRegionList.Contains(x.ZCOUNTRYCODE)).Select(x => x.ZCOUNTRYCODE).ToList();
                var updateOids = countryList.Where(x => CountryRegionList.Contains(x.ZCOUNTRYCODE)).Select(x => x.ZCOUNTRYCODE).ToList();
                if (insertData.Any())
                {
                    //插入操作
                    var batchData = countryList.Where(x => insertData.Contains(x.ZCOUNTRYCODE)).ToList();
                    foreach (var item in batchData)
                    {
                        foreach (var items in item.ZLANG_LIST)
                        {
                            CountryLanguage projectUsedName = new CountryLanguage()
                            {
                                ZLANGCODE = items.ZLANGCODE,
                                ZCODE_DESC = items.ZCODE_DESC,

                            };
                            insertzMDGS_OLDNAMEs.Add(projectUsedName);
                        }
                    }
                    await _dbContext.Fastest<CountryRegion>().BulkCopyAsync(batchData);
                    await _dbContext.Fastest<CountryRegion>().BulkUpdateAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = countryList.Where(x => updateOids.Contains(x.ZCOUNTRYCODE)).ToList();
                    foreach (var item in batchData)
                    {
                        foreach (var items in item.ZLANG_LIST)
                        {
                            CountryLanguage projectUsedName = new CountryLanguage()
                            {
                                ZLANGCODE = items.ZLANGCODE,
                                ZCODE_DESC = items.ZCODE_DESC,

                            };
                            updatezMDGS_OLDNAMEs.Add(projectUsedName);
                        }
                    }
                    await _dbContext.Fastest<CountryRegion>().BulkUpdateAsync(batchData);
                    await _dbContext.Fastest<CountryLanguage>().BulkUpdateAsync(updatezMDGS_OLDNAMEs);
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
                    //更新曾用名
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
        public async Task<MDMResponseResult> RegionalDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> UnitMeasurementDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> ProjectClassificationDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
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
        public async Task<MDMResponseResult> DeviceClassCodeDataAsync(BaseReceiveDataRequestDto<DeviceClassCodeItem> receiveDataMDMRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.ClassDevice,
                RequestParame = receiveDataMDMRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = receiveDataMDMRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                List<DeviceClassAttribute> insertItem = new();//分类属性
                List<DeviceClassAttribute> updateItem = new();//分类属性
                List<DeviceClassAttributeValue> insertItem2 = new();//分类属性值
                List<DeviceClassAttributeValue> updateItem2 = new();//分类属性值

                var dcCodes = await _dbContext.Queryable<DeviceClassCode>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = receiveDataMDMRequestDto.IT_DATA.item.Where(x => !dcCodes.Select(x => x.ZCLASS).ToList().Contains(x.ZCLASS)).ToList();
                var updateOids = receiveDataMDMRequestDto.IT_DATA.item.Where(x => dcCodes.Select(x => x.ZCLASS).ToList().Contains(x.ZCLASS)).ToList();

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
                                Id = ic.Id.Value,
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
                                Id = ic.Id.Value,
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
        public async Task<MDMResponseResult> RegionalCenterDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> BankCardDataAsync()
        {

            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
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
        public async Task<MDMResponseResult> InvoiceTypeDataAsync(BaseReceiveDataRequestDto<InvoiceTypeItem> receiveDataMDMRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.Invoice,
                RequestParame = receiveDataMDMRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = receiveDataMDMRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                List<InvoiceLanguage> insertItem = new();
                List<InvoiceLanguage> updateItem = new();

                var invoiceCodes = await _dbContext.Queryable<InvoiceType>().Where(x => x.IsDelete == 1).ToListAsync();
                var insertOids = receiveDataMDMRequestDto.IT_DATA.item.Where(x => !invoiceCodes.Select(x => x.ZINVTCODE).ToList().Contains(x.ZINVTCODE)).ToList();
                var updateOids = receiveDataMDMRequestDto.IT_DATA.item.Where(x => invoiceCodes.Select(x => x.ZINVTCODE).ToList().Contains(x.ZINVTCODE)).ToList();

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
                                Id = ic.Id.Value,
                                InvoiceCode = ic.ZINVTCODE,
                                ZCODE_DESC = cc.ZCODE_DESC,
                                ZLANGCODE = cc.ZLANGCODE,
                                CreateTime = DateTime.Now
                            };
                            insertItem.Add(invoiceLanguage);
                        }
                    }

                    var invoiceList = _mapper.Map<List<InvoiceTypeItem>, List<InvoiceType>>(insertOids);
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

            await Console.Out.WriteLineAsync("接收到的数据：" + baseReceiveDataRequestDto.IT_DATA.ToJson());
            var responseAjaxResult = new MDMResponseResult();
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
        public async Task<MDMResponseResult> ProjectDataAsync(BaseReceiveDataRequestDto<ProjectItem> receiveDataMDMRequestDto)
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
                var insertOids = receiveDataMDMRequestDto.IT_DATA.item.Where(x => !projectCodeList.Select(x => x.ZPROJECT).ToList().Contains(x.ZPROJECT)).ToList();
                //需要更新的数据
                var updateOids = receiveDataMDMRequestDto.IT_DATA.item.Where(x => projectCodeList.Select(x => x.ZPROJECT).ToList().Contains(x.ZPROJECT)).ToList();
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
        public async Task<MDMResponseResult> ScientifiCNoProjectDataAsync(BaseReceiveDataRequestDto<ScientifiCNoProjectItem> receiveDataMDMRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.Rcientific,
                RequestParame = receiveDataMDMRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = receiveDataMDMRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                var invoiceList = _mapper.Map<List<ScientifiCNoProjectItem>, List<ScientifiCNoProject>>(receiveDataMDMRequestDto.IT_DATA.item);
                var secUnit = new List<SecUnit>();//二级单位
                var cDUnit = new List<CDUnit>();//承担单位
                var nameCeng = new List<NameCeng>();//曾用名
                var canYUnit = new List<CanYUnit>();//参与单位
                var weiTUnit = new List<WeiTUnit>();//委托单位
                var pLeader = new List<PLeader>();//项目负责人
                var canYDep = new List<CanYDep>();//参与部门

                foreach (var item in invoiceList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    var su = item.IT_AI.Item;
                    var cu = item.IT_AG.Item;
                    var nc = item.IT_ONAME.Item;
                    var cy = item.IT_AH.Item;
                    var wt = item.IT_AK.Item;
                    var pl = item.IT_AJ.Item;
                    var cd = item.IT_DE.Item;
                    foreach (var s in su) { s.Code = item.ZSRP; }
                    foreach (var s in cu) { s.Code = item.ZSRP; }
                    foreach (var s in nc) { s.Code = item.ZSRP; }
                    foreach (var s in cy) { s.Code = item.ZSRP; }
                    foreach (var s in wt) { s.Code = item.ZSRP; }
                    foreach (var s in pl) { s.Code = item.ZSRP; }
                    foreach (var s in cd) { s.Code = item.ZSRP; }
                    secUnit.AddRange(su);
                    cDUnit.AddRange(cu);
                    nameCeng.AddRange(nc);
                    canYUnit.AddRange(cy);
                    weiTUnit.AddRange(wt);
                    pLeader.AddRange(pl);
                    canYDep.AddRange(cd);
                }
                var invoiceCodes = await _dbContext.Queryable<ScientifiCNoProject>().Where(x => x.IsDelete == 1).Select(x => x.ZSRP).ToListAsync();
                var insertOids = invoiceList.Where(x => !invoiceCodes.Contains(x.ZSRP)).Select(x => x.ZSRP).ToList();
                var updateOids = invoiceList.Where(x => invoiceCodes.Contains(x.ZSRP)).Select(x => x.ZSRP).ToList();

                #region 其他
                var suList = _mapper.Map<List<SecUnit>, List<KySecUnit>>(secUnit);
                var suCodes = await _dbContext.Queryable<KySecUnit>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertISuCodes = secUnit.Where(x => !suCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateISuCodes = secUnit.Where(x => suCodes.Contains(x.Code)).Select(x => x.Code).ToList();

                var cuList = _mapper.Map<List<CDUnit>, List<KyCDUnit>>(cDUnit);
                var cuCodes = await _dbContext.Queryable<KyCDUnit>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertICuCodes = cDUnit.Where(x => !cuCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateICuCodes = cDUnit.Where(x => cuCodes.Contains(x.Code)).Select(x => x.Code).ToList();

                var ncList = _mapper.Map<List<NameCeng>, List<KyNameCeng>>(nameCeng);
                var ncCodes = await _dbContext.Queryable<KyNameCeng>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertINcCodes = nameCeng.Where(x => !ncCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateINcCodes = nameCeng.Where(x => ncCodes.Contains(x.Code)).Select(x => x.Code).ToList();

                var cyList = _mapper.Map<List<CanYUnit>, List<KyCanYUnit>>(canYUnit);
                var cyCodes = await _dbContext.Queryable<KyCanYUnit>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertICyCodes = canYUnit.Where(x => !cyCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateICyCodes = canYUnit.Where(x => cyCodes.Contains(x.Code)).Select(x => x.Code).ToList();

                var wtList = _mapper.Map<List<WeiTUnit>, List<KyWeiTUnit>>(weiTUnit);
                var wtCodes = await _dbContext.Queryable<KyWeiTUnit>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertIWtCodes = weiTUnit.Where(x => !wtCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateIWtCodes = weiTUnit.Where(x => wtCodes.Contains(x.Code)).Select(x => x.Code).ToList();

                var plList = _mapper.Map<List<PLeader>, List<KyPLeader>>(pLeader);
                var plCodes = await _dbContext.Queryable<KyPLeader>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertIPlCodes = pLeader.Where(x => !plCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateIPlCodes = pLeader.Where(x => plCodes.Contains(x.Code)).Select(x => x.Code).ToList();

                var cdList = _mapper.Map<List<CanYDep>, List<KyCanYDep>>(canYDep);
                var cdCodes = await _dbContext.Queryable<KyCanYDep>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertICdCodes = canYDep.Where(x => !cdCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateICdCodes = canYDep.Where(x => cdCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                #endregion

                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = invoiceList.Where(x => insertOids.Contains(x.ZSRP)).ToList();
                    await _dbContext.Fastest<ScientifiCNoProject>().BulkCopyAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = invoiceList.Where(x => updateOids.Contains(x.ZSRP)).ToList();
                    batchData.ForEach(x => x.UpdateTime = DateTime.Now);
                    await _dbContext.Fastest<ScientifiCNoProject>().BulkUpdateAsync(batchData);
                }

                #region 其他
                if (insertISuCodes.Any())
                {

                    //插入操作
                    var batchData = suList.Where(x => insertISuCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KySecUnit>().BulkCopyAsync(batchData);
                }
                if (updateISuCodes.Any())
                {
                    //更新操作
                    var batchData = suList.Where(x => updateISuCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KySecUnit>().BulkUpdateAsync(batchData);
                }
                if (insertICuCodes.Any())
                {
                    //插入操作
                    var batchData = cuList.Where(x => insertICuCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KyCDUnit>().BulkCopyAsync(batchData);
                }
                if (updateICuCodes.Any())
                {
                    //更新操作
                    var batchData = cuList.Where(x => updateICuCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KyCDUnit>().BulkUpdateAsync(batchData);
                }
                if (insertINcCodes.Any())
                {
                    //插入操作
                    var batchData = ncList.Where(x => insertINcCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KyNameCeng>().BulkCopyAsync(batchData);
                }
                if (updateINcCodes.Any())
                {
                    //更新操作
                    var batchData = ncList.Where(x => updateINcCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KyNameCeng>().BulkUpdateAsync(batchData);
                }
                if (insertICyCodes.Any())
                {
                    //插入操作
                    var batchData = cyList.Where(x => insertICyCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KyCanYUnit>().BulkCopyAsync(batchData);
                }
                if (updateICyCodes.Any())
                {
                    //更新操作
                    var batchData = cyList.Where(x => updateICyCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KyCanYUnit>().BulkUpdateAsync(batchData);
                }
                if (insertIWtCodes.Any())
                {
                    //插入操作
                    var batchData = wtList.Where(x => insertIWtCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KyWeiTUnit>().BulkCopyAsync(batchData);
                }
                if (updateIWtCodes.Any())
                {
                    //更新操作
                    var batchData = wtList.Where(x => updateIWtCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KyWeiTUnit>().BulkUpdateAsync(batchData);
                }
                if (insertIPlCodes.Any())
                {
                    //插入操作
                    var batchData = plList.Where(x => insertIPlCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KyPLeader>().BulkCopyAsync(batchData);
                }
                if (updateIPlCodes.Any())
                {
                    //更新操作
                    var batchData = plList.Where(x => updateIPlCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KyPLeader>().BulkUpdateAsync(batchData);
                }
                if (insertICdCodes.Any())
                {
                    //插入操作
                    var batchData = cdList.Where(x => insertICdCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<KyCanYDep>().BulkCopyAsync(batchData);
                }
                if (updateICdCodes.Any())
                {
                    //更新操作
                    var batchData = cdList.Where(x => updateICdCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<KyCanYDep>().BulkUpdateAsync(batchData);
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
        public async Task<MDMResponseResult> LouDongDataAsync()
        {
            ///***
            // * 测试写入数据
            // */
            //var tt = new List<LouDong>();
            //var test = new LouDong()
            //{
            //    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
            //    ZZSERIAL = "1",
            //    ZBLDG = "2",
            //    ZBLDG_NAME = "3",
            //    ZSTATE = "4",
            //    ZFORMATINF = "5",
            //    ZSYSTEM = "6",
            //    ZPROJECT = "7"
            //};
            //tt.Add(test);
            //var x = _dbContext.Storageable(tt).ToStorage();
            //await x.AsInsertable.ExecuteCommandAsync();//不存在插入
            //await x.AsUpdateable.ExecuteCommandAsync();//存在更新
            var responseAjaxResult = new MDMResponseResult();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        public async Task<MDMResponseResult> RoomNumberDataAsync(BaseReceiveDataRequestDto<RoomNumberItem> receiveDataMDMRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.Room,
                RequestParame = receiveDataMDMRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = receiveDataMDMRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                var invoiceList = _mapper.Map<List<RoomNumberItem>, List<RoomNumber>>(receiveDataMDMRequestDto.IT_DATA.item);
                foreach (var item in invoiceList)
                {
                    item.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                }
                var invoiceCodes = await _dbContext.Queryable<RoomNumber>().Where(x => x.IsDelete == 1).Select(x => x.ZROOM).ToListAsync();
                var insertOids = invoiceList.Where(x => !invoiceCodes.Contains(x.ZROOM)).Select(x => x.ZROOM).ToList();
                var updateOids = invoiceList.Where(x => invoiceCodes.Contains(x.ZROOM)).Select(x => x.ZROOM).ToList();

                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = invoiceList.Where(x => insertOids.Contains(x.ZROOM)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<RoomNumber>().BulkCopyAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = invoiceList.Where(x => updateOids.Contains(x.ZROOM)).ToList();
                    await _dbContext.Fastest<RoomNumber>().BulkUpdateAsync(batchData);
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
        public async Task<MDMResponseResult> LanguageDataAsync(BaseReceiveDataRequestDto<LanguageItem> receiveDataMDMRequestDto)
        {
            MDMResponseResult responseAjaxResult = new MDMResponseResult();
            #region 记录日志
            var receiceRecordId = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
            ReceiveRecordLog receiveRecordLog = new ReceiveRecordLog()
            {
                Id = receiceRecordId,
                ReceiveType = ReceiveDataType.Language,
                RequestParame = receiveDataMDMRequestDto.IT_DATA.item.ToJson(),
                ReceiveNumber = receiveDataMDMRequestDto.IT_DATA.item.Count,
            };
            await baseService.ReceiveRecordLogAsync(receiveRecordLog, DataOperationType.Insert);
            #endregion
            try
            {
                var invoiceList = _mapper.Map<List<LanguageItem>, List<Language>>(receiveDataMDMRequestDto.IT_DATA.item);
                var item = new List<LanguageC>();
                foreach (var ic in invoiceList)
                {
                    ic.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                    var val = ic.ZLANG_LIST.Item;
                    foreach (var i in val) { i.Code = ic.ZLANG_TER; }
                    item.AddRange(val);
                }
                var invoiceCodes = await _dbContext.Queryable<Language>().Where(x => x.IsDelete == 1).Select(x => x.ZLANG_TER).ToListAsync();
                var insertOids = invoiceList.Where(x => !invoiceCodes.Contains(x.ZLANG_TER)).Select(x => x.ZLANG_TER).ToList();
                var updateOids = invoiceList.Where(x => invoiceCodes.Contains(x.ZLANG_TER)).Select(x => x.ZLANG_TER).ToList();

                #region  其他
                var laList = _mapper.Map<List<LanguageC>, List<LanguageDetails>>(item);
                var laCodes = await _dbContext.Queryable<LanguageDetails>().Where(t => t.IsDelete == 1).Select(t => t.Code).ToListAsync();
                var insertILaCodes = item.Where(x => !laCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                var updateILaCodes = item.Where(x => laCodes.Contains(x.Code)).Select(x => x.Code).ToList();
                #endregion

                if (insertOids.Any())
                {
                    //插入操作
                    var batchData = invoiceList.Where(x => insertOids.Contains(x.ZLANG_TER)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<Language>().BulkCopyAsync(batchData);
                }
                if (updateOids.Any())
                {
                    //更新操作
                    var batchData = invoiceList.Where(x => updateOids.Contains(x.ZLANG_TER)).ToList();
                    await _dbContext.Fastest<Language>().BulkUpdateAsync(batchData);
                }
                if (insertILaCodes.Any())
                {
                    //插入操作
                    var batchData = laList.Where(x => insertILaCodes.Contains(x.Code)).ToList();
                    foreach (var i in batchData) { i.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(); }
                    await _dbContext.Fastest<LanguageDetails>().BulkCopyAsync(batchData);
                }
                if (updateILaCodes.Any())
                {
                    //更新操作
                    var batchData = laList.Where(x => updateILaCodes.Contains(x.Code)).ToList();
                    await _dbContext.Fastest<LanguageDetails>().BulkUpdateAsync(batchData);
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
