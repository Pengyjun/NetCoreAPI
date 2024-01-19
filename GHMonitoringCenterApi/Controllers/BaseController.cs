using GHMonitoringCenterApi.Application.Contracts.Dto.Upload;
using GHMonitoringCenterApi.Application.Contracts.IService.File;
using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MiniExcelLibs;
using Newtonsoft.Json.Linq;
using System.Web;
using UtilsSharp;
using HttpStatusCode = GHMonitoringCenterApi.Domain.Shared.Enums.HttpStatusCode;

namespace GHMonitoringCenterApi.Controllers
{
    /// <summary>
    /// 基本控制器
    /// </summary>
    public class BaseController : Controller
    {

        #region 当前用户信息
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public CurrentUser CurrentUser { get; set; } = new (){ };

        #endregion

        #region 文件上传  在WebHelper 工具类里面也有上传文件的方法和下载文件的方法
        #region 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// <summary>
        /// 单个文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseAjaxResult<UploadResponseDto>> SingleFileUpdateAsync(IFormFile file, string defaultAllowFileType = "DefaultAllowFileType")
        {
            ResponseAjaxResult<UploadResponseDto> responseAjaxResult = new ResponseAjaxResult<UploadResponseDto>();
            var suffixName = Path.GetExtension(file.FileName);
            var fileSize = file.Length;
            var allowFileTypeArray = AppsettingsHelper.GetValue($"UpdateItem:{defaultAllowFileType}").Split(",");
            var allowFileSize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:LittleFileSize"));
            if (allowFileTypeArray.SingleOrDefault(x => x == suffixName.Replace(".", "").TrimAll()) == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOADFILETYPE_FAIL, HttpStatusCode.UploadFileTypeNoAllow);
                return responseAjaxResult;
            }
            if (fileSize > allowFileSize)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOADFILESIZE_FAIL, HttpStatusCode.UploadFileSizeNoAllow);
                return responseAjaxResult;
            }
            var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
            var newFileName = Guid.NewGuid();
            savePath = Path.Combine(savePath, $"{newFileName}{suffixName}");
            using (var stream = System.IO.File.Create(savePath))
            {
                await file.CopyToAsync(stream);
            }
            UploadResponseDto uploadResponseDto = new UploadResponseDto()
            {
                Id = newFileName,
                Name = newFileName + suffixName,
                OriginName = file.FileName,
                SuffixName= suffixName
            };
            responseAjaxResult.Data = uploadResponseDto;
            responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_SUCCESS);
            return responseAjaxResult;
        }
        #endregion
        /// <summary>
        /// 批量文件上传
        /// </summary>
        /// <param name="files"></param>
        /// <param name="defaultAllowFileType"></param>
        /// <returns></returns>
        #region 批量文件上传(针对小文件上传大小建议不超过4M)  可以修改此值如果单个文件过大建议使用流式上传
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseAjaxResult<List<UploadResponseDto>>> BatchFileUpdatesAsync(List<IFormFile>  files, string defaultAllowFileType = "DefaultAllowFileType")
        {
            ResponseAjaxResult<List<UploadResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<UploadResponseDto>>();
            if (files.Count>10)
            {
                responseAjaxResult.Fail(ResponseMessage.NOT_MAXIMUM_FILE, HttpStatusCode.MaximumUpload);
                return responseAjaxResult;
            }
            var allowFileTypeArray = AppsettingsHelper.GetValue($"UpdateItem:{defaultAllowFileType}").Split(",");
            //var suffixName = new List<string>();
            bool Determine = false;
            foreach (var item in files)
            {
                var suffixName = Path.GetExtension(item.FileName).Replace(".", "").TrimAll();
                var suffixNames =  allowFileTypeArray.Where(x => x.Contains(suffixName)).FirstOrDefault();
                if (suffixNames == null)
                {
                    Determine = true;
                    break;
                }
            }
            //var suffixName = Path.GetExtension(file.FileName);
            var filesLenght =  files.Select(x => x.Length).ToList();
            var fileSize = filesLenght.Sum();
            
            var allowFileSize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:LittleFileSize"));
            if (Determine)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOADFILETYPE_FAIL, HttpStatusCode.UploadFileTypeNoAllow);
                return responseAjaxResult;
            }
            if (fileSize > allowFileSize)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOADFILESIZE_FAIL, HttpStatusCode.UploadFileSizeNoAllow);
                return responseAjaxResult;
            }
            
            var uploadresponseDto = new List<UploadResponseDto>();
            foreach (var item in files)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var name = Path.GetExtension(item.FileName);
                var newFileName = Guid.NewGuid();
                savePath = Path.Combine(savePath, $"{newFileName}{name}");
                using (var stream = System.IO.File.Create(savePath))
                {
                    await item.CopyToAsync(stream);
                }
                UploadResponseDto uploadResponseDto = new UploadResponseDto()
                {
                    Id = newFileName,
                    Name = newFileName + name,
                    OriginName = item.FileName,
                    SuffixName = name
                };
                uploadresponseDto.Add(uploadResponseDto);
            }
            responseAjaxResult.Data = uploadresponseDto;
            responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_SUCCESS);
            return responseAjaxResult;
        }
        #endregion

        #region 大文件流式上传
        /// <summary>
        /// 大文件上传流式上传(单文件)
        /// </summary>
        /// <param name="defaultSingleFileSize">默认文件大小限制</param>
        /// <param name="defaultAllowFileType">默认文件类型限制</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseAjaxResult<UploadResponseDto>> StreamUpdateFileAsync(string defaultSingleFileSize= "DefaultSingleFileSize", string defaultAllowFileType= "DefaultAllowFileType",string Address = "UpdateItem:SavePath")
        {
            ResponseAjaxResult<UploadResponseDto> responseAjaxResult = new ResponseAjaxResult<UploadResponseDto>();
            var contentType = HttpContext.Request.ContentType;
            //multipart/
            if (string.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) < 0)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_HTTPCONTENTTYPE_FAIL, HttpStatusCode.ContentTypeNoAllow);
                return responseAjaxResult;
            }
            var requestbBoundary = MediaTypeHeaderValue.Parse(Request.ContentType);
            var boundary = HeaderUtilities.RemoveQuotes(requestbBoundary.Boundary).Value;
            using (var body=HttpContext.Request.Body) 
            {
                var reader = new MultipartReader(boundary, body);
                var section = await reader.ReadNextSectionAsync();
                string savePath;
                if (Address == "UpdateItem:SavePath")
                {
                     savePath = AppsettingsHelper.GetValue(Address);
                }
                else
                {
                    savePath = Address;
                }
                var newFileName = Guid.NewGuid();
                //原始文件名
                var originFileName = section.AsFileSection().FileName;
                //后缀名
                var suffixName = Path.GetExtension(originFileName);
                var streamSize = body.Length;
                var allowFileTypeArray = AppsettingsHelper.GetValue($"UpdateItem:{defaultAllowFileType}").Split(",");
                var allowFileSize = int.Parse(AppsettingsHelper.GetValue($"UpdateItem:{defaultSingleFileSize}"));
                if (allowFileTypeArray.SingleOrDefault(x => x == suffixName.Replace(".", "").TrimAll()) == null)
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOADFILETYPE_FAIL, HttpStatusCode.UploadFileTypeNoAllow);
                    return responseAjaxResult;
                }
                if (streamSize > allowFileSize)
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOADFILESIZE_FAIL, HttpStatusCode.UploadFileSizeNoAllow);
                    return responseAjaxResult;
                }
                //var log = Request.HttpContext.RequestServices.GetService<ILogger<BaseController>>();
                while (section != null)
                {
                    var hasContentDispositionHeader =
                        ContentDispositionHeaderValue.TryParse(
                            section.ContentDisposition, out var contentDisposition);
                    if (hasContentDispositionHeader)
                    {
                        using (var memoryStream = new MemoryStream())
                        {

                            await section.Body.CopyToAsync(memoryStream);
                            if (section.Body.Length > 1)
                            {
                               
                                using (var targetStream = System.IO.File.Create(Path.Combine(savePath, $"{newFileName}{suffixName}")))
                                {
                                    await targetStream.WriteAsync(memoryStream.ToArray());
                                }
                               
                            }
                        }
                    }
                    section = await reader.ReadNextSectionAsync();
                }
                UploadResponseDto uploadResponseDto = new UploadResponseDto()
                {
                    Id = newFileName,
                    Name = newFileName + suffixName,
                    OriginName = originFileName,
                    SuffixName = suffixName
                };
                responseAjaxResult.Data = uploadResponseDto;
            }
           
            responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_SUCCESS);
            return responseAjaxResult;
        }
        #endregion
        #endregion

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
        public async Task<IActionResult> ExcelImportAsync<T>(T data,List<string> ignoreColumns,string fileName, string fileSuffixName= "xlsx")
        {
            MemoryStream memoryStream = new MemoryStream();
            var fileService = Request.HttpContext.RequestServices.GetService<IFileService>();
            memoryStream = await fileService.ExcelImportAsync<T>(ignoreColumns,fileName,data);
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.FlushAsync();
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"{fileName}.{fileSuffixName}", System.Text.Encoding.UTF8)}");
            return new FileStreamResult(memoryStream, Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }
        #endregion

        #region 流式下载（excel导出 模版导出）
        /// <summary>
        /// 流式下载（excel导出 模版导出）
        /// </summary>
        /// <typeparam name="object">数据源</typeparam>
        /// <param name="data">数据源</param>
        /// <param name="templatePath">模版路径</param>
        /// <param name="fileName">文件名称 不包含后缀名</param>
        /// <param name="fileSuffixName">文件后缀名 默认xlsx</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ExcelTemplateImportAsync(string templatePath, object data, string fileName, string fileSuffixName = "xlsx")
        {
            MemoryStream memoryStream = new MemoryStream();
            await memoryStream.SaveAsByTemplateAsync(templatePath, data);
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.FlushAsync();
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"{fileName}.{fileSuffixName}", System.Text.Encoding.UTF8)}");
            return new FileStreamResult(memoryStream, Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }
        #endregion


        #region Word 下载
        /// <summary>
        /// Word 下载
        /// </summary>
        /// <typeparam name="object">数据源</typeparam>
        /// <param name="data">数据源</param>
        /// <param name="templatePath">模版路径</param>
        /// <param name="fileName">文件名称 不包含后缀名</param>
        /// <param name="fileSuffixName">文件后缀名 默认xlsx</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> WordTemplateImportAsync(Stream memoryStream ,string fileName, string fileSuffixName = "docx")
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.FlushAsync();
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"{fileName}.{fileSuffixName}", System.Text.Encoding.UTF8)}");
            return new FileStreamResult(memoryStream, Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }
        #endregion


    }
}
