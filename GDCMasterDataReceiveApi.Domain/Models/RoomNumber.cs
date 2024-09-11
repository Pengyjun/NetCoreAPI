using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 房号
    /// </summary>
    [SugarTable("t_roomnumber", IsDisabledDelete = true)]
    public class RoomNumber : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 楼栋编码:该房号所属的楼栋编码
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "BuildCode")]
        public string? ZBLDG { get; set; }
        /// <summary>
        /// 业态信息:该房号的业态，单选，在楼栋业态范围内中选择；通用类字典102接口下发
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "BFormat")]
        public string? ZFORMAT { get; set; }
        /// <summary>
        /// 项目主数据编码 :该房号所属的分期（子项目）编码，如无分期（子项目）可填主项目的编码，与所属楼栋的项目编码一致
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "PjectMDCode")]
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 所属单元:房号所属单元，如没有可默认填0
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "UnitOfBuild")]
        public string? ZRE_UNIT { get; set; }
        /// <summary>
        /// 房号编码:由4位数字编码组成，范围为0001～9999
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "Code")]
        public string? ZROOM { get; set; }
        /// <summary>
        /// 房号名称:所属楼栋下房号名称与单元共同组成房号的唯一标识，同一个项目同一楼栋同一单元，房号名称不允许重复(0:无效,1:有效)
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "Name")]
        public string? ZROOM_NAME { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "SourceSystem")]
        public string? ZSYSTEM { get; set; }
    }
}
