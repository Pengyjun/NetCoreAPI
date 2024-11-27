namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{
    /// <summary>
    /// 数据列信息
    /// </summary>
    public class Tables
    {
        /// <summary>
        /// 表
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string? TName { get; set; }
    }
    /// <summary>
    /// 字段类型
    /// </summary>
    public class ColumnsInfo
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ColumnSize { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ColumnsInfoMapper
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public string? TYPE_NAME { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public int COLUMN_SIZE { get; set; }
    }
}
