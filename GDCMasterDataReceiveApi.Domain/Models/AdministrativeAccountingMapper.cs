using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 行政机构和核算机构映射关系
    /// </summary>
    [SugarTable("t_administrativeaccountingmapper", IsDisabledDelete = true)]
    public class AdministrativeAccountingMapper:BaseEntity<long>
    {

    }
}
