using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH项目信息
    /// </summary>
    [SugarTable("t_dh_tprojects", IsDisabledDelete = true)]
    public class DHtProjects : BaseEntity<long>
    {
        /// <summary>
        /// 项目主数据编码 主键
        /// </summary>
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ZPROJNAME { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        public string? ZPROJENAME { get; set; }
        /// <summary>
        /// 项目GUID
        /// </summary>
        public string? ZPROJECTID { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ZPROJTYPE { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        public string? ZPROJLOC { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? ZCPBC { get; set; }
        /// <summary>
        /// 投资主体
        /// </summary>
        public string? ZINVERSTOR { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        public string? FzitOname { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<FzitOnames>? FzitOnameList { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        public string? ZAPPROVAL { get; set; }
        /// <summary>
        /// 项目批复/决议时间
        /// </summary>
        public DateTime? ZAPVLDATE { get; set; }
        /// <summary>
        /// 收入来源
        /// </summary>
        public string? ZSI { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        public string? ZPRO_ORG { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        public string? ZHEREINAFTER { get; set; }
        /// <summary>
        /// 上级项目主数据编码
        /// </summary>
        public string? ZPROJECTUP { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        public string? ZPROJYEAR { get; set; }
        /// <summary>
        /// 项目计划开始日期
        /// </summary>
        public DateTime? ZSTARTDATE { get; set; }
        /// <summary>
        /// 项目计划完成日期
        /// </summary>
        public DateTime? ZFINDATE { get; set; }
        /// <summary>
        /// 项目地块名称
        /// </summary>
        public string? ZBLOCK { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        public string? ZENG { get; set; }
        /// <summary>
        /// 责任主体
        /// </summary>
        public string? ZRESP { get; set; }
        /// <summary>
        /// 土地成交确认书编号
        /// </summary>
        public string? ZLDLOC { get; set; }
        /// <summary>
        /// 土地成交确认书获取时间
        /// </summary>
        public DateTime? ZLDLOCGT { get; set; }
        /// <summary>
        /// 工商变更时间
        /// </summary>
        public DateTime? ZCBR { get; set; }
        /// <summary>
        /// 操盘情况
        /// </summary>
        public string? ZTRADER { get; set; }
        /// <summary>
        /// 保险机构名称
        /// </summary>
        public string? ZINSURANCE { get; set; }
        /// <summary>
        /// 保单号
        /// </summary>
        public string? ZPOLICYNO { get; set; }
        /// <summary>
        /// 投保人
        /// </summary>
        public string? ZINSURED { get; set; }
        /// <summary>
        /// 保险起始日期
        /// </summary>
        public DateTime? ZISTARTDATE { get; set; }
        /// <summary>
        /// 保险终止日期
        /// </summary>
        public DateTime? ZIFINDATE { get; set; }
        /// <summary>
        /// 基金主数据编码
        /// </summary>
        public string? ZFUND { get; set; }
        /// <summary>
        /// 基金名称
        /// </summary>
        public string? ZFUNDNAME { get; set; }
        /// <summary>
        /// 基金编号
        /// </summary>
        public string? ZFUNDNO { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string? ZZCURRENCY { get; set; }
        /// <summary>
        /// 基金组织形式
        /// </summary>
        public string? ZFUNDORGFORM { get; set; }
        /// <summary>
        /// 商机项目机构
        /// </summary>
        public string? ZPRO_BP { get; set; }
        /// <summary>
        /// 基金管理人类型
        /// </summary>
        public string? ZFUNDMTYPE { get; set; }
        /// <summary>
        /// 基金成立日期
        /// </summary>
        public DateTime? ZFSTARTDATE { get; set; }
        /// <summary>
        /// 基金到期日期
        /// </summary>
        public DateTime? ZFFINDATE { get; set; }
        /// <summary>
        /// 托管人名称
        /// </summary>
        public string? ZCUSTODIAN { get; set; }
        /// <summary>
        /// 承租人名称
        /// </summary>
        public string? ZLESSEE { get; set; }
        /// <summary>
        /// 承租人类型
        /// </summary>
        public string? ZLESSEETYPE { get; set; }
        /// <summary>
        /// 租赁物名称
        /// </summary>
        public string? ZLEASESNAME { get; set; }
        /// <summary>
        /// 起租日期
        /// </summary>
        public DateTime? ZLSTARTDATE { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime? ZLFINDATE { get; set; }
        /// <summary>
        /// 曾用名列表
        /// </summary>
        public string? ZOLDNAME { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 停用原因
        /// </summary>
        public string? ZSTOPREASON { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 项目组织形式
        /// </summary>
        public string? ZPOS { get; set; }
        /// <summary>
        /// 中标主体
        /// </summary>
        public string? ZAWARDMAI { get; set; }
        /// <summary>
        /// 并表情况
        /// </summary>
        public string? ZCS { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        public string? ZBIZDEPT { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? Zversion { get; set; }
        /// <summary>
        /// 状态 0:停用;1:启用
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 0:已删除;1:未删除
        /// </summary>
        public string? Zdelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// 项目管理方式
        /// </summary>
        public string? FZmanagemode { get; set; }
        /// <summary>
        /// 参与二级单位，该项目参与的其他二级单位，支持多值
        /// </summary>
        public string? FZcy2ndorg { get; set; }
        /// <summary>
        /// 是否联合体 是否联合体：1是，2否
        /// </summary>
        public string? FZwinningc { get; set; }
        /// <summary>
        /// 中标交底项目 中标交底项目编号，传入多值时用逗号给开
        /// </summary>
        public string? FZawardp { get; set; }

    }
    /// <summary>
    /// 曾用名
    /// </summary>
    public class FzitOnames
    {
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        public string? ZOLDNAME { get; set; }
    }
}
