using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization
{
    /// <summary>
    /// 多组织-核算机构反显响应dto
    /// </summary>
    public class AccountingOrganizationSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 财务核算机构主数据编码
        /// </summary>
        public string? ZACO { get; set; }
        /// <summary>
        /// 机构状态“0-停用”、“1-启用”
        /// </summary>
        public string? ZYORGSTATE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? ZREMARK { get; set; }
        /// <summary>
        /// 核算机构中文名称
        /// </summary>
        public string? ZACNAME_CHS { get; set; }
        /// <summary>
        /// 核算机构英文名称
        /// </summary>
        public string? ZACNAME_EN { get; set; }
        /// <summary>
        /// 核算机构当地语言名称
        /// </summary>
        public string? ZACNAME_LOC { get; set; }
        /// <summary>
        /// 核算机构简称-中文
        /// </summary>
        public string? ZACSHORTNAME_CHS { get; set; }
        /// <summary>
        /// 核算机构简称-英文
        /// </summary>
        public string? ZACSHORTNAME_EN { get; set; }
        /// <summary>
        /// 核算机构简称-当地语言
        /// </summary>
        public string? ZACSHORTNAME_LOC { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（国资委）
        /// </summary>
        public string? ZBRGZW { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（商务部）
        /// </summary>
        public string? ZBRHW { get; set; }
        /// <summary>
        /// 是否高新企业 当“组织节点性质”为“单户单位”时必填。0-否，1-是
        /// </summary>
        public string? ZHTE { get; set; }
        /// <summary>
        /// 州别名称
        /// </summary>
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 报表节点性质
        /// </summary>
        public string? ZRPNATURE { get; set; }
        /// <summary>
        /// 境内/境外 根据所属国家自动生成；值域为“境内”“境外”
        /// </summary>
        public string? ZH_IN_OUT { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 机构名称（中文）:机构的规范全称
        /// </summary>
        public string? ZZTNAME_ZH { get; set; }
        /// <summary>
        /// 机构简称（中文）:机构的规范简称
        /// </summary>
        public string? ZZTSHNAME_CHS { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        public string? ZZTNAME_EN { get; set; }
        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        public string? ZZTSHNAME_EN { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        public string? ZZTNAME_LOC { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        public string? ZZTSHNAME_LOC { get; set; }
        /// <summary>
        /// 机构状态:值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        public string? ZOSTATE { get; set; }
        /// <summary>
        /// 国家名称:机构所处的国家/地区
        /// </summary>
        public string? ZCYNAME { get; set; }
    }
    /// <summary>
    /// 多组织-核算机构 详情
    /// </summary>
    public class AccountingOrganizationDetailsDto
    {
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        [ExcelColumnName("机构主数据编码")]
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 财务核算机构主数据编码
        /// </summary>
        [ExcelColumnName("财务核算机构主数据编码")]
        public string? ZACO { get; set; }
        /// <summary>
        /// 机构状态“0-停用”、“1-启用”
        /// </summary>
        [ExcelIgnore]
        public string? ZYORGSTATE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]
        public string? ZREMARK { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        [ExcelColumnName("组织树编码")]
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        [ExcelIgnore]
        public string? ZTORG { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [ExcelIgnore]
        public string? ZRULE { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [ExcelIgnore]
        public string? ZACLAYER { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        [ExcelIgnore]
        public string? ZACSORTORDER { get; set; }
        /// <summary>
        /// 核算机构中文名称
        /// </summary>
        [ExcelColumnName("核算机构中文名称")]
        public string? ZACNAME_CHS { get; set; }
        /// <summary>
        /// 核算机构英文名称
        /// </summary>
        [ExcelColumnName("核算机构英文名称")]
        public string? ZACNAME_EN { get; set; }
        /// <summary>
        /// 核算机构当地语言名称
        /// </summary>
        [ExcelColumnName("核算机构当地语言名称")]
        public string? ZACNAME_LOC { get; set; }
        /// <summary>
        /// 核算机构简称-中文
        /// </summary>
        [ExcelColumnName("核算机构简称-中文")]
        public string? ZACSHORTNAME_CHS { get; set; }
        /// <summary>
        /// 核算机构简称-英文
        /// </summary>
        [ExcelColumnName("核算机构简称-英文")]
        public string? ZACSHORTNAME_EN { get; set; }
        /// <summary>
        /// 核算机构简称-当地语言
        /// </summary>
        [ExcelColumnName("核算机构简称-当地语言")]
        public string? ZACSHORTNAME_LOC { get; set; }
        /// <summary>
        /// 上级核算组织编号
        /// </summary>
        [ExcelIgnore]
        public string? ZACPARENTCODE { get; set; }
        /// <summary>
        /// 是否投资项目/公司  ，当“组织节点性质”为“单户单位”时必填。0-否，1-是
        /// </summary>
        [ExcelIgnore]
        public string? ZIVFLGID { get; set; }
        /// <summary>
        /// 是否集团合并范围内单位
        /// </summary>
        [ExcelIgnore]
        public string? ZACJTHBFWN { get; set; }
        /// <summary>
        /// 是否启用财务云报表  多机构修改，是否集团合并范围内单位为“是”
        /// </summary>
        [ExcelIgnore]
        public string? ZREPORT_FLAG { get; set; }
        /// <summary>
        /// 启用报表时间  2024年6月12日新增，当“是否启用财务云报表”字段为是 或 核算机构状态为启用或休眠且“是否启用财务云报表”为“是”，必填
        /// </summary>
        [ExcelIgnore]
        public string? ZQYBBDAT { get; set; }
        /// <summary>
        /// 转入不启用报表时间 ，是否启用报表由“是”变更为“否”时，必填；时间采用yyyymmdd格式
        /// </summary>
        [ExcelIgnore]
        public string? ZREPORT_TIME { get; set; }
        /// <summary>
        /// 组织节点性质
        /// </summary>
        [ExcelColumnName("组织节点性质")]
        public string? ZREPORT_NODE { get; set; }
        /// <summary>
        /// 核算机构状态 值域：0-停用、1-启用、2-休眠、3-删除
        /// </summary>
        [ExcelIgnore]
        public string? ZAORGSTATE { get; set; }
        /// <summary>
        /// 删除原因 当核算机构状态=删除时，输入删除原因。01-错误录入;02-财务云历史数据映射错误
        /// </summary>
        [ExcelIgnore]
        public string? ZDEL_REA { get; set; }
        /// <summary>
        /// 是否删除主映射关系
        /// </summary>
        [ExcelIgnore]
        public string? ZDEL_MAP { get; set; }
        /// <summary>
        /// 停用日期  当核算机构状态=休眠或停用时，0-否，1-是
        /// </summary>
        [ExcelColumnName("停用日期")]
        public string? ZACDISABLEYEAR { get; set; }
        /// <summary>
        /// 建账时间 存在初始化数据为空的情况
        /// </summary>
        [ExcelColumnName("建账时间")]
        public string? ZACCOUNT_DATE { get; set; }
        /// <summary>
        /// 决算业务板块
        /// </summary>
        [ExcelColumnName("决算业务板块")]
        public string? ZDCID { get; set; }
        /// <summary>
        /// 业务分类
        /// </summary>
        [ExcelColumnName("业务分类")]
        public string? ZBTID { get; set; }
        /// <summary>
        /// 记账本位币 ，当“组织节点性质”为“单户单位”时必填
        /// </summary>
        [ExcelColumnName("记账本位币")]
        public string? ZZCURRENCY { get; set; }
        /// <summary>
        /// 计税方式 当“组织节点性质”为“单户单位”时必填
        /// </summary>
        [ExcelColumnName("计税方式")]
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 所属税务组织 当“组织节点性质”为“单户单位”时必填
        /// </summary>
        [ExcelColumnName("所属税务组织")]
        public string? ZTAX_ORGANIZATION { get; set; }
        /// <summary>
        /// 税组织纳税人类别 当“组织节点性质”为“单户单位”时必填
        /// </summary>
        [ExcelColumnName("税组织纳税人类别")]
        public string? ZTAXPAYER_CATEGORY { get; set; }
        /// <summary>
        /// 税组织纳税人识别号 ，当“组织节点性质”为“单户单位”时必填；
        /// </summary>
        [ExcelColumnName("税组织纳税人识别号")]
        public string? ZTRNO { get; set; }
        /// <summary>
        /// 所属事业部 当“组织节点性质”为“单户单位”时必
        /// </summary>
        [ExcelIgnore]
        public string? ZBUSINESS_UNIT { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [ExcelColumnName("申请人")]
        public string? ZUNAME { get; set; }
        /// <summary>
        /// 审批单位
        /// </summary>
        [ExcelColumnName("审批单位")]
        public string? ZAPPROVAL_ORG { get; set; }
        /// <summary>
        /// 是否明细 1、是；0、否
        /// </summary>
        [ExcelIgnore]
        public string? ZACISDETAIL { get; set; }
        /// <summary>
        /// 分级码
        /// </summary>
        [ExcelIgnore]
        public string? ZACPATH { get; set; }
        /// <summary>
        /// 是否为工商撤销机构 当组织机构视图状态为“撤销”时，自动填充为“是”
        /// </summary>
        [ExcelIgnore]
        public string? ZBUSINESS_RECOCATION { get; set; }
        /// <summary>
        /// 核算组织编号 与ZACO值一致
        /// </summary>
        [ExcelIgnore]
        public string? ZACORGNO { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        [ExcelIgnore]
        public string? ZACID { get; set; }
        /// <summary>
        /// 上级核算组织ID
        /// </summary>
        [ExcelIgnore]
        public string? ZACPARENTID { get; set; }
        /// <summary>
        /// 机构属性
        /// </summary>
        [ExcelIgnore]
        public string? ZORGATTR { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        [ExcelIgnore]
        public string? ZORGCHILDATTR { get; set; }
        /// <summary>
        /// 业务板块
        /// </summary>
        [ExcelColumnName("业务板块")]
        public string? ZBBID { get; set; }
        /// <summary>
        /// 是否内部非盈利
        /// </summary>
        [ExcelIgnore]
        public string? ZNBFYL { get; set; }
        /// <summary>
        /// 所属区域共享中心
        /// </summary>
        [ExcelIgnore]
        public string? ZSCENTER { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（国资委）
        /// </summary>
        [ExcelIgnore]
        public string? ZBRGZW { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（商务部）
        /// </summary>
        [ExcelIgnore]
        public string? ZBRHW { get; set; }
        /// <summary>
        /// 是否启用财务云财务管理
        /// </summary>
        [ExcelIgnore]
        public string? ZCWYGL { get; set; }
        /// <summary>
        /// 不启用财务云财务管理原因说明  值域：01-大型合作单位（如绿城、碧水源、CCCI-John Holland等公司）；02-海外敏感项目；03-收尾完工账套无业务；04-RH项目业主要求不允许上线；05-联营项目；06-	其他（中国港湾境外业务专用）。
        /// </summary>
        [ExcelIgnore]
        public string? ZCWYGL_REA { get; set; }
        /// <summary>
        /// 是否高新企业 当“组织节点性质”为“单户单位”时必填。0-否，1-是
        /// </summary>
        [ExcelIgnore]
        public string? ZHTE { get; set; }
        /// <summary>
        /// 州别名称
        /// </summary>
        [ExcelColumnName("州别名称")]
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 报表节点性质
        /// </summary>
        [ExcelColumnName("报表节点性质")]
        public string? ZRPNATURE { get; set; }
        /// <summary>
        /// 境内/境外 根据所属国家自动生成；值域为“境内”“境外”
        /// </summary>
        [ExcelColumnName("境内/境外")]
        public string? ZH_IN_OUT { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        [ExcelIgnore]
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 机构名称（中文）:机构的规范全称
        /// </summary>
        [ExcelColumnName("机构名称（中文）")]
        public string? ZZTNAME_ZH { get; set; }
        /// <summary>
        /// 机构简称（中文）:机构的规范简称
        /// </summary>
        [ExcelColumnName("机构简称（中文）")]
        public string? ZZTSHNAME_CHS { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        [ExcelColumnName("机构名称（英文）")]
        public string? ZZTNAME_EN { get; set; }
        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        [ExcelColumnName("机构简称（英文）")]
        public string? ZZTSHNAME_EN { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        [ExcelColumnName("机构名称（当地语言）")]
        public string? ZZTNAME_LOC { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        [ExcelColumnName("机构简称（当地语言）")]
        public string? ZZTSHNAME_LOC { get; set; }
        /// <summary>
        /// 机构状态:值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        [ExcelIgnore]
        public string? ZOSTATE { get; set; }
        /// <summary>
        /// 节点排序号:当前节点下机构的排序号
        /// </summary>
        [ExcelIgnore]
        public string? ZSNO { get; set; }
        /// <summary>
        /// 国家名称:机构所处的国家/地区
        /// </summary>
        [ExcelColumnName("国家名称")]
        public string? ZCYNAME { get; set; }
        /// <summary>
        /// 机构所在地:当国家名称为142中国时必填
        /// </summary>
        [ExcelColumnName("机构所在地")]
        public string? ZORGLOC { get; set; }
        /// <summary>
        /// 地域属性:机构所处的位置对应的中交的区域划分
        /// </summary>
        [ExcelColumnName("地域属性")]
        public string? ZREGIONAL { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        [ExcelIgnore]
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 视图标识:多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [ExcelIgnore]
        public string? VIEW_FLAG { get; set; }
    }
    /// <summary>
    /// 多组织-核算机构接收响应dto
    /// </summary>
    public class AccountingOrganizationReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 财务核算机构主数据编码
        /// </summary>
        public string? ZACO { get; set; }
        /// <summary>
        /// 机构状态“0-停用”、“1-启用”
        /// </summary>
        public string? ZYORGSTATE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? ZREMARK { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        public string? ZTORG { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? ZRULE { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? ZACLAYER { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        public string? ZACSORTORDER { get; set; }
        /// <summary>
        /// 核算机构中文名称
        /// </summary>
        public string? ZACNAME_CHS { get; set; }
        /// <summary>
        /// 核算机构英文名称
        /// </summary>
        public string? ZACNAME_EN { get; set; }
        /// <summary>
        /// 核算机构当地语言名称
        /// </summary>
        public string? ZACNAME_LOC { get; set; }
        /// <summary>
        /// 核算机构简称-中文
        /// </summary>
        public string? ZACSHORTNAME_CHS { get; set; }
        /// <summary>
        /// 核算机构简称-英文
        /// </summary>
        public string? ZACSHORTNAME_EN { get; set; }
        /// <summary>
        /// 核算机构简称-当地语言
        /// </summary>
        public string? ZACSHORTNAME_LOC { get; set; }
        /// <summary>
        /// 上级核算组织编号
        /// </summary>
        public string? ZACPARENTCODE { get; set; }
        /// <summary>
        /// 是否投资项目/公司  ，当“组织节点性质”为“单户单位”时必填。0-否，1-是
        /// </summary>
        public string? ZIVFLGID { get; set; }
        /// <summary>
        /// 是否集团合并范围内单位
        /// </summary>
        public string? ZACJTHBFWN { get; set; }
        /// <summary>
        /// 是否启用财务云报表  多机构修改，是否集团合并范围内单位为“是”
        /// </summary>
        public string? ZREPORT_FLAG { get; set; }
        /// <summary>
        /// 启用报表时间  2024年6月12日新增，当“是否启用财务云报表”字段为是 或 核算机构状态为启用或休眠且“是否启用财务云报表”为“是”，必填
        /// </summary>
        public string? ZQYBBDAT { get; set; }
        /// <summary>
        /// 转入不启用报表时间 ，是否启用报表由“是”变更为“否”时，必填；时间采用yyyymmdd格式
        /// </summary>
        public string? ZREPORT_TIME { get; set; }
        /// <summary>
        /// 组织节点性质
        /// </summary>
        public string? ZREPORT_NODE { get; set; }
        /// <summary>
        /// 核算机构状态 值域：0-停用、1-启用、2-休眠、3-删除
        /// </summary>
        public string? ZAORGSTATE { get; set; }
        /// <summary>
        /// 删除原因 当核算机构状态=删除时，输入删除原因。01-错误录入;02-财务云历史数据映射错误
        /// </summary>
        public string? ZDEL_REA { get; set; }
        /// <summary>
        /// 是否删除主映射关系
        /// </summary>
        public string? ZDEL_MAP { get; set; }
        /// <summary>
        /// 停用日期  当核算机构状态=休眠或停用时，0-否，1-是
        /// </summary>
        public string? ZACDISABLEYEAR { get; set; }
        /// <summary>
        /// 建账时间 存在初始化数据为空的情况
        /// </summary>
        public string? ZACCOUNT_DATE { get; set; }
        /// <summary>
        /// 决算业务板块
        /// </summary>
        public string? ZDCID { get; set; }
        /// <summary>
        /// 业务分类
        /// </summary>
        public string? ZBTID { get; set; }
        /// <summary>
        /// 记账本位币 ，当“组织节点性质”为“单户单位”时必填
        /// </summary>
        public string? ZZCURRENCY { get; set; }
        /// <summary>
        /// 计税方式 当“组织节点性质”为“单户单位”时必填
        /// </summary>
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 所属税务组织 当“组织节点性质”为“单户单位”时必填
        /// </summary>
        public string? ZTAX_ORGANIZATION { get; set; }
        /// <summary>
        /// 税组织纳税人类别 当“组织节点性质”为“单户单位”时必填
        /// </summary>
        public string? ZTAXPAYER_CATEGORY { get; set; }
        /// <summary>
        /// 税组织纳税人识别号 ，当“组织节点性质”为“单户单位”时必填；
        /// </summary>
        public string? ZTRNO { get; set; }
        /// <summary>
        /// 所属事业部 当“组织节点性质”为“单户单位”时必
        /// </summary>
        public string? ZBUSINESS_UNIT { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string? ZUNAME { get; set; }
        /// <summary>
        /// 审批单位
        /// </summary>
        public string? ZAPPROVAL_ORG { get; set; }
        /// <summary>
        /// 是否明细 1、是；0、否
        /// </summary>
        public string? ZACISDETAIL { get; set; }
        /// <summary>
        /// 分级码
        /// </summary>
        public string? ZACPATH { get; set; }
        /// <summary>
        /// 是否为工商撤销机构 当组织机构视图状态为“撤销”时，自动填充为“是”
        /// </summary>
        public string? ZBUSINESS_RECOCATION { get; set; }
        /// <summary>
        /// 核算组织编号 与ZACO值一致
        /// </summary>
        public string? ZACORGNO { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? ZACID { get; set; }
        /// <summary>
        /// 上级核算组织ID
        /// </summary>
        public string? ZACPARENTID { get; set; }
        /// <summary>
        /// 机构属性
        /// </summary>
        public string? ZORGATTR { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        public string? ZORGCHILDATTR { get; set; }
        /// <summary>
        /// 业务板块
        /// </summary>
        public string? ZBBID { get; set; }
        /// <summary>
        /// 是否内部非盈利
        /// </summary>
        public string? ZNBFYL { get; set; }
        /// <summary>
        /// 所属区域共享中心
        /// </summary>
        public string? ZSCENTER { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（国资委）
        /// </summary>
        public string? ZBRGZW { get; set; }
        /// <summary>
        /// 是否“一带一路”机构（商务部）
        /// </summary>
        public string? ZBRHW { get; set; }
        /// <summary>
        /// 是否启用财务云财务管理
        /// </summary>
        public string? ZCWYGL { get; set; }
        /// <summary>
        /// 不启用财务云财务管理原因说明  值域：01-大型合作单位（如绿城、碧水源、CCCI-John Holland等公司）；02-海外敏感项目；03-收尾完工账套无业务；04-RH项目业主要求不允许上线；05-联营项目；06-	其他（中国港湾境外业务专用）。
        /// </summary>
        public string? ZCWYGL_REA { get; set; }
        /// <summary>
        /// 是否高新企业 当“组织节点性质”为“单户单位”时必填。0-否，1-是
        /// </summary>
        public string? ZHTE { get; set; }
        /// <summary>
        /// 州别名称
        /// </summary>
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 报表节点性质
        /// </summary>
        public string? ZRPNATURE { get; set; }
        /// <summary>
        /// 境内/境外 根据所属国家自动生成；值域为“境内”“境外”
        /// </summary>
        public string? ZH_IN_OUT { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 机构名称（中文）:机构的规范全称
        /// </summary>
        public string? ZZTNAME_ZH { get; set; }
        /// <summary>
        /// 机构简称（中文）:机构的规范简称
        /// </summary>
        public string? ZZTSHNAME_CHS { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        public string? ZZTNAME_EN { get; set; }
        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        public string? ZZTSHNAME_EN { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        public string? ZZTNAME_LOC { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        public string? ZZTSHNAME_LOC { get; set; }
        /// <summary>
        /// 机构状态:值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        public string? ZOSTATE { get; set; }
        /// <summary>
        /// 节点排序号:当前节点下机构的排序号
        /// </summary>
        public string? ZSNO { get; set; }
        /// <summary>
        /// 国家名称:机构所处的国家/地区
        /// </summary>
        public string? ZCYNAME { get; set; }
        /// <summary>
        /// 机构所在地:当国家名称为142中国时必填
        /// </summary>
        public string? ZORGLOC { get; set; }
        /// <summary>
        /// 地域属性:机构所处的位置对应的中交的区域划分
        /// </summary>
        public string? ZREGIONAL { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 视图标识:多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        public string? VIEW_FLAG { get; set; }
    }
}
