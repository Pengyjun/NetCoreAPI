namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 生产经营管理组织其他models
    /// </summary>
    public class POPManagOrgModels
    {
        public List<ZACO_LIST>? Item { get; set; }
    }

    public class ZACO_LIST
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 财务核算机构主数据编码
        /// </summary>
        public string? ZACO { get; set; }
        /// <summary>
        /// 核算机构状态
        /// </summary>
        public string? ZYORGSTATE { get; set; }
        /// <summary>
        /// 组织树编码
        /// </summary>
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// （备注）
        /// </summary>
        public string? ZREASON { get; set; }
        /// <summary>
        /// 节点排序号
        /// </summary>
        public string? ZSORTOR { get; set; }
        /// <summary>
        /// 核算组织中文名称
        /// </summary>
        public string? ZACONAME { get; set; }
        /// <summary>
        /// 删除标记	1:删除0：正常　
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 是否投资项目/公司 1:是，0:否
        /// </summary>
        public string? ZIVFLGID { get; set; }
        /// <summary>
        /// 是否集团合并范围内单位1:是，0:否
        /// </summary>
        public string? ZACJTHBFWN { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        public string? ZCOUNTRYCODE { get; set; }
        /// <summary>
        /// 国家中文名称
        /// </summary>
        public string? ZCOUNTRYNAME { get; set; }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        public string? ZADDVSCODE { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string? ZADDVSNAME { get; set; }
        /// <summary>
        /// 区域属性
        /// </summary>
        public string? ZOWARID { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        public string? ZOWARNAME { get; set; }
        /// <summary>
        /// 决算业务分类代码
        /// </summary>
        public string? ZDCID { get; set; }
        /// <summary>
        /// 决算业务分类名称
        /// </summary>
        public string? ZDCNAME { get; set; }
        /// <summary>
        /// 业务分类
        /// </summary>
        public string? ZBTID { get; set; }
        /// <summary>
        /// 生产经营管理层级
        /// </summary>
        public string? ZTREE_LEVEL { get; set; }
        /// <summary>
        /// 机构性质
        /// </summary>
        public string? ZORG_PROP { get; set; }
        /// <summary>
        /// 上级维度组织编码
        /// </summary>
        public string? ZNODEUP { get; set; }
        /// <summary>
        /// 是否激活 0:未激活，1:激活
        /// </summary>
        public string? ZNODESTAT { get; set; }
        /// <summary>
        /// 是否存在行政组织
        /// </summary>
        public string? ZISORG { get; set; }
        /// <summary>
        /// 机构主数据编码
        /// </summary>
        public string? ZORG { get; set; }
        /// <summary>
        /// 上级维度组织名称
        /// </summary>
        public string? ZNODEUPNM { get; set; }
        /// <summary>
        /// 是否久其出表 0：否。1：是
        /// </summary>
        public string? ZACJQCB { get; set; }
    }
}
