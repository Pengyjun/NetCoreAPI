using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:HNKC.CrewManagePlatform.Services/Interface/Jwt/IJwtService.cs
namespace HNKC.CrewManagePlatform.Web.Jwt
========
namespace HNKC.Demo.Web.Jwt
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Jwt/IJwtService.cs
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
