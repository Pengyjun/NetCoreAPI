using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.RepairParts
{


    /// <summary>
    /// 修理备件管理接口服务层
    /// </summary>
    public interface IRepairPartsService
    {
        /// <summary>
        ///  获取发船备件清单列表
        /// </summary>
        /// <param name="sendShipSparePartListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SendShipSparePartListResponseDto>>> GetSendShipSparePartListAsync(SendShipSparePartListRequestDto sendShipSparePartListRequestDto);
        /// <summary>
        /// 保存发船备件清单列表
        /// </summary>
        /// <param name="saveSendShipSparePartListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSendShipSparePartListAsync( SaveSendShipSparePartListRequestDto saveSendShipSparePartListRequestDto);
        /// <summary>
        /// 删除发船备件清单列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteSendShipSparePartListAsync(BasePrimaryRequestDto basePrimaryRequestDto);
        /// <summary>
        /// 获取备件仓储运输清单
        /// </summary>
        /// <param name="getSparePartStorageListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<GetSparePartStorageListResponseDto>>> GetSparePartStorageListAsync(GetSparePartStorageListRequestDto getSparePartStorageListRequestDto);
        /// <summary>
        /// 保存备件仓储运输清单
        /// </summary>
        /// <param name="saveSparePartStorageListResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSparePartStorageListAsync(SaveSparePartStorageListResponseDto saveSparePartStorageListResponseDto);
        /// <summary>
        /// 删除备件仓储运输清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteSparePartStorageListAsync(BasePrimaryRequestDto basePrimaryRequestDto);
        /// <summary>
        /// 获取修理项目清单
        /// </summary>
        /// <param name="getSparePartStorageListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<GetRepairItemsListResponseDto>>> GetRepairItemsListAsync(GetSparePartStorageListRequestDto getSparePartStorageListRequestDto);
        /// <summary>
        /// 保存修理项目清单
        /// </summary>
        /// <param name="saveRepairItemsListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveRepairItemsListAsync( SaveRepairItemsListRequestDto saveRepairItemsListRequestDto);
        /// <summary>
        /// 删除修理项目清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteRepairItemsListAsync(BasePrimaryRequestDto basePrimaryRequestDto);
        /// <summary>
        /// 获取备件项目清单
        /// </summary>
        /// <param name="getSparePartStorageListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SparePartProjectListResponseDto>>> GetSparePartProjectListAsync(GetSparePartStorageListRequestDto getSparePartStorageListRequestDto);
        /// <summary>
        /// 保存备件项目清单
        /// </summary>
        /// <param name="saveSparePartProjectListRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSparePartProjectListAsync(SaveSparePartProjectListRequestDto saveSparePartProjectListRequestDto);
        /// <summary>
        /// 删除备件项目清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteSparePartProjectListAsync(BasePrimaryRequestDto basePrimaryRequestDto);

        /// <summary>
        /// 新增修理备件
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddRepairPartsAsync(List<RepairProjectList> models);
        /// <summary>
        /// 新增备件项目清单
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddSparePartProjectAsync(List<SparePartProjectList> models);
        /// <summary>
        /// 新增发船备件清单
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddSendShipSparePartAsync(List<SendShipSparePartList> models);
        /// <summary>
        /// 新增发船备件清单
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddSparePartStoragePartAsync(List<SparePartStorageList> models);


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<object>> ImportRepairPartsStreamAsync();

        /// <summary>
        ///导出自动统计Excel
        /// </summary>
        /// <returns></returns>
        Task<byte[]> ExportExcelAutomaticPartsAsync();
    }
}
