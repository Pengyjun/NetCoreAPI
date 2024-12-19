namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 保存请求体
    /// </summary>
    public class CrewArchivesRequest
    {
        /// <summary>
        /// 业务主键id
        /// </summary>
        public Guid? BId { get; set; }
        /// <summary>
        /// 基本信息
        /// </summary>
        public BaseInfoDto? BaseInfoDto { get; set; }
        /// <summary>
        /// 适任及证书
        /// </summary>
        public CertificateOfCompetencyDto? CertificateOfCompetencyDto { get; set; }
        /// <summary>
        /// 学历信息
        /// </summary>
        public EducationalBackgroundDto? EducationalBackgroundDto { get; set; }
        /// <summary>
        /// 职务晋升
        /// </summary>
        public PromotionDto? PromotionDto { get; set; }
        /// <summary>
        /// 任职船舶
        /// </summary>
        public WorkShipDto? WorkShipDto { get; set; }
        /// <summary>
        /// 培训记录
        /// </summary>
        public TrainingRecordDto? TrainingRecordDto { get; set; }
        /// <summary>
        /// 年度考核
        /// </summary>
        public YearCheckDto? YearCheckDto { get; set; }
    }
}
