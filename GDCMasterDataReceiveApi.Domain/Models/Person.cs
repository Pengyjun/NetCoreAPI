using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 人员主数据
    /// </summary>
    [SugarTable("t_person", IsDisabledDelete = true)]
    public class Person : BaseEntity<long>
    {

    }
}
