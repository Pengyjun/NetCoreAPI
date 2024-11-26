using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{

    /// <summary>
    /// 数据治理控制器  金融机构  物资明细编码 往来单位数据治理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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
        /// 治理数据  1是金融机构  2是物资明细编码  3 是往来单位数据  4DH生产经营管理组织 5DH核算部门
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GovernanceData")]
        public async Task<bool> GovernanceDataAsync(int type = 1)
        {
            return await governanceDataService.GovernanceDataAsync(type);
        }

        #region 数据资源
        /// <summary>
        /// 获取数据资源列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchMetaData")]
        public async Task<ResponseAjaxResult<List<MetaDataDto>>> SearchMetaDataAsync([FromQuery] BaseRequestDto requestDto)
        {
            return await governanceDataService.SearchMetaDataAsync(requestDto);
        }
        /// <summary>
        /// 获取数据资源所有表(左侧树)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Table")]
        public ResponseAjaxResult<List<Tables>> SearchTables()
        {
            return governanceDataService.SearchTables();
        }
        /// <summary>
        /// 保存资源列表信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveMetaData")]
        public async Task<ResponseAjaxResult<bool>> SaveMetaDataAsync([FromBody] MetaDataRequestDto requestDto)
        {
            return await governanceDataService.SaveMetaDataAsync(requestDto);
        }
        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetColumnsTypes")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<List<ColumnsInfo>>> GetColumnsTypesAsync()
        {
            return await governanceDataService.GetColumnsTypesAsync();
        }

        #endregion

        #region 数据质量

        #endregion

        #region 数据标准
        /// <summary>
        /// 获取值域分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchValueDomainType")]
        public async Task<ResponseAjaxResult<List<ValueDomainTypeResponseDto>>> SearchValueDomainTypeAsync()
        {
            return await governanceDataService.SearchValueDomainTypeAsync();
        }
        #endregion
    }
}
