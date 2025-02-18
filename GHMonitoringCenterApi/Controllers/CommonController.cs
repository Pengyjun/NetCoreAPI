using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ApprovalUser;
using GHMonitoringCenterApi.Application.Contracts.Dto.AttributeLabelc;
using GHMonitoringCenterApi.Application.Contracts.Dto.Common;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionQualification;
using GHMonitoringCenterApi.Application.Contracts.Dto.Currency;
using GHMonitoringCenterApi.Application.Contracts.Dto.DictionaryTable;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.Institution;
using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectArea;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload;
using GHMonitoringCenterApi.Application.Contracts.Dto.Province;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.Subsidiary;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace GHMonitoringCenterApi.Controllers
{

    /// <summary>
    ///公共控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("公共相关控制器")]
    [Authorize]
    public class CommonController : BaseController
    {
        #region 依赖注入

        private IBaseService baseService { get; set; }

        public CommonController(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        #endregion

        /// <summary>
        /// 获取所属公司下拉列表接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchCompanyPullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyPullDownAsync([FromQuery] BaseKeyWordsRequestDto baseKeyWordsRequestDto)
        {
            return await baseService.SearchCompanyPullDownAsync(CurrentUser.CurrentLoginInstitutionOid, baseKeyWordsRequestDto.KeyWords);
        }

        /// <summary>
        /// 获取下级公司下拉列表接口
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchCompanyPullPullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyPullDownAsync([FromQuery] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await baseService.SearchCompanySubPullDownAsync(basePrimaryRequestDto.Id);
        }
        /// <summary>
        /// 获取项目状态下拉列表接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchProjectStatusPullPullDown")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectStatusPullDownAsync()
        {
            return await baseService.SearchProjectStatusPullDownAsync();
        }

        /// <summary>
        /// 获取机构组织树 懒加载模式
        /// </summary>
        /// <param name="institutionTreeRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchInstitutionTree")]
        public async Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchInstitutionTreeAsync([FromQuery] InstitutionTreeRequestDto institutionTreeRequestDto)
        {
            return await baseService.SearchInstitutionTreeAsync(string.IsNullOrWhiteSpace(institutionTreeRequestDto.Poid) == true ? CurrentUser.CurrentLoginInstitutionPoid : institutionTreeRequestDto.Poid, "101114066");
        }
        /// <summary>
        /// 获取机构组织树 非懒加载模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchInstitutionNoLazyLoadingLaTree")]
        public async Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchInstitutionTreeAsync()
        {
            return await baseService.SearchInstitutionNoLazyLoadingTreeAsync("101114066");
        }

        /// <summary>
        /// 获取机构组织树  根据不同公司获取不通的树
        /// </summary>
        /// <param name="institutionTreeRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchAllInstitutionTree")]

        public async Task<ResponseAjaxResult<List<InstitutionTreeResponseDto>>> SearchAllInstitutionTreeAsync()
        {
            if (CurrentUser.CurrentLoginInstitutionPoid == "101114066")
            {
                return await baseService.SearchInstitutionNoLazyLoadingTreeAsync(CurrentUser.CurrentLoginInstitutionPoid);
            }
            else
            {
                //var a = CurrentUser;
                return await baseService.SearchInstitutionNoLazyLoadingTreeAsync(CurrentUser.CurrentLoginInstitutionOid);
            }

        }

        /// <summary>
        /// 获取项目类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("PorjectType")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectTypePullDownAsync([FromQuery] ProjectTypRequsetDto projectTypRequsetDto)
        {
            return await baseService.SearchProjectTypePullDownAsync(projectTypRequsetDto);
        }

        /// <summary>
        /// 获取项目管理类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchManagerType")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchManagerTypeAsync([FromQuery] ProjectTypRequsetDto projectTypRequsetDto)
        {
            return await baseService.SearchManagerTypeAsync(projectTypRequsetDto);
        }
        /// <summary>
        /// 获取项目区域
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectArea")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectAreaPullDownAsync([FromQuery] ProjectAreaRequsetDto projectAreaRequsetDto)
        {
            return await baseService.SearchProjectAreaPullDownAsync(projectAreaRequsetDto);
        }
        /// <summary>
        /// 获取字典表数据类型  根据不同参数获取
        /// 1项目干系人员职位类型  2项目干系单位类型   3船舶类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("DictionaryTable")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchDictionaryTablePullDownAsync([FromQuery] DictionaryTableRequestDto baseDictionaryTableRequestDto)
        {
            return await baseService.SearchDictionaryTableTreeAsyncc(baseDictionaryTableRequestDto);
        }
        /// <summary>
        /// 获取项目省份
        /// </summary>
        /// <returns></returns>
        [HttpGet("Porvnice")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProvniceTypePullDownAsync([FromQuery] ProvinceRequsetDto baseprovinceRequestDto)
        {
            return await baseService.SearchProjectProvincePullDownAsync(baseprovinceRequestDto);
        }
        /// <summary>
        /// 项目规模
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectScale")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProvniceTypePullDownAsync()
        {
            return await baseService.SearchProjectScalePullDownAsync();
        }
        /// <summary>
        /// 项目施工资质
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConstructionQualification")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchConstructionQualificationPullDownAsync([FromQuery] ConstructionQualificationRequsetDto qualificationRequsetDto)
        {
            return await baseService.SearchConstructionQualificationPullDownAsync(qualificationRequsetDto);
        }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        /// <returns></returns>
        [HttpGet("IndustryClassification")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchIndustryClassificationPullDownAsync([FromQuery] IndustryClassificationRequsetDto currencyResponseDto)
        {
            return await baseService.SearchIndustryClassificationPullDownAsync(currencyResponseDto);
        }
        /// <summary>
        /// 水运工况级别
        /// </summary>
        /// <returns></returns>
        [HttpGet("WaterCarriage")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchWaterCarriagePullDownAsync()
        {
            return await baseService.SearchWaterCarriagePullDownAsync();
        }
        /// <summary>
        /// 属性标签
        /// </summary>
        /// <returns></returns>
        [HttpGet("AttributeLabel")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchAttributeLabelcPullDownAsync([FromQuery] AttributeLabelcResqusetDto attributeLabelcResqusetDto)
        {
            return await baseService.SearchAttributeLabelcPullDownAsync(attributeLabelcResqusetDto);
        }
        /// <summary>
        /// 项目部 暂时不用
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectDepartment")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectDepartmentPullDownAsync([FromQuery] ProjectDepartmentRuqusetDto projectDepartmentRuqusetDto)
        {
            return await baseService.SearchProjectDepartmentPullDownAsync(projectDepartmentRuqusetDto);
        }

        /// <summary>
        /// 获取职位类型人员信息
        /// </summary>
        /// <param name="positionUserRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchPositionTypeUser")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchPositionTypeUserDownAsync([FromQuery] PositionUserRequestDto positionUserRequestDto)
        {
            return await baseService.SearchPositionTypeUserDownAsync(positionUserRequestDto);
        }

        /// <summary>
        /// 根据名称或手机号获取人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUser")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchrDownAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            return await baseService.SearchUserDownAsync(baseRequestDto);
        }

        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        [HttpGet("Currency")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCurrencyPullDownAsync()
        {
            return await baseService.SearchCurrencyPullDownAsync();
        }

        /// <summary>
        /// 获取行业分类标准下拉树
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchClassTree")]
        public async Task<ResponseAjaxResult<List<ClassifyStandardResponseDto>>> SerchClassifyTreeListAsync()
        {
            return await baseService.SerchClassifyTreeListAsync();
        }

        /// <summary>
        /// 获取往来单位名称
        /// </summary>
        /// <returns></returns>
        [HttpGet("DealingUnit")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchDealingUnitPullDownAsync([FromQuery] BaseRequestDto dealingUnitRequseDto)
        {
            return await baseService.SearchDealingUnitPullDownAsync(dealingUnitRequseDto);
        }
        /// <summary>
        /// 获取当前公司的所有子公司 暂时不用
        /// </summary>
        /// <returns></returns>
        [HttpGet("subsidiary")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseAjaxResult<List<SubordinateCompaniesDto>>> SearchSubsidiaryPullDownAsync([FromQuery] SubsidiaryRequsetDto subsidiaryRequsetDto)
        {
            return await baseService.SearchSubsidiaryPullDownAsync(subsidiaryRequsetDto);
        }

        /// <summary>
        /// 境外区域地点模糊查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchOverAreaRegion")]
        public async Task<ResponseAjaxResult<List<ProjectAreaRegionResponseDto>>> SearchOverAreaRegionAsync([FromQuery] BaseKeyWordsRequestDto requestDto)
        {
            return await baseService.SearchOverAreaRegionAsync(requestDto);
        }

        /// <summary>
        /// 自有分包船舶查询（自有船舶 （type==1），分包船舶+往来单位（type=2）,分包船舶（type=3））
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchOwnerSubShip")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchOwnerSubShipAsync([FromQuery] ShipRequestDto requestDto)
        {
            return await baseService.SearchOwnerSubShipAsync(requestDto);
        }

        /// <summary>
        /// 项目列表导出字段查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchProjectImportColumns")]
        public ResponseAjaxResult<List<BasePullDownResponseDto>> SearchProjectImportColumns()
        {
            return baseService.SearchProjectImportColumns();
        }
        /// <summary>
        /// 获取首页菜单表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchHomeMenu")]
        public async Task<ResponseAjaxResult<SearchHomePageMenuResponseDto>> SearchHomeMenuAsync([FromQuery] SearchHomeMenuRequestDto searchHomeMenuRequestDto)
        {
            return await baseService.SearchHomeMenuAsync(searchHomeMenuRequestDto.Id);
        }
        /// <summary>
        /// 获取审批用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchApprovalUser")]
        public async Task<ResponseAjaxResult<List<ApprovalUser>>> SearchApprovalUserAsync([FromQuery] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await baseService.SearchApprovalUserAsync(CurrentUser.CurrentLoginDepartmentId, basePrimaryRequestDto.Id);
        }

        /// <summary>
        ///  获取未填报日报 、 安监 、船舶 下拉集合
        /// </summary>
        /// <param name="requestDto">type=1 项目日报 type=2安监日报 type=3船舶日报</param>
        /// <returns></returns>
        [HttpGet("GetSearchNotFillRep")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetNotFillDayRepsSearch([FromQuery] BaseKeyWordsRequestDto requestDto)
        {
            return await baseService.GetNotFillDayRepsSearch(CurrentUser, requestDto);
        }
        /// <summary>
        /// 获取船舶名称
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchShipPingName")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipPingNameAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            return await baseService.SearchShipPingNameAsync(baseRequestDto);
        }
        /// <summary>
        /// 获取所属公司下拉接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchCompany")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyAsync([FromQuery] BaseKeyWordsRequestDto baseKeyWordsRequestDto)
        {
            return await baseService.SearchCompanyAsync(baseKeyWordsRequestDto, "101162350");
        }
        /// <summary>
        /// 获取部门下项目信息下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectInformation")]
        public async Task<ResponseAjaxResult<List<ProjectInformationResponseDto>>> SearchProjectInformationAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            return await baseService.SearchProjectInformationAsync(baseRequestDto);
        }
        /// <summary>
        /// 获取港口数据 keywords：船名称模糊搜索 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchShipPort")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetSearchShipPortAsync([FromQuery] BaseRequestDto requestDto)
        {
            return await baseService.GetSearchShipPortAsync(requestDto);
        }
        /// <summary>
        /// 获取施工性质模糊搜索 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetConstructionNature")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetConstructionNatureAsync([FromQuery] BaseRequestDto requestDto)
        {
            return await baseService.GetConstructionNatureAsync(requestDto);
        }
        /// <summary>
        /// 产值日报自有分包船舶查询（自有船舶 （type==1），分包船舶+往来单位（type=2）,分包船舶（type=3）,分包-自有船舶（type=4））
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchLogOwnerSubShip")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchLogOwnerSubShipAsync([FromQuery] ShipRequestDto requestDto)
        {
            return await baseService.SearchLogOwnerSubShipAsync(requestDto);
        }

        /// <summary>
        /// 疏浚吹填分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipWorkTypePullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipWorkTypePullDownAsync()
        {
            return await baseService.SearchShipWorkTypePullDownAsync();
        }

        /// <summary>
        /// 工艺方式
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipWorkModePullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipWorkModePullDownAsync()
        {
            return await baseService.SearchShipWorkModePullDownAsync();
        }

        /// <summary>
        /// 疏浚土分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchSoilPullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchSoilPullDownAsync()
        {
            return await baseService.SearchSoilPullDownAsync();
        }

        /// <summary>
        /// 疏浚岩土分级
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchSoilGradePullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchSoilGradePullDownAsync()
        {
            return await baseService.SearchSoilGradePullDownAsync();
        }

        /// <summary>
        /// 船级社
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipClassicPullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipClassicPullDownAsync()
        {
            return await baseService.SearchShipClassicPullDownAsync();
        }

        /// <summary>
        /// 船舶类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipTypePullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipTypePullDownAsync()
        {
            return await baseService.SearchShipTypePullDownAsync();
        }

        /// <summary>
        /// 船舶状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipStatusPullDown")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchShipStatusPullDownAsync()
        {
            return await baseService.SearchShipStatusPullDownAsync();
        }

        /// <summary>
        /// 获取项目组织结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("InsertProjectWBS")]
        public async Task<ResponseAjaxResult<List<ProjectWBSUpload>>> InsertProjectWBSAsync([FromQuery] ProjectWBSUploadRequestDto projectWBSUploadRequestDto)
        {
            return await baseService.InsertProjectWBSAsync(projectWBSUploadRequestDto);
        }
        /// <summary>
        /// 获取预览和下载网址
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchWebsite")]
        public async Task<ResponseAjaxResult<SearchWebsiteResponseDto>> SearchWebsiteAsync()
        {
            return await baseService.SearchWebsiteAsync(this.HttpContext.Request.Host.Value);
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeviceInformation")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetDeviceInformationAsync([FromQuery] GetDeviceInformationRequesDto getDeviceInformationResponseDto)
        {
            return await baseService.GetDeviceInformationAsync(getDeviceInformationResponseDto);
           
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUser")]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchUserInformationAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            return await baseService.SearchUserInformationAsync(baseRequestDto);
        }
        /// <summary>
        /// 获取首页菜单显示权限用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHomeMenuPermissionUser")]
        public async Task<ResponseAjaxResult<List<InformationResponseDto>>> GetHomeMenuPermissionUserAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            return await baseService.GetHomeMenuPermissionUserAsync(baseRequestDto);
        }


        [HttpPost("SearchProject")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchProjectAsync(BaseRequestDto baseRequestDto)
        {
            return await baseService.SearchProjectAsync(baseRequestDto);
        }
        /// <summary>
        /// 记录节假日
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("RecordHoliday")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> RecordHolidayAsync(int year)
        {
            return await baseService.RecordHolidayAsync(year);
        }

        [HttpGet("DayReportApprovePush")]
        [AllowAnonymous]
        public async Task<bool> DayReportApprovePushAsync()
        {
            return await baseService.DayReportApprovePushAsync();
        }
        /// <summary>
        /// 生产日报审核
        /// </summary>
        /// <returns></returns>
        [HttpGet("DayReportApprove")]
        public async Task<ResponseAjaxResult<bool>> DayReportApproveAsync()
        {
            return await baseService.DayReportApproveAsync();
        }

        /// <summary>
        /// 生产日报审核查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchDayReportApprove")]
        public async Task<string> SearchDayReportApproveAsync()
        {
            return await baseService.SearchDayReportApproveAsync();
        }
    }
}
