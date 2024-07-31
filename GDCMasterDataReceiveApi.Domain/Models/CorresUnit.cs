using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 往来单位主数据
    /// </summary>
    [SugarTable("t_corresunit", IsDisabledDelete = true)]
    public class CorresUnit : BaseEntity<long>
    {

    }
}
