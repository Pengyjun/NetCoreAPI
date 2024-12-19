using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Services.Interface.Salary;
using HNKC.CrewManagePlatform.Web.ActionResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{


    /// <summary>
    /// 工资控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalaryController : BaseController
    {

        public ISalaryService  salaryService { get; set; }
        public SalaryController(ISalaryService salaryService) 
        {
            this.salaryService = salaryService; 
        }
        /// <summary>
        /// 工资短信推送列表
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        [HttpGet("SearchSalaryList")]
        public async Task<PageResult<SalaryResponse>> SearchSalaryListAsync( [FromQuery]SalaryRequest salaryRequest)
        { 
            return await salaryService.SearchSalaryListAsync(salaryRequest);
        }


        /// <summary>
        /// 工资推送记录
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        [HttpGet("SearchSalaryPushRecord")]
        public async Task<PageResult<SalaryPushResponse>> SearchSalaryPushRecordAsync([FromQuery]SalaryPushRequest salaryPushRequest)
        {
            return await salaryService.SearchSalaryPushRecordAsync(salaryPushRequest);
        }

        /// <summary>
        /// 个人工资推送记录
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        [HttpGet("GetSalaryPushRecordByUser")]
        public async Task<PageResult<SalaryPushResponse>> GetSalaryPushRecordByUserAsync([FromQuery] SalaryPushRequest salaryPushRequest)
        {
            return await salaryService.GetSalaryPushRecordByUserAsync(salaryPushRequest);
        }
    }
}
