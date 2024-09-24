using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 机构业务类型
    /// </summary>
    [SugarTable("t_institutionbusinesstype", IsDisabledDelete = true)]
    public class InstitutionBusinessType : BaseEntity<long>
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string? Code {  get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name {  get; set; }
    }
}
