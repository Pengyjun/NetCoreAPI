using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 通用类字典数据
    /// </summary>
    [SugarTable("t_common", IsDisabledDelete = true)]
    public class Common : BaseEntity<long>
    {

    }
}
