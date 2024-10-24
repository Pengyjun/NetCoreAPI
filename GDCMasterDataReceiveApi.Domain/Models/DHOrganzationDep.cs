using SqlSugar;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH行政机构-多组织
    /// </summary>
    [SugarTable("t_dh_organzationdep", IsDisabledDelete = true)]
    public class DHOrganzationDep : BaseEntity<long>
    {
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [DisplayName("机构主数据编码")]
        public string? MdmCode { get; set; }
        /// <summary>
        /// 机构名称（中文）
        /// </summary>
        [DisplayName("机构名称（中文）")]
        public string? ZztnameZh { get; set; }
        /// <summary>
        /// 机构简称（中文）
        /// </summary>
        [DisplayName("机构简称（中文）")]
        public string? ZztshnameChs { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        [DisplayName("机构名称（英文）")]
        public string? ZztshnameEn { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        [DisplayName("机构名称（当地语言）")]
        public string? ZztnameLoc { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        [DisplayName("机构简称（当地语言）")]
        public string? ZztshnameLoc { get; set; }
        /// <summary>
        /// 组织机构的上级机构主数据编码
        /// </summary>
        [DisplayName("组织机构的上级机构主数据编码")]
        public string? Zorgup { get; set; }
        /// <summary>
        /// 机构主数据编码（旧）
        /// </summary>
        [DisplayName("机构主数据编码（旧）")]
        public string? Oid { get; set; }
        /// <summary>
        /// 上级机构主数据编码（旧）
        /// </summary>
        [DisplayName("上级机构主数据编码（旧）")]
        public string? Poid { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [DisplayName("机构规则码")]
        public string? Zorule { get; set; }
        /// <summary>
        /// 所属二级单位编码
        /// </summary>
        [DisplayName("所属二级单位编码")]
        public string? Zgpoid { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [DisplayName("层级")]
        public string? ZoLevel { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        [DisplayName("节点排序号")]
        public string? Zsno { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        [DisplayName("机构编码")]
        public string? Zorgno { get; set; }
        /// <summary>
        /// 机构属性
        /// </summary>
        [DisplayName("机构属性")]
        public string? Zoattr { get; set; }
        /// <summary>
        /// 机构子属性
        /// </summary>
        [DisplayName("机构子属性")]
        public string? Zocattr { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        [DisplayName("国家名称")]
        public string? Zcyname { get; set; }
        /// <summary>
        /// 机构所在地
        /// </summary>
        [DisplayName("机构所在地")]
        public string? Zorgloc { get; set; }
        /// <summary>
        /// 地域属性
        /// </summary>
        [DisplayName("地域属性")]
        public string? Zregional { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        [DisplayName("注册号/统一社会信用代码")]
        public string? Zcuscc { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        [DisplayName("通讯地址")]
        public string? Zaddress { get; set; }
        /// <summary>
        /// 持股情况
        /// </summary>
        [DisplayName("持股情况")]
        public string? Zholding { get; set; }
        /// <summary>
        /// 是否独立核算  是否单独建立核算账套对本机构发生的业务进行会计核算，填写“是”或“否”
        /// </summary>
        [DisplayName("是否独立核算")]
        public string? Zcheckind { get; set; }
        /// <summary>
        /// 机构树形编码 机构树形编码，编码1，代表组织机构
        /// </summary>
        [DisplayName("机构树形编码")]
        public string? Ztreeid1 { get; set; }
        /// <summary>
        /// 多组织树形版本 
        /// </summary>
        [DisplayName("多组织树形版本")]
        public string? Ztreever { get; set; }
        /// <summary>
        /// 视图标识 多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [DisplayName("视图标识")]
        public string? ViewFlag { get; set; }
        /// <summary>
        /// 值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        [DisplayName("状态")]
        public string? Zostate { get; set; }
        /// <summary>
        /// 企业分类,如果多值则按“,”（英文逗号）进行隔开
        /// </summary>
        [DisplayName("企业分类")]
        public string? Zentc { get; set; }
    }
}
