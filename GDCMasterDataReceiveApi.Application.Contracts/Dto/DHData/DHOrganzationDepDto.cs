namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// DH行政机构-多组织
    /// </summary>
    public class DHOrganzationDepDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? MdmCode { get; set; }
        /// <summary>
        /// 机构名称（中文）
        /// </summary>
        public string? ZztnameZh { get; set; }
        /// <summary>
        /// 机构简称（中文）
        /// </summary>
        public string? ZztshnameChs { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        public string? ZztshnameEn { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        public string? ZztnameLoc { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        public string? ZztshnameLoc { get; set; }
        /// <summary>
        /// 组织机构的上级机构主数据编码
        /// </summary>
        public string? Zorgup { get; set; }
        /// <summary>
        /// 机构主数据编码（旧）
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 上级机构主数据编码（旧）
        /// </summary>
        public string? Poid { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? Zorule { get; set; }
        /// <summary>
        /// 所属二级单位编码
        /// </summary>
        public string? Zgpoid { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string? ZoLevel { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        public string? Zsno { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string? Zorgno { get; set; }
        /// <summary>
        /// 机构属性
        /// </summary>
        public string? Zoattr { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        public string? Zocattr { get; set; }
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
        /// 注册号/统一社会信用代码
        /// </summary>
        public string? Zcuscc { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string? Zaddress { get; set; }
        /// <summary>
        /// 持股情况
        /// </summary>
        public string? Zholding { get; set; }
        /// <summary>
        /// 是否独立核算  是否单独建立核算账套对本机构发生的业务进行会计核算，填写“是”或“否”
        /// </summary>
        public string? Zcheckind { get; set; }
        /// <summary>
        /// 机构树形编码 机构树形编码，编码1，代表组织机构
        /// </summary>
        public string? Ztreeid1 { get; set; }
        /// <summary>
        /// 多组织树形版本 
        /// </summary>
        public string? Ztreever { get; set; }
        /// <summary>
        /// 视图标识 多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        public string? ViewFlag { get; set; }
        /// <summary>
        /// 值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        public string? Zostate { get; set; }
        /// <summary>
        /// 企业分类,如果多值则按“,”（英文逗号）进行隔开
        /// </summary>
        public string? Zentc { get; set; }
    }
}
