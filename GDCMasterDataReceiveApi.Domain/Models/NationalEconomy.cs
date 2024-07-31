using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 国民经济行业分类
    /// </summary>
    [SugarTable("t_nationaleconomy", IsDisabledDelete = true)]
    public class NationalEconomy : BaseEntity<long>
    {

    }
}
