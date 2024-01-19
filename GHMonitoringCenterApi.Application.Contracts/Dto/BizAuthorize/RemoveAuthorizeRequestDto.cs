using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize
{
    /// <summary>
    /// 移除授权业务请求
    /// </summary>
    public  class RemoveAuthorizeRequestDto
    {
        /// <summary>
        /// 授权Id
        /// </summary>
        public Guid AuthorizeId { get; set; }
    }
}
