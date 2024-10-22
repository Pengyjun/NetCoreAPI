using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// 科研
    /// </summary>
    public class DHResearchDto
    {
        /// <summary>
        /// 科研项目主数据编码 主键
        /// </summary>
        [DisplayName("科研项目主数据编码")]
        public string? FzsrpCode { get; set; }
        /// <summary>
        /// 上级科研项目主数据编码
        /// </summary>
        [DisplayName("上级科研项目主数据编码")]
        public string? FzsrpupCode { get; set; }
        /// <summary>
        /// 科研项目名称
        /// </summary>
        [DisplayName("科研项目名称")]
        public string? Fzsrpn { get; set; }
        /// <summary>
        /// 科研项目外文
        /// </summary>
        [DisplayName("科研项目外文")]
        public string? FzsrpnFn { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        [DisplayName("曾用名")]
        public string? FzitOname { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        [DisplayName("曾用名")]
        public List<FzitOname>? FzitOnameList { get; set; }
        /// <summary>
        /// 科研项目分类
        /// </summary>
        [DisplayName("科研项目分类")]
        public string? Fzsrpclass { get; set; }
        /// <summary>
        /// 专业类型
        /// </summary>
        [DisplayName("专业类型")]
        public string? Fzmajortype { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [DisplayName("项目状态")]
        public string? Fzkpstate { get; set; }
        /// <summary>
        /// 否高新项目 (0:否;1:是)
        /// </summary>
        [DisplayName("否高新项目")]
        public string? Fzhitech { get; set; }
        /// <summary>
        /// 是否委外项目 (0:否;1:是)
        /// </summary>
        [DisplayName("是否委外项目")]
        public string? Fzoutsourcing { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [DisplayName("所属二级单位")]
        public string? FzitAi { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [DisplayName("所属二级单位")]
        public List<FzitAi>? FzitAiList { get; set; }
        /// <summary>
        /// 承担单位
        /// </summary>
        [DisplayName("承担单位")]
        public string? FzitAg { get; set; }
        /// <summary>
        /// 承担单位
        /// </summary>
        [DisplayName("承担单位")]
        public List<FzitAg>? FzitAgList { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        [DisplayName("委托单位")]
        public string? FzitAk { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        [DisplayName("委托单位")]
        public List<FzitAk>? FzitAkList { get; set; }
        /// <summary>
        /// 参与单位
        /// </summary>
        [DisplayName("参与单位")]
        public string? FzitAh { get; set; }
        /// <summary>
        /// 参与单位
        /// </summary>
        [DisplayName("参与单位")]
        public List<FzitAh>? FzitAhList { get; set; }
        /// <summary>
        /// 参与部门
        /// </summary>
        [DisplayName("参与部门")]
        public string? FzitDe { get; set; }
        /// <summary>
        /// 参与部门
        /// </summary>
        [DisplayName("参与部门")]
        public List<FzitDe>? FzitDeList { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        [DisplayName("项目负责人")]
        public string? FzitAj { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        [DisplayName("项目负责人")]
        public List<FzitAj>? FzitAjList { get; set; }
        /// <summary>
        /// 项目总费用
        /// </summary>
        [DisplayName("项目总费用")]
        public decimal? Fzprojcost { get; set; }
        /// <summary>
        /// 项目总费用币种
        /// </summary>
        [DisplayName("项目总费用币种")]
        public string? Fzprojcostcur { get; set; }
        /// <summary>
        /// 立项年份
        /// </summary>
        [DisplayName("立项年份")]
        public int Fzprojyear { get; set; }
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [DisplayName("计划开始日期")]
        public DateTime? Fzpstartdate { get; set; }
        /// <summary>
        /// 计划结束日期
        /// </summary>
        [DisplayName("计划结束日期")]
        public DateTime? Fzpfindate { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [DisplayName("版本")]
        public string? Fzversion { get; set; }
        /// <summary>
        /// 状态(0:停用;1:启用)
        /// </summary>
        [DisplayName("状态")]
        public string? Fzstate { get; set; }
        /// <summary>
        /// 是否删除(0:已删除;1:未删除)
        /// </summary>
        [DisplayName("是否删除")]
        public string? Fzdelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
    /// <summary>
    /// 所属二级单位
    /// </summary>
    public class FzitAi
    {
        /// <summary>
        /// 所属二级单位名称
        /// </summary>
        public string? Z2NDORGN { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? Z2NDORG { get; set; }
    }
    /// <summary>
    /// 承担单位
    /// </summary>
    public class FzitAg
    {
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        public string? ZIOSIDE { get; set; }
        /// <summary>
        /// 承担单位名称
        /// </summary>
        public string? ZUDTKN { get; set; }
        /// <summary>
        /// 承担单位
        /// </summary>
        public string? ZUDTK { get; set; }

    }
    /// <summary>
    /// 委托单位
    /// </summary>
    public class FzitAk
    {
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        public string? ZIOSIDE { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        public string? ZAUTHORISE { get; set; }
        /// <summary>
        /// 委托单位名称
        /// </summary>
        public string? ZAUTHORISEN { get; set; }

    }
    /// <summary>
    /// 参与单位
    /// </summary>
    public class FzitAh
    {
        /// <summary>
        /// 参与单位
        /// </summary>
        public string? ZPU { get; set; }
        /// <summary>
        /// 内部/外部 :1 内部/2 外部
        /// </summary>
        public string? ZIOSIDE { get; set; }
        /// <summary>
        /// 参与单位名称
        /// </summary>
        public string? ZPUN { get; set; }

    }
    /// <summary>
    /// 参与部门
    /// </summary>
    public class FzitDe
    {
        /// <summary>
        /// 参与部门名称
        /// </summary>
        public string? ZKZDEPARTNM { get; set; }
        /// <summary>
        /// 参与部门编号
        /// </summary>
        public string? ZKZDEPART { get; set; }

    }
    /// <summary>
    /// 项目负责人
    /// </summary>
    public class FzitAj
    {
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string? ZPRINCIPAL { get; set; }
        /// <summary>
        /// 项目负责人名称
        /// </summary>
        public string? ZPRINCIPALN { get; set; }
    }
    /// <summary>
    /// 曾用名
    /// </summary>
    public class FzitOname
    {
        /// <summary>
        /// 行项目编号
        /// </summary>
        public string? ZITEM { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        public string? ZOLDNAME { get; set; }
    }
}
