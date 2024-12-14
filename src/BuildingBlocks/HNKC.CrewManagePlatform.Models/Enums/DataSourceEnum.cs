using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    public enum DataSourceEnum
    {
        /// <summary>
        /// 导入
        /// </summary>
        [Description("导入")]
        Import=0,

        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Insert = 1,
    }
}
