using AutoMapper;
using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Org.BouncyCastle.Utilities.Date;
using SkiaSharp;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilsSharp;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements.EnterShipsResponseDto;

namespace GHMonitoringCenterApi.Application.Service.Projects
{
    /// <summary>
    /// 项目-船舶动向
    /// </summary>
    public class ProjectShipMovementsService : IProjectShipMovementsService
    {

        /// <summary>
        /// 项目
        /// </summary>
        private readonly IBaseRepository<Project> _dbProject;
        private readonly IBaseRepository<ShipMovementRecord> baseRepositoryShipMovementRecord;

        /// <summary>
        /// 船舶动向
        /// </summary>
        private readonly IBaseRepository<ShipMovement> _dbShipMovement;

        /// <summary>
        /// 分包船舶
        /// </summary>
        private readonly IBaseRepository<SubShip> _dbSubShip;

        /// <summary>
        /// 自有船舶
        /// </summary>
        private readonly IBaseRepository<OwnerShip> _dbOwnerShip;

        /// <summary>
        /// 用户表
        /// </summary>
        private readonly IBaseRepository<Domain.Models.User> _dbUser;

        /// <summary>
        /// 船舶类型
        /// </summary>
        private readonly IBaseRepository<ShipPingType> _dbShipPingType;

        /// <summary>
        /// 船舶日报
        /// </summary>
        private readonly IBaseRepository<ShipDayReport> _dbShipDayReport;
        /// <summary>
        /// 机构
        /// </summary>
        private readonly IBaseRepository<Institution> _dbInstitution;
        private readonly IBaseRepository<Domain.Models.Role> _dbRole;

        private readonly IBaseService _baseService;

        /// <summary>
        /// 匹配
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        /// <summary>
        /// 项目日报构造
        /// </summary>
        public ProjectShipMovementsService(IBaseRepository<Project> dbProject
            , IBaseRepository<ShipMovement> dbShipMovement
            , IBaseRepository<SubShip> dbSubShip
            , IBaseRepository<OwnerShip> dbOwnerShip
            , IBaseRepository<Domain.Models.User> dbUser
            , IBaseRepository<ShipPingType> dbShipPingType
            , IBaseRepository<ShipDayReport> dbShipDayReport
            , IBaseRepository<Institution> dbInstitution
            , IBaseRepository<Domain.Models.Role> dbRole
            , IBaseService baseService
            , IMapper mapper
            , GlobalObject globalObject
            , IBaseRepository<ShipMovementRecord> baseRepositoryShipMovementRecord
            )
        {

            _dbProject = dbProject;
            _dbShipMovement = dbShipMovement;
            _dbSubShip = dbSubShip;
            _dbOwnerShip = dbOwnerShip;
            _dbUser = dbUser;
            _dbShipPingType = dbShipPingType;
            _dbShipDayReport = dbShipDayReport;
            _mapper = mapper;
            _globalObject = globalObject;
            _dbInstitution = dbInstitution;
            _baseService = baseService;
            _dbRole = dbRole;
            this.baseRepositoryShipMovementRecord = baseRepositoryShipMovementRecord;
        }

        /// <summary>
        /// 新增船舶进出场
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddShipMovementAsync(AddShipMovementRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            if (await ExistsShipMovementAsync(project.Id, model.ShipType, model.ShipId))
            {
                return result.FailResult(HttpStatusCode.InsertFail, $"当前船舶已经在{project.Name}项目上");
            }
            var enterShipMovement = await GetShipMovementAsync(model.ShipId, model.ShipType, ShipMovementStatus.Enter, model.ProjectId);
            if (enterShipMovement != null)
            {
                var hasOtherEnterProject = await GetProjectPartAsync(enterShipMovement.ProjectId);
                if (hasOtherEnterProject != null)
                {
                    await JjtMessageReminder(model.ShipId, model.ShipType, hasOtherEnterProject.Name, project.Name);
                    //todo 交建通发送一条消息至陈翠  消息内容 “（船舶名称）目前在（已在场项目名称）进行进场，（触发提示用户）需要进场至（后面项目名称）中，请协调处理！”
                    return result.FailResult(HttpStatusCode.InsertFail, $"当前船舶已经在{hasOtherEnterProject.Name}项目上进场");
                }
            }
            var shipMovement = new ShipMovement();
            shipMovement = _mapper.Map(model, shipMovement);
            shipMovement.CreateId = _currentUser.Id;
            shipMovement.Id = GuidUtil.Next();
            shipMovement.Status = ShipMovementStatus.None;
            #region 日志信息
            LogInfo logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
                DataId = model.ProjectId,
                BusinessModule = "/装备管理/船舶进出场/新增船舶",
                BusinessRemark = "/装备管理/船舶进出场/新增船舶"
            };
            #endregion
            #region 记录船舶进退场
            ShipMovementRecord shipMovementRecord = new ShipMovementRecord()
            {
                ShipMovementId = shipMovement.Id,
                ShipId = model.ShipId,
                ProjectId = model.ProjectId,
                Status = 0,
            };
            await  baseRepositoryShipMovementRecord.InsertAsync(shipMovementRecord);
            #endregion
            await _dbShipMovement.AsInsertable(shipMovement).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            return result.SuccessResult(true, ResponseMessage.OPERATION_INSERT_SUCCESS);
        }


        #region 旧更改船舶进出场状态

        /// <summary>
        ///  更改船舶进出场状态
        /// </summary>
        /// <returns></returns>
        //public async Task<ResponseAjaxResult<bool>> ChangeShipMovementStatusAsync(ChangeShipMovementStatusRequestDto model)
        //{
        //    var result = new ResponseAjaxResult<bool>();
        //    var shipMovement = await GetShipMovementAsync(model.ShipMovementId);
        //    if (shipMovement == null)
        //    {
        //        return result.FailResult(HttpStatusCode.DataNotEXIST, "船舶进出场对象不存在");
        //    }
        //    shipMovement.UpdateId = _currentUser.Id;
        //    shipMovement.Status = model.Status;
        //    shipMovement.Remarks = model.Remarks;
        //    if (shipMovement.Status == ShipMovementStatus.Enter)
        //    {
        //        var enterShipMovement = await GetShipMovementAsync(shipMovement.ShipId, shipMovement.ShipType, ShipMovementStatus.Enter, shipMovement.ProjectId);
        //        if (enterShipMovement != null)
        //        {
        //            var hasOtherEnterProject = await GetProjectPartAsync(enterShipMovement.ProjectId);
        //            if (hasOtherEnterProject != null)
        //            {
        //                var project = await GetProjectPartAsync(shipMovement.ProjectId);
        //                await JjtMessageReminder(shipMovement.ShipId, shipMovement.ShipType, hasOtherEnterProject.Name, project.Name);
        //                //todo 交建通发送一条消息至陈翠  消息内容 “（船舶名称）目前在（已在场项目名称）进行进场，（触发提示用户）需要进场至（后面项目名称）中，请协调处理！”
        //                return result.FailResult(HttpStatusCode.InsertFail, $"当前船舶已经在{hasOtherEnterProject.Name}项目上进场");
        //            }
        //        }
        //        //这次进场之后，有进场时间，并没有出场，出场时间置空
        //        shipMovement.EnterTime = model.EnterOrQuitTime;
        //        shipMovement.QuitTime = null;
        //    }
        //    else if (shipMovement.Status == ShipMovementStatus.Quit)
        //    {
        //        if (shipMovement.EnterTime > model.EnterOrQuitTime)
        //        {
        //            return result.FailResult(HttpStatusCode.SaveFail, "退场时间不能小于进场时间");
        //        }
        //        shipMovement.QuitTime = model.EnterOrQuitTime;
        //    }
        //    #region 日志信息
        //    LogInfo logDto = new LogInfo()
        //    {
        //        Id = GuidUtil.Increment(),
        //        OperationId = _currentUser.Id,
        //        OperationName = _currentUser.Name,
        //        DataId = shipMovement.ProjectId,
        //        BusinessModule = "/装备管理/船舶进出场/退场",
        //        BusinessRemark = "/装备管理/船舶进出场/退场"
        //    };
        //    #endregion
        //    await _dbShipMovement.AsUpdateable(shipMovement).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
        //    return result.SuccessResult(true, EnumExtension.GetEnumDescription(model.Status) + "成功");
        //}

        #endregion

        #region 新 更改船舶进出场状态

        /// <summary>
        ///  更改船舶进出场状态
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ChangeShipMovementStatusAsync(ChangeShipMovementStatusRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var shipMovement = await GetShipMovementAsync(model.ShipMovementId);
            if (shipMovement == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, "船舶进出场对象不存在");
            }
            shipMovement.UpdateId = _currentUser.Id;
            shipMovement.Status = model.Status;
            shipMovement.Remarks = model.Remarks;
            shipMovement.EnterTime = model.EnterOrQuitTime;
            if (shipMovement.Status == ShipMovementStatus.Enter)
            {
                var enterShipMovement = await GetShipMovementAsync(shipMovement.ShipId, shipMovement.ShipType, ShipMovementStatus.Enter, shipMovement.ProjectId);
                if (enterShipMovement != null)
                {
                    var hasOtherEnterProject = await GetProjectPartAsync(enterShipMovement.ProjectId);
                    if (hasOtherEnterProject != null)
                    {
                        var project = await GetProjectPartAsync(shipMovement.ProjectId);
                        await JjtMessageReminder(shipMovement.ShipId, shipMovement.ShipType, hasOtherEnterProject.Name, project.Name);
                        //todo 交建通发送一条消息至陈翠  消息内容 “（船舶名称）目前在（已在场项目名称）进行进场，（触发提示用户）需要进场至（后面项目名称）中，请协调处理！”
                        return result.FailResult(HttpStatusCode.InsertFail, $"当前船舶已经在{hasOtherEnterProject.Name}项目上进场");
                    }
                }
                //// 原来数据删除状态更改为2 
                //shipMovement.IsDelete = 2;
                //await _dbShipMovement.AsUpdateable(shipMovement).WhereColumns(x => x.Id).UpdateColumns(x => x.IsDelete).ExecuteCommandAsync();
                //新增最新进场数据
                //var addShipMovement = new ShipMovement()
                //{
                //    Id = GuidUtil.Next(),
                //    CreateId = shipMovement.CreateId,
                //    CreateTime = shipMovement.CreateTime,
                //    EnterTime = model.EnterOrQuitTime,
                //    QuitTime = null,
                //    UpdateId = _currentUser.Id,
                //    Status = model.Status,
                //    Remarks = model.Remarks,
                //    ProjectId = shipMovement.ProjectId,
                //    ShipId = shipMovement.ShipId,
                //    ShipType = shipMovement.ShipType,
                //    UpdateTime = DateTime.Now
                //};
                LogInfo addLogDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                    DataId = shipMovement.ProjectId,
                    BusinessModule = "/装备管理/船舶进出场/进场",
                    BusinessRemark = "/装备管理/船舶进出场/进场"
                };

                #region 船舶进退场
                //获取船舶进退场记录表
                var shiMovementRecord = await baseRepositoryShipMovementRecord.GetFirstAsync(x => x.ShipMovementId == model.ShipMovementId);
                if (shiMovementRecord != null)
                {
                    if (model.Status == ShipMovementStatus.Enter)
                    {
                        shiMovementRecord.EnterTime = model.EnterOrQuitTime;
                        shiMovementRecord.Status = (int)ShipMovementStatus.Enter;
                        shiMovementRecord.ShipId = shipMovement.ShipId;
                        shiMovementRecord.ShipType = (int)shipMovement.ShipType;
                        await baseRepositoryShipMovementRecord.UpdateAsync(shiMovementRecord);
                    }
                   
                }
                #endregion

                await _dbShipMovement.AsUpdateable(shipMovement).EnableDiffLogEvent(addLogDto).ExecuteCommandAsync();
            }
            else if (shipMovement.Status == ShipMovementStatus.Quit)
            {
                #region 船舶进退场
                //获取船舶进退场记录表
                var shiMovementRecord = await baseRepositoryShipMovementRecord.GetFirstAsync(x => x.ShipMovementId == model.ShipMovementId);
                if (shiMovementRecord != null)
                {
                    if (model.Status == ShipMovementStatus.Quit)
                    {
                     
                        shiMovementRecord.QuitTime = model.EnterOrQuitTime;
                        shiMovementRecord.Status = (int)ShipMovementStatus.Quit;
                        await baseRepositoryShipMovementRecord.UpdateAsync(shiMovementRecord);
                    }
                }
                #endregion

                if (shipMovement.EnterTime > model.EnterOrQuitTime)
                {
                    return result.FailResult(HttpStatusCode.SaveFail, "退场时间不能小于进场时间");
                }
                shipMovement.QuitTime = model.EnterOrQuitTime;
                #region 日志信息
                LogInfo logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                    DataId = shipMovement.ProjectId,
                    BusinessModule = "/装备管理/船舶进出场/退场",
                    BusinessRemark = "/装备管理/船舶进出场/退场"
                };
                #endregion
                await _dbShipMovement.AsUpdateable(shipMovement).EnableDiffLogEvent(logDto).ExecuteCommandAsync();



            }
            else if (shipMovement.Status == ShipMovementStatus.None)
            {
                shipMovement.EnterTime = model.EnterOrQuitTime;
                shipMovement.UpdateId = _currentUser.Id;
                shipMovement.Status = model.Status;
                shipMovement.Remarks = model.Remarks;
                LogInfo addLogDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                    DataId = shipMovement.ProjectId,
                    BusinessModule = "/装备管理/船舶进出场/进场",
                    BusinessRemark = "/装备管理/船舶进出场/进场"
                };
                #region 船舶进退场
                //获取船舶进退场记录表
                var shiMovementRecord = await baseRepositoryShipMovementRecord.GetFirstAsync(x => x.ShipMovementId == model.ShipMovementId);
                if (shiMovementRecord != null)
                {
                  
                shiMovementRecord.EnterTime = model.EnterOrQuitTime;
                shiMovementRecord.Status = (int)ShipMovementStatus.Enter;
                await baseRepositoryShipMovementRecord.InsertAsync(shiMovementRecord);
                }
                #endregion
                await _dbShipMovement.AsUpdateable(shipMovement).WhereColumns(x => x.Id).UpdateColumns(x => new { x.UpdateId, x.UpdateTime, x.Status, x.Remarks, x.EnterTime }).EnableDiffLogEvent(addLogDto).ExecuteCommandAsync();
            }

          
            return result.SuccessResult(true, EnumExtension.GetEnumDescription(model.Status) + "成功");
        }
        #endregion

        /// <summary>
        ///  移除船舶进出场
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RemoveShipMovementAsync(RemoveShipMovementRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var shipMovement = await GetShipMovementAsync(model.ShipMovementId);
            if (shipMovement == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, $"船舶进出场对象不存在");
            }
            if (shipMovement.Status == ShipMovementStatus.Enter)
            {
                return result.FailResult(HttpStatusCode.DeleteFail, $"船舶已进场,需要先退场才能删除");
            }
            shipMovement.IsDelete = 0;
            shipMovement.DeleteId = _currentUser.Id;
            shipMovement.DeleteTime = DateTime.Now;
            #region 日志信息
            LogInfo logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
                DataId = shipMovement.ProjectId,
                BusinessModule = "/装备管理/船舶进出场/删除",
                BusinessRemark = "/装备管理/船舶进出场/删除"
            };
            #endregion
            await _dbShipMovement.AsUpdateable(shipMovement).UpdateColumns(t => new { t.DeleteId, t.DeleteTime, t.IsDelete }).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            return result.SuccessResult(true, ResponseMessage.OPERATION_DELETE_SUCCESS);
        }

        /// <summary>
        ///项目-搜索船舶动向列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> SearchShipMovementsAsync(ShipMovementsRequestDto model)
        {
            var result = new ResponseAjaxResult<List<ShipMovementResponseDto>>();
            var shipMovements = _dbShipMovement.AsQueryable().Where(t => t.IsDelete == 1 && t.ProjectId == model.ProjectId).ToList();
            var ownerShipIds = shipMovements.Where(t => t.ShipType == ShipType.OwnerShip).Select(t => t.ShipId).ToArray();
            var subShipIds = shipMovements.Where(t => t.ShipType == ShipType.SubShip).Select(t => t.ShipId).ToArray();
            var ships = await GetShipPartsAsync(ownerShipIds, ShipType.OwnerShip);
            ships.AddRange(await GetShipPartsAsync(subShipIds, ShipType.SubShip));
            var shipKindTypeIds = ships.Select(t => t.ShipKindTypeId).ToArray();
            var shipKindTypes = await GetShipPingTypePartsAsync(shipKindTypeIds);
            var resShipMovements = _mapper.Map<List<ShipMovement>, List<ShipMovementResponseDto>>(shipMovements);
            resShipMovements.ForEach(resShipMovement =>
            {
                var ship = ships.FirstOrDefault(t => t.ShipType == resShipMovement.ShipType && t.PomId == resShipMovement.ShipId);
                if (ship != null)
                {
                    resShipMovement.ShipName = ship.Name;
                    resShipMovement.ShipKindTypeName = shipKindTypes.FirstOrDefault(t => t.PomId == ship.ShipKindTypeId)?.Name;
                }
            });
            return result.SuccessResult(resShipMovements);
        }

        #region 项目-搜索进场船舶（自有船舶   旧代码 
        /// <summary>
        /// 项目-搜索进场船舶（自有船舶）
        /// </summary>
        /// <returns></returns>
        //public async Task<ResponseAjaxResult<EnterShipsResponseDto>> SearchEnterShipsAsync(EnterShipsRequestDto model)
        //{
        //    //耙吸船、绞吸船 和 抓斗船 非关联项目过滤条件
        //    var filtShipType = CommonData.ShipType.Split(',').ToList();
        //    var result = new ResponseAjaxResult<EnterShipsResponseDto>();
        //    var dateDayTime = model.DateDayTime ?? DateTime.Now.AddDays(-1);
        //    var dateDay = dateDayTime.ToDateDay();
        //    //获取所有进场船舶
        //    var shipMovements = await _dbShipMovement.AsQueryable().Where(t => t.IsDelete == 1 && t.ShipType == ShipType.OwnerShip && t.Status == ShipMovementStatus.Enter || (t.Status == ShipMovementStatus.Quit && t.EnterTime <= model.DateDayTime)).ToListAsync();
        //    // 追加已退场的船舶 传参日期<=退场日期
        //    var quitShips = await _dbShipMovement.AsQueryable().Where(t => t.IsDelete == 1 && t.ShipType == ShipType.OwnerShip && t.Status == ShipMovementStatus.Quit && t.QuitTime <= model.DateDayTime).ToListAsync();
        //    shipMovements.AddRange(quitShips);
        //    shipMovements = shipMovements.GroupBy(x => new { x.ShipId, x.Status, x.ProjectId }).Select(x => x.First()).ToList();
        //    //获取所有自有船舶
        //    var ownShips = await GetShipPartsAsync(ShipType.OwnerShip);
        //    var shipIds = ownShips.Select(x => x.PomId.Value).ToArray();
        //    var shipKindTypeIds = ownShips.Select(t => t.ShipKindTypeId).ToArray();
        //    //获取所有船舶类型
        //    var shipKindTypes = await GetShipPingTypePartsAsync(shipKindTypeIds);
        //    //查询所有自有船舶是否填写了日报
        //    var fillReportShips = await GetFillReportShipsAsync(shipIds, dateDay);
        //    var resShips = new List<EnterShipsResponseDto.ResEnterShipDto>();
        //    //获取所有项目信息
        //    var projects = await GetProjectInfoAsync();
        //    ownShips.ForEach(ship =>
        //    {
        //        //if (ship.PomId != "d38e3b12-f68a-40c3-9d3a-07f8daa4f923".ToGuid())
        //        //{
        //        //    return;
        //        //}
        //        //获取船舶进退场时间
        //        var shipMovement = shipMovements.Where(t => t.ShipId == ship.PomId).ToList();// SingleOrDefault(t => t.ShipId == ship.PomId);
        //        //获取船舶填报时间
        //        var fillReportShip = fillReportShips.FirstOrDefault(t => t.PomId == ship.PomId);
        //        DateTime? fillReportTime = null;
        //        if (fillReportShip != null)
        //        {
        //            if (ConvertHelper.TryConvertDateTimeFromDateDay(fillReportShip.FillReportDateDay, out DateTime dayTime))
        //            {
        //                fillReportTime = dayTime;
        //            }
        //        }
        //        if (shipMovement.Count() == 0)
        //        {
        //            var resShipMovement = new EnterShipsResponseDto.ResEnterShipDto()
        //            {
        //                ProjectId = Guid.Empty,
        //                ProjectName = null,
        //                ShipId = ship.PomId.Value,
        //                DateDayTime = Convert.ToDateTime(fillReportTime),
        //                ShipName = ship.Name,
        //                EnterTime = null,
        //                ShipCompanyId = ship.CompanyId,
        //                ShipKindTypeName = shipKindTypes.FirstOrDefault(t => t.PomId == ship.ShipKindTypeId)?.Name,
        //                FillReportTime = fillReportTime,
        //                AssociationProject = 2,
        //                FillReportStatus = GetFillState(model.DateDayTime, null, fillReportTime)
        //            };
        //            //判断当前船舶类型是否在耙吸船、绞吸船 和 抓斗船范围内
        //            if (filtShipType.Contains(ship.ShipKindTypeId.Value.ToString()))
        //            {
        //                resShips.Add(resShipMovement);
        //            }
        //        }
        //        if (shipMovement.Count() > 1 && model.ProjectId != null && model.ProjectId != Guid.Empty)
        //        {
        //            shipMovement = shipMovement.Where(x => x.ProjectId == model.ProjectId).ToList();


        //            ////查询船舶日报表中是否关联项目
        //            //foreach (var item in shipMovement)
        //            //{
        //            //    var isExist = fillReportShips.FirstOrDefault(x => x.ProjectId == item.ProjectId && x.PomId == item.ShipId);
        //            //    if (isExist == null)
        //            //    {
        //            //        item.ProjectId = Guid.Empty;
        //            //    }
        //            //}
        //            #region 添加的逻辑
        //            if (!shipMovement.Any())
        //            {
        //                var resShipMovement = new EnterShipsResponseDto.ResEnterShipDto()
        //                {

        //                    ShipId = ship.PomId.Value,
        //                    DateDayTime = Convert.ToDateTime(fillReportTime),
        //                    ShipName = ship.Name,

        //                    ShipCompanyId = ship.CompanyId,
        //                    ShipKindTypeName = shipKindTypes.FirstOrDefault(t => t.PomId == ship.ShipKindTypeId)?.Name,
        //                    FillReportTime = fillReportTime,
        //                    AssociationProject = 2,
        //                    FillReportStatus = GetFillState(model.DateDayTime, null, fillReportTime)
        //                };
        //                resShips.Add(resShipMovement);
        //            }
        //            #endregion

        //        }
        //        shipMovement.ForEach(item =>
        //        {



        //            var resShipMovement = new EnterShipsResponseDto.ResEnterShipDto()
        //            {
        //                ProjectId = item.Status == ShipMovementStatus.Quit && !(item.EnterTime <= model.DateDayTime && item.QuitTime >= model.DateDayTime) ? Guid.Empty : item.ProjectId,
        //                ProjectName = item.Status == ShipMovementStatus.Quit && !(item.EnterTime <= model.DateDayTime && item.QuitTime >= model.DateDayTime) ? null : projects.Where(p => p.ProjectId == item.ProjectId).SingleOrDefault()?.ProjectName,
        //                ShipId = ship.PomId.Value,
        //                DateDayTime = Convert.ToDateTime(fillReportTime),
        //                ShipName = ship.Name,
        //                EnterTime = item.EnterTime,
        //                ShipCompanyId = ship.CompanyId,
        //                ShipKindTypeName = shipKindTypes.FirstOrDefault(t => t.PomId == ship.ShipKindTypeId)?.Name,
        //                FillReportTime = fillReportTime,
        //                AssociationProject = item.Status == ShipMovementStatus.Quit && !(item.EnterTime <= model.DateDayTime && item.QuitTime >= model.DateDayTime) ? 2 : item.ProjectId == Guid.Empty ? 2 : 1,
        //                FillReportStatus = GetFillState(model.DateDayTime, item.EnterTime, fillReportTime)
        //            };
        //            #region 添加的逻辑
        //            if (item.Status == ShipMovementStatus.Enter && model.DateDayTime < item.EnterTime && item.QuitTime == null)
        //            {
        //                resShipMovement.ProjectId = Guid.Empty;
        //                resShipMovement.AssociationProject = 2;
        //                resShipMovement.ProjectName = string.Empty;
        //            }
        //            if (item.Status == ShipMovementStatus.Quit && item.QuitTime != null && model.DateDayTime > item.QuitTime)
        //            {
        //                resShipMovement.ProjectId = Guid.Empty;
        //                resShipMovement.AssociationProject = 2;
        //                resShipMovement.ProjectName = string.Empty;
        //            }
        //            #endregion
        //            //判断当前船舶是否未非关联项目  并且类型是否在耙吸船、绞吸船 和 抓斗船范围内
        //            if (resShipMovement.AssociationProject == 2 && filtShipType.Contains(ship.ShipKindTypeId.Value.ToString()))
        //            {
        //                resShips.Add(resShipMovement);
        //            }
        //            if (resShipMovement.AssociationProject == 1)
        //            {
        //                resShips.Add(resShipMovement);
        //            }
        //        });

        //    });
        //    //获取当前角色信息
        //    var curRoleInfo = _currentUser.RoleInfos.Where(role => role.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
        //    //获取角色id
        //    var role = await _dbRole.AsQueryable().Where(x => x.Id == _currentUser.CurrentLoginRoleId).FirstAsync();
        //    if (curRoleInfo.IsAdmin || (role != null && role.Type == 2))//超级管理员  或者  管理员
        //    {
        //        if (!curRoleInfo.IsAdmin && curRoleInfo.Oid != "101162350")//公司管理员
        //        {
        //            //获取当前机构下所有自有船舶
        //            var institutionFirst = await _dbInstitution.AsQueryable().Where(x => x.Oid == curRoleInfo.Oid).FirstAsync();
        //            var institutionIdResponse = await _baseService.SearchCompanySubPullDownAsync(institutionFirst.PomId.Value);
        //            var institutionIds = institutionIdResponse.Data.Select(x => x.Id.Value).ToList();
        //            //追加当前角色公司
        //            institutionIds.Add(institutionFirst.PomId.Value);
        //            //除了可以看公司的以外还要可看当前项目绑定的船舶
        //            if (model.ProjectId != Guid.Empty && !string.IsNullOrWhiteSpace(model.ProjectId.ToString()))
        //            {
        //                var bships = resShips.Where(x => x.ProjectId == model.ProjectId).ToList();
        //                resShips = resShips.Where(x => institutionIds.Contains(x.ShipCompanyId.Value)).ToList();
        //                resShips.AddRange(bships);
        //            }
        //            else
        //            {
        //                resShips = resShips.Where(x => institutionIds.Contains(x.ShipCompanyId.Value)).ToList();
        //            }
        //        }
        //        if (model.ProjectId != Guid.Empty && !string.IsNullOrWhiteSpace(model.ProjectId.ToString()))
        //        {
        //            resShips = resShips.Where(x => (x.ProjectId == model.ProjectId && x.AssociationProject == 1) || (x.AssociationProject == 2)).ToList();
        //        }
        //        resShips = resShips.GroupBy(x => x).Select(x => x.First()).ToList();
        //    }
        //    else
        //    {
        //        //项目部人员登陆 只需要看关联项目的船舶
        //        if (model.ProjectId != Guid.Empty && !string.IsNullOrWhiteSpace(model.ProjectId.ToString()))
        //        {
        //            resShips = resShips.Where(x => x.ProjectId == model.ProjectId && x.AssociationProject == 1).ToList();
        //        }
        //    }
        //    resShips = resShips.OrderBy(x => x.AssociationProject).OrderByDescending(x => x.EnterTime).ToList();
        //    if (model.AssociationProject == 1) resShips = resShips.Where(x => x.AssociationProject == 1).ToList();
        //    if (model.AssociationProject == 2) resShips = resShips.Where(x => x.AssociationProject == 2).ToList();
        //    int skipCount = (model.PageIndex - 1) * model.PageSize;
        //    var pageData = resShips.Skip(skipCount).Take(model.PageSize);
        //    result.Count = resShips.Count();
        //    var resEnterShips = new EnterShipsResponseDto() { Ships = pageData.ToArray(), DateDayTime = dateDayTime.Date };
        //    return result.SuccessResult(resEnterShips);
        //}
        #endregion


        #region 项目-搜索进场船舶（自有船舶   旧代码
        /// <summary>
        /// 项目-搜索进场船舶（自有船舶）
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<EnterShipsResponseDto>> SearchEnterShipsAsync(EnterShipsRequestDto model)
        {
            var result = new ResponseAjaxResult<EnterShipsResponseDto>();
            var userInfo = _currentUser;
            #region 权限控制
            var oids = await _dbInstitution.AsQueryable().Where(x => x.IsDelete == 1 && x.Oid == userInfo.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await _baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            var departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            #endregion

            //项目信息
            var projectList = await _dbProject.AsQueryable().Where(x => x.IsDelete == 1&& departmentIds.Contains(x.ProjectDept.Value)).ToListAsync();
            //2019013759   2020012489   2016146340
            //所有自有船舶数据
            var allShipList = await _dbOwnerShip.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            //船舶进退场
            var projectIds = projectList.Select(x => x.Id).ToList();
           var shipMovementList = await _dbShipMovement.AsQueryable().Where(x => x.IsDelete == 1&&x.Status==ShipMovementStatus.Enter).ToListAsync();
            //船舶日报
            if (!model.DateDayTime.HasValue)
            {
                model.DateDayTime = DateTime.Now.AddDays(-1);
            }
            var currentDayTime = ConvertHelper.ToDateDay(model.DateDayTime.Value);
            var shipDayReportList = await _dbShipDayReport.AsQueryable().Where(x => x.IsDelete == 1&&x.DateDay== currentDayTime).ToListAsync();
            //船舶类型
            var typeIds= CommonData.ShipTypes.Select(x=>x.Value).ToList();
             var shipTypeList=await _dbShipPingType.AsQueryable().Where(x => x.IsDelete == 1&&typeIds.Contains(x.PomId.Value)).ToListAsync();
            if (shipMovementList.Any())
            {
                List<ResEnterShipDto> resEnterShipDtos = new List<ResEnterShipDto>();
                    foreach (var item in allShipList)
                {
                    ResEnterShipDto res = new ResEnterShipDto()
                    {
                        DateDayTime = model.DateDayTime.Value,
                        FillReportTime = model.DateDayTime.Value,
                        ShipId = item.PomId,
                    };

                    //进场时间
                    var currentEnterShipInfo = shipMovementList.Where(x => x.ShipId == item.PomId && x.Status == ShipMovementStatus.Enter).FirstOrDefault();
                    if (currentEnterShipInfo != null)
                    {
                        res.EnterTime = currentEnterShipInfo.EnterTime;
                        res.ProjectId = currentEnterShipInfo.ProjectId;
                        res.ProjectName= projectList.Where(x=>x.Id== currentEnterShipInfo.ProjectId).Select(x=>x.Name).FirstOrDefault();
                        res.AssociationProject = 1;
                    }
                    else {
                        res.AssociationProject = 2;
                    }

                    //船舶类型
                    var shipInfo = allShipList.Where(x => x.PomId == item.PomId).FirstOrDefault();
                    if (shipInfo != null)
                    {
                        res.ShipName = shipInfo.Name;
                        var shipType = shipTypeList.Where(x => x.PomId.Value == shipInfo.TypeId).FirstOrDefault();
                        if (shipType != null)
                        {
                            res.ShipKindTypeName = shipType.Name;
                        }
                        else {
                            continue;
                        }
                    }
                    else {
                        continue;
                    }

                    //船舶日报
                   var currentShipDay= shipDayReportList.Where(x => x.ShipId == item.PomId).FirstOrDefault();
                    if (currentShipDay != null)
                    {
                        res.FillReportStatus = 2;
                    }
                    else {
                        res.FillReportStatus = 1;
                    }
                    resEnterShipDtos.Add(res);

                }
                var pids= new List<Guid>();
                if (userInfo.Account != "2019013759" &&
              userInfo.Account != "2020012489" &&
              userInfo.Account != "2016146340" &&
              !userInfo.CurrentLoginIsAdmin)
                {
                    pids = projectList.Select(x => x.Id).ToList();
                }
                
                result.Count = resEnterShipDtos
                    .WhereIF(model.AssociationProject!=0,x=>x.AssociationProject== model.AssociationProject)
                    .WhereIF(pids.Any(), x=> pids.Contains(x.ProjectId))
                    .Count();
                EnterShipsResponseDto enterShipsResponseDto = new EnterShipsResponseDto()
                {
                    Ships = resEnterShipDtos
                    .WhereIF(model.AssociationProject != 0, x => x.AssociationProject == model.AssociationProject)
                      .WhereIF(pids.Any(), x => pids.Contains(x.ProjectId))
                      .OrderByDescending(x => x.ProjectId == Guid.Empty).ThenByDescending(x => x.ProjectId)
                    .Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToArray(),
                    DateDayTime = model.DateDayTime.Value,
                };
                
                result.Data = enterShipsResponseDto;
                
            }
            result.Success();
           
          return result;
        }
        #endregion
        /// <summary>
        /// 获取填报状态结果
        /// </summary>
        /// <param name="paramFillDate">入参填报日期</param>
        /// <param name="enterTime">进场时间</param>
        /// <param name="fillTime">已填报日期</param>
        /// <returns></returns>
        public int GetFillState(DateTime? paramFillDate, DateTime? enterTime, DateTime? fillTime)
        {
            //进场时间不为空，代表关联项目
            if (enterTime != null)
            {
                //如果实际填报日期不为空 
                if (fillTime != null)
                {
                    if (Convert.ToDateTime(paramFillDate).Date > Convert.ToDateTime(fillTime).Date)
                    {
                        return 0;//无需填报
                    }
                    else if (Convert.ToDateTime(fillTime).Date == DateTime.Now.Date)
                    {
                        return 2;//已填报
                    }
                    else if (Convert.ToDateTime(fillTime).Date == Convert.ToDateTime(paramFillDate).Date)
                    {
                        return 2;//已填报
                    }
                    else
                    {
                        return 1;//未填报
                    }
                }
                else
                {
                    if (Convert.ToDateTime(paramFillDate).Date > DateTime.Now.Date)
                    {
                        return 0;//无需填报
                    }
                    else
                    {
                        return 1;//未填报
                    }
                }
            }
            //进场时间为空，代表不关联项目
            else if (enterTime == null)
            {
                //如果已填报日期不是空
                if (fillTime != null)
                {
                    //如果填报日期<入参填报日期
                    if (Convert.ToDateTime(fillTime).Date < Convert.ToDateTime(paramFillDate).Date)
                    {
                        return 0;//无需填报
                    }
                    else if (Convert.ToDateTime(fillTime).Date == Convert.ToDateTime(paramFillDate).Date)
                    {
                        return 2;//已填报
                    }
                }
                else
                {
                    return 1;//未填报
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取船舶今日填写的船舶日报
        /// </summary>
        /// <returns></returns>
        private async Task<List<ShipPartDto>> GetFillReportShipsAsync(Guid projectId, Guid[] shipIds, int dateDay)
        {
            return await _dbShipDayReport.AsQueryable().Where(t => t.IsDelete == 1 && t.ProjectId == projectId
            && shipIds.Contains(t.ShipId) && t.DateDay == dateDay)
                .Select(t => new ShipPartDto() { PomId = t.ShipId, FillReportDateDay = t.DateDay }).ToListAsync();
        }
        /// <summary>
        /// 今日是否填写船舶日报
        /// </summary>
        /// <param name="shipIds"></param>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        private async Task<List<ShipPartDto>> GetFillReportShipsAsync(Guid[] shipIds, int dateDay)
        {
            return await _dbShipDayReport.AsQueryable().Where(t => t.IsDelete == 1 && shipIds.Contains(t.ShipId) && t.DateDay == dateDay).Select(t => new ShipPartDto() { PomId = t.ShipId, FillReportDateDay = t.DateDay, ProjectId = t.ProjectId }).ToListAsync();
        }

        /// <summary>
        /// 是否已存在船舶进出场
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="shipType">船舶类型</param>
        /// <param name="shipId">船舶Id</param>
        /// <returns></returns>
        public async Task<bool> ExistsShipMovementAsync(Guid projectId, ShipType shipType, Guid shipId)
        {
            return await _dbShipMovement.CountAsync(t => t.IsDelete == 1 && t.ProjectId == projectId && t.ShipType == shipType && t.ShipId == shipId) > 0;
        }

        /// <summary>
        /// 获取一个船舶进出场
        /// </summary>
        /// <param name="shipMovementId">船舶进出场Id</param>
        /// <returns></returns>
        public async Task<ShipMovement> GetShipMovementAsync(Guid shipMovementId)
        {
            return await _dbShipMovement.GetFirstAsync(t => t.IsDelete == 1 && t.Id == shipMovementId);
        }

        /// <summary>
        /// 获取一个船舶进出场
        /// </summary>
        /// <param name="shipId">船舶Id</param>
        /// <param name="shipType">船舶类型</param>
        /// <param name="shipMovementStatus">船舶动态</param>
        /// <param name="excludeProjectId">排除掉的项目Id</param>
        /// <returns></returns>
        public async Task<ShipMovement> GetShipMovementAsync(Guid shipId, ShipType shipType, ShipMovementStatus shipMovementStatus, Guid excludeProjectId)
        {
            return await _dbShipMovement.GetFirstAsync(t => t.IsDelete == 1 && t.ShipId == shipId && t.ShipType == shipType && t.Status == shipMovementStatus && t.ProjectId != excludeProjectId);
        }

        /// <summary>
        /// 获取一个项目(部分字段（Id,Name,CreateId）)
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns></returns>
        private async Task<Project?> GetProjectPartAsync(Guid projectId)
        {
            return await _dbProject.AsQueryable().Where(t => t.Id == projectId && t.IsDelete == 1).Select(t => new Project() { Id = t.Id, Name = t.Name, CreateId = t.CreateId }).FirstAsync();
        }

        /// <summary>
        /// 获取船舶集合（部分字段（PomId, Name,ShipType,TypeId））
        /// </summary>
        /// <returns></returns>
        private async Task<List<ShipPartDto>> GetShipPartsAsync(Guid[] shipIds, ShipType shipType)
        {
            if (!shipIds.Any())
            {
                return new List<ShipPartDto>();
            }
            if (shipType == ShipType.OwnerShip)
            {
                return await _dbOwnerShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = ShipType.OwnerShip, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
            }
            else if (shipType == ShipType.SubShip)
            {
                return await _dbSubShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = ShipType.SubShip, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
            }
            return new List<ShipPartDto>();
        }
        /// <summary>
        /// 获取船舶列表
        /// </summary>
        /// <param name="shipType"></param>
        /// <returns></returns>
        private async Task<List<ShipPartDto>> GetShipPartsAsync(ShipType shipType)
        {
            if (shipType == ShipType.OwnerShip)
            {
                return await _dbOwnerShip.AsQueryable().Where(t => t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = ShipType.OwnerShip, Name = t.Name, ShipKindTypeId = t.TypeId, CompanyId = t.CompanyId }).ToListAsync();
            }
            else if (shipType == ShipType.SubShip)
            {
                return await _dbSubShip.AsQueryable().Where(t => t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = ShipType.SubShip, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
            }
            return new List<ShipPartDto>();
        }
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectInfoDto>> GetProjectInfoAsync()
        {
            return await _dbProject.AsQueryable().Where(p => p.IsDelete == 1).Select(p => new ProjectInfoDto { ProjectId = p.Id, ProjectName = p.Name }).ToListAsync();
        }
        /// <summary>
        /// 获取船舶类型集合（部分字段（PomId,Name））
        /// </summary>
        /// <returns></returns>
        private async Task<List<ShipPingType>> GetShipPingTypePartsAsync(Guid?[] shipTypeIds)
        {
            if (!shipTypeIds.Any())
            {
                return new List<ShipPingType>();
            }
            return await _dbShipPingType.AsQueryable().Where(t => t.IsDelete == 1 && shipTypeIds.Contains(t.PomId)).Select(t => new ShipPingType() { PomId = t.PomId, Name = t.Name }).ToListAsync();
        }
        private async Task<bool> JjtMessageReminder(Guid? ShipId, ShipType shipType, String? ProjectName, String? ProjecnNames)
        {
            var shipName = await GetShipNamePartsAsync(ShipId, shipType);
            var user = new List<string>
            {
                "2016146340"
            };
            var jjtMessageReminder = new SingleMessageTemplateRequestDto()
            {
                IsAll = false,
                MessageType = "text",
                UserIds = user,
                TextContent = shipName + "目前在" + ProjectName + "进行进场，" + _currentUser.Name + "需要进场至" + ProjecnNames + "中，请协调处理！"
            };
            JjtUtils.SinglePushMessage(jjtMessageReminder);
            return true;
        }
        private async Task<string?> GetShipNamePartsAsync(Guid? ShipId, ShipType shipType)
        {
            if (shipType == ShipType.OwnerShip)
            {
                return await _dbOwnerShip.AsQueryable().Where(t => t.PomId == ShipId && t.IsDelete == 1).Select(t => t.Name).SingleAsync();
            }
            else if (shipType == ShipType.SubShip)
            {
                return await _dbSubShip.AsQueryable().Where(t => t.PomId == ShipId && t.IsDelete == 1).Select(t => t.Name).SingleAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
