namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecordSearch
    {
        /// <summary>
        /// /增改业务主键
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? FillRepUserName { get; set; }
        /// <summary>
        /// 填报时间
        /// </summary>
        public string? FillReportTime { get; set; }
        /// <summary>
        /// 培训时间
        /// </summary>
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 培训主题
        /// </summary>
        public string? TrainingTitle { get; set; }
        /// <summary>
        /// 培训类型
        /// </summary>
        public string? TrainingType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? TrainingTypeName { get; set; }
        /// <summary>
        /// 培训地点
        /// </summary>
        public string? TrainingAddress { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int TrainingCount { get; set; }
        /// <summary>
        /// 具体人员
        /// </summary>
        public string? UserDetails { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? UserIds { get; set; }
    }
}
