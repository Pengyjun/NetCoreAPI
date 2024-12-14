using HNKC.CrewManagePlatform.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar;
using System.Security.Claims;
using UtilsSharp;
using HNKC.CrewManagePlatform.Services.Interface.AuditLog;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Filters
{

    /// <summary>
    /// action结果过滤器
    /// </summary>
    public class ActionResultFilter : IAsyncResultFilter
    {
        public IAuditLogService auditLogService { get; set; }
        public ActionResultFilter(IAuditLogService auditLogService)
        {
            this.auditLogService = auditLogService;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var obj = (context.Result as BadRequestObjectResult);
            if (obj != null&& obj.StatusCode.HasValue&&obj.StatusCode.Value==400)
            {
                await next.Invoke();
            }
            await auditLogService.SaveAuditLogAsync();
            await next.Invoke();
        }
    }
}
