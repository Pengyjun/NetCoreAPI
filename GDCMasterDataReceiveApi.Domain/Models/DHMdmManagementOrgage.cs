using SqlSugar;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH生产经营管理组织
    /// </summary>
    [SugarTable("t_dh_mdmmanagementorgage", IsDisabledDelete = true)]
    public class DHMdmManagementOrgage : BaseEntity<long>
    {
        /// <summary>
        /// 组织树编码 主键
        /// </summary>
        [DisplayName("")]
        public string? Ztreeid { get; set; }
        /// <summary>
        /// 组织树月度版本
        /// </summary>
        [DisplayName("")]
        public string? Ztreever { get; set; }
        /// <summary>
        /// 快照组织树版本
        /// </summary>
        [DisplayName("")]
        public string? Zshottver { get; set; }
        /// <summary>
        /// 组织树名称
        /// </summary>
        public string? Ztreename { get; set; }
        /// <summary>
        /// 组织树状态 0：审批中，1：审批通过，6：删除
        /// </summary>
        public string? Ztreestat { get; set; }
        /// <summary>
        /// 版本激活日期
        /// </summary>
        public string? ZactiveDate { get; set; }
        /// <summary>
        /// 删除标记 1:删除0：正常
        /// </summary>
        public string? Zdelete { get; set; }
        /// <summary>
        /// 定版标识
        /// </summary>
        public string? Zfixed { get; set; }
        /// <summary>
        /// 定版人
        /// </summary>
        public string? ZfixedBy { get; set; }
        /// <summary>
        /// 定版日期
        /// </summary>
        public string? ZfixedDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Zremark { get; set; }
        /// <summary>
        /// 财务核算机构主数据编码  主键
        /// </summary>
        public string? Zaco { get; set; }
        /// <summary>
        /// 核算机构状态 值域：0-停用、1-启用、2-休眠、3-删除
        /// </summary>
        public string? Zyorgstate { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? MdmCode { get; set; }
        /// <summary>
        /// 机构名称（英文
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
        /// 核算组织简称-中文
        /// </summary>
        public string? ZztshnameLoc { get; set; }
        /// <summary>
        /// 核算组织简称-英文
        /// </summary>
        public string? ZacshortnameChs { get; set; }
        /// <summary>
        /// 核算组织简称-当地语言
        /// </summary>
        public string? ZacshortnameEn { get; set; }
        /// <summary>
        /// 是否投资项目/公司
        /// </summary>
        public string? ZacshortnameLoc { get; set; }
        /// <summary>
        /// 是否集团合并范围内单位
        /// </summary>
        public string? Zivflgid { get; set; }
        /// <summary>
        /// 是否启用报表
        /// </summary>
        public string? Zacjthbfwn { get; set; }
        /// <summary>
        /// 转入不启用报表时间
        /// </summary>
        public string? ZreportFl { get; set; }
        /// <summary>
        /// 报表节点性质
        /// </summary>
        public string? ZreportTi { get; set; }
        /// <summary>
        /// 核算机构状态
        /// </summary>
        public string? ZreportNo { get; set; }
        /// <summary>
        /// 停用年度
        /// </summary>
        public string? Zyorgstat { get; set; }
        /// <summary>
        /// 建账时间
        /// </summary>
        public string? ZdeaYear { get; set; }
        /// <summary>
        /// 停用年度
        /// </summary>
        public string? ZaccountD { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string? Zcountrycode { get; set; }
        /// <summary>
        /// 机构所在地
        /// </summary>
        public string? Zaddvscode { get; set; }
        /// <summary>
        /// 区域属性
        /// </summary>
        public string? Zowarid { get; set; }
        /// <summary>
        /// 决算业务分类代码
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
        public string? Ztaxmetho { get; set; }
        /// <summary>
        /// 纳税人类别
        /// </summary>
        public string? ZaxpayerC { get; set; }
        /// <summary>
        /// 所属税务组织
        /// </summary>
        public string? ZtaxOrgan { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        public string? Zbusiness { get; set; }
        /// <summary>
        /// 管理机构状态
        /// </summary>
        public string? Zmgorgstat { get; set; }
        /// <summary>
        /// 生产经营管理层级
        /// </summary>
        public string? ZtreeLevel { get; set; }
        /// <summary>
        /// 机构性质
        /// </summary>
        public string? ZorgProp { get; set; }
        /// <summary>
        /// 是否为工商撤销机构
        /// </summary>
        public string? BusinessRecocation { get; set; }
        /// <summary>
        /// 视图标识 多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        public string? ViewFlag { get; set; }
        /// <summary>
        /// 上级维度组织编码
        /// </summary>
        public string? Znodeup { get; set; }
        /// <summary>
        /// 是否激活 0:未激活，1:激活
        /// </summary>
        public string? Znodestat { get; set; }
        /// <summary>
        /// 是否存在行政组织
        /// </summary>
        public string? Zisorg { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? Zorg { get; set; }
        /// <summary>
        /// （备注）
        /// </summary>
        public string? Zreason { get; set; }
        /// <summary>
        /// 上级机构主数据编码
        /// </summary>
        public string? Zorgup1 { get; set; }
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
        public string? Zaclayer { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        public string? Zsortor { get; set; }
        /// <summary>
        /// 核算组织中文名称
        /// </summary>
        public string? Zaconame { get; set; }
        /// <summary>
        /// 核算理组织英文名称
        /// </summary>
        public string? ZacnameEn { get; set; }
        /// <summary>
        /// 核算组织当地语言名称
        /// </summary>
        public string? ZacnameLoc { get; set; }
    }
}
