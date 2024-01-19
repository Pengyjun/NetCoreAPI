namespace GHMonitoringCenterApi.CustomAttribute
{

    /// <summary>
    /// 忽略字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreFieldAttribute : Attribute
    {
    }
}
