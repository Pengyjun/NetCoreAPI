namespace HNKC.CrewManagePlatform.Models.Attributes
{
    /// <summary>
    ///自定义依赖注入特性 接口打上这个特性   不需要在IOC容器手动添加接口
    ///起到标记作用
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class DependencyInjectionAttribute : Attribute
    { }
}
