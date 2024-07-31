using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备分类编码
    /// </summary>
    [SugarTable("t_deviceclasscode", IsDisabledDelete = true)]
    public class DeviceClassCode : BaseEntity<long>
    {

    }
}
