using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecordRequest : PageRequest
    {
        /// <summary>
        /// 填报开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 填报结束时间
        /// </summary>
        public string? EndTime { get; set; }
        /// <summary>
        /// 填报类型
        /// </summary>
        public string? TraningType { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SaveTrainingRecord
    {
        /// <summary>
        /// 1新增 2 修改
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? BId { get; set; }
        /// <summary>
        /// 培训类型：安全培训...
        /// </summary>
        public string? TrainingType { get; set; }
        /// <summary>
        /// 培训日期
        /// </summary>
        public DateTime? TrainingTime { get; set; }
        /// <summary>
        /// 培训主题
        /// </summary>
        public string? TrainingTitle { get; set; }
        /// <summary>
        /// 培训地点
        /// </summary>
        public string? TrainingAddress { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public List<UploadResponse>? Scans { get; set; }
        /// <summary>
        /// 参与人员
        /// </summary>
        public List<string>? UIds { get; set; }
    }
}
