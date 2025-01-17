using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 值班类型
    /// </summary>
    public enum RotaEnum
    {
        /// <summary>
        /// 甲板部
        /// </summary>
        [Description("甲板部")]
        JiaBan = 1,
        ///// <summary>
        ///// 甲板部非班
        ///// </summary>
        //[Description("甲板部非班")]
        //JiaBanFeiBan = 2,
        /// <summary>
        /// 轮机部
        /// </summary>
        [Description("轮机部")]
        LunJi = 2,
        ///// <summary>
        ///// 轮机部非班
        ///// </summary>
        //[Description("轮机部非班")]
        //LunJiFeiBan = 4
    }
    /// <summary>
    /// 时间枚举 固定/非固定
    /// </summary>
    public enum TimeEnum
    {
        /// <summary>
        /// 固定时间排班
        /// </summary>
        [Description("固定时间排班")]
        FixedTime = 1,
        /// <summary>
        /// 非固定时间排班
        /// </summary>
        [Description("非固定时间排班")]
        NotFixedTime = 2
    }
    /// <summary>
    /// 固定时间区间类型 1：0-4 2：4-8 3：8-12
    /// </summary>
    public enum TimeSoltEnum
    {
        /// <summary>
        /// 0-4
        /// </summary>
        [Description("0-4")]
        First = 1,
        /// <summary>
        /// 4-8
        /// </summary>
        [Description("4-8")]
        Seconed = 2,
        /// <summary>
        /// 8-12
        /// </summary>
        [Description("8-12")]
        Third = 3
    }
}
