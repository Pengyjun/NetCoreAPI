using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Enums
{
    public enum UserType
    {
        /// <summary>
        /// 内部人员
        /// </summary>
        [Description("内部人员")]
        Internals = 1,

        /// <summary>
        /// 外部人员
        /// </summary>
        [Description("外部人员")]
        Outsiders = 2
    }
}
