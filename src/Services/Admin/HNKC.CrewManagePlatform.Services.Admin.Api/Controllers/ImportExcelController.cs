using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Services.Interface;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using HNKC.CrewManagePlatform.Web.ActionResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// excel相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImportExcelController : BaseController
    {

        #region 依赖注入
        public IBaseService baseService { get; set; }

        public ImportExcelController(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        #endregion


        /// <summary>
        /// 工资单上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("SalaryUpload")]
        [Transactional]
        public async Task<Result> SalaryUploadAsync(IFormFile file)
        {
            var stream = await SingleFileUpdateAsync(file);
            if (stream == null||stream?.Length == 0)
            {
                return Result.Fail("请先上传文件");
            }
            return await baseService.ReadExcelAsModelAsync(stream);
        }
    }
}
