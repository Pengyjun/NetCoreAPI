using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ProductionValueImport
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductionValueImportController : BaseController
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
        //[HttpGet("ImportProduction")]
        //public IActionResult ImportProductionValues([FromQuery] ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        //{

        //    //数据读取
        //    var responseAjaxData = _productionValueImportService.ReadImportProductionData(importHistoryProductionValuesRequestDto);

        //    var startDate = importHistoryProductionValuesRequestDto.GetStartDate();
        //    var endDate = importHistoryProductionValuesRequestDto.GetEndDate();

        //    ConvertHelper.TryConvertDateTimeFromDateDay(startDate, out DateTime startTime);
        //    ConvertHelper.TryConvertDateTimeFromDateDay(endDate, out DateTime endTime);

        //    var templeFile = @"E:\project\HNKC.SZGHAPI\GHMonitoringCenterApi.Domain.Shared\Template\Excel\ExcelHistoryProdution.xlsx";
        //    //区分月份  starttime-endTime 几个月份 几个sheet
        //    var newDate = DateTime.MinValue;
        //    int sheetNum = startDate == endDate ? 1 : endTime.Month - startTime.Month + 1;
        //    NPOI.XSSF.UserModel.XSSFWorkbook book = new NPOI.XSSF.UserModel.XSSFWorkbook(); 
        //    var sheet = book.CreateSheet();
        //    for (int i = 0; i < 1; i++)
        //    {
        //        newDate = startTime;
        //        //外层循环控制sheet个数
        //        //var sheetName = workbook.CreateSheet(newDate.ToString("yyyy年MM月"));
        //        //当月天数 控制多少张表格
        //        int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);
        //        var inDateTime = newDate;
        //        for (int days = 0; days < daysInMonth; days++)
        //        {
        //            //更新当前日期
        //            var inDateDay = inDateTime.ToDateDay();
        //            var oneTable = responseAjaxData.ExcelCompanyProjectBasePoductions.Where(x => x.DateDay == inDateDay).ToList();
        //            //创建行
        //            var rowIndex = 1;
        //            foreach (var item in oneTable)
        //            {
        //                var datarow = sheet.CreateRow(rowIndex);
        //                datarow.CreateCell(0).SetCellValue(rowIndex);
        //                datarow.CreateCell(1).SetCellValue(item.UnitName);
        //                datarow.CreateCell(2).SetCellValue(item.OnContractProjectCount);
        //                rowIndex++;
        //            }
        //            inDateTime = inDateTime.AddDays(1);
        //        }
        //        newDate = newDate.AddMonths(1);
        //    }
        //    using (var f = System.IO.File.OpenWrite(templeFile))
        //    {
        //        book.Write(f);
        //    }
        //    using MemoryStream stream = new MemoryStream(); 
        //    //将Excel 文件作为下载返回给客户端
        //    var bytes = System.IO.File.ReadAllBytes(templeFile);
        //    return File(bytes, "application/octet-stream", $"{System.DateTime.Now.ToString("yyyyMMdd")}.xlsx");
        //}
        [HttpGet("ExcelJJtSendMessageWrite")]
        public Task<ResponseAjaxResult<bool>> ExcelJJtSendMessageWriteAsync()
        {
            return _productionValueImportService.ExcelJJtSendMessageWriteAsync();
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ImportHistoryProduction")]
        public async Task<IActionResult> ImportHistoryProduction([FromQuery] ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            #region 逻辑判断
            int year = 2024, month = 1;
            if (importHistoryProductionValuesRequestDto.TimeValue.HasValue == false)
            {
                //说明没有传  默认当前时间前一天
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            else {
                var time = importHistoryProductionValuesRequestDto.TimeValue.Value;
                year = time.Year;
                month = time.Month;
            }

            #endregion

            #region 模版路径
            //var tempPath = "E:\\project\\HNKC.SZGHAPI\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProductionDayReport.xlsx";
            var tempPath = "D:\\projectconllection\\dotnet\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProductionDayReport.xlsx";
            //var tempPath = "Template/Excel/CompanyOnProjectTemplate.xlsx";

            //importHistoryProductionValuesRequestDto.GetYearAndMonth();
            #endregion

            #region 查询数据
            var baseProject = await _productionValueImportService.ExcelJJtSendMessageAsync(year,month);
            #endregion


           

            var value = new 
            {
                datesheet1 = importHistoryProductionValuesRequestDto.TimeValue.HasValue ? importHistoryProductionValuesRequestDto.TimeValue.Value.ToString("yyyy年MM月") : DateTime.Now.AddDays(-1).ToString("yyyy年MM月"),
                result1 = baseProject.Data[0].CompanyProjectBasePoduction,
                result2 = baseProject.Data[0].CompanyBasePoductionValue,
                result3 = baseProject.Data[0].CompanyShipBuildInfo,
                result4 = baseProject.Data[0].CompanyShipProductionValueInfo,
                result5 = baseProject.Data[0].ShipProductionValue,
                result6 = baseProject.Data[0].SpecialProjectInfo,
                result7 = baseProject.Data[0].CompanyWriteReportInfo,
                result8 = baseProject.Data[0].CompanyUnWriteReportInfo,
                result9 = baseProject.Data[0].CompanyShipUnWriteReportInfo,

                //result10 = baseProject.Data[0].ProjectShiftProductionInfo,
                //result11 = baseProject.Data[0].UnProjectShitInfo

            };
            return await ExcelTemplateImportAsync(tempPath, value, $"{year}年广航局生产运营监控日报");
        }

    }
}
