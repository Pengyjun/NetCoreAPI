namespace GHMonitoringCenterApi.CustomAttribute
{
    /// <summary>
    /// 自定义工作单元特性起到一个标志性作用
    /// </summary>

    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute:Attribute
    {
    }
}
