using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectWBSUpload;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan;
using GHMonitoringCenterApi.Application.Contracts.Dto.Word;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.File;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.IService.ResourceManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.Word;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using MiniExcelLibs;
using NPOI.SS.UserModel;
using NSwag.Annotations;
using SqlSugar;
using System.Web;
using UtilsSharp;

namespace GHMonitoringCenterApi.Controllers.File
{


    /// <summary>
    /// 文件导出相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("导出/导入/")]
    [Authorize]
    public class FileController : BaseController
    {



        #region 依赖注入
        public IFileService fileService { get; set; }
        public IWordService wordService { get; set; }
        public IMapper mapper { get; set; }

        public IEquipmentManagementService equipmentManagementService { get; set; }
        public IResourceManagementService resourceManagementService { get; set; }
        public IProjectService projectService { get; set; }
        public IProjectReportService projectReportService { get; set; }
        public IProjectProductionReportService projectProductionReportService { get; set; }
        public ILogService logService { get; set; }
        private IBaseService baseService { get; set; }
        public IShipPlanService shipPlanService { get; set; }
        public IBaseLinePlanProjectService baseLinePlanProjectService { get; set; }
        public FileController(IFileService fileService, IWordService wordService, IMapper mapper, IEquipmentManagementService equipmentManagementService, IResourceManagementService resourceManagementService, IProjectService projectService, IProjectReportService projectReportService, IProjectProductionReportService projectProductionReportService, ILogService logService, IBaseService baseService, IShipPlanService shipPlanService, IBaseLinePlanProjectService baseLinePlanProjectService)
        {
            this.fileService = fileService;
            this.wordService = wordService;
            this.mapper = mapper;
            this.equipmentManagementService = equipmentManagementService;
            this.resourceManagementService = resourceManagementService;
            this.projectService = projectService;
            this.projectReportService = projectReportService;
            this.projectProductionReportService = projectProductionReportService;
            this.logService = logService;
            this.baseService = baseService;
            this.shipPlanService = shipPlanService;
            this.baseLinePlanProjectService = baseLinePlanProjectService;
        }
        #endregion

        #region 项目导出excel（此方式需要落地）
        /// <summary>
        /// 项目导出excel（此方式需要落地）
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectExcelImport")]
        public async Task<ResponseAjaxResult<string>> ProjectExcelImportAsync([FromQuery] ProjectSearchRequestDto projectSearchRequestDto)
        {
            ResponseAjaxResult<string> responseAjaxResult = new ResponseAjaxResult<string>();
            var dataSource = await projectService.SearchProjectAsync(projectSearchRequestDto);

            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/项目业务数据/项目信息清单/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion

            if (dataSource.Data != null && dataSource.Data.Any())
            {
                var fileName = $"{Guid.NewGuid()}.xlsx";
                responseAjaxResult.Data = await fileService.ExcelImportSaveAsync<List<ProjectResponseDto>>(fileName, null, dataSource.Data);
                responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 项目导出excel（此方式不需要落地）
        /// <summary>
        /// 项目导出excel（此方式不需要落地）
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectImport")]
        public async Task<IActionResult> ExcelImportAsync([FromQuery] ProjectSearchRequestDto projectSearchRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/项目业务数据/项目信息清单/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置      
            var templatePath = $"Template/Excel/ProjectTemplate.xlsx";
            //var templatePath = @"D:\szghApi\ghmonitoringcenterapi\GHMonitoringCenterApi.Domain.Shared\Template\Excel\ProjectTemplate.xlsx";
            //获取数据
            var data = await projectService.GetProjectExcelAsync(projectSearchRequestDto);
            return await ExcelTemplateImportAsync(templatePath, data.Data, "广航局项目信息清单");
            //List<ProjectExcelImportResponseDto> projectExcelList = null;
            //List<string> ignoreColumns = null;
            //projectExcelList = mapper.Map<List<ProjectExcelSearchResponseDto>, List<ProjectExcelImportResponseDto>>(data.Data.projectExcelData);
            //if (!string.IsNullOrWhiteSpace(projectSearchRequestDto.ColumnsStr))
            //{
            //    ignoreColumns = projectSearchRequestDto.ColumnsStr.SplitStr(",").ToList();
            //}
            //var baseService = Request.HttpContext.RequestServices.GetService<IBaseService>();
            //var result = baseService.SearchProjectImportColumns();
            //if (result.Data != null && result.Data.Any())
            //{
            //    if (ignoreColumns != null)
            //    {
            //        var ignoreList = result.Data.Where(x => !ignoreColumns.Contains(x.Code)).ToList();
            //        ignoreColumns.Clear();
            //        if (ignoreList.Any())
            //        {
            //            foreach (var item in ignoreList)
            //            {
            //                ignoreColumns.Add(item.Code);
            //            }
            //        }
            //    }
            //}
            //var importData = data.Data;
            //    new
            //{
            //    projectExcelData = data.Data
            //}; 
            //return await ExcelImportAsync<List<ProjectExcelImportResponseDto>>(projectExcelList, ignoreColumns, "广航局项目信息清单");
        }
        #endregion

        /// <summary>
        /// 导出 html转换文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("ExportHtmlToFile")]
        public async Task<IActionResult> ExportHtmlToFileAsync(HtmlConvertFileRequestDto model)
        {
            var ressult = await fileService.ExportHtmlToFileAsync(model);
            return File(ressult.Data.Buffer, Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }

        #region 产值日报Excel导出
        /// <summary>
        /// 产值日报Excel导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("ExcelDayImport")]
        public async Task<IActionResult> ExcelDayImportAsync([FromQuery] ProductionSafetyRequestDto searchRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/在建项目日报数据/产值日报/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion

            //默认全量导出
            searchRequestDto.IsFullExport = true;
            string templatePath;
            //获取数据
            var data = await projectProductionReportService.SearchDayReportExcelAsync(searchRequestDto);
            if (data.Data.dayReportExcels.Select(x => x.IsHoliday).Contains(true))
            {
                //模板位置
                templatePath = $"Template/Excel/HolidayDayReportTemplate.xlsx";
                //templatePath = @"D:\GHJ1007\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Excel\HolidayDayReportTemplate.xlsx";


            }
            else
            {
                //模板位置
                templatePath = $"Template/Excel/DayReportTemplate.xlsx";
                // templatePath = @"D:\GHJ1007\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Excel\DayReportTemplate.xlsx";
            }
            var importData = new
            {
                dateTime = data.Data.TimeValue,
                DayReportInfo = data.Data.dayReportExcels
            };
            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.dateTime}产值日报");
        }
        #endregion

        #region 船舶日报Excel导出
        /// <summary>
        /// 船舶日报Excel导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("ExcelShipImport")]
        public async Task<IActionResult> ExcelShipImportAsync([FromQuery] ShipDailyRequestDto searchRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/在建项目日报数据/船舶日报/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置
            var templatePath = $"Template/Excel/ShipReportTemplate.xlsx";
            //默认全量导出
            searchRequestDto.IsFullExport = true;
            //获取数据
            var data = await projectProductionReportService.SearchShipDayReportAsync(searchRequestDto);
            var importData = new
            {
                dateTime = data.Data.TimeValue,
                ShipDayReportInfo = data.Data.shipsDayReportInfos
            };
            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.dateTime}船舶日报");
        }
        #endregion

        #region 安监日报Excel导出
        /// <summary>
        /// 安监日报Excel导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("ExcelSafeImport")]
        public async Task<IActionResult> ExcelSafeImportAsync([FromQuery] ProductionSafetyRequestDto searchRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/在建项目日报数据/安监日报/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置
            var templatePath = $"Template/Excel/SafeReportTemplate.xlsx";
            //默认全量导出
            searchRequestDto.IsFullExport = true;
            //获取数据
            var data = await projectProductionReportService.SearchSafeDayReportAsync(searchRequestDto);
            var importData = new
            {
                dateTime = data.Data.TimeValue,
                SafeDayReportInfo = data.Data.safeDayReportInfos
            };
            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.dateTime}安监日报");
        }
        #endregion

        #region 项目月报Excel导出
        /// <summary>
        /// 项目月报Excel导出
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ExcelMonthreport")]
        public async Task<IActionResult> ExcelMonthreportAsync([FromQuery] MonthtReportsRequstDto searchRequestDto)
        {
            searchRequestDto.IsFullExport = true;
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/在建项目生产月报/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置      
            //var templatePath = $@"E:\project\HNKC.SZGHAPI\szgh\ghmonitoringcenterapi\GHMonitoringCenterApi.Domain.Shared\Template\Excel\ProjectMonthly.xlsx";
            var templatePath = $"Template/Excel/ProjectMonthly.xlsx";
            var startMonth = searchRequestDto.StartTime?.ToString("yyyy-MM");// ((DateTime)searchRequestDto.StartTime).ToString("yyyy-MM");
            var endMonth = searchRequestDto.EndTime?.ToString("yyyy-MM");// ((DateTime)searchRequestDto.EndTime).ToString("yyyy-MM");
            var title = startMonth == endMonth ? $"{startMonth}月在建项目报表" : $"{startMonth}~{endMonth}月在建项目报表";
            //获取数据
            var result = await projectReportService.SearchMonthReportsAsync(searchRequestDto);
            var fileName = title;
            return await ExcelTemplateImportAsync(templatePath, new { MonthtreportData = result.Data.Reports, Title = title }, fileName);
        }
        #endregion

        #region 项目组织结构传入
        /// <summary>
        /// 项目组织结构传入
        /// </summary>
        /// <returns></returns>
        [HttpPost("ExcelProjectWBSAfferent")]
        public async Task<ResponseAjaxResult<List<ProjectWBSUpload>>> ExcelProjectWBSAfferentAsync()
        {
            ResponseAjaxResult<List<ProjectWBSUpload>> responseAjaxResult = new ResponseAjaxResult<List<ProjectWBSUpload>>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamUpdate = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamUpdate.Name;
                var rows = MiniExcel.Query<ProjectWBSUpload>(path, startCell: "A3").ToList();
                var projectWBSUploadsList = rows.Where(x => x.Number != null).ToList();
                if (projectWBSUploadsList.Any())
                {
                    var projectWBSU = new ProjectWBSUploadRequestDto()
                    {
                        //ProjectId = basePrimaryRequestDto.Id,
                        projectWBSUploads = projectWBSUploadsList
                    };
                    var insert = await baseService.InsertProjectWBSAsync(projectWBSU);
                    responseAjaxResult.Data = insert.Data;
                }
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOAD_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 项目组织结构模板导出
        /// <summary>
        /// 项目组织结构模板导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcelProjectWBSTemplate")]
        public async Task<IActionResult> ExcelProjectWBSTemplateAsync()
        {
            var templatePath = $"Template/Excel/ProjectWBSTemplate.xlsx";
            var importData = new
            {
                year = DateTime.Now.Year,
                month = DateTime.Now.Month,
                day = DateTime.Now.Day,
            };
            return await ExcelTemplateImportAsync(templatePath, importData, "项目组织结构模板");
        }
        #endregion

        #region 未填报项目产值日报、安检日报和船舶日报导出

        /// <summary>
        /// 未填项目日报的项目数据导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("UnReportProjectExport")]
        public async Task<IActionResult> UnReportProjectsExportAsync([FromQuery] UnReportProjectsRequestDto unReportProjectsRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/未填项目日报的项目数据//导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置
            var templatePath = $"Template/Excel/UnReportProjectTemplate.xlsx";
            //获取数据
            var dataResult = await projectReportService.SearchUnReportProjectsAsync(unReportProjectsRequestDto);
            var fileName = "未填报项目日报的项目列表";
            var title = "未填报项目日报的项目列表";
            if (unReportProjectsRequestDto.StartTime.HasValue && unReportProjectsRequestDto.EndTime.HasValue)
                title += $"{unReportProjectsRequestDto.StartTime.Value.ToString("yyyy-MM-dd")}~{unReportProjectsRequestDto.EndTime.Value.ToString("yyyy-MM-dd")}";
            return await ExcelTemplateImportAsync(templatePath, new { title, ReportInfo = dataResult.Data }, fileName);
        }

        /// <summary>
        /// 未填安检日报的项目数据导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("UnReportSafeExport")]
        public async Task<IActionResult> UnReportSafeExportAsync([FromQuery] UnReportProjectsRequestDto unReportProjectsRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/未填安检日报的项目数据/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置
            var templatePath = $"Template/Excel/UnReportProjectTemplate.xlsx";
            //获取数据
            var dataResult = await projectReportService.SearchUnReportSafeProjectsAsync(unReportProjectsRequestDto);
            var fileName = "未填报安检日报的项目列表";
            var title = "未填报安检日报的项目列表";
            if (unReportProjectsRequestDto.StartTime.HasValue && unReportProjectsRequestDto.EndTime.HasValue)
                title += $"{unReportProjectsRequestDto.StartTime.Value.ToString("yyyy-MM-dd")}~{unReportProjectsRequestDto.EndTime.Value.ToString("yyyy-MM-dd")}";
            return await ExcelTemplateImportAsync(templatePath, new { title, ReportInfo = dataResult.Data }, fileName);
        }
        /// <summary>
        /// 未填船舶日报的船舶数据导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("UnReportShipExport")]
        public async Task<IActionResult> UnReportShipExportAsync([FromQuery] UnReportShipsRequestDto unReportShipsRequestDto)
        {

            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/未填船舶日报的船舶数据/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion

            //模板位置
            var templatePath = $"Template/Excel/UnReportShipTemplate.xlsx";
            //获取数据
            var dataResult = await projectReportService.SearchUnReportShipsAsync(unReportShipsRequestDto);
            var fileName = "未填船舶日报的船舶列表";
            var title = "未填船舶日报的船舶列表";
            if (unReportShipsRequestDto.StartTime.HasValue && unReportShipsRequestDto.EndTime.HasValue)
                title += $"{unReportShipsRequestDto.StartTime.Value.ToString("yyyy-MM-dd")}~{unReportShipsRequestDto.EndTime.Value.ToString("yyyy-MM-dd")}";

            return await ExcelTemplateImportAsync(templatePath, new { title, ReportInfo = dataResult.Data }, fileName);
        }


        #endregion

        #region 船舶月报导出
        /// <summary>
        /// 自有船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("ExcelOwnShipMonthRep")]
        public async Task<IActionResult> NotFillOwnShipExcelAsync([FromQuery] MonthRepRequestDto requestDto)
        {
            //模板位置
            //var templatePath = $@"F:\GDC.WOM\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Excel\OwnShipMonthRep.xlsx";
            var templatePath = $"Template/Excel/OwnShipMonthRep.xlsx";
            #region 日期控制
            //开始日期结束日期都是空  默认按当天日期匹配
            if ((requestDto.InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InStartDate.ToString())) && (requestDto.InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InEndDate.ToString())))
            {
                if (DateTime.Now.Day >= 26)
                {
                    requestDto.InStartDate = DateTime.Now;
                    requestDto.InEndDate = DateTime.Now;
                }
                else
                {
                    requestDto.InStartDate = DateTime.Now.AddMonths(-1);
                    requestDto.InEndDate = DateTime.Now.AddMonths(-1);
                }
            }
            #endregion
            var title = requestDto.InEndDate.Value.ToString("yyyy年MM月");
            //获取数据
            var dataResult = await projectReportService.OwnShipMonthRepExcel(requestDto);
            var importData = new
            {
                Title = title,
                OMR = dataResult.Data.OwnShipMonthRepExcelDtos
            };
            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.Title}自有船舶月报");
        }
        /// <summary>
        /// 分包船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("ExcelSubShipMonthRep")]
        public async Task<IActionResult> NotFillSubShipExcelAsync([FromQuery] MonthRepRequestDto requestDto)
        {
            //模板位置
            //var templatePath = $@"F:\GDC.WOM\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Excel\SubShipMonthRep.xlsx";
            var templatePath = $"Template/Excel/SubShipMonthRep.xlsx";
            #region 日期控制
            //开始日期结束日期都是空  默认按当天日期匹配
            if ((requestDto.InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InStartDate.ToString())) && (requestDto.InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InEndDate.ToString())))
            {
                if (DateTime.Now.Day >= 26)
                {
                    requestDto.InStartDate = DateTime.Now;
                    requestDto.InEndDate = DateTime.Now;
                }
                else
                {
                    requestDto.InStartDate = DateTime.Now.AddMonths(-1);
                    requestDto.InEndDate = DateTime.Now.AddMonths(-1);
                }
            }
            #endregion
            var title = requestDto.InEndDate.Value.ToString("yyyy年MM月");
            //获取数据
            var dataResult = await projectReportService.SubShipMonthRepExcel(requestDto);
            var importData = new
            {
                Title = title,
                SMR = dataResult.Data.SubShipMonthRepExcelDtos
            };
            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.Title}分包船舶月报");
        }
        #endregion

        #region 未填项目月报的未填月报数据导出
        /// <summary>
        /// 未填项目月报的未填月报数据导出
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("UnMonthReportProjectExport")]
        public async Task<IActionResult> UnMonthReportProjectExportAsync([FromQuery] ProjectMonthRepRequestDto projectMonthRepRequestDto)
        {

            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/未填项目月报的未填月报数据导出/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion

            //模板位置
            var templatePath = $"Template/Excel/UnMonthReportProjectTemplate.xlsx";
            //获取数据
            projectMonthRepRequestDto.IsFullExport = true;
            var dataResult = await projectReportService.GetSearchNotFillProjectMonthRepAsync(projectMonthRepRequestDto);
            var fileName = "未填项目月报的项目列表";
            var title = "未填项目月报的项目列表";
            if (projectMonthRepRequestDto.StartTime.HasValue && projectMonthRepRequestDto.EndTime.HasValue)
            {
                if (projectMonthRepRequestDto.StartTime.Value.ToDateDay() == projectMonthRepRequestDto.EndTime.Value.ToDateDay())
                {
                    title += projectMonthRepRequestDto.StartTime.Value.ToString("yyyy-MM");
                }
                else
                {
                    title += $"{projectMonthRepRequestDto.StartTime.Value.ToString("yyyy-MM")}~{projectMonthRepRequestDto.EndTime.Value.ToString("yyyy-MM")}";
                }
            }
            return await ExcelTemplateImportAsync(templatePath, new { title, ReportInfo = dataResult.Data }, fileName);
        }
        #endregion

        #region 在建项目月报产报导出
        /// <summary>
        /// 在建项目月报产报导出
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ExcelMonthImport")]
        public async Task<IActionResult> ProjectMonthExcelAsync([FromQuery] ProjectMonthReportRequestDto searchRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/在建项目月报数据/编辑/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置
            var templatePath = $"Template/Excel/ProjectMonthRepWbsTemplate.xlsx";
            //var templatePath = "D:\\GDCWOM\\wom.api\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProjectMonthRepWbsTemplate.xlsx";
            //获取数据
            var data = await projectReportService.ProjectMonthExcelAsync(searchRequestDto);
            var importData = new
            {
                dateTime = data.Data.TimeValue,
                ProjectOverInfo = data.Data.projectOverall,
                ProjectWbsInfo = data.Data.projectWbsDatas,
            };
            return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.dateTime.Year}年{importData.dateTime.Month}月在建项目月报产报构成");
        }

        #endregion

        #region 产值产报汇总
        /// <summary>
        /// 产值产报汇总
        /// </summary>
        /// <param name="putExcelRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ExcelProjectOutPut")]
        public async Task<IActionResult> SearchProjectOutPutExcelAsync([FromQuery] ProjectOutPutExcelRequestDto putExcelRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/在建项目月报数据/产值日报/产值产量导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            //模板位置
            //var templatePath = $"Template/Excel/ProjectMonthOutPutTemplate.xlsx";
            //var templatePath = $@"E:\project\HNKC.SZGHAPI\szgh\ghmonitoringcenterapi\GHMonitoringCenterApi.Domain.Shared\Template\Excel\ProjectMonthOutPutTemplate.xlsx";
            ////获取数据
            //var data = await projectReportService.SearchProjectOutPutExcelAsync(putExcelRequestDto);
            //var importData = new
            //{
            //    dateTime = data.Data.TimeValue,
            //    SumList = data.Data.sumOutPutInfos,
            //    AllList = data.Data.outPutInfos
            //};
            //return await ExcelTemplateImportAsync(templatePath, importData, $"{importData.dateTime}产值产量汇总");

            var bytes = await projectReportService.SearchProjectOutPutNpoiExcelAsync(putExcelRequestDto);
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"产值产报汇总.xlsx", System.Text.Encoding.UTF8)}");
            return new FileStreamResult(new MemoryStream(bytes), Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }
        #endregion

        #region 项目月报简报导出Word
        /// <summary>
        /// 项目月报简报导出Word
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectMonthReportImportWord")]
        public async Task<IActionResult> ProjectMonthReportImportWordAsync([FromQuery] MonthtReportsRequstDto model)
        {
            //获取logo图片
            //测试或生产地址
            //var templatePath = @$"D:\projectconllection\dotnet\szgh\GHMonitoringCenterApi.Domain.Shared\Template\Images\logo.png";
            //本机地址
            //var templatePath = @$"D:\SZGH\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Images\logo.png";

            var templatePath = $"Template/Images/logo.png";
            BaseConfig config = new BaseConfig()
            {
                Foot = "仿宋",
                Size = 28,
                Title = "在建项目月度简报",
                Time = DateTime.Now.ToString("yyyy年MM月"),
                SubTitle = "生产运营管理部",
                SubTime = DateTime.Now.ToString("yyyy年MM月"),
                WordImageSetup = new WordImageSetup()
                {
                    type = PictureType.PNG,
                    FileName = Path.GetFileName(templatePath),
                    LogoStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read)
                }
            };
            var stream = await wordService.MonthReportImportWordAsync(config, model);
            return await WordTemplateImportAsync(stream, "项目月报简报导出");

        }
        #endregion

        #region 船舶滚动计划表
        /// <summary>
        /// 船舶滚动计划表
        /// </summary>
        /// <returns></returns>
        [HttpGet("ShipRollingPlanExport")]
        public async Task<IActionResult> ShipRollingPlanExportAsync()
        {
            var templatePath = $"Template/Excel/ShipRollingPlanTemplate.xlsx";
            var searchShipRepairRolling = await resourceManagementService.SearchShipRepairRolling();
            var importData = new
            {
                title = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日船舶修理滚动计划表",
                ShipRollingPlan = searchShipRepairRolling.Data,
            };
            var fileName = importData.title;
            return await ExcelTemplateImportAsync(templatePath, importData, fileName);
        }
        #endregion

        #region 项目计划Excel导出
        /// <summary>
        /// 项目计划Excel导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectPlanExport")]
        public async Task<IActionResult> ProjectPlanExcelExportAsync([FromQuery] MonthlyPlanRequestDto monthlyPlanRequestDto)
        {
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/进度与成本管控/月度计划产值管理/导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            var templatePath = $"Template/Excel/ProjectMonthPlanTemplate.xlsx";
            //var templatePath = "D:\\GHJWOM\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProjectMonthPlanTemplate.xlsx";
            //默认全量导出
            monthlyPlanRequestDto.IsFullExport = true;
            var list = await projectService.SearchProjectPlanAsync(monthlyPlanRequestDto);
            var importData = new
            {
                title = DateTime.Now.Year + "年项目月度维护计划",
                ProjectPlan = list.Data.projectSearchInfos
            };
            return await ExcelTemplateImportAsync(templatePath, importData, importData.title);
        }
        #endregion

        #region 项目计划导入模板下载
        /// <summary>
        /// 项目计划导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectPlanDownloadTemplate")]
        public async Task<IActionResult> ProjectPlanDownloadTemplateAsync()
        {
            var templatePath = $"Template/Excel/ProjectMonthPlanTemplate.xlsx";
            //var templatePath = "D:\\GHJWOM\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProjectMonthPlanTemplate.xlsx";
            var importData = new
            {
                title = DateTime.Now.Year + "年项目月度维护计划",
                ProjectPlan = new List<ProjectPlanResponseDto>()
            };
            return await ExcelTemplateImportAsync(templatePath, importData, importData.title);
        }
        #endregion

        #region 项目计划Excel导入
        /// <summary>
        /// 项目计划Excel导入
        /// </summary>
        /// <returns></returns>
        [HttpPost("ProjectPlanImport")]
        public async Task<ResponseAjaxResult<bool>> ImportExcelProjectPlan()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamData = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamData.Name;
                var rows = MiniExcel.Query<ProjectPlanInfo>(path, startCell: "A3").ToList();
                if (rows != null)
                {
                    var insert = await projectService.SaveProjectPlanExcelImportAsync(rows);
                    if (insert.Data)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
                    }
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 项目计划Excel导入
        /// <summary>
        /// 项目计划Excel导出
        /// </summary>
        /// <returns></returns>
        [HttpPost("DevicePlanImport")]
        public async Task<ResponseAjaxResult<bool>> DeviceExcelProjectPlan()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamData = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamData.Name;
                var rows = MiniExcel.Query<ProjectPlanInfo>(path, startCell: "A3").ToList();
                if (rows != null)
                {
                    var insert = await projectService.SaveProjectPlanExcelImportAsync(rows);
                    if (insert.Data)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
                    }
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 非自有设备
        /// <summary>
        /// 非自有设备Excel导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("EquipmentManagementExport")]
        public async Task<IActionResult> EquipmentManagementExportAsync([FromQuery] SearchEquipmentManagementRequestDto searchEquipmentManagementRequestDto)
        {
            #region 记录日志
            //var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            //var logObj = new LogInfo()
            //{
            //    Id = GuidUtil.Increment(),
            //    BusinessModule = "/进度与成本管控/月度计划产值管理/导出",
            //    BusinessRemark = "导出",
            //    OperationId = CurrentUser.Id,
            //    OperationName = CurrentUser.Name,
            //    OperationType = 4
            //};
            //await logService.WriteLogAsync(logObj);
            #endregion
            var templatePath = $"Template/Excel/DeviceExportTemplate.xlsx";
            //var templatePath = @"D:\GHJ1007\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Excel\DeviceExportTemplate.xlsx";
            //默认全量导出
            //baseRequestDto.IsFullExport = true;
            var list = await equipmentManagementService.SearchEquipmentManagementExportAsync(searchEquipmentManagementRequestDto);
            var importData = new
            {
                title = list.Data.Title,
                exportMarineEquipment = list.Data.exportMarineEquipment,
                exportLandEquipment = list.Data.exportLandEquipment,
                exportSpecialEquipment = list.Data.exportSpecialEquipment
            };
            return await ExcelTemplateImportAsync(templatePath, importData, importData.title);
        }
        /// <summary>
        /// 非自有设备导入-水上设备
        /// </summary>
        /// <returns></returns>
        [HttpPost("MarineEquipmentImport")]
        public async Task<ResponseAjaxResult<bool>> MarineEquipmentImportAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamData = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamData.Name;
                var rows = MiniExcel.Query<ExportMarineEquipment>(path, startCell: "A3").ToList();
                if (rows != null)
                {
                    var insert = await equipmentManagementService.EquipmentManagementImport(rows.Where(x => x.ReportingMonth != null).ToList());
                    if (insert.Data)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
                    }
                    else
                    {
                        return insert;
                    }
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 非自有设备导入-陆域设备
        /// </summary>
        /// <returns></returns>
        [HttpPost("LandEquipmentImport")]
        public async Task<ResponseAjaxResult<bool>> LandEquipmentImportAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamData = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamData.Name;
                var rows = MiniExcel.Query<ExportLandEquipment>(path, startCell: "A3").ToList();
                if (rows != null)
                {
                    var insert = await equipmentManagementService.ExportLandEquipmentImport(rows);
                    if (insert.Data)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
                    }
                    else
                    {
                        return insert;
                    }
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 非自有设备导入-特种设备
        /// </summary>
        /// <returns></returns>
        [HttpPost("SpecialEquipmentImport")]
        public async Task<ResponseAjaxResult<bool>> SpecialEquipmentImportAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamData = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamData.Name;
                var rows = MiniExcel.Query<ExportSpecialEquipment>(path, startCell: "A3").ToList();
                if (rows != null)
                {
                    var insert = await equipmentManagementService.ExportSpecialEquipmentImport(rows);
                    if (insert.Data)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
                    }
                    else
                    {
                        return insert;
                    }
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 非自有设模板导出
        /// <summary>
        /// 非自有设模板导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcelDeviceTemplate")]
        public async Task<IActionResult> ExcelDeviceTemplateAsync(int type)
        {
            string templatePath = null;
            var importData = new
            {
                year = DateTime.Now.Year,
                month = DateTime.Now.Month,
                day = DateTime.Now.Day,
            };
            if (type == 1) //水上设备
            {
                templatePath = $"Template/Excel/ExportMarineEquipmentImport.xlsx";
                //templatePath = @"Template\Excel\ExportMarineEquipmentImport.xlsx";
            }
            else if (type == 2) // 陆域设备
            {
                templatePath = $"Template/Excel/ExportLandEquipmentImport.xlsx";
                //templatePath = @"Template\Excel\ExportLandEquipmentImport.xlsx";
            }
            else  // 特种设备
            {
                templatePath = $"Template/Excel/ExportSpecialEquipmentImport.xlsx";
                //templatePath = @"Template\Excel\ExportSpecialEquipmentImport.xlsx";
            }
            return await ExcelTemplateImportAsync(templatePath, importData, "非自有设模板导出");
        }
        #endregion

        #region 修理备件管理模板导出
        /// <summary>
        /// 修理备件管理模板导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("SparePartsManagementTemplate")]
        public async Task<IActionResult> SparePartsManagementTemplateAsync(int type)
        {
            string templatePath = null;
            var importData = new
            {
                year = DateTime.Now.Year,
                month = DateTime.Now.Month,
                day = DateTime.Now.Day,
            };
            if (type == 1) //修理备件
            {
                templatePath = $"Template/Excel/RepairPartsTemplate.xlsx";
                //templatePath = @"Template\Excel\ExportMarineEquipmentImport.xlsx";
                return await ExcelTemplateImportAsync(templatePath, importData, "修理备件模板");
            }
            else if (type == 2) // 修理项目
            {
                templatePath = $"Template/Excel/SparePartProjectTemplate.xlsx";
                //templatePath = @"Template\Excel\ExportLandEquipmentImport.xlsx";
                return await ExcelTemplateImportAsync(templatePath, importData, "修理项目模板");
            }
            else if (type == 3) // 发船备件清单
            {
                templatePath = $"Template/Excel/SendShipSparePartTemplate.xlsx";
                //templatePath = @"Template\Excel\ExportSpecialEquipmentImport.xlsx";
                return await ExcelTemplateImportAsync(templatePath, importData, "发船备件清单模板");
            }
            else  // 备件仓储运输清单
            {
                templatePath = $"Template/Excel/SparePartStoragePartTemplate.xlsx";
                //templatePath = @"Template\Excel\ExportSpecialEquipmentImport.xlsx";
            }
            return await ExcelTemplateImportAsync(templatePath, importData, "备件仓储运输清单模板");
        }
        #endregion

        #region pdf导出
        /// <summary>
        /// pdf导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("WordConvertPdf")]
        //[AllowAnonymous]
        public async Task<IActionResult> WordConvertPdfAsync([FromQuery] MonthtReportsRequstDto model)
        {


            //获取logo图片
            //测试或生产地址
            //var templatePath = @$"D:\GHJ1007\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Images\logo.png";
            //本机地址
            //var templatePath = @$"D:\SZGH\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Images\logo.png";
            var templatePath = $"Template/Images/logo.png";
            BaseConfig config = new BaseConfig()
            {
                Foot = "仿宋",
                Size = 28,
                Title = "在建项目月度简报",
                Time = DateTime.Now.ToString("yyyy年MM月"),
                SubTitle = "生产运营管理部",
                SubTime = DateTime.Now.ToString("yyyy年MM月"),
                WordImageSetup = new WordImageSetup()
                {
                    type = PictureType.PNG,
                    FileName = Path.GetFileName(templatePath),
                    LogoStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read)
                }
            };
            Stream response = new MemoryStream();
            var stream = await wordService.MonthReportImportWordAsync1(config, model);
            //var path = @$"C:\Users\admin\Desktop\22222222\1.docx";
            var path = $"Template/Images/pdfExport.docx";
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                //读取文件流
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
                FormFile formFile = new FormFile(fs, 0, fs.Length, "项目月报简报导出", "项目月报简报导出.docx");
                var url = AppsettingsHelper.GetValue("Pdf:Url");
                var name = "fileInput";
                var fileName = formFile.FileName;
                var file = formFile.OpenReadStream();
                var formData = new MultipartFormDataContent();
                //var a = new StreamContent(file, (int)file.Length);
                formData.Add(new StreamContent(file, (int)file.Length), name, fileName);
                var _httpclient = new HttpClient();
                response = await _httpclient.PostAsync(url, formData).Result.Content.ReadAsStreamAsync();
            }
            System.IO.File.Delete(path);
            return await WordTemplateImportAsync(response, "项目月报简报导出", "pdf");
        }
        #endregion

        #region 导出船舶年度计划产值
        [HttpGet("ImportShipPlan")]
        public async Task<IActionResult> ImportShipPlanAsync([FromQuery] ShipPlanRequestDto shipPlanRequestDto)
        {

            //获取数据
            var result = await shipPlanService.SearchShipPlanAsync(shipPlanRequestDto);
            return await ExcelImportAsync<List<ShipPlanResponseDto>>(result.Data, null, $"{shipPlanRequestDto.Year}度船舶产值计划");
        }

        #endregion

    }
}