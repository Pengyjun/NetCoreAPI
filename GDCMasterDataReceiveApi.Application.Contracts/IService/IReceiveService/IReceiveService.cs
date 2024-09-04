﻿using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
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
        Task<ResponseAjaxResult<bool>> CommonDataAsync();
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CorresUnitDataAsync();
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> EscrowOrganizationDataAsync();
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BusinessProjectDataAsync();
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ResponseResult>> CountryRegionDataAsync(RequestResult<CountryRegionReceiveDto> requestDto);
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CountryContinentDataAsync();
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RegionalDataAsync();
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UnitMeasurementDataAsync();
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ProjectClassificationDataAsync();
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> FinancialInstitutionDataAsync();
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeviceClassCodeDataAsync();
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AccountingDepartmentDataAsync();
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RegionalCenterDataAsync();
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BankCardDataAsync();
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> NationalEconomyDataAsync();
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AdministrativeOrganizationDataAsync();
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> InvoiceTypeDataAsync();
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> CurrencyDataAsync();
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AdministrativeAccountingMapperDataAsync();
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ProjectDataAsync();
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ScientifiCNoProjectDataAsync();
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BusinessNoCpportunityDataAsync();
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RelationalContractsDataAsync();
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ManagementOrganizationDataAsync();
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> LouDongDataAsync();
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RoomNumberDataAsync();
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AdministrativeDivisionDataAsync();
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> LanguageDataAsync();
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeviceDetailCodeDataAsync();
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AccountingOrganizationDataAsync();

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
    }
}
