using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 机构主数据
    /// </summary>

    [SugarTable("t_institution", IsDisabledDelete = true)]
    public class Institution : BaseEntity<long>
    {

    }
}
