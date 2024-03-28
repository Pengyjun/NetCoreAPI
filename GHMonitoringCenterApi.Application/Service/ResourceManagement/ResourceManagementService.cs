using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.ResourceManagement;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.ResourceManagement
{
    /// <summary>
    /// 资源业务实现层
    /// </summary>
    public class ResourceManagementService : IResourceManagementService
    {
        public ISqlSugarClient dbContext { get; set; }
        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        public ResourceManagementService(ISqlSugarClient dbContext, GlobalObject globalObject)
        {
            this.dbContext = dbContext;
            _globalObject = globalObject;
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        /// <summary>
        /// 获取船舶列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SearchShipTabulationRequestDto>>> SearchResourcesAsync(SearchShipTabulationResponseDto searchShipTabulationResponseDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SearchShipTabulationRequestDto>>();
            if (searchShipTabulationResponseDto.Type == ConstructionOutPutType.Self)
            {
                //资源扩展表数据
                var extendedData = await dbContext.Queryable<ShipResources>()
                   .LeftJoin<DictionaryTable>((x, z) => x.DictionaryTableNameId == z.Id)
                   .Where(x => x.IsDelete == 1 && x.Type == ConstructionOutPutType.Self)
                   .WhereIF(!string.IsNullOrWhiteSpace(searchShipTabulationResponseDto.KeyWords), (x, z) => z.Name.Contains(searchShipTabulationResponseDto.KeyWords))
                   .Select((x, z) => new SearchShipTabulationRequestDto { Id = x.DictionaryTableNameId.Value, Name = z.Name, Type = x.Type, TypeName = z.Name, RequestType = true }).ToListAsync();
                //自有船舶表数据
                var shippingList = new List<SearchShipTabulationRequestDto>();
                shippingList = await dbContext.Queryable<OwnerShip>()
                    .LeftJoin<Institution>((x, y) => x.CompanyId == y.PomId)
                .Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(searchShipTabulationResponseDto.KeyWords), x => x.Name.Contains(searchShipTabulationResponseDto.KeyWords))
                .Select((x, y) => new SearchShipTabulationRequestDto { Id = x.PomId, Name = x.Name, DealingUnitName = y.Name, TypeName = "自有船舶", RequestType = false })
               .ToListAsync();
                extendedData.AddRange(shippingList);
                int skipCount = (searchShipTabulationResponseDto.PageIndex - 1) * searchShipTabulationResponseDto.PageSize;
                var resourcesList = extendedData.Skip(skipCount).Take(searchShipTabulationResponseDto.PageSize).ToList();
                responseAjaxResult.Data = resourcesList;
                responseAjaxResult.Count = extendedData.Count;
                responseAjaxResult.Success();
            }
            else if (searchShipTabulationResponseDto.Type == ConstructionOutPutType.SubPackage || searchShipTabulationResponseDto.Type == ConstructionOutPutType.SubOwner)
            {
                //资源扩展表数据
                var extendedData = await dbContext.Queryable<ShipResources>()
                   .LeftJoin<DealingUnit>((x, z) => x.DealingUnitsId == z.PomId)
                   .LeftJoin<DictionaryTable>((x, z, t) => x.DictionaryTableNameId == t.Id)
                   .LeftJoin<OwnerShip>((x, z, t, y) => x.ShipPingId == y.Id)
                   .Where(x => x.IsDelete == 1 && (x.Type == ConstructionOutPutType.SubPackage || x.Type == ConstructionOutPutType.SubOwner))
                    .WhereIF(!string.IsNullOrWhiteSpace(searchShipTabulationResponseDto.KeyWords), (x, z, t) => t.Name.Contains(searchShipTabulationResponseDto.KeyWords))
                   .Select((x, z, t, y) => new SearchShipTabulationRequestDto
                   {
                       Id = x.DictionaryTableNameId.Value,
                       Name = t.Name,
                       Type = x.Type,
                       OwnerShipPingId = x.ShipPingId,
                       OwnerShipPingName = y.Name,
                       DealingUnitName = z.ZBPNAME_ZH,
                       DealingUnitId = z.PomId.ToString(),
                       TypeName = x.Type == ConstructionOutPutType.SubOwner ? "自有船舶" : t.Name,
                       RequestType = true
                   }).ToListAsync();

                var shippingList = new List<SearchShipTabulationRequestDto>();
                var dealingUnit = new List<SearchShipTabulationRequestDto>();
                //分包船舶表数据
                var shippingLists = await dbContext.Queryable<SubShip>()
                .Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(searchShipTabulationResponseDto.KeyWords), x => x.Name.Contains(searchShipTabulationResponseDto.KeyWords)).ToListAsync();
                var ids = shippingLists.Select(x => x.CompanyId).ToList();
                var dealingUnits = await dbContext.Queryable<DealingUnit>().Where(x => ids.Contains(x.PomId.ToString())).ToListAsync();
                foreach (var item in shippingLists)
                {
                    var a = new SearchShipTabulationRequestDto()
                    {
                        Id = item.PomId,
                        Name = item.Name,
                        TypeName = "分包船舶",
                        DealingUnitId = item.CompanyId,
                        RequestType = false
                    };
                    if (!string.IsNullOrWhiteSpace(item.CompanyId))
                    {
                        a.DealingUnitName = dealingUnits.FirstOrDefault(x => x.PomId == item.CompanyId.ToGuid())?.ZBPNAME_ZH;
                    }
                    shippingList.Add(a);
                }
                //往来单位数据
                dealingUnit = await dbContext.Queryable<DealingUnit>().Where(x => x.IsDelete == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(searchShipTabulationResponseDto.KeyWords), x => x.ZBPNAME_ZH.Contains(searchShipTabulationResponseDto.KeyWords))
                    .Select(x => new SearchShipTabulationRequestDto { Id = x.PomId.Value, Name = x.ZBPNAME_ZH, TypeName = "往来单位", RequestType = false })
                    .ToListAsync();

                //业务判断
                extendedData.AddRange(shippingList);
                extendedData.AddRange(dealingUnit);
                int skipCount = (searchShipTabulationResponseDto.PageIndex - 1) * searchShipTabulationResponseDto.PageSize;
                //var resourcesList = extendedData.Skip(skipCount).Take(searchShipTabulationResponseDto.PageSize).ToList();
                responseAjaxResult.Data = resourcesList;
                responseAjaxResult.Count = extendedData.Count;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 保存船舶列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveResourcesAsync(SaveShipPingTabulationResponseDto saveShipPingTabulationResponseDto)
        {
            var type = await dbContext.Queryable<DictionaryTable>().Where(x => x.IsDelete == 1 && x.TypeNo == 8).Select(x => x.Type).ToListAsync();
            var typeMax = type.Max() + 1;
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (saveShipPingTabulationResponseDto.RequestType)
            {
                if (saveShipPingTabulationResponseDto.DealingUnitId != null && saveShipPingTabulationResponseDto.ShipPingId != null)
                {
                    var dealingUnits = await dbContext.Queryable<DealingUnit>().Where(x => x.PomId == saveShipPingTabulationResponseDto.DealingUnitId && x.IsDelete == 1).SingleAsync();
                    var shipResourcesList = await dbContext.Queryable<ShipResources>().Where(x => x.ShipPingId == saveShipPingTabulationResponseDto.ShipPingId && x.IsDelete == 1).FirstAsync();
                    var owmship = await dbContext.Queryable<OwnerShip>().Where(x => x.PomId == saveShipPingTabulationResponseDto.ShipPingId && x.IsDelete == 1).FirstAsync();
                    if (shipResourcesList != null)
                    {
                        responseAjaxResult.Success(ResponseMessage.OPERATION_DEALINGUNIT_FAIL);
                        return responseAjaxResult;
                    }
                    if (dealingUnits != null && owmship != null)
                    {
                        saveShipPingTabulationResponseDto.Name = dealingUnits.ZBPNAME_ZH + "__" + owmship.Name;
                    }
                }
                DictionaryTable dictionaryTable = new DictionaryTable()
                {
                    Id = GuidUtil.Next(),
                    Name = saveShipPingTabulationResponseDto.Name,
                    TypeNo = 8,
                    Type = typeMax
                };
                ShipResources shipResources = new ShipResources()
                {
                    Id = GuidUtil.Next(),
                    DictionaryTableNameId = dictionaryTable.Id,
                    Type = saveShipPingTabulationResponseDto.Type,
                    ShipPingId = saveShipPingTabulationResponseDto.ShipPingId,
                    DealingUnitsId = saveShipPingTabulationResponseDto.DealingUnitId
                };
                #region 日志信息
                LogInfo logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    OperationId = _currentUser.Id,
                    BusinessModule = "进度与成本管控/产值属性与资源管理/新增",
                    BusinessRemark = "进度与成本管控/产值属性与资源管理/新增",
                    OperationName = _currentUser.Name,
                };
                #endregion
                var ass = await dbContext.Insertable(shipResources).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                await dbContext.Insertable(dictionaryTable).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                if (ass > 0)
                {
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success();
                }
                else
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL);
                }
            }
            else
            {
                var shipResources = await dbContext.Queryable<ShipResources>().Where(x => x.IsDelete == 1 && x.DictionaryTableNameId == saveShipPingTabulationResponseDto.Id).SingleAsync();
                if (shipResources != null)
                {
                    var dictionaryTable = await dbContext.Queryable<DictionaryTable>().Where(x => x.Id == shipResources.DictionaryTableNameId).SingleAsync();
                    if (dictionaryTable != null)
                    {
                        if (saveShipPingTabulationResponseDto.DealingUnitId != null && saveShipPingTabulationResponseDto.ShipPingId != null)
                        {
                            var dealingUnits = await dbContext.Queryable<DealingUnit>().Where(x => x.PomId == saveShipPingTabulationResponseDto.DealingUnitId && x.IsDelete == 1).SingleAsync();
                            var shipResourcesList = await dbContext.Queryable<ShipResources>().Where(x => x.ShipPingId == saveShipPingTabulationResponseDto.ShipPingId && x.IsDelete == 1).SingleAsync();
                            var owmship = await dbContext.Queryable<OwnerShip>().Where(x => x.PomId == saveShipPingTabulationResponseDto.ShipPingId && x.IsDelete == 1).FirstAsync();

                            if (shipResourcesList != null)
                            {
                                responseAjaxResult.Success(ResponseMessage.OPERATION_DEALINGUNIT_FAIL);
                                return responseAjaxResult;
                            }
                            dictionaryTable.Name = dealingUnits + "__" + owmship.Name;
                        }
                        else
                        {
                            dictionaryTable.Name = saveShipPingTabulationResponseDto.Name;
                        }
                        shipResources.Type = saveShipPingTabulationResponseDto.Type;
                        shipResources.ShipPingId = saveShipPingTabulationResponseDto.ShipPingId;
                        shipResources.DealingUnitsId = saveShipPingTabulationResponseDto.DealingUnitId;
                        #region 日志信息
                        LogInfo logDto = new LogInfo()
                        {
                            Id = GuidUtil.Increment(),
                            OperationId = _currentUser.Id,
                            BusinessModule = "进度与成本管控/产值属性与资源管理/编辑",
                            BusinessRemark = "进度与成本管控/产值属性与资源管理/编辑",
                            OperationName = _currentUser.Name,
                            DataId = shipResources.Id
                        };
                        #endregion
                        var ass = await dbContext.Updateable(shipResources).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                        await dbContext.Updateable(dictionaryTable).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                        if (ass > 0)
                        {
                            responseAjaxResult.Data = true;
                            responseAjaxResult.Success();
                        }
                        else
                        {
                            responseAjaxResult.Data = false;
                            responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL);
                        }
                    }
                }
            }
            #region  暂时不用
            //if (saveShipPingTabulationResponseDto.RequestType)
            //{
            //        if (saveShipPingTabulationResponseDto.DealingUnitsId != null && saveShipPingTabulationResponseDto.ShipPingId!=null)
            //        {
            //            var dealingUnits = await dbContext.Queryable<DealingUnit>().Where(x => x.PomId == saveShipPingTabulationResponseDto.DealingUnitsId).SingleAsync();
            //            var shipResourcesList = await dbContext.Queryable<ShipResources>().Where(x => x.ShipPingId == saveShipPingTabulationResponseDto.ShipPingId).SingleAsync();
            //            var subShip = await dbContext.Queryable<SubShip>().Where(x => x.PomId == saveShipPingTabulationResponseDto.ShipPingId).FirstAsync();
            //            var owmship = await dbContext.Queryable<OwnerShip>().Where(x => x.PomId == saveShipPingTabulationResponseDto.ShipPingId).FirstAsync();

            //        if (shipResourcesList.DealingUnitsId != null || subShip.ConstructionArea != null)
            //            {
            //                responseAjaxResult.Success(ResponseMessage.OPERATION_DEALINGUNIT_FAIL);
            //                return responseAjaxResult;
            //            }
            //        saveShipPingTabulationResponseDto.Name = dealingUnits + "-" + (subShip.Name == null ? owmship.Name : subShip.Name);
            //    }
            //    DictionaryTable dictionaryTable = new DictionaryTable()
            //    {
            //        Id = GuidUtil.Next(),
            //        Name = saveShipPingTabulationResponseDto.Name,
            //        TypeNo = 8,
            //        Type = typeMax
            //    };
            //        ShipResources shipResources = new ShipResources()
            //        {
            //            Id = GuidUtil.Next(),
            //            DictionaryTableNameId = dictionaryTable.Id,
            //            CategoryId = saveShipPingTabulationResponseDto.CategoryId,
            //            Type = saveShipPingTabulationResponseDto.Attribute,
            //            ShipPingId = saveShipPingTabulationResponseDto.ShipPingId,
            //            DealingUnitsId = saveShipPingTabulationResponseDto.DealingUnitsId
            //        };

            //        var ass = await dbContext.Insertable(shipResources).ExecuteCommandAsync();
            //          await dbContext.Insertable(dictionaryTable).ExecuteCommandAsync();
            //    if (ass > 0)
            //    {
            //        responseAjaxResult.Data = true;
            //        responseAjaxResult.Success();
            //    }
            //    else 
            //    {
            //        responseAjaxResult.Data = false;
            //        responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL);
            //    }
            //}
            //else
            //{
            //    var shipResources = await dbContext.Queryable<ShipResources>().Where(x => x.IsDelete == 1 && x.Id == saveShipPingTabulationResponseDto.Id).SingleAsync();
            //    if (shipResources != null)
            //    {
            //        var dictionaryTable = await dbContext.Queryable<DictionaryTable>().Where(x => x.Id == shipResources.DictionaryTableNameId).SingleAsync();
            //        if (dictionaryTable != null)
            //        {
            //            var category = await dbContext.Queryable<ShipCategory>().Where(x => x.IsDelete == 1 && x.Id == saveShipPingTabulationResponseDto.CategoryId).SingleAsync();
            //            if (saveShipPingTabulationResponseDto.DealingUnitsId != null && saveShipPingTabulationResponseDto.ShipPingId != null)
            //            {
            //                var dealingUnits = await dbContext.Queryable<DealingUnit>().Where(x => x.PomId == saveShipPingTabulationResponseDto.DealingUnitsId).SingleAsync();
            //                var shipResourcesList = await dbContext.Queryable<ShipResources>().Where(x => x.ShipPingId == saveShipPingTabulationResponseDto.ShipPingId).SingleAsync();
            //                var subShip = await dbContext.Queryable<SubShip>().Where(x => x.PomId == saveShipPingTabulationResponseDto.ShipPingId).FirstAsync();
            //                var owmship = await dbContext.Queryable<OwnerShip>().Where(x => x.PomId == saveShipPingTabulationResponseDto.ShipPingId).FirstAsync();

            //                if (shipResourcesList.DealingUnitsId != null || subShip.ConstructionArea != null)
            //                {
            //                    responseAjaxResult.Success(ResponseMessage.OPERATION_DEALINGUNIT_FAIL);
            //                    return responseAjaxResult;
            //                }
            //                dictionaryTable.Name = dealingUnits + "-" + (subShip.Name == null ? owmship.Name : subShip.Name);
            //            }
            //            shipResources.CategoryId = saveShipPingTabulationResponseDto.CategoryId;
            //            shipResources.ShipPingId = saveShipPingTabulationResponseDto.ShipPingId;
            //            shipResources.Type = category.shipType;
            //            var ass = await dbContext.Updateable(shipResources).ExecuteCommandAsync();
            //            await dbContext.Updateable(dictionaryTable).ExecuteCommandAsync();
            //            if (ass > 0)
            //            {
            //                responseAjaxResult.Data = true;
            //                responseAjaxResult.Success();
            //            }
            //            else
            //            {
            //                responseAjaxResult.Data = false;
            //                responseAjaxResult.Fail(ResponseMessage.OPERATION_SAVE_FAIL);
            //            }
            //        }
            //    }
            //}
            #endregion
            return responseAjaxResult;
        }
        /// <summary>
        /// 删除船舶资源
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteResourcesAsync(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var shipResources = await dbContext.Queryable<ShipResources>().Where(x => x.DictionaryTableNameId == basePrimaryRequestDto.Id).SingleAsync();
            if (shipResources == null)
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL);
                return responseAjaxResult;
            }
            shipResources.IsDelete = 0;
            #region 日志信息
            LogInfo logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                OperationId = _currentUser.Id,
                BusinessModule = "进度与成本管控/产值属性与资源管理/删除",
                BusinessRemark = "进度与成本管控/产值属性与资源管理/删除",
                OperationName = _currentUser.Name,
            };
            #endregion
            var ass = await dbContext.Updateable(shipResources).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            if (ass > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL);
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取船舶修理滚动计划表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchShipRepairRollingResponseDto>>> SearchShipRepairRolling()
        {

            var responseAjaxResult = new ResponseAjaxResult<List<SearchShipRepairRollingResponseDto>>();
            List<SearchShipRepairRollingResponseDto> searchShipRepairRollingResponseDtos = new List<SearchShipRepairRollingResponseDto>();
            var ownerShip = await dbContext.Queryable<OwnerShip>().Where(x => CommonData.ShipTypes.Contains(x.TypeId)).ToListAsync();
            var shipPingType = await dbContext.Queryable<ShipPingType>().ToListAsync();
            var shipRepairRolling = await dbContext.Queryable<ShipRepairRolling>().ToListAsync();
            var userList = await dbContext.Queryable<Domain.Models.User>().ToListAsync();
            foreach (var item in ownerShip)
            {
                var shipRepairRollings = shipRepairRolling.Where(x => x.ShipId == item.PomId).OrderByDescending(x => x.CreateTime).FirstOrDefault();
                if (shipRepairRollings == null)
                {
                    searchShipRepairRollingResponseDtos.Add(new SearchShipRepairRollingResponseDto()
                    {
                        ShipId = item.PomId,
                        ShipName = item.Name == "东祥" ? "金广" : item.Name,
                        ShipTypeId = item.TypeId,
                        ShipTypeName = shipPingType.FirstOrDefault(x => x.PomId == item.TypeId).Name,
                        Sort = GetOwnShipSort(item.PomId)
                    });
                }
                else
                {
                    searchShipRepairRollingResponseDtos.Add(new SearchShipRepairRollingResponseDto()
                    {
                        ShipId = item.PomId,
                        ShipName = item.Name == "东祥" ? "金广" : item.Name,
                        ShipTypeId = item.TypeId,
                        ShipTypeName = shipPingType.FirstOrDefault(x => x.PomId == item.TypeId).Name,
                        RepairCategory = shipRepairRollings.RepairCategory,
                        MainRepairContent = shipRepairRollings.MainRepairContent,
                        RepairLocation = shipRepairRollings.RepairLocation,
                        PlannedRepairPeriod = shipRepairRollings.PlannedRepairPeriod, //TimeHelper.GetTimeSpan(shipRepairRollings.ScheduledStartTime.Value, shipRepairRollings.ScheduledEndTime.Value).Days + 1,
                        ScheduledStartTime = shipRepairRollings.ScheduledStartTime,
                        ScheduledEndTime = shipRepairRollings.ScheduledEndTime,
                        ActualStartTime = shipRepairRollings.ActualStartTime,
                        ExpectEndTime = shipRepairRollings.ExpectEndTime,
                        MaintenanceName = shipRepairRollings.MaintenanceName,
                        Remarks = shipRepairRollings.Remarks,
                        Sort = GetOwnShipSort(item.PomId)
                    }); ;
                }
            }
            searchShipRepairRollingResponseDtos = searchShipRepairRollingResponseDtos.OrderBy(x => x.Sort).ToList();
            responseAjaxResult.Count = searchShipRepairRollingResponseDtos.Count;
            responseAjaxResult.Data = searchShipRepairRollingResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 保存船舶修理滚动计划表
        /// </summary>
        /// <param name="saveShipRepairRollingRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveShipRepairRolling(SaveShipRepairRollingRequestDto saveShipRepairRollingRequestDto)
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            ShipRepairRolling shipRepairRolling = new ShipRepairRolling();
            var shipRepairRollings = await dbContext.Queryable<ShipRepairRolling>().Where(x => x.ShipId == saveShipRepairRollingRequestDto.ShipId).OrderByDescending(x=>x.CreateTime).ToListAsync();
            if (shipRepairRollings.Any())
            {
               var MaintenanceInformation =await GetResponsibleMaintenanceAsync(shipRepairRollings[0]);
                if (!MaintenanceInformation)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL);
                    return responseAjaxResult;
                }
            }
            if (saveShipRepairRollingRequestDto.ScheduledStartTime != null && saveShipRepairRollingRequestDto.ScheduledEndTime != null)
            {
                shipRepairRolling = new ShipRepairRolling()
                {
                    Id = GuidUtil.Next(),
                    ShipId = saveShipRepairRollingRequestDto.ShipId.Value,
                    RepairCategory = saveShipRepairRollingRequestDto.RepairCategory,
                    MainRepairContent = saveShipRepairRollingRequestDto.MainRepairContent,
                    RepairLocation = saveShipRepairRollingRequestDto.RepairLocation,
                    PlannedRepairPeriod = TimeHelper.GetTimeSpan(saveShipRepairRollingRequestDto.ScheduledStartTime.Value, saveShipRepairRollingRequestDto.ScheduledEndTime.Value).Days + 1,
                    ScheduledStartTime = saveShipRepairRollingRequestDto.ScheduledStartTime,
                    ScheduledEndTime = saveShipRepairRollingRequestDto.ScheduledEndTime,
                    ActualStartTime = saveShipRepairRollingRequestDto.ActualStartTime,
                    ExpectEndTime = saveShipRepairRollingRequestDto.ExpectEndTime,
                    MaintenanceName = saveShipRepairRollingRequestDto.MaintenanceName,
                    Remarks = saveShipRepairRollingRequestDto.Remarks
                };
            }
            else
            {
                shipRepairRolling = new ShipRepairRolling()
                {
                    Id = GuidUtil.Next(),
                    ShipId = saveShipRepairRollingRequestDto.ShipId.Value,
                    RepairCategory = saveShipRepairRollingRequestDto.RepairCategory,
                    MainRepairContent = saveShipRepairRollingRequestDto.MainRepairContent,
                    RepairLocation = saveShipRepairRollingRequestDto.RepairLocation,
                    //PlannedRepairPeriod = TimeHelper.GetTimeSpan(saveShipRepairRollingRequestDto.ScheduledStartTime.Value, saveShipRepairRollingRequestDto.ScheduledEndTime.Value).Days + 1,
                    ScheduledStartTime = saveShipRepairRollingRequestDto.ScheduledStartTime,
                    ScheduledEndTime = saveShipRepairRollingRequestDto.ScheduledEndTime,
                    ActualStartTime = saveShipRepairRollingRequestDto.ActualStartTime,
                    ExpectEndTime = saveShipRepairRollingRequestDto.ExpectEndTime,
                    MaintenanceName = saveShipRepairRollingRequestDto.MaintenanceName,
                    Remarks = saveShipRepairRollingRequestDto.Remarks
                };
            }
            var save = await dbContext.Insertable(shipRepairRolling).ExecuteCommandAsync();
            if (save > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }

        /// <summary>
        /// 获取负责机务
        /// </summary>
        /// <returns></returns>

        public async Task<bool> GetResponsibleMaintenanceAsync(ShipRepairRolling searchShipRepairRollingResponseDtos)
        {
            //疏浚公司Id
            string companyId = "3c5b138b-601a-442a-9519-2508ec1c1eb2";
            //陈翠
            Guid ChenCui = "08db4e0d-a531-4691-8dfc-24c2766074ce".ToGuid();
            //刘国银
            Guid liuGuoYin = "08db4e0d-a531-46bd-8059-c4e65518ea1c".ToGuid();
            //张伟
            Guid zhangWei = "08db4e0d-a531-4694-8119-b369828590a4".ToGuid();
            //邱旻炜
            Guid qiWenWei = "08db4e0d-a531-457b-8443-f657b1218d3d".ToGuid();
            var userName = await dbContext.Queryable<Domain.Models.User>().Where(x => x.Id == _currentUser.Id && x.CompanyId == companyId.ToGuid()).Select(x => x.Name).FirstAsync();
            var roleIdList = await dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1).ToListAsync();
            var roleIds = roleIdList.Where(x => x.InstitutionId == companyId.ToGuid()).Select(x => x.UserId).ToList();
            if (!roleIds.Contains(ChenCui))
            {
                roleIds.Add(ChenCui);
            }
            if (!roleIds.Contains(liuGuoYin))
            {
                roleIds.Add(liuGuoYin);
            }
            if (!roleIds.Contains(zhangWei))
            {
                roleIds.Add(zhangWei);
            }
            if (!roleIds.Contains(qiWenWei))
            {
                roleIds.Add(qiWenWei);
            }
            //通用权限
            //var userName = await dbContext.Queryable<Domain.Models.User>().Where(x => roleIds.Contains(x.Id)).ToListAsync()
            if (roleIds.Contains(_currentUser.Id))
            {
                return true;
            }
            //设置权限
            else if (searchShipRepairRollingResponseDtos.MaintenanceName == userName  && !roleIds.Contains(_currentUser.Id) && searchShipRepairRollingResponseDtos.MaintenanceName != null)
            {
                return true;
            }
            return false;
        }
        #region 船舶固定排序
        /// <summary>
        /// 船舶固定排序
        /// </summary>
        /// <param name="ownShipId"></param>
        /// <returns></returns>
        public int GetOwnShipSort(Guid? ownShipId)
        {
            if (ownShipId == "d38e3b12-f68a-40c3-9d3a-07f8daa4f923".ToGuid()) { return 1; }
            else if (ownShipId == "5500c50d-4bb6-444f-955b-a45d44055d45".ToGuid()) { return 2; }
            else if (ownShipId == "c534a50d-15ba-4a0f-9083-914f4e181711".ToGuid()) { return 3; }
            else if (ownShipId == "44799b7f-4d7d-45bb-8988-238f4e7337f5".ToGuid()) { return 4; }
            else if (ownShipId == "6432ef85-becb-487b-a405-33c40cf46d2c".ToGuid()) { return 5; }
            else if (ownShipId == "294e51b8-ba69-462a-ae52-590094a74192".ToGuid()) { return 6; }
            else if (ownShipId == "94622dea-7c94-47a9-8de4-890beb9e4ebe".ToGuid()) { return 7; }
            else if (ownShipId == "cf2ae936-fef9-4946-8f1a-72fda23e2080".ToGuid()) { return 8; }
            else if (ownShipId == "6a67a2b5-7753-4533-863e-29216692645f".ToGuid()) { return 9; }
            else if (ownShipId == "48ad8d07-7729-4855-9296-38756470a74d".ToGuid()) { return 10; }
            else if (ownShipId == "f96790f4-ca8f-46d8-b6f7-d454ac4a26c1".ToGuid()) { return 11; }
            else if (ownShipId == "537398a8-fced-4d1a-bb4f-e8f47f5e05e7".ToGuid()) { return 12; }
            else if (ownShipId == "eb273d8e-e630-404d-bf80-8b43d2a10e74".ToGuid()) { return 13; }
            else if (ownShipId == "63885c65-63ae-47b7-9c8e-16de2fda2e56".ToGuid()) { return 14; }
            else if (ownShipId == "fe4e954b-4663-495e-a5cc-92d6621b9563".ToGuid()) { return 15; }
            else if (ownShipId == "d5ba0594-e902-4faa-8a91-f62caff10f83".ToGuid()) { return 16; }
            else if (ownShipId == "10506132-b6c3-4da9-9169-dbefa3f77373".ToGuid()) { return 17; }
            else if (ownShipId == "2c11be77-628a-4cf8-99d7-06026fb4ddf0".ToGuid()) { return 18; }
            else if (ownShipId == "98b22eff-ef0e-4754-9af0-4360a56d47a3".ToGuid()) { return 19; }
            else if (ownShipId == "de83b2fb-ae2b-4509-bf46-1eed92e2db5e".ToGuid()) { return 20; }
            else if (ownShipId == "e9cba5b9-f3dd-464b-9485-116a4343b6b5".ToGuid()) { return 21; }
            else return 0;
        }
        #endregion
    }
}
