using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Globalization;

namespace GHMonitoringCenterApi.Application.Service.EquipmentManagement
{
    /// <summary>
    /// 设备管理接口实现层
    /// </summary>
    public class EquipmentManagementService : IEquipmentManagementService
    {
        public ISqlSugarClient dbContext { get; set; }
        public ILogService logService { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        public EquipmentManagementService(ISqlSugarClient dbContext, ILogService logService, GlobalObject globalObject)
        {
            this.dbContext = dbContext;
            this.logService = logService;
            _globalObject = globalObject;
        }
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>> SearchEquipmentManagementAsync(SearchEquipmentManagementRequestDto searchEquipmentManagementRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>();
            var project = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            var shipPingType = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1).ToListAsync();
            var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
            var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1).ToListAsync();
            RefAsync<int> total = 0;
            if (searchEquipmentManagementRequestDto.DeviceType == 1)//水上设备
            {
                DateTime? dateTime;
                var marineEquipmentList = await dbContext.Queryable<MarineEquipment>()
                    //.LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                    //.LeftJoin<ShipPingType>((x, y, z) => x.ShipTypeId == z.PomId)
                    //.LeftJoin<Institution>((x, y, z, p) => x.CompanyId == p.PomId)
                    //.LeftJoin<EquipmentList>((x, y, z, p, q) => q.Oid == 45 && q.Id == x.NavigationAreaId)
                    .Where(x => x.IsDelete == 1)
                    .WhereIF(searchEquipmentManagementRequestDto.StarTime.HasValue && searchEquipmentManagementRequestDto.EndTime.HasValue,
                    x => x.ReportingMonth >= searchEquipmentManagementRequestDto.StarTime.Value.ToDateMonth() && x.ReportingMonth <= searchEquipmentManagementRequestDto.EndTime.Value.ToDateMonth())
                    .WhereIF(searchEquipmentManagementRequestDto.CompanyId != null, x => x.CompanyId == searchEquipmentManagementRequestDto.CompanyId)
                    .WhereIF(searchEquipmentManagementRequestDto.ProjectId != null, x => x.ProjectId == searchEquipmentManagementRequestDto.ProjectId)
                    .OrderByDescending(x => x.ReportingMonth)
                    .ToPageListAsync(searchEquipmentManagementRequestDto.PageIndex, searchEquipmentManagementRequestDto.PageSize, total);
                List<SearchEquipmentManagementResponseDto> searchEquipmentManagementResponseDto = new List<SearchEquipmentManagementResponseDto>();
                foreach (var item in marineEquipmentList)
                {
                    ConvertHelper.TryParseFromDateMonth(item.ReportingMonth, out DateTime dateTime1);
                    SearchEquipmentManagementResponseDto searchEquipmentManagementResponseDto1 = new SearchEquipmentManagementResponseDto()
                    {
                        Id = item.Id,
                        ReportingMonth = dateTime1,
                        CompanyId = item.CompanyId,
                        CompanyName = institution.SingleOrDefault(x => x.PomId == item.CompanyId)?.Name,
                        ProjectId = item.ProjectId,
                        ProjectName = project.SingleOrDefault(x => x.Id == item.ProjectId)?.Name,
                        SubcontractorName = item.SubcontractorName,
                        ContractAmount = item.ContractAmount,
                        ShipName = item.ShipName,
                        NavigationAreaName = equipmentList.SingleOrDefault(x => x.Id == item.NavigationAreaId)?.DeviceName,
                        NavigationAreaId = item.NavigationAreaId,
                        MMSI = item.MMSI,
                        ShipTypeId = item.ShipTypeId,
                        ShipTypeName = equipmentList.FirstOrDefault(x => x.Id == item.ShipTypeId)?.DeviceName,
                        ShipOwner = item.ShipOwner,
                        CompletionDate = item.CompletionDate,
                        ShipSizeSpecifications = item.ShipSizeSpecifications,
                        Specification1 = item.Specification1,
                        Specification2 = item.Specification2,
                        ShipCapacity = item.ShipCapacity,
                        EntryDate = item.EntryDate,
                        ExitDate = item.ExitDate,
                        PresentNot = item.PresentNot.ToString(),
                        Notes = item.Notes
                    };
                    searchEquipmentManagementResponseDto.Add(searchEquipmentManagementResponseDto1);
                }

                if (marineEquipmentList.Any())
                {
                    responseAjaxResult.Data = searchEquipmentManagementResponseDto;
                    responseAjaxResult.Count = total;
                }
            }
            else if (searchEquipmentManagementRequestDto.DeviceType == 2)// 陆域设备
            {

                List<SearchEquipmentManagementResponseDto> searchEquipmentManagementResponseDtos = new List<SearchEquipmentManagementResponseDto>();
                var landDevice1 = await dbContext.Queryable<LandDevice>().ToListAsync();
                var landDevice = await dbContext.Queryable<LandDevice>()
                    .Where(x => x.IsDelete == 1)
                    .WhereIF(searchEquipmentManagementRequestDto.StarTime.HasValue && searchEquipmentManagementRequestDto.EndTime.HasValue,
                    x => x.ReportingMonth >= searchEquipmentManagementRequestDto.StarTime.Value.ToDateMonth() && x.ReportingMonth <= searchEquipmentManagementRequestDto.EndTime.Value.ToDateMonth())
                    .WhereIF(searchEquipmentManagementRequestDto.CompanyId != null, x => x.CompanyId == searchEquipmentManagementRequestDto.CompanyId)
                    .WhereIF(searchEquipmentManagementRequestDto.ProjectId != null, x => x.ProjectId == searchEquipmentManagementRequestDto.ProjectId)
                    .OrderByDescending(x => x.ReportingMonth)
                   .ToPageListAsync(searchEquipmentManagementRequestDto.PageIndex, searchEquipmentManagementRequestDto.PageSize, total);
                foreach (var item in landDevice)
                {
                    ConvertHelper.TryParseFromDateMonth(item.ReportingMonth, out DateTime dateTime1);
                    var searchEquipmentManagementResponseDto = new SearchEquipmentManagementResponseDto()
                    {
                        Id = item.Id,
                        ReportingMonth = dateTime1,
                        CompanyId = item.CompanyId,
                        CompanyName = institution.FirstOrDefault(x => x.PomId == item.CompanyId)?.Name,
                        ProjectId = item.ProjectId,
                        ProjectName = project.FirstOrDefault(x => x.Id == item.ProjectId)?.Name,
                        SubcontractorName = item.SubcontractorName,
                        ContractAmount = item.ContractAmount,
                        EquipmentCategoryId = item.EquipmentCategoryId,
                        EquipmentCategoryName = equipmentList.FirstOrDefault(x => x.Id == item.EquipmentCategoryId)?.DeviceName,
                        DeviceClassId = item.DeviceClassId,
                        DeviceClassName = equipmentList.FirstOrDefault(x => x.Id == item.DeviceClassId)?.DeviceName,
                        EquipmentSubcategoriesId = item.EquipmentSubcategoriesId,
                        EquipmentSubcategoriesName = equipmentList.FirstOrDefault(x => x.Id == item.EquipmentSubcategoriesId)?.DeviceName,
                        EquipmentModel = item.EquipmentModel,
                        Manufacturer = item.Manufacturer,
                        FactoryDate = item.FactoryDate,
                        CertificateNumber = item.CertificateNumber,
                        Specification1 = item.EquipmentSpecifications,
                        Specification2 = item.Specifications,
                        EntryDate = item.EntryDate,
                        ExitDate = item.ExitDate,
                        PresentNot = item.PresentNot.ToString(),
                        Notes = item.Notes,
                    };
                    searchEquipmentManagementResponseDtos.Add(searchEquipmentManagementResponseDto);
                }
                responseAjaxResult.Data = searchEquipmentManagementResponseDtos;
                responseAjaxResult.Count = total;
            }
            else if (searchEquipmentManagementRequestDto.DeviceType == 3)//特种设备
            {
                List<SearchEquipmentManagementResponseDto> searchEquipmentManagementResponseDtos = new List<SearchEquipmentManagementResponseDto>();
                var landDevice = await dbContext.Queryable<SpecialEquipment>()
                    .Where(x => x.IsDelete == 1)
                    .WhereIF(searchEquipmentManagementRequestDto.StarTime.HasValue && searchEquipmentManagementRequestDto.EndTime.HasValue,
                    x => x.ReportingMonth >= searchEquipmentManagementRequestDto.StarTime.Value.ToDateMonth() && x.ReportingMonth <= searchEquipmentManagementRequestDto.EndTime.Value.ToDateMonth())
                    .WhereIF(searchEquipmentManagementRequestDto.CompanyId != null, x => x.CompanyId == searchEquipmentManagementRequestDto.CompanyId)
                    .WhereIF(searchEquipmentManagementRequestDto.ProjectId != null, x => x.ProjectId == searchEquipmentManagementRequestDto.ProjectId)
                    .OrderByDescending(x => x.ReportingMonth)
                   .ToPageListAsync(searchEquipmentManagementRequestDto.PageIndex, searchEquipmentManagementRequestDto.PageSize, total);
                foreach (var item in landDevice)
                {
                    var searchEquipmentManagementResponseDto = new SearchEquipmentManagementResponseDto()
                    {
                        Id = item.Id,
                        ReportingMonth = DateTime.ParseExact(item.ReportingMonth.ToString(), "yyyyMM", CultureInfo.InvariantCulture),
                        CompanyId = item.CompanyId,
                        CompanyName = institution.FirstOrDefault(x => x.PomId == item.CompanyId).Name,
                        ProjectId = item.ProjectId,
                        ProjectName = project.FirstOrDefault(x => x.Id == item.ProjectId).Name,
                        SubcontractorName = item.SubcontractorName,
                        ContractAmount = item.ContractAmount,
                        EquipmentCategoryId = item.EquipmentCategoryId,
                        EquipmentCategoryName = equipmentList.FirstOrDefault(x => x.Id == item.EquipmentCategoryId).DeviceName,
                        EquipmentModel = item.EquipmentModel,
                        Manufacturer = item.Manufacturer,
                        FactoryDate = item.FactoryDate,
                        CertificateNumber = item.CertificateNumber,
                        Specification1 = item.EquipmentSpecifications,
                        Specification2 = item.Specifications,
                        EntryDate = item.EntryDate,
                        SpecialEquipmentNumber = item.SpecialEquipmentNumber,
                        ExitDate = item.ExitDate,
                        PresentNot = item.PresentNot.ToString(),
                        Notes = item.Notes,
                    };
                    searchEquipmentManagementResponseDtos.Add(searchEquipmentManagementResponseDto);
                }
                responseAjaxResult.Data = searchEquipmentManagementResponseDtos;
                responseAjaxResult.Count = total;
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 保存设备
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveEquipmentManagementAsync(SaveEquipmentManagementRequestDto saveEquipmentManagementRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            DateTime dateTime = DateTime.Now;
            int datetime = 0;
            #region 时间判断
            if (saveEquipmentManagementRequestDto.ReportingMonth == null)
            {
                if (DateTime.Now.Day <= 25)
                {
                    datetime = DateTime.Now.ToDateMonth();
                }
                else
                {
                    datetime = DateTime.Now.AddMonths(+1).ToDateMonth();
                }
            }
            else
            {
                datetime = saveEquipmentManagementRequestDto.ReportingMonth.Value.ToDateMonth();
            }
            #endregion

            #region 新增设备月报
            if (saveEquipmentManagementRequestDto.RequestType)
            {
                if (saveEquipmentManagementRequestDto.DeviceType == 1) // 水上设备
                {
                    var marineEquipmentDelete = await dbContext.Queryable<MarineEquipment>().Where(x => x.ShipName == saveEquipmentManagementRequestDto.ShipName && x.ProjectId == saveEquipmentManagementRequestDto.ProjectId && x.IsDelete == 1 && x.ReportingMonth == datetime).ToListAsync();
                    if (marineEquipmentDelete.Any())
                    {
                        await dbContext.Deleteable(marineEquipmentDelete).ExecuteCommandAsync();
                    }
                    MarineEquipment marineEquipment = new MarineEquipment()
                    {
                        Id = Guid.NewGuid(),
                        ReportingMonth = datetime,
                        CompanyId = saveEquipmentManagementRequestDto.CompanyId.Value,
                        ProjectId = saveEquipmentManagementRequestDto.ProjectId.Value,
                        SubcontractorName = saveEquipmentManagementRequestDto.SubcontractorName,
                        ContractAmount = saveEquipmentManagementRequestDto.ContractAmount,
                        ShipName = saveEquipmentManagementRequestDto.ShipName,
                        ShipTypeId = saveEquipmentManagementRequestDto.ShipTypeId.Value,
                        NavigationAreaId = saveEquipmentManagementRequestDto.NavigationAreaId.Value,
                        MMSI = saveEquipmentManagementRequestDto.MMSI,
                        ShipOwner = saveEquipmentManagementRequestDto.ShipOwner,
                        CompletionDate = saveEquipmentManagementRequestDto.CompletionDate.HasValue == false ? default(DateTime) : saveEquipmentManagementRequestDto.CompletionDate.Value,
                        ShipSizeSpecifications = saveEquipmentManagementRequestDto.ShipSizeSpecifications,
                        Specification1 = saveEquipmentManagementRequestDto.Specification1,
                        Specification2 = saveEquipmentManagementRequestDto.Specification2,
                        ShipCapacity = saveEquipmentManagementRequestDto.ShipCapacity,
                        EntryDate = saveEquipmentManagementRequestDto.EntryDate.Value,
                        ExitDate = saveEquipmentManagementRequestDto.ExitDate,
                        PresentNot = saveEquipmentManagementRequestDto.PresentNot == "是" ? 1 : 2,
                        Notes = saveEquipmentManagementRequestDto.Notes,
                    };
                    var marineEquipmentList = await dbContext.Queryable<MarineEquipment>().Where(x => x.IsDelete == 1 && x.ShipName == marineEquipment.ShipName).ToListAsync();
                    //逻辑判断
                    var judgmentStatus = await JudgmentStatus(marineEquipment.EntryDate, marineEquipment.ExitDate, marineEquipment.ShipName, 1, marineEquipment.ProjectId, marineEquipment.ReportingMonth, marineEquipment.Id);
                    if (judgmentStatus)
                    {
                        #region 记录日志
                        var logObj = new LogInfo()
                        {
                            //Id = GuidUtil.Increment(),
                            BusinessModule = "/装备管理/非自有设备管理/新增",
                            BusinessRemark = "新增",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            OperationType = 1
                        };
                        #endregion
                        await dbContext.Insertable(marineEquipment).EnableDiffLogEvent(logObj).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                    }
                    else
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Fail(ResponseMessage.DEVICE_SAVER_FAIL);
                        return responseAjaxResult;
                    }
                }
                else if (saveEquipmentManagementRequestDto.DeviceType == 2) //陆域设备
                {
                    LandDevice landDevice = new LandDevice()
                    {
                        Id = Guid.NewGuid(),
                        ReportingMonth = datetime,
                        CompanyId = saveEquipmentManagementRequestDto.CompanyId.Value,
                        ProjectId = saveEquipmentManagementRequestDto.ProjectId.Value,
                        SubcontractorName = saveEquipmentManagementRequestDto.SubcontractorName,
                        ContractAmount = saveEquipmentManagementRequestDto.ContractAmount,
                        EquipmentCategoryId = saveEquipmentManagementRequestDto.EquipmentCategoryId,
                        DeviceClassId = saveEquipmentManagementRequestDto.DeviceClassId,
                        EquipmentSubcategoriesId = saveEquipmentManagementRequestDto.EquipmentSubcategoriesId,
                        EquipmentModel = saveEquipmentManagementRequestDto.EquipmentModel,
                        Manufacturer = saveEquipmentManagementRequestDto.Manufacturer,
                        FactoryDate = saveEquipmentManagementRequestDto.FactoryDate.Value,
                        CertificateNumber = saveEquipmentManagementRequestDto.CertificateNumber,
                        EquipmentSpecifications = saveEquipmentManagementRequestDto.Specification1,
                        Specifications = saveEquipmentManagementRequestDto.Specification2,
                        EntryDate = saveEquipmentManagementRequestDto.EntryDate.Value,
                        ExitDate = saveEquipmentManagementRequestDto.ExitDate,
                        PresentNot = saveEquipmentManagementRequestDto.PresentNot == "是" ? 1 : 2,
                        Notes = saveEquipmentManagementRequestDto.Notes
                    };
                    var landDeviceList = await dbContext.Queryable<LandDevice>().Where(x => x.IsDelete == 1 && x.EquipmentSubcategoriesId == landDevice.EquipmentSubcategoriesId).ToListAsync();
                    //var marineEquipmentList = await dbContext.Queryable<MarineEquipment>().Where(x => x.IsDelete == 1 && x.ShipName == marineEquipment.ShipName).ToListAsync();
                    //逻辑判断
                    var judgmentStatus = await JudgmentStatus(landDevice.EntryDate, landDevice.ExitDate, landDevice.EquipmentModel, 2, landDevice.ProjectId, landDevice.ReportingMonth, landDevice.Id);
                    if (judgmentStatus)
                    {
                        #region 记录日志
                        var logObj = new LogInfo()
                        {
                            //Id = GuidUtil.Increment(),
                            BusinessModule = "/装备管理/非自有设备管理/新增",
                            BusinessRemark = "导出",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            OperationType = 1
                        };
                        #endregion
                        await dbContext.Insertable(landDevice).EnableDiffLogEvent(logObj).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                    }
                    else
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Fail(ResponseMessage.DEVICE_SAVER_FAIL);
                        return responseAjaxResult;
                    };
                }
                else if (saveEquipmentManagementRequestDto.DeviceType == 3)
                {
                    SpecialEquipment specialEquipment = new SpecialEquipment()
                    {
                        Id = Guid.NewGuid(),
                        ReportingMonth = datetime,
                        CompanyId = saveEquipmentManagementRequestDto.CompanyId.Value,
                        ProjectId = saveEquipmentManagementRequestDto.ProjectId.Value,
                        SubcontractorName = saveEquipmentManagementRequestDto.SubcontractorName,
                        ContractAmount = saveEquipmentManagementRequestDto.ContractAmount,
                        EquipmentCategoryId = saveEquipmentManagementRequestDto.EquipmentCategoryId,
                        EquipmentModel = saveEquipmentManagementRequestDto.EquipmentModel,
                        Manufacturer = saveEquipmentManagementRequestDto.Manufacturer,
                        FactoryDate = saveEquipmentManagementRequestDto.FactoryDate.Value,
                        CertificateNumber = saveEquipmentManagementRequestDto.CertificateNumber,
                        EquipmentSpecifications = saveEquipmentManagementRequestDto.Specification1,
                        Specifications = saveEquipmentManagementRequestDto.Specification2,
                        SpecialEquipmentNumber = saveEquipmentManagementRequestDto.SpecialEquipmentNumber,
                        EntryDate = saveEquipmentManagementRequestDto.EntryDate.Value,
                        ExitDate = saveEquipmentManagementRequestDto.ExitDate,
                        PresentNot = saveEquipmentManagementRequestDto.PresentNot == "是" ? 1 : 2,
                        Notes = saveEquipmentManagementRequestDto.Notes
                    };

                    var judgmentStatus = await JudgmentStatus(specialEquipment.EntryDate, specialEquipment.ExitDate, specialEquipment.EquipmentModel, 3, specialEquipment.ProjectId, specialEquipment.ReportingMonth, specialEquipment.Id);
                    if (judgmentStatus)
                    {
                        #region 记录日志
                        var logObj = new LogInfo()
                        {
                            //Id = GuidUtil.Increment(),
                            BusinessModule = "/装备管理/非自有设备管理/新增",
                            BusinessRemark = "导出",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            OperationType = 1
                        };
                        #endregion
                        await dbContext.Insertable(specialEquipment).EnableDiffLogEvent(logObj).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                    }
                    else
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Fail(ResponseMessage.DEVICE_SAVER_FAIL);
                        return responseAjaxResult;
                    };
                }
            }
            #endregion
            #region 修改设备月报
            else
            {
                if (saveEquipmentManagementRequestDto.DeviceType == 1)
                {
                    var marineEquipment = await dbContext.Queryable<MarineEquipment>().Where(x => x.Id == saveEquipmentManagementRequestDto.Id).FirstAsync();
                    if (marineEquipment != null)
                    {
                        var marineEquipmentDelete = await dbContext.Queryable<MarineEquipment>().Where(x => x.ShipName == saveEquipmentManagementRequestDto.ShipName && x.ProjectId == saveEquipmentManagementRequestDto.ProjectId && x.IsDelete == 1 && x.ReportingMonth == datetime).ToListAsync();
                        if (marineEquipmentDelete.Any())
                        {
                            foreach (var item in marineEquipmentDelete)
                            {
                                item.IsDelete = 0;
                            }
                            await dbContext.Updateable(marineEquipmentDelete).ExecuteCommandAsync();
                        }
                        marineEquipment = new MarineEquipment()
                        {
                            Id = Guid.NewGuid(),
                            ReportingMonth = datetime,
                            CompanyId = saveEquipmentManagementRequestDto.CompanyId.Value,
                            ProjectId = saveEquipmentManagementRequestDto.ProjectId.Value,
                            SubcontractorName = saveEquipmentManagementRequestDto.SubcontractorName,
                            ContractAmount = saveEquipmentManagementRequestDto.ContractAmount,
                            ShipName = saveEquipmentManagementRequestDto.ShipName,
                            ShipTypeId = saveEquipmentManagementRequestDto.ShipTypeId.Value,
                            NavigationAreaId = saveEquipmentManagementRequestDto.NavigationAreaId.Value,
                            MMSI = saveEquipmentManagementRequestDto.MMSI,
                            ShipOwner = saveEquipmentManagementRequestDto.ShipOwner,
                            CompletionDate = saveEquipmentManagementRequestDto.CompletionDate.Value,
                            ShipSizeSpecifications = saveEquipmentManagementRequestDto.ShipSizeSpecifications,
                            Specification1 = saveEquipmentManagementRequestDto.Specification1,
                            Specification2 = saveEquipmentManagementRequestDto.Specification2,
                            ShipCapacity = saveEquipmentManagementRequestDto.ShipCapacity,
                            EntryDate = saveEquipmentManagementRequestDto.EntryDate.Value,
                            ExitDate = saveEquipmentManagementRequestDto.ExitDate,
                            PresentNot = saveEquipmentManagementRequestDto.PresentNot == "1" ? 1 : 2,
                            Notes = saveEquipmentManagementRequestDto.Notes,
                        };
                        #region 记录日志
                        var logObj = new LogInfo()
                        {
                            //Id = GuidUtil.Increment(),
                            BusinessModule = "/装备管理/非自有设备管理/水上设备/修改",
                            BusinessRemark = "新增",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            OperationType = 1
                        };
                        #endregion
                        await dbContext.Insertable(marineEquipment).EnableDiffLogEvent(logObj).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                    }
                }
                else if (saveEquipmentManagementRequestDto.DeviceType == 2)
                {
                    var landDevice = await dbContext.Queryable<LandDevice>().Where(x => x.Id == saveEquipmentManagementRequestDto.Id).FirstAsync();
                    if (landDevice != null)
                    {
                        landDevice = new LandDevice()
                        {
                            Id = saveEquipmentManagementRequestDto.Id.Value,
                            ReportingMonth = datetime,
                            CompanyId = saveEquipmentManagementRequestDto.CompanyId.Value,
                            ProjectId = saveEquipmentManagementRequestDto.ProjectId.Value,
                            SubcontractorName = saveEquipmentManagementRequestDto.SubcontractorName,
                            ContractAmount = saveEquipmentManagementRequestDto.ContractAmount,
                            EquipmentCategoryId = saveEquipmentManagementRequestDto.EquipmentCategoryId,
                            DeviceClassId = saveEquipmentManagementRequestDto.DeviceClassId,
                            EquipmentSubcategoriesId = saveEquipmentManagementRequestDto.EquipmentSubcategoriesId,
                            EquipmentModel = saveEquipmentManagementRequestDto.EquipmentModel,
                            Manufacturer = saveEquipmentManagementRequestDto.Manufacturer,
                            FactoryDate = saveEquipmentManagementRequestDto.FactoryDate.Value,
                            CertificateNumber = saveEquipmentManagementRequestDto.CertificateNumber,
                            EquipmentSpecifications = saveEquipmentManagementRequestDto.Specification1,
                            Specifications = saveEquipmentManagementRequestDto.Specification2,
                            EntryDate = saveEquipmentManagementRequestDto.EntryDate.Value,
                            ExitDate = saveEquipmentManagementRequestDto.ExitDate,
                            PresentNot = saveEquipmentManagementRequestDto.PresentNot == "1" ? 1 : 2,
                            Notes = saveEquipmentManagementRequestDto.Notes
                        };
                        #region 记录日志
                        var logObj = new LogInfo()
                        {
                            //Id = GuidUtil.Increment(),
                            BusinessModule = "/装备管理/非自有设备管理/陆域设备/修改",
                            BusinessRemark = "新增",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            OperationType = 1
                        };
                        #endregion
                        await dbContext.Updateable(landDevice).EnableDiffLogEvent(logObj).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                    }
                }
                else if (saveEquipmentManagementRequestDto.DeviceType == 3)
                {
                    var specialEquipment = await dbContext.Queryable<SpecialEquipment>().Where(x => x.Id == saveEquipmentManagementRequestDto.Id).FirstAsync();
                    if (specialEquipment != null)
                    {
                        specialEquipment = new SpecialEquipment()
                        {
                            Id = saveEquipmentManagementRequestDto.Id.Value,
                            ReportingMonth = datetime,
                            CompanyId = saveEquipmentManagementRequestDto.CompanyId.Value,
                            ProjectId = saveEquipmentManagementRequestDto.ProjectId.Value,
                            SubcontractorName = saveEquipmentManagementRequestDto.SubcontractorName,
                            ContractAmount = saveEquipmentManagementRequestDto.ContractAmount,
                            EquipmentCategoryId = saveEquipmentManagementRequestDto.EquipmentCategoryId,
                            EquipmentModel = saveEquipmentManagementRequestDto.EquipmentModel,
                            Manufacturer = saveEquipmentManagementRequestDto.Manufacturer,
                            FactoryDate = saveEquipmentManagementRequestDto.FactoryDate.Value,
                            CertificateNumber = saveEquipmentManagementRequestDto.CertificateNumber,
                            EquipmentSpecifications = saveEquipmentManagementRequestDto.Specification1,
                            SpecialEquipmentNumber = saveEquipmentManagementRequestDto.SpecialEquipmentNumber,
                            Specifications = saveEquipmentManagementRequestDto.Specification2,
                            EntryDate = saveEquipmentManagementRequestDto.EntryDate.Value,
                            ExitDate = saveEquipmentManagementRequestDto.ExitDate,
                            PresentNot = saveEquipmentManagementRequestDto.PresentNot == "1" ? 1 : 2,
                            Notes = saveEquipmentManagementRequestDto.Notes
                        };
                        #region 记录日志
                        var logObj = new LogInfo()
                        {
                            //Id = GuidUtil.Increment(),
                            BusinessModule = "/装备管理/非自有设备管理/特种设备/修改",
                            BusinessRemark = "新增",
                            OperationId = _currentUser.Id,
                            OperationName = _currentUser.Name,
                            OperationType = 1
                        };
                        #endregion
                        await dbContext.Updateable(specialEquipment).EnableDiffLogEvent(logObj).ExecuteCommandAsync();
                        responseAjaxResult.Data = true;
                    }
                }
            }
            #endregion
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 判断进退场时间是否合理
        /// </summary>
        /// <param name="StarTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="DeviceId"></param>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> JudgmentStatus(DateTime? StarTime, DateTime? EndTime, string? DeviceName, int DeviceType, Guid? ProjectId, int DatetimeMonth, Guid? Id = null)
        {
            return true;
            //if (DeviceType == 1)//水上设备
            //{
            //    var marineEquipment = await dbContext.Queryable<MarineEquipment>().Where(x => x.IsDelete == 1 && x.ShipName == DeviceName && x.ReportingMonth == DatetimeMonth)
            //        .WhereIF(Id != null, x => x.Id != Id)
            //        .OrderByDescending(x => x.EntryDate).ToListAsync();
            //    if (!marineEquipment.Any())
            //    {
            //        return true;
            //    }
            //    if (EndTime != null)
            //    {
            //        if (marineEquipment.Count > 1)
            //        {
            //            //传入进场时间大于最小进场时间且小于最大进场时间
            //            if (marineEquipment[marineEquipment.Count - 1].EntryDate < StarTime && marineEquipment[0].EntryDate > StarTime)
            //            {
            //                for (int i = 0; i < marineEquipment.Count - 1; i++)
            //                {
            //                    if (marineEquipment[i].EntryDate > EndTime && marineEquipment[i + 1].ExitDate < StarTime)
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }
            //            if (marineEquipment[marineEquipment.Count - 1].ExitDate != null)
            //            {
            //                // 最小进场时间小于传入进场时间且最小退场时间小于最小传入退场时间
            //                if (marineEquipment[0].ExitDate < StarTime)
            //                {
            //                    return true;
            //                }
            //                else if (marineEquipment[marineEquipment.Count - 1].EntryDate > EndTime)
            //                {
            //                    return true;
            //                }
            //            }
            //        }
            //        else if ((marineEquipment[0].EntryDate > StarTime && marineEquipment[0].ExitDate > EndTime) || (marineEquipment[0].EntryDate < StarTime && marineEquipment[0].ExitDate < EndTime))
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        if (marineEquipment[marineEquipment.Count - 1].ExitDate != null && marineEquipment[marineEquipment.Count - 1].ExitDate < StarTime)
            //        {
            //            return true;
            //        }
            //    }
            //}
            //if (DeviceType == 2)//陆域设备
            //{
            //    var landDevice = await dbContext.Queryable<LandDevice>().Where(x => x.IsDelete == 1 && x.EquipmentModel == DeviceName && x.ReportingMonth == DatetimeMonth)
            //        .WhereIF(Id != null, x => x.Id != Id)
            //        .OrderByDescending(x => x.EntryDate).ToListAsync();
            //    if (!landDevice.Any())
            //    {
            //        return true;
            //    }
            //    if (EndTime != null)
            //    {
            //        if (landDevice.Count > 1)
            //        {
            //            //传入进场时间大于最小进场时间且小于最大进场时间
            //            if (landDevice[landDevice.Count - 1].EntryDate < StarTime && landDevice[0].EntryDate > StarTime)
            //            {
            //                for (int i = 0; i < landDevice.Count - 1; i++)
            //                {
            //                    if (landDevice[i].EntryDate > EndTime && landDevice[i + 1].ExitDate < StarTime)
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }
            //            if (landDevice[landDevice.Count - 1].ExitDate != null)
            //            {
            //                // 最小进场时间小于传入进场时间且最小退场时间小于最小传入退场时间
            //                if (landDevice[0].ExitDate < StarTime)
            //                {
            //                    return true;
            //                }
            //                else if (landDevice[landDevice.Count - 1].EntryDate > EndTime)
            //                {
            //                    return true;
            //                }
            //            }
            //        }
            //        else if ((landDevice[0].EntryDate > StarTime && landDevice[0].ExitDate > EndTime) || (landDevice[0].EntryDate < StarTime && landDevice[0].ExitDate < EndTime))
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        if (landDevice[landDevice.Count - 1].ExitDate != null && landDevice[landDevice.Count - 1].ExitDate < StarTime)
            //        {
            //            return true;
            //        }
            //    }
            //}
            //else if (DeviceType == 3)//特种设备
            //{
            //    var specialEquipment = await dbContext.Queryable<SpecialEquipment>().Where(x => x.IsDelete == 1 && x.EquipmentModel == DeviceName && x.ReportingMonth == DatetimeMonth)
            //        .WhereIF(Id != null, x => x.Id != Id)
            //        .OrderByDescending(x => x.EntryDate).ToListAsync();
            //    if (!specialEquipment.Any())
            //    {
            //        return true;
            //    }
            //    if (EndTime != null)
            //    {
            //        if (specialEquipment.Count > 1)
            //        {
            //            //传入进场时间大于最小进场时间且小于最大进场时间
            //            if (specialEquipment[specialEquipment.Count - 1].EntryDate < StarTime && specialEquipment[0].EntryDate > StarTime)
            //            {
            //                for (int i = 0; i < specialEquipment.Count - 1; i++)
            //                {
            //                    if (specialEquipment[i].EntryDate > EndTime && specialEquipment[i + 1].ExitDate < StarTime)
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }
            //            if (specialEquipment[specialEquipment.Count - 1].ExitDate != null)
            //            {
            //                // 最小进场时间小于传入进场时间且最小退场时间小于最小传入退场时间
            //                if (specialEquipment[0].ExitDate < StarTime)
            //                {
            //                    return true;
            //                }
            //                else if (specialEquipment[specialEquipment.Count - 1].EntryDate > EndTime)
            //                {
            //                    return true;
            //                }
            //            }
            //        }
            //        else if ((specialEquipment[0].EntryDate > StarTime && specialEquipment[0].ExitDate > EndTime) || (specialEquipment[0].EntryDate < StarTime && specialEquipment[0].ExitDate < EndTime))
            //        {
            //            return true;
            //        }
            //    }
            //}
            //return false;
        }
        /// <summary>
        /// 获取设备导出信息
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<DeviceManagementExportResponseDto>> SearchEquipmentManagementExportAsync(SearchEquipmentManagementRequestDto searchEquipmentManagementRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<DeviceManagementExportResponseDto>();
            var project = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            var shipPingType = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1).ToListAsync();
            var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
            var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1).ToListAsync();
            DeviceManagementExportResponseDto deviceManagementExportResponseDto = new DeviceManagementExportResponseDto();
            RefAsync<int> total = 0;
            DateTime? dateTime;
            int time = 0;
            #region 时间判定
            if (searchEquipmentManagementRequestDto.StarTime == null && searchEquipmentManagementRequestDto.EndTime == null)
            {

                if (DateTime.Now.Day < 26)
                {
                    time = DateTime.Now.ToDateMonth();
                    deviceManagementExportResponseDto.Title = DateTime.Now.ToString("yyyy年-MM月") + "设备信息";
                }
                else
                {
                    time = DateTime.Now.AddMonths(+1).ToDateMonth();
                    deviceManagementExportResponseDto.Title = DateTime.Now.AddMonths(+1).ToString("yyyy年-MM月") + "设备信息";
                }
            }
            else
            {
                if (searchEquipmentManagementRequestDto.StarTime == searchEquipmentManagementRequestDto.EndTime)
                {
                    deviceManagementExportResponseDto.Title = searchEquipmentManagementRequestDto.StarTime.Value.ToString("yyyy年-MM月") + "设备信息";
                }
                deviceManagementExportResponseDto.Title = searchEquipmentManagementRequestDto.StarTime.Value.ToString("yyyy年-MM月") + "--" + searchEquipmentManagementRequestDto.EndTime.Value.ToString("yyyy年-MM月") + "设备信息";
            }
            #endregion
            var marineEquipmentList = await dbContext.Queryable<MarineEquipment>()
                .Where(x => x.IsDelete == 1)
                .WhereIF(time != 0, x => x.ReportingMonth == time)
                .WhereIF(searchEquipmentManagementRequestDto.StarTime.HasValue && searchEquipmentManagementRequestDto.EndTime.HasValue,
                x => x.ReportingMonth >= searchEquipmentManagementRequestDto.StarTime.Value.ToDateMonth() && x.ReportingMonth <= searchEquipmentManagementRequestDto.EndTime.Value.ToDateMonth())
                 .WhereIF(searchEquipmentManagementRequestDto.CompanyId != null, x => x.CompanyId == searchEquipmentManagementRequestDto.CompanyId)
                 .WhereIF(searchEquipmentManagementRequestDto.ProjectId != null, x => x.ProjectId == searchEquipmentManagementRequestDto.ProjectId)
                 .OrderByDescending(x => x.ReportingMonth)
                .ToListAsync();
            foreach (var item in marineEquipmentList)
            {
                ConvertHelper.TryParseFromDateMonth(item.ReportingMonth, out DateTime dateTime1);
                ExportMarineEquipment searchEquipmentManagementResponseDto1 = new ExportMarineEquipment()
                {
                    ReportingMonth = dateTime1.ToString("yyyy年-MM月"),
                    CompanyName = institution.SingleOrDefault(x => x.PomId == item.CompanyId)?.Name,
                    ProjectName = project.SingleOrDefault(x => x.Id == item.ProjectId)?.Name,
                    SubcontractorName = item.SubcontractorName,
                    ContractAmount = item.ContractAmount,
                    ShipTypeName = equipmentList.FirstOrDefault(x => x.Id == item.ShipTypeId)?.DeviceName,
                    ShipName = item.ShipName,
                    NavigationAreaName = equipmentList.SingleOrDefault(x => x.Id == item.NavigationAreaId)?.DeviceName,
                    MMSI = item.MMSI,
                    ShipOwner = item.ShipOwner,
                    CompletionDate = item.CompletionDate,
                    ShipSizeSpecifications = item.ShipSizeSpecifications,
                    Specification1 = item.Specification1,
                    Specification2 = item.Specification2,
                    ShipCapacity = item.ShipCapacity,
                    EntryDate = item.EntryDate,
                    ExitDate = item.ExitDate,
                    PresentNot = item.PresentNot == 1 ? "是" : "否",
                    Notes = item.Notes
                };
                deviceManagementExportResponseDto.exportMarineEquipment.Add(searchEquipmentManagementResponseDto1);
            }
            var landDevice = await dbContext.Queryable<LandDevice>()
                .Where(x => x.IsDelete == 1)
                 .WhereIF(time != 0, x => x.ReportingMonth == time)
                .WhereIF(searchEquipmentManagementRequestDto.StarTime.HasValue && searchEquipmentManagementRequestDto.EndTime.HasValue,
                x => x.ReportingMonth >= searchEquipmentManagementRequestDto.StarTime.Value.ToDateMonth() && x.ReportingMonth <= searchEquipmentManagementRequestDto.EndTime.Value.ToDateMonth())
                 .WhereIF(searchEquipmentManagementRequestDto.CompanyId != null, x => x.CompanyId == searchEquipmentManagementRequestDto.CompanyId)
                 .WhereIF(searchEquipmentManagementRequestDto.ProjectId != null, x => x.ProjectId == searchEquipmentManagementRequestDto.ProjectId)
                 .OrderByDescending(x => x.ReportingMonth)
              .ToListAsync();
            foreach (var item in landDevice)
            {
                ConvertHelper.TryParseFromDateMonth(item.ReportingMonth, out DateTime dateTime1);
                ExportLandEquipment exportLandEquipment = new ExportLandEquipment()
                {
                    ReportingMonth = dateTime1.ToString("yyyy年-MM月"),
                    CompanyName = institution.FirstOrDefault(x => x.PomId == item.CompanyId)?.Name,
                    ProjectName = project.FirstOrDefault(x => x.Id == item.ProjectId)?.Name,
                    SubcontractorName = item.SubcontractorName,
                    ContractAmount = item.ContractAmount,
                    EquipmentCategoryName = equipmentList.FirstOrDefault(x => x.Id == item.EquipmentCategoryId)?.DeviceName,
                    DeviceClassName = equipmentList.FirstOrDefault(x => x.Id == item.DeviceClassId)?.DeviceName,
                    EquipmentSubcategoriesName = equipmentList.FirstOrDefault(x => x.Id == item.EquipmentSubcategoriesId)?.DeviceName,
                    EquipmentModel = item.EquipmentModel,
                    Manufacturer = item.Manufacturer,
                    FactoryDate = item.FactoryDate,
                    CertificateNumber = item.CertificateNumber,
                    EquipmentSpecifications = item.EquipmentSpecifications,
                    Specifications = item.Specifications,
                    EntryDate = item.EntryDate,
                    ExitDate = item.ExitDate,
                    PresentNot = item.PresentNot == 1 ? "是" : "否",
                    Notes = item.Notes,
                };
                deviceManagementExportResponseDto.exportLandEquipment.Add(exportLandEquipment);
            }
            var specialEquipment = await dbContext.Queryable<SpecialEquipment>()
                .Where(x => x.IsDelete == 1)
                 .WhereIF(time != 0, x => x.ReportingMonth == time)
                .WhereIF(searchEquipmentManagementRequestDto.StarTime.HasValue && searchEquipmentManagementRequestDto.EndTime.HasValue,
                x => x.ReportingMonth >= searchEquipmentManagementRequestDto.StarTime.Value.ToDateMonth() && x.ReportingMonth <= searchEquipmentManagementRequestDto.EndTime.Value.ToDateMonth())
                 .WhereIF(searchEquipmentManagementRequestDto.CompanyId != null, x => x.CompanyId == searchEquipmentManagementRequestDto.CompanyId)
                 .WhereIF(searchEquipmentManagementRequestDto.ProjectId != null, x => x.ProjectId == searchEquipmentManagementRequestDto.ProjectId)
                 .OrderByDescending(x => x.ReportingMonth)
             .ToListAsync();
            foreach (var item in specialEquipment)
            {
                ConvertHelper.TryParseFromDateMonth(item.ReportingMonth, out DateTime dateTime1);
                var searchEquipmentManagementResponseDto = new ExportSpecialEquipment()
                {
                    ReportingMonth = dateTime1.ToString("yyyy年-MM月"),
                    CompanyName = institution.FirstOrDefault(x => x.PomId == item.CompanyId)?.Name,
                    ProjectName = project.FirstOrDefault(x => x.Id == item.ProjectId)?.Name,
                    SubcontractorName = item.SubcontractorName,
                    ContractAmount = item.ContractAmount,
                    EquipmentCategoryName = equipmentList.FirstOrDefault(x => x.Id == item.EquipmentCategoryId)?.DeviceName,
                    EquipmentModel = item.EquipmentModel,
                    Manufacturer = item.Manufacturer,
                    FactoryDate = item.FactoryDate,
                    CertificateNumber = item.CertificateNumber,
                    EquipmentSpecifications = item.EquipmentSpecifications,
                    Specifications = item.Specifications,
                    EntryDate = item.EntryDate,
                    ExitDate = item.ExitDate,
                    PresentNot = item.PresentNot == 1 ? "是" : "否",
                    Notes = item.Notes,
                };
                deviceManagementExportResponseDto.exportSpecialEquipment.Add(searchEquipmentManagementResponseDto);
            }
            responseAjaxResult.Data = deviceManagementExportResponseDto;
            responseAjaxResult.Count = deviceManagementExportResponseDto.exportSpecialEquipment.Count + deviceManagementExportResponseDto.exportMarineEquipment.Count + deviceManagementExportResponseDto.exportLandEquipment.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary> 
        /// 水上设备信息导入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> EquipmentManagementImport(List<ExportMarineEquipment> exportMarineEquipment)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            try
            {
                var marineEquipmentList = await dbContext.Queryable<MarineEquipment>().ToListAsync();
                var project = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
                var shipPingType = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1).ToListAsync();
                var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
                var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1).ToListAsync();
                List<MarineEquipment> marineEquipment = new List<MarineEquipment>();
                foreach (var item in exportMarineEquipment)
                {
                    int num = int.Parse(item.ReportingMonth);
                    if (num == 0)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_REPORTINGMONTH_FAIL);
                        return responseAjaxResult;
                    }
                    var institutionId = institution.FirstOrDefault(x => x.Name == item.CompanyName)?.PomId;
                    if (institutionId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_COMPANYNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var projectId = project.FirstOrDefault(x => x.Name == item.ProjectName)?.Id;
                    if (projectId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_PROJECTNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var equipmentShipTypeId = equipmentList.FirstOrDefault(x => x.DeviceName == item.ShipTypeName)?.Id;
                    if (equipmentShipTypeId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_SHIPTYPENAME_FAIL);
                        return responseAjaxResult;
                    }
                    Guid? equipmentNavigationAreaId = null;
                    if (item.NavigationAreaName != null)
                    {
                        item.NavigationAreaName = item.NavigationAreaName.Replace("\n", ""); // 去除所有空格
                        equipmentNavigationAreaId = equipmentList.FirstOrDefault(x => x.DeviceName == item.NavigationAreaName)?.Id;
                        if (equipmentNavigationAreaId == null)
                        {
                            responseAjaxResult.Data = false;
                            responseAjaxResult.Success(ResponseMessage.EQUIPMENT_NAVIGATIONAREANAME_FAIL);
                            return responseAjaxResult;
                        }
                    }
                    var marineEquipments = marineEquipmentList.Where(x => x.IsDelete == 1 && x.ReportingMonth == num && x.ShipName == item.ShipName).ToList(); ;
                    if (marineEquipments != null)
                    {
                        await dbContext.Deleteable(marineEquipments).ExecuteCommandAsync();
                    }
                    //if (!await JudgmentStatus(item.EntryDate, item.ExitDate, item.ShipName, 1, projectId , num))
                    //{
                    //    responseAjaxResult.Data = false;
                    //    responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
                    //    return responseAjaxResult;
                    //}

                    marineEquipment.Add(new MarineEquipment()
                    {
                        Id = Guid.NewGuid(),
                        ReportingMonth = num,
                        CompanyId = institutionId.Value,
                        ProjectId = projectId.Value,
                        SubcontractorName = item.SubcontractorName,
                        ContractAmount = item.ContractAmount,
                        ShipName = item.ShipName,
                        ShipTypeId = equipmentShipTypeId.Value,
                        NavigationAreaId = equipmentNavigationAreaId,
                        MMSI = item.MMSI,
                        ShipOwner = item.ShipOwner,
                        CompletionDate = item.CompletionDate,//.HasValue == false ? default(DateTime) : item.CompletionDate.Value,
                        ShipSizeSpecifications = item.ShipSizeSpecifications,
                        Specification1 = item.Specification1,
                        Specification2 = item.Specification2,
                        ShipCapacity = item.ShipCapacity,
                        EntryDate = item.EntryDate.Value,
                        ExitDate = item.ExitDate,
                        PresentNot = item.PresentNot == "是" ? 1 : 2,
                        Notes = item.Notes,
                    });
                }
                //foreach (var item in marineEquipment)
                //{
                //    if (!await JudgmentStatus(marineEquipment.Where(x => x.Id != item.Id && x.ShipName == item.ShipName && x.ReportingMonth == item.ReportingMonth).OrderByDescending(x => x.EntryDate).ToList(), item.EntryDate, item.ExitDate))
                //    {
                //        responseAjaxResult.Data = false;
                //        responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
                //        return responseAjaxResult;
                //    }
                //}
                await dbContext.Insertable(marineEquipment).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
                return responseAjaxResult;
            }
            catch
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 设备信息导入时间判定
        /// </summary>
        /// <param name="marineEquipment"></param>
        /// <param name="StarTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public async Task<bool> JudgmentStatus(List<MarineEquipment> marineEquipment, DateTime? StarTime, DateTime? EndTime)
        {
            if (!marineEquipment.Any())
            {
                return true;
            }
            if (EndTime != null)
            {
                if (marineEquipment.Count > 1)
                {
                    //传入进场时间大于最小进场时间且小于最大进场时间
                    if (marineEquipment[marineEquipment.Count - 1].EntryDate < StarTime && marineEquipment[0].EntryDate > StarTime)
                    {
                        for (int i = 0; i < marineEquipment.Count - 1; i++)
                        {
                            if (marineEquipment[i].EntryDate > EndTime && marineEquipment[i + 1].ExitDate < StarTime)
                            {
                                return true;
                            }
                        }
                    }
                    if (marineEquipment[marineEquipment.Count - 1].ExitDate != null)
                    {
                        // 最小进场时间小于传入进场时间且最小退场时间小于最小传入退场时间
                        if (marineEquipment[0].ExitDate < StarTime)
                        {
                            return true;
                        }
                        else if (marineEquipment[marineEquipment.Count - 1].EntryDate > EndTime)
                        {
                            return true;
                        }
                    }
                }
                else if ((marineEquipment[0].EntryDate > StarTime && marineEquipment[0].ExitDate > EndTime) || (marineEquipment[0].EntryDate < StarTime && marineEquipment[0].ExitDate < EndTime))
                {
                    return true;
                }
                else if (marineEquipment[0].EntryDate > StarTime && marineEquipment[0].ExitDate == null)
                {
                    return true;
                }
            }
            else
            {
                if (marineEquipment[marineEquipment.Count - 1].ExitDate != null && marineEquipment[marineEquipment.Count - 1].ExitDate < StarTime)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 陆域设备信息导入
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ExportLandEquipmentImport(List<ExportLandEquipment>? exportLandEquipment)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            try
            {
                var project = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
                var shipPingType = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1).ToListAsync();
                var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
                var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1).ToListAsync();
                List<LandDevice> landDevice = new List<LandDevice>();
                foreach (var item in exportLandEquipment)
                {
                    int num = int.Parse(item.ReportingMonth);
                    if (num == 0)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_REPORTINGMONTH_FAIL);
                        return responseAjaxResult;
                    }
                    var institutionId = institution.FirstOrDefault(x => x.Name == item.CompanyName)?.PomId;
                    if (institutionId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_COMPANYNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var projectId = project.FirstOrDefault(x => x.Name == item.ProjectName)?.Id;
                    if (projectId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_PROJECTNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var EquipmentCategoryId = equipmentList.FirstOrDefault(x => x.DeviceName == item.EquipmentCategoryName)?.Id;
                    if (EquipmentCategoryId == null && item.EquipmentCategoryName != null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_CATEGORYNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var DeviceClassId = equipmentList.FirstOrDefault(x => x.DeviceName == item.DeviceClassName)?.Id;
                    if (DeviceClassId == null && item.DeviceClassName != null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_DEVUICECLASSNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var equipmentSubcategoriesId = equipmentList.FirstOrDefault(x => x.DeviceName == item.EquipmentSubcategoriesName)?.Id;
                    if (equipmentSubcategoriesId == null && item.EquipmentSubcategoriesName != null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_SUBCATEGORIESNAME_FAIL);
                        return responseAjaxResult;
                    }
                    //if (!await JudgmentStatus(item.EntryDate, item.ExitDate, item.EquipmentModel, 2, ProjectId: projectId, num))
                    //{
                    //    responseAjaxResult.Data = false;
                    //    responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
                    //    return responseAjaxResult;
                    //}

                    landDevice.Add(new LandDevice()
                    {
                        Id = Guid.NewGuid(),
                        ReportingMonth = num,
                        CompanyId = institutionId.Value,
                        ProjectId = projectId.Value,
                        SubcontractorName = item.SubcontractorName,
                        ContractAmount = item.ContractAmount,
                        EquipmentCategoryId = EquipmentCategoryId,
                        DeviceClassId = DeviceClassId,
                        EquipmentSubcategoriesId = equipmentSubcategoriesId,
                        EquipmentModel = item.EquipmentModel,
                        Manufacturer = item.Manufacturer,
                        FactoryDate = item.FactoryDate,
                        CertificateNumber = item.CertificateNumber,
                        EquipmentSpecifications = item.EquipmentSpecifications,
                        Specifications = item.Specifications,
                        EntryDate = item.EntryDate,
                        ExitDate = item.ExitDate,
                        PresentNot = item.PresentNot == "是" ? 1 : 2,
                        Notes = item.Notes
                    });
                }
                //foreach (var item in landDevice)
                //{
                //    if (!await EquipmentManagementJudgmentStatus(landDevice.Where(x => x.Id != item.Id && x.EquipmentModel == item.EquipmentModel && x.ReportingMonth == item.ReportingMonth).OrderByDescending(x => x.EntryDate).ToList(), item.EntryDate, item.ExitDate))
                //    {
                //        responseAjaxResult.Data = false;
                //        responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
                //        return responseAjaxResult;
                //    }
                //}
                await dbContext.Insertable(landDevice).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
                return responseAjaxResult;
            }
            catch
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }

        }
        /// <summary>
        /// 陆域设备信息导入时间判定
        /// </summary>
        /// <param name="marineEquipment"></param>
        /// <param name="StarTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public async Task<bool> EquipmentManagementJudgmentStatus(List<LandDevice>? exportLandEquipment, DateTime? StarTime, DateTime? EndTime)
        {
            if (!exportLandEquipment.Any())
            {
                return true;
            }
            if (EndTime != null)
            {
                if (exportLandEquipment.Count > 1)
                {
                    //传入进场时间大于最小进场时间且小于最大进场时间
                    if (exportLandEquipment[exportLandEquipment.Count - 1].EntryDate < StarTime && exportLandEquipment[0].EntryDate > StarTime)
                    {
                        for (int i = 0; i < exportLandEquipment.Count - 1; i++)
                        {
                            if (exportLandEquipment[i].EntryDate > EndTime && exportLandEquipment[i + 1].ExitDate < StarTime)
                            {
                                return true;
                            }
                        }
                    }
                    if (exportLandEquipment[exportLandEquipment.Count - 1].ExitDate != null)
                    {
                        // 最小进场时间小于传入进场时间且最小退场时间小于最小传入退场时间
                        if (exportLandEquipment[0].ExitDate < StarTime)
                        {
                            return true;
                        }
                        else if (exportLandEquipment[exportLandEquipment.Count - 1].EntryDate > EndTime)
                        {
                            return true;
                        }
                    }
                }
                else if ((exportLandEquipment[0].EntryDate > StarTime && exportLandEquipment[0].ExitDate > EndTime) || (exportLandEquipment[0].EntryDate < StarTime && exportLandEquipment[0].ExitDate < EndTime))
                {
                    return true;
                }
                else if (exportLandEquipment[0].EntryDate > StarTime && exportLandEquipment[0].ExitDate == null)
                {
                    return true;
                }
            }
            else
            {
                if (exportLandEquipment[exportLandEquipment.Count - 1].ExitDate != null && exportLandEquipment[exportLandEquipment.Count - 1].ExitDate < StarTime)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 特种设备信息导入
        /// </summary>
        /// <param name="exportMarineEquipment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> ExportSpecialEquipmentImport(List<ExportSpecialEquipment> exportMarineEquipment)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            try
            {
                var project = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
                var shipPingType = await dbContext.Queryable<ShipPingType>().Where(x => x.IsDelete == 1).ToListAsync();
                var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
                var equipmentList = await dbContext.Queryable<EquipmentList>().Where(x => x.IsDelete == 1).ToListAsync();
                List<SpecialEquipment> specialEquipment = new List<SpecialEquipment>();
                foreach (var item in exportMarineEquipment)
                {
                    int num = int.Parse(item.ReportingMonth);
                    if (num == 0)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_REPORTINGMONTH_FAIL);
                        return responseAjaxResult;
                    }
                    var institutionId = institution.FirstOrDefault(x => x.Name == item.CompanyName)?.PomId;
                    if (institutionId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_COMPANYNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var projectId = project.FirstOrDefault(x => x.Name == item.ProjectName)?.Id;
                    if (projectId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_PROJECTNAME_FAIL);
                        return responseAjaxResult;
                    }
                    var EquipmentCategoryId = equipmentList.FirstOrDefault(x => x.DeviceName == item.EquipmentCategoryName)?.Id;
                    if (EquipmentCategoryId == null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.EQUIPMENT_CATEGORYNAME_FAIL);
                        return responseAjaxResult;
                    }
                    //if (!await JudgmentStatus(item.EntryDate, item.ExitDate, item.EquipmentModel, 3, ProjectId: projectId, num))
                    //{
                    //    responseAjaxResult.Data = false;
                    //    responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
                    //    return responseAjaxResult;
                    //}
                    specialEquipment.Add(new SpecialEquipment()
                    {
                        Id = Guid.NewGuid(),
                        ReportingMonth = num,
                        CompanyId = institutionId.Value,
                        ProjectId = projectId.Value,
                        SubcontractorName = item.SubcontractorName,
                        ContractAmount = item.ContractAmount,
                        EquipmentCategoryId = EquipmentCategoryId,
                        EquipmentModel = item.EquipmentModel,
                        Manufacturer = item.Manufacturer,
                        FactoryDate = item.FactoryDate,
                        CertificateNumber = item.CertificateNumber,
                        EquipmentSpecifications = item.EquipmentSpecifications,
                        Specifications = item.Specifications,
                        SpecialEquipmentNumber = item.SpecialEquipmentNumber,
                        EntryDate = item.EntryDate,
                        ExitDate = item.ExitDate,
                        PresentNot = item.PresentNot == "是" ? 1 : 2,
                        Notes = item.Notes
                    });
                }
                //foreach (var item in specialEquipment)
                //{
                //    if (!await ExportSpecialEquipmentJudgmentStatus(specialEquipment.Where(x => x.Id != item.Id && x.EquipmentModel == item.EquipmentModel && x.ReportingMonth == item.ReportingMonth).OrderByDescending(x => x.EntryDate).ToList(), item.EntryDate, item.ExitDate))
                //    {
                //        responseAjaxResult.Data = false;
                //        responseAjaxResult.Success(ResponseMessage.DEVICE_IMPORT_FAIL);
                //        return responseAjaxResult;
                //    }
                //}
                await dbContext.Insertable(specialEquipment).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
                return responseAjaxResult;
            }

            catch
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }
        /// <summary>
        /// 特种设备信息导入时间判定
        /// </summary>
        /// <param name="marineEquipment"></param>
        /// <param name="StarTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public async Task<bool> ExportSpecialEquipmentJudgmentStatus(List<SpecialEquipment>? exportLandEquipment, DateTime? StarTime, DateTime? EndTime)
        {
            if (!exportLandEquipment.Any())
            {
                return true;
            }
            if (EndTime != null)
            {
                if (exportLandEquipment.Count > 1)
                {
                    //传入进场时间大于最小进场时间且小于最大进场时间
                    if (exportLandEquipment[exportLandEquipment.Count - 1].EntryDate < StarTime && exportLandEquipment[0].EntryDate > StarTime)
                    {
                        for (int i = 0; i < exportLandEquipment.Count - 1; i++)
                        {
                            if (exportLandEquipment[i].EntryDate > EndTime && exportLandEquipment[i + 1].ExitDate < StarTime)
                            {
                                return true;
                            }
                        }
                    }
                    if (exportLandEquipment[exportLandEquipment.Count - 1].ExitDate != null)
                    {
                        // 最小进场时间小于传入进场时间且最小退场时间小于最小传入退场时间
                        if (exportLandEquipment[0].ExitDate < StarTime)
                        {
                            return true;
                        }
                        else if (exportLandEquipment[exportLandEquipment.Count - 1].EntryDate > EndTime)
                        {
                            return true;
                        }
                    }
                }
                else if ((exportLandEquipment[0].EntryDate > StarTime && exportLandEquipment[0].ExitDate > EndTime) || (exportLandEquipment[0].EntryDate < StarTime && exportLandEquipment[0].ExitDate < EndTime))
                {
                    return true;
                }
                else if (exportLandEquipment[0].EntryDate > StarTime && exportLandEquipment[0].ExitDate == null)
                {
                    return true;
                }
            }
            else
            {
                if (exportLandEquipment[exportLandEquipment.Count - 1].ExitDate != null && exportLandEquipment[exportLandEquipment.Count - 1].ExitDate < StarTime)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 水上设备月报自动生成
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddMarineEquipmentAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (DateTime.Now.Day == 1)
            {
                var marineEquipmentList = await dbContext.Queryable<MarineEquipment>().Where(x => x.IsDelete == 1 && x.PresentNot == 1 && x.ReportingMonth == DateTime.Now.AddMonths(-1).ToDateMonth()).ToListAsync();
                if (marineEquipmentList != null)
                {
                    foreach (var item in marineEquipmentList)
                    {
                        item.Id = Guid.NewGuid();
                        item.ReportingMonth = DateTime.Now.ToDateMonth();
                    }
                    await dbContext.Insertable(marineEquipmentList).ExecuteCommandAsync();
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL);
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteMarineEquipmentAsync(BasePullDownResponseDto basePullDownResponseDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (basePullDownResponseDto.Type == 1)
            {
                var MarineEquipment = await dbContext.Queryable<MarineEquipment>().Where(x => x.Id == basePullDownResponseDto.Id).SingleAsync();
                if (MarineEquipment == null)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                await dbContext.Deleteable(MarineEquipment).ExecuteCommandAsync();
            }
            else if (basePullDownResponseDto.Type == 2)
            {
                var landDevice = await dbContext.Queryable<LandDevice>().Where(x => x.Id == basePullDownResponseDto.Id).SingleAsync();
                if (landDevice == null)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                await dbContext.Deleteable(landDevice).ExecuteCommandAsync();
            }
            else if (basePullDownResponseDto.Type == 3)
            {
                var specialEquipment = await dbContext.Queryable<SpecialEquipment>().Where(x => x.Id == basePullDownResponseDto.Id).SingleAsync();
                if (specialEquipment == null)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                await dbContext.Deleteable(specialEquipment).ExecuteCommandAsync();
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_FAIL);
                return responseAjaxResult;
            }
            responseAjaxResult.Data = true;
            responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
            return responseAjaxResult;
        }

        /// <summary>
        /// 特种设备月报自动生成
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddMSpecialEquipmentAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var specialEquipmentList = await dbContext.Queryable<SpecialEquipment>().Where(x => x.IsDelete == 1 && x.PresentNot == 1 && x.ReportingMonth == DateTime.Now.AddMonths(-1).ToDateMonth()).ToListAsync();
            if (specialEquipmentList != null)
            {
                foreach (var item in specialEquipmentList)
                {
                    item.Id = Guid.NewGuid();
                    item.ReportingMonth = DateTime.Now.ToDateMonth();
                }
                await dbContext.Insertable(specialEquipmentList).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 陆域设备月报自动生成
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddLandEquipmentAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var landEquipmentList = await dbContext.Queryable<LandDevice>().Where(x => x.IsDelete == 1 && x.PresentNot == 1 && x.ReportingMonth == DateTime.Now.AddMonths(-1).ToDateMonth()).ToListAsync();
            if (landEquipmentList != null)
            {
                foreach (var item in landEquipmentList)
                {
                    item.Id = Guid.NewGuid();
                    item.ReportingMonth = DateTime.Now.ToDateMonth();
                }
                await dbContext.Insertable(landEquipmentList).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
    }
}

