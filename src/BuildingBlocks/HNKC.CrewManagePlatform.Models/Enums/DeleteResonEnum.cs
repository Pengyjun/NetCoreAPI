﻿using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 删除原因
    /// </summary>
    public enum DeleteResonEnum
    {
        /// <summary>
        /// 在岗
        /// </summary>
        [Description("在岗")]
        Normal = 0,
        /// <summary>
        /// 离职
        /// </summary>
        [Description("离职")]
        LiZhi = 1,
        /// <summary>
        /// 调离
        /// </summary>
        [Description("调离")]
        DiaoLi = 2,
        /// <summary>
        /// 退休
        /// </summary>
        [Description("退休")]
        TuiXiu = 3,
        /// <summary>
        /// 休假
        /// </summary>
        [Description("休假")]
        XiuJia = 4
    }
}
