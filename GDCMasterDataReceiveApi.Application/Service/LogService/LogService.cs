using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ILogService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.Extensions.Logging;
using SqlSugar;
using SqlSugar.Extensions;

namespace GDCMasterDataReceiveApi.Application.Service.LogService
{


    /// <summary>
    /// 查询日志接口实现层
    /// </summary>
    public class LogService : ILogService
    {
        #region 依赖注入
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LogService> logger;

        public LogService(ISqlSugarClient dbContext, IMapper mapper, ILogger<LogService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            this.logger = logger;
        }
        #endregion


        /// <summary>
        /// 查询审计日志
        /// </summary>
        /// <param name="auditLogRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<AuditLogResponseDto>>> SearchAuditLogAsync(AuditLogRequestDto auditLogRequestDto)
        {
            ResponseAjaxResult<List<AuditLogResponseDto>> responseAjaxResult = new();
            RefAsync<int> total = 0;


            responseAjaxResult.Data = await _dbContext.Queryable<AuditLogs>()
                .WhereIF(!string.IsNullOrWhiteSpace(auditLogRequestDto.StartTime), x =>SqlFunc.ToDate(x.RequestTime) >=auditLogRequestDto.StartTime.ObjToDate())
                .WhereIF(!string.IsNullOrWhiteSpace(auditLogRequestDto.StartTime), x => SqlFunc.ToDate(x.RequestTime) <= auditLogRequestDto.EndTime.ObjToDate())
                .WhereIF(!string.IsNullOrWhiteSpace(auditLogRequestDto.KeyWords),x=>x.Url.Contains(auditLogRequestDto.KeyWords))
                 .Select(x => new AuditLogResponseDto() {
                     Exceptions = x.Exceptions,
                     Ip = x.ClientIpAddress,
                     RequestParame = x.RequestParames,
                     RequestTime = x.RequestTime,
                     ResponseStatus = x.HttpStatusCode,
                     Url = x.Url
                 }).OrderByDescending(x => x.RequestTime)
                 .ToPageListAsync(auditLogRequestDto.PageIndex, auditLogRequestDto.PageSize,total);
            responseAjaxResult.Count= total;
            responseAjaxResult.Success();
            return responseAjaxResult;

        }
        /// <summary>
        /// 查询接收日志
        /// </summary>
        /// <param name="receiveLogRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<ReceiveLogResponseDto>>> SearchReceiveLogAsync(ReceiveLogRequestDto receiveLogRequestDto)
        {
            ResponseAjaxResult<List<ReceiveLogResponseDto>> responseAjaxResult = new();
            RefAsync<int> total = 0;
            responseAjaxResult.Data = await _dbContext.Queryable<ReceiveRecordLog>()
                .WhereIF(!string.IsNullOrWhiteSpace(receiveLogRequestDto.StartTime), x => x.CreateTime >= receiveLogRequestDto.StartTime.ObjToDate())
                .WhereIF(!string.IsNullOrWhiteSpace(receiveLogRequestDto.StartTime), x => x.CreateTime <= receiveLogRequestDto.EndTime.ObjToDate())
                .WhereIF(receiveLogRequestDto.ReceiveDataType != 0, x => x.ReceiveType == receiveLogRequestDto.ReceiveDataType)
                 .Select(x => new ReceiveLogResponseDto()
                 {
                     FailMessage = x.FailMessage,
                     ReceiceDataType = x.ReceiveType,
                     ReceiveCount = x.ReceiveNumber,
                     RequestParame = x.RequestParame,
                     RequestTime = x.CreateTime,
                     SuccessCount = x.SuccessNumber

                 })
                 .OrderByDescending(x=>x.RequestTime).ToPageListAsync(receiveLogRequestDto.PageIndex, receiveLogRequestDto.PageSize, total);
            responseAjaxResult.Count = total;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}
