using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 银行账号
    /// </summary>
    [SugarTable("t_bankcard", IsDisabledDelete = true)]
    public class BankCard : BaseEntity<long>
    {

    }
}
