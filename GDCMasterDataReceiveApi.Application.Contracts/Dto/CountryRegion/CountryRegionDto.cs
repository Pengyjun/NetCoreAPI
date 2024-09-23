using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion
{
    #region 后台管理使用
    /// <summary>
    /// 国家地区列表 反显
    /// </summary>
    public class CountryRegionSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
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
        /// 状态:1是已启用，0是已停用
        /// </summary>
        public string State { get; set; }
    }
    /// <summary>
    /// 国家地区详情
    /// </summary>
    public class CountryRegionDetailsDto
    {
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
        [ExcelIgnore]
        public string Id { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        [ExcelColumnName("国家地区代码")]
        public string Country { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        [ExcelColumnName("中文名称")]
        public string Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [ExcelColumnName("英文名称")]
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 国标三字符代码
        /// </summary>
        [ExcelColumnName("国标三字符代码")]
        public string? NationalCode { get; set; }
        /// <summary>
        /// 国标数字代码
        /// </summary>
        [ExcelColumnName("国标数字代码")]
        public string? DigitCode { get; set; }
        /// <summary>
        /// 大洲代码
        /// </summary>
        [ExcelColumnName("大洲代码")]
        public string? ContinentCode { get; set; }
        /// <summary>
        /// 中交区域中心代码
        /// </summary>
        [ExcelColumnName("中交区域中心代码")]
        public string? CCCCCenterCode { get; set; }
        /// <summary>
        /// 版本号:数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [ExcelIgnore]
        public int Version { get; set; }
        /// <summary>
        /// 状态:1是已启用，0是已停用
        /// </summary>
        [ExcelIgnore]
        public string State { get; set; }
        /// <summary>
        /// 是否删除数据是否有效的标识:    有效：1无效：0
        /// </summary>
        [ExcelIgnore]
        public string DataIdentifier { get; set; }
        /// <summary>
        /// 一带一路(国资委):0-否，1-是
        /// </summary>
        [ExcelIgnore]
        public string RoadGuoZiW { get; set; }
        /// <summary>
        /// 一带一路(海外):0-否，1-是
        /// </summary>
        [ExcelIgnore]
        public string RoadHaiW { get; set; }
        /// <summary>
        /// 一带一路(共建):0-否，1-是
        /// </summary>
        [ExcelIgnore]
        public string RoadGongJ { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        [ExcelColumnName("区域代码")]
        public string AreaCode { get; set; }
    }
    #endregion

    #region 数据接收 国家地区
    /// <summary>
    /// 国家地区 接收
    /// </summary>
    public class CountryRegionReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        public string? ZCOUNTRYCODE { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string? ZCOUNTRYNAME { get; set; }
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
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除数据是否有效的标识:    有效：1无效：0
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 一带一路(国资委):0-否，1-是
        /// </summary>
        public string? ZBRGZW { get; set; }
        /// <summary>
        /// 一带一路(海外):0-否，1-是
        /// </summary>
        public string? ZBRHW { get; set; }
        /// <summary>
        /// 一带一路(共建):0-否，1-是
        /// </summary>
        public string? ZBRGJ { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string? ZAREACODE { get; set; }

        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public CountryRegionModels? ZLANG_LIST { get; set; }
    }
    #endregion
}
