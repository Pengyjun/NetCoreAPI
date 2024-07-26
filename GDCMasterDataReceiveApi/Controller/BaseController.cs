using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 基本控制器
    /// </summary>
    public abstract class BaseController : ControllerBase
    {

        /// <summary>
        /// 全局对象
        /// </summary>
        public GlobalCurrentUser GlobalCurrentUser {
            get {
                var currentUser=(ICurrentUser)Request.HttpContext.RequestServices.GetService(typeof(ICurrentUser));
                if (currentUser != null)
                {
                    return currentUser.UserAuthenticatedAsync().GetAwaiter().GetResult();
                }
                else {
                    return new GlobalCurrentUser() { };
                }
                
            }
        }


        #region 流式下载（excel导出 非模版导出）
        /// <summary>
        /// 流式下载（excel导出 非模版导出）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">数据源</param>
        /// <param name="ignoreColumns">需要忽略的列名</param>
        /// <param name="fileName">文件名称 不包含后缀名</param>
        /// <param name="fileSuffixName">文件后缀名 默认xlsx</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ExcelImportAsync<T>(T data, List<string> ignoreColumns, string fileName, string fileSuffixName = "xlsx")
        {
            MemoryStream memoryStream = new MemoryStream();
            var fileService = Request.HttpContext.RequestServices.GetService<IFileService>();
            memoryStream = await fileService.ExcelImportAsync<T>(ignoreColumns, fileName, data);
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.FlushAsync();
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"{fileName}.{fileSuffixName}", System.Text.Encoding.UTF8)}");
            return new FileStreamResult(memoryStream, Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }
        #endregion
    }
}
