using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 科研项目
    /// </summary>
    [SugarTable("t_scientificnoproject", IsDisabledDelete = true)]
    public class ScientifiCNoProject : BaseEntity<long>
    {

    }
}
