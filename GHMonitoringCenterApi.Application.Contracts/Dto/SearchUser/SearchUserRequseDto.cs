using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.SearchUser
{
    /// <summary>
    /// 获取用户列表响应Dto
    /// </summary>
    public class SearchUserRequseDto:BaseRequestDto
    {
        /// <summary>
        /// 当前角色Id
        /// </summary>
        
       public Guid RoleId { get; set; }
    }
}
