using GDCMasterDataReceiveApi.Application.Contracts.Dto.System;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISystemService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        public IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider { get; set; }
        public ISchemaGenerator schemaGenerator { get; set; }
        public ISystemService systemService { get; set; }
        public SystemController(ISystemService systemService, IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider, ISchemaGenerator schemaGenerator)
        {
            this.systemService = systemService;
            this.schemaGenerator = schemaGenerator;
            this.apiDescriptionGroupCollectionProvider = apiDescriptionGroupCollectionProvider;
        }
        #endregion

        #region 
        /// <summary>
        /// 获取所有接口方法 
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchInterfaceMethods")]
        [Obsolete]
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceMethodsAsync([FromQuery] SystemInterfaceRequestDto systemInterfaceRequestDto)
        {
            return await systemService.SearchInterfaceMethodsAsync(systemInterfaceRequestDto.SystemIdentity);
        }
        /// <summary>
        /// 获取所有接口响应字段  暂时不用
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchInterfaceFields")]
        [Obsolete]
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceFieldsAsync([FromQuery] SystemMethodFieldRequestDto systemMethodFieldRequestDto)
        {
            // var a= new OpenApiDocument();
            //new OpenApiComponents().
            //foreach (var item in apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items)
            //{
            //    foreach (var api in item.Items)
            //    {

            //        foreach (var parame in api.ParameterDescriptions)
            //        {
            //           new OpenApiDocument().

            //            schemaGenerator.ge()
            //        }
            //    }
            //}
            return await systemService.SearchInterfaceFieldsAsync(systemMethodFieldRequestDto.InterfaceName);
        }
        #endregion

        #region adyu
        [HttpGet("adyu")]
        [AllowAnonymous]
        public bool AadYu()
        {
            return systemService.AadYu();
        }
        #endregion
    }
}
