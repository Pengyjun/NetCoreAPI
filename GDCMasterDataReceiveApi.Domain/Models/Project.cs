using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 项目类
    /// </summary>
    [SugarTable("t_Project", IsDisabledDelete = true)]
    public class Project : BaseEntity<long>
    {

    }
}
