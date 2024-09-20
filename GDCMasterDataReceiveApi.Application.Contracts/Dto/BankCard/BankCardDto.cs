using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.BankCard
{
    /// <summary>
    /// 银行账号 反显dto
    /// </summary>
    public class BankCardSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 账户名称：填报银行账户的全称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 银行账号/IBAN：填报银行账号/IBAN，该字段作为银行账户的唯一标识
        /// </summary>
        public string? BankAccount { get; set; }
        /// <summary>
        /// 账户状态：银行账户所处状态，包括正常、冻结、其他，默认正常。
        /// </summary>
        public string? AccountStatus { get; set; }
        /// <summary>
        /// 账户币种：按账户实际币种填报。默认人民币。可多值以英文逗号隔开。
        /// </summary>
        public string? AccountCurrency { get; set; }
    }
    /// <summary>
    /// 银行账号详情
    /// </summary>
    public class BankCardDetailsDto
    {
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        public string? DealUnitCode { get; set; }
        /// <summary>
        /// 银行账号主键：银行账号主键
        /// </summary>
        public string? BankNoPK { get; set; }
        /// <summary>
        /// 账户名称：填报银行账户的全称
        /// </summary>
        [ExcelColumnName("账户名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 银行账号/IBAN：填报银行账号/IBAN，该字段作为银行账户的唯一标识
        /// </summary>
        [ExcelColumnName("银行账号/IBAN")]
        public string? BankAccount { get; set; }
        /// <summary>
        /// 金融机构编码：填报具体开户网点，引用金融机构主数据
        /// </summary>
        [ExcelColumnName("金融机构编码")]
        public string? FinancialOrgCode { get; set; }
        /// <summary>
        /// 金融机构名称：填报具体开户网点，引用金融机构主数据
        /// </summary>
        [ExcelColumnName("金融机构名称")]
        public string? FinancialOrgName { get; set; }
        /// <summary>
        /// 账户状态：银行账户所处状态，包括正常、冻结、其他，默认正常。
        /// </summary>
        public string? AccountStatus { get; set; }
        /// <summary>
        /// 账户币种：按账户实际币种填报。默认人民币。可多值以英文逗号隔开。
        /// </summary>
        [ExcelColumnName("账户币种")]
        public string? AccountCurrency { get; set; }
        /// <summary>
        /// 国家/地区：金融机构对应的国家/地区
        /// </summary>
        [ExcelColumnName("国家/地区")]
        public string? Country { get; set; }
        /// <summary>
        /// 省：金融机构对应的省，条件必填，国家/地区为中国的必填
        /// </summary>
        [ExcelColumnName("省")]
        public string? Province { get; set; }
        /// <summary>
        /// 市：金融机构对应的市，条件必填，国家/地区为中国的必填
        /// </summary>
        [ExcelColumnName("市")]
        public string? City { get; set; }
        /// <summary>
        /// 县：2023.2改为非必填
        /// </summary>
        [ExcelColumnName("县")]
        public string? County { get; set; }
    }
    /// <summary>
    /// 银行账号 接收dto
    /// </summary>
    public class BankCardItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id {  get; set; }
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        public string? ZBP { get; set; }
        /// <summary>
        /// 银行账号主键：银行账号主键
        /// </summary>
        public string? ZBANK { get; set; }
        /// <summary>
        /// 账户名称：填报银行账户的全称
        /// </summary>
        public string? ZKOINH { get; set; }
        /// <summary>
        /// 银行账号/IBAN：填报银行账号/IBAN，该字段作为银行账户的唯一标识
        /// </summary>
        public string? ZBANKN { get; set; }
        /// <summary>
        /// 金融机构编码：填报具体开户网点，引用金融机构主数据
        /// </summary>
        public string? ZFINC { get; set; }
        /// <summary>
        /// 金融机构名称：填报具体开户网点，引用金融机构主数据
        /// </summary>
        public string? ZFINAME { get; set; }
        /// <summary>
        /// 账户状态：银行账户所处状态，包括正常、冻结、其他，默认正常。
        /// </summary>
        public string? ZBANKSTA { get; set; }
        /// <summary>
        /// 账户币种：按账户实际币种填报。默认人民币。可多值以英文逗号隔开。
        /// </summary>
        public string? ZCURR { get; set; }
        /// <summary>
        /// 国家/地区：金融机构对应的国家/地区
        /// </summary>
        public string? ZZCOUNTR2 { get; set; }
        /// <summary>
        /// 省：金融机构对应的省，条件必填，国家/地区为中国的必填
        /// </summary>
        public string? ZPROVINC2 { get; set; }
        /// <summary>
        /// 市：金融机构对应的市，条件必填，国家/地区为中国的必填
        /// </summary>
        public string? ZCITY2 { get; set; }
        /// <summary>
        /// 县：2023.2改为非必填
        /// </summary>
        public string? ZCOUNTY2 { get; set; }
    }
}
