using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 人员数据调整接口层
    /// </summary>
    public interface IPersonnelOrganizeAdjustService
    {
        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        public Task<ResponseAjaxResult<List<SearchAddedPersonnelReponseDto>>> SearchAddedPersonnelAsync(SearchAddedPersonnelRequestDto searchAddedPersonnelRequestDto,Guid UserId , Guid CompanyId);
        /// <summary>
        /// 获取人员授权机构列表
        /// </summary>
        /// <returns></returns>
        public Task<ResponseAjaxResult<List<AuthorizedInstitutionsReponseDto>>> SearchAuthorizedInstitutionsAsync(SearchAuthorizedInstitutionsRequestDto searchAuthorizedInstitutionsRequestDto);
        /// <summary>
        /// 添加授权机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseAjaxResult<bool>> AddAuthorizedInstitutionsAsync(AddAuthorizedInstitutionsRequestDto addAuthorizedInstitutionsRequesDto);
        /// <summary>
        /// 删除人员授权机构列表
        /// </summary>
        /// <returns></returns>
        public Task<ResponseAjaxResult<bool>> DeleteAuthorizedAutAsync(DeleteAddedPersonnelRequestDto deleteAddedPersonnelRequestDto);

        /// <summary>
        /// 人员组织调整获取所属公司下拉接口
        /// </summary>
        /// <param name="deleteAddedPersonnelRequestDto"></param>
        /// <returns></returns>
        public Task<ResponseAjaxResult<List<GetCompanyPullDownAuthAsyncResponseDto>>> GetCompanyPullDownAuthAsync(GetCompanyPullDownAuthAsyncRequestDto getCompanyPullDownAuthAsyncRequestDto);
    }
      
}
