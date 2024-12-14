using HNKC.CrewManagePlatform.Models.Attributes;
using HNKC.CrewManagePlatform.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.CurrentUserService
{
    [DependencyInjection]
    public interface ICurrentUserService
    {
        /// <summary>
        /// 验证用户信息并返回全局用户对象
        /// </summary>
        /// <returns></returns>
        Task<GlobalCurrentUser> CurrentUserAsync();
    }
}
