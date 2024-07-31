using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 多组织-核算机构
    /// </summary>
    [SugarTable("t_accountingorganization", IsDisabledDelete = true)]
    public class AccountingOrganization : BaseEntity<long>
    {

    }
}
