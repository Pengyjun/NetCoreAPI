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
        /// 机构 ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Oid")]
        public string OID { get; set; }
        /// <summary>
        /// "分组所属的实体机构 ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "GOid")]
        public string? GOID { get; set; }
        /// <summary>
        /// 企业分类
        /// 0 外经企业 1 基建企业 2 疏浚企业 3 设计企业 4 装备制造企业 5 投资企业 6 房地产企业 7 贸易企业 8 金融企业 9 其他
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "EntClass")]
        public string? ENTCLASS { get; set; }
        /// <summary>
        /// 隶属单位 ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "COid")]
        public string COID { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "Crossorgan")]
        public string CROSSORGAN { get; set; }
        /// <summary>
        /// 机构层级（没有用过）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OrgGrade")]
        public string? ORGGRADE { get; set; }
        /// <summary>
        /// 上级机构 id
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "POid")]
        public string POID { get; set; }
        /// <summary>
        /// "机构状态，类型编码"
        /// 1运营 2筹备 3停用 4撤销
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "Status")]
        public string STATUS { get; set; }
        /// <summary>
        /// "版本号
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Version")]
        public string? VERSION { get; set; }
        /// <summary>
        /// "创建时间
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "StartDate")]
        public string? STARTDATE { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(Length = 22, ColumnName = "Sno", ColumnDataType = "int")]
        public int SNO { get; set; }
        /// <summary>
        /// 机构所在地，类型编码
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "OrgProvince")]
        public string? ORGPROVINCE { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "RegisterCode")]
        public string? REGISTERCODE { get; set; }
        /// <summary>
        /// 项目类型"
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "PjectType")]
        public string? PROJECTTYPE { get; set; }
        /// <summary>
        /// 国家地区，类型编码
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Country")]
        public string? CAREA { get; set; }
        /// <summary>
        /// "临时机构名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "TemporaryOrgName")]
        public string? TEMORGANNAME { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OrgAemp")]
        public string? ORGANEMP { get; set; }
        /// <summary>
        /// "机构属性，类型编码
        /// C:企业 C10：常设公司 C20：项目公司 C30：特殊目的（SPV）公司 D30：分公司 C60：办事处/代表处 C70：模拟分公司 C80：非企业法人  C99：有限合伙企业 D：内部机构 D00：总部 D10：总部部门
        /// D20：事业部 D40：区域总部 D50：共享服务机构 D60：项目部/指挥部 D70：虚拟机构（行政管理） D80：财务虚拟核算单位 D90：生产单元 G：分组 G10：办事处 G20：子公司 G30：项目公司 G40：项目部
        /// G50：分公司 G80：虚拟分组下的 SPV公司 G99：虚拟机构 G120：直属机构 D80：直属机构 G85：非企业法人
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "Type")]
        public string TYPE { get; set; }
        /// <summary>
        /// 机构编码"
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Code")]
        public string? OCODE { get; set; }
        /// <summary>
        /// 分组时的上级机构 ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "GPOid")]
        public string GPOID { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "ManType")]
        public string? PROJECTMANTYPE { get; set; }
        /// <summary>
        /// 分组模式规则码
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Grule")]
        public string? GRULE { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        [SugarColumn(Length = 150, ColumnName = "Name")]
        public string NAME { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "BizDomain")]
        public string? BIZDOMAIN { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Grade")]
        public string GRADE { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Oper")]
        public string? OPER { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "QyGrade")]
        public string? QYGRADE { get; set; }
        /// <summary>
        /// "临时机构名称
        /// 01 中交京津冀区域总部（北京、天津、河北）
        /// 02 中交东北区域总部（黑龙江、吉林、辽宁、内 蒙） 
        /// 04 中交西北区域总部（陕西、甘肃、青海、宁夏、 新疆） 
        /// 06 中交海南区域总部（海南、南海） 
        /// 07 中交海西区域总部（福建、江西、湖南、台湾） 
        /// 08 中交西南区域总部（四川、重庆、云南、贵州、 西藏）
        /// 09 中交华中区域总部（湖北、河南、山西）
        /// 12 中交粤港澳大湾区区域总部(广东、香港、澳门、 广西)  
        /// 13 中交雄安总部(雄安新区) 
        /// 15 中交长三角区域总部(浙江、安徽、江苏、上海、 山东
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "TerritoryName")]
        public string? TERRITORYPRO { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Orule")]
        public string ORULE { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        [SugarColumn(Length = 22, ColumnName = "OrganGrade", ColumnDataType = "int")]
        public int ORGANGRADE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "Remark")]
        public string? NOTE { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Mrut")]
        public string? MRUT { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "RoId")]
        public string? ROID { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "GlobalSno")]
        public string? GLOBAL_SNO { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Scale")]
        public string? PROJECTSCALE { get; set; }
        /// <summary>
        /// 主键（机构关联信息 id，非本表主键）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "O2Bid")]
        public string O2BID { get; set; }
        /// <summary>
        /// "组织子属性，类型编码
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "TypeExt")]
        public string TYPEEXT { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ShortName")]
        public string SHORTNAME { get; set; }
        /// <summary>
        /// "独立授权",
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "Rown")]
        public string? ROWN { get; set; }
        /// <summary>
        /// 组织业务类型，类型编码
        /// base：基础平台
        /// finance：房地产业务组织
        /// hr：人力资源组织
        /// money：疏浚业务组织
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "BizType")]
        public string BIZTYPE { get; set; }
        /// <summary>
        /// :"持股情况 3 全资 2 控股  1 参股  0 不持股
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "ShareHoldings")]
        public string? SHAREHOLDINGS { get; set; }
        /// <summary>
        /// 是否独立核算 01 是  02 否  03 待定
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "IsIndependent")]
        public string? IS_INDEPENDENT { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "EnglishName")]
        public string? ENGLISHNAME { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "EnglishShortName")]
        public string? ENGLISHSHORTNAME { get; set; }
        /// <summary>
        /// "机构主数据编码
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "MDMCode")]
        public string? MDM_CODE { get; set; }
    }
}
