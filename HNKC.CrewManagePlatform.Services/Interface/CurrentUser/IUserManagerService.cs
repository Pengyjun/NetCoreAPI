using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.CurrentUser
{
    public interface IUserManagerService
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        Task<Result> UserLoginAsync(UserLoginRequest userLoginRequest);


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="resetPwdResquest"></param>
        /// <returns></returns>
        Task<Result> ResetPwdAsync(ResetPwdResquest  resetPwdResquest);
    }
}
