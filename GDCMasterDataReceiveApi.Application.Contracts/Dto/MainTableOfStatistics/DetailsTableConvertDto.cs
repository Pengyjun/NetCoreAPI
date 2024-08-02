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
}
