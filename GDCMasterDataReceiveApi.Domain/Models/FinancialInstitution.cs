using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 金融机构
    /// </summary>
    [SugarTable("t_financialinstitution", IsDisabledDelete = true)]
    public class FinancialInstitution : BaseEntity<long>
    {

    }
}
