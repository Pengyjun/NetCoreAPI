using GDCMasterDataReceiveApi.Application.Contracts.Dto.System;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISystemService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 系统控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : BaseController
    {

        #region 依赖注入
        public ISystemService systemService { get; set; }
        public SystemController(ISystemService systemService)
        {
            this.systemService = systemService;
        }
        #endregion

        /// <summary>
        /// 获取所有接口方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchInterfaceMethods")]
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceMethodsAsync()
        {
            return await systemService.SearchInterfaceMethodsAsync();
        }

        /// <summary>
        /// 获取所有接口方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchInterfaceFields")]
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceFieldsAsync([FromQuery] SystemMethodFieldRequestDto systemMethodFieldRequestDto)
        {
            return await systemService.SearchInterfaceFieldsAsync(systemMethodFieldRequestDto.InterfaceName);
        }
    }
}
