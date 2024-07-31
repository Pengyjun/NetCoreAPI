using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交区域中心
    /// </summary>
    [SugarTable("t_regionalcenter", IsDisabledDelete = true)]
    public class RegionalCenter : BaseEntity<long>
    {

    }
}
