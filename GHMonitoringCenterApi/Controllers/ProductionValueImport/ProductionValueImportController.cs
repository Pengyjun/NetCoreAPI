using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using SqlSugar.Extensions;

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
        /// 导出广航局监控日报历史数据
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
            else
            {
                var time = importHistoryProductionValuesRequestDto.TimeValue.Value;
                year = time.Year;
                month = time.Month;
            }

            #endregion

            #region 模版路径
            //var tempPath = "E:\\project\\HNKC.SZGHAPI\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProductionDayReport.xlsx";
            //var tempPath = "D:\\projectconllection\\dotnet\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProductionDayReport.xlsx";
            var tempPath = "Template/Excel/ProductionDayReport.xlsx";

            #endregion

            #region 查询数据
            var baseProject = await _productionValueImportService.ExcelJJtSendMessageAsync(year, month);

            //如果没有三十天的数据  追加三十天的空数据
            if (baseProject.Data.Count() != 31)
            {
                //差的天数
                int diffDays = 31 - baseProject.Data.Count();
                for (int i = 0; i < diffDays; i++)
                {
                    baseProject.Data.Add(new ProductionDayReportHistoryResponseDto
                    {
                        CompanyBasePoductionValue = new List<CompanyBasePoductionValue>(),
                        CompanyProjectBasePoduction = new List<CompanyProjectBasePoduction>(),
                        CompanyShipBuildInfo = new List<CompanyShipBuildInfo>(),
                        CompanyShipProductionValueInfo = new List<CompanyShipProductionValueInfo>(),
                        CompanyShipUnWriteReportInfo = new List<CompanyShipUnWriteReportInfo>(),
                        CompanyUnWriteReportInfo = new List<CompanyUnWriteReportInfo>(),
                        CompanyWriteReportInfo = new List<CompanyWriteReportInfo>(),
                        ExcelTitle = new List<Domain.Models.ExcelTitle>(),
                        ShipProductionValue = new List<ShipProductionValue>(),
                        SpecialProjectInfo = new List<SpecialProjectInfo>()
                    });
                }
            }
            #endregion

            var value = new ResultResponseDto
            {
                datesheetone1 = baseProject.Data[0].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[0].CompanyProjectBasePoduction[0].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultone1 = baseProject.Data[0].CompanyProjectBasePoduction,
                resultone2 = baseProject.Data[0].CompanyBasePoductionValue,
                resultone3 = baseProject.Data[0].CompanyShipBuildInfo,
                resultone4 = baseProject.Data[0].CompanyShipProductionValueInfo,
                resultone5 = baseProject.Data[0].ShipProductionValue,
                resultone6 = baseProject.Data[0].SpecialProjectInfo,
                resultone7 = baseProject.Data[0].CompanyWriteReportInfo,
                resultone8 = baseProject.Data[0].CompanyUnWriteReportInfo,
                resultone9 = baseProject.Data[0].CompanyShipUnWriteReportInfo,
                titleone1 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titleone2 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titleone3 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titleone4 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titleone5 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titleone6 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titleone7 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titleone8 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titleone9 = baseProject.Data[0].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwo1 = baseProject.Data[1].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[1].CompanyProjectBasePoduction[1].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwo1 = baseProject.Data[1].CompanyProjectBasePoduction,
                resulttwo2 = baseProject.Data[1].CompanyBasePoductionValue,
                resulttwo3 = baseProject.Data[1].CompanyShipBuildInfo,
                resulttwo4 = baseProject.Data[1].CompanyShipProductionValueInfo,
                resulttwo5 = baseProject.Data[1].ShipProductionValue,
                resulttwo6 = baseProject.Data[1].SpecialProjectInfo,
                resulttwo7 = baseProject.Data[1].CompanyWriteReportInfo,
                resulttwo8 = baseProject.Data[1].CompanyUnWriteReportInfo,
                resulttwo9 = baseProject.Data[1].CompanyShipUnWriteReportInfo,
                titletwo1 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwo2 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwo3 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwo4 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwo5 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwo6 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwo7 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwo8 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwo9 = baseProject.Data[1].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetthird1 = baseProject.Data[2].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[2].CompanyProjectBasePoduction[2].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultthird1 = baseProject.Data[2].CompanyProjectBasePoduction,
                resultthird2 = baseProject.Data[2].CompanyBasePoductionValue,
                resultthird3 = baseProject.Data[2].CompanyShipBuildInfo,
                resultthird4 = baseProject.Data[2].CompanyShipProductionValueInfo,
                resultthird5 = baseProject.Data[2].ShipProductionValue,
                resultthird6 = baseProject.Data[2].SpecialProjectInfo,
                resultthird7 = baseProject.Data[2].CompanyWriteReportInfo,
                resultthird8 = baseProject.Data[2].CompanyUnWriteReportInfo,
                resultthird9 = baseProject.Data[2].CompanyShipUnWriteReportInfo,
                titlethird1 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlethird2 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlethird3 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlethird4 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlethird5 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlethird6 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlethird7 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlethird8 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlethird9 = baseProject.Data[2].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetfour1 = baseProject.Data[3].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[3].CompanyProjectBasePoduction[3].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultfour1 = baseProject.Data[3].CompanyProjectBasePoduction,
                resultfour2 = baseProject.Data[3].CompanyBasePoductionValue,
                resultfour3 = baseProject.Data[3].CompanyShipBuildInfo,
                resultfour4 = baseProject.Data[3].CompanyShipProductionValueInfo,
                resultfour5 = baseProject.Data[3].ShipProductionValue,
                resultfour6 = baseProject.Data[3].SpecialProjectInfo,
                resultfour7 = baseProject.Data[3].CompanyWriteReportInfo,
                resultfour8 = baseProject.Data[3].CompanyUnWriteReportInfo,
                resultfour9 = baseProject.Data[3].CompanyShipUnWriteReportInfo,
                titlefour1 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlefour2 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlefour3 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlefour4 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlefour5 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlefour6 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlefour7 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlefour8 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlefour9 = baseProject.Data[3].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetfive1 = baseProject.Data[4].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[4].CompanyProjectBasePoduction[4].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultfive1 = baseProject.Data[4].CompanyProjectBasePoduction,
                resultfive2 = baseProject.Data[4].CompanyBasePoductionValue,
                resultfive3 = baseProject.Data[4].CompanyShipBuildInfo,
                resultfive4 = baseProject.Data[4].CompanyShipProductionValueInfo,
                resultfive5 = baseProject.Data[4].ShipProductionValue,
                resultfive6 = baseProject.Data[4].SpecialProjectInfo,
                resultfive7 = baseProject.Data[4].CompanyWriteReportInfo,
                resultfive8 = baseProject.Data[4].CompanyUnWriteReportInfo,
                resultfive9 = baseProject.Data[4].CompanyShipUnWriteReportInfo,
                titlefive1 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlefive2 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlefive3 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlefive4 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlefive5 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlefive6 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlefive7 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlefive8 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlefive9 = baseProject.Data[4].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetsix1 = baseProject.Data[5].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[5].CompanyProjectBasePoduction[5].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultsix1 = baseProject.Data[5].CompanyProjectBasePoduction,
                resultsix2 = baseProject.Data[5].CompanyBasePoductionValue,
                resultsix3 = baseProject.Data[5].CompanyShipBuildInfo,
                resultsix4 = baseProject.Data[5].CompanyShipProductionValueInfo,
                resultsix5 = baseProject.Data[5].ShipProductionValue,
                resultsix6 = baseProject.Data[5].SpecialProjectInfo,
                resultsix7 = baseProject.Data[5].CompanyWriteReportInfo,
                resultsix8 = baseProject.Data[5].CompanyUnWriteReportInfo,
                resultsix9 = baseProject.Data[5].CompanyShipUnWriteReportInfo,
                titlesix1 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlesix2 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlesix3 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlesix4 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlesix5 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlesix6 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlesix7 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlesix8 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlesix9 = baseProject.Data[5].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetseven1 = baseProject.Data[6].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[6].CompanyProjectBasePoduction[6].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultseven1 = baseProject.Data[6].CompanyProjectBasePoduction,
                resultseven2 = baseProject.Data[6].CompanyBasePoductionValue,
                resultseven3 = baseProject.Data[6].CompanyShipBuildInfo,
                resultseven4 = baseProject.Data[6].CompanyShipProductionValueInfo,
                resultseven5 = baseProject.Data[6].ShipProductionValue,
                resultseven6 = baseProject.Data[6].SpecialProjectInfo,
                resultseven7 = baseProject.Data[6].CompanyWriteReportInfo,
                resultseven8 = baseProject.Data[6].CompanyUnWriteReportInfo,
                resultseven9 = baseProject.Data[6].CompanyShipUnWriteReportInfo,
                titleseven1 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titleseven2 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titleseven3 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titleseven4 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titleseven5 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titleseven6 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titleseven7 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titleseven8 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titleseven9 = baseProject.Data[6].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheeteight1 = baseProject.Data[7].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[7].CompanyProjectBasePoduction[7].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulteight1 = baseProject.Data[7].CompanyProjectBasePoduction,
                resulteight2 = baseProject.Data[7].CompanyBasePoductionValue,
                resulteight3 = baseProject.Data[7].CompanyShipBuildInfo,
                resulteight4 = baseProject.Data[7].CompanyShipProductionValueInfo,
                resulteight5 = baseProject.Data[7].ShipProductionValue,
                resulteight6 = baseProject.Data[7].SpecialProjectInfo,
                resulteight7 = baseProject.Data[7].CompanyWriteReportInfo,
                resulteight8 = baseProject.Data[7].CompanyUnWriteReportInfo,
                resulteight9 = baseProject.Data[7].CompanyShipUnWriteReportInfo,
                titleeight1 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titleeight2 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titleeight3 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titleeight4 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titleeight5 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titleeight6 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titleeight7 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titleeight8 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titleeight9 = baseProject.Data[7].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetnine1 = baseProject.Data[8].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[8].CompanyProjectBasePoduction[8].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultnine1 = baseProject.Data[8].CompanyProjectBasePoduction,
                resultnine2 = baseProject.Data[8].CompanyBasePoductionValue,
                resultnine3 = baseProject.Data[8].CompanyShipBuildInfo,
                resultnine4 = baseProject.Data[8].CompanyShipProductionValueInfo,
                resultnine5 = baseProject.Data[8].ShipProductionValue,
                resultnine6 = baseProject.Data[8].SpecialProjectInfo,
                resultnine7 = baseProject.Data[8].CompanyWriteReportInfo,
                resultnine8 = baseProject.Data[8].CompanyUnWriteReportInfo,
                resultnine9 = baseProject.Data[8].CompanyShipUnWriteReportInfo,
                titlenine1 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlenine2 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlenine3 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlenine4 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlenine5 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlenine6 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlenine7 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlenine8 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlenine9 = baseProject.Data[8].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetten1 = baseProject.Data[9].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[9].CompanyProjectBasePoduction[9].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultten1 = baseProject.Data[9].CompanyProjectBasePoduction,
                resultten2 = baseProject.Data[9].CompanyBasePoductionValue,
                resultten3 = baseProject.Data[9].CompanyShipBuildInfo,
                resultten4 = baseProject.Data[9].CompanyShipProductionValueInfo,
                resultten5 = baseProject.Data[9].ShipProductionValue,
                resultten6 = baseProject.Data[9].SpecialProjectInfo,
                resultten7 = baseProject.Data[9].CompanyWriteReportInfo,
                resultten8 = baseProject.Data[9].CompanyUnWriteReportInfo,
                resultten9 = baseProject.Data[9].CompanyShipUnWriteReportInfo,
                titleten1 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titleten2 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titleten3 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titleten4 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titleten5 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titleten6 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titleten7 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titleten8 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titleten9 = baseProject.Data[9].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheeteleven1 = baseProject.Data[10].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[10].CompanyProjectBasePoduction[10].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulteleven1 = baseProject.Data[10].CompanyProjectBasePoduction,
                resulteleven2 = baseProject.Data[10].CompanyBasePoductionValue,
                resulteleven3 = baseProject.Data[10].CompanyShipBuildInfo,
                resulteleven4 = baseProject.Data[10].CompanyShipProductionValueInfo,
                resulteleven5 = baseProject.Data[10].ShipProductionValue,
                resulteleven6 = baseProject.Data[10].SpecialProjectInfo,
                resulteleven7 = baseProject.Data[10].CompanyWriteReportInfo,
                resulteleven8 = baseProject.Data[10].CompanyUnWriteReportInfo,
                resulteleven9 = baseProject.Data[10].CompanyShipUnWriteReportInfo,
                titleeleven1 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titleeleven2 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titleeleven3 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titleeleven4 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titleeleven5 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titleeleven6 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titleeleven7 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titleeleven8 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titleeleven9 = baseProject.Data[10].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwelve1 = baseProject.Data[11].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[11].CompanyProjectBasePoduction[11].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwelve1 = baseProject.Data[11].CompanyProjectBasePoduction,
                resulttwelve2 = baseProject.Data[11].CompanyBasePoductionValue,
                resulttwelve3 = baseProject.Data[11].CompanyShipBuildInfo,
                resulttwelve4 = baseProject.Data[11].CompanyShipProductionValueInfo,
                resulttwelve5 = baseProject.Data[11].ShipProductionValue,
                resulttwelve6 = baseProject.Data[11].SpecialProjectInfo,
                resulttwelve7 = baseProject.Data[11].CompanyWriteReportInfo,
                resulttwelve8 = baseProject.Data[11].CompanyUnWriteReportInfo,
                resulttwelve9 = baseProject.Data[11].CompanyShipUnWriteReportInfo,
                titletwelve1 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwelve2 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwelve3 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwelve4 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwelve5 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwelve6 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwelve7 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwelve8 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwelve9 = baseProject.Data[11].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetthirteen1 = baseProject.Data[12].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[12].CompanyProjectBasePoduction[12].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultthirteen1 = baseProject.Data[12].CompanyProjectBasePoduction,
                resultthirteen2 = baseProject.Data[12].CompanyBasePoductionValue,
                resultthirteen3 = baseProject.Data[12].CompanyShipBuildInfo,
                resultthirteen4 = baseProject.Data[12].CompanyShipProductionValueInfo,
                resultthirteen5 = baseProject.Data[12].ShipProductionValue,
                resultthirteen6 = baseProject.Data[12].SpecialProjectInfo,
                resultthirteen7 = baseProject.Data[12].CompanyWriteReportInfo,
                resultthirteen8 = baseProject.Data[12].CompanyUnWriteReportInfo,
                resultthirteen9 = baseProject.Data[12].CompanyShipUnWriteReportInfo,
                titlethirteen1 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlethirteen2 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlethirteen3 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlethirteen4 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlethirteen5 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlethirteen6 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlethirteen7 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlethirteen8 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlethirteen9 = baseProject.Data[12].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetfourteen1 = baseProject.Data[13].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[13].CompanyProjectBasePoduction[13].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultfourteen1 = baseProject.Data[13].CompanyProjectBasePoduction,
                resultfourteen2 = baseProject.Data[13].CompanyBasePoductionValue,
                resultfourteen3 = baseProject.Data[13].CompanyShipBuildInfo,
                resultfourteen4 = baseProject.Data[13].CompanyShipProductionValueInfo,
                resultfourteen5 = baseProject.Data[13].ShipProductionValue,
                resultfourteen6 = baseProject.Data[13].SpecialProjectInfo,
                resultfourteen7 = baseProject.Data[13].CompanyWriteReportInfo,
                resultfourteen8 = baseProject.Data[13].CompanyUnWriteReportInfo,
                resultfourteen9 = baseProject.Data[13].CompanyShipUnWriteReportInfo,
                titlefourteen1 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlefourteen2 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlefourteen3 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlefourteen4 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlefourteen5 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlefourteen6 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlefourteen7 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlefourteen8 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlefourteen9 = baseProject.Data[13].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetfifteen1 = baseProject.Data[14].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[14].CompanyProjectBasePoduction[14].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultfifteen1 = baseProject.Data[14].CompanyProjectBasePoduction,
                resultfifteen2 = baseProject.Data[14].CompanyBasePoductionValue,
                resultfifteen3 = baseProject.Data[14].CompanyShipBuildInfo,
                resultfifteen4 = baseProject.Data[14].CompanyShipProductionValueInfo,
                resultfifteen5 = baseProject.Data[14].ShipProductionValue,
                resultfifteen6 = baseProject.Data[14].SpecialProjectInfo,
                resultfifteen7 = baseProject.Data[14].CompanyWriteReportInfo,
                resultfifteen8 = baseProject.Data[14].CompanyUnWriteReportInfo,
                resultfifteen9 = baseProject.Data[14].CompanyShipUnWriteReportInfo,
                titlefifteen1 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlefifteen2 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlefifteen3 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlefifteen4 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlefifteen5 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlefifteen6 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlefifteen7 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlefifteen8 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlefifteen9 = baseProject.Data[14].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetsixteen1 = baseProject.Data[15].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[15].CompanyProjectBasePoduction[15].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultsixteen1 = baseProject.Data[15].CompanyProjectBasePoduction,
                resultsixteen2 = baseProject.Data[15].CompanyBasePoductionValue,
                resultsixteen3 = baseProject.Data[15].CompanyShipBuildInfo,
                resultsixteen4 = baseProject.Data[15].CompanyShipProductionValueInfo,
                resultsixteen5 = baseProject.Data[15].ShipProductionValue,
                resultsixteen6 = baseProject.Data[15].SpecialProjectInfo,
                resultsixteen7 = baseProject.Data[15].CompanyWriteReportInfo,
                resultsixteen8 = baseProject.Data[15].CompanyUnWriteReportInfo,
                resultsixteen9 = baseProject.Data[15].CompanyShipUnWriteReportInfo,
                titlesixteen1 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlesixteen2 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlesixteen3 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlesixteen4 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlesixteen5 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlesixteen6 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlesixteen7 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlesixteen8 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlesixteen9 = baseProject.Data[15].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetsevevnteen1 = baseProject.Data[16].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[16].CompanyProjectBasePoduction[16].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultsevevnteen1 = baseProject.Data[16].CompanyProjectBasePoduction,
                resultsevevnteen2 = baseProject.Data[16].CompanyBasePoductionValue,
                resultsevevnteen3 = baseProject.Data[16].CompanyShipBuildInfo,
                resultsevevnteen4 = baseProject.Data[16].CompanyShipProductionValueInfo,
                resultsevevnteen5 = baseProject.Data[16].ShipProductionValue,
                resultsevevnteen6 = baseProject.Data[16].SpecialProjectInfo,
                resultsevevnteen7 = baseProject.Data[16].CompanyWriteReportInfo,
                resultsevevnteen8 = baseProject.Data[16].CompanyUnWriteReportInfo,
                resultsevevnteen9 = baseProject.Data[16].CompanyShipUnWriteReportInfo,
                titlesevevnteen1 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlesevevnteen2 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlesevevnteen3 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlesevevnteen4 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlesevevnteen5 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlesevevnteen6 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlesevevnteen7 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlesevevnteen8 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlesevevnteen9 = baseProject.Data[16].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheeteighteen1 = baseProject.Data[17].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[17].CompanyProjectBasePoduction[17].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulteighteen1 = baseProject.Data[17].CompanyProjectBasePoduction,
                resulteighteen2 = baseProject.Data[17].CompanyBasePoductionValue,
                resulteighteen3 = baseProject.Data[17].CompanyShipBuildInfo,
                resulteighteen4 = baseProject.Data[17].CompanyShipProductionValueInfo,
                resulteighteen5 = baseProject.Data[17].ShipProductionValue,
                resulteighteen6 = baseProject.Data[17].SpecialProjectInfo,
                resulteighteen7 = baseProject.Data[17].CompanyWriteReportInfo,
                resulteighteen8 = baseProject.Data[17].CompanyUnWriteReportInfo,
                resulteighteen9 = baseProject.Data[17].CompanyShipUnWriteReportInfo,
                titleeighteen1 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titleeighteen2 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titleeighteen3 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titleeighteen4 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titleeighteen5 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titleeighteen6 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titleeighteen7 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titleeighteen8 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titleeighteen9 = baseProject.Data[17].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetnineteen1 = baseProject.Data[18].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[18].CompanyProjectBasePoduction[18].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultnineteen1 = baseProject.Data[18].CompanyProjectBasePoduction,
                resultnineteen2 = baseProject.Data[18].CompanyBasePoductionValue,
                resultnineteen3 = baseProject.Data[18].CompanyShipBuildInfo,
                resultnineteen4 = baseProject.Data[18].CompanyShipProductionValueInfo,
                resultnineteen5 = baseProject.Data[18].ShipProductionValue,
                resultnineteen6 = baseProject.Data[18].SpecialProjectInfo,
                resultnineteen7 = baseProject.Data[18].CompanyWriteReportInfo,
                resultnineteen8 = baseProject.Data[18].CompanyUnWriteReportInfo,
                resultnineteen9 = baseProject.Data[18].CompanyShipUnWriteReportInfo,
                titlenineteen1 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlenineteen2 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlenineteen3 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlenineteen4 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlenineteen5 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlenineteen6 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlenineteen7 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlenineteen8 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlenineteen9 = baseProject.Data[18].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwenty1 = baseProject.Data[19].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[19].CompanyProjectBasePoduction[19].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwenty1 = baseProject.Data[19].CompanyProjectBasePoduction,
                resulttwenty2 = baseProject.Data[19].CompanyBasePoductionValue,
                resulttwenty3 = baseProject.Data[19].CompanyShipBuildInfo,
                resulttwenty4 = baseProject.Data[19].CompanyShipProductionValueInfo,
                resulttwenty5 = baseProject.Data[19].ShipProductionValue,
                resulttwenty6 = baseProject.Data[19].SpecialProjectInfo,
                resulttwenty7 = baseProject.Data[19].CompanyWriteReportInfo,
                resulttwenty8 = baseProject.Data[19].CompanyUnWriteReportInfo,
                resulttwenty9 = baseProject.Data[19].CompanyShipUnWriteReportInfo,
                titletwenty1 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwenty2 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwenty3 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwenty4 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwenty5 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwenty6 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwenty7 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwenty8 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwenty9 = baseProject.Data[19].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentyone1 = baseProject.Data[20].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[20].CompanyProjectBasePoduction[20].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentyone1 = baseProject.Data[20].CompanyProjectBasePoduction,
                resulttwentyone2 = baseProject.Data[20].CompanyBasePoductionValue,
                resulttwentyone3 = baseProject.Data[20].CompanyShipBuildInfo,
                resulttwentyone4 = baseProject.Data[20].CompanyShipProductionValueInfo,
                resulttwentyone5 = baseProject.Data[20].ShipProductionValue,
                resulttwentyone6 = baseProject.Data[20].SpecialProjectInfo,
                resulttwentyone7 = baseProject.Data[20].CompanyWriteReportInfo,
                resulttwentyone8 = baseProject.Data[20].CompanyUnWriteReportInfo,
                resulttwentyone9 = baseProject.Data[20].CompanyShipUnWriteReportInfo,
                titletwentyone1 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentyone2 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentyone3 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentyone4 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentyone5 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentyone6 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentyone7 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentyone8 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentyone9 = baseProject.Data[20].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentytwo1 = baseProject.Data[21].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[21].CompanyProjectBasePoduction[21].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentytwo1 = baseProject.Data[21].CompanyProjectBasePoduction,
                resulttwentytwo2 = baseProject.Data[21].CompanyBasePoductionValue,
                resulttwentytwo3 = baseProject.Data[21].CompanyShipBuildInfo,
                resulttwentytwo4 = baseProject.Data[21].CompanyShipProductionValueInfo,
                resulttwentytwo5 = baseProject.Data[21].ShipProductionValue,
                resulttwentytwo6 = baseProject.Data[21].SpecialProjectInfo,
                resulttwentytwo7 = baseProject.Data[21].CompanyWriteReportInfo,
                resulttwentytwo8 = baseProject.Data[21].CompanyUnWriteReportInfo,
                resulttwentytwo9 = baseProject.Data[21].CompanyShipUnWriteReportInfo,
                titletwentytwo1 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentytwo2 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentytwo3 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentytwo4 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentytwo5 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentytwo6 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentytwo7 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentytwo8 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentytwo9 = baseProject.Data[21].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentythree1 = baseProject.Data[22].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[22].CompanyProjectBasePoduction[22].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentythree1 = baseProject.Data[22].CompanyProjectBasePoduction,
                resulttwentythree2 = baseProject.Data[22].CompanyBasePoductionValue,
                resulttwentythree3 = baseProject.Data[22].CompanyShipBuildInfo,
                resulttwentythree4 = baseProject.Data[22].CompanyShipProductionValueInfo,
                resulttwentythree5 = baseProject.Data[22].ShipProductionValue,
                resulttwentythree6 = baseProject.Data[22].SpecialProjectInfo,
                resulttwentythree7 = baseProject.Data[22].CompanyWriteReportInfo,
                resulttwentythree8 = baseProject.Data[22].CompanyUnWriteReportInfo,
                resulttwentythree9 = baseProject.Data[22].CompanyShipUnWriteReportInfo,
                titletwentythree1 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentythree2 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentythree3 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentythree4 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentythree5 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentythree6 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentythree7 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentythree8 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentythree9 = baseProject.Data[22].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentyfour1 = baseProject.Data[23].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[23].CompanyProjectBasePoduction[23].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentyfour1 = baseProject.Data[23].CompanyProjectBasePoduction,
                resulttwentyfour2 = baseProject.Data[23].CompanyBasePoductionValue,
                resulttwentyfour3 = baseProject.Data[23].CompanyShipBuildInfo,
                resulttwentyfour4 = baseProject.Data[23].CompanyShipProductionValueInfo,
                resulttwentyfour5 = baseProject.Data[23].ShipProductionValue,
                resulttwentyfour6 = baseProject.Data[23].SpecialProjectInfo,
                resulttwentyfour7 = baseProject.Data[23].CompanyWriteReportInfo,
                resulttwentyfour8 = baseProject.Data[23].CompanyUnWriteReportInfo,
                resulttwentyfour9 = baseProject.Data[23].CompanyShipUnWriteReportInfo,
                titletwentyfour1 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentyfour2 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentyfour3 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentyfour4 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentyfour5 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentyfour6 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentyfour7 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentyfour8 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentyfour9 = baseProject.Data[23].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentyfive1 = baseProject.Data[24].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[24].CompanyProjectBasePoduction[24].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentyfive1 = baseProject.Data[24].CompanyProjectBasePoduction,
                resulttwentyfive2 = baseProject.Data[24].CompanyBasePoductionValue,
                resulttwentyfive3 = baseProject.Data[24].CompanyShipBuildInfo,
                resulttwentyfive4 = baseProject.Data[24].CompanyShipProductionValueInfo,
                resulttwentyfive5 = baseProject.Data[24].ShipProductionValue,
                resulttwentyfive6 = baseProject.Data[24].SpecialProjectInfo,
                resulttwentyfive7 = baseProject.Data[24].CompanyWriteReportInfo,
                resulttwentyfive8 = baseProject.Data[24].CompanyUnWriteReportInfo,
                resulttwentyfive9 = baseProject.Data[24].CompanyShipUnWriteReportInfo,
                titletwentyfive1 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentyfive2 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentyfive3 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentyfive4 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentyfive5 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentyfive6 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentyfive7 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentyfive8 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentyfive9 = baseProject.Data[24].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentysix1 = baseProject.Data[25].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[25].CompanyProjectBasePoduction[25].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentysix1 = baseProject.Data[25].CompanyProjectBasePoduction,
                resulttwentysix2 = baseProject.Data[25].CompanyBasePoductionValue,
                resulttwentysix3 = baseProject.Data[25].CompanyShipBuildInfo,
                resulttwentysix4 = baseProject.Data[25].CompanyShipProductionValueInfo,
                resulttwentysix5 = baseProject.Data[25].ShipProductionValue,
                resulttwentysix6 = baseProject.Data[25].SpecialProjectInfo,
                resulttwentysix7 = baseProject.Data[25].CompanyWriteReportInfo,
                resulttwentysix8 = baseProject.Data[25].CompanyUnWriteReportInfo,
                resulttwentysix9 = baseProject.Data[25].CompanyShipUnWriteReportInfo,
                titletwentysix1 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentysix2 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentysix3 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentysix4 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentysix5 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentysix6 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentysix7 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentysix8 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentysix9 = baseProject.Data[25].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentyseven1 = baseProject.Data[26].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[26].CompanyProjectBasePoduction[26].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentyseven1 = baseProject.Data[26].CompanyProjectBasePoduction,
                resulttwentyseven2 = baseProject.Data[26].CompanyBasePoductionValue,
                resulttwentyseven3 = baseProject.Data[26].CompanyShipBuildInfo,
                resulttwentyseven4 = baseProject.Data[26].CompanyShipProductionValueInfo,
                resulttwentyseven5 = baseProject.Data[26].ShipProductionValue,
                resulttwentyseven6 = baseProject.Data[26].SpecialProjectInfo,
                resulttwentyseven7 = baseProject.Data[26].CompanyWriteReportInfo,
                resulttwentyseven8 = baseProject.Data[26].CompanyUnWriteReportInfo,
                resulttwentyseven9 = baseProject.Data[26].CompanyShipUnWriteReportInfo,
                titletwentyseven1 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentyseven2 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentyseven3 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentyseven4 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentyseven5 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentyseven6 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentyseven7 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentyseven8 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentyseven9 = baseProject.Data[26].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentyeight1 = baseProject.Data[27].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[27].CompanyProjectBasePoduction[27].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentyeight1 = baseProject.Data[27].CompanyProjectBasePoduction,
                resulttwentyeight2 = baseProject.Data[27].CompanyBasePoductionValue,
                resulttwentyeight3 = baseProject.Data[27].CompanyShipBuildInfo,
                resulttwentyeight4 = baseProject.Data[27].CompanyShipProductionValueInfo,
                resulttwentyeight5 = baseProject.Data[27].ShipProductionValue,
                resulttwentyeight6 = baseProject.Data[27].SpecialProjectInfo,
                resulttwentyeight7 = baseProject.Data[27].CompanyWriteReportInfo,
                resulttwentyeight8 = baseProject.Data[27].CompanyUnWriteReportInfo,
                resulttwentyeight9 = baseProject.Data[27].CompanyShipUnWriteReportInfo,
                titletwentyeight1 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentyeight2 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentyeight3 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentyeight4 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentyeight5 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentyeight6 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentyeight7 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentyeight8 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentyeight9 = baseProject.Data[27].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheettwentynine1 = baseProject.Data[28].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[28].CompanyProjectBasePoduction[28].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resulttwentynine1 = baseProject.Data[28].CompanyProjectBasePoduction,
                resulttwentynine2 = baseProject.Data[28].CompanyBasePoductionValue,
                resulttwentynine3 = baseProject.Data[28].CompanyShipBuildInfo,
                resulttwentynine4 = baseProject.Data[28].CompanyShipProductionValueInfo,
                resulttwentynine5 = baseProject.Data[28].ShipProductionValue,
                resulttwentynine6 = baseProject.Data[28].SpecialProjectInfo,
                resulttwentynine7 = baseProject.Data[28].CompanyWriteReportInfo,
                resulttwentynine8 = baseProject.Data[28].CompanyUnWriteReportInfo,
                resulttwentynine9 = baseProject.Data[28].CompanyShipUnWriteReportInfo,
                titletwentynine1 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titletwentynine2 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titletwentynine3 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titletwentynine4 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titletwentynine5 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titletwentynine6 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titletwentynine7 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titletwentynine8 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titletwentynine9 = baseProject.Data[28].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetthirty1 = baseProject.Data[29].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[29].CompanyProjectBasePoduction[29].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultthirty1 = baseProject.Data[29].CompanyProjectBasePoduction,
                resultthirty2 = baseProject.Data[29].CompanyBasePoductionValue,
                resultthirty3 = baseProject.Data[29].CompanyShipBuildInfo,
                resultthirty4 = baseProject.Data[29].CompanyShipProductionValueInfo,
                resultthirty5 = baseProject.Data[29].ShipProductionValue,
                resultthirty6 = baseProject.Data[29].SpecialProjectInfo,
                resultthirty7 = baseProject.Data[29].CompanyWriteReportInfo,
                resultthirty8 = baseProject.Data[29].CompanyUnWriteReportInfo,
                resultthirty9 = baseProject.Data[29].CompanyShipUnWriteReportInfo,
                titlethirty1 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlethirty2 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlethirty3 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlethirty4 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlethirty5 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlethirty6 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlethirty7 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlethirty8 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlethirty9 = baseProject.Data[29].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,


                datesheetthirtyone1 = baseProject.Data[30].CompanyProjectBasePoduction.Count() == 0 ? "" : DateTime.ParseExact(baseProject.Data[30].CompanyProjectBasePoduction[30].DateDay.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日"),
                resultthirtyone1 = baseProject.Data[30].CompanyProjectBasePoduction,
                resultthirtyone2 = baseProject.Data[30].CompanyBasePoductionValue,
                resultthirtyone3 = baseProject.Data[30].CompanyShipBuildInfo,
                resultthirtyone4 = baseProject.Data[30].CompanyShipProductionValueInfo,
                resultthirtyone5 = baseProject.Data[30].ShipProductionValue,
                resultthirtyone6 = baseProject.Data[30].SpecialProjectInfo,
                resultthirtyone7 = baseProject.Data[30].CompanyWriteReportInfo,
                resultthirtyone8 = baseProject.Data[30].CompanyUnWriteReportInfo,
                resultthirtyone9 = baseProject.Data[30].CompanyShipUnWriteReportInfo,
                titlethirtyone1 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 1).FirstOrDefault()?.TtileContent,
                titlethirtyone2 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 2).FirstOrDefault()?.TtileContent,
                titlethirtyone3 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 3).FirstOrDefault()?.TtileContent,
                titlethirtyone4 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 4).FirstOrDefault()?.TtileContent,
                titlethirtyone5 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 5).FirstOrDefault()?.TtileContent,
                titlethirtyone6 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 6).FirstOrDefault()?.TtileContent,
                titlethirtyone7 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 7).FirstOrDefault()?.TtileContent,
                titlethirtyone8 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 8).FirstOrDefault()?.TtileContent,
                titlethirtyone9 = baseProject.Data[30].ExcelTitle.Where(x => x.Type == 9).FirstOrDefault()?.TtileContent,

            };
            return await ExcelTemplateImportAsync(tempPath, value, $"{year}年广航局生产运营监控日报");
        }
        /// <summary>
        /// 导出节假日数据
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ImportHistoryHolidays")]
        public async Task<IActionResult> ImportHistoryHolidays([FromQuery] ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            #region 逻辑判断
            int year = 2024, month = 1;
            if (importHistoryProductionValuesRequestDto.TimeValue.HasValue == false)
            {
                //说明没有传  默认当前时间前一天
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            else
            {
                var time = importHistoryProductionValuesRequestDto.TimeValue.Value;
                year = time.Year;
                month = time.Month;
            }
            #endregion
            #region 模版路径
            //var tempPath = "E:\\project\\HNKC.SZGHAPI\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\HistoryHolidayReport.xlsx";
            var tempPath = "Template/Excel/HistoryHolidayReport.xlsx";

            #endregion


            #region 查询数据

            var baseProject = await _productionValueImportService.ExcelJJtSendMessageHolidaysAsync(year, month);
            #endregion

            var value = new ResultResponseDto
            {
                titileProjectShiftProductionInfo = baseProject.Data.ExcelTitle.Where(x => x.Type == 10).FirstOrDefault()?.TtileContent,
                titileUnProjectShitInfo = baseProject.Data.ExcelTitle.Where(x => x.Type == 11).FirstOrDefault()?.TtileContent + baseProject.Data.UnProjectShitInfo.Count() + "个。",
                ProjectShiftProductionInfo = baseProject.Data.ProjectShiftProductionInfo,
                UnProjectShitInfo = baseProject.Data.UnProjectShitInfo,
                date = $"{year}年{month}月"
            };

            return await ExcelTemplateImportAsync(tempPath, value, $"{year}年{month}月在建项目带班生产动态");

        }
    }
}
