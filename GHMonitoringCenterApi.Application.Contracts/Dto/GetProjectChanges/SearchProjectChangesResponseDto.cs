using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.GetProjectChanges
{
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class SearchProjectChangesResponseDto
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string? DateTime { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 日志记录
        /// </summary>
        public List<OperationRecords> operationRecords { get; set; }
    }
    /// <summary>
    /// 日志记录
    /// </summary>
    public class OperationRecords
    {
        /// <summary>
        /// 修改类型
        /// </summary>
        public int? DtoType { get; set; }
        /// <summary>
        /// 日志记录Id
        /// </summary>
        public Guid? LoggingId { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// 原值
        /// </summary>
        public string? Original { get; set; }
        /// <summary>
        /// 改后值
        /// </summary>
        public string? Modified { get; set; }
    }
}
