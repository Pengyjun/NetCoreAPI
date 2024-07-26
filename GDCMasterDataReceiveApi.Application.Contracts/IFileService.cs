using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 文件导出接口层
    /// </summary>
    [DependencyInjection]
    public interface IFileService
    {
        /// <summary>
        /// 动态列导出Excel(注意此方式需要落地文件)
        /// </summary>
        /// <param name="saveFileName">保存文件文件名</param>
        /// <param name="ignoreColumns">忽略的列明集合</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        Task<string> ExcelImportSaveAsync<T>(string saveFileName, List<string> ignoreColumns, T data);

        /// <summary>
        /// 动态列导出Excel(注意此方式文件不落地)
        /// </summary>
        /// <param name="ignoreColumns">忽略的列明集合</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        Task<MemoryStream> ExcelImportAsync<T>(List<string> ignoreColumns, string sheetName, T data);





    }
}
