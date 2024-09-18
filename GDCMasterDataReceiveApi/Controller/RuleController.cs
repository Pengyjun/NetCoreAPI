using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IRuleService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : BaseController
    {
        private readonly IInterfaceRuleService _interfaceRuleService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceRuleService"></param>
        public RuleController(IInterfaceRuleService interfaceRuleService)
        {
            this._interfaceRuleService = interfaceRuleService;
        }
        /// <summary>
        /// 获取接口列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetInterfaceNames")]
        public async Task<ResponseAjaxResult<List<InterfaceNamesResponseDto>>> GetInterfaceNamesAsync()
        {
            return await _interfaceRuleService.GetInterfaceNamesAsync();
        }
    }
}
