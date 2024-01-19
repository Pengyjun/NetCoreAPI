using GHMonitoringCenterApi.Domain;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// 推送业务Dto
    /// </summary>
    public class PushChangeEntityDto<TEntity> where TEntity : BaseEntity<Guid>
    {
        /// <summary>
        /// 变更对象
        /// </summary>
        public TEntity Entity { get; set; }

        /// <summary>
        /// 变更记录对象
        /// </summary>
        public Guid RecordId { get; set; }
    }
}
