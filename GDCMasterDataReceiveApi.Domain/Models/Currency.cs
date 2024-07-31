using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 币种
    /// </summary>
    [SugarTable("t_currency", IsDisabledDelete = true)]
    public class Currency : BaseEntity<long>
    {
    }
}
