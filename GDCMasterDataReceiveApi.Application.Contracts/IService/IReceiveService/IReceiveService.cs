﻿using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.POPManagOrg;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService
{
    /// <summary>
    /// 接收主数据推送接口
    /// </summary>
    [DependencyInjection]
    public interface IReceiveService
    {
        /// <summary>
        /// 获取通用字典数据
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> CommonDataAsync(BaseReceiveDataRequestDto<ValueDomainReceiveRequestDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> CorresUnitDataAsync(BaseReceiveDataRequestDto<CorresUnitReceiveDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> EscrowOrganizationDataAsync(BaseReceiveDataRequestDto<EscrowOrganizationItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 商机项目 国家地区区分  142境内，142以为境外
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> BusinessProjectDataAsync(BaseReceiveDataRequestDto<BusinessCpportunityItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> CountryRegionDataAsync(BaseReceiveDataRequestDto<CountryRegionReceiveDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> CountryContinentDataAsync(BaseReceiveDataRequestDto<CountryContinentReceiveDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> RegionalDataAsync(BaseReceiveDataRequestDto<RegionalItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> UnitMeasurementDataAsync(BaseReceiveDataRequestDto<UnitMeasurementItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> ProjectClassificationDataAsync(BaseReceiveDataRequestDto<ProjectClassificationItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> FinancialInstitutionDataAsync(BaseReceiveDataRequestDto<FinancialInstitutionReceiveDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> DeviceClassCodeDataAsync(BaseReceiveDataRequestDto<DeviceClassCodeItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> AccountingDepartmentDataAsync(BaseReceiveDataRequestDto<AccountingDepartmentReceiveDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> RegionalCenterDataAsync(BaseReceiveDataRequestDto<RegionalCenterItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> NationalEconomyDataAsync(BaseReceiveDataRequestDto<NationalEconomyItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> AdministrativeOrganizationDataAsync(BaseReceiveDataRequestDto<AdministrativeOrganizationReceiveRequestDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> InvoiceTypeDataAsync(BaseReceiveDataRequestDto<InvoiceTypeItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> CurrencyDataAsync(BaseReceiveDataRequestDto<CurrencyReceiveDto> baseReceiveDataRequestDto);
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> AdministrativeAccountingMapperDataAsync(BaseReceiveDataRequestDto<AdministrativeAccountingMapperItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> ProjectDataAsync(BaseReceiveDataRequestDto<ProjectItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> ScientifiCNoProjectDataAsync(BaseReceiveDataRequestDto<ScientifiCNoProjectItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> RelationalContractsDataAsync(BaseReceiveDataRequestDto<RelationalContractsItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> ManagementOrganizationDataAsync(BaseReceiveDataRequestDto<POPMangOrgItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> LouDongDataAsync(BaseReceiveDataRequestDto<LouDongItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> RoomNumberDataAsync(BaseReceiveDataRequestDto<RoomNumberItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> AdministrativeDivisionDataAsync(BaseReceiveDataRequestDto<AdministrativeDivisionItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> LanguageDataAsync(BaseReceiveDataRequestDto<LanguageItem> receiveDataMDMRequestDto);
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> DeviceDetailCodeDataAsync(BaseReceiveDataRequestDto<DeviceDetailCodeItem> baseReceiveDataRequestDto);
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> AccountingOrganizationDataAsync(BaseReceiveDataRequestDto<AccountingOrganizationReceiveDto> baseReceive);

        #region 接收主数据人员和机构的数据
        /// <summary>
        /// 人员主数据
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> PersonDataAsync(ReceiveUserRequestDto receiveUserRequestDto);
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        Task<MDMResponseResult> InstitutionDataAsync(ReceiveInstitutionRequestDto receiveInstitutionRequestDto);
        #endregion


        Task<string> TestAsync(BaseReceiveDataRequestDto<ValueDomainReceiveRequestDto> baseReceiveDataRequestDto);
    }
}
