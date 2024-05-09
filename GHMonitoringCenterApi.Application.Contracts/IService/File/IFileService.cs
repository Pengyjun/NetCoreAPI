using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.File
{
    /// <summary>
    /// 文件导出接口层
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// 动态列导出Excel(注意此方式需要落地文件)
        /// </summary>
        /// <param name="saveFileName">保存文件文件名</param>
        /// <param name="ignoreColumns">忽略的列明集合</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        Task<string> ExcelImportSaveAsync<T>(string saveFileName, List<string> ignoreColumns,T data);

        /// <summary>
        /// 动态列导出Excel(注意此方式文件不落地)
        /// </summary>
        /// <param name="ignoreColumns">忽略的列明集合</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        Task<MemoryStream> ExcelImportAsync<T>( List<string> ignoreColumns, string sheetName, T data);

        /// <summary>
        /// 导出 html转换文件
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<FileResponseDto>> ExportHtmlToFileAsync(HtmlConvertFileRequestDto model);

        /// <summary>
        /// 交建通上传图片
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UploadImageJJT(IFormFile formFile);
        /// <summary>
        /// 船舶日报图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UploadShipImageJJT(IFormFile formFile);
		/// <summary>
		/// 项目生产动态图片
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		Task<ResponseAjaxResult<bool>> UploadProjectShiftImageJJT(IFormFile formFile);

        Task<ResponseAjaxResult<bool>> UploadProjectShiftTextJJT(string text);

        /// <summary>
        /// 获取交建通上传的图片
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        Task<bool> GetImageJJT(string media_id);

        /// <summary>
        /// 发送消息失败重试机制
        /// </summary>
        /// <param name="_httpclient"></param>
        /// <param name="media"></param>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        Task<string> RetryAsync(HttpClient _httpclient, string media, string url, MultipartFormDataContent formData, int retryCount = 3);

    }
}
