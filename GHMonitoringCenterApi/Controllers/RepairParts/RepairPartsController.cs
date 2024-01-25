using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Application.Contracts.IService.RepairParts;
using GHMonitoringCenterApi.Domain.Shared;
using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Application.Contracts.Dto.Role;
using GHMonitoringCenterApi.Application.Contracts.IService.RepairParts;
using GHMonitoringCenterApi.Application.Service.Role;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using System.Collections.Generic;
using UtilsSharp;
using NPOI.SS.Formula.Functions;
using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Service.Projects;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System.Web;
using GHMonitoringCenterApi.Application.Service.RepairParts;
using GHMonitoringCenterApi.Filters;

namespace GHMonitoringCenterApi.Controllers.RepairParts
{


    /// <summary>
    /// 修理备件控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RepairPartsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RepairPartsController> logger;
        public IRepairPartsService repairPartsService { get; set; }
        public RepairPartsController(IRepairPartsService repairPartsService , ILogger<RepairPartsController> logger,IMapper _mapper)
        {
            this.repairPartsService = repairPartsService;
            this._mapper = _mapper;
            this.logger = logger;
        }
        /// <summary>
        /// 获取发船备件清单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSendShipSparePartList")]
        public async Task<ResponseAjaxResult<List<SendShipSparePartListResponseDto>>> GetSendShipSparePartListAsync([FromQuery] SendShipSparePartListRequestDto sendShipSparePartListRequestDto)
        {
            return await repairPartsService.GetSendShipSparePartListAsync(sendShipSparePartListRequestDto);
        }
        /// <summary>
        /// 保存发船备件清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveSendShipSparePartList")]
        public async Task<ResponseAjaxResult<bool>> SaveSendShipSparePartListAsync([FromBody]SaveSendShipSparePartListRequestDto saveSendShipSparePartListRequestDto)
        {
            return await repairPartsService.SaveSendShipSparePartListAsync(saveSendShipSparePartListRequestDto);
        }
        /// <summary>
        /// 删除发船备件清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteSendShipSparePartList")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteSendShipSparePartListAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await repairPartsService.DeleteSendShipSparePartListAsync(basePrimaryRequestDto);
        }
        /// <summary>
        /// 获取备件仓储运输清单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSparePartStorageList")]
        public async Task<ResponseAjaxResult<List<GetSparePartStorageListResponseDto>>> GetSparePartStorageListAsync([FromQuery] GetSparePartStorageListRequestDto getSparePartStorageListRequestDto)
        {
            return await repairPartsService.GetSparePartStorageListAsync(getSparePartStorageListRequestDto);
        }
        /// <summary>
        /// 保存发船备件清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveSparePartStorageList")]
        public async Task<ResponseAjaxResult<bool>> SaveSparePartStorageListAsync([FromBody] SaveSparePartStorageListResponseDto saveSparePartStorageListResponseDto)
        {
            return await repairPartsService.SaveSparePartStorageListAsync(saveSparePartStorageListResponseDto);
        }
        /// <summary>
        /// 删除发船备件清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteSparePartStorageList")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteSparePartStorageListAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await repairPartsService.DeleteSparePartStorageListAsync(basePrimaryRequestDto);
        }
        /// <summary>
        /// 获取修理项目清单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRepairItemsList")]
        public async Task<ResponseAjaxResult<List<GetRepairItemsListResponseDto>>> GetRepairItemsListAsync([FromQuery] GetSparePartStorageListRequestDto getSparePartStorageListRequestDto)
        {
            return await repairPartsService.GetRepairItemsListAsync(getSparePartStorageListRequestDto);
        }
        /// <summary>
        /// 保存修理项目清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveRepairItemsList")]
        public async Task<ResponseAjaxResult<bool>> SaveRepairItemsListAsync([FromBody] SaveRepairItemsListRequestDto saveRepairItemsListRequestDto)
        {
            return await repairPartsService.SaveRepairItemsListAsync(saveRepairItemsListRequestDto);
        }
        /// <summary>
        /// 删除修理项目清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteRepairItemsList")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteRepairItemsListAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await repairPartsService.DeleteRepairItemsListAsync(basePrimaryRequestDto);
        }
        /// <summary>
        /// 获取备件项目清单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSparePartProjectList")]
        public async Task<ResponseAjaxResult<List<SparePartProjectListResponseDto>>> GetSparePartProjectListAsync([FromQuery] GetSparePartStorageListRequestDto getSparePartStorageListRequestDto)
        {
            return await repairPartsService.GetSparePartProjectListAsync(getSparePartStorageListRequestDto);
        }
        /// <summary>
        /// 保存备件项目清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveSparePartProjectList")]
        public async Task<ResponseAjaxResult<bool>> SaveSparePartProjectListAsync([FromBody] SaveSparePartProjectListRequestDto saveSparePartProjectListRequestDto)
        {
            return await repairPartsService.SaveSparePartProjectListAsync(saveSparePartProjectListRequestDto);
        }
        /// <summary>
        /// 删除备件项目清单
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteSparePartProjectList")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteSparePartProjectListAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await repairPartsService.DeleteSparePartProjectListAsync(basePrimaryRequestDto);
        }

        /// <summary>
        /// 新增修理备件
        /// </summary>
        /// <param name="excelFile">excel文件</param>
        /// <returns></returns>
        [HttpPost("ImportRepairParts")]
        public async Task<ResponseAjaxResult<bool>> ImportRepairPartsAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var fileResponse = await StreamUpdateFileAsync();
            if (fileResponse.Code == HttpStatusCode.Success)
            {
                List<RepairProjectList> projectList = new List<RepairProjectList>();
                try
                {//UpdateItem:SavePath
                    var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                    savePath = Path.Combine(savePath, fileResponse.Data.Name);
                    //开始读取excel文件
                    var repairProjectList = MiniExcel.Query<SparePartProjectListRequestDto>(savePath).ToList();
                    if (repairProjectList.Any())
                    {
                        projectList = _mapper.Map<List<SparePartProjectListRequestDto>, List<RepairProjectList>>(repairProjectList);
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                        return responseAjaxResult;
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "修理备件上传excel报错");
                    responseAjaxResult.Fail(ResponseMessage.DATA_EXCELERROR_FAIL, HttpStatusCode.UploadFail);
                    return responseAjaxResult;
                }
                return await repairPartsService.AddRepairPartsAsync(projectList);

            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 新增修理项目
        /// </summary>
        /// <param name="excelFile">excel文件</param>
        /// <returns></returns>
        [HttpPost("ImportSparePartProject")]
        public async Task<ResponseAjaxResult<bool>> ImportSparePartProjectAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var fileResponse = await StreamUpdateFileAsync();
            if (fileResponse.Code == HttpStatusCode.Success)
            {
                List<SparePartProjectList> projectList = new List<SparePartProjectList>();
                try
                {
                    var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                    savePath = Path.Combine(savePath, fileResponse.Data.Name);
                    //开始读取excel文件
                    var repairProjectList = MiniExcel.Query<ExcelSparePartProjectListRequseDto>(savePath).ToList();
                    if (repairProjectList.Any())
                    {
                        projectList = _mapper.Map<List<ExcelSparePartProjectListRequseDto>, List<SparePartProjectList>>(repairProjectList);
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                        return responseAjaxResult;
                    }

                }
                catch (Exception ex)
                {

                    responseAjaxResult.Fail(ResponseMessage.DATA_EXCELERROR_FAIL, HttpStatusCode.UploadFail);
                    return responseAjaxResult;
                }
                return await repairPartsService.AddSparePartProjectAsync(projectList);

            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 新增发船备件清单
        /// </summary>
        /// <param name="excelFile">excel文件</param>
        /// <returns></returns>
        [HttpPost("ImportSendShipSparePart")]
        public async Task<ResponseAjaxResult<bool>> ImportSendShipSparePartAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var fileResponse = await StreamUpdateFileAsync();
            if (fileResponse.Code == HttpStatusCode.Success)
            {
                List<SendShipSparePartList> projectList = new List<SendShipSparePartList>();
                try
                {
                    var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                    savePath = Path.Combine(savePath, fileResponse.Data.Name);
                    //开始读取excel文件
                    var sendShipSparePart = MiniExcel.Query<ExcelSendShipSparePartRequestDto>(savePath).ToList();
                    if (sendShipSparePart.Any())
                    {
                        projectList = _mapper.Map<List<ExcelSendShipSparePartRequestDto>, List<SendShipSparePartList>>(sendShipSparePart);
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                        return responseAjaxResult;
                    }

                }
                catch (Exception ex)
                {

                    responseAjaxResult.Fail(ResponseMessage.DATA_EXCELERROR_FAIL, HttpStatusCode.UploadFail);
                    return responseAjaxResult;
                }
                return await repairPartsService.AddSendShipSparePartAsync(projectList);

            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 新增备件仓储运输清单
        /// </summary>
        /// <param name="excelFile">excel文件</param>
        /// <returns></returns>
        [HttpPost("ImportSparePartStoragePart")]
        public async Task<ResponseAjaxResult<bool>> ImportSparePartStoragePartAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var fileResponse = await StreamUpdateFileAsync();
            if (fileResponse.Code == HttpStatusCode.Success)
            {
                List<SparePartStorageList> projectList = new List<SparePartStorageList>();
                try
                {
                    var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                    savePath = Path.Combine(savePath, fileResponse.Data.Name);
                    //开始读取excel文件
                    var sendShipSparePart = MiniExcel.Query<ExcelSparePartStorageRequerDto>(savePath).ToList();
                    if (sendShipSparePart.Any())
                    {
                        projectList = _mapper.Map<List<ExcelSparePartStorageRequerDto>, List<SparePartStorageList>>(sendShipSparePart);
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                        return responseAjaxResult;
                    }

                }
                catch (Exception ex)
                {

                    responseAjaxResult.Fail(ResponseMessage.DATA_EXCELERROR_FAIL, HttpStatusCode.UploadFail);
                    return responseAjaxResult;
                }
                return await repairPartsService.AddSparePartStoragePartAsync(projectList);

            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 修理备件导出Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportRepairParts")]
        public async Task<IActionResult> ExportRepairPartsAsync()
        {
             var obj=await repairPartsService.ImportRepairPartsStreamAsync();
             return  await ExcelImportAsync(obj.Data, null, "修理备件表格模版");
        }

        #region 自动统计导出

        /// <summary>
        /// 自动统计导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportExcelAutomaticParts")]
        public async Task<IActionResult> ExportExcelAutomaticPartsAsync()
        {
          var bytes = await   repairPartsService.ExportExcelAutomaticPartsAsync();
            #region 记录日志
            var logService = Request.HttpContext.RequestServices.GetService<ILogService>();
            var logObj = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/修理备件管理/自动统计导出",
                BusinessRemark = "导出",
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                OperationType = 4
            };
            await logService.WriteLogAsync(logObj);
            #endregion
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={HttpUtility.UrlEncode($"自动统计.xlsx", System.Text.Encoding.UTF8)}");
            return new FileStreamResult(new MemoryStream(bytes), Domain.Shared.Const.ContentType.APPLICATIONSTREAM);
        }

        /// <summary>
        /// 搜索自动统计列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAutomaticParts")]
        public async Task<ResponseAjaxResult<AutomaticPartsResponseDto>> SearchAutomaticPartsAsync([FromQuery] AutomaticPartsRequestDto model)
        {
            return await repairPartsService.SearchAutomaticPartsAsync(model);
        }
        #endregion

    }
}
