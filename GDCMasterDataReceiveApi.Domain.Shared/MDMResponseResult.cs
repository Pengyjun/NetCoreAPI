using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared
{

    /// <summary>
    /// 4A接口接收返回数据
    /// </summary>
    public class MDMResponseResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string RETURN_CODE { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string RETURN_DESC { get; set; }
    }
}
