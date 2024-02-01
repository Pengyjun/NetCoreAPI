﻿using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Application.Service.Projects;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;

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

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ImportHistoryProduction")]
        public async Task<IActionResult> ImportHistoryProduction([FromQuery] ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            //需要导入几个月
            var needMonth = 1;
            #region 逻辑判断
            if (importHistoryProductionValuesRequestDto.StartTime.HasValue==false && importHistoryProductionValuesRequestDto.EndTime.HasValue==true)
            {
                responseAjaxResult.Fail("时间格式不合法");
                return Ok(responseAjaxResult);
            }
            if (importHistoryProductionValuesRequestDto.StartTime.HasValue&& importHistoryProductionValuesRequestDto.EndTime.HasValue
                && importHistoryProductionValuesRequestDto.StartTime.Value >importHistoryProductionValuesRequestDto.EndTime.Value)
            {
                responseAjaxResult.Fail("开始时间不能大于结束时间");
                return Ok(responseAjaxResult);
            }
            if (importHistoryProductionValuesRequestDto.StartTime.HasValue == true 
                &&importHistoryProductionValuesRequestDto.EndTime.HasValue == true
                &&importHistoryProductionValuesRequestDto.StartTime.Value!=importHistoryProductionValuesRequestDto.EndTime.Value)
            {
                needMonth=importHistoryProductionValuesRequestDto.EndTime.Value.Month - importHistoryProductionValuesRequestDto.StartTime.Value.Month;
            }
            #endregion
            var tempPath = "D:\\projectconllection\\dotnet\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProductionDayReport.xlsx";
            //var tempPath = "Template/Excel/CompanyOnProjectTemplate.xlsx";
            var baseProject = await _productionValueImportService.ExcelJJtSendMessageAsync(importHistoryProductionValuesRequestDto,needMonth);
            var value = new
            {
                day = DateTime.Now.ToString("yyyy/MM/dd"),
                result = baseProject.Data[0].CompanyProjectBasePoduction,
                result1= baseProject.Data[0].CompanyBasePoductionValue

            };
            return await ExcelTemplateImportAsync(tempPath, value, $"{DateTime.Now.Year}年公司在手项目清单");
        }
    }
}
