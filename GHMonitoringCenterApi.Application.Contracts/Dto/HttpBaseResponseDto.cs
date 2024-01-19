using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{


    /// <summary>
    /// 调用http接口返回的基本信息类  
    /// </summary>
    public class HttpBaseResponseDto<T>
    {
        /// <summary>
        /// 状态吗
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 条数
        /// </summary>

        public int Count { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>

        public List<T> Data { get; set; }
    }
}
