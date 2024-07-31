using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交区域总部
    /// </summary>
    [SugarTable("t_regional", IsDisabledDelete = true)]
    public class Regional : BaseEntity<long>
    {

    }
}
