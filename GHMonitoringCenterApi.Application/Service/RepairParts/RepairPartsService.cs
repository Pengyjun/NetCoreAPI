using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Application.Contracts.Dto.Timing;
using GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts;
using GHMonitoringCenterApi.Application.Contracts.IService.RepairParts;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Mvc;
using NPOI.Util;
using NPOI.XWPF.UserModel;
using SqlSugar;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniExcelLibs;
using NPOI.SS.Formula.Functions;
using UtilsSharp;
using static NPOI.HSSF.Util.HSSFColor;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

namespace GHMonitoringCenterApi.Application.Service.RepairParts
{
    /// <summary>
    /// 修理备件管理接口实现层
    /// </summary>
    public class RepairPartsService : IRepairPartsService
    {

        #region 依赖注入
        public IBaseRepository<SendShipSparePartList> baseRepository { get; set; }
        public IBaseRepository<SparePartStorageList> sparePartStorageList { get; set; }
        public IBaseRepository<RepairProjectList> repairProjectList { get; set; }
        public IBaseRepository<SparePartProjectList> sparePartProjectList { get; set; }
        public ISqlSugarClient _dbContext;
        public IMapper mapper { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        public RepairPartsService(IBaseRepository<SendShipSparePartList> baseRepository, IMapper mapper, GlobalObject _globalObject, ISqlSugarClient _dbContext, IBaseRepository<SparePartStorageList> sparePartStorageList, IBaseRepository<RepairProjectList> repairProjectList, IBaseRepository<SparePartProjectList> sparePartProjectList)
        {
            this.baseRepository = baseRepository;
            this.mapper = mapper;
            this._globalObject = _globalObject;
            this.sparePartStorageList = sparePartStorageList;
            this.repairProjectList = repairProjectList;
            this.sparePartProjectList = sparePartProjectList;
            this._dbContext = _dbContext;
        }
        #endregion

        #region 获取发船备件清单
        /// <summary>
        /// 获取发船备件清单
        /// </summary>
        /// <param name="sendShipSparePartListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SendShipSparePartListResponseDto>>> GetSendShipSparePartListAsync(SendShipSparePartListRequestDto sendShipSparePartListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SendShipSparePartListResponseDto>>();
            RefAsync<int> total = 0;
            var sendShipSparePartList = await baseRepository.AsQueryable()
                .Where(x => x.IsDelete == 1)
                .WhereIF(sendShipSparePartListRequestDto.StartDocumentate.HasValue && sendShipSparePartListRequestDto.EndDocumentate.HasValue, x => x.DocumentDate >= sendShipSparePartListRequestDto.StartDocumentate && x.DocumentDate <= sendShipSparePartListRequestDto.EndDocumentate)
                .WhereIF(sendShipSparePartListRequestDto.SourceNumber != null, x => x.SourceNumber.Contains(sendShipSparePartListRequestDto.SourceNumber))
                .OrderByDescending(x => x.CreateTime)
                .Select(x => new SendShipSparePartListResponseDto
                {
                    Id = x.Id,
                    DocumentDate = x.DocumentDate,
                    ReceivedOn = x.ReceivedOn,
                    SourceType = x.SourceType,
                    SourceNumber = x.SourceNumber,
                    SparePartsType = x.SparePartsType,
                    SparePartsName = x.SparePartsName,
                    SpecificationModel = x.SpecificationModel,
                    MaterialQuality = x.MaterialQuality,
                    Unit = x.Unit,
                    SupplierName = x.SupplierName,
                    OutboundQuantity = x.OutboundQuantity,
                    DeliveryUnitPrice = x.DeliveryUnitPrice,
                    OutboundAmount = x.OutboundAmount,
                    UnitCode = x.UnitCode,
                    MaterialRequisitionUnit = x.MaterialRequisitionUnit,
                    SubmitFinance = x.SubmitFinance,
                    SubmitFinanceTime = x.SubmitFinanceTime,
                    AdjustSubmitFinance = x.AdjustSubmitFinance,
                    AdjustSubmitFinanceTime = x.AdjustSubmitFinanceTime,
                    OutboundRemarks = x.OutboundRemarks,
                }).ToPageListAsync(sendShipSparePartListRequestDto.PageIndex, sendShipSparePartListRequestDto.PageSize, total);
            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = sendShipSparePartList;
            return responseAjaxResult;
        }
        #endregion

        #region 保存发船备件清单
        /// <summary>
        /// 保存发船备件清单
        /// </summary>
        /// <param name="saveSendShipSparePartListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveSendShipSparePartListAsync(SaveSendShipSparePartListRequestDto saveSendShipSparePartListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (saveSendShipSparePartListRequestDto.RequestType)
            {
                var sendShipSparePart = mapper.Map<SaveSendShipSparePartListRequestDto, SendShipSparePartList>(saveSendShipSparePartListRequestDto);
                if (sendShipSparePart != null)
                {
                    sendShipSparePart.Id = GuidUtil.Next();
                    var Save = await baseRepository.InsertAsync(sendShipSparePart);
                    if (Save)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                }
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL, HttpStatusCode.SaveFail);
            }
            else
            {
                var sendShipSparePartList = await baseRepository.AsQueryable().Where(x => x.Id == saveSendShipSparePartListRequestDto.Id && x.IsDelete == 1).SingleAsync();
                if (sendShipSparePartList != null)
                {
                    var sendShipSparePart = mapper.Map<SaveSendShipSparePartListRequestDto, SendShipSparePartList>(saveSendShipSparePartListRequestDto);
                    #region 日志信息
                    LogInfo logDto = new LogInfo()
                    {
                        Id = GuidUtil.Increment(),
                        OperationId = _currentUser.Id,
                        OperationName = _currentUser.Name,
                        DataId = sendShipSparePart.Id,
                        BusinessModule = "/装备管理/修理备件管理/发船备件清单/修改",
                        BusinessRemark = "/装备管理/修理备件管理/发船备件清单/修改"
                    };
                    #endregion
                    var Save = await baseRepository.AsUpdateable(sendShipSparePart).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                    if (Save > 0)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                }
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL, HttpStatusCode.SaveFail);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 删除发船备件清单
        /// <summary>
        /// 删除发船备件清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteSendShipSparePartListAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var sendShipSparePartList = await baseRepository.AsQueryable().Where(x => x.Id == basePrimaryRequestDto.Id && x.IsDelete == 1).FirstAsync();
            if (sendShipSparePartList != null)
            {
                var delete = await baseRepository.DeleteAsync(sendShipSparePartList);
                if (delete)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                    return responseAjaxResult;
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DeleteFail);
            return responseAjaxResult;
        }
        #endregion

        #region 获取备件仓储运输清单
        /// <summary>
        /// 获取备件仓储运输清单
        /// </summary>
        /// <param name="getSparePartStorageListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<GetSparePartStorageListResponseDto>>> GetSparePartStorageListAsync(GetSparePartStorageListRequestDto getSparePartStorageListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<GetSparePartStorageListResponseDto>>();
            RefAsync<int> total = 0;
            var sparePartStorageLists = await sparePartStorageList.AsQueryable().Where(x => x.IsDelete == 1)
                .WhereIF(getSparePartStorageListRequestDto.ProjectCode != null, x => x.ProjectNo.Contains(getSparePartStorageListRequestDto.ProjectCode))
                .WhereIF(getSparePartStorageListRequestDto.ShipName != null, x => x.ShipName.Contains(getSparePartStorageListRequestDto.ShipName))
                .WhereIF(getSparePartStorageListRequestDto.Supplier != null, x => x.Supplier.Contains(getSparePartStorageListRequestDto.Supplier))
                .WhereIF(getSparePartStorageListRequestDto.ResponsiblePerson != null, x => x.ResponsiblePerson.Contains(getSparePartStorageListRequestDto.ResponsiblePerson))
                .OrderByDescending(x => x.CreateTime)
                .ToPageListAsync(getSparePartStorageListRequestDto.PageIndex, getSparePartStorageListRequestDto.PageSize, total);
            var getSparePartStorageListResponseDto = mapper.Map<List<SparePartStorageList>, List<GetSparePartStorageListResponseDto>>(sparePartStorageLists);
                responseAjaxResult.Data = getSparePartStorageListResponseDto;
                responseAjaxResult.Count = total;
                responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 保存备件仓储运输清单
        /// <summary>
        /// 保存备件仓储运输清单
        /// </summary>
        /// <param name="saveSparePartStorageListResponseDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveSparePartStorageListAsync(SaveSparePartStorageListResponseDto saveSparePartStorageListResponseDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (saveSparePartStorageListResponseDto.RequestType)
            {
                var SparePartStorageListResponseDto = mapper.Map<SaveSparePartStorageListResponseDto, SparePartStorageList>(saveSparePartStorageListResponseDto);
                if (SparePartStorageListResponseDto != null)
                {
                    var save = await sparePartStorageList.InsertAsync(SparePartStorageListResponseDto);
                    if(save)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.SaveFail);
            }
            else
            {
                var sparePartStorage = await sparePartStorageList.AsQueryable().Where(x => x.Id == saveSparePartStorageListResponseDto.Id).FirstAsync();
                if (sparePartStorage != null)
                {
                    var SparePartStorageListResponseDto = mapper.Map<SaveSparePartStorageListResponseDto, SparePartStorageList>(saveSparePartStorageListResponseDto);
                    if (SparePartStorageListResponseDto != null)
                    {
                        #region 日志信息
                        LogInfo logDto = new LogInfo()
                        {
                            Id = GuidUtil.Increment(),
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            DataId = saveSparePartStorageListResponseDto.Id,
                            BusinessModule = "/装备管理/修理备件管理/备件仓储运输清单/修改",
                            BusinessRemark = "/装备管理/修理备件管理/备件仓储运输清单/修改"
                        };
                        #endregion
                        var save = await sparePartStorageList.AsUpdateable(SparePartStorageListResponseDto).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                        if (save > 0)
                        {
                            responseAjaxResult.Data = true;
                            responseAjaxResult.Success();
                            return responseAjaxResult;
                        }
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.SaveFail);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 删除备件仓储运输清单
        /// <summary>
        /// 删除备件仓储运输清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteSparePartStorageListAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var sparePartStorageLists = await sparePartStorageList.AsQueryable().Where(x => x.Id == basePrimaryRequestDto.Id && x.IsDelete == 1).FirstAsync();
            if (sparePartStorageLists != null)
            {
                var delete = await sparePartStorageList.DeleteAsync(sparePartStorageLists);
                if (delete)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                    return responseAjaxResult;
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DeleteFail);
            return responseAjaxResult;
        }
        #endregion

        #region 获取修理项目清单
        /// <summary>
        /// 获取修理项目清单
        /// </summary>
        /// <param name="getSparePartStorageListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<GetRepairItemsListResponseDto>>> GetRepairItemsListAsync(GetSparePartStorageListRequestDto getSparePartStorageListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<GetRepairItemsListResponseDto>>();
            RefAsync<int> total = 0;
            var getRepairItemsList = await repairProjectList.AsQueryable().Where(x => x.IsDelete == 1)
                .WhereIF(getSparePartStorageListRequestDto.ProjectCode != null, x => x.ProjectNo.Contains(getSparePartStorageListRequestDto.ProjectCode))
                .WhereIF(getSparePartStorageListRequestDto.ShipName != null, x => x.ShipName.Contains(getSparePartStorageListRequestDto.ShipName))
                .WhereIF(getSparePartStorageListRequestDto.Supplier != null, x => x.Supplier.Contains(getSparePartStorageListRequestDto.Supplier))
                .WhereIF(getSparePartStorageListRequestDto.ResponsiblePerson != null, x => x.ResponsiblePerson.Contains(getSparePartStorageListRequestDto.ResponsiblePerson))
                .OrderByDescending( x=> x.CreateTime)
                .ToPageListAsync(getSparePartStorageListRequestDto.PageIndex, getSparePartStorageListRequestDto.PageSize, total);
                var mapRepairItemsList = mapper.Map<List<RepairProjectList>, List<GetRepairItemsListResponseDto>>(getRepairItemsList);
                    responseAjaxResult.Success();
                    responseAjaxResult.Data = mapRepairItemsList;
                    responseAjaxResult.Count = total;
            return responseAjaxResult;
        }
        #endregion

        #region 保存修理项目清单
        /// <summary>
        /// 保存修理项目清单
        /// </summary>
        /// <param name="saveRepairItemsListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveRepairItemsListAsync(SaveRepairItemsListRequestDto saveRepairItemsListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (saveRepairItemsListRequestDto.RequestType)
            {
                var mapRepairProject = mapper.Map<SaveRepairItemsListRequestDto, RepairProjectList>(saveRepairItemsListRequestDto);
                if (mapRepairProject != null)
                {
                    var save = await repairProjectList.InsertAsync(mapRepairProject);
                    if (save)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.SaveFail);
            }
            else
            {
                var repairProject = await repairProjectList.AsQueryable().Where(x => x.Id == saveRepairItemsListRequestDto.Id).FirstAsync();
                if (repairProject != null)
                {
                    var mapRepairProject = mapper.Map<SaveRepairItemsListRequestDto, RepairProjectList>(saveRepairItemsListRequestDto);
                    if (mapRepairProject != null)
                    {
                        #region 日志信息
                        LogInfo logDto = new LogInfo()
                        {
                            Id = GuidUtil.Increment(),
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            DataId = saveRepairItemsListRequestDto.Id,
                            BusinessModule = "/装备管理/修理备件管理/修理项目清单/修改",
                            BusinessRemark = "/装备管理/修理备件管理/修理项目清单/修改"
                        };
                        #endregion
                        var save = await repairProjectList.AsUpdateable(mapRepairProject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                        if (save > 0)
                        {
                            responseAjaxResult.Data = true;
                            responseAjaxResult.Success();
                            return responseAjaxResult;
                        }
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.SaveFail);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 删除修理项目清单
        /// <summary>
        /// 删除修理项目清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteRepairItemsListAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var repairProject = await repairProjectList.AsQueryable().Where(x => x.Id == basePrimaryRequestDto.Id).FirstAsync();
            if (repairProject != null)
            {
                var delete = await repairProjectList.DeleteAsync(repairProject);
                if (delete)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                    return responseAjaxResult;
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DeleteFail);
            return responseAjaxResult;
        }
        #endregion

        #region 获取备件项目清单
        /// <summary>
        /// 获取备件项目清单
        /// </summary>
        /// <param name="getSparePartStorageListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SparePartProjectListResponseDto>>> GetSparePartProjectListAsync(GetSparePartStorageListRequestDto getSparePartStorageListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SparePartProjectListResponseDto>>();
            RefAsync<int> total = 0;
            var getsparePartProjectList = await sparePartProjectList.AsQueryable().Where(x => x.IsDelete == 1)
                .WhereIF(getSparePartStorageListRequestDto.ProjectCode != null, x => x.ProjectNo.Contains(getSparePartStorageListRequestDto.ProjectCode))
                .WhereIF(getSparePartStorageListRequestDto.Supplier != null, x => x.Supplier.Contains(getSparePartStorageListRequestDto.Supplier))
                .WhereIF(getSparePartStorageListRequestDto.ResponsiblePerson != null, x => x.ResponsiblePerson.Contains(getSparePartStorageListRequestDto.ResponsiblePerson))
                .OrderByDescending(x => x.CreateTime)
                .ToPageListAsync(getSparePartStorageListRequestDto.PageIndex, getSparePartStorageListRequestDto.PageSize, total);
            
                var mapsparePartProjectList = mapper.Map<List<SparePartProjectList>, List<SparePartProjectListResponseDto>>(getsparePartProjectList);
                    responseAjaxResult.Success();
                    responseAjaxResult.Data = mapsparePartProjectList;
                    responseAjaxResult.Count = total;
            return responseAjaxResult;
        }
        #endregion

        #region 保存备件项目清单
        /// <summary>
        /// 保存备件项目清单
        /// </summary>
        /// <param name="saveSparePartProjectListRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveSparePartProjectListAsync(SaveSparePartProjectListRequestDto saveSparePartProjectListRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (saveSparePartProjectListRequestDto.RequestType)
            {
                var mapsparePartProject = mapper.Map<SaveSparePartProjectListRequestDto, SparePartProjectList>(saveSparePartProjectListRequestDto);
                if (mapsparePartProject != null)
                {
                    var save = await sparePartProjectList.InsertAsync(mapsparePartProject);
                    if (save)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.SaveFail);
            }
            else
            {
                var sparePartProject = await sparePartProjectList.AsQueryable().Where(x => x.Id == saveSparePartProjectListRequestDto.Id).FirstAsync();
                if (sparePartProject != null)
                {
                    var mapsparePartProject = mapper.Map<SaveSparePartProjectListRequestDto, SparePartProjectList>(saveSparePartProjectListRequestDto);
                    if (mapsparePartProject != null)
                    {
                        #region 日志信息
                        LogInfo logDto = new LogInfo()
                        {
                            Id = GuidUtil.Increment(),
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            DataId = saveSparePartProjectListRequestDto.Id,
                            BusinessModule = "/装备管理/修理备件管理/备件项目清单/修改",
                            BusinessRemark = "/装备管理/修理备件管理/备件项目清单/修改"
                        };
                        #endregion
                        var save = await sparePartProjectList.AsUpdateable(mapsparePartProject).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                        if (save > 0)
                        {
                            responseAjaxResult.Data = true;
                            responseAjaxResult.Success();
                            return responseAjaxResult;
                        }
                    }
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.SaveFail);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 删除备件项目清单
        /// <summary>
        /// 删除备件项目清单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteSparePartProjectListAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var sparePartProject = await sparePartProjectList.AsQueryable().Where(x => x.Id == basePrimaryRequestDto.Id).FirstAsync();
            if (sparePartProject != null)
            {
                var delete = await sparePartProjectList.DeleteAsync(sparePartProject);
                if (delete)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                    return responseAjaxResult;
                }
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DeleteFail);
            return responseAjaxResult;
        }
        #endregion

        #region 新增修理备件
        /// <summary>
        /// 新增修理项目
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddRepairPartsAsync(List<RepairProjectList> models)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var result = await _dbContext.Insertable<RepairProjectList>(models).ExecuteCommandAsync();
            if (result > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, Domain.Shared.Enums.HttpStatusCode.InsertFail);
            }
            return responseAjaxResult;
        }


        #endregion

        #region 修理备件模块导出
        /// <summary>
        /// 修理备件模块导出
        /// </summary>
        /// <param name="baseRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<object>> ImportRepairPartsStreamAsync()
        {
            
            ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>();
            var repairProjectList = await _dbContext.Queryable<RepairProjectList>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new SparePartProjectListRequestDto() {
                    BiddingSituation = x.BiddingSituation,
                    CompleteTime = x.CompleteTime,
                    ContractMoney = x.ContractMoney,
                    ContractName = x.ContractName,
                    ContractNo = x.ContractNo,
                    ContractTime = x.ContractTime,
                    ExpenseNO = x.ExpenseNO,
                    PayNo = x.PayNo,
                    ProjectDesc = x.ProjectDesc,
                    ProjectNo = x.ProjectNo,
                    Remark = x.Remark,
                    ResponsiblePerson = x.ResponsiblePerson,
                    SettlementAmount = x.SettlementAmount,
                    ShipName = x.ShipName,
                    ShipRepairType = x.ShipRepairType,
                    StartTime = x.StartTime,
                    Supplier = x.Supplier
                }).ToListAsync();
            var sparePartProjectList = await _dbContext.Queryable<SparePartProjectList>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ExcelSparePartProjectListRequseDto()
                {
                    PayNo = x.PayNo,
                    Supplier = x.Supplier,
                    SettlementAmount = x.SettlementAmount,
                    ResponsiblePerson = x.ResponsiblePerson,
                    BiddingSituation = x.BiddingSituation,
                    ContractMoney = x.ContractMoney,
                    ContractName = x.ContractName,
                    ContractNo = x.ContractNo,
                    ContractTime = x.ContractTime,
                    ExpenseNO = x.ExpenseNO,
                    ProjectDesc = x.ProjectDesc,
                    ProjectNo = x.ProjectNo,
                    SparePartType = x.SparePartType
                }).ToListAsync();
            var sendShipSparePartList = await _dbContext.Queryable<SendShipSparePartList>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ExcelSendShipSparePartRequestDto() {
                    AdjustSubmitFinance = x.AdjustSubmitFinance,
                    AdjustSubmitFinanceTime = x.AdjustSubmitFinanceTime,
                    DeliveryUnitPrice = x.DeliveryUnitPrice,
                    DocumentDate = x.DocumentDate,
                    MaterialQuality = x.MaterialQuality,
                    MaterialRequisitionUnit = x.MaterialRequisitionUnit,
                    OutboundAmount = x.OutboundAmount,
                    OutboundQuantity = x.OutboundQuantity,
                    OutboundRemarks = x.OutboundRemarks,
                    ReceivedOn = x.ReceivedOn,
                    SourceNumber = x.SourceNumber,
                    SourceType = x.SourceType,
                    SparePartsName = x.SparePartsName,
                    SparePartsType = x.SparePartsType,
                    SpecificationModel = x.SpecificationModel,
                    SubmitFinance = x.SubmitFinance,
                    SubmitFinanceTime = x.SubmitFinanceTime,
                    SupplierName = x.SupplierName,
                    Unit = x.Unit,
                    UnitCode = x.UnitCode
                }).ToListAsync();
            var sparePartStorageList = await _dbContext.Queryable<SparePartStorageList>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ExcelSparePartStorageRequerDto()
                {
                    ProjectNo = x.ProjectNo,
                    ShipName = x.ShipName,
                    ContractMoney = x.ContractMoney,
                    ContractName = x.ContractName,
                    ContractNo = x.ContractNo,
                    ContractTime = x.ContractTime,
                    ExpenseNO = x.ExpenseNO,
                    BidNegotiationSituation = x.BidNegotiationSituation,
                    ProjectDesc = x.ProjectDesc,
                    Remark = x.Remark,
                    ResponsiblePerson = x.ResponsiblePerson,
                    SettlementAmount = x.SettlementAmount,
                    Supplier = x.Supplier
                }).ToListAsync();
            var sheets = new Dictionary<string, object>
            {
                ["修理项目清单"] = repairProjectList,
                ["备件项目清单"] = sparePartProjectList,
                ["发船备件清单"] = sendShipSparePartList,
                ["备件仓储运输清单"] = sparePartStorageList
            };
            responseAjaxResult.Data=sheets;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region 新增备件项目清单
        /// <summary>
        /// 新增备件项目清单
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddSparePartProjectAsync(List<SparePartProjectList> models)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var result = await _dbContext.Insertable<SparePartProjectList>(models).ExecuteCommandAsync();
            if (result > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, Domain.Shared.Enums.HttpStatusCode.InsertFail);
            }
            return responseAjaxResult;
        }


        #endregion

        #region 新增发船备件清单
        /// <summary>
        /// 新增发船备件清单
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddSendShipSparePartAsync(List<SendShipSparePartList> models)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var result = await _dbContext.Insertable<SendShipSparePartList>(models).ExecuteCommandAsync();
            if (result > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, Domain.Shared.Enums.HttpStatusCode.InsertFail);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 新增备件仓储运输清单
        /// <summary>
        /// 新增备件仓储运输清单
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddSparePartStoragePartAsync(List<SparePartStorageList> models)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var result = await _dbContext.Insertable<SparePartStorageList>(models).ExecuteCommandAsync();
            if (result > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, Domain.Shared.Enums.HttpStatusCode.InsertFail);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 自动统计

        /// <summary>
        ///导出自动统计Excel
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ExportExcelAutomaticPartsAsync()
        {
            var list = (await GetExcelAutomaticPartsAsync()).OrderByDescending(t => t.ProjectNo).ThenBy(t => t.ShipName).ToList();
            // var templatePath = "D:\\projectconllection\\dotnet\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\AutomaticPartTemplate.xlsx";
            var templatePath = "Template/Excel/AutomaticPartTemplate.xlsx";
            using var fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
            //创建工作簿
            using var workbook = new XSSFWorkbook(fs);
            //获取工作表
            ISheet sheet = workbook.GetSheetAt(0);
            int startRowIndex = 2;
            int rowIndex = startRowIndex;
            // 数据填充
            list.ForEach(item =>
            {
                var row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(item.ProjectNo);
                row.CreateCell(1).SetCellValue(item.ShipName);
                row.CreateCell(2).SetCellValue(Convert.ToDouble(item.UnsettledAmount));
                row.CreateCell(3).SetCellValue(Convert.ToDouble(item.SettleAmount));
                row.CreateCell(4).SetCellValue(Convert.ToDouble(item.ReportedAmount));
                row.CreateCell(5).SetCellValue(Convert.ToDouble(item.OutboundAmount));
                row.CreateCell(6).SetCellValue(Convert.ToDouble(item.TransportationAmount));
                rowIndex++;
            });
            // 合并单元格
            var projectNos = list.GroupBy(t => t.ProjectNo).Select(t => new { ProjectNo = t.Key, Num = t.Count() }).ToList();
            rowIndex = startRowIndex;
            projectNos.ForEach(item =>
            {
                var lastRowIndex = rowIndex + item.Num - 1;
                if (item.Num > 1)
                {

                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, lastRowIndex, 0, 0));
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, lastRowIndex, 6, 6));
                }
                rowIndex = lastRowIndex+1;
            });

            using var memory = new MemoryStream();
            workbook.Write(memory);
            return memory.GetBuffer();
        }

        /// <summary>
        ///获取自动统计Excel数据
        /// </summary>
        /// <returns></returns>
        private  async Task<List<ExcelAutomaticPartResponseDto>> GetExcelAutomaticPartsAsync()
        {
            var result = new ResponseAjaxResult<List<ExcelAutomaticPartResponseDto>>();
            var list = new List<ExcelAutomaticPartResponseDto>();
            // 修理项目清单
            var repairList = await repairProjectList.AsQueryable().Where(t => t.IsDelete == 1 && !string.IsNullOrEmpty(t.ProjectNo) && !string.IsNullOrEmpty(t.ShipName))
                             .GroupBy(t => new { t.ProjectNo, t.ShipName })
                             .Select(t => new ExcelAutomaticPartResponseDto()
                             {
                                 ProjectNo = t.ProjectNo,
                                 ShipName = t.ShipName,
                                 UnsettledAmount = SqlFunc.AggregateSum(t.SettlementAmount == null && (t.ExpenseNO == null || t.ExpenseNO == "") ? t.ContractMoney : 0),
                                 SettleAmount = SqlFunc.AggregateSum(t.SettlementAmount != null && (t.ExpenseNO == null || t.ExpenseNO == "") ? t.SettlementAmount : 0),
                                 ReportedAmount = SqlFunc.AggregateSum(t.SettlementAmount != null && t.ExpenseNO != null && t.ExpenseNO != "" ? t.SettlementAmount : 0)
                             }).ToListAsync();
            // 发船备件清单
            var sendQuery = baseRepository.AsQueryable().Where(t => t.IsDelete == 1 && t.SubmitFinanceTime != null && t.OutboundAmount != null)
                            .Select(t => new SumSendShipSparePartList()
                            {
                                SubmitFinanceYear = SqlFunc.Substring(t.SubmitFinanceTime, 0, 4),
                                MaterialRequisitionUnit = t.MaterialRequisitionUnit,
                                OutboundAmount = t.OutboundAmount
                            });
            var sendList = await _dbContext.Queryable(sendQuery)
                           .GroupBy(t => new { t.SubmitFinanceYear, t.MaterialRequisitionUnit })
                           .Select(t => new SumSendShipSparePartList
                           {
                               SubmitFinanceYear = t.SubmitFinanceYear,
                               MaterialRequisitionUnit = t.MaterialRequisitionUnit,
                               OutboundAmount = SqlFunc.AggregateSum(t.OutboundAmount ?? 0)
                           }).ToListAsync();
            // 统计仓储运输金额
            //var sumTransportations = repairList.GroupBy(t => t.ProjectNo).Select(t => new { ProjectNo=t.Key, ReportedAmount = t.Sum(i => i.ReportedAmount ?? 0) });

            //统计仓储运输金额  备件仓储运输清单
            var sumTransportations =await  sparePartStorageList.AsQueryable().Where(x => x.IsDelete == 1).GroupBy(t => t.ProjectNo)
              .Select(x => new { ProjectNo = x.ProjectNo, ReportedAmount = SqlFunc.AggregateSum(SqlFunc.IsNullOrEmpty(x.ExpenseNO) ? 0 : x.SettlementAmount) }).ToListAsync();
          

            repairList.ForEach(item =>
            {
                item.OutboundAmount = sendList.FirstOrDefault(t => t.SubmitFinanceYear == item.ProjectNo && t.MaterialRequisitionUnit == item.ShipName)?.OutboundAmount;
                item.TransportationAmount = sumTransportations.FirstOrDefault(t => t.ProjectNo == item.ProjectNo)?.ReportedAmount;
            });
            return repairList;
        }

        #endregion

        /// <summary>
        ///获取自动统计数据
        /// </summary>
        /// <returns></returns>
        private async Task<List<AutomaticPartResponseDto>> GetAutomaticPartsAsync()
        {
            var result = new ResponseAjaxResult<List<AutomaticPartResponseDto>>();
            var list = new List<AutomaticPartResponseDto>();
            // 修理项目清单
            var repairList = await repairProjectList.AsQueryable().Where(t => t.IsDelete == 1 && !string.IsNullOrEmpty(t.ProjectNo) && !string.IsNullOrEmpty(t.ShipName))
                             .GroupBy(t => new { t.ProjectNo, t.ShipName })
                             .Select(t => new AutomaticPartResponseDto()
                             {
                                 ProjectNo = t.ProjectNo,
                                 ShipName = t.ShipName,
                                 UnsettledAmount = SqlFunc.AggregateSum(t.SettlementAmount == null && (t.ExpenseNO == null || t.ExpenseNO == "") ? t.ContractMoney : 0),
                                 SettleAmount = SqlFunc.AggregateSum(t.SettlementAmount != null && (t.ExpenseNO == null || t.ExpenseNO == "") ? t.SettlementAmount : 0),
                                 ReportedAmount = SqlFunc.AggregateSum(t.SettlementAmount != null && t.ExpenseNO != null && t.ExpenseNO != "" ? t.SettlementAmount : 0)
                             }).ToListAsync();
            // 发船备件清单
            var sendQuery = baseRepository.AsQueryable().Where(t => t.IsDelete == 1 && t.SubmitFinanceTime != null && t.OutboundAmount != null)
                            .Select(t => new SumSendShipSparePartList()
                            {
                                SubmitFinanceYear = SqlFunc.Substring(t.SubmitFinanceTime, 0, 4),
                                MaterialRequisitionUnit = t.MaterialRequisitionUnit,
                                OutboundAmount = t.OutboundAmount
                            });
            var sendList = await _dbContext.Queryable(sendQuery)
                           .GroupBy(t => new { t.SubmitFinanceYear, t.MaterialRequisitionUnit })
                           .Select(t => new SumSendShipSparePartList
                           {
                               SubmitFinanceYear = t.SubmitFinanceYear,
                               MaterialRequisitionUnit = t.MaterialRequisitionUnit,
                               OutboundAmount = SqlFunc.AggregateSum(t.OutboundAmount ?? 0)
                           }).ToListAsync();
            // 统计仓储运输金额
            var sumTransportations = repairList.GroupBy(t => t.ProjectNo).Select(t => new { ProjectNo = t.Key, ReportedAmount = t.Sum(i => i.ReportedAmount ?? 0) });
            repairList.ForEach(item =>
            {
                item.OutboundAmount = sendList.FirstOrDefault(t => t.SubmitFinanceYear == item.ProjectNo && t.MaterialRequisitionUnit == item.ShipName)?.OutboundAmount;
                item.TransportationAmount = sumTransportations.Single(t => t.ProjectNo == item.ProjectNo).ReportedAmount;
            });
            return repairList;
        }
        /// <summary>
        /// 搜索自动统计列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<AutomaticPartsResponseDto>> SearchAutomaticPartsAsync(AutomaticPartsRequestDto model)
        {
            var result = new ResponseAjaxResult<AutomaticPartsResponseDto>();
            var list = (await GetAutomaticPartsAsync()).OrderByDescending(t => t.ProjectNo).ThenBy(t => t.ShipName).ToList();
            decimal transportationAmount = 0;
            string? projectNo = model.ProjectCode;
            if (string.IsNullOrWhiteSpace(projectNo))
            {
                projectNo = DateTime.Now.Year.ToString();
            }
            list = list.Where(t => t.ProjectNo == projectNo).ToList();
            if (list.Any())
            {
                transportationAmount = Math.Round(list.First().TransportationAmount ?? 0, 2);
            }
            list.ForEach(item =>
            {
                item.SettleAmount = Math.Round(item.SettleAmount ?? 0, 2);
                item.OutboundAmount = Math.Round(item.OutboundAmount ?? 0, 2);
                item.UnsettledAmount = Math.Round(item.UnsettledAmount ?? 0, 2);
                item.ReportedAmount = Math.Round(item.ReportedAmount ?? 0, 2);
            });
            var data = new AutomaticPartsResponseDto()
            {
                List = list,
                TransportationAmount = transportationAmount,
            };
            return result.SuccessResult(data);
        }
    }
}

