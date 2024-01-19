using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    ///  基本关键词搜索
    /// </summary>
    public class BaseKeyWordsRequestDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 关键词搜索
        /// </summary>
        public string? KeyWords { get; set; }
   }
}
