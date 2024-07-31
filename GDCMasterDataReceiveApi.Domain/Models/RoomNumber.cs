using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 房号
    /// </summary>
    [SugarTable("t_roomnumber", IsDisabledDelete = true)]
    public class RoomNumber:BaseEntity<long>
    {

    }
}
