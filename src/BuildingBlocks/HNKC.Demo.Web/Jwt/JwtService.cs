using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

<<<<<<<< HEAD:HNKC.CrewManagePlatform.Services/Interface/Jwt/JwtService.cs
namespace HNKC.CrewManagePlatform.Web.Jwt
========
namespace HNKC.Demo.Web.Jwt
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Jwt/JwtService.cs
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        public JwtService(IOptionsMonitor<JwtOptions> optionsMonitor)
        {
            _jwtOptions = optionsMonitor.Get("JwtOptions");
        }

        /// <summary>
        /// 生成AccessToken
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public string CreateAccessToken(Claim[] claims, int expires)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(expires)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey)),
                    SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// 生成刷新token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string CreateRefreshToken()
        {
            var refreshBytes = new byte[32];
            using (var number = RandomNumberGenerator.Create())
            {
                number.GetBytes(refreshBytes);
                return Convert.ToBase64String(refreshBytes);
            }
        }
    }
}
