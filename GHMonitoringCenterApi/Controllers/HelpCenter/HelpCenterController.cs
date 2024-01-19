using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter;
using GHMonitoringCenterApi.Application.Contracts.Dto.Upload;
using GHMonitoringCenterApi.Application.Contracts.IService.HelpCenter;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.HelpCenter
{
    /// <summary>
    /// 帮助中心
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   
    
    public class HelpCenterController : BaseController
    {
        #region 依赖注入
       public IHelpCenterService helpCenter { get; set; }
        public HelpCenterController(IHelpCenterService helpCenter)
        {
            this.helpCenter = helpCenter;
        }
        #endregion
        /// <summary>
        /// 新增或修改帮助中心菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveHelpCenterMenu")]
        public async Task<ResponseAjaxResult<bool>> SaveHelpCenterMenuAsync([FromBody]SaveHelpCenterRequsetDto saveHelpCenterRequsetDto)
        {
            return await helpCenter.SaveHelpCenterMenuAsync(saveHelpCenterRequsetDto);
        }
        /// <summary>
        /// 获取帮助中心菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchHelpCenterMenu")]
        public async Task<ResponseAjaxResult<List<SearchHelpCenterMenuResponseDto>>> SearchHelpCenterMenuAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            return await helpCenter.SearchHelpCenterMenuAsync(baseRequestDto);
        }
        /// <summary>
        /// 获取帮助中心详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchHelpCenterDetails")]
        public async Task<ResponseAjaxResult<SearchHelpCenterDetailsResponseDto>> SearchHelpCenterDetailsAsync([FromQuery] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await helpCenter.SearchHelpCenterDetailsAsync(basePrimaryRequestDto.Id);
        }
        /// <summary>
        /// 删除帮助中心菜单和内容
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeletehHelpCenterMenu")]
        public async Task<ResponseAjaxResult<bool>> DeletehHelpCenterMenuAsync([FromBody] DeletehHelpCenterMenuRequsetDto deletehHelpCenterMenuRequsetDto)
        {
            return await helpCenter.DeletehHelpCenterMenuAsync(deletehHelpCenterMenuRequsetDto);
        }

        /// <summary>
        /// 帮助中心上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost("HelpCenterUploadFile")]
        public async Task<ResponseAjaxResult<UploadResponseDto>> DeletehHelpCenterMenuAsync(IFormFile file)
        {
            return await SingleFileUpdateAsync(file, "HelpCenterUploadFileType");
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("HelpCenterDownloadFile")]
        public async Task<IActionResult> HelpCenterDownloadFileAsync()
        {
            return await helpCenter.HelpCenterDownloadFileAsync();
        }

        /// <summary>
        /// 用户手册
        /// </summary>
        /// <returns></returns>
        //public async Task<ResponseAjaxResult<string>> PostUsersManualAsync()
        //{

        //}
    }
}
