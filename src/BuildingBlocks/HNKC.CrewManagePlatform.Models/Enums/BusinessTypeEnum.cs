using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessTypeEnum
    {

        /// <summary>
        /// 批量推送
        /// </summary>
        [Description("批量推送")]
        BatchPush=1,


        /// <summary>
        /// 个人推送
        /// </summary>
        [Description("个人推送")]
        PersonalPush = 0,
    }
}
