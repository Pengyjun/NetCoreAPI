namespace GDCMasterDataReceiveApi.Domain.Shared
{


    /// <summary>
    /// 记录日志请求DTO
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 日志主键ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 日志操作类型  1 新增或插入操作 2 更新操作 3 删除操作  4 导入操作 5登录操作 6 登出操作 7 下载操作 8审批操作 9上传操作
        /// </summary>
        public int OperationType { get; set; }
        /// <summary>
        /// 更新数据Id
        /// </summary>
        public Guid? DataId { get; set; }
        /// 业务模块
        /// </summary>
        public string? BusinessModule { get; set; }
        /// <summary>
        /// 业务说明
        /// </summary>
        public string? BusinessRemark { get; set; }
        /// <summary>
        /// 操作对象
        /// </summary>
        public string? OperationObject { get; set; }
        /// <summary>
        /// 操作对象改变的值
        /// </summary>
        //public string? ObjectchangeValue { get; set; }
        ///// <summary>
        ///// 客户端IP
        ///// </summary>
        //public string? ClientIp { get; set; }
        /// <summary>
        /// 设备信息
        /// </summary>
        //public string? Deviceinformation { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public Guid OperationId { get; set; }
        /// <summary>
        /// 操作人姓名 
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 分组标识
        /// </summary>
        public string GroupIdentity { get; set; }
        /// <summary>
        /// 操作对象改变的值
        /// </summary>
        //public string OperationTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public List<LogDiffDto> logDiffDtos { get; set; }


    }

    public class LogDiffDto
    {
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
