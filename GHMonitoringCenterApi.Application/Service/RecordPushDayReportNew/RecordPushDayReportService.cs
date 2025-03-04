using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.RecordPushDayReport;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.RecordPushDayReport;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using UtilsSharp;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;

namespace GHMonitoringCenterApi.Application.Service.RecordPushDayReportNew
{
    /// <summary>
    /// 
    /// </summary>
    public class RecordPushDayReportService : IRecordPushDayReportService
    {

        #region 注入实例

        /// <summary>
        /// 每天推送日报结果记录
        /// </summary>
        private readonly IBaseRepository<RecordPushDayReport> baseRecordPushDayReportRepository;


        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarClient _dbContext;

        public RecordPushDayReportService(IBaseRepository<RecordPushDayReport> baseRecordPushDayReportRepository, GlobalObject globalObject, ISqlSugarClient dbContext)
        {
            this.baseRecordPushDayReportRepository = baseRecordPushDayReportRepository;
            _globalObject = globalObject;
            _dbContext = dbContext;
        }
        #endregion

        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }


        /// <summary>
        /// 获取每日最新一条推送数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RecordPushDayReportResponseDto>>> GetRecordPushDayReportListAsync(RecordPushDayReportRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RecordPushDayReportResponseDto>>();

            var startDate = Convert.ToInt32(requestDto.StartDateDay);
            var endDate = Convert.ToInt32(requestDto.EndDateDay);

            var subQuery = _dbContext.Queryable<RecordPushDayReport>()
                .GroupBy(x => x.DateDay)
                .Select(x => new
                {
                    dateday = x.DateDay,
                    max_createtime = SqlFunc.AggregateMax(x.CreateTime)
                });

            var result = _dbContext.Queryable<RecordPushDayReport>()
                .InnerJoin(subQuery, (x, y) => x.DateDay == y.dateday && x.CreateTime == y.max_createtime)
                .Where((x,y)=>x.DateDay>= startDate && x.DateDay<=endDate)
                .Select((x, y) => new RecordPushDayReportResponseDto()
                {
                    DateDay = x.DateDay,
                    Json = x.Json,
                })

                .ToList();

            foreach (var item in result)
            {
                var json = JsonHelper.FromJson<JjtSendMessageMonitoringDayReportResponseDto>(item.Json).ToJson(true);
                item.Json = json;
            }


            //var list = await baseRecordPushDayReportRepository.AsQueryable().Where(p => p.DateDay >= startDate && p.DateDay <= endDate).Select(it => new RecordPushDayReportResponseDto()
            //{
            //    DateDay = it.DateDay,
            //    Json = it.Json,
            //    Index = SqlFunc.RowNumber(SqlFunc.Desc(it.CreateTime), it.DateDay)
            //}).MergeTable().Where(it => it.Index == 1).ToListAsync();
            responseAjaxResult.Data = result;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

    }
}
