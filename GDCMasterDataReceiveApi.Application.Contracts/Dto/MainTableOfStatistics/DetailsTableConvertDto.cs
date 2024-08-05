namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics
{
    /// <summary>
    /// 转换dto
    /// </summary>
    public class DetailsTableConvertDto
    { }
    /// <summary>
    /// 
    /// </summary>
    public class MainTableOfStatisticsDto
    {
        public string TableId { get; set; }
        public long Id { get; set; }
        public string InsertOrModify { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 查询到的字典表转换dto
    /// </summary>
    public class MainTableDictoryDto
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 表行数
        /// </summary>
        public int TableRows { get; set; }
        /// <summary>
        /// 主键id 查看详细写入
        /// </summary>
        public string? TableId { get; set; }
    }
}
