using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 楼栋
    /// </summary>
    [SugarTable("t_loudong", IsDisabledDelete = true)]
    public class LouDong : BaseEntity<long>
    {

    }
}
