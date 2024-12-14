using HNKC.CrewManagePlatform.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.AuditLog
{
    [DependencyInjection]
    public interface IAuditLogService
    {
        Task SetupParameAsync(string actionName, string requestParame);

        Task SaveAuditLogAsync();
    }
}
