using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.EquipmentManagement
{
    /// <summary>
    /// 设备管理接口层
    /// </summary>
    public interface IEquipmentManagementService
    {
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>> SearchEquipmentManagementAsync(SearchEquipmentManagementRequestDto searchEquipmentManagementRequestDto);
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveEquipmentManagementAsync(SaveEquipmentManagementRequestDto saveEquipmentManagementRequestDto);
        /// <summary>
        /// 获取设备信息（导出）
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<DeviceManagementExportResponseDto>> SearchEquipmentManagementExportAsync(SearchEquipmentManagementRequestDto searchEquipmentManagementRequestDto);
        /// <summary>
        /// 水上设备信息导入
        /// </summary>
        /// <param name="exportMarineEquipment"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> EquipmentManagementImport(List<ExportMarineEquipment> exportMarineEquipment);
        /// <summary>
        /// 陆域设备信息导入
        /// </summary>
        /// <param name="exportMarineEquipment"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ExportLandEquipmentImport(List<ExportLandEquipment> exportMarineEquipment);
        /// <summary>
        /// 特种设备信息导入
        /// </summary>
        /// <param name="exportMarineEquipment"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ExportSpecialEquipmentImport(List<ExportSpecialEquipment> exportMarineEquipment);
        /// <summary>
        /// 水上设备月报自动生成
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddMarineEquipmentAsync();
        /// <summary>
        /// 特种设备月报自动生成
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddMSpecialEquipmentAsync();
        /// <summary>
        /// 陆域设备月报自动生成
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddLandEquipmentAsync();
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteMarineEquipmentAsync(BasePullDownResponseDto basePullDownResponseDto);
    }
}
