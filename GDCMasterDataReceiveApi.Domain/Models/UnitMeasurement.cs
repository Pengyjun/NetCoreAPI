using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 常用计量单位
    /// </summary>
    [SugarTable("t_unitmeasurement", IsDisabledDelete = true)]
    public class UnitMeasurement : BaseEntity<long>
    {

    }
}
