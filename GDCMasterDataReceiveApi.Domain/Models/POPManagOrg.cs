using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 生产经营管理组织
    /// </summary>
    [SugarTable("t_popmanagorg", IsDisabledDelete = true)]
    public class POPManagOrg : BaseEntity<long>
    {
        /// <summary>
        /// 组织树编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树月度版本
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 快照组织树版本
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZSHOTTVER { get; set; }
        /// <summary>
        /// 组织树名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZTREENAME { get; set; }
        /// <summary>
        /// 组织树状态 0：审批中，1：审批通过，6：删除
        /// </summary>
        [SugarColumn(Length = 10)]
        public string? ZTREESTAT { get; set; }
        /// <summary>
        /// 版本激活日期
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZACTIVE_DATE { get; set; }
        /// <summary>
        /// 删除标记 1:删除0：正常　
        /// </summary>
        [SugarColumn(Length = 10)]
        public string? ZDELETE { get; set; }
    }

    /// <summary>
    /// 生产经营管理组织  组织树明细
    /// </summary>
    [SugarTable("t_poptreedetails", IsDisabledDelete = true)]
    public class POPTreeDetails:BaseEntity<long>
    {
        /// <summary>
        /// 财务核算机构主数据编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZACO { get; set; }
        /// <summary>
        /// 核算机构状态
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZYORGSTATE { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// （备注）
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ZREASON { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZSORTOR { get; set; }
        /// <summary>
        /// 核算组织中文名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZACONAME { get; set; }
        /// <summary>
        /// 删除标记	1:删除0：正常　
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 是否投资项目/公司 1:是，0:否
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZIVFLGID { get; set; }
        /// <summary>
        /// 是否集团合并范围内单位1:是，0:否
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZACJTHBFWN { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZCOUNTRYCODE { get; set; }
        /// <summary>
        /// 国家中文名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZCOUNTRYNAME { get; set; }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZADDVSCODE { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZADDVSNAME { get; set; }
        /// <summary>
        /// 区域属性
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZOWARID { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZOWARNAME { get; set; }
        /// <summary>
        /// 决算业务分类代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZDCID { get; set; }
        /// <summary>
        /// 决算业务分类名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZDCNAME { get; set; }
        /// <summary>
        /// 业务分类
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZBTID { get; set; }
        /// <summary>
        /// 生产经营管理层级
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZTREE_LEVEL { get; set; }
        /// <summary>
        /// 机构性质
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZORG_PROP { get; set; }
        /// <summary>
        /// 上级维度组织编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZNODEUP { get; set; }
        /// <summary>
        /// 是否激活 0:未激活，1:激活
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZNODESTAT { get; set; }
        /// <summary>
        /// 是否存在行政组织
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZISORG { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZORG { get; set; }
        /// <summary>
        /// 上级维度组织名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZNODEUPNM { get; set; }
        /// <summary>
        /// 是否久其出表 0：否。1：是
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ZACJQCB { get; set; }
    }
}
