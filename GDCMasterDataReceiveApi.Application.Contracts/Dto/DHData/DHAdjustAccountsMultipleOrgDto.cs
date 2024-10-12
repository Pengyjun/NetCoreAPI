namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// DH核算机构(多组织)
    /// </summary>
    public class DHAdjustAccountsMultipleOrgDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 财务核算机构主数据编码 主键
        /// </summary>
        public string? Zaco { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? MdmCode { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        public string? ZztnameEn { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        public string? ZztnameLoc { get; set; }
        /// <summary>
        /// 机构名称（中文）
        /// </summary>
        public string? ZztnameZh { get; set; }
        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        public string? ZztshnameEn { get; set; }
        /// <summary>
        /// 机构简称（中文）
        /// </summary>
        public string? ZztshnameChs { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        public string? ZztshnameLoc { get; set; }
        /// <summary>
        /// 机构状态 0-停用，1-启用
        /// </summary>
        public string? Zyorgstate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Zremark { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? Ztreeid { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        public string? Ztreever { get; set; }
        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        public string? Ztorg { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? Zrule { get; set; }
        /// <summary>
        /// 所属二级单位编码
        /// </summary>
        public string? Zgpoid { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? Zaclayer { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        public string? Zacsortorder { get; set; }
        /// <summary>
        /// 核算机构中文名称
        /// </summary>
        public string? ZacnameChs { get; set; }
        /// <summary>
        /// 核算机构英文名称
        /// </summary>
        public string? ZacnameEn { get; set; }
        /// <summary>
        /// 核算机构当地语言名称
        /// </summary>
        public string? ZacnameLoc { get; set; }
        /// <summary>
        /// 核算机构简称-中文
        /// </summary>
        public string? ZacshortnameChs { get; set; }
        /// <summary>
        /// 核算机构简称-英文
        /// </summary>
        public string? ZacshortnameEn { get; set; }
        /// <summary>
        /// 核算机构简称-当地语言
        /// </summary>
        public string? ZacshortnameLoc { get; set; }
        /// <summary>
        /// 上级核算组织编号
        /// </summary>
        public string? Zacparentcode { get; set; }
        /// <summary>
        /// 是否投资项目/公司
        /// </summary>
        public string? Zivflgid { get; set; }
        /// <summary>
        /// 是否集团合并范围内单位
        /// </summary>
        public string? Zacjthbfwn { get; set; }
        /// <summary>
        /// 是否启用财务云报表
        /// </summary>
        public string? ZreportFlag { get; set; }
        /// <summary>
        /// 转入不启用报表时间
        /// </summary>
        public string? ZreportTime { get; set; }
        /// <summary>
        /// 组织节点性质
        /// </summary>
        public string? ZreportNode { get; set; }
        /// <summary>
        /// 核算机构状态 值域：0-停用、1-启用、2-休眠、3-删除
        /// </summary>
        public string? Zaorgstate { get; set; }
        /// <summary>
        /// 删除原因 当核算机构状态=删除时，输入删除原因。01-错误录入
        /// </summary>
        public string? ZdelRea { get; set; }
        /// <summary>
        /// 停用日期
        /// </summary>
        public string? Zacdisableyear { get; set; }
        /// <summary>
        /// 建账时间
        /// </summary>
        public string? ZaccountDate { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string? Zcyname { get; set; }
        /// <summary>
        /// 机构所在地
        /// </summary>
        public string? Zorgloc { get; set; }
        /// <summary>
        /// 地域属性
        /// </summary>
        public string? Zregional { get; set; }
        /// <summary>
        /// 决算业务板块
        /// </summary>
        public string? Zdcid { get; set; }
        /// <summary>
        /// 业务分类
        /// </summary>
        public string? Zbtid { get; set; }
        /// <summary>
        /// 记账本位币
        /// </summary>
        public string? Zzcurrency { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        public string? Ztaxmethod { get; set; }
        /// <summary>
        /// 所属税务组织
        /// </summary>
        public string? ZtaxOrganization { get; set; }
        /// <summary>
        /// 税组织纳税人类别
        /// </summary>
        public string? ZtaxpayerCategory { get; set; }
        /// <summary>
        /// 税组织纳税人识别号
        /// </summary>
        public string? Ztrno { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        public string? ZbusinessUnit { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string? Zuname { get; set; }
        /// <summary>
        /// 审批单位 
        /// </summary>
        public string? ZapprovalOrg { get; set; }
        /// <summary>
        /// 是否明细 1、是；0、否
        /// </summary>
        public string? Zacisdetail { get; set; }
        /// <summary>
        /// 分级码
        /// </summary>
        public string? Zacpath { get; set; }
        /// <summary>
        /// 是否为工商撤销机构 当组织机构视图状态为“撤销”时，自动填充为“是”
        /// </summary>
        public string? ZbusinessRecocation { get; set; }
        /// <summary>
        /// 核算组织编号
        /// </summary>
        public string? Zacorgno { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? Zacid { get; set; }
        /// <summary>
        /// 上级核算组织ID
        /// </summary>
        public string? Zacparentid { get; set; }
        /// <summary>
        /// 机构属性
        /// </summary>
        public string? Zorgattr { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        public string? Zorgchildattr { get; set; }
        /// <summary>
        /// 业务板块
        /// </summary>
        public string? Zbbid { get; set; }
        /// <summary>
        /// 是否内部非盈利
        /// </summary>
        public string? Znbfyl { get; set; }
        /// <summary>
        /// 所属区域共享中心
        /// </summary>
        public string? Zscenter { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（国资委）
        /// </summary>
        public string? Zbrgzw { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（商务部）
        /// </summary>
        public string? Zbrhw { get; set; }
        /// <summary>
        /// 是否启用财务云财务管理 
        /// </summary>
        public string? Zcwygl { get; set; }
        /// <summary>
        /// 不启用财务云财务管理原因说明 当zcwygl=否时，为必填字段，值域：01-大型合作单位（如绿城、碧水源、ccci-john holland等公司）；02-海外敏感项目
        /// </summary>
        public string? ZcwyglRea { get; set; }
        /// <summary>
        /// 是否高新企业 0-否，1-是
        /// </summary>
        public string? Zhte { get; set; }
        /// <summary>
        /// 州别名称
        /// </summary>
        public string? Zcontinentcode { get; set; }
        /// <summary>
        /// 报表节点性质
        /// </summary>
        public string? Zrpnature { get; set; }
        /// <summary>
        /// 境内/境外
        /// </summary>
        public string? ZhInOut { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string? Zsystem { get; set; }
        /// <summary>
        /// 视图标识  多值按,隔开，值域为zx-机构视图、zy-核算组织视图、zg-管理组织视图
        /// </summary>
        public string? ZviewFlag { get; set; }
        /// <summary>
        /// 是否删除主映射关系 当核算机构状态=休眠或停用时，0-否，1-是
        /// </summary>
        public string? Zdelmap { get; set; }
    }
}
