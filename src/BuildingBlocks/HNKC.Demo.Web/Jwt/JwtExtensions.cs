using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.Demo.Web.Jwt
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
