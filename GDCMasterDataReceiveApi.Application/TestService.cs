using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestService : ITestService
    {
        /// <summary>
        /// 上下文
        /// </summary>
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="dbContext"></param>
        public TestService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DealingUnit>>> SearchDelineTest(BaseRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DealingUnit>>();
            RefAsync<int> total = 0;
            var data = await _dbContext.Queryable<DealingUnit>()
                .Where(x => x.IsDelete == 1)
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageIndex, total);
            return responseAjaxResult;
        }
    }
}
