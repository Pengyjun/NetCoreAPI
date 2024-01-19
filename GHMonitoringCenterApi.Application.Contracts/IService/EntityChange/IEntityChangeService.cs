using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Contracts.IService
{
    /// <summary>
    /// 对象变更业务层
    /// </summary>
    public interface IEntityChangeService
    {
        /// <summary>
        /// 记录对象的变更
        /// </summary>
        /// <returns></returns>
        Task RecordEntitysChangeAsync(EntityType type, params Guid[] itemIds);
    }
}
