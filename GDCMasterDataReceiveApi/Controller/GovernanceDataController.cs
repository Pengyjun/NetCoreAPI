using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData;
using GDCMasterDataReceiveApi.Application.Service.GovernanceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Controller
{

    /// <summary>
    /// 数据治理控制器  金融机构  物资明细编码 往来单位数据治理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GovernanceDataController : ControllerBase
    {

        #region 依赖注入
        private readonly IGovernanceDataService governanceDataService;
  

        public GovernanceDataController(IGovernanceDataService GovernanceDataService)
        {
            governanceDataService = GovernanceDataService;
        }
        #endregion
        /// <summary>
        /// 治理数据  1是金融机构  2是物资明细编码  3 是往来单位数据  4DH生产经营管理组织
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GovernanceData")]
        [AllowAnonymous]
        public async Task<bool> GovernanceDataAsync(int type = 1)
        {
            return await governanceDataService.GovernanceDataAsync(type);
        }
    }
}
