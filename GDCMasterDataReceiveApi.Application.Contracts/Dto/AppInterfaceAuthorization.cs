using GDCMasterDataReceiveApi.Domain;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 系统授权表
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_appsystemauthorization", IsDisabledDelete = true)]
    public class AppSystemAuthorization 
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 系统简称标识
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? SystemIdentity { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? CompanyName { get; set; }
        /// <summary>
        /// 系统标识名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? SystemIdentityName { get; set; }

        /// <summary>
        /// 系统授权码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? AppKey { get; set; }

        /// <summary>
        /// 从哪个系统调用接口关联ID
        /// </summary>
        public long? SystemApiId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? Remark { get; set; }
    }
    /// <summary>
    /// 接口授权表
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_appinterfaceauthorization")]
    public class AppInterfaceAuthorization
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 应用系统主键   关联关系
        /// </summary>
        [SugarColumn(Length = 64)]
        public long? AppSystemId { get; set; }
        /// <summary>
        /// 应用系统接口主键   关联关系
        /// </summary>
        [SugarColumn(Length = 64)]
        public long? SystemInterfaceId { get; set; }
        /// <summary>
        /// 接口函数码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? AppInterfaceCode { get; set; }
        /// <summary>
        /// 标识一个接口返回是否加密处理   0 是不加密  1是加密处理  前端 以及下游需要解密
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 16)]
        public int? ReturnDataEncrypt { get; set; }
        /// <summary>
        /// 接口访问IP限制
        /// </summary>
        [SugarColumn(Length = 512, DefaultValue = "*")]
        public string? AccessRestrictedIP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? Remark { get; set; }
    }
    /// <summary>
    /// 展示   不做表逻辑处理
    /// </summary>
    public class SearchDataDesensitizationRule
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 所属系统ID
        /// </summary>
        public string? AppSystemInterfaceId { get; set; }
        /// <summary>
        /// 系统接口字段名称
        /// </summary>
        public string? FieidName { get; set; }
        /// <summary>
        /// 系统接口字段中文名称
        /// </summary>
        public string? FieidZHName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
    /// <summary>
    /// 接口授权表
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_systeminterfacefield")]
    public class SystemInterfaceField
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 所属系统ID
        /// </summary>
        public long? AppSystemInterfaceId { get; set; }
        /// <summary>
        /// 系统接口字段名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? FieidName { get; set; }
        /// <summary>
        /// 系统接口字段中文名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? FieidZHName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? Remark { get; set; }
        /// <summary>
        /// 是否启用   0 未启用  1 启用
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public int? Enable { get; set; }

    }

    /// <summary>
    /// 数据脱敏规则表
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_datadesensitizationrule", IsDisabledDelete = true)]
    public class DataDesensitizationRule 
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id {  set; get; }
        /// <summary>
        /// 外部系统ID
        /// </summary>
        public long? AppSystemApiId { get; set; }
        /// <summary>
        /// 应用系统接口字段 主键ID  关联关系
        /// </summary>
        [SugarColumn(Length = 64)]
        public long AppSystemInterfaceFieIdId { get; set; }
        /// <summary>
        /// 起始位置
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int? StartIndex { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int? EndIndex { get; set; }

        /// <summary>
        /// 是否启用   0 未启用  1 启用
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public int? Enable { get; set; }
        /// <summary>
        /// 数据脱敏类型   input  是截取    replace是替换
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public DataDesensitizationType DesensitizationType { get; set; }

    }
    /// <summary>
    /// 系统接口
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_systeminterface", IsDisabledDelete = true)]
    public class SystemInterface : BaseEntity<long>
    {
        public long Id {  get; set; }
        /// <summary>
        /// 所属系统ID
        /// </summary>

        public long? SystemApiId { get; set; }
        /// <summary>
        /// 系统接口名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? InterfaceName { get; set; }
        /// <summary>
        /// 系统接口中文名称
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? InterfaceZHName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? Remark { get; set; }
        /// <summary>
        /// 是否启用   0 未启用  1 启用
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public int? Enable { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DataDesensitizationType
    {
        /// <summary>
        /// 截取类型
        /// </summary>
        Input = 1,
        /// <summary>
        /// 文本替换类型
        /// </summary>
        Replace = 2
    }

    ///<summary>
    ///应付票据表
    ///</summary>
    [Tenant("finance")]
    [SugarTable("dwd_ffm_ap_bill_d", IsDisabledDelete = true)]
    public partial class dwd_ffm_ap_bill_d
    {
        public dwd_ffm_ap_bill_d()
        {


        }
        /// <summary>
        /// Desc:票据内码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_id { get; set; }

        /// <summary>
        /// Desc:票据编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_cd { get; set; }

        /// <summary>
        /// Desc:来源单据编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string src_bill_cd { get; set; }

        /// <summary>
        /// Desc:开票单位机构主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ivc_org_cd { get; set; }

        /// <summary>
        /// Desc:开票所属二级单位机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ivc_z2nd_org_id { get; set; }

        /// <summary>
        /// Desc:开票所属三级单位机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ivc_z3nd_org_id { get; set; }

        /// <summary>
        /// Desc:用票单位机构主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string utkt_org_cd { get; set; }

        /// <summary>
        /// Desc:用票所属二级单位机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string utkt_z2nd_org_id { get; set; }

        /// <summary>
        /// Desc:用票所属三级单位机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string utkt_z3nd_org_id { get; set; }

        /// <summary>
        /// Desc:开票单位资金组织编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ivc_fund_org_cd { get; set; }

        /// <summary>
        /// Desc:开票方式编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ivc_meth_cd { get; set; }

        /// <summary>
        /// Desc:票据类型编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_typ_cd { get; set; }

        /// <summary>
        /// Desc:票据形式编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_form_cd { get; set; }

        /// <summary>
        /// Desc:票据状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_sta_cd { get; set; }

        /// <summary>
        /// Desc:票据金额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? bill_amt { get; set; }

        /// <summary>
        /// Desc:到期日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? due_dt { get; set; }

        /// <summary>
        /// Desc:出票日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? out_tkt_dt { get; set; }

        /// <summary>
        /// Desc:出票账户编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string out_tkt_acc_cd { get; set; }

        /// <summary>
        /// Desc:出票行金融机构主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string out_tkt_bk_acc_cd { get; set; }

        /// <summary>
        /// Desc:出票行联行号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string out_tkt_bk_no { get; set; }

        /// <summary>
        /// Desc:出票人全称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string out_tkt_nm { get; set; }

        /// <summary>
        /// Desc:收款人账户编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string pyee_acc_cd { get; set; }

        /// <summary>
        /// Desc:收款行金融机构主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string pyee_bank_acc_cd { get; set; }

        /// <summary>
        /// Desc:收款人开户行联行号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string pyee_bank_no { get; set; }

        /// <summary>
        /// Desc:收款人全称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string pyee_nm { get; set; }

        /// <summary>
        /// Desc:承兑人全称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string acpt_nm { get; set; }

        /// <summary>
        /// Desc:结算编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string stl_cd { get; set; }

        /// <summary>
        /// Desc:结算类型编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string stl_typ_cd { get; set; }

        /// <summary>
        /// Desc:警告金额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? warn_amt { get; set; }

        /// <summary>
        /// Desc:警告金额账户编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string warn_amt_acc_cd { get; set; }

        /// <summary>
        /// Desc:警告金额率
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? warn_amt_rt { get; set; }

        /// <summary>
        /// Desc:银行警告金额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? bk_warn_amt { get; set; }

        /// <summary>
        /// Desc:银行警告金额率
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? bk_warn_amt_rt { get; set; }

        /// <summary>
        /// Desc:内部警告金额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? inr_warn_amt { get; set; }

        /// <summary>
        /// Desc:内部现金账户编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string inr_cash_acc_cd { get; set; }

        /// <summary>
        /// Desc:内部警告金额率
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? inr_warn_amt_rt { get; set; }

        /// <summary>
        /// Desc:银行提交信息
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bk_info { get; set; }

        /// <summary>
        /// Desc:贷方协议编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string cr_prot_cd { get; set; }

        /// <summary>
        /// Desc:贷方限额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? cr_lim { get; set; }

        /// <summary>
        /// Desc:是否待签
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_wait_sign { get; set; }

        /// <summary>
        /// Desc:到息状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string due_dt_sta_cd { get; set; }

        /// <summary>
        /// Desc:是否直联
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_dir_conn { get; set; }

        /// <summary>
        /// Desc:后手可转让
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string retransf { get; set; }

        /// <summary>
        /// Desc:兑付日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? cash_dt { get; set; }

        /// <summary>
        /// Desc:提示承兑状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string pmpt_acpt_sta_cd { get; set; }

        /// <summary>
        /// Desc:提示收票状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string pmpt_coll_tkt_sta_cd { get; set; }

        /// <summary>
        /// Desc:撤销状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string rvk_sta_cd { get; set; }

        /// <summary>
        /// Desc:撤票状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ccltn_tkt_sta_cd { get; set; }

        /// <summary>
        /// Desc:业务单据编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string biz_bill_cd { get; set; }

        /// <summary>
        /// Desc:供应链产品编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sc_prod_cd { get; set; }

        /// <summary>
        /// Desc:期限
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? term { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string rmk { get; set; }

        /// <summary>
        /// Desc:禁用标识（0 启用 1 禁用）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string dis_flg { get; set; }

        /// <summary>
        /// Desc:禁用时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? dis_tm { get; set; }

        /// <summary>
        /// Desc:创建人员
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string cre_by { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? cre_tm { get; set; }

        /// <summary>
        /// Desc:修改人员
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string mod_by { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? mod_tm { get; set; }

        /// <summary>
        /// Desc:ETL加载时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? etl_time { get; set; }

        /// <summary>
        /// Desc:来源系统
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string source_system { get; set; }

        /// <summary>
        /// Desc:来源表名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string source_table { get; set; }

        /// <summary>
        /// Desc:批次日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string batch_date { get; set; }

        /// <summary>
        /// Desc:
        /// Default:CURRENT_TIMESTAMP
        /// Nullable:True
        /// </summary>           
        public DateTime? insertdate { get; set; }

    }
    ///<summary>
    ///应收票据表
    ///</summary>
    [Tenant("finance")]
    [SugarTable("dwd_ffm_receive_new_d", IsDisabledDelete = true)]
    public partial class dwd_ffm_receive_new_d
    {
        public dwd_ffm_receive_new_d()
        {


        }
        /// <summary>
        /// Desc:票据数据内码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_data_id { get; set; }

        /// <summary>
        /// Desc:票据数据编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_data_cd { get; set; }

        /// <summary>
        /// Desc:核算组织编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string accti_org_cd { get; set; }

        /// <summary>
        /// Desc:所属二级单位机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string z2nd_org_id { get; set; }

        /// <summary>
        /// Desc:所属二级单位机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string z3nd_org_id { get; set; }

        /// <summary>
        /// Desc:批次编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string batch_cd { get; set; }

        /// <summary>
        /// Desc:年度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string year { get; set; }

        /// <summary>
        /// Desc:月份
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string month { get; set; }

        /// <summary>
        /// Desc:票据状态编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string bill_sta_cd { get; set; }

        /// <summary>
        /// Desc:是否附追索权编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string reoue_cd { get; set; }

        /// <summary>
        /// Desc:贴现金额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? dcout_amt { get; set; }

        /// <summary>
        /// Desc:贴现率%
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? dcout_rate { get; set; }

        /// <summary>
        /// Desc:贴现利息
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? dcout_int { get; set; }

        /// <summary>
        /// Desc:禁用标识（0 启用 1 禁用）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string dis_flg { get; set; }

        /// <summary>
        /// Desc:禁用时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? dis_tm { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? cre_tm { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? mod_tm { get; set; }

        /// <summary>
        /// Desc:ETL加载时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? etl_time { get; set; }

        /// <summary>
        /// Desc:来源系统
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string source_system { get; set; }

        /// <summary>
        /// Desc:来源表名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string source_table { get; set; }

        /// <summary>
        /// Desc:批次日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string batch_date { get; set; }

    }

    /// <summary>
    /// 合同基本信息表
    /// </summary>
    [Tenant("finance")]
    [SugarTable("dwd_ffm_receive_new_d", IsDisabledDelete = true)]
    public partial class dwd_cdm_con_bas_i_d
    {
        public dwd_cdm_con_bas_i_d()
        {


        }
        /// <summary>
        /// Desc:合同自增主键id
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int my_id { get; set; }

        /// <summary>
        /// Desc:合同主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_main_code { get; set; }

        /// <summary>
        /// Desc:合同名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_name { get; set; }

        /// <summary>
        /// Desc:合同名称中文
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_name_cn { get; set; }

        /// <summary>
        /// Desc:原系统合同编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_code { get; set; }

        /// <summary>
        /// Desc:是否补充合同
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_supply_contract { get; set; }

        /// <summary>
        /// Desc:补充合同所属原合同主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sup_con_main_code { get; set; }

        /// <summary>
        /// Desc:对方合同主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string opposite_con_main_code { get; set; }

        /// <summary>
        /// Desc:项目主数据编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string proj_number { get; set; }

        /// <summary>
        /// Desc:项目名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string proj_name { get; set; }

        /// <summary>
        /// Desc:收支类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string income_expend_type { get; set; }

        /// <summary>
        /// Desc:合同业务类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string business_type { get; set; }

        /// <summary>
        /// Desc:合同履行地（国家）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_perform_country { get; set; }

        /// <summary>
        /// Desc:合同履行地（省）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_perform_provinces { get; set; }

        /// <summary>
        /// Desc:合同履行地（市）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_perform_city { get; set; }

        /// <summary>
        /// Desc:合同履行地（县）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_perform_xian { get; set; }

        /// <summary>
        /// Desc:合同承办人ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_undertaker_id { get; set; }

        /// <summary>
        /// Desc:合同承办人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_undertaker { get; set; }

        /// <summary>
        /// Desc:合同承办人联系方式
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_undertaker_contact { get; set; }

        /// <summary>
        /// Desc:合同语言
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_language { get; set; }

        /// <summary>
        /// Desc:合同语言编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string conc_language_code { get; set; }

        /// <summary>
        /// Desc:签约日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? sign_date { get; set; }

        /// <summary>
        /// Desc:履约进度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string perform_schedule { get; set; }

        /// <summary>
        /// Desc:履约阶段
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string perform_stage { get; set; }

        /// <summary>
        /// Desc:合同概述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_summary { get; set; }

        /// <summary>
        /// Desc:合同税率
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? tax_rate { get; set; }

        /// <summary>
        /// Desc:合同税额
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? tax_amt { get; set; }

        /// <summary>
        /// Desc:合同总金额币种
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string currency { get; set; }

        /// <summary>
        /// Desc:合同总价
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? tol_con_price_exd_tax { get; set; }

        /// <summary>
        /// Desc:合同总金额（含税）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? hs_amt { get; set; }

        /// <summary>
        /// Desc:不适用说明(合同总金额(含税))
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string not_desc { get; set; }

        /// <summary>
        /// Desc:变更后合同总价
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? now_tol_con_price_exd_tax { get; set; }

        /// <summary>
        /// Desc:合同变更后总金额（含税）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? hs_bgamt { get; set; }

        /// <summary>
        /// Desc:合同签约方类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string qyf_type { get; set; }

        /// <summary>
        /// Desc:我方签约单位ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parties_unit_id { get; set; }

        /// <summary>
        /// Desc:我方签约单位
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parties_unit_name { get; set; }

        /// <summary>
        /// Desc:我方签约单位所属二级
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string report_own_partyid { get; set; }

        /// <summary>
        /// Desc:我方签约授权人信息（姓名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parties_authorized { get; set; }

        /// <summary>
        /// Desc:合同实际履约主体ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sjlydw_id { get; set; }

        /// <summary>
        /// Desc:合同实际履约主体
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sjlydw_name { get; set; }

        /// <summary>
        /// Desc:实际履约主体所属法人单位OID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sjlydw_ejdwoid { get; set; }

        /// <summary>
        /// Desc:关联状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? relation_state { get; set; }

        /// <summary>
        /// Desc:合同状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_status { get; set; }

        /// <summary>
        /// Desc:停用
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string enable_status { get; set; }

        /// <summary>
        /// Desc:停用说明
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string deactivate_desc { get; set; }

        /// <summary>
        /// Desc:数据来源
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string from_sys_code { get; set; }

        /// <summary>
        /// Desc:原系统合同主键ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string from_id { get; set; }

        /// <summary>
        /// Desc:版本号（自增）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? version_no { get; set; }

        /// <summary>
        /// Desc:是否虚拟合同
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_invented { get; set; }

        /// <summary>
        /// Desc:变更次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string change_num { get; set; }

        /// <summary>
        /// Desc:是否最新版本
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_newest { get; set; }

        /// <summary>
        /// Desc:是否删除
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_delete { get; set; }

        /// <summary>
        /// Desc:是否有保证金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_sfybzj { get; set; }

        /// <summary>
        /// Desc:是否有担保
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_sfydb { get; set; }

        /// <summary>
        /// Desc:创建机构
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string create_org { get; set; }

        /// <summary>
        /// Desc:管理部门名称id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string oid { get; set; }

        /// <summary>
        /// Desc:管理部门名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string oname { get; set; }

        /// <summary>
        /// Desc:财务云入库执行状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string change_status { get; set; }

        /// <summary>
        /// Desc:数据类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string data_type { get; set; }

        /// <summary>
        /// Desc:关联补充协议合同ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string supply_number { get; set; }

        /// <summary>
        /// Desc:核算组织单位名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string account_unit_name { get; set; }

        /// <summary>
        /// Desc:核算组织单位编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string account_unit_code { get; set; }

        /// <summary>
        /// Desc:是否审批中
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string is_approving { get; set; }

        /// <summary>
        /// Desc:审批状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string examin_state { get; set; }

        /// <summary>
        /// Desc:审批类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string examin_type { get; set; }

        /// <summary>
        /// Desc:流程状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string process_status { get; set; }

        /// <summary>
        /// Desc:审批表单id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string business_bill_id { get; set; }

        /// <summary>
        /// Desc:争议解决方式
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string dispute_solution_way { get; set; }

        /// <summary>
        /// Desc:合同范本使用情况
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string con_model_use_condition { get; set; }

        /// <summary>
        /// Desc:录入人ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string create_user_id { get; set; }

        /// <summary>
        /// Desc:创建人姓名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string create_user { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? create_time { get; set; }

        /// <summary>
        /// Desc:修改人ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string update_user_id { get; set; }

        /// <summary>
        /// Desc:修改人姓名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string update_user { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? update_time { get; set; }

        /// <summary>
        /// Desc:入库赋码时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? create_maincode_time { get; set; }

        /// <summary>
        /// Desc:合同入库时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? contract_warehousing_time { get; set; }

        /// <summary>
        /// Desc:数据来源（接口推送或者页面录入）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string data_from { get; set; }

        /// <summary>
        /// Desc:加载时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? load_time { get; set; }

        /// <summary>
        /// Desc:所属二级单位编码（4A）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sec_org { get; set; }

        /// <summary>
        /// Desc:所属二级单位名称（4A）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string sec_org_name { get; set; }

        /// <summary>
        /// Desc:中标交底项目名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string zbjd_proj_name { get; set; }

        /// <summary>
        /// Desc:中标交底项目编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string zbjd_proj_code { get; set; }

        /// <summary>
        /// Desc:所属三级单位编码（4A）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string third_org { get; set; }

    }
}
