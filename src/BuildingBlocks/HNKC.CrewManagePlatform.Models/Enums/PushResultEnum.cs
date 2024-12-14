using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    public enum PushResultEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success=1,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 0,
    }
}
