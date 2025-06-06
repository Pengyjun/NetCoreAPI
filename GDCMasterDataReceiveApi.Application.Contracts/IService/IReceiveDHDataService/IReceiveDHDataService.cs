﻿using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveDHDataService
{
    /// <summary>
    /// 接收DH相关数据接口
    /// </summary>
    [DependencyInjection]
    public interface IReceiveDHDataService
    {
        /// <summary>
        /// Dh机构数据写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveOrganzationAsync();
        /// <summary>
        /// DH行政和核算机构映射写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveAdministrativeAsync(DateTime? datetime);
        /// <summary>
        /// DH行政机构(多组织)写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveOrganzationDepAsync();
        /// <summary>
        /// DH核算机构(多组织)写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveAdjustAccountsMultipleOrgAsync();
        /// <summary>
        /// DH核算部门写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReveiveAccountingDeptAsync(DateTime? datetime);
        /// <summary>
        /// DH项目信息写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveProjectsAsync(DateTime? datetime, bool isAllDelete = false);
        /// <summary>
        /// DH虚拟项目写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveVirtualProjectAsync(DateTime? datetime, bool isAllDelete=false);
        /// <summary>
        /// DH商机项目写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveOpportunityAsync(DateTime? datetime, bool isAllDelete = false);
        /// <summary>
        /// DH科研项目写入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveResearchListAsync(DateTime? datetime, bool isAllDelete = false);
        /// <summary>
        /// DH生产经营管理组织
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveGetMdmManagementOrgageListAsync();
        /// <summary>
        /// DH委托关系
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReceiveGetDHMdmMultOrgAgencyRelPageListAsync();
    }
}
