using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(imageFileStream, "image/jpeg");
        }
    }
}
