using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 委托关系
    /// </summary>
    [SugarTable("t_relationalcontracts", IsDisabledDelete = true)]
    public class RelationalContracts : BaseEntity<long>
    {

    }
}
