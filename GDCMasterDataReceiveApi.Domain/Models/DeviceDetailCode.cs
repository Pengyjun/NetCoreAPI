using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备明细编码
    /// </summary>
    [SugarTable("t_devicedetailcode", IsDisabledDelete = true)]
    public class DeviceDetailCode : BaseEntity<long>
    {

    }
}
