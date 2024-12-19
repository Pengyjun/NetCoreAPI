using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 船舶类型
    /// </summary>
    public enum ShipTypeEnum
    {
        /// <summary>
        /// 耙吸
        /// </summary>
        [Description("耙吸")]
        Pax = 0,
        /// <summary>
        /// 绞吸
        /// </summary>
        [Description("绞吸")]
        Jiaox = 1,
        /// <summary>
        /// 抓斗
        /// </summary>
        [Description("抓斗")]
        Zhuad = 2,
        /// <summary>
        /// 辅助
        /// </summary>
        [Description("辅助")]
        Fuz = 3
    }
}
