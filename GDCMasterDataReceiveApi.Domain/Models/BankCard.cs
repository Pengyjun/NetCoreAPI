using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 银行账号
    /// </summary>
    [SugarTable("t_bankcard", IsDisabledDelete = true)]
    public class BankCard : BaseEntity<long>
    {
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "MainDataCodeOfTheContactUnit")]
        public string ZBP { get; set; }
        /// <summary>
        /// 银行账号主键：银行账号主键
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "BankAccountPrimaryKey")]
        public string ZBANK { get; set; }
        /// <summary>
        /// 账户名称：填报银行账户的全称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "NameOfAccount")]
        public string ZKOINH { get; set; }
        /// <summary>
        /// 银行账号/IBAN：填报银行账号/IBAN，该字段作为银行账户的唯一标识
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "BankAccount")]
        public string ZBANKN { get; set; }
        /// <summary>
        /// 金融机构编码：填报具体开户网点，引用金融机构主数据
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "FinancialInstitutionCode")]
        public string ZFINC { get; set; }
        /// <summary>
        /// 金融机构名称：填报具体开户网点，引用金融机构主数据
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "NameOfFinancialInstitution")]
        public string ZFINAME { get; set; }
        /// <summary>
        /// 账户状态：银行账户所处状态，包括正常、冻结、其他，默认正常。
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "AccountStatus")]
        public string ZBANKSTA { get; set; }
        /// <summary>
        /// 账户币种：按账户实际币种填报。默认人民币。可多值以英文逗号隔开。
        /// </summary>
        [SugarColumn(Length = 300, ColumnName = "AccountCurrency")]
        public string ZCURR { get; set; }
        /// <summary>
        /// 国家/地区：金融机构对应的国家/地区
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "Country")]
        public string ZZCOUNTR2 { get; set; }
        /// <summary>
        /// 省：金融机构对应的省，条件必填，国家/地区为中国的必填
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "Province")]
        public string ZPROVINC2 { get; set; }
        /// <summary>
        /// 市：金融机构对应的市，条件必填，国家/地区为中国的必填
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "City")]
        public string ZCITY2 { get; set; }
        /// <summary>
        /// 县：2023.2改为非必填
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "County")]
        public string? ZCOUNTY2 { get; set; }
    }
}
