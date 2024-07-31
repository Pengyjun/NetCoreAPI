using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 大洲
    /// </summary>
    [SugarTable("t_countrycontinent", IsDisabledDelete = true)]
    public class CountryContinent : BaseEntity<long>
    {

    }
}
