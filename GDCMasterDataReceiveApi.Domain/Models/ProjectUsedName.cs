using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 项目曾用名
    /// </summary>
    [SugarTable("t_projectusedname", IsDisabledDelete = true)]
    public class ProjectUsedName:BaseEntity<long>
    {
        /// <summary>
        /// 曾用名
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZOLDNAME { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        [SugarColumn(Length = 128)]
        public  string? ZPROJECT { get; set; }
    }
}
