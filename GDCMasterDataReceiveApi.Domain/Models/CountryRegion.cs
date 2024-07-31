using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 国家地区
    /// </summary>
    [SugarTable("t_countryregion", IsDisabledDelete = true)]
    public class CountryRegion : BaseEntity<long>
    {

    }
}
