using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 数据脱敏规则表
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_datadesensitizationrule")]
    public class DataDesensitizationRule
    {
        public long Id { get; set; }
        /// <summary>
        /// 应用系统接口  关联关系
        /// </summary>
        public long AppInterfaceId { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string? Field { get; set; }
        /// <summary>
        /// 字段中文名称
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// 起始位置
        /// </summary>
        public int? StartIndex { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public int? EndIndex { get; set; }
        /// <summary>
        /// 数据脱敏类型   input  是截取    replace是替换
        /// </summary>
        public int DesensitizationType { get; set; }
        /// <summary>
        /// 是否是引用对象  0不是 1 是  默认是0
        /// </summary>
        public int? IsRefObject { get; set; }
        /// <summary>
        /// 是否启用   0 未启用  1 启用
        /// </summary>
        public int? Enable { get; set; }
    }
    /// <summary>
    /// 展示   不做表逻辑处理
    /// </summary>
    public class SearchDataDesensitizationRule
    {
        public string? Id { get; set; }
        /// <summary>
        /// 应用系统接口  关联关系
        /// </summary>
        public string? AppInterfaceId { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string? Field { get; set; }
        /// <summary>
        /// 字段中文名称
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// 起始位置
        /// </summary>
        public int? StartIndex { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public int? EndIndex { get; set; }
        /// <summary>
        /// 数据脱敏类型   input  是截取    replace是替换
        /// </summary>
        public int DesensitizationType { get; set; }
        /// <summary>
        /// 是否是引用对象  0不是 1 是  默认是0
        /// </summary>
        public int? IsRefObject { get; set; }
        /// <summary>
        /// 是否启用   0 未启用  1 启用
        /// </summary>
        public int? Enable { get; set; }
    }
    /// <summary>
    /// 接口授权表
    /// </summary>
    [Tenant("gdcdatasecurityapi")]
    [SugarTable("t_appinterfaceauthorization")]
    public class AppInterfaceAuthorization
    {
        public long Id { get; set; }
        /// <summary>
        /// 应用系统主键   关联关系
        /// </summary>
        public long? AppSystemId { get; set; }
        /// <summary>
        /// 接口函数码
        /// </summary>
        public string? AppInterfaceCode { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string? InterfaceName { get; set; }
        /// <summary>
        /// 接口中文名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 标识一个接口返回是否加密处理   0 是不加密  1是加密处理  前端 以及下游需要解密
        /// </summary>
        public int? ReturnDataEncrypt { get; set; }
        /// <summary>
        /// 接口访问IP限制
        /// </summary>
        public string? AccessRestrictedIP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
