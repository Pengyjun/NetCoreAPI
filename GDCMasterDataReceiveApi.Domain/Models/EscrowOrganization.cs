using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 多组织-税务代管组织(行政)
    /// </summary>
    [SugarTable("t_escroworganization", IsDisabledDelete = true)]
    public class EscrowOrganization : BaseEntity<long>
    {

    }
}
