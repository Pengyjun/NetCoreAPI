using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
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
        /// <param name="sql">前端给的sql语句  只拼接到from之前 不包含</param>
        /// <param name="filterParams">相关参数条件、符号</param>
        /// <param name="IsPaging">是否需要分页</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">显示的行数</param>
        /// <returns></returns>
        Task<List<T>> GetSearchListAsync<T>(string tableName, string sql, List<FilterParams> filterParams, bool IsPaging, int pageIndex, int pageSize) where T : class, new();
    }
}
