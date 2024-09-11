using GDCMasterDataReceiveApi.Domain.OtherModels;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType
{
    /// <summary>
    /// 发票类型 反显
    /// </summary>
    public class InvoiceTypeDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 发票类型代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 发票类型名称
        /// </summary>
        public string Name { get; set; }
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
    /// 发票类型 接收
    /// </summary>
    public class InvoiceTypeItem
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 发票类型代码
        /// </summary>
        public string ZINVTCODE { get; set; }
        /// <summary>
        /// 发票类型名称
        /// </summary>
        public string ZINVTNAME { get; set; }
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
        public InvoiceLang? ZLANG_LIST { get; set; }
    }
}
