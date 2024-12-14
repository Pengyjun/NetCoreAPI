using Microsoft.OpenApi.Models;
using System.Reflection;
using HNKC.Demo.Web.Filters;
using HNKC.Demo.SqlSugars.UnitOfTransaction;
using HNKC.Demo.SqlSugars.Extensions;
using HNKC.Demo.Services.Admin.Api;
using HNKC.Demo.Web.DateTimeHandler;

namespace HNKC.Demo.Services.Admin.Api
{
    public static class ProgramExtensions
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="builder"></param>
        public static void AddControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new HttpGlobalExceptionFilter());
                options.Filters.Add(typeof(ValidateModelFilter));
                options.Filters.Add(new APIResultFilter());
                options.Filters.Add(typeof(TransactionalFilter));
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverterExtension());
            });
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="builder"></param>
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// 注入SqlSugar
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSqlSugar(this WebApplicationBuilder builder)
        {
            builder.Services.AddSqlSugarContext(builder.Configuration.GetConnectionString("MySql"));
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

                options.SwaggerDoc("v1", new OpenApiInfo { Title = $"POMS-Vivible-Leader", Version = "v1" });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// UseSwagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomSwagger(this WebApplication app)
        {
            var prefixUrl = app.Configuration.GetValue<string>("SwaggerPrefixUrl");
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((doc, request) =>
                {
                    doc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = prefixUrl + "/visible-leader", Description = "gateway" },
                        new OpenApiServer { Url = prefixUrl + "/", Description = "local" }
                    };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/visible-leader/swagger/v1/swagger.json", $"Visible Leader Api V1");
                c.OAuthClientId("swaggerui");
                c.OAuthAppName("Swagger UI");
            });
        }

        /// <summary>
        /// 跨域
        /// </summary>
        /// <param name="builder"></param>
        public static void AddCors(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(policy =>
                {
                    policy.AddPolicy("CorsPolicy", opt => opt
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Authorization"));
                });
            }
            else
            {
                var origins = builder.Configuration.GetSection("Origins").Get<string[]>();
                builder.Services.AddCors(policy =>
                {
                    policy.AddPolicy("CorsPolicy", opt => opt
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Authorization"));
                });
            }
        }
    }
}
