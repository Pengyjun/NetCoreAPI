using GDCMasterDataReceiveApi.Application.Contracts;
using Microsoft.Extensions.Logging;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application
{

    /// <summary>
    /// 文件导出接口层实现层
    /// </summary>
    public class FileService : IFileService
    {

        #region 依赖注入
        public ILogger<FileService> logger { get; set; }
        public ISqlSugarClient dbContext { get; set; }
        public FileService(ILogger<FileService> logger, ISqlSugarClient dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
        #endregion

        #region 动态列导出Excel
        /// <summary>
        /// 动态列导出Excel(注意此方式需要落地文件)
        /// </summary>
        /// <param name="saveFileName">保存文件的文件名(文件名称用GUID命名)</param>
        /// <param name="ignoreColumns">忽略的列明列名集合(注意列名要和您的实体属性名称保持一致)</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public async Task<string> ExcelImportSaveAsync<T>(string saveFileName, List<string> ignoreColumns, T data)
        {
            var filePath = string.Empty;
            try
            {
                filePath = AppsettingsHelper.GetValue("Excel:SavePath") + Path.DirectorySeparatorChar + saveFileName;
                DynamicExcelColumn[] dynamicExcelColumsn = null;
                if (ignoreColumns != null && ignoreColumns.Any())
                {
                    dynamicExcelColumsn = new DynamicExcelColumn[ignoreColumns.Count];
                    for (int i = 0; i < dynamicExcelColumsn.Length; i++)
                    {
                        dynamicExcelColumsn[i] = new DynamicExcelColumn(ignoreColumns[i]) { Ignore = true };
                    }
                }
                var config = new OpenXmlConfiguration
                {
                    DynamicColumns = dynamicExcelColumsn
                };
                MiniExcel.SaveAs(filePath, data, configuration: config, overwriteFile: true);
                filePath = saveFileName;
            }
            catch (Exception ex)
            {
                filePath = string.Empty;
                logger.LogError("落地导出Excel出现错误", ex);
            }
            return filePath;
        }


        #endregion

        #region 动态列导出Excel
        /// <summary>
        /// <summary>
        /// 动态列导出Excel(注意此方式文件不落地)
        /// </summary>
        /// <param name="ignoreColumns">忽略的列明集合</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public async Task<MemoryStream> ExcelImportAsync<T>(List<string> ignoreColumns, string sheetName, T data)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                DynamicExcelColumn[] dynamicExcelColumsn = null;
                if (ignoreColumns != null && ignoreColumns.Any())
                {
                    dynamicExcelColumsn = new DynamicExcelColumn[ignoreColumns.Count];
                    for (int i = 0; i < dynamicExcelColumsn.Length; i++)
                    {
                        dynamicExcelColumsn[i] = new DynamicExcelColumn(ignoreColumns[i]) { Ignore = true };
                    }
                }
                var config = new OpenXmlConfiguration
                {
                    DynamicColumns = dynamicExcelColumsn
                };
                await memoryStream.SaveAsAsync(data, sheetName: sheetName, configuration: config);
            }
            catch (Exception ex)
            {
                logger.LogError("导出Excel出现错误", ex.Message);
            }
            return memoryStream;
        }
        #endregion

    }
}
