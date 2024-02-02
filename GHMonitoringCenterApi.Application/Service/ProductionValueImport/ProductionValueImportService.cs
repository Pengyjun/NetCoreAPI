using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using model = GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using models = GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;

namespace GHMonitoringCenterApi.Application.Service.ProductionValueImport
{
    /// <summary>
    ///  生产日报每天推送和节假日日报推送历史数据导出实现层
    /// </summary>
    public class ProductionValueImportService : IProductionValueImportService
    {
        /// <summary>
        /// 上下文注入
        /// </summary>
        public ISqlSugarClient _dbContext { get; set; }
        /// <summary>
        /// 映射注入
        /// </summary>
        public IMapper _mapper { get; set; }
        /// <summary>
        /// 注入发消息内容
        /// </summary>
        public IJjtSendMessageService _jjtSendMessageService { get; set; }
        /// <summary>
        /// 注入元旦期间在建项目带班生产动态
        /// </summary>
        public IProjectReportService _projectReportService { get; set; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="jjtSendMessageService"></param>
        /// <param name="projectReportService"></param>
        /// <param name="dbContext"></param>
        public ProductionValueImportService(IJjtSendMessageService jjtSendMessageService, IProjectReportService projectReportService, ISqlSugarClient dbContext, IMapper mapper)
        {
            this._jjtSendMessageService = jjtSendMessageService;
            this._projectReportService = projectReportService;
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        /// <summary>
        /// 导出历史数据产值信息
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public Stream ImportProductionValues(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        //{
        //    //数据读取
        //    var responseAjaxData = ReadImportProductionData(importHistoryProductionValuesRequestDto);

        //    var startDate = importHistoryProductionValuesRequestDto.GetStartDate();
        //    var endDate = importHistoryProductionValuesRequestDto.GetEndDate();

        //    ConvertHelper.TryConvertDateTimeFromDateDay(startDate, out DateTime startTime);
        //    ConvertHelper.TryConvertDateTimeFromDateDay(endDate, out DateTime endTime);

        //    var templeFile = @"E:\\szghupload\\ExcelHistoryProdution.xlsx";
        //    //区分月份  starttime-endTime 几个月份 几个sheet
        //    var newDate = DateTime.MinValue;
        //    int sheetNum = startDate == endDate ? 1 : endTime.Month - startTime.Month + 1;
        //    XSSFWorkbook workbook;
        //    using (var fs = new FileStream(templeFile, FileMode.Open, FileAccess.Read))
        //    {
        //        workbook = new XSSFWorkbook(fs);
        //        ISheet sheet = workbook.GetSheetAt(0);
        //        var memory = new MemoryStream();
        //        sheet.DisplayGridlines = true;
        //        for (int i = 0; i < 1; i++)
        //        {
        //            newDate = startTime;
        //            //外层循环控制sheet个数
        //            var sheetName = workbook.CreateSheet(newDate.ToString("yyyy年MM月"));
        //            //当月天数 控制多少张表格
        //            int daysInMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);
        //            var inDateTime = newDate;
        //            for (int days = 0; days < daysInMonth; days++)
        //            {
        //                //更新当前日期
        //                var inDateDay = inDateTime.ToDateDay();
        //                var oneTable = responseAjaxData.ExcelCompanyProjectBasePoductions.Where(x => x.DateDay == inDateDay).ToList();
        //                //创建行
        //                var rowIndex = 1;
        //                foreach (var item in oneTable)
        //                {
        //                    var datarow = sheet.CreateRow(rowIndex);
        //                    datarow.CreateCell(0).SetCellValue(rowIndex);
        //                    datarow.CreateCell(1).SetCellValue(item.UnitName);
        //                    datarow.CreateCell(2).SetCellValue(item.OnContractProjectCount);
        //                    rowIndex++;
        //                }
        //                inDateTime = inDateTime.AddDays(1);
        //            }
        //            newDate = newDate.AddMonths(1);
        //        }
        //        workbook.Write(memory);
        //        memory.Position = 0;
        //        memory.Flush();
        //        return memory;
        //    }

        //}

        /// <summary>
        /// 根据日期读取所含日期内的数据
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        //public ImportHistoryProductionValuesResponseDto ReadImportProductionData(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        //{
        //    var readData = new ImportHistoryProductionValuesResponseDto();

        //    //读取12张表的数据
        //    var startDate = importHistoryProductionValuesRequestDto.GetStartDate();
        //    var endDate = importHistoryProductionValuesRequestDto.GetEndDate();

        //    var excelCompanyProjectBasePoductions = _dbContext.Queryable<ExcelCompanyProjectBasePoduction>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .OrderBy(x => x.UnitDesc)
        //      .ToList();

        //    var excelCompanyBasePoductionValue = _dbContext.Queryable<ExcelCompanyBasePoductionValue>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .OrderBy(x => x.UnitDesc)
        //      .ToList();

        //    var excelCompanyShipBuildInfo = _dbContext.Queryable<ExcelCompanyShipBuildInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .OrderBy(x => x.ShipTypeDesc)
        //      .ToList();

        //    var excelCompanyShipProductionValueInfo = _dbContext.Queryable<ExcelCompanyShipProductionValueInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .OrderBy(x => x.ShipTypeDesc)
        //      .ToList();

        //    var excelShipProductionValue = _dbContext.Queryable<ExcelShipProductionValue>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .ToList();

        //    var excelSpecialProjectInfo = _dbContext.Queryable<ExcelSpecialProjectInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .ToList();

        //    var excelCompanyWriteReportInfo = _dbContext.Queryable<ExcelCompanyWriteReportInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .OrderBy(x => x.UnitDesc)
        //      .ToList();

        //    var excelCompanyUnWriteReportInfo = _dbContext.Queryable<ExcelCompanyUnWriteReportInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .ToList();

        //    var excelCompanyShipUnWriteReportInfo = _dbContext.Queryable<ExcelCompanyShipUnWriteReportInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .ToList();

        //    var excelProjectShiftProductionInfo = _dbContext.Queryable<ExcelProjectShiftProductionInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .ToList();

        //    var excelUnProjectShitInfo = _dbContext.Queryable<ExcelUnProjectShitInfo>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .OrderBy(x => x.UnitDesc)
        //      .ToList();

        //    var excelTitle = _dbContext.Queryable<ExcelTitle>()
        //      .Where(x => x.IsDelete == 1 && x.DateDay >= startDate && x.DateDay <= endDate)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Year != 0, x => importHistoryProductionValuesRequestDto.Year == x.Year)
        //      .WhereIF(importHistoryProductionValuesRequestDto.Month != 0, x => importHistoryProductionValuesRequestDto.Month == x.Month)
        //      .ToList();

        //    readData = new ImportHistoryProductionValuesResponseDto
        //    {
        //        ExcelCompanyProjectBasePoductions = excelCompanyProjectBasePoductions,
        //        ExcelCompanyBasePoductions = excelCompanyBasePoductionValue,
        //        ExcelCompanyShipBuildInfos = excelCompanyShipBuildInfo,
        //        ExcelCompanyShipProductionValueInfos = excelCompanyShipProductionValueInfo,
        //        ExcelShipProductionValues = excelShipProductionValue,
        //        ExcelSpecialProjectInfos = excelSpecialProjectInfo,
        //        ExcelCompanyWriteReportInfos = excelCompanyWriteReportInfo,
        //        ExcelCompanyUnWriteReportInfos = excelCompanyUnWriteReportInfo,
        //        ExcelCompanyShipUnWriteReportInfos = excelCompanyShipUnWriteReportInfo,
        //        ExcelProjectShiftProductionInfos = excelProjectShiftProductionInfo,
        //        ExcelUnProjectShitInfos = excelUnProjectShitInfo,
        //        ExcelTitles = excelTitle
        //    };
        //    return readData;
        //}

        /// <summary>
        /// Excel 智慧运营监控中心图片数据写入表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ExcelJJtSendMessageWriteAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            //调用监控中心图片信息方法
            var getData = await _jjtSendMessageService.JjtTextCardMsgDetailsAsync();

            //各个公司基本项目情况
            var excelCompanyProjectBasePoduction = getData.Data.projectBasePoduction.CompanyProjectBasePoductions;
            //各个公司基本产值情况
            var excelCompanyBasePoductionValue = getData.Data.projectBasePoduction.CompanyBasePoductionValues;
            //自有船施工运转情况
            var excelOwnerShipBuildInfo = getData.Data.OwnerShipBuildInfo.companyShipBuildInfos;
            //各个公司自有船施工产值情况集合
            var excelCompanyShipProductionValueInfo = getData.Data.OwnerShipBuildInfo.companyShipProductionValueInfos;
            //前五船舶产值
            var excelShipProductionValue = getData.Data.OwnerShipBuildInfo.companyShipTopFiveInfoList;
            //特殊情况
            var excelSpecialProjectInfo = getData.Data.SpecialProjectInfo;
            //各单位填报情况
            var excelCompanyWriteReportInfo = getData.Data.CompanyWriteReportInfos;
            //项目生产数据存在不完整部分主要是以下项目未填报
            var excelCompanyUnWriteReportInfo = getData.Data.CompanyUnWriteReportInfos;
            //船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            var excelCompanyShipUnWriteReportInfo = getData.Data.CompanyShipUnWriteReportInfos;

            //调用元旦期间各在建项目带班生产动态
            var getNewYearDayData = await _projectReportService.GetProjectShiftProductionAsync();

            //项目带班生产动态已填报项目
            var excelProjectShiftProductionInfo = getNewYearDayData.Data.projectShiftProductionInfos;
            //项目带班生产动态未填报项目
            var excelUnProjectShitInfo = getNewYearDayData.Data.unProjectShitInfos;

            //数据写入(只写了新增)
            var year = DateTime.Now.AddDays(-1).ToDateYear();
            var month = DateTime.Now.AddDays(-1).ToDateMonth();
            var dateDay = DateTime.Now.AddDays(-1).ToDateDay();
            //titile集合
            var excelTitles = new List<ExcelTitle>();

            //各个公司基本项目情况
            if (excelCompanyProjectBasePoduction.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyProjectBasePoduction>, List<ExcelCompanyProjectBasePoduction>>(excelCompanyProjectBasePoduction);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.UnitName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleOne = "广航局合同项目" + getData.Data.projectBasePoduction.TotalOnContractProjectCount + "个，在建" + getData.Data.projectBasePoduction.TotalOnBuildProjectCount + "个， 停缓建" + getData.Data.projectBasePoduction.TotalStopBuildProjectCount + "个，在建数量占" + getData.Data.projectBasePoduction.TotalBuildCountPercent + "%；今日现场施工设备" + getData.Data.projectBasePoduction.TotalFacilityCount + "台，现场工人" + getData.Data.projectBasePoduction.TotalWorkerCount + "名， 危大工程施工" + getData.Data.projectBasePoduction.TotalRiskWorkCount + "项。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 项目总体生产情况",
                    TtileContent = titleOne,
                    DateDay = dateDay,
                    Type = 1,
                    Year = year
                });
            }
            //各个公司基本产值情况
            if (excelCompanyBasePoductionValue.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyBasePoductionValue>, List<ExcelCompanyBasePoductionValue>>(excelCompanyBasePoductionValue);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.UnitName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleTwo = DateTime.Now.AddDays(-1).ToString("MM月dd日") + " 广航局在建项目产值" + getData.Data.projectBasePoduction.DayProductionValue + "万元，" + getData.Data.Year + "年累计施工产值" + getData.Data.projectBasePoduction.TotalYearProductionValue + "亿元。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 项目总体生产情况",
                    TtileContent = titleTwo,
                    DateDay = dateDay,
                    Type = 2,
                    Year = year
                });
            }
            //自有船施工运转情况
            if (excelOwnerShipBuildInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyShipBuildInfo>, List<ExcelCompanyShipBuildInfo>>(excelOwnerShipBuildInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.ShipTypeDesc = GetShipDesc(item.ShipTypeName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleThree = "广航局三类船舶" + getData.Data.OwnerShipBuildInfo.TotalCount + "艘，其中施工" + getData.Data.OwnerShipBuildInfo.BuildCount + "艘，检修" + getData.Data.OwnerShipBuildInfo.ReconditionCount + "艘，调遣" + getData.Data.OwnerShipBuildInfo.AssignCount + "艘，待命" + getData.Data.OwnerShipBuildInfo.AwaitCount + "艘，开工率" + getData.Data.OwnerShipBuildInfo.BuildPercent + "%；";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 自有船施工运转情况",
                    TtileContent = titleThree,
                    DateDay = dateDay,
                    Type = 3,
                    Year = year
                });
            }
            //各个公司自有船施工产值情况集合
            if (excelCompanyShipProductionValueInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyShipProductionValueInfo>, List<ExcelCompanyShipProductionValueInfo>>(excelCompanyShipProductionValueInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.ShipTypeDesc = GetShipDesc(item.ShipTypeName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleFour = "自有船施工产值" + getData.Data.OwnerShipBuildInfo.BulidProductionValue + "万元，运转时间" + getData.Data.OwnerShipBuildInfo.DayTurnHours + "h；当年累计船舶产值" + getData.Data.OwnerShipBuildInfo.YearTotalProductionValue + "亿元，累计运转时间" + getData.Data.OwnerShipBuildInfo.YearTotalTurnHours + "h。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 自有船施工运转情况",
                    TtileContent = titleFour,
                    DateDay = dateDay,
                    Type = 4,
                    Year = year
                });
            }
            //前五船舶产值
            if (excelShipProductionValue.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.ShipProductionValue>, List<ExcelShipProductionValue>>(excelShipProductionValue);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                //无合计行  新增
                result.Add(new ExcelShipProductionValue
                {
                    Id = GuidUtil.Next(),
                    CreateTime = DateTime.Now,
                    DateDay = dateDay,
                    Month = month,
                    ShipDayOutput = getData.Data.projectBasePoduction.YearTopFiveTotalOutput,
                    ShipName = "合计",
                    ShipYearOutput = getData.Data.projectBasePoduction.YearFiveTotalOutput,
                    TimePercent = getData.Data.projectBasePoduction.YearTotalTopFiveOutputPercent,
                    Year = year
                });
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleFive = getData.Data.Year + "年自有船产值前五的船舶生产情况如下表所示，当年合计产值" + getData.Data.projectBasePoduction.YearFiveTotalOutput + "亿元，占当年自有船总产值" + getData.Data.projectBasePoduction.YearFiveTimeRate + "%。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 自有船施工运转情况",
                    TtileContent = titleFive,
                    DateDay = dateDay,
                    Type = 5,
                    Year = year
                });
            }
            //特殊情况
            if (excelSpecialProjectInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.SpecialProjectInfo>, List<ExcelSpecialProjectInfo>>(excelSpecialProjectInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.TypeDescription = item.Type == 3 ? "提醒事项" : item.Type == 1 ? "异常预警" : item.Type == 2 ? "嘉奖通报" : "";
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleSix = "当日统计特殊情况" + result.Count() + "项，如列表所示：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 特殊情况通报",
                    TtileContent = titleSix,
                    DateDay = dateDay,
                    Type = 6,
                    Year = year
                });
            }
            //各单位填报情况
            if (excelCompanyWriteReportInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyWriteReportInfo>, List<ExcelCompanyWriteReportInfo>>(excelCompanyWriteReportInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.CompanyName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleSeven = "各单位产值日报填报率情况：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 各单位填报情况",
                    TtileContent = titleSeven,
                    DateDay = dateDay,
                    Type = 7,
                    Year = year
                });
            }
            //项目生产数据存在不完整部分主要是以下项目未填报
            if (excelCompanyUnWriteReportInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyUnWriteReportInfo>, List<ExcelCompanyUnWriteReportInfo>>(excelCompanyUnWriteReportInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleEight = "说明：项目生产数据存在不完整部分主要是以下项目未填报：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 各单位填报情况",
                    TtileContent = titleEight,
                    DateDay = dateDay,
                    Type = 8,
                    Year = year
                });
            }
            //船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            if (excelCompanyShipUnWriteReportInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<models.CompanyShipUnWriteReportInfo>, List<ExcelCompanyShipUnWriteReportInfo>>(excelCompanyShipUnWriteReportInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleNine = "说明：船舶生产数据存在不完整部分主要是项目部未填报以下船舶：";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 各单位填报情况",
                    TtileContent = titleNine,
                    DateDay = dateDay,
                    Type = 9,
                    Year = year
                });
            }
            //项目带班生产动态已填报项目
            if (excelProjectShiftProductionInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<ProjectShiftProductionInfo>, List<ExcelProjectShiftProductionInfo>>(excelProjectShiftProductionInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleTen = getNewYearDayData.Data.TimeValue.ToString("yyyy年MM月dd日") + "，广航局在建项目" + result.Count() + "个，现场管理人员" + getNewYearDayData.Data.sumInfo.SumSiteManagementPersonNum + "人，陆域作业" + getNewYearDayData.Data.sumInfo.SumSiteConstructionPersonNum + "人， 陆域设备" + getNewYearDayData.Data.sumInfo.SumConstructionDeviceNum + "台，在场船舶" + getNewYearDayData.Data.sumInfo.SumSiteShipNum + "艘，在船人员" + getNewYearDayData.Data.sumInfo.SumOnShipPersonNum + "人，危大工程施工" + getNewYearDayData.Data.sumInfo.SumHazardousConstructionNum + "项，陆地3~9人作业工点" + getNewYearDayData.Data.sumInfo.SumFewLandWorkplace + "处，陆地10人以上作业工点" + getNewYearDayData.Data.sumInfo.SumLandWorkplace + "处。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 元旦期间各在建项目带班生产动态",
                    TtileContent = titleTen,
                    DateDay = dateDay,
                    Type = 10,
                    Year = year
                });
            }
            //项目带班生产动态未填报项目
            if (excelUnProjectShitInfo.Count() > 0)
            {
                //数据映射
                var result = _mapper.Map<List<UnProjectShitInfo>, List<ExcelUnProjectShitInfo>>(excelUnProjectShitInfo);
                foreach (var item in result)
                {
                    item.Id = GuidUtil.Next();
                    item.DateDay = dateDay;
                    item.Year = year;
                    item.Month = month;
                    item.UnitDesc = GetUnitDesc(item.UnitName);
                }
                await _dbContext.Insertable(result).ExecuteCommandAsync();

                //titile写入
                string titleEleven = "注：陆域3～9人作业工点要求项目领导带班旁站，陆域10人及以上作业工点要求所属三级单位领导带班驻点；以下为未填报项目清单：" + result.Count() + "个。";
                excelTitles.Add(new ExcelTitle()
                {
                    CreateTime = DateTime.Now,
                    Id = GuidUtil.Next(),
                    Month = month,
                    TitleName = "【" + dateDay + "】广航局生产运营监控日报 元旦期间各在建项目带班生产动态",
                    TtileContent = titleEleven,
                    DateDay = dateDay,
                    Type = 11,
                    Year = year
                });
            }

            await _dbContext.Insertable(excelTitles).ExecuteCommandAsync();
            responseAjaxResult.Success();
            responseAjaxResult.Data = true;

            return responseAjaxResult;
        }
        /// <summary>
        /// 获取单位序号
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public int GetUnitDesc(string unitName)
        {
            int num = 1;
            switch (unitName)
            {
                case "疏浚公司":
                    break;
                case "交建公司":
                    num = 2;
                    break;
                case "三公司":
                    num = 3;
                    break;
                case "四公司":
                    num = 4;
                    break;
                case "五公司":
                    num = 5;
                    break;
                case "福建公司":
                    num = 6;
                    break;
                case "直营项目":
                    num = 7;
                    break;
                case "广航局总体":
                    num = 8;
                    break;
            }
            return num;
        }
        /// <summary>
        /// 获取船舶类型排序
        /// </summary>
        /// <param name="shipTypeName"></param>
        /// <returns></returns>
        public int GetShipDesc(string shipTypeName)
        {
            int num = 1;
            switch (shipTypeName)
            {
                case "耙吸船":
                    break;
                case "绞吸船":
                    num = 2;
                    break;
                case "抓斗船":
                    num = 3;
                    break;
                case "合计":
                    num = 4;
                    break;
            }
            return num;
        }

        public async Task<ResponseAjaxResult<List<ProductionDayReportHistoryResponseDto>>> ExcelJJtSendMessageAsync(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            ResponseAjaxResult<List<ProductionDayReportHistoryResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ProductionDayReportHistoryResponseDto>>();

            List<ProductionDayReportHistoryResponseDto> productionDayReportHistoryResponseDtos = new List<ProductionDayReportHistoryResponseDto>();
            ProductionDayReportHistoryResponseDto productionDayReportHistoryResponseDto = new ProductionDayReportHistoryResponseDto();
            importHistoryProductionValuesRequestDto.GetYearAndMonth();
            int year = importHistoryProductionValuesRequestDto.Year;
            int month = importHistoryProductionValuesRequestDto.Month-1;

            #region 数据基础查询
            var baseProjectInfo = await _dbContext.Queryable<ExcelCompanyProjectBasePoduction>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyProjectBasePoduction()
                {
                    Name = x.UnitName,
                    BuildCountPercent = x.BuildCountPercent,
                    FacilityCount = x.FacilityCount,
                    NotWorkCount = x.NotWorkCount,
                    OnBuildProjectCount = x.OnBuildProjectCount,
                    OnContractProjectCount = x.OnContractProjectCount,
                    RiskWorkCount = x.RiskWorkCount,
                    StopBuildProjectCount = x.StopBuildProjectCount,
                    WorkerCount = x.WorkerCount
                }).ToListAsync();
            var baseProjectProductionValue = await _dbContext.Queryable<ExcelCompanyBasePoductionValue>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyBasePoductionValue()
                {
                    DayProductionValue = x.DayProductionValue,
                    Name = x.UnitName,
                    YearProductionValueProgressPercent = x.YearProductionValueProgressPercent,
                    TotalYearProductionValue = x.TotalYearProductionValue
                }).ToListAsync();

            var baseCompanyShipBuildInfo = await _dbContext.Queryable<ExcelCompanyShipBuildInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyShipBuildInfo()
                {
                    Name = x.ShipTypeName,
                    AssignCount = x.AssignCount,
                    AwaitCount = x.AwaitCount,
                    BuildCount = x.BuildCount,
                    BuildPercent = x.BuildPercent,
                    Count = x.Count,
                    ReconditionCount = x.ReconditionCount
                })
                .ToListAsync();
            var baseCompanyShipProductionValueInfo = await _dbContext.Queryable<ExcelCompanyShipProductionValueInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyShipProductionValueInfo()
                {
                    Name = x.ShipTypeName,
                    DayProductionValue = x.DayProductionValue,
                    DayTurnHours = x.DayTurnHours,
                    TimePercent = x.TimePercent,
                    YearTotalProductionValue = x.YearTotalProductionValue,
                    YearTotalTurnHours = x.YearTotalTurnHours
                })
                .ToListAsync();
            var baseShipProductionValue = await _dbContext.Queryable<ExcelShipProductionValue>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.ShipProductionValue()
                {
                    ShipName = x.ShipName,
                    ShipDayOutput = x.ShipDayOutput,
                    ShipYearOutput = x.ShipYearOutput,
                    TimePercent = x.TimePercent
                })
                .ToListAsync();

            var baseSpecialProjectInfo = await _dbContext.Queryable<ExcelSpecialProjectInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.SpecialProjectInfo()
                {
                    SourceMatter = x.SourceMatter,
                    Type = x.Type,
                    TypeDesc = x.Type == 1 ? "1异常预警" : x.Type == 2 ? "嘉奖通报" : "提醒事项",
                    Description = x.Description
                }).ToListAsync();
               if (baseSpecialProjectInfo == null|| baseSpecialProjectInfo.Count==0)
                {
                    baseSpecialProjectInfo.Add(new SpecialProjectInfo()
                    {
                        SourceMatter = "无",
                        Type = 1,
                        Name = "111",
                        Description = "无"
                    });
                }
               
            var baseCompanyWriteReportInfo = await _dbContext.Queryable<ExcelCompanyWriteReportInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyWriteReportInfo()
                {
                    Name = x.CompanyName,
                    OnBulidCount = x.OnBulidCount,
                    QualityLevel = x.QualityLevel,
                    UnReportCount = x.UnReportCount,
                    WritePercent = x.WritePercent
                })
                .ToListAsync();
            var baseCompanyUnWriteReportInfo = await _dbContext.Queryable<ExcelCompanyUnWriteReportInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyUnWriteReportInfo()
                {
                    Name = x.UnitName,
                    Count = x.Count,
                    ProjectName = x.ProjectName,
                })
                .ToListAsync();

            foreach (var item in baseCompanyUnWriteReportInfo) {

                string xingxing = string.Empty;
                for (var i = 1; i <= item.Count; i++) {

                    xingxing += "★";
                }
                item.unCount = xingxing;
            }

            var baseCompanyShipUnWriteReportInfo = await _dbContext.Queryable<ExcelCompanyShipUnWriteReportInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new model.CompanyShipUnWriteReportInfo()
                {
                    Name = x.ShipName,
                    ShipName = x.ShipName,
                    OnProjectName = x.OnProjectName
                })
                .ToListAsync();
            var baseProjectShiftProductionInfo = await _dbContext.Queryable<ExcelProjectShiftProductionInfo>()
             .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
             .Select(x => new ProjectShiftProductionInfo()
             {
                 ProjectName = x.ProjectName,
                 ConstructionDeviceNum = x.ConstructionDeviceNum,
                 FewLandWorkplace = x.FewLandWorkplace,
                 HazardousConstructionDescription = x.HazardousConstructionDescription,
                 HazardousConstructionNum = x.HazardousConstructionNum.Value,
                 LandWorkplace = x.LandWorkplace,
                 OnShipPersonNum = x.OnShipPersonNum,
                 ShiftLeader = x.ShiftLeader,
                 ShiftPhone = x.ShiftPhone,
                 SiteConstructionPersonNum = x.SiteConstructionPersonNum,
                 SiteManagementPersonNum = x.SiteManagementPersonNum,
                 SiteShipNum = x.SiteShipNum
             })
             .ToListAsync();
            var baseUnProjectShitInfo = await _dbContext.Queryable<ExcelUnProjectShitInfo>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .Select(x => new UnProjectShitInfo()
                {
                    CompanyName = x.UnitName,
                    ProjectName = x.ProjectName
                })
                .ToListAsync();
            var baseExcelTitle = await _dbContext.Queryable<ExcelTitle>()
                .Where(x => x.IsDelete == 1 && x.Year == year && x.Month == month)
                .ToListAsync();
            #endregion

            productionDayReportHistoryResponseDto.CompanyProjectBasePoduction = baseProjectInfo;
            productionDayReportHistoryResponseDto.CompanyBasePoductionValue = baseProjectProductionValue;
            productionDayReportHistoryResponseDto.CompanyShipBuildInfo = baseCompanyShipBuildInfo;
            productionDayReportHistoryResponseDto.CompanyShipProductionValueInfo = baseCompanyShipProductionValueInfo;
            productionDayReportHistoryResponseDto.SpecialProjectInfo = baseSpecialProjectInfo;
            productionDayReportHistoryResponseDto.CompanyWriteReportInfo = baseCompanyWriteReportInfo;
            productionDayReportHistoryResponseDto.CompanyUnWriteReportInfo = baseCompanyUnWriteReportInfo;
            productionDayReportHistoryResponseDto.CompanyShipUnWriteReportInfo = baseCompanyShipUnWriteReportInfo;
            productionDayReportHistoryResponseDto.ShipProductionValue = baseShipProductionValue;
            productionDayReportHistoryResponseDto.ProjectShiftProductionInfo = baseProjectShiftProductionInfo;
            productionDayReportHistoryResponseDto.UnProjectShitInfo = baseUnProjectShitInfo;
            productionDayReportHistoryResponseDto.ExcelTitle = baseExcelTitle;
            productionDayReportHistoryResponseDtos.Add(productionDayReportHistoryResponseDto);

            responseAjaxResult.Data = productionDayReportHistoryResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}
