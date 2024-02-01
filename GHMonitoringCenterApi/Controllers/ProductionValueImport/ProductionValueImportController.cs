using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;

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
        public IActionResult ImportProductionValues([FromQuery] ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {

            //数据读取
            var responseAjaxData = _productionValueImportService.ReadImportProductionData(importHistoryProductionValuesRequestDto);

            var startDate = importHistoryProductionValuesRequestDto.GetStartDate();
            var endDate = importHistoryProductionValuesRequestDto.GetEndDate();

            ConvertHelper.TryConvertDateTimeFromDateDay(startDate, out DateTime startTime);
            ConvertHelper.TryConvertDateTimeFromDateDay(endDate, out DateTime endTime);

            var templeFile = @"E:\project\HNKC.SZGHAPI\GHMonitoringCenterApi.Domain.Shared\Template\Excel\ExcelHistoryProdution.xlsx";
            //区分月份  starttime-endTime 几个月份 几个sheet
            var newDate = DateTime.MinValue;
            int sheetNum = startDate == endDate ? 1 : endTime.Month - startTime.Month + 1;
            NPOI.XSSF.UserModel.XSSFWorkbook book = new NPOI.XSSF.UserModel.XSSFWorkbook(); 
            var sheet = book.CreateSheet();
            for (int i = 0; i < 1; i++)
            {
                newDate = startTime;
                //外层循环控制sheet个数
                //var sheetName = workbook.CreateSheet(newDate.ToString("yyyy年MM月"));
                //当月天数 控制多少张表格
                int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);
                var inDateTime = newDate;
                for (int days = 0; days < daysInMonth; days++)
                {
                    //更新当前日期
                    var inDateDay = inDateTime.ToDateDay();
                    var oneTable = responseAjaxData.ExcelCompanyProjectBasePoductions.Where(x => x.DateDay == inDateDay).ToList();
                    //创建行
                    var rowIndex = 1;
                    foreach (var item in oneTable)
                    {
                        var datarow = sheet.CreateRow(rowIndex);
                        datarow.CreateCell(0).SetCellValue(rowIndex);
                        datarow.CreateCell(1).SetCellValue(item.UnitName);
                        datarow.CreateCell(2).SetCellValue(item.OnContractProjectCount);
                        rowIndex++;
                    }
                    inDateTime = inDateTime.AddDays(1);
                }
                newDate = newDate.AddMonths(1);
            }
            using (var f = System.IO.File.OpenWrite(templeFile))
            {
                book.Write(f);
            }
            using MemoryStream stream = new MemoryStream(); 
            //将Excel 文件作为下载返回给客户端
            var bytes = System.IO.File.ReadAllBytes(templeFile);
            return File(bytes, "application/octet-stream", $"{System.DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        }
    }
}
