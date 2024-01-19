using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.GetProjectChanges
{

    /// <summary>
    /// 获取日志记录
    /// </summary>
    public class GetProjectChangesResponseDto
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 日志操作类型  1 新增或插入操作 2 更新操作 3 删除操作  4 导入操作 5登录操作 6 登出操作 7 下载操作 8审批操作
        /// </summary>
        public int OperationType { get; set; }
        /// <summary>
        /// 业务模块对应哪个菜单下面的操作
        /// </summary>
        public string? BusinessModule { get; set; }
        /// <summary>
        /// 业务备注说明
        /// </summary>
        public string? BusinessRemark { get; set; }
        /// <summary>
        /// 操作对象  操作的哪张表
        /// </summary>
        public string? OperationObject { get; set; }
        /// <summary>
        /// 操作的哪张表 表名
        /// </summary>
        public string? OperationObjectName { get; set; }
        /// <summary>
        /// 操作实体改变的值
        /// </summary>
        public string? ObjectchangeValue { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public Guid OperationId { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string? OperationName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string? OperationTime { get; set; }

        /// <summary>
        /// 实体变更字段记录表
        /// </summary>
        public List<Logging>? getDiffLogs { get; set; }
    }
    /// <summary>
    /// 日志记录
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// 修改类型
        /// </summary>
        public int? DtoType { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string? Describe { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string? ColumnName { get; set; }
        /// <summary>
        /// 原值
        /// </summary>
        public string? OriginalValue { get; set; }
        /// <summary>
        /// 改变后的值
        /// </summary>
        public string? ChangeValue { get; set; }
    }
}
