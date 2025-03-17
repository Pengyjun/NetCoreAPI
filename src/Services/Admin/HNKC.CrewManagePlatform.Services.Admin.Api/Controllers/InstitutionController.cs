using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Common;
using HNKC.CrewManagePlatform.Services.Interface;
using HNKC.CrewManagePlatform.Services.Interface.PullResult;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using HNKC.CrewManagePlatform.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstitutionController : BaseController
    {
        private IBaseService baseService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="dataDictionaryService"></param>
        public InstitutionController(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        /// <summary>
        /// 搜索机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchInstitutionTree")]
        public async Task<Result> SearchInstitutionTreeAsync()
        {
            return await baseService.SearchInstitutionTreeAsync();
        }

        /// <summary>
        /// 添加机构树
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost("AddInstitutionTree")]
        [Transactional]
        public async Task<Result> AddInstitutionTreeAsync([FromBody] AddInstutionRequestDto requestDto)
        {
            return await baseService.AddInstitutionTreeAsync(requestDto);
        }

    }
}
