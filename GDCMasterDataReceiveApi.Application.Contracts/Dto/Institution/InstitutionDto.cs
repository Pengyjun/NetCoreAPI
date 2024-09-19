using SqlSugar;

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
        /// 机构业务类型 见7.2.3 字典
        /// </summary>
        public string? BizType { get; set; }
        /// <summary>
        /// 关联机构id
        /// </summary>
        public string? OSecBid { get; set; }
        /// <summary>
        /// 分组时的上级机构ID
        /// </summary>
        public string? Gpoid { get; set; }
        /// <summary>
        /// 机构属性,见7.2.2 字典
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 机构子属性,见7.2.2 字典
        /// </summary>
        public string? TypeExt { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public string? Mrut { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string? Sno { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
        public string? Coid { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        public string? Crossorgan { get; set; }
        /// <summary>
        /// 分组所属的实体机构ID
        /// </summary>
        public string? Goid { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? Grade { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string? Oper { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Note { get; set; }
        /// <summary>
        /// 临时机构名称
        /// </summary>
        public string? TemorganName { get; set; }
        /// <summary>
        /// 机构所在地, 见7.2.7 字典
        /// </summary>
        public string? OrgProvince { get; set; }
        /// <summary>
        /// 国家名称, 见7.2.6 字典
        /// </summary>
        public string? Carea { get; set; }
        /// <summary>
        ///   地域属性, 见7.2.5 字典
        /// </summary>
        public string? TerritoryPro { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        public string? BizDomain { get; set; }
        /// <summary>
        /// 机构层级(没有用过)
        /// </summary>
        public string? OrgGrade { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        public string? ProjectScale { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        public string? ProjectManType { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ProjectType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartDate { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string? Organemp { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        public string? OrganGrade { get; set; }
        /// <summary>
        /// 独立授权
        /// </summary>
        public string? Rown { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        public string? RoId { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        public string? GlobalSno { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        public string? QyGrade { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        public string? RegisterCode { get; set; }
        /// <summary>
        ///版本号
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 持股情况，见7.2.9 字典
        /// </summary>
        public string? ShareHoldings { get; set; }
        /// <summary>
        /// 是否独立核算，见7.2.8 字典
        /// </summary>
        public string? IsIndependent { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? MdmCode { get; set; }
        /// <summary>
        /// 机构子集
        /// </summary>
        public List<InstitutionDetatilsDto>? Children { get; set; }
    }

}
