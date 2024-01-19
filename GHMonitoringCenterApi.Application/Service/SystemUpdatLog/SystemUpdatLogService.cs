using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog;
using GHMonitoringCenterApi.Application.Contracts.IService.SystemUpdatLog;
using GHMonitoringCenterApi.Domain.IRepository;
using Model=GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using System.Text.RegularExpressions;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.SystemUpdatLog
{
    /// <summary>
    /// 系统日志实现层
    /// </summary>
    public class SystemUpdatLogService : ISystemUpdatLogService
    {       
        #region 依赖注入
        public IBaseRepository<Model.SystemUpdatLogs> baseSystemUpdatLogsRepository { get; set; }
        public IBaseRepository<Model.LogPromptSign> baseLogPromptSignRepository { get; set; }
        public IBaseRepository<Model.User> baseUserRepository { get; set; }

        public SystemUpdatLogService(IBaseRepository<Model.SystemUpdatLogs> baseSystemUpdatLogsRepository, IBaseRepository<Model.LogPromptSign> baseLogPromptSignRepository, IBaseRepository<Model.User> baseUserRepository)
        {
            this.baseSystemUpdatLogsRepository = baseSystemUpdatLogsRepository;
            this.baseLogPromptSignRepository = baseLogPromptSignRepository;
            this.baseUserRepository = baseUserRepository;
        }
        #endregion
        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> SaveSystemLogAsync(AddSystemLogRequsetDto addSystemLog,bool IsAdmin, CurrentUser currentUser)
        {

            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (!IsAdmin)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                return responseAjaxResult;
            }
            SystemUpdatLogs systemUpdatLogs = new SystemUpdatLogs()
            {
                Id = GuidUtil.Next(),
                LogContent = addSystemLog.LogContent
            };
            systemUpdatLogs.DateDay = DateTime.Now.ToDateDay();
            if (!string.IsNullOrWhiteSpace(systemUpdatLogs.LogContent))
            {
                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/更新日志/新增",
                    BusinessRemark = "/系统管理/更新日志/新增",
                    OperationId = currentUser.Id,
                    OperationName = currentUser.Name,
                };
                #endregion
                var UserId = await baseLogPromptSignRepository.AsQueryable().ToListAsync();
                await baseLogPromptSignRepository.DeleteAsync(UserId);
                await baseSystemUpdatLogsRepository.AsInsertable(systemUpdatLogs).EnableDiffLogEvent(logDto).ExecuteReturnIdentityAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL);
            }            
            return responseAjaxResult;
        }
        /// <summary>
        /// 查询系统日志
        /// </summary>
        /// <param name="QuerySystemLog"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SystemUpdatLogReponseDto>>> SearchSystemLogAsync(SearchSystemUpdatLogRequsetDto QuerySystemLog)
        {
            RefAsync<int> total = 0;
            ResponseAjaxResult<List<SystemUpdatLogReponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SystemUpdatLogReponseDto>>();
            QuerySystemLog.KeyWords = Utils.ReplaceSQLChar(QuerySystemLog.KeyWords);
            var systemLogList =await baseSystemUpdatLogsRepository.AsQueryable()
                .Where(x=>x.IsDelete == 1)
                .WhereIF(QuerySystemLog.StartingTime != null && QuerySystemLog.EndTime != null, x => x.IsDelete == 1 && x.DateDay>= QuerySystemLog.StartingTime.Value.ToDateDay()&&x.DateDay<= QuerySystemLog.EndTime.Value.ToDateDay())             
                .WhereIF(!string.IsNullOrWhiteSpace(QuerySystemLog.KeyWords),x=>x.IsDelete == 1&& x.LogContent.Contains(QuerySystemLog.KeyWords))
                .OrderByDescending(x => x.CreateTime)
                .Select(x => new SystemUpdatLogReponseDto { LogContent = x.LogContent, Id = x.Id,Time = x.CreateTime.Value })                
                .ToPageListAsync(QuerySystemLog.PageIndex, QuerySystemLog.PageSize, total);            
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = systemLogList;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 系统日志已读标识
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<SearchLogReadSignReponseDto>> SearchLogReadSignAsync(Guid Id)
        {
            ResponseAjaxResult<SearchLogReadSignReponseDto> responseAjaxResult = new ResponseAjaxResult<SearchLogReadSignReponseDto>();
            var User =  await baseLogPromptSignRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.Id == Id).FirstAsync();
            var systemLogList = await baseSystemUpdatLogsRepository.AsQueryable()
               .Where(x => x.IsDelete == 1)
               .OrderByDescending(x => x.CreateTime)
               .Select(x => new SystemUpdatLogReponseDto { LogContent = x.LogContent, Id = x.Id }).ToListAsync();
            
            if (User != null|| systemLogList.Count == 0)
            {
                SearchLogReadSignReponseDto systemUpdatLogReponseDto = new SearchLogReadSignReponseDto()
                {
                    Determinar = false
                };
                responseAjaxResult.Data = systemUpdatLogReponseDto;
                responseAjaxResult.Success();               
            }
            else
            {
                systemLogList[0].LogContent = Regex.Replace(systemLogList[0].LogContent, @"\s+", " ");
                var contentArray = systemLogList[0].LogContent.Trim().Split(' ');
                SearchLogReadSignReponseDto systemUpdatLogReponseDto = new SearchLogReadSignReponseDto()
                {
                    Id = systemLogList[0].Id,
                    LogContent = systemLogList[0].LogContent,
                    LogContentList = contentArray,
                    Determinar = true
                };

                responseAjaxResult.Data = systemUpdatLogReponseDto;
                responseAjaxResult.Success();
                
            }
            return responseAjaxResult;
         }
        /// <summary>
        /// 添加已读标识
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddLogReadSignAsync(Guid Id)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var Logpromptsign =  await baseLogPromptSignRepository.AsQueryable().Where(x => x.Id == Id).SingleAsync();
            if (Logpromptsign != null)
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL);
                return responseAjaxResult;
            }
            LogPromptSign logPromptSign = new LogPromptSign()
            {
                Id = Id
            };

            await baseLogPromptSignRepository.InsertAsync(logPromptSign);           
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }         
        /// <summary>
        /// 修改系统日志内容
        /// </summary>
        /// <param name="updataSystemUpdatLogRequsetDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> UpdataSystemLogAsync(UpdataSystemUpdatLogRequsetDto updataSystemUpdatLogRequsetDto, bool IsAdmin, CurrentUser currentUser)
        {
            
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (!IsAdmin)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                return responseAjaxResult;
            }
            var systemUpdatLogs = await baseSystemUpdatLogsRepository.AsQueryable()
                .Where(x => x.IsDelete == 1 && x.Id == updataSystemUpdatLogRequsetDto.Id).FirstAsync();
            if (systemUpdatLogs == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL);
                responseAjaxResult.Data = false;
                return responseAjaxResult;
            }
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/更新日志/编辑",
                BusinessRemark = "/系统管理/更新日志/编辑",
                OperationId = currentUser.Id,
                OperationName = currentUser.Name,
            };
            #endregion
            systemUpdatLogs.LogContent = updataSystemUpdatLogRequsetDto.LogContent;
            await baseSystemUpdatLogsRepository.AsUpdateable(systemUpdatLogs).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);                                       
            
            return responseAjaxResult;
        }
        /// <summary>
        /// 删除系统日志
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeleteSystemLogAsync(Guid Id, bool IsAdmin, CurrentUser currentUser)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (!IsAdmin)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                return responseAjaxResult;
            }
            var systemUpdatLogMax = await baseSystemUpdatLogsRepository.AsQueryable().OrderByDescending(x => x.CreateTime).ToListAsync();
            var systemUpdatLogs = await baseSystemUpdatLogsRepository.AsQueryable()
                .Where(x => x.IsDelete == 1 && x.Id == Id).SingleAsync();
            if (systemUpdatLogMax[0].Id == Id)
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL);
                return responseAjaxResult;
            }
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/更新日志/删除",
                BusinessRemark = "/系统管理/更新日志/删除",
                OperationId = currentUser.Id,
                OperationName = currentUser.Name,
            };
            #endregion
            systemUpdatLogs.IsDelete = 0;
            await baseSystemUpdatLogsRepository.AsUpdateable(systemUpdatLogs).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
            responseAjaxResult.Data = true;
            return responseAjaxResult;
        }

       
    } 
}
