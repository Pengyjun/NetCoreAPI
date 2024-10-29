using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 列表字段展示权限
    /// </summary>
    [SugarTable("t_searchfiledcolumnspermission", IsDisabledDelete = true)]
    public class SearchFiledColumnsPermission : BaseEntity<long>
    {
        /// <summary>
        /// 字段列 字符串json
        /// </summary>
        [SugarColumn(Length = 3000)]
        public string? FiledColumns { get; set; }
        /// <summary>
        /// 租户  / 后续根据登录用户保存  目前暂不处理
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? Tenant { get; set; }
        /// <summary>
        /// 接口列表id
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InterfaceId { get; set; }
    }
}
