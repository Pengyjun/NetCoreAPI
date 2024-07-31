using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 多组织-行政组织
    /// </summary>
    [SugarTable("t_administrativeorganization", IsDisabledDelete = true)]
    public class AdministrativeOrganization : BaseEntity<long>
    {

    }
}
