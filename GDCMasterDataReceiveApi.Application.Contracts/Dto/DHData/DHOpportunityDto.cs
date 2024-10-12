namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// 商机项目
    /// </summary>
    public class DHOpportunityDto
    {
        /// <summary>
        /// 商机项目主数据编码
        /// </summary>
        public string? ZBOP { get; set; }
        /// <summary>
        /// 商机项目名称
        /// </summary>
        public string? ZBOPN { get; set; }
        /// <summary>
        /// 商机项目外文名称
        /// </summary>
        public string? ZBOPN_EN { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ZPROJTYPE { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? ZCPBC { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        public string? ZPROJLOC { get; set; }
        /// <summary>
        /// 开始跟踪日期
        /// </summary>
        public DateTime? ZSFOLDATE { get; set; }
        /// <summary>
        /// 跟踪单位
        /// </summary>
        public string? ZORG { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 状态 0:停用;1:启用
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 资质单位
        /// </summary>
        public string? ZORG_QUAL { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 是否删除 0:已删除;1:未删除
        /// </summary>
        public string? Zdelete { get; set; }
        /// <summary>
        /// 参与单位 填写参与部门的行政机构主数据编码，可多值，用英文逗号隔开
        /// </summary>
        public string? ZCY2NDORG { get; set; }
        /// <summary>
        /// 中标交底项目字符串
        /// </summary>
        public string? ZAWARDP_LIST { get; set; }
        /// <summary>
        /// 中标交底项目
        /// </summary>
        public List<DHAwardpList>? DHAwardpList { get; set; }
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
    /// 商机项目关联的中标交底项目
    /// </summary>
    public class DHAwardpList
    {
        /// <summary>
        /// 项目所在地 境内项目必填
        /// </summary>
        public string? ZSPROJLOC { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ZAWARDPN { get; set; }
        /// <summary>
        /// 中标交底项目编码
        /// </summary>
        public string? ZAWARDP { get; set; }
        /// <summary>
        /// 是否联合体总项目 0-否，1-是
        /// </summary>
        public string? ZSZCONTPRO { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ZSPROTYPE { get; set; }
        /// <summary>
        /// 交底项目二级单位
        /// </summary>
        public string? ZAP2NDORG { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? ZSCPBC { get; set; }
        /// <summary>
        /// 中标资质单位
        /// </summary>
        public string? ZAWARDORG { get; set; }
        /// <summary>
        /// 状态 有效：1无效：0
        /// </summary>
        public string? ZSSTATE { get; set; }
    }
}
