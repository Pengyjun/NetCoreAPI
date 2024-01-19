using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectYearPlan;
using GHMonitoringCenterApi.Application.Service.ShipSurvey;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace GHMonitoringCenterApi.Controllers.ProjectYearPlan
{

    /// <summary>
    /// 项目年初计划控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProjectYearPlanController : BaseController
    {

        #region 依赖注入
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarClient _dbContext;
        public IProjectYearPlanService projectYearPlanService { get; set; }
        public ProjectYearPlanController(IProjectYearPlanService projectYearPlanService, ISqlSugarClient _dbContext) { 
        
        this.projectYearPlanService = projectYearPlanService;
            this._dbContext = _dbContext;   
        }
        #endregion
        #region 获取项目年初计划列表
        /// <summary>
        /// 获取项目年初计划列表
        /// </summary>
        /// <param name="projectYearPlanRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("SearchProjectPlan")]
        public async Task<ResponseAjaxResult<ProjectYearPlanResponseDto>> SearchProjectPlanAsync([FromQuery]ProjectYearPlanRequestDto projectYearPlanRequestDto)
        {
           return await projectYearPlanService.SearchProjectPlanAsync(projectYearPlanRequestDto);
        }
        #endregion

        /// <summary>
        /// 新增年初项目
        /// </summary>
        /// <param name="projectYearPlanRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("InsertProjectPlan")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> InsertProjectPlanAsync([FromBody] InsertProjectYearPlanRequestDto  insertProjectYearPlanRequestDto)
        {
            return await projectYearPlanService.InsertPlanBuildProject(insertProjectYearPlanRequestDto);
        }

        /// <summary>
        /// 获取项目的wbs
        /// </summary>
        /// <param name="projectYearPlanRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("GetProjectPlanWbs")]
        public async Task<ResponseAjaxResult<ProjectPlanWbsResponseDto>> GetProjectPlanWbsAsync([FromQuery] ProjectPlanWbsRequestDto   projectPlanWbsRequestDto)
        {
            return await projectYearPlanService.GetProjectPlanWbs(projectPlanWbsRequestDto);
        }
        /// <summary>
        /// 导出年初项目计划
        /// </summary>
        /// <param name="projectYearPlanRequestDto"></param>
        /// <returns></returns>
        //[HttpGet("ImportProjectPlan")]
        //public async Task<IActionResult> GetProjectPlanWbsAsync([FromQuery] ProjectYearPlanRequestDto projectYearPlanRequestDto)
        //{
        //    if (projectYearPlanRequestDto.IsFullExport == true)
        //    {
        //        projectYearPlanRequestDto.PageIndex = 1;
        //        projectYearPlanRequestDto.PageSize = 1000000;
        //    }
        //    var data = await projectYearPlanService.SearchProjectPlanAsync(projectYearPlanRequestDto);
        //    List<string> ignoreColumns = new List<string>();
            
        //    return await ExcelImportAsync(data.Data.ProjectYearPlanDetails, ignoreColumns, "年初项目计划");
        //}

        [HttpGet("ImportProjectPlan")]
        public async Task<IActionResult> GetProjectPlanWbsAsync([FromQuery] ProjectYearPlanRequestDto projectYearPlanRequestDto)
        {
            var templatePath = $"Template/Excel/AnnualOutputPlan.xlsx"; //$"C:\\Users\\pyej0\\Desktop\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\AnnualOutputPlan.xlsx";//样板位置 
            if (projectYearPlanRequestDto.IsFullExport == true)
            {
                projectYearPlanRequestDto.PageIndex = 1;
                projectYearPlanRequestDto.PageSize = 1000000;
            }
            var data = await projectYearPlanService.SearchProjectPlanAsync(projectYearPlanRequestDto);
            List<string> ignoreColumns = new List<string>();
            var importData = new
            {
                TimeNow = DateTime.Now.Year,
                value = data.Data.ProjectYearPlanDetails
            };

            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.TimeNow}年初项目计划");
            //return await ExcelImportAsync(data.Data.ProjectYearPlanDetails, ignoreColumns, "年初项目计划");
        }

        /// <summary>
        /// 保存项目年初wbs
        /// </summary>
        /// <param name="projectYearPlanRequestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveProjectnWbs")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveProjectnWbsAsync([FromBody] ProjectYearPlanTreeRequestDto  projectYearPlanTreeRequestDto)
        {
            return await projectYearPlanService.SaveProjectPlanWbsAsync(projectYearPlanTreeRequestDto);
        }

        [HttpGet("GetInfo")]
        public async Task<ResponseAjaxResult<FuZhuClass>> GetInfoAsync() {
            ResponseAjaxResult<FuZhuClass> responseAjaxResult = new ResponseAjaxResult<FuZhuClass>();
            var id = CurrentUser.Id;
           var singleUser= await _dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.User>().Where(x => x.IsDelete == 1
            &&x.Id== id
            ).FirstAsync();
            var singleInstitution = await _dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.Institution>().Where(x => x.IsDelete == 1
          && x.PomId== singleUser.DepartmentId
          ).FirstAsync();
            FuZhuClass fuZhuClass = new FuZhuClass() 
            {
              //CompanyId= CurrentUser.CurrentLoginInstitutionId,
              CompanyName= CurrentUser.CurrentLoginInstitutionName,
               //Department
                DepartmentName= singleInstitution?.Name
            };
            responseAjaxResult.Data = fuZhuClass;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        
    }
  public  class FuZhuClass {

        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyName { get; set; }


        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Department { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

    }
}
