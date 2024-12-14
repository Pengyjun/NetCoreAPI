using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:HNKC.CrewManagePlatform.Services/Interface/Jwt/JwtExtensions.cs
namespace HNKC.CrewManagePlatform.Web.Jwt
========
namespace HNKC.Demo.Web.Jwt
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Jwt/JwtExtensions.cs
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services, Action<JwtOptions> options)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));
            services = services ?? throw new ArgumentNullException(nameof(services));

            //将实例注入到服务容器中，并将获取的选项值赋值给 options 参数。
            services.Configure("JwtOptions", options);

            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
