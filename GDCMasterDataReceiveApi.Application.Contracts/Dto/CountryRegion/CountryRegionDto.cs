using GDCMasterDataReceiveApi.Domain.Models;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion
{
    /// <summary>
    /// 国家地区 反显
    /// </summary>
    public class CountryRegionDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 国标三字符代码
        /// </summary>
        public string? NationalCode { get; set; }
        /// <summary>
        /// 国标数字代码
        /// </summary>
        public string? DigitCode { get; set; }
        /// <summary>
        /// 大洲代码
        /// </summary>
        public string? ContinentCode { get; set; }
        /// <summary>
        /// 中交区域中心代码
        /// </summary>
        public string? CCCCCenterCode { get; set; }
        /// <summary>
        /// 版本号:数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 状态:1是已启用，0是已停用
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 是否删除数据是否有效的标识:    有效：1无效：0
        /// </summary>
        public string DataIdentifier { get; set; }
        /// <summary>
        /// 一带一路(国资委):0-否，1-是
        /// </summary>
        public string RoadGuoZiW { get; set; }
        /// <summary>
        /// 一带一路(海外):0-否，1-是
        /// </summary>
        public string RoadHaiW { get; set; }
        /// <summary>
        /// 一带一路(共建):0-否，1-是
        /// </summary>
        public string RoadGongJ { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public List<ZMDGTT_ZLANG>? ZLANG_LIST { get; set; }
    }
    /// <summary>
    /// 国家地区 接收
    /// </summary>
    public class CountryRegionReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        public string ZCOUNTRYCODE { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string ZCOUNTRYNAME { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string? ZCOUNTRYENAME { get; set; }
        /// <summary>
        /// 国标三字符代码
        /// </summary>
        public string? ZGBCHAR { get; set; }
        /// <summary>
        /// 国标数字代码
        /// </summary>
        public string? ZGBNUM { get; set; }
        /// <summary>
        /// 大洲代码
        /// </summary>
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 中交区域中心代码
        /// </summary>
        public string? ZCRCCODE { get; set; }
        /// <summary>
        /// 版本号:数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public int ZVERSION { get; set; }
        /// <summary>
        /// 状态:1是已启用，0是已停用
        /// </summary>
        public string ZSTATE { get; set; }
        /// <summary>
        /// 是否删除数据是否有效的标识:    有效：1无效：0
        /// </summary>
        public string ZDELETE { get; set; }
        /// <summary>
        /// 一带一路(国资委):0-否，1-是
        /// </summary>
        public string ZBRGZW { get; set; }
        /// <summary>
        /// 一带一路(海外):0-否，1-是
        /// </summary>
        public string ZBRHW { get; set; }
        /// <summary>
        /// 一带一路(共建):0-否，1-是
        /// </summary>
        public string ZBRGJ { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string ZAREACODE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public List<ZMDGTT_ZLANG>? ZLANG_LIST { get; set; }

    }
}
