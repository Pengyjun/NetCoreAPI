using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Web.Jwt

{
    public interface IJwtService
    {
        /// <summary>
        /// 生成AccessToken
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        string CreateAccessToken(Claim[] claims, int expires);

        /// <summary>
        /// 生成刷新token
        /// </summary>
        /// <returns></returns>
        string CreateRefreshToken();
    }
}
