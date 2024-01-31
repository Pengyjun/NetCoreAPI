using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ProductionValueImport
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductionValueImportController : ControllerBase
    {

        #region 依赖注入
        public IProductionValueImportService _productionValueImportService { get; set; }
        public ProductionValueImportController(IProductionValueImportService productionValueImportService)
        {
            this._productionValueImportService = productionValueImportService;
        }
        #endregion

        /// <summary>
        /// 导出历史数据产值信息
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ImportProduction")]
        public IActionResult ImportProductionValuesAsync([FromQuery] ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            return File(_productionValueImportService.ImportProductionValuesAsync(importHistoryProductionValuesRequestDto), "application/vnd.ms-excel");
        }
    }
}
