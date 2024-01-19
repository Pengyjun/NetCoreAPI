using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.SearchUser;
using GHMonitoringCenterApi.Application.Contracts.Dto.User;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.User
{


    /// <summary>
    /// 用户接口层
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        CurrentUser GetUserInfoAsync(string token);
        /// <summary>
        /// 获取用户列表
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InformationResponseDto>>> SearchPersonnelInformationAsync(SearchUserRequseDto stringrequestDto,string oid);
        
    }
}
