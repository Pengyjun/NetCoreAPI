using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared.Enums
{


    /// <summary>
    /// 接收数据类型枚举
    /// </summary>
    public enum ReceiveDataType
    {
         /// <summary>
         /// 人员
         /// </summary>
         Person=1,
        /// <summary>
        /// 机构
        /// </summary>
        Institution = 2,
        /// <summary>
        /// 项目
        /// </summary>
         Project = 3,
        /// <summary>
        /// 币种
        /// </summary>
        Currency = 3,
    }
}
