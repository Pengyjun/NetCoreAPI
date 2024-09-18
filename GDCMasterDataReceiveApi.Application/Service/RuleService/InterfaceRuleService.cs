using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IRuleService;
using GDCMasterDataReceiveApi.Domain.Shared;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Service.RuleService
{
    /// <summary>
    /// 接口规则实现
    /// </summary>
    public class InterfaceRuleService : IInterfaceRuleService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public InterfaceRuleService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 获取接口名称
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InterfaceNamesResponseDto>>> GetInterfaceNamesAsync()
        {
            ResponseAjaxResult<List<InterfaceNamesResponseDto>> responseAjaxReslut = new();
            var interfaceNames = new List<InterfaceNamesResponseDto>();
           

            return responseAjaxReslut;
        }

    }
}
