namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 上传文件响应dto
    /// </summary>
    public class UploadResponseDto
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string? OriginName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? SuffixName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long? FileSize { get; set; }
    }
}
