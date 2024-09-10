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
        Person = 1,
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

        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        ClassDevice = 10,
        /// <summary>
        /// 发票
        /// </summary>
        Invoice = 11,
        /// <summary>
        /// 科研
        /// </summary>
        Rcientific = 12,
        /// <summary>
        /// 房号
        /// </summary>
        Room = 13,
        /// <summary>
        /// 语言
        /// </summary>
        Language = 14
    }
}
