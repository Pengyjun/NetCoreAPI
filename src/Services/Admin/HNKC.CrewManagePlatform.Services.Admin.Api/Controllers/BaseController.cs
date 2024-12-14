using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using SqlSugar;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Web;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{

    /// <summary>
    /// 基本控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 全局对象
        /// </summary>
        public GlobalCurrentUser GlobalCurrentUser
        {
            get
            {
                var currentUser = (ICurrentUserService)Request.HttpContext.RequestServices.GetService(typeof(ICurrentUserService));
                if (currentUser != null)
                {
                    return currentUser.CurrentUserAsync().GetAwaiter().GetResult();
                }
                else
                {
                    return new GlobalCurrentUser() { };
                }

            }
        }




        #region 文件上传  在WebHelper 工具类里面也有上传文件的方法和下载文件的方法
        #region 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// <summary>
        /// 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Stream> SingleFileUpdateAsync(IFormFile file)
        {
            var suffixName = Path.GetExtension(file.FileName);
            var fileSize = file.Length;
            var allowFileTypeArray = AppsettingsHelper.GetValue($"UpdateItem:DefaultAllowFileType").Split(",");
            var allowFileSize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:LittleFileSize"));
            if (allowFileTypeArray.SingleOrDefault(x => x == suffixName.Replace(".", "").Trim()) == null)
            {
               // return Result<Stream>().Fail("文件类型不允许");
            }
            if (fileSize > allowFileSize)
            {
                //return Result.Fail("文件大小不允许");
            }
            return file.OpenReadStream();
            //var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
            //var newFileName = Guid.NewGuid();
            //savePath = Path.Combine(savePath, $"{newFileName}{suffixName}");
            //using (var stream = System.IO.File.Create(savePath))
            //{
            //    await file.CopyToAsync(stream);
            //}
            //return Result.Success("文件上传成功"); ;
        }
        #endregion
        #endregion

    }
}
