namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 上传文件响应dto
    /// </summary>
    public class UploadResponse
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string? OriginName { get; set; }
        /// <summary>
        /// 文件后缀
        /// </summary>
        public string? SuffixName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long? FileSize { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 业务id
        /// </summary>
        public Guid? BId { get; set; }
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid? FileId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid? UserId { get; set; }
    }
}
