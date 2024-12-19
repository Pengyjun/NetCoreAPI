using HNKC.CrewManagePlatform.Common;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUserService;
using HNKC.CrewManagePlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.CurrentUser
{
    public class CurrentUserService: ICurrentUserService
    {
        #region 全局对象暴露属性
        /// <summary>
        /// 全局对象暴露属性
        /// </summary>
        public GlobalCurrentUser GlobalCurrentUser
        {
            get { return CurrentUserAsync().GetAwaiter().GetResult(); }
        }
        #endregion
        #region 获取用户信息并返回全局用户对象
        /// <summary>
        /// 获取用户信息并返回全局用户对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GlobalCurrentUser> CurrentUserAsync()
        {
            GlobalCurrentUser  userInfo = new GlobalCurrentUser();
            var isAuth = HttpContentAccessFactory.Current.User?.Identity?.IsAuthenticated;
            var requestToken = HttpContentAccessFactory.Current.Request.Headers["Authorization"];
            if (isAuth.HasValue&&isAuth.Value)
            {
                //解析用户信息
                var userList=HttpContentAccessFactory.Current.User?.Claims.ToList();
                var id = userList?.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault();
                var token=await RedisUtil.Instance.GetAsync(id);
                if (!string.IsNullOrWhiteSpace(token) && token.Trim().Equals(requestToken.ToString().Replace("Bearer","").Trim()))
                {
                    var workNumber = userList?.Where(x => x.Type == "WorkNumber").Select(x => x.Value).FirstOrDefault();
                    var bId = userList?.Where(x => x.Type == "BId").Select(x => x.Value).FirstOrDefault();
                    var name = userList?.Where(x => x.Type == "Name").Select(x => x.Value).FirstOrDefault();
                    var oid = userList?.Where(x => x.Type == "Oid").Select(x => x.Value).FirstOrDefault();
                    var phone = userList?.Where(x => x.Type == "Phone").Select(x => x.Value).FirstOrDefault();
                    var roleBusinessId = userList?.Where(x => x.Type == "RoleBusinessId").Select(x => x.Value).FirstOrDefault();
                    var bInstitutionId = userList?.Where(x => x.Type == "BInstitutionId").Select(x => x.Value).FirstOrDefault();
                    userInfo.Id = long.Parse(id);
                    userInfo.UserBusinessId = Guid.Parse(bId);
                    userInfo.Name = name;
                    userInfo.Oid = oid;
                    userInfo.RoleBusinessId = Guid.Parse(roleBusinessId);
                    userInfo.WorkNumber = workNumber;
                    userInfo.Phone= phone;
                    userInfo.InstitutionBusiessId = Guid.Parse(bInstitutionId);
                }
               
            }
            return userInfo;
        }
        #endregion
    }
}
