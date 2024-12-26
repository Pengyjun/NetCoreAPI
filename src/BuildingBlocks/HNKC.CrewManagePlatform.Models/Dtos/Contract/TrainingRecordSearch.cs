namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecordSearch
    {
        /// <summary>
        /// 填报人
        /// </summary>
        public Guid? FillRepUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? FillRepUserName{ get; set; }
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
        /// 培训地点
        /// </summary>
        public string? TrainingAddress { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public string? TrainingCount { get; set; }
        /// <summary>
        /// 具体人员
        /// </summary>
        public string? UserDetails { get; set; }
    }
}
