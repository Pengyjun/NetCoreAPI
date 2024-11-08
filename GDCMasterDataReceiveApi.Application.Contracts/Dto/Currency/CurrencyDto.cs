using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency
{

    #region 后台管理使用
    /// <summary>
    /// 币种 反显
    /// </summary>
    public class CurrencySearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 数字代码:货币数字代码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 货币名称:货币的中文描述
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 字母代码:货币字母代码
        /// </summary>
        public string? LetterCode { get; set; }
        /// <summary>
        /// 货币标准名称:货币标准名称
        /// </summary>
        public string? StandardName { get; set; }
    }
    /// <summary>
    /// 币种详情
    /// </summary>
    public class CurrencyDetailsDto
    {
        public string? Id { get; set; }
        /// <summary>
        /// 数字代码:货币数字代码
        /// </summary>
        [Description("ZCURRENCYCODE")]
        [DisplayName("数字代码")]
        public string? Code { get; set; }
        /// <summary>
        /// 货币名称:货币的中文描述
        /// </summary>
        [Description("ZCURRENCYNAME")]
        [DisplayName("货币名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 字母代码:货币字母代码
        /// </summary>
        [Description("ZCURRENCYALPHABET")]
        [DisplayName("字母代码")]
        public string? LetterCode { get; set; }
        /// <summary>
        /// 货币标准名称:货币标准名称
        /// </summary>
        [Description("STANDARDNAMEE")]
        [DisplayName("货币标准名称")]
        public string? StandardName { get; set; }
        /// <summary>
        /// 备注:说明备注
        /// </summary>
        [Description("ZREMARKS")]
        [DisplayName("备注")]
        public string? Remark { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [Description("ZVERSION")]
        [DisplayName("版本")]
        public string? Version { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [Description("ZSTATE")]
        [DisplayName("状态")]
        public string? State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [Description("ZDELETE")]
        [DisplayName("是否删除")]
        public string? DataIdentifier { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    #endregion



    #region 主数据接收币种请求DTO

    /// <summary>
    /// 币种 接收
    /// </summary>
    public class CurrencyReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 数字代码:货币数字代码
        /// </summary>
        public string? ZCURRENCYCODE { get; set; }
        /// <summary>
        /// 货币名称:货币的中文描述
        /// </summary>
        public string? ZCURRENCYNAME { get; set; }
        /// <summary>
        /// 字母代码:货币字母代码
        /// </summary>
        public string? ZCURRENCYALPHABET { get; set; }
        /// <summary>
        /// 货币标准名称:货币标准名称
        /// </summary>
        public string? STANDARDNAMEE { get; set; }
        /// <summary>
        /// 备注:说明备注
        /// </summary>
        public string? ZREMARKS { get; set; }
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
        ///// <summary>
        ///// 多语言描述表类型
        ///// </summary>
        public CurrencyLanguageRequestDto? ZLANG_LIST { get; set; }
    }
    #endregion
}
