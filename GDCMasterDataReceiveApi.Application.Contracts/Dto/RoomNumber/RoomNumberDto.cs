namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber
{
    /// <summary>
    /// 房号 反显
    /// </summary>
    public class RoomNumberSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 楼栋编码:该房号所属的楼栋编码
        /// </summary>
        public string? BuildCode { get; set; }
        /// <summary>
        /// 业态信息:该房号的业态，单选，在楼栋业态范围内中选择；通用类字典102接口下发
        /// </summary>
        public string? BFormat { get; set; }
        /// <summary>
        /// 房号编码:由4位数字编码组成，范围为0001～9999
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 房号名称:所属楼栋下房号名称与单元共同组成房号的唯一标识，同一个项目同一楼栋同一单元，房号名称不允许重复(0:无效,1:有效)
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        public string? State { get; set; }
    }
    /// <summary>
    /// 房号明细
    /// </summary>
    public class RoomNumberDetailsDto
    {
        /// <summary>
        /// 楼栋编码:该房号所属的楼栋编码
        /// </summary>
        public string? BuildCode { get; set; }
        /// <summary>
        /// 业态信息:该房号的业态，单选，在楼栋业态范围内中选择；通用类字典102接口下发
        /// </summary>
        public string? BFormat { get; set; }
        /// <summary>
        /// 项目主数据编码 :该房号所属的分期（子项目）编码，如无分期（子项目）可填主项目的编码，与所属楼栋的项目编码一致
        /// </summary>
        public string? PjectMDCode { get; set; }
        /// <summary>
        /// 所属单元:房号所属单元，如没有可默认填0
        /// </summary>
        public string? UnitOfBuild { get; set; }
        /// <summary>
        /// 房号编码:由4位数字编码组成，范围为0001～9999
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 房号名称:所属楼栋下房号名称与单元共同组成房号的唯一标识，同一个项目同一楼栋同一单元，房号名称不允许重复(0:无效,1:有效)
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string? SourceSystem { get; set; }
    }
    /// <summary>
    /// 房号 接收
    /// </summary>
    public class RoomNumberItem
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 楼栋编码:该房号所属的楼栋编码
        /// </summary>
        public string? ZBLDG { get; set; }
        /// <summary>
        /// 业态信息:该房号的业态，单选，在楼栋业态范围内中选择；通用类字典102接口下发
        /// </summary>
        public string? ZFORMAT { get; set; }
        /// <summary>
        /// 项目主数据编码 :该房号所属的分期（子项目）编码，如无分期（子项目）可填主项目的编码，与所属楼栋的项目编码一致
        /// </summary>
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 所属单元:房号所属单元，如没有可默认填0
        /// </summary>
        public string? ZRE_UNIT { get; set; }
        /// <summary>
        /// 房号编码:由4位数字编码组成，范围为0001～9999
        /// </summary>
        public string? ZROOM { get; set; }
        /// <summary>
        /// 房号名称:所属楼栋下房号名称与单元共同组成房号的唯一标识，同一个项目同一楼栋同一单元，房号名称不允许重复(0:无效,1:有效)
        /// </summary>
        public string? ZROOM_NAME { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string? ZSYSTEM { get; set; }
    }
}
