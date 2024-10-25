using CH.Simple.SqlSugar;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CH.Simple.APP.API
{
    public static class ProgramExtensions
    {
        /// <summary>
        /// 注入SqlSugar
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSqlSugar(this WebApplicationBuilder builder)
        {
            builder.Services.AddSqlSugarContext(builder.Configuration.GetConnectionString("MySQL"));
        }


        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="builder"></param>
        public static void AddCustomSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请输入token格式为Bearer xxxxxx(中间必须有空格)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                     new OpenApiSecurityScheme()
                     {
                          Reference=new OpenApiReference
                          {
                              Type=ReferenceType.SecurityScheme,
                              Id="Bearer"
                          }
                     },new string[]{ }
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo { Title = $"POMS-Admin", Version = "v1" });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
