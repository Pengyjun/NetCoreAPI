using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using SqlSugar;

namespace HNKC.CrewManagePlatform.Services.Interface.Contract
{
    /// <summary>
    /// 合同实现层
    /// </summary>
    public class ContractService : IContractService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ContractService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 合同列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        //public async Task<ResponsePageResult<List<ContractSearch>>> SearchContractAsync(ContractRequest requestBody)
        //{
        //    ResponsePageResult<List<ContractSearch>> rt = new();
        //    RefAsync<int> total = 0;

        //    //var rr=await _dbContext.Queryable<User>()
        //    //    .LeftJoin<>

        //    return rt.SuccessPageResult(new List<ContractSearch>(), requestBody.PageIndex, requestBody.PageSize, total);
        //}
    }
}
