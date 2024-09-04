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
        public long Id { get; set; }
        /// <summary>
        /// 机构 ID
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// "分组所属的实体机构 ID
        /// </summary>
        public string? GOid { get; set; }
        /// <summary>
        /// 企业分类
        /// 0 外经企业 1 基建企业 2 疏浚企业 3 设计企业 4 装备制造企业 5 投资企业 6 房地产企业 7 贸易企业 8 金融企业 9 其他
        /// </summary>
        public string? EntClass { get; set; }
        /// <summary>
        /// 隶属单位 ID
        /// </summary>
        public string? COid { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        public string? Crossorgan { get; set; }
        /// <summary>
        /// 机构层级（没有用过）
        /// </summary>
        public string? OrgGrade { get; set; }
        /// <summary>
        /// 上级机构 id
        /// </summary>
        public string? POid { get; set; }
        /// <summary>
        /// "机构状态，类型编码"
        /// 1运营 2筹备 3停用 4撤销
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// "版本号
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// "创建时间
        /// </summary>
        public string? StartDate { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sno { get; set; }
        /// <summary>
        /// 机构所在地，类型编码
        /// </summary>
        public string? OrgProvince { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        public string? RegisterCode { get; set; }
        /// <summary>
        /// 项目类型"
        /// </summary>
        public string? PjectType { get; set; }
        /// <summary>
        /// 国家地区，类型编码
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// "临时机构名称
        /// </summary>
        public string? TemporaryOrgName { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string? OrgAemp { get; set; }
        /// <summary>
        /// "机构属性，类型编码
        /// C:企业 C10：常设公司 C20：项目公司 C30：特殊目的（SPV）公司 D30：分公司 C60：办事处/代表处 C70：模拟分公司 C80：非企业法人  C99：有限合伙企业 D：内部机构 D00：总部 D10：总部部门
        /// D20：事业部 D40：区域总部 D50：共享服务机构 D60：项目部/指挥部 D70：虚拟机构（行政管理） D80：财务虚拟核算单位 D90：生产单元 G：分组 G10：办事处 G20：子公司 G30：项目公司 G40：项目部
        /// G50：分公司 G80：虚拟分组下的 SPV公司 G99：虚拟机构 G120：直属机构 D80：直属机构 G85：非企业法人
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 机构编码"
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 分组时的上级机构 ID
        /// </summary>
        public string? GPOid { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        public string? ManType { get; set; }
        /// <summary>
        /// 分组模式规则码
        /// </summary>
        public string? Grule { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        public string? BizDomain { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? Grade { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string? Oper { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        public string? QyGrade { get; set; }
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
        public string? TerritoryName { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? Orule { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        public int OrganGrade { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public string? Mrut { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        public string? RoId { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        public string? GlobalSno { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        public string? Scale { get; set; }
        /// <summary>
        /// 主键（机构关联信息 id，非本表主键）
        /// </summary>
        public string? O2Bid { get; set; }
        /// <summary>
        /// "组织子属性，类型编码
        /// </summary>
        public string? TypeExt { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// "独立授权",
        /// </summary>
        public string? Rown { get; set; }
        /// <summary>
        /// 组织业务类型，类型编码
        /// base：基础平台
        /// finance：房地产业务组织
        /// hr：人力资源组织
        /// money：疏浚业务组织
        /// </summary>
        public string? BizType { get; set; }
        /// <summary>
        /// :"持股情况 3 全资 2 控股  1 参股  0 不持股
        /// </summary>
        public string? ShareHoldings { get; set; }
        /// <summary>
        /// 是否独立核算 01 是  02 否  03 待定
        /// </summary>
        public string? IsIndependent { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        public string? EnglishName { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        public string? EnglishShortName { get; set; }
        /// <summary>
        /// "机构主数据编码
        /// </summary>
        public string? MDMCode { get; set; }
    }
    /// <summary>
    /// 机构主数据 接收
    /// </summary>
    public class InstitutionReceiveDto
    {
        /// <summary>
        /// 机构 ID
        /// </summary>
        public string? OID { get; set; }
        /// <summary>
        /// "分组所属的实体机构 ID
        /// </summary>
        public string? GOID { get; set; }
        /// <summary>
        /// 企业分类
        /// 0 外经企业 1 基建企业 2 疏浚企业 3 设计企业 4 装备制造企业 5 投资企业 6 房地产企业 7 贸易企业 8 金融企业 9 其他
        /// </summary>
        public string? ENTCLASS { get; set; }
        /// <summary>
        /// 隶属单位 ID
        /// </summary>
        public string? COID { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        public string? CROSSORGAN { get; set; }
        /// <summary>
        /// 机构层级（没有用过）
        /// </summary>
        public string? ORGGRADE { get; set; }
        /// <summary>
        /// 上级机构 id
        /// </summary>
        public string? POID { get; set; }
        /// <summary>
        /// "机构状态，类型编码"
        /// 1运营 2筹备 3停用 4撤销
        /// </summary>
        public string? STATUS { get; set; }
        /// <summary>
        /// "版本号
        /// </summary>
        public string? VERSION { get; set; }
        /// <summary>
        /// "创建时间
        /// </summary>
        public string? STARTDATE { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SNO { get; set; }
        /// <summary>
        /// 机构所在地，类型编码
        /// </summary>
        public string? ORGPROVINCE { get; set; }
        /// <summary>
        /// 注册码
        /// </summary
        public string? REGISTERCODE { get; set; }
        /// <summary>
        /// 项目类型"
        /// </summary>
        public string? PROJECTTYPE { get; set; }
        /// <summary>
        /// 国家地区，类型编码
        /// </summary>
        public string? CAREA { get; set; }
        /// <summary>
        /// "临时机构名称
        /// </summary>
        public string? TEMORGANNAME { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string? ORGANEMP { get; set; }
        /// <summary>
        /// "机构属性，类型编码
        /// C:企业 C10：常设公司 C20：项目公司 C30：特殊目的（SPV）公司 D30：分公司 C60：办事处/代表处 C70：模拟分公司 C80：非企业法人  C99：有限合伙企业 D：内部机构 D00：总部 D10：总部部门
        /// D20：事业部 D40：区域总部 D50：共享服务机构 D60：项目部/指挥部 D70：虚拟机构（行政管理） D80：财务虚拟核算单位 D90：生产单元 G：分组 G10：办事处 G20：子公司 G30：项目公司 G40：项目部
        /// G50：分公司 G80：虚拟分组下的 SPV公司 G99：虚拟机构 G120：直属机构 D80：直属机构 G85：非企业法人
        /// </summary>
        public string? TYPE { get; set; }
        /// <summary>
        /// 机构编码"
        /// </summary>
        public string? OCODE { get; set; }
        /// <summary>
        /// 分组时的上级机构 ID
        /// </summary>
        public string? GPOID { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        public string? PROJECTMANTYPE { get; set; }
        /// <summary>
        /// 分组模式规则码
        /// </summary>
        public string? GRULE { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        public string? BIZDOMAIN { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? GRADE { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string? OPER { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
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
        public string? TERRITORYPRO { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? ORULE { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        public int ORGANGRADE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? NOTE { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public string? MRUT { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        public string? ROID { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        public string? GLOBAL_SNO { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        public string? PROJECTSCALE { get; set; }
        /// <summary>
        /// 主键（机构关联信息 id，非本表主键）
        /// </summary>
        public string? O2BID { get; set; }
        /// <summary>
        /// "组织子属性，类型编码
        /// </summary>
        public string? TYPEEXT { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string? SHORTNAME { get; set; }
        /// <summary>
        /// "独立授权",
        /// </summary>
        public string? ROWN { get; set; }
        /// <summary>
        /// 组织业务类型，类型编码
        /// base：基础平台
        /// finance：房地产业务组织
        /// hr：人力资源组织
        /// money：疏浚业务组织
        /// </summary>
        public string? BIZTYPE { get; set; }
        /// <summary>
        /// :"持股情况 3 全资 2 控股  1 参股  0 不持股
        /// </summary>
        public string? SHAREHOLDINGS { get; set; }
        /// <summary>
        /// 是否独立核算 01 是  02 否  03 待定
        /// </summary>
        public string? IS_INDEPENDENT { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        public string? ENGLISHNAME { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        public string? ENGLISHSHORTNAME { get; set; }
        /// <summary>
        /// "机构主数据编码
        /// </summary>
        public string? MDM_CODE { get; set; }
    }
}
