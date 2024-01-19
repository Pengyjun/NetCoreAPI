using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.SearchUser;
using GHMonitoringCenterApi.Application.Contracts.Dto.User;
using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace GHMonitoringCenterApi.Controllers.User
{


    /// <summary>
    /// 系统用户相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {

        #region 依赖注入
        public IUserService UserService { get; set; }
        public ISqlSugarClient dbContent { get; set; }
        public UserController(IUserService UserService, ISqlSugarClient dbContent)
        {
            this.UserService = UserService;
            this.dbContent = dbContent;
        }
        #endregion
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        public async Task<ResponseAjaxResult<UserResponseDto>> GetUserInfoAsync()
        {
            ResponseAjaxResult<UserResponseDto> responseAjaxResult = new ResponseAjaxResult<UserResponseDto>();
            var user = CurrentUser;
            if (user.Id != Guid.Empty)
            {
                //获取当前登陆部门id  是否包含00p
                var isContains = string.Empty;
                if (user.CurrentLoginDepartmentId != null)
                {
                    isContains = await dbContent.Queryable<Institution>().Where(x => x.PomId == user.CurrentLoginDepartmentId).Select(x => x.Ocode).FirstAsync();
                }
                responseAjaxResult.Data = new UserResponseDto()
                {

                    Id = user.Id,
                    Name = user.Name,
                    CurrentLoginDepartmentId = user.CurrentLoginDepartmentId,
                    CurrentLoginInstitutionGrule = user.CurrentLoginInstitutionGrule,
                    CurrentLoginInstitutionPoid = user.CurrentLoginInstitutionPoid,
                    CurrentLoginInstitutionOid = user.CurrentLoginInstitutionOid,
                    CurrentLoginInstitutionId = user.CurrentLoginInstitutionId,
                    CurrentLoginRoleId = user.CurrentLoginRoleId,
                    CurrentLoginInstitutionName = user.CurrentLoginInstitutionName,
                    CurrentLoginUserType = isContains.Contains("00P") ? 3 : 0,
                    RoleInfos = user.RoleInfos,
                };
                responseAjaxResult.Count = 1;
                responseAjaxResult.Success();
            }

            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_NOLOGIN_FAIL, HttpStatusCode.NoLogin);
            };

            return responseAjaxResult;

        }


        /// <summary>
        /// 激活当前登录用户
        /// </summary>
        /// <param name="currentLoginUserRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ActivateCurrentLoginUser")]
        public async Task<ResponseAjaxResult<bool>> ActivateCurrentUserAsync([FromQuery] CurrentLoginUserRequestDto currentLoginUserRequestDto)
        {
            var user = CurrentUser;
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            RoleInfo currentActivateUser = new RoleInfo();
            if (CurrentUser != null && CurrentUser.Id != Guid.Empty)
            {
                currentActivateUser = user.RoleInfos.SingleOrDefault(x => x.Id == currentLoginUserRequestDto.RoleId.Value && x.InstitutionId == currentLoginUserRequestDto.InstitutionId.Value);
                var redis = RedisUtil.Instance;
                var isExist = await redis.ExistsAsync(user.Account);
                if (isExist && currentActivateUser != null && currentActivateUser.Id != Guid.Empty)
                {
                    CurrentUser.CurrentLoginInstitutionOid = currentActivateUser.Oid;
                    CurrentUser.CurrentLoginInstitutionId = currentActivateUser.InstitutionId;
                    CurrentUser.CurrentLoginInstitutionPoid = currentActivateUser.Poid;
                    CurrentUser.CurrentLoginRoleId = currentActivateUser.Id;
                    CurrentUser.CurrentLoginInstitutionGrule = currentActivateUser.Grule;
                    CurrentUser.CurrentLoginDepartmentId = currentActivateUser.DepartmentInfos.Id;
                    CurrentUser.CurrentLoginInstitutionName = currentActivateUser.DepartmentInfos?.Name;
                    CurrentUser.CurrentLoginIsAdmin = currentActivateUser.IsAdmin;
                    CurrentUser.CurrentLoginUserType = currentActivateUser.Type;
                    CurrentUser.CurrentLoginOperationType = currentActivateUser.OperationType;
                    //更新缓存
                    var isSuccess = await redis.SetAsync(user.Account, CurrentUser);
                    if (isSuccess)
                    {
                        responseAjaxResult.Data = true;
                        responseAjaxResult.Success(ResponseMessage.NOT_ACTIVATEUSER_SUCCESS);
                        return responseAjaxResult;
                    }
                }
            }
            responseAjaxResult.Fail(ResponseMessage.NOT_ACTIVATEUSER_FILE, HttpStatusCode.ActivateUserFile);
            return responseAjaxResult;

        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUser")]
        public async Task<ResponseAjaxResult<List<InformationResponseDto>>> SearchUserAsync([FromQuery] SearchUserRequseDto searchUserRequseDto)
        {
            return await UserService.SearchPersonnelInformationAsync(searchUserRequseDto, CurrentUser.CurrentLoginInstitutionOid);
        }
    }
}
