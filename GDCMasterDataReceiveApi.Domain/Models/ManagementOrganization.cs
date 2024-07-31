using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 生产经营管理组织
    /// </summary>
    [SugarTable("t_managementorganization", IsDisabledDelete = true)]
    public class ManagementOrganization : BaseEntity<long>
    {

    }
}
