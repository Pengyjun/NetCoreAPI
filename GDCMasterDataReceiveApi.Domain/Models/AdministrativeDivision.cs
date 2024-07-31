using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 境内行政区划
    /// </summary>
    [SugarTable("t_administrativedivision", IsDisabledDelete = true)]
    public class AdministrativeDivision : BaseEntity<long>
    {

    }
}
