using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH机构
    /// </summary>
    [SugarTable("t_dh_organzation", IsDisabledDelete = true)]
    public class DHOrganzation : BaseEntity<long>
    {
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? OID { get; set; }
        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        public string? POID { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string? OCODE { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? SHORTNAME { get; set; }
        /// <summary>
        /// 机构状态
        /// </summary>
        public string? STATUS { get; set; }
        /// <summary>
        /// 机构属性
        /// </summary>
        public string? TYPE { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        public string? TYPEEXT { get; set; }
        /// <summary>
        /// 企业分类
        /// </summary>
        public string? ENTCLASS { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string? CAREA { get; set; }
        /// <summary>
        /// 机构所在地
        /// </summary>
        public string? ORGPROVINCE { get; set; }
        /// <summary>
        /// 地域属性
        /// </summary>
        public string? TERRITORYPRO { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        public string? REGISTERCODE { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string? ADDRESS { get; set; }
        /// <summary>
        /// 机构业务类型
        /// </summary>
        public string? BIZTYPE { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        public string? BIZDOMAIN { get; set; }
        /// <summary>
        /// 关联机构id
        /// </summary>
        public string? O2BID { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
        public string? COID { get; set; }
        /// <summary>
        /// 分组所属实体机构ID
        /// </summary>
        public string? GOID { get; set; }
        /// <summary>
        /// 分组时的上级机构ID
        /// </summary>
        public string? GPOID { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? GRADE { get; set; }
        /// <summary>
        /// 机构层级
        /// </summary>
        public string? ORGGRADE { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? ORULE { get; set; }
        /// <summary>
        /// 分组机构规则码
        /// </summary>
        public string? GRULE { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string? SNO { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        public string? CROSSORGAN { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime? MRUT { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string? OPER { get; set; }
        /// <summary>
        /// 临时机构名称
        /// </summary>
        public string? TEMORGANNAME { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        public string? PROJECTSCALE { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        public string? PROJECTMANTYPE { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? PROJECTTYPE { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? STARTDATE { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string? ORGANEMP { get; set; }
        /// <summary>
        /// 独立授权
        /// </summary>
        public string? ROWN { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        public string? ROID { get; set; }
        /// <summary>
        /// 全部机构大流水号
        /// </summary>
        public string? GLOBAL_SNO { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        public string? QYGRADE { get; set; }
        /// <summary>
        /// 持股情况
        /// </summary>
        public string? SHAREHOLDINGS { get; set; }
        /// <summary>
        /// 是否独立核算
        /// </summary>
        public string? IS_INDEPENDENT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? NOTE { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? VERSION { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? FZ2ndorg { get; set; }
        /// <summary>
        /// 状态 0:停用;1:启用
        /// </summary>
        public string? STATE { get; set; }
        /// <summary>
        /// 是否删除 0:已删除;1:未删除
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? DELETE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
