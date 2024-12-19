using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 船舶状态
    /// </summary>
    public enum ShipStateEnum
    {
        /// <summary>
        /// 施工
        /// </summary>
        [Description("施工")]
        Construction = 0,
        /// <summary>
        /// 调遣
        /// </summary>
        [Description("调遣")]
        Dispatch = 1,
        /// <summary>
        /// 厂修
        /// </summary>
        [Description("厂修")]
        ShopRepair = 2,
        /// <summary>
        /// 待命
        /// </summary>
        [Description("待命")]
        Standby = 3
    }
}
