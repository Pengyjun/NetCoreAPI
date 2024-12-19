using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Services.Interface.CrewArchives;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 船员档案控制器 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CrewArchivesController : BaseController
    {
        private ICrewArchivesService _service;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public CrewArchivesController(ICrewArchivesService service)
        {
            this._service = service;
        }
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchCrewArchives")]
        public async Task<Result> SearchCrewArchivesAsync([FromQuery] SearchCrewArchivesRequest requestBody)
        {
            return await _service.SearchCrewArchivesAsync(requestBody);
        }
        /// <summary>
        /// 船员数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("CrewArchivesCount")]
        public async Task<Result> CrewArchivesCountAsync()
        {
            return await _service.CrewArchivesCountAsync();
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveData")]
        public async Task<Result> SaveDataAsync([FromBody] CrewArchivesRequest requestBody)
        {
            return await _service.SaveDataAsync(requestBody);
        }

        #region 下拉列表
        ///// <summary>
        ///// 获取基本下拉列表
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public async Task<Result> DropDownListAsync([FromQuery] int type)
        //{
        //    return await _service.DropDownListAsync(type);
        //}

        #endregion
    }
}
