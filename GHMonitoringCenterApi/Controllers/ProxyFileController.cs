using GHMonitoringCenterApi.Domain.Shared.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using UtilsSharp;

namespace GHMonitoringCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyFileController : BaseController
    {

        /// <summary>
        /// 图片预览
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("ImagePrview")]
        public async Task<IActionResult> ImagePrviewAsync(string fileName)
        {
            var filePath=AppsettingsHelper.GetValue("UpdateItem:SavePath")+ fileName;
            var extName= Path.GetExtension(fileName);
            var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            if (extName == ".xlsx")
            {
                HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"{fileName}{extName}", System.Text.Encoding.UTF8)}");
                return File(imageFileStream, "application/octet-stream");
            }
            return File(imageFileStream, "image/jpeg");
        }
    }
}
