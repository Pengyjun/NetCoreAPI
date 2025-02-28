using GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.CompanyProductionValueInfos
{
    /// <summary>
    /// 公司产值信息
    /// </summary>
    public interface ICompanyProductionValueInfoService
    {
        /// <summary>
        /// 获取公司产值信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<CompanyProductionValueInfoResponseDto>>> SearchCompanyProductionValueInfoAsync(CompanyProductionValueInfoRequsetDto requsetDto);

        /// <summary>
        /// 新增或修改公司产值信息
        /// </summary>
        /// <param name="companyProductionValueInfoRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ResponseAjaxResult<bool>> AddORUpdateCompanyProductionValueInfoAsync(AddOrUpdateCompanyProductionValueInfoRequestDto companyProductionValueInfoRequestDto);


        /// <summary>
        /// 删除公司产值信息
        /// </summary>
        /// <param name="deleteCompanyProductionValueInfoRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ResponseAjaxResult<bool>> DelectCompanyProductionValueInfoAsync(DeleteCompanyProductionValueInfoRequestDto deleteCompanyProductionValueInfoRequestDto);

        /// <summary>
        /// 查询公司下拉框
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ResponseAjaxResult<List<ProductionMonitoringOperationDayReport>>> SearchCompanyAsync();

        /// <summary>
        /// 添加或修改公司调整值
        /// </summary>
        /// <param name="addORUpdateCompanyAdjustmentValueRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddORUpdateCompanyAdjustmentValueAsync(AddORUpdateCompanyAdjustmentValueRequestDto  addORUpdateCompanyAdjustmentValueRequestDto);

    }
}

