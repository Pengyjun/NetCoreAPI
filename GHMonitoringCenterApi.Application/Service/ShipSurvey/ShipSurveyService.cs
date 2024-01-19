using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipSurvey;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipSurvey;
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

namespace GHMonitoringCenterApi.Application.Service.ShipSurvey
{
    public class ShipSurveyService : IShipSurveyService
    {

        #region 依赖注入
        public ISqlSugarClient _dbContext;
        public IMapper mapper { get; set; }
        public ShipSurveyService(ISqlSugarClient dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            this.mapper = mapper;
        }
        #endregion
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveShipSurveyRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> ModifyShipSurveyAsync(SaveShipSurveyRequestDto saveShipSurveyRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var singleShipSurvey= await _dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.ShipSurvey>().Where(x => x.IsDelete == 1 && x.Id == saveShipSurveyRequestDto.Id).FirstAsync();
            if (singleShipSurvey == null)
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST,Domain.Shared.Enums.HttpStatusCode.DataNotEXIST); 
                return responseAjaxResult;
            }
            var shipSurvey = mapper.Map<SaveShipSurveyRequestDto, GHMonitoringCenterApi.Domain.Models.ShipSurvey>(saveShipSurveyRequestDto);
            var reg= await _dbContext.Updateable(shipSurvey).ExecuteCommandAsync();
            if (reg > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS, Domain.Shared.Enums.HttpStatusCode.Success);
            }
            else {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, Domain.Shared.Enums.HttpStatusCode.DataNotEXIST);
            }
            return responseAjaxResult;

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ShipSurveyResponseDto>>> SearchShipSurveyAsync(BaseRequestDto baseRequestDto)
        {
            RefAsync<int> total = 0;
            ResponseAjaxResult<List<ShipSurveyResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<ShipSurveyResponseDto>>();
            var list = await _dbContext.Queryable<GHMonitoringCenterApi.Domain.Models.ShipSurvey>().Where(x => x.IsDelete == 1)
               .WhereIF(!string.IsNullOrWhiteSpace(baseRequestDto.KeyWords), x => x.ShipName.Contains(baseRequestDto.KeyWords.TrimAll()))
               //.Skip((baseRequestDto.PageIndex - 1) * baseRequestDto.PageSize).Take(baseRequestDto.PageSize)
               .Select(x => new ShipSurveyResponseDto()
               {
                   Id = x.Id,
                   AirRoute = x.AirRoute ?? string.Empty,
                   Break = x.Break ?? string.Empty,
                   ClassSociety = x.ClassSociety ?? string.Empty,
                   Countdown = x.Countdown ?? string.Empty,
                   DockingInspectionTime = x.DockingInspectionTime.Value,
                   IntermediateInspection = x.IntermediateInspection.Value,
                   Nationality = x.Nationality ?? string.Empty,
                   NationalityCertStatus = x.NationalityCertStatus ?? string.Empty,
                   NationalityCertValidTime = x.NationalityCertValidTime.Value,
                   NavigationZone = x.NavigationZone ?? string.Empty,
                   NextYearSurveyTime = x.NextYearSurveyTime.Value,
                   PreSurveyTime = x.PreSurveyTime.Value,
                   ShipCertStatus = x.ShipCertStatus ?? string.Empty,
                   ShipName = x.ShipName ?? string.Empty,
                   ShipRegistNumber = x.ShipRegistNumber ?? string.Empty,
                   SpecialSurvey = x.SpecialSurvey.Value,
                   TailAxleBlade = x.TailAxleBlade.Value
               }).OrderBy(x => x.Sort).ToPageListAsync(baseRequestDto.PageIndex, baseRequestDto.PageSize, total);
            List<DateTime> arr = new List<DateTime>();
            foreach (var item in list)
            {
                if (item.NextYearSurveyTime.HasValue)
                {
                    arr.Add(item.NextYearSurveyTime.Value);
                }
                if (item.IntermediateInspection.HasValue)
                {
                    arr.Add(item.IntermediateInspection.Value);
                }
                if (item.DockingInspectionTime.HasValue)
                {
                    arr.Add(item.DockingInspectionTime.Value);
                }
                if (item.TailAxleBlade.HasValue )
                {
                    arr.Add(item.TailAxleBlade.Value);
                }
                if (item.SpecialSurvey.HasValue)
                {
                    arr.Add(item.SpecialSurvey.Value);
                }
                if(!arr.Any())
                {
                    continue;
                }
                //求出最小时间
                var minTime = arr.Min();
                //计算
                DateTime date1 = new DateTime(1900, 01, 01);
                DateTime date3 = DateTime.Now;
                //当前时间距离1900年过了多少天
                var ofDay = (date3.Subtract(date1).Days + 2);
                //计算是否小于倒计时
                var isBeyond = ((minTime.Subtract(date1).Days + 2) - ofDay);
                item.IsBeyond = isBeyond >=90 ? false : true;
                item.ShipCertStatus = item.IsBeyond == true ? "紧急" : "ok";
                //
                item.Countdown = isBeyond<0 ? $"({Math.Abs(isBeyond)})": $"{Math.Abs(isBeyond)}";
                if (item.NationalityCertValidTime.HasValue)
                {
                    DateTime date2 = item.NationalityCertValidTime.Value;
                    item.NationalityCertStatus = (date2.Subtract(date1).Days + 2 - ofDay) >= 60 ? "ok" : "紧急";
                }
                arr.Clear();
            }
            return responseAjaxResult.SuccessResult(list, total);
        }
    }
}
