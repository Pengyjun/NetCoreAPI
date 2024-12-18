using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 机构
    /// </summary>
    [SugarTable("t_institution", IsDisabledDelete = true, TableDescription = "机构")]
    public class Institution : BaseEntity<long>
    {
        /// <summary>
        /// oid
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Oid { get; set; }
        /// <summary>
        /// 机构业务类型 见7.2.3 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? BizType { get; set; }
        /// <summary>
        /// 关联机构id
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? O2bid { get; set; }
        /// <summary>
        /// 上级机构id
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Poid { get; set; }
        /// <summary>
        /// 分组时的上级机构ID
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? GPoid { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Orule { get; set; }
        /// <summary>
        /// 分组机构规则码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Grule { get; set; }
        /// <summary>
        /// 机构属性,见7.2.2 字典
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? Type { get; set; }
        /// <summary>
        /// 机构子属性,见7.2.2 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TypeExt { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Mrut { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Sno { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? Name { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ShortName { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>  
        [SugarColumn(Length = 128)]
        public string? Ocode { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Coid { get; set; }
        /// <summary>
        /// 拥有兼管职能
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Crossorgan { get; set; }
        /// <summary>
        /// 分组所属的实体机构ID
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Goid { get; set; }
        /// <summary>
        /// 机构状态, 见7.2.1 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Status { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Grade { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Oper { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? Note { get; set; }
        /// <summary>
        /// 临时机构名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TemorganName { get; set; }
        /// <summary>
        /// 机构所在地, 见7.2.7 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? OrgProvince { get; set; }
        /// <summary>
        /// 国家名称, 见7.2.6 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Carea { get; set; }
        /// <summary>
        ///地域属性, 见7.2.5 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? TerritoryPro { get; set; }
        /// <summary>
        /// 业务领域
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? BizDomain { get; set; }
        /// <summary>
        /// 企业分类, 见7.2.4 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? EntClass { get; set; }
        /// <summary>
        /// 机构层级(没有用过)
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? OrgGrade { get; set; }
        /// <summary>
        /// 项目规模
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ProjectScale { get; set; }
        /// <summary>
        /// 项目管理类型
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ProjectManType { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ProjectType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? StartDate { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Organemp { get; set; }
        /// <summary>
        /// 管理层级
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? OrganGrade { get; set; }
        /// <summary>
        /// 独立授权
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Rown { get; set; }
        /// <summary>
        /// 授权机构
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Roid { get; set; }
        /// <summary>
        /// 全部机构的大流水号
        /// </summary>
        public string? Global_Sno { get; set; }
        /// <summary>
        /// 股权层级
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? QyGrade { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? RegisterCode { get; set; }
        /// <summary>
        ///版本号
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Version { get; set; }
        /// <summary>
        /// 持股情况，见7.2.9 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ShareHoldings { get; set; }
        /// <summary>
        /// 是否独立核算，见7.2.8 字典
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Is_Independent { get; set; }
        /// <summary>
        /// 英文全称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? EnglishName { get; set; }
        /// <summary>
        /// 英文简称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? EnglishShortName { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? Mdm_Code { get; set; }
    }
}
