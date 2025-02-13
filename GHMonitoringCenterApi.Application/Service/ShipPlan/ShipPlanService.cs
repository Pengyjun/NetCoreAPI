using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipPlan;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using Npgsql.TypeHandlers.DateTimeHandlers;
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
            if (saveShipPlanRequestDto.ReuqestType)
            {
                ShipYearPlanProduction model=mapper.Map<SaveShipPlanRequestDto, ShipYearPlanProduction>(saveShipPlanRequestDto);
                int days= TimeHelper.GetTimeSpan(saveShipPlanRequestDto.StartTime, saveShipPlanRequestDto.EndTime).Days;
                model.Days = days;
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
    }
}
