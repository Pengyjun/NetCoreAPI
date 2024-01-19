using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Application.Contracts.IService.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 人员组织调整
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonnelOrganizeAdjustControllers : BaseController
    {
        #region 依赖注入
        public IPersonnelOrganizeAdjustService personnelOrganizeAdjustService { get; set; }

        public PersonnelOrganizeAdjustControllers(IPersonnelOrganizeAdjustService personnelOrganizeAdjustService)
        {
            this.personnelOrganizeAdjustService = personnelOrganizeAdjustService;
        }
        #endregion
        /// <summary>
        /// 获取已授权人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAddedPersonnel")]
        public async Task<ResponseAjaxResult<List<SearchAddedPersonnelReponseDto>>> SearchAddedPersonnelAsync([FromQuery] SearchAddedPersonnelRequestDto searchAddedPersonnelRequestDto)
        {
            return await personnelOrganizeAdjustService.SearchAddedPersonnelAsync(searchAddedPersonnelRequestDto, CurrentUser.Id, CurrentUser.CurrentLoginInstitutionId);
        }
        /// <summary>
        /// 获取人员授权机构列表
        /// </summary>
        /// <param name="searchAddedPersonnelRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchAuthorizedInstitutions")]
        public async Task<ResponseAjaxResult<List<AuthorizedInstitutionsReponseDto>>> SearchAuthorizedInstitutionsAsync([FromQuery] SearchAuthorizedInstitutionsRequestDto searchAuthorizedInstitutionsRequestDto )
        {
            return await personnelOrganizeAdjustService.SearchAuthorizedInstitutionsAsync(searchAuthorizedInstitutionsRequestDto);
        }
        /// <summary>
        /// 添加授权机构列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpPost("AddAuthorizedInstitutions")]
        public async Task<ResponseAjaxResult<bool>> AddAuthorizedAutAsync([FromBody] AddAuthorizedInstitutionsRequestDto addAuthorizedInstitutionsRequesDto)
        {
            return await personnelOrganizeAdjustService.AddAuthorizedInstitutionsAsync(addAuthorizedInstitutionsRequesDto);
        }
        /// <summary>
        /// 删除人员授权机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteAuthorizedInstitutions")]
        public async Task<ResponseAjaxResult<bool>> DeleteAuthorizedAutAsync([FromBody] DeleteAddedPersonnelRequestDto deleteAddedPersonnelRequestDto)
        {
            return await personnelOrganizeAdjustService.DeleteAuthorizedAutAsync(deleteAddedPersonnelRequestDto);
        }
        /// <summary>
        /// 人员组织调整获取所属公司下拉接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCompanyPullDownAuth")]
        public async Task<ResponseAjaxResult<List<GetCompanyPullDownAuthAsyncResponseDto>>> GetCompanyPullDownAuthAsync([FromQuery] GetCompanyPullDownAuthAsyncRequestDto getCompanyPullDownAuthAsyncRequestDto)
        {
            return await personnelOrganizeAdjustService.GetCompanyPullDownAuthAsync(getCompanyPullDownAuthAsyncRequestDto);
        }

    }
}
