using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ProductionValueImport
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductionValueImportController : ControllerBase
    {

        #region 依赖注入
        public IProductionValueImportService productionValueImportService { get; set; }
        public ProductionValueImportController(IProductionValueImportService productionValueImportService)
        {
            this.productionValueImportService = productionValueImportService;
        }
        #endregion

        /// <summary>
        /// 导出历史数据产值信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("ImportHistoryProdutionValue")]
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> ImportHistoryProdutionValueAsync(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto) 
        {
            return await productionValueImportService.ImportProductionValuesAsync(importHistoryProductionValuesRequestDto);
        }
    }
}
