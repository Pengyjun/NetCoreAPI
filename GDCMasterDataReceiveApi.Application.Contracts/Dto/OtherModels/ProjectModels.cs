namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 项目类其他models
    /// </summary>
    public class ProjectModels
    {
    }
    /// <summary>
    /// 曾用名列表
    /// </summary>
    public class ZMDGS_OLDNAME
    {
        /// <summary>
        /// 项目主数据编码
        /// 12
        /// </summary>
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 曾用名
        /// 500
        /// </summary>
        public string? ZOLDNAME { get; set; }
    }
}
