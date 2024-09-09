using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 用户状态
    /// </summary>
    [SugarTable("t_userstatus", IsDisabledDelete = true)]
    public class UserStatus : BaseEntity<long>
    {
        /// <summary>
        /// 一级编码
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? OneCode { get; set; }
        /// <summary>
        /// 一级编码名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? OneName { get; set; }
        /// <summary>
        /// 二级编码
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? SecCode { get; set; }
        /// <summary>
        /// 二级编码名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? SecName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Remark { get; set; }
    }
}
