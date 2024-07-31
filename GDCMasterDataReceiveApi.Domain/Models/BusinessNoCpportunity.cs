using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 商机项目(不含境外商机项目)
    /// </summary>
    [SugarTable("t_businessnocpportunity", IsDisabledDelete = true)]
    public class BusinessNoCpportunity : BaseEntity<long>
    {

    }
}
