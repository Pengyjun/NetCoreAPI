using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DataAuthority;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 字段授权维护控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DataAuthorityController : BaseController
    {
        private readonly IDataAuthorityService _dataAuthorityService;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="dataAuthorityService"></param>
        public DataAuthorityController(IDataAuthorityService dataAuthorityService)
        {
            this._dataAuthorityService = dataAuthorityService;
        }
        /// <summary>
        /// 新增或修改可授权字段（列表选择字段点击确认后使用）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("AddOrModifyDataAuthory")]
        public async Task<ResponseAjaxResult<bool>> AddOrModifyDataAuthoryAsync([FromBody] AddOrModifyDataAuthorityRequestDto requestDto)
            => await _dataAuthorityService.AddOrModifyDataAuthoryAsync(requestDto.Id, requestDto.Colums, requestDto.UId, requestDto.RId, requestDto.InstutionId, requestDto.DepId, requestDto.PjectId);
        /// <summary>
        /// 获取用户可查看的字段
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetDataAuthority")]
        public async Task<ResponseAjaxResult<DataAuthorityDto>> GetDataAuthorityAsync([FromQuery] DataAuthorityRequestDto requestDto)
            => await _dataAuthorityService.GetDataAuthorityAsync(requestDto.UId, requestDto.RId, requestDto.InstutionId, requestDto.DepId, requestDto.PjectId);
    }
}
