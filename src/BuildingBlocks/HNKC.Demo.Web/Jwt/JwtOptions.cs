using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

<<<<<<<< HEAD:HNKC.CrewManagePlatform.Services/Interface/Jwt/JwtOptions.cs
namespace HNKC.CrewManagePlatform.Web.Jwt
========
namespace HNKC.Demo.Web.Jwt
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Jwt/JwtOptions.cs
{
    public class JwtOptions
    {
        public string SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
