using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution
{
    /// <summary>
    /// 机构主数据  反显
    /// </summary>
    public class InstitutionDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 机构 ID
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 企业分类
        /// 0 外经企业 1 基建企业 2 疏浚企业 3 设计企业 4 装备制造企业 5 投资企业 6 房地产企业 7 贸易企业 8 金融企业 9 其他
        /// </summary>
        public string? EntClass { get; set; }
        /// <summary>
        /// 上级机构 id
        /// </summary>
        public string? POid { get; set; }
        /// <summary>
        /// 分组oid
        /// </summary>
        public string? GpOid { get; set; }
        /// <summary>
        /// "机构状态，类型编码"
        /// 1运营 2筹备 3停用 4撤销
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 机构编码"
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? Orule { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sno { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        public string? EnglishName { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        public string? EnglishShortName { get; set; }
        /// <summary>
        /// 机构子集
        /// </summary>
        public List<InstitutionDto>? Children { get; set; }
    }
    /// <summary>
    /// 机构详情dto
    /// </summary>
    public class InstitutionDetatilsDto
    {
        /// <summary>
        /// ids（首页下钻）
        /// </summary>
        public List<string>? Ids { get; set; }
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 机构 ID
        /// </summary>
        [Description("OID")]
        [DisplayName("机构Oid")]
        public string? Oid { get; set; }
        /// <summary>
        /// 企业分类
        /// 0 外经企业 1 基建企业 2 疏浚企业 3 设计企业 4 装备制造企业 5 投资企业 6 房地产企业 7 贸易企业 8 金融企业 9 其他
        /// </summary>
        [Description("ENTCLASS")]
        [DisplayName("企业分类")]
        public string? EntClass { get; set; }
        /// <summary>
        /// 上级机构 id
        /// </summary>
        [Description("POID")]
        [DisplayName("上级机构id")]
        public string? POid { get; set; }
        /// <summary>
        /// "机构状态，类型编码"
        /// 1运营 2筹备 3停用 4撤销
        /// </summary>
        [Description("STATUS")]
        [DisplayName("机构状态,类型编码")]
        public string? Status { get; set; }
        /// <summary>
        /// 机构编码"
        /// </summary>
        [Description("OCODE")]
        [DisplayName("机构编码")]
        public string? Code { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        [Description("NAME")]
        [DisplayName("机构全称")]
        public string? Name { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [Description("ORULE")]
        [DisplayName("机构规则码")]
        public string? Orule { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        [Description("SHORTNAME")]
        [DisplayName("机构简称")]
        public string? ShortName { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        [Description("ENGLISHNAME")]
        [DisplayName("英文全称")]
        public string? EnglishName { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        [Description("ENGLISHSHORTNAME")]
        [DisplayName("英文简称")]
        public string? EnglishShortName { get; set; }
        /// <summary>
        /// 机构业务类型 见7.2.3 字典
        /// </summary>
        [Description("BIZTYPE")]
        [DisplayName("机构业务类型")]
        public string? BizType { get; set; }
        /// <summary>
        /// 关联机构id
        /// </summary>
        [Description("O2BID")]
        [DisplayName("关联机构id")]
        public string? OSecBid { get; set; }
        /// <summary>
        /// 分组时的上级机构ID
        /// </summary>
        [Description("GPOID")]
        [DisplayName("分组时的上级机构ID")]
        public string? Gpoid { get; set; }
        /// <summary>
        /// 机构属性,见7.2.2 字典
        /// </summary>
        [Description("TYPE")]
        [DisplayName("机构属性")]
        public string? Type { get; set; }
        /// <summary>
        /// 机构子属性,见7.2.2 字典
        /// </summary>
        [Description("TYPEEXT")]
        [DisplayName("机构子属性")]
        public string? TypeExt { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [Description("MRUT")]
        [DisplayName("最近更新时间")]
        public string? Mrut { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [Description("SNO")]
        [DisplayName("序号")]
        public string? Sno { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
        [Description("COID")]
        [DisplayName("隶属单位")]
        public string? Coid { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        [Description("CROSSORGAN")]
        [DisplayName("拥有兼管职能")]
        public string? Crossorgan { get; set; }
        /// <summary>
        /// 分组所属的实体机构ID
        /// </summary>
        [Description("GOID")]
        [DisplayName("分组所属的实体机构ID")]
        public string? Goid { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [Description("GRADE")]
        [DisplayName("层级")]
        public string? Grade { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        [Description("OPER")]
        [DisplayName("操作员")]
        public string? Oper { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("NOTE")]
        [DisplayName("备注")]
        public string? Note { get; set; }
        /// <summary>
        /// 临时机构名称
        /// </summary>
        [Description("TEMORGANNAME")]
        [DisplayName("临时机构名称")]
        public string? TemorganName { get; set; }
        /// <summary>
        /// 机构所在地, 见7.2.7 字典
        /// </summary>
        [Description("ORGPROVINCE")]
        [DisplayName("机构所在地")]
        public string? OrgProvince { get; set; }
        /// <summary>
        /// 国家名称, 见7.2.6 字典
        /// </summary>
        [Description("CAREA")]
        [DisplayName("国家名称")]
        public string? Carea { get; set; }
        /// <summary>
        ///   地域属性, 见7.2.5 字典
        /// </summary>
        [Description("TERRITORYPRO")]
        [DisplayName("地域属性")]
        public string? TerritoryPro { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        [Description("BIZDOMAIN")]
        [DisplayName("业务领域")]
        public string? BizDomain { get; set; }
        /// <summary>
        /// 机构层级(没有用过)
        /// </summary>
        [Description("ORGGRADE")]
        [DisplayName("机构层级(没有用过)")]
        public string? OrgGrade { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        [Description("PROJECTSCALE")]
        [DisplayName("项目规模")]
        public string? ProjectScale { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        [Description("PROJECTMANTYPE")]
        [DisplayName("项目管理类型")]
        public string? ProjectManType { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [Description("PROJECTTYPE")]
        [DisplayName("项目类型")]
        public string? ProjectType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("STARTDATE")]
        [DisplayName("开始时间")]
        public string? StartDate { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [Description("ORGANEMP")]
        [DisplayName("负责人")]
        public string? Organemp { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        [Description("ORGANGRADE")]
        [DisplayName("管理层级")]
        public string? OrganGrade { get; set; }
        /// <summary>
        /// 独立授权
        /// </summary>
        [Description("ROWN")]
        [DisplayName("独立授权")]
        public string? Rown { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        [Description("ROID")]
        [DisplayName("授权机构")]
        public string? RoId { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        [Description("GLOBAL_SNO")]
        [DisplayName("DisplayName")]
        public string? GlobalSno { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        [Description("QYGRADE")]
        [DisplayName("股权层级")]
        public string? QyGrade { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        [Description("REGISTERCODE")]
        [DisplayName("注册号/统一社会信用代码")]
        public string? RegisterCode { get; set; }
        /// <summary>
        ///版本号
        /// </summary>
        [Description("VERSION")]
        [DisplayName("版本号")]
        public string? Version { get; set; }
        /// <summary>
        /// 持股情况，见7.2.9 字典
        /// </summary>
        [Description("SHAREHOLDINGS")]
        [DisplayName("持股情况")]
        public string? ShareHoldings { get; set; }
        /// <summary>
        /// 是否独立核算，见7.2.8 字典
        /// </summary>
        [Description("IS_INDEPENDENT")]
        [DisplayName("是否独立核算")]
        public string? IsIndependent { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [Description("MDM_CODE")]
        [DisplayName("机构主数据编码")]
        public string? MdmCode { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 机构  新版
    /// </summary>
    public class InstitutionResponseDto
    {
        /// <summary>
        /// 机构oid
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 机构上级id
        /// </summary>
        public string? Gpoid { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string? Sno { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string OCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<InstitutionResponseDto>? Children { get; set; }
    }

    public class InstitutionConvertDto
    {
        public string? OID { get; set; }
        public string? GPOID { get; set; }
        public string? SHORTNAME { get; set; }
        public int? IsDelete { get; set; }
        public string? SNO { get; set; }
        public long Id { get; set; }
        public string? GRULE { get; set; }
        public string? NAME { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string OCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
    }

    ///// <summary>
    ///// 外部系统添加选择机构时  使用 
    ///// </summary>
    //public class InstitutionTreeResponseDto:TreeNode<InstitutionTreeResponseDto>{

      


    //}
}
