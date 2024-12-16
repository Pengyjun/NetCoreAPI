using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Services.Interface.Salary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HNKC.CrewManagePlatform.Utils;
using HNKC.CrewManagePlatform.Sms.Interfaces;
using HNKC.CrewManagePlatform.Sms.Model;
using HNKC.CrewManagePlatform.Sms;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Web.ActionResults;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 移动端
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MobileController : BaseController
    {
        public ISalaryService salaryService { get; set; }
     
        public MobileController(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }

        /// <summary>
        /// 短信链接查询
        /// </summary>
        /// <param name="sgin"></param>
        /// <returns></returns>
        [HttpGet("FindUserInfo")]
        public async Task<Result> FindUserInfoAsync([FromQuery]string sign)
        {
            var data =await salaryService.FindUserInfoAsync(sign);
            if (data == null)
            {
                return Result.Fail(message:"连接已失效或数据不存在");
            }
            return Result.Success(data);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        [HttpGet("SmsSend")]
        public async Task<Result> SmsSendAsync([FromQuery] BaseRequest baseRequest)
        {
          return  (await salaryService.SendSmsAllAsync(baseRequest));
        }
    }
}
