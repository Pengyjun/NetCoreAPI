using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared.Enums
{
    /// <summary>
    /// 数据操作类型
    /// </summary>
    public enum DataOperationType
    {

        /// <summary>
        /// 增
        /// </summary>
        Insert=1,
        /// <summary>
        /// 改
        /// </summary>
        Update=2,
        /// <summary>
        /// 删
        /// </summary>
        Delete=3,
        /// <summary>
        /// 查
        /// </summary>
        Select=4,
    }
}
