using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 实体改变记录表
    /// </summary>
    [SugarTable("t_entitychangerecord", IsDisabledDelete = true)]
    public class EntityChangeRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey =true,Length = 36)]
        public Guid Id { get; set; }
        /// <summary>
        /// 推送类型  1 推送项目  2  待定
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public EntityType Type { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? tableName { get; set; }
        /// <summary>
        /// 主键ID  推送类型的ID  1 就是项目ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ItemId { get; set; }
        /// <summary>
        /// 推送状态  1 未推送   2 推送失败  3 推送成功 
        /// </summary>
        [SugarColumn(ColumnDataType ="int",DefaultValue ="1")]
        public PushStatus PushStatus { get; set; }
        /// <summary>
        /// 实体改变时间
        /// </summary>
        [SugarColumn(ColumnDataType ="datetime")]
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 推送pom系统的时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? PushTime { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? FailReason { get; set; }
        /// <summary>
        /// 是否推送
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsPush { get; set; }
    }
}
