using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 基本接口层 
    /// </summary>
    [DependencyInjection]
    public interface IBaseService
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="sqlParams">查询的参数</param>
        /// <param name="filterConditions">where条件密文</param>
        /// <param name="IsPaging">是否需要分页</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">显示的行数</param>
        /// <returns></returns>
        Task<List<T>> GetSearchListAsync<T>(string tableName, string sqlParams, List<string>? filterConditions, bool IsPaging, int pageIndex, int pageSize) where T : class, new();
        /// <summary>
        /// 默认加载获取条件参数
        /// </summary>
        /// <returns></returns>
        ResponseAjaxResult<List<FilterParams>> GetFilterParams();
        /// <summary>
        /// 获取所有属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetPropertyNames<T>();
    }
}
