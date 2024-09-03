using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 数据权限
    /// </summary>
    [SugarTable("t_dataauthority", IsDisabledDelete = true)]
    public class DataAuthority : BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UId { get; set; }
        /// <summary>
        /// 用户角色id
        /// </summary>
        public string RId { get; set; }
        /// <summary>
        /// 用户机构id
        /// </summary>
        public long InstutionId { get; set; }
        /// <summary>
        /// 用户部门id
        /// </summary>
        public long? DepId { get; set; }
        /// <summary>
        /// 用户所属项目id
        /// </summary>
        public long? PjectId { get; set; }
        /// <summary>
        /// 授权可查看的字段 ,拼接串
        /// </summary>
        public string? AuthorityColumns { get; set; }
    }
}
