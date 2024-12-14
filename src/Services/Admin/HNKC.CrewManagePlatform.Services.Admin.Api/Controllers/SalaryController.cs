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
        public async Task<IActionResult> SearchSalaryListAsync( [FromQuery]SalaryRequest salaryRequest)
        { 
           var data= await salaryService.SearchSalaryListAsync(salaryRequest);
            return Ok(data);
        }


        /// <summary>
        /// 工资推送记录
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        [HttpGet("SearchSalaryPushRecord")]
        public async Task<IActionResult> SearchSalaryPushRecordAsync([FromQuery]SalaryPushRequest salaryPushRequest)
        {
            var data=await salaryService.SearchSalaryPushRecordAsync(salaryPushRequest);
            return Ok(data);
        }

        /// <summary>
        /// 个人工资推送记录
        /// </summary>
        /// <param name="salaryRequest"></param>
        /// <returns></returns>
        [HttpGet("GetSalaryPushRecordByUser")]
        public async Task<IActionResult> GetSalaryPushRecordByUserAsync([FromQuery] SalaryPushRequest salaryPushRequest)
        {
            var data= await salaryService.GetSalaryPushRecordByUserAsync(salaryPushRequest);
            return Ok(data);
        }
    }
}
