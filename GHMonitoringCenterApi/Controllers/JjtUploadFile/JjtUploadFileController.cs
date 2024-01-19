using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.File;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Service.File;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.JjtUploadFile
{
	/// <summary>
	/// 上传临时素材
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class JjtUploadFileController : BaseController
	{
		#region 依赖注入
		public IFileService fileService { get; set; }
		public ILogService logService { get; set; }
		private IBaseService baseService { get; set; }

		public JjtUploadFileController(IFileService fileService, ILogService logService, IBaseService baseService)
		{
			this.fileService = fileService;
			this.logService = logService;
			this.baseService = baseService;
		}

		#endregion


		/// <summary>
		/// 交建通上传图片并推送消息
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPost("UploadImage")]
		[AllowAnonymous]
		public async Task<ResponseAjaxResult<bool>> UploadImageJJT(IFormFile formFile)
		{
			return await fileService.UploadImageJJT(formFile);
		}
        /// <summary>
        /// 船舶日报图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>

        [HttpPost("UploadShipImage")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> UploadShipImageJJT(IFormFile formFile)
        {
            return await fileService.UploadShipImageJJT(formFile);
        }

		/// <summary>
		/// 项目生产动态图片
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		[HttpPost("UploadProjectShiftImage")]
		[AllowAnonymous]
		public async Task<ResponseAjaxResult<bool>> UploadProjectShiftImageJJT(IFormFile formFile)
		{
			return await fileService.UploadProjectShiftImageJJT(formFile);
		}

        [HttpGet("UploadProjectShiftText")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> UploadProjectShiftTextAsync([FromQuery] string text)
        {
            return await fileService.UploadProjectShiftTextJJT(text);
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        [HttpGet]
		public async Task<bool> GetImageJJT(string media_id)
		{
			return await fileService.GetImageJJT(media_id);
		}
	}
}
