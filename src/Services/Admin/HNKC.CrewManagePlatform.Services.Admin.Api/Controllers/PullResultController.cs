using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.PullResult;
using HNKC.CrewManagePlatform.Services.Interface.PullResult;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 数据拉取控制器  Restful
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Obsolete]
    public class PullResultController : BaseController
    {
        private IDataDictionaryService _dataDictionaryService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="dataDictionaryService"></param>
        public PullResultController(IDataDictionaryService dataDictionaryService)
        {
            _dataDictionaryService = dataDictionaryService;
        }
        /// <summary>
        /// 获取主数据值域&基础数据写入数据库
        /// </summary>
        /// <param name="pullRequestDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> SaveValueDomainAsync([FromQuery] PullRequestDto pullRequestDto)
        {
            return await _dataDictionaryService.SaveValueDomainAsync(pullRequestDto);
        }
    }
}
