using GHMonitoringCenterApi.Application.Contracts.IService.ProjectMasterData;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Shared;
using Model = GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectMasterData;
using SqlSugar;
using GHMonitoringCenterApi.Application.Contracts.Dto;

namespace GHMonitoringCenterApi.Application.Service.ProjectMasterData
{
    /// <summary>
    /// 项目主数据实现层
    /// </summary>
    public class ProjectMasterDataService : IProjectMasterDataService
    {

        #region 依赖注入
        public IBaseRepository<Model.ProjectMasterData> baseProjectMasterDataRepository { get; set; }
        public ProjectMasterDataService(IBaseRepository<Model.ProjectMasterData> baseProjectMasterDataRepository)
        {
            this.baseProjectMasterDataRepository = baseProjectMasterDataRepository;
        }
        #endregion



        public async Task<ResponseAjaxResult<List<SearchProjectMasterDataResponseDto>>> SearchProjectMasterDataAsync(BaseRequestDto baseRequestDto)
        {
            ResponseAjaxResult<List<SearchProjectMasterDataResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SearchProjectMasterDataResponseDto>>();
            RefAsync<int> total = 0;
            var ProjectMasterDataList = await baseProjectMasterDataRepository.AsQueryable()
                .Select(x => new SearchProjectMasterDataResponseDto
                {
                    ProjectName = x.ProjectName,
                    ProjectType = x.ProjectType,
                    ProjectMasterCode = x.ProjectMasterCode,
                    Foreign = x.Foreign,
                    BeforeName = x.BeforeName,
                    ProjectCountry = x.ProjectCountry,
                    ProjectCity = x.ProjectCity,
                    BusinessClassification = x.BusinessClassification
                })
                .ToPageListAsync(baseRequestDto.PageIndex, baseRequestDto.PageSize,total);
            responseAjaxResult.Data = ProjectMasterDataList;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}
