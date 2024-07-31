using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 语言语种
    /// </summary>
    [SugarTable("t_language", IsDisabledDelete = true)]
    public class Language : BaseEntity<long>
    {

    }
}
