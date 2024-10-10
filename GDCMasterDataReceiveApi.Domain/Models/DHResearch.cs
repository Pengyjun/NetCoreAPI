using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH科研项目
    /// </summary>
    [SugarTable("t_dh_research", IsDisabledDelete = true)]
    public class DHResearch : BaseEntity<long>
    {
        /// <summary>
        /// 科研项目主数据编码 主键
        /// </summary>
        public string? FzsrpCode { get; set; }
        /// <summary>
        /// 上级科研项目主数据编码
        /// </summary>
        public string? FzsrpupCode { get; set; }
        /// <summary>
        /// 科研项目名称
        /// </summary>
        public string? Fzsrpn { get; set; }
        /// <summary>
        /// 科研项目外文
        /// </summary>
        public string? FzsrpnFn { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        public string? FzitOname { get; set; }
        /// <summary>
        /// 科研项目分类
        /// </summary>
        public string? Fzsrpclass { get; set; }
        /// <summary>
        /// 专业类型
        /// </summary>
        public string? Fzmajortype { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string? Fzkpstate { get; set; }
        /// <summary>
        /// 否高新项目 (0:否;1:是)
        /// </summary>
        public string? Fzhitech { get; set; }
        /// <summary>
        /// 是否委外项目 (0:否;1:是)
        /// </summary>
        public string? Fzoutsourcing { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? FzitAi { get; set; }
        /// <summary>
        /// 承担单位
        /// </summary>
        public string? FzitAg { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        public string? FzitAk { get; set; }
        /// <summary>
        /// 参与单位
        /// </summary>
        public string? FzitAh { get; set; }
        /// <summary>
        /// 参与部门
        /// </summary>
        public string? FzitDe { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string? FzitAj { get; set; }
        /// <summary>
        /// 项目总费用
        /// </summary>
        public decimal? Fzprojcost { get; set; }
        /// <summary>
        /// 项目总费用币种
        /// </summary>
        public string? Fzprojcostcur { get; set; }
        /// <summary>
        /// 立项年份
        /// </summary>
        public int Fzprojyear { get; set; }
        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime? Fzpstartdate { get; set; }
        /// <summary>
        /// 计划结束日期
        /// </summary>
        public DateTime? Fzpfindate { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? Fzversion { get; set; }
        /// <summary>
        /// 状态(0:停用;1:启用)
        /// </summary>
        public string? Fzstate { get; set; }
        /// <summary>
        /// 是否删除(0:已删除;1:未删除)
        /// </summary>
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
}
