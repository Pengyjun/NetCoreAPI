using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 机构主数据
    /// </summary>
    [SugarTable("t_institution", IsDisabledDelete = true)]
    public class Institution : BaseEntity<long>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? OID { get; set; }

        /// <summary>
        /// 机构业务类型 见7.2.3 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? BIZTYPE { get; set; }
        /// <summary>
        /// 关联机构id
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? O2BID { get; set; }
        /// <summary>
        /// 上级机构id
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? POID { get; set; }
        /// <summary>
        /// 分组时的上级机构ID
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? GPOID { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ORULE { get; set; }
        /// <summary>
        /// 分组机构规则码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? GRULE { get; set; }
        /// <summary>
        /// 机构属性,见7.2.2 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TYPE { get; set; }
        /// <summary>
        /// 机构子属性,见7.2.2 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TYPEEXT { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? MRUT { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? SNO { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? NAME { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? SHORTNAME { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>  
        [SugarColumn(Length = 128)]
        public string? OCODE { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? COID { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? CROSSORGAN { get; set; }
        /// <summary>
        /// 分组所属的实体机构ID
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? GOID { get; set; }
        /// <summary>
        /// 机构状态, 见7.2.1 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? STATUS { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? GRADE { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? OPER { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? NOTE { get; set; }
        /// <summary>
        /// 临时机构名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TEMORGANNAME { get; set; }
        /// <summary>
        /// 机构所在地, 见7.2.7 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ORGPROVINCE { get; set; }
        /// <summary>
        /// 国家名称, 见7.2.6 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? CAREA { get; set; }
        /// <summary>
        ///   地域属性, 见7.2.5 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TERRITORYPRO { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? BIZDOMAIN { get; set; }
        /// <summary>
        /// 企业分类, 见7.2.4 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ENTCLASS { get; set; }
        /// <summary>
        /// 机构层级(没有用过)
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ORGGRADE { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? PROJECTSCALE { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? PROJECTMANTYPE { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? PROJECTTYPE { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? STARTDATE { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ORGANEMP { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ORGANGRADE { get; set; }
        /// <summary>
        /// 独立授权
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ROWN { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ROID { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        public string? GLOBAL_SNO { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? QYGRADE { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? REGISTERCODE { get; set; }
        /// <summary>
        ///版本号
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? VERSION { get; set; }
        /// <summary>
        /// 持股情况，见7.2.9 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? SHAREHOLDINGS { get; set; }
        /// <summary>
        /// 是否独立核算，见7.2.8 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? IS_INDEPENDENT { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ENGLISHNAME { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ENGLISHSHORTNAME { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? MDM_CODE { get; set; }
    }
}
