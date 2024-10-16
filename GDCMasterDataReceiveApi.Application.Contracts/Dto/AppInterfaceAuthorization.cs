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
}
