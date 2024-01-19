using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize
{
    /// <summary>
    /// 搜索授权用户
    /// </summary>
    public  class SearchAuthorizeRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 业务模块
        /// </summary>
        public BizModule BizModule { get; set; }


        /// <summary>
        /// 是否授权
        /// </summary>
        public AuthorizeMode? AuthorizeMode { get; set; }
    }
}
