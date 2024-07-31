﻿using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IReceiveService
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
        Task<ResponseAjaxResult<bool>> CommonData();
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CorresUnitData();
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> EscrowOrganizationData();
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BusinessProjectData();
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CountryRegionData();
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CountryContinentData();
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RegionalData();
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UnitMeasurementData();
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ProjectClassificationData();
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> FinancialInstitutionData();
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeviceClassCodeData();
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AccountingDepartmentData();
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RegionalCenterData();
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BankCardData();
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> NationalEconomyData();
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AdministrativeOrganizationData();
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> InvoiceTypeData();
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CurrencyData();
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AdministrativeAccountingMapperData();
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ProjectData();
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ScientifiCNoProjectData();
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BusinessNoCpportunityData();
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RelationalContractsData();
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ManagementOrganizationData();
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> LouDongData();
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RoomNumberData();
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AdministrativeDivisionData();
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> LanguageData();
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeviceDetailCodeData();
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AccountingOrganizationData();
        /// <summary>
        /// 人员主数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PersonData();
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> InstitutionData();
    }
}
