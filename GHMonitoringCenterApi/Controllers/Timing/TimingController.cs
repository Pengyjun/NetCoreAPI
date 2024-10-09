using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Timing;
using GHMonitoringCenterApi.Application.Contracts.Dto.User;
using GHMonitoringCenterApi.Application.Contracts.IService.Timing;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.SqlSugarCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SqlSugar;

namespace GHMonitoringCenterApi.Controllers.Timing
{
    /// <summary>
    /// 此控制器是定时来调用
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("定时任务相关控制器")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class TimingController : ControllerBase
    {
        public ITimeService timeService { get; set; }
        public TimingController(ITimeService timeService)
        {
            this.timeService = timeService;
        }

        /// <summary>
        /// 定时拉取相关数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseAjaxResult<bool>> GetTimeingDataListAsync(int parame = 1)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (parame == 1)
                return await timeService.GetServiceResponse<PomCompanyResponseDto>(parame);
            //if (parame == 2)
            //    return await timeService.GetServiceResponse<PomUserResponseDto>(parame);
            //if (parame == 3)
            //    return await timeService.GetServiceResponse<PomInstitutionResponseDto>(parame);
            if (parame == 4)
                return await timeService.GetServiceResponse<PomCurrencyResponseDto>(parame);
            if (parame == 5)
                return await timeService.GetServiceResponse<PomDealingUnitResponseDto>(parame);
            if (parame == 6)
                return await timeService.GetServiceResponse<PomProjectResponseDto>(parame);
            if (parame == 7)
                return await timeService.GetServiceResponse<PomProjectStatusResponseDto>(parame);
            if (parame == 8)
                return await timeService.GetServiceResponse<PomProjectAreaResponseDto>(parame);
            if (parame == 9)
                return await timeService.GetServiceResponse<PomProjectLeaderResponseDto>(parame);
            if (parame == 10)
                return await timeService.GetServiceResponse<PomInstitutionResponseDto>(parame);
            if (parame == 11)
                return await timeService.GetServiceResponse<PomUserResponseDto>(parame);
            if (parame == 12)
                return await timeService.GetServiceResponse<PomProvinceResponseDto>(parame);
            if (parame == 13)
                return await timeService.GetServiceResponse<PomProjectTypeResponseDto>(parame);
            if (parame == 14)
                return await timeService.GetServiceResponse<PomProjectScaleResponseDto>(parame);
            if (parame == 15)
                return await timeService.GetServiceResponse<PomConstructionQualificationResponseDto>(parame);
            if (parame == 16)
                return await timeService.GetServiceResponse<PomIndustryClassificationResponseDto>(parame);
            if (parame == 17)
                return await timeService.GetServiceResponse<PomWaterCarriageResponseDto>(parame);
            if (parame == 18)
                return await timeService.GetServiceResponse<PomProjectDepartmentResponseDto>(parame);
            if (parame == 19)
                return await timeService.GetServiceResponse<PomProjectOrgResponseDto>(parame);
            if (parame == 20)
                return await timeService.GetServiceResponse<PomOwnerShipResponseDto>(parame);
            if (parame == 21)
                return await timeService.GetServiceResponse<PomSubShipResponseDto>(parame);
            if (parame == 22)
                return await timeService.GetServiceResponse<PomProjectMasterDataResponseDto>(parame);
            if (parame == 23)
                return await timeService.GetServiceResponse<ProjectYearPlanDto>(parame);
            if (parame == 24)
                return await timeService.GetServiceResponse<JjtWriteDataRecordResponseDto>(parame);
            if (parame == 25)
                return await timeService.GetServiceResponse<PomPortDataResponseDto>(parame);
            if (parame == 26)
                return await timeService.GetServiceResponse<PomShipPingTypeResponseDto>(parame);
            if (parame == 27)
                return await timeService.GetServiceResponse<PomSoilResponseDto>(parame);
            if (parame == 28)
                return await timeService.GetServiceResponse<PomShipWorkTypeResponseDto>(parame);
            if (parame == 29)
                return await timeService.GetServiceResponse<PomShipWorkModeResponseDto>(parame);
            if (parame == 30)
                return await timeService.GetServiceResponse<PomSoilGradeResponseDto>(parame);
            if (parame == 31)
                return await timeService.GetServiceResponse<PomShipClassicResponseDto>(parame);
            return responseAjaxResult;
        }

        /// <summary>
        /// 定时同步每个公司的每天的完成产值汇总 
        /// </summary>
        /// <returns></returns>
        [HttpGet("SynchronizationDayProduction")]
        public async Task<ResponseAjaxResult<bool>> SynchronizationDayProductionAsync()
        {
          return await timeService.SynchronizationDayProductionAsync();
        }


        /// <summary>
        /// 定时同步往来单位
        /// </summary>
        /// <returns></returns>
        [HttpGet("SynchronizationDealUnit")]
        public async Task<string> SynchronizationDealUnitAsync()
        {
            return await timeService.SynchronizationDealUnitsync();
        }

        /// <summary>
        /// 定时同步往来单位2
        /// </summary>
        /// <returns></returns>
        [HttpGet("SynchronizationDealUnitNew")]
        public async Task<string> SynchronizationDealUnitNewAsync()
        {
            return await timeService.SynchronizationDealUnitNewsync();
        }
    }
}
