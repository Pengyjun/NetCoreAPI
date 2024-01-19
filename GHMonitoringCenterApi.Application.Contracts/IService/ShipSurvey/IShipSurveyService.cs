using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipSurvey;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ShipSurvey
{
    public interface IShipSurveyService
    {
        /// <summary>
        /// 搜索船舶检验列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipSurveyResponseDto>>> SearchShipSurveyAsync(BaseRequestDto baseRequestDto);

        /// <summary>
        /// 修改船舶检验列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ModifyShipSurveyAsync(SaveShipSurveyRequestDto saveShipSurveyRequestDto);

    }
}
