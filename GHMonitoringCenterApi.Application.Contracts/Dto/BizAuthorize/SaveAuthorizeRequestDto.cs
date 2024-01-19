using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize
{
    /// <summary>
    /// 保存授权请求业务
    /// </summary>
    public  class SaveAuthorizeRequestDto
    {
        /// <summary>
        /// 授权的用户id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public BizModule BizModule { get; set; } = BizModule.MonthReport;

        /// <summary>
        ///  授权模式
        /// </summary>
        public AuthorizeMode AuthorizeMode { get; set; }
    }
}
