using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.IRepository;

namespace GHMonitoringCenterApi.Application.Service
{
    /// <summary>
    /// 对象变更业务层
    /// </summary>
    public class EntityChangeService : IEntityChangeService
    {
        /// <summary>
        /// 对象变更记录表
        /// </summary>
        private readonly IBaseRepository<EntityChangeRecord> _dbEntityChangeRecord;
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly ISqlSugarClient _dbContext;

        public EntityChangeService(ISqlSugarClient dbContext, IBaseRepository<EntityChangeRecord> dbEntityChangeRecord)
        {
            _dbContext = dbContext;
            _dbEntityChangeRecord = dbEntityChangeRecord;
        }

        /// <summary>
        /// 变更对象变更记录
        /// </summary>
        /// <returns></returns>
        public async Task RecordEntitysChangeAsync(EntityType type, params Guid[] itemIds)
        {
            if (itemIds == null || !itemIds.Any())
            {
                return;
            }
            var records = new List<EntityChangeRecord>();
            var existRecords = await _dbEntityChangeRecord.GetListAsync(t => t.Type == type && itemIds.Contains(t.ItemId));
            var addRecords = new List<EntityChangeRecord>();
            var updateRecords = new List<EntityChangeRecord>();
            foreach (var itemId in itemIds)
            {
                var existRecord = existRecords.FirstOrDefault(t => t.ItemId == itemId);
                if (existRecord == null)
                {
                    addRecords.Add(new EntityChangeRecord() { Id = GuidUtil.Next(), IsPush = false, ItemId = itemId, ChangeTime = DateTime.Now, PushStatus = PushStatus.UnPush, Type = type });
                }
                else
                {
                    existRecord.IsPush = false;
                    existRecord.ChangeTime = DateTime.Now;
                    updateRecords.Add(existRecord);
                }
            }
            if (addRecords.Any())
            {
                await  _dbEntityChangeRecord.InsertRangeAsync(addRecords);
            }
            if (updateRecords.Any())
            {
                await _dbEntityChangeRecord.UpdateRangeAsync(updateRecords);
            }
        }
    }
}
