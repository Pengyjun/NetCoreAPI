using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.File;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Service.File;
using GHMonitoringCenterApi.Application.Service.JjtSendMessage;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public ILogger<JjtUploadFileController>  logger { get; set; }
        public IFileService fileService { get; set; }
		public ILogService logService { get; set; }
		private IBaseService baseService { get; set; }

		public JjtUploadFileController(IFileService fileService, ILogService logService, IBaseService baseService, ILogger<JjtUploadFileController> logger)
		{
			this.fileService = fileService;
			this.logService = logService;
			this.baseService = baseService;
			this.logger = logger;
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
		public async Task<ResponseAjaxResult<bool>> UploadImageJJT(IFormFile formFile, [FromQuery] int isSystemSend = 0)
        {
            if (isSystemSend == 1)
            {
                await Console.Out.WriteLineAsync("开始发送");
                return await fileService.UploadImageJJT(formFile);
               
            }
            else
            {
                logger.LogWarning($"人为触发消息推送机制,浏览器版本信息:{Request.Headers["User-Agent"].ToString()}");
                return new ResponseAjaxResult<bool>()
                {
                    Code = GHMonitoringCenterApi.Domain.Shared.Enums.HttpStatusCode.VerifyFail,
                    Message = "发送生产推送日报人为触发",
                };
            }
           
		}


        /// <summary>
		/// 交建通上传图片并推送消息 （交建公司）
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPost("UploadImageJJByJJT")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> UploadImageJJByJJT(IFormFile formFile, [FromQuery] int isSystemSend = 0)
        {
            if (isSystemSend == 1)
            {
                return await fileService.UploadImageJJByJJT(formFile);
            }
            else
            {
                logger.LogWarning($"人为触发消息推送机制,浏览器版本信息:{Request.Headers["User-Agent"].ToString()}");
                return new ResponseAjaxResult<bool>()
                {
                    Code = GHMonitoringCenterApi.Domain.Shared.Enums.HttpStatusCode.Success,
                    Message = "发送生产推送日报人为触发",
                };
            }

        }

        /// <summary>
        /// 船舶日报图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>

        [HttpPost("UploadShipImage")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> UploadShipImageJJT(IFormFile formFile,[FromQuery] int isSystemSend = 0)
        {
            
            if (isSystemSend == 1)
            {
                return await fileService.UploadShipImageJJT(formFile);
            }
            else
            {
                logger.LogWarning($"人为触发消息推送机制,浏览器版本信息:{Request.Headers["User-Agent"].ToString()}");
                return new ResponseAjaxResult<bool>()
                {
                    Code = GHMonitoringCenterApi.Domain.Shared.Enums.HttpStatusCode.Success,
                    Message = "发送船舶动态日报人为触发",
                };
            }
        }

		/// <summary>
		/// 项目生产动态图片
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		[HttpPost("UploadProjectShiftImage")]
		[AllowAnonymous]
		public async Task<ResponseAjaxResult<bool>> UploadProjectShiftImageJJT(IFormFile formFile, [FromQuery] int isSystemSend = 0)
		{
            if (isSystemSend == 1)
            {
                return await fileService.UploadProjectShiftImageJJT(formFile);
            }
            else
            {
                logger.LogWarning($"人为触发消息推送机制,浏览器版本信息:{Request.Headers["User-Agent"].ToString()}");
                return new ResponseAjaxResult<bool>()
                {
                    Code = GHMonitoringCenterApi.Domain.Shared.Enums.HttpStatusCode.Success,
                    Message = "发送节假日图片人为触发",
                };
            }
           
		}

        [HttpGet("UploadProjectShiftText")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> UploadProjectShiftTextAsync([FromQuery] string text, [FromQuery] int isSystemSend = 0)
        {
            if (isSystemSend == 1)
            {
                return await fileService.UploadProjectShiftTextJJT(text);
            }
            else
            {
                logger.LogWarning($"人为触发消息推送机制,浏览器版本信息:{Request.Headers["User-Agent"].ToString()}");
                return new ResponseAjaxResult<bool>()
                {
                    Code = GHMonitoringCenterApi.Domain.Shared.Enums.HttpStatusCode.Success,
                    Message = "发送节假日文本人为触发",
                };
            }
            //return await fileService.UploadProjectShiftTextJJT(text);
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
