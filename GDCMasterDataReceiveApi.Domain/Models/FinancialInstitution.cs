﻿using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 金融机构
    /// </summary>
    [SugarTable("t_financialinstitution", IsDisabledDelete = true)]
    public class FinancialInstitution : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 金融机构主数据编码:金融机构的唯一编码标识
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "MDCode")]
        public string? ZFINC { get; set; }
        /// <summary>
        /// 总行编号:开户行总行的唯一编码，若金融机构类型为“银行业存款类金融机构”则该字段为必填项
        /// </summary>
        [SugarColumn(Length = 30, ColumnName = "No")]
        public string? ZBANK { get; set; }
        /// <summary>
        /// 银行联行号:中国人民银行根据支付系统行名行号的编码规则为银行机构和中国人民银行分支机构编制的用于账户管理系统识别其身份的唯一标识
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "BankNo")]
        public string? ZBANKN { get; set; }
        /// <summary>
        /// 总行名称:开户行总行的名称，若金融机构类型为“银行业存款类金融机构”则该字段为必填项
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Name")]
        public string? ZBANKNAME { get; set; }
        /// <summary>
        /// 国家/地区:该金融机构所在的国家/地区
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "Country")]
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 省:该金融机构所在的省份/直辖市/自治区/特别行政区，境内金融机构必
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "Province")]
        public string? ZPROVINCE { get; set; }
        /// <summary>
        /// 市 :该金融机构所在的市/地区/自治州/盟，境内金融机构必填
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "City")]
        public string? ZCITY { get; set; }
        /// <summary>
        /// 县 :该金融机构所在的县（自治县、县级市、旗、自治旗、市辖区、林区、特区），境内金融机构必填
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "County")]
        public string? ZCOUNTY { get; set; }
        /// <summary>
        /// 境内金融机构类型:该金融机构所属类型，境内金融机构必填
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "TypesOfOrg")]
        public string? ZDFITYPE { get; set; }
        /// <summary>
        /// 金融机构名称:该金融机构的全称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "NameOfOrg")]
        public string ZFINAME { get; set; }
        /// <summary>
        /// 英文名称:该金融机构的英文名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "EnglishName")]
        public string? ZFINAME_E { get; set; }
        /// <summary>
        /// 境外金融机构类型:该金融机构所属类型，境外金融机构必填
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "TypesOfAbroadOrg")]
        public string? ZOFITYPE { get; set; }
        /// <summary>
        /// 机构主数据编码:行政组织编码，集团内的金融机构必填
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "MDCodeofOrg")]
        public string? ZORG { get; set; }
        /// <summary>
        /// swift code:银行国际代码，境外金融机构必填
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "SwiftCode")]
        public string? ZSWIFTCOD { get; set; }
        /// <summary>
        /// 统一社会信用代码:该金融机构的统一社会信用代码
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "RegistrationNo")]
        public string? ZUSCC { get; set; }
        /// <summary>
        /// 状态:数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 18, ColumnName = "State")]
        public string? ZDATSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 最后修改二级单位 :记录数据最后修改所属二级单位
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "UnitSec")]
        public string? ZFIN2NDORG { get; set; }
        /// <summary>
        /// 提交时间：该金融机构最后一次修时间
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal", Length = 15, ColumnName = "SubmitTime")]
        public decimal? ZFZCHAT { get; set; }
        /// <summary>
        /// 提交人：该金融机构最后一次修改人
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "SubmitBy")]
        public string? ZFZCHBY { get; set; }
    }
}
