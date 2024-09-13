using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{


    /// <summary>
    /// 文件导出控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileImportController : BaseController
    {
        #region 依赖注入
        private readonly ISearchService   searchService;
        public FileImportController(ISearchService searchService)
        {
           this.searchService = searchService;
        }
        #endregion

        #region 人员数据导出
        [HttpGet("ImportExecl")]
        public async Task<IActionResult> ImportExeclAsync([FromQuery] BaseRequestDto request)
        {
            if (request.IsFullExport)
            {
                request.PageSize = 1000000;
            }
            if (request.ImportType == 1)
            {
                var data = await searchService.GetUserSearchAsync(new UserSearchRequestDto() {  PageSize= request.PageSize });
                return await ExcelImportAsync(data.Data, null, "人员信息");
            }

            return Ok("导出成功");
        }
        #endregion
    }
}
