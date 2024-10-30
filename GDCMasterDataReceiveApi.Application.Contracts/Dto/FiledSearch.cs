namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 显示字段
    /// </summary>
    public class FiledSearch
    {
        public string FiledId { get; set; }
        public string FiledName { get; set; }
    }
    /// <summary>
    /// 增改展示字段列
    /// </summary>
    public class FiledColumnsPermissionDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 接口id
        /// </summary>
        public string InterfaceId { get; set; }
        /// <summary>
        /// 字段列json
        /// </summary>
        public string FiledColumns { get; set; }
    }
}
