using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent
{
    /// <summary>
    /// 大洲  反显
    /// </summary>
    public class CountryContinentSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 大洲代码：大洲代码
        /// </summary>
        public string ContinentCode { get; set; }
        /// <summary>
        /// 大洲名称：大洲名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区域描述：区域描述
        /// </summary>
        public string RegionalDescr { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string State { get; set; }
    }
    /// <summary>
    /// 大洲详情
    /// </summary>
    public class CountryContinentDetailsDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 大洲代码：大洲代码
        /// </summary>
        [DisplayName("大洲代码")]
        public string ContinentCode { get; set; }
        /// <summary>
        /// 大洲名称：大洲名称
        /// </summary>
        [DisplayName("大洲名称")]
        public string Name { get; set; }
        /// <summary>
        /// 区域代码：大洲所属区域代码
        /// </summary>
        [DisplayName("区域代码")]
        public string AreaCode { get; set; }
        /// <summary>
        /// 区域描述：区域描述
        /// </summary>
        [DisplayName("区域描述")]
        public string RegionalDescr { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [DisplayName("版本")]
        public string Version { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [DisplayName("状态")]
        public string State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [DisplayName("是否删除")]
        public string DataIdentifier { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 大洲  接收
    /// </summary>
    public class CountryContinentReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public   long? Id { get; set; }
        /// <summary>
        /// 大洲代码：大洲代码
        /// </summary>
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 大洲名称：大洲名称
        /// </summary>
        public string? ZCONTINENTNAME { get; set; }
        /// <summary>
        /// 区域代码：大洲所属区域代码
        /// </summary>
        public string? ZAREACODE { get; set; }
        /// <summary>
        /// 区域描述：区域描述
        /// </summary>
        public string? ZAREANAME { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public CountryContinentModels? ZLANG_LIST { get; set; }
    }
}
