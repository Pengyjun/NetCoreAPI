using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 测试
    /// </summary>
    [DependencyInjection]
    public interface ITestService
    {
        /// <summary>
        /// 测试查询
        /// </summary>
        /// <returns></returns>
        public Task<ResponseAjaxResult<List<DealingUnit>>> SearchDelineTest(BaseRequestDto requestDto);
        /// <summary>
        /// 大数据插入
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddTestAsync();
        /// <summary>
        /// 统计数据库所有表每天的增量数据
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseAjaxResult<bool> GetDailyIncrementalData(string schema, DateTime date);
    }
}
