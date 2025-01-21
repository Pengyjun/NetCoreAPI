using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.Dto.External;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ConstructionLog
{
    /// <summary>
    /// 施工日志接口层
    /// </summary>
    public interface IConstructionLogService
    {
        /// <summary>
        /// 获取施工日志列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ConstructionLogResponseDto>>> SearchConstructionLogAsync(ConstructionLogRequestDto constructionLogRequestDto, string oid);
        /// <summary>
        /// 获取已填报日志日期
        /// </summary>
        Task<ResponseAjaxResult<List<SearchCompletedConstructionLogRequestDto>>> SearchCompletedConstructionLogAsync(DateTime? dateTime,Guid ProjectId);
        /// <summary>
        /// 获取施工日志详情
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchConstructionLoDetailsgResponseDto>> SearchConstructionLogDetailsgAsync(Guid? Id,int? dateTime);

        /// <summary>
        /// 获取施工日志详情（对外接口使用）
        /// </summary> 
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchConstructionLoDetailsgResponseDto>>> SearchExternalConstructionLogDetailsgAsync(ExternalDateRequestDto requestDto);
    }
}
