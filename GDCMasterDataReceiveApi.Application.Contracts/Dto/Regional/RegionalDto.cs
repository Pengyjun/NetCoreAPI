using GDCMasterDataReceiveApi.Domain.OtherModels;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional
{
    /// <summary>
    /// 中交区域总部 反显
    /// </summary>
    public class RegionalDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 中交区域总部代码:中交区域中心编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 中交区域总部描述:编码描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 简称:编码描述简称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区域范围:中交区域中心管辖范围
        /// </summary>
        public string AreaRange { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string DataIdentifier { get; set; }
    }
    /// <summary>
    /// 中交区域总部 接收
    /// </summary>
    public class RegionalReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 中交区域总部代码:中交区域中心编码
        /// </summary>
        public string ZCRHCODE { get; set; }
        /// <summary>
        /// 中交区域总部描述:编码描述
        /// </summary>
        public string ZCRHNAME { get; set; }
        /// <summary>
        /// 简称:编码描述简称
        /// </summary>
        public string ZCRHABBR { get; set; }
        /// <summary>
        /// 区域范围:中交区域中心管辖范围
        /// </summary>
        public string ZCRHSCOPE { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public List<ZMDGTT_ZLANG3>? ZLANG_LIST { get; set; }
    }
}
