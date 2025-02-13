using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipPlan;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Mvc;
using Npgsql.TypeHandlers.DateTimeHandlers;
using NPOI.HPSF;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.ShipPlan
{
    public class ShipPlanService : IShipPlanService
    {
        #region 依赖注入
     
        public ISqlSugarClient dbContent { get; set; }

        public IMapper mapper { get; set; }
        public ILogService logService { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        public ShipPlanService( ISqlSugarClient dbContent, IMapper mapper, ILogService logService, GlobalObject globalObject)
        {
            this.dbContent = dbContent;
            this.mapper = mapper;
            this.logService = logService;
            _globalObject = globalObject;
        }
        #endregion
        /// <summary>
        /// 保存船舶计划产值
        /// </summary>
        /// <param name="saveShipPlanRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveShipPlanAsync(SaveShipPlanRequestDto saveShipPlanRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ();
            var shipTypeId= await dbContent.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && x.PomId == saveShipPlanRequestDto.ShipId).Select(x=>x.TypeId.Value).FirstAsync();
            if (saveShipPlanRequestDto.ReuqestType)
            {
                ShipYearPlanProduction model=mapper.Map<SaveShipPlanRequestDto, ShipYearPlanProduction>(saveShipPlanRequestDto);
                int days= TimeHelper.GetTimeSpan(saveShipPlanRequestDto.StartTime, saveShipPlanRequestDto.EndTime).Days;
                model.Days = days;
                model.ShipTypeId = shipTypeId;
                model.Year=DateTime.Now.Year;
                var total = await dbContent.Queryable<ShipYearPlanProduction>().CountAsync(x => x.IsDelete == 1 && x.ShipId == saveShipPlanRequestDto.ShipId
                 && x.ProjectId == saveShipPlanRequestDto.ProjectId);

                if (total >= 1)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success("数据已存在", Domain.Shared.Enums.HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
                await dbContent.Insertable<ShipYearPlanProduction>(model).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            } 
            else  if(!saveShipPlanRequestDto.ReuqestType) 
            {
                ShipYearPlanProduction model = mapper.Map<SaveShipPlanRequestDto, ShipYearPlanProduction>(saveShipPlanRequestDto);
                int days = TimeHelper.GetTimeSpan(saveShipPlanRequestDto.StartTime, saveShipPlanRequestDto.EndTime).Days;
                model.Days = days;
                model.ShipTypeId = shipTypeId;
                var entity = await dbContent.Queryable<ShipYearPlanProduction>().Where(x => x.IsDelete == 1 && x.ShipId == saveShipPlanRequestDto.ShipId
                 && x.ProjectId == saveShipPlanRequestDto.ProjectId).FirstAsync();
                if (entity ==null)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success("数据不存在", Domain.Shared.Enums.HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                model.Id = entity.Id;
                model.Year = entity.Year;
                await dbContent.Updateable<ShipYearPlanProduction>(model).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();

            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 查询船舶计划 列表
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ShipPlanResponseDto>>> SearchShipPlanAsync(ShipPlanRequestDto shipPlanRequestDto)
        {
            ResponseAjaxResult<List<ShipPlanResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ShipPlanResponseDto>>();
            RefAsync<int> total = 0;
            //导出
            if (shipPlanRequestDto.IsFullExport)
            {
                shipPlanRequestDto.PageSize = int.MaxValue;
            }
            var result = await dbContent.Queryable<ShipYearPlanProduction>()
               .Where(x => x.IsDelete == 1 && x.Year == shipPlanRequestDto.Year)
               .WhereIF(shipPlanRequestDto.StartTime.HasValue && shipPlanRequestDto.EndTime.HasValue, x => x.StartTime >= shipPlanRequestDto.StartTime && x.EndTime <= shipPlanRequestDto.EndTime)
               .WhereIF(!string.IsNullOrWhiteSpace(shipPlanRequestDto.ProjectName), x => x.ProjectName.Contains(shipPlanRequestDto.ProjectName))
               .WhereIF(!string.IsNullOrWhiteSpace(shipPlanRequestDto.ShipName), x => x.ShipName.Contains(shipPlanRequestDto.ShipName))
               .Select(x => new ShipPlanResponseDto {
                   Id = x.Id,
                   Days = x.Days,
                   EigPlannedOutputValue = x.EigPlannedOutputValue,
                   ElePlannedOutputValue = x.ElePlannedOutputValue,
                   EndTime = x.EndTime,
                   FivPlannedOutputValue = x.FivPlannedOutputValue,
                   FourPlannedOutputValue = x.FourPlannedOutputValue,
                   NinPlannedOutputValue = x.NinPlannedOutputValue,
                   OnePlannedOutputValue = x.OnePlannedOutputValue,
                   Price = x.Price,
                   ProjectId = x.ProjectId,
                   ProjectName = x.ProjectName,
                   QuantityWork = x.QuantityWork,
                   SevPlannedOutputValue = x.SevPlannedOutputValue,
                   ShipStatusType = x.ShipStatusType,
                   ShipStatusName = x.ShipStatusName,
                   Remark = x.Remark,
                   ShipId = x.ShipId,
                   ShipName = x.ShipName,
                   SixPlannedOutputValue = x.SixPlannedOutputValue,
                   StartTime = x.StartTime,
                   TenPlannedOutputValue = x.TenPlannedOutputValue,
                   ThreePlannedOutputValue = x.ThreePlannedOutputValue,
                   TwePlannedOutputValue = x.TwePlannedOutputValue,
                   TwoPlannedOutputValue = x.TwoPlannedOutputValue,
                   Year = x.Year
               })
               .ToPageListAsync(shipPlanRequestDto.PageIndex, shipPlanRequestDto.PageSize, total);

            responseAjaxResult.Data = result;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        ///  保存船舶完成产值
        /// </summary>
        /// <param name="saveShipCompleteRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveShipCompleteAsync(SaveShipCompleteRequestDto saveShipCompleteRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();
            if (saveShipCompleteRequestDto.ReuqestType)
            {
                ShipCompleteProduction model = mapper.Map<SaveShipCompleteRequestDto, ShipCompleteProduction>(saveShipCompleteRequestDto);
                model.DateDay = saveShipCompleteRequestDto.DateDay;
                model.DiffProductionValue = model.CompleteOutputValue - model.PlanOutputValue;
                var total = await dbContent.Queryable<ShipCompleteProduction>().CountAsync(x => x.IsDelete == 1 && x.ShipId == saveShipCompleteRequestDto.ShipId);

                if (total >= 1)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success("数据已存在", Domain.Shared.Enums.HttpStatusCode.VerifyFail);
                    return responseAjaxResult;
                }
                await dbContent.Insertable<ShipCompleteProduction>(model).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else if (!saveShipCompleteRequestDto.ReuqestType)
            {
                ShipCompleteProduction model = mapper.Map<SaveShipCompleteRequestDto, ShipCompleteProduction>(saveShipCompleteRequestDto);
                model.DateDay = saveShipCompleteRequestDto.DateDay;
                model.DiffProductionValue = model.CompleteOutputValue - model.PlanOutputValue;
                var entity = await dbContent.Queryable<ShipCompleteProduction>().Where(x => x.IsDelete == 1 && x.ShipId == saveShipCompleteRequestDto.ShipId
                 ).FirstAsync();
                if (entity == null)
                {
                    responseAjaxResult.Data = false;
                    responseAjaxResult.Success("数据不存在", Domain.Shared.Enums.HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                model.Id = entity.Id;
              
                await dbContent.Updateable<ShipCompleteProduction>(model).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();

            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 搜索船舶完成产值列表
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCompleteResponseDto>>> SearchShiCompleteAsync(ShipCompleteRequestDto shipPlanRequestDto)
        {
            ResponseAjaxResult<List<ShipCompleteResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ShipCompleteResponseDto>>();
            RefAsync<int> total = 0;
            //导出
            if (shipPlanRequestDto.IsFullExport)
            {
                shipPlanRequestDto.PageSize = int.MaxValue;
            }
            var result = await dbContent.Queryable<ShipCompleteProduction>()
               .Where(x => x.IsDelete == 1)
               .WhereIF(shipPlanRequestDto.StartMonth.HasValue && shipPlanRequestDto.EndMonth.HasValue, x => x.DateDay >= shipPlanRequestDto.StartMonth && x.DateDay <= shipPlanRequestDto.EndMonth)
               .WhereIF(!string.IsNullOrWhiteSpace(shipPlanRequestDto.ShipName), x => x.ShipName.Contains(shipPlanRequestDto.ShipName))
               .Select(x => new ShipCompleteResponseDto
               { Id = x.Id,
                   CompleteOutputValue = x.CompleteOutputValue,
                   DiffProductionValue = x.DiffProductionValue,
                   PlanOutputValue = x.PlanOutputValue,
                   Price = x.Price,
                   QuantityWork = x.QuantityWork,
                   Reason = x.Reason,
                   Remark = x.Remark,
                   ShipId = x.ShipId,
                   ShipName = x.ShipName,
                   DateDay = x.DateDay
               })
               .ToPageListAsync(shipPlanRequestDto.PageIndex, shipPlanRequestDto.PageSize, total);

            responseAjaxResult.Data = result;
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 根据用户填的船舶计划数据 生产图  
        /// </summary>
        /// <param name="type">1是项目为中心   2是船舶为中心</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ShipPlanImageResponseDto>> SearchShipPlanImagesAsync(int type)
        {
            ResponseAjaxResult<ShipPlanImageResponseDto> response = new ResponseAjaxResult<ShipPlanImageResponseDto>();
            if (type == 1)
            {
                //第一级
                ShipPlanImageResponseDto shipPlanImageResponseDtoItem = new ShipPlanImageResponseDto()
                {
                    Name = $"公司{DateTime.Now.Year}年船舶计划",
                    Days = Utils.IsLeapYear(DateTime.Now.Year) ? 366 : 365,
                    Children = new List<ShipPlanImageResponseDto>(),
                    Id = GuidUtil.Next(),
                    StartTime = DateTime.Now.ToString("yyyy-01-01"),
                    EndTime = DateTime.Now.ToString("yyyy-12-31"),
                    Sid=1,
                    Pid=0,
                };
                //第二级
                List<ShipPlanImageResponseDto> shipPlanImageResponseDtos = new List<ShipPlanImageResponseDto>()
                { 
                  new ShipPlanImageResponseDto(){ 
                   Name="耙吸船",
                    ShipType="06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf".ToGuid(),
                     Sid=2,
                     Pid=1,
                  },
                  new ShipPlanImageResponseDto(){
                   Name="绞吸船",
                    ShipType="f1718922-c213-4409-a59f-fdaf3d6c5e23".ToGuid(),
                     Sid=3,
                     Pid=1,
                  },
                  new ShipPlanImageResponseDto(){
                   Name="抓斗船",
                   ShipType="6959792d-27a4-4f2b-8fa4-a44222f08cb2".ToGuid(),
                    Sid=4,
                    Pid=1,
                  },
                };
                shipPlanImageResponseDtoItem.Children= shipPlanImageResponseDtos;
                //第三级
                List<ShipPlanImageResponseDto> pxShip = new List<ShipPlanImageResponseDto>();
                List<ShipPlanImageResponseDto> jxShip = new List<ShipPlanImageResponseDto>();
                List<ShipPlanImageResponseDto> zdShip = new List<ShipPlanImageResponseDto>();
                //第四级
                List<ShipPlanImageResponseDto> pxProjectShip = new List<ShipPlanImageResponseDto>();
                List<ShipPlanImageResponseDto> jxProjectShip = new List<ShipPlanImageResponseDto>();
                List<ShipPlanImageResponseDto> zdProjectShip = new List<ShipPlanImageResponseDto>();


                #region 数据查询
                var shipYearPlanList = await dbContent.Queryable<ShipYearPlanProduction>()
                    .Where(x => x.IsDelete == 1).ToListAsync();
                //船舶类型
                var shipTypeIds = shipYearPlanList.Select(x => x.ShipTypeId).ToList();
                //船舶类型
                var shipTypeList = await dbContent.Queryable<ShipPingType>().Where(x => x.IsDelete == 1 && shipTypeIds.Contains(x.PomId.Value)).OrderBy(x => x.Sequence).ToListAsync();
                #endregion


                #region 数据结构拼接
                //第三级查询
                var sid = 4;
                foreach (var item in shipYearPlanList)
                {
                    var result = shipYearPlanList.Where(x => x.ShipTypeId == item.ShipTypeId)
                       .Select(x => new ShipPlanImageResponseDto()
                       {
                           Children = new List<ShipPlanImageResponseDto>(),
                           Name = x.ShipName,
                           Id = x.ShipId.Value,
                           ShipType = x.ShipTypeId,
                           StartTime = x.StartTime.ToString("yyyy-MM-dd"),
                           EndTime = x.EndTime.ToString("yyyy-MM-dd"),
                           Days = TimeHelper.GetTimeSpan(x.StartTime, x.EndTime).Days,
                           ShipName = x.ShipName,
                           Pid= item.ShipTypeId== "06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf".ToGuid()?2:
                           item.ShipTypeId == "f1718922-c213-4409-a59f-fdaf3d6c5e23".ToGuid()?3:
                           item.ShipTypeId == "6959792d-27a4-4f2b-8fa4-a44222f08cb2".ToGuid()?4:0,
                       })
                       .ToList();
                    if (
                        shipPlanImageResponseDtos.Count(x => x.ShipType == item.ShipTypeId)> 0
                        && shipPlanImageResponseDtos.Where(x => x.ShipType == item.ShipTypeId).First().Children.Count==0)
                    {
                        
                        foreach (var res in result)
                        {
                            res.Sid = sid +=1;
                        }
                        shipPlanImageResponseDtos.Where(x => x.ShipType == item.ShipTypeId).First().Children.AddRange(result);
                      
                    }
                    
                }
                //第四级查询
                foreach (var shipItem in shipPlanImageResponseDtos)
                {
                    
                    foreach (var sitem in shipItem.Children)
                    {
                        var result = shipYearPlanList.Where(x => x.ShipTypeId == sitem.ShipType && x.ShipId == sitem.Id)
                       .Select(x => new ShipPlanImageResponseDto()
                       {
                           Children = new List<ShipPlanImageResponseDto>(),
                           Name = string.IsNullOrWhiteSpace(x.ProjectName) ? x.ShipStatusName : x.ProjectName,
                           ShipType = x.ShipTypeId,
                           StartTime = x.StartTime.ToString("yyyy-MM-dd"),
                           EndTime = x.EndTime.ToString("yyyy-MM-dd"),
                           Days = TimeHelper.GetTimeSpan(x.StartTime, x.EndTime).Days,
                           Id = x.Id,
                           Pid= shipItem.Pid,
                           ShipName = x.ShipName,
                           
                       })
                       .ToList();

                        foreach (var s in result)
                        {
                            s.Sid = sid += 1;
                        }
                        sitem.Children.AddRange(result);
                    }

                    shipItem.StartTime = shipItem.Children?.Min(x => x.StartTime);
                    shipItem.EndTime = shipItem.Children?.Max(x => x.EndTime);
                    shipItem.Children = shipItem.Children!=null?shipItem.Children.GroupBy(x => x.ShipName).Select(x => x.First()).ToList():new List<ShipPlanImageResponseDto>();
                }
                response.Data = shipPlanImageResponseDtoItem;
                response.Count= shipPlanImageResponseDtos.Count;
                #endregion



            }
            else if (type == 2) 
            {

                ShipPlanImageResponseDto shipPlanImageResponseDtoItem = new ShipPlanImageResponseDto()
                {
                    Name = $"公司{DateTime.Now.Year}年船舶计划",
                    Days = Utils.IsLeapYear(DateTime.Now.Year) ? 366 : 365,
                    Children = new List<ShipPlanImageResponseDto>(),
                    Id = GuidUtil.Next(),
                    StartTime = DateTime.Now.ToString("yyyy-01-01"),
                    EndTime = DateTime.Now.ToString("yyyy-12-31")
                };
                List<ShipPlanImageResponseDto> pxShip = new List<ShipPlanImageResponseDto>();
                List<ShipPlanImageResponseDto> jxShip = new List<ShipPlanImageResponseDto>();
                List<ShipPlanImageResponseDto> zdShip = new List<ShipPlanImageResponseDto>();

                var shipYearPlanList = await dbContent.Queryable<ShipYearPlanProduction>()
                   .Where(x => x.IsDelete == 1).ToListAsync();
                var shipTypeIds = shipYearPlanList.Select(x => x.ShipTypeId).ToList();
                var shipTypeList = await dbContent.Queryable<ShipPingType>().Where(x => x.IsDelete == 1 && shipTypeIds.Contains(x.PomId.Value)).OrderBy(x => x.Sequence).ToListAsync();
                List<ShipPlanImageResponseDto> shipPlanImageResponseDtos = new List<ShipPlanImageResponseDto>();
                var index = 0;
                foreach (var item in shipTypeList)
                {

                    var shipPlanImageitem = new ShipPlanImageResponseDto()
                    {
                        Name = item.Name.Replace("挖泥", "").Trim(),
                        Days = Utils.IsLeapYear(DateTime.Now.Year) ? 366 : 365,
                        Children = new List<ShipPlanImageResponseDto>(),
                        Id = GuidUtil.Next(),
                        StartTime = DateTime.Now.ToString("yyyy-01-01"),
                        EndTime = DateTime.Now.ToString("yyyy-12-31")
                    };
                    //过滤类型
                    shipYearPlanList = shipYearPlanList.Where(x => x.ShipTypeId == item.PomId).ToList();
                    foreach (var ship in shipYearPlanList)
                    {
                        if (index == 0)
                        {
                            pxShip.Add(new ShipPlanImageResponseDto()
                            {
                                Name = ship.ShipName,
                                Days = TimeHelper.GetTimeSpan(ship.StartTime, ship.EndTime).Days,
                                Children = new List<ShipPlanImageResponseDto>(),
                                Id = ship.Id,
                                StartTime = ship.StartTime.ToString("yyyy-MM-dd"),
                                EndTime = ship.EndTime.ToString("yyyy-MM-dd"),
                            });
                            shipPlanImageitem.Children = pxShip;
                        }
                        else if (index == 1)
                        {
                            jxShip.Add(new ShipPlanImageResponseDto()
                            {
                                Name = ship.ShipName,
                                Days = TimeHelper.GetTimeSpan(ship.StartTime, ship.EndTime).Days,
                                Children = new List<ShipPlanImageResponseDto>(),
                                Id = ship.Id,
                                StartTime = ship.StartTime.ToString("yyyy-MM-dd"),
                                EndTime = ship.EndTime.ToString("yyyy-MM-dd"),
                            });
                            shipPlanImageitem.Children = jxShip;
                        }
                        else if (index == 2)
                        {
                            zdShip.Add(new ShipPlanImageResponseDto()
                            {
                                Name = ship.ShipName,
                                Days = TimeHelper.GetTimeSpan(ship.StartTime, ship.EndTime).Days,
                                Children = new List<ShipPlanImageResponseDto>(),
                                Id = ship.Id,
                                StartTime = ship.StartTime.ToString("yyyy-MM-dd"),
                                EndTime = ship.EndTime.ToString("yyyy-MM-dd"),
                            });
                            shipPlanImageitem.Children = zdShip;
                        }
                    }
                    shipPlanImageResponseDtos.Add(shipPlanImageitem);
                }
                shipPlanImageResponseDtoItem.Children = shipPlanImageResponseDtos;
                response.Data = shipPlanImageResponseDtoItem;
            }
            response.Success();
            return response;
        }
    }
}
