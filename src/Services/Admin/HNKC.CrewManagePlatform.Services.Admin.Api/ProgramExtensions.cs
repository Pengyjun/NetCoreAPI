using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Services.Admin.Api;
using HNKC.CrewManagePlatform.Services.Admin.Api.AutoMapper;
using HNKC.CrewManagePlatform.Services.Admin.Api.Filters;
using HNKC.CrewManagePlatform.Services.Interface;
using HNKC.CrewManagePlatform.Services.Interface.AuditLog;
using HNKC.CrewManagePlatform.Services.Interface.Contract;
using HNKC.CrewManagePlatform.Services.Interface.CrewArchives;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUser;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUserService;
using HNKC.CrewManagePlatform.Services.Interface.PullResult;
using HNKC.CrewManagePlatform.Services.Interface.Salary;
using HNKC.CrewManagePlatform.Services.Menus;
using HNKC.CrewManagePlatform.Services.Role;
using HNKC.CrewManagePlatform.Sms.Interfaces;
using HNKC.CrewManagePlatform.Sms.Services;
using HNKC.CrewManagePlatform.SqlSugars.Extensions;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using HNKC.CrewManagePlatform.Utils;
using HNKC.CrewManagePlatform.Web.DateTimeHandler;
using HNKC.CrewManagePlatform.Web.Filters;
using HNKC.CrewManagePlatform.Web.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Admin.Api
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
                options.Filters.Add(typeof(RequestInfoFilter));
                options.Filters.Add(typeof(ActionResultFilter));
                options.Filters.Add(new APIResultFilter());
                options.Filters.Add(typeof(TransactionalFilter));
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverterExtension());
            });
        }

        /// <summary>
        /// 文件上传配置
        /// </summary>
        /// <param name="builder"></param>
        public static void AddConfigUpload(this WebApplicationBuilder builder)
        {
            //上传文件限制大小配置
            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                options.Limits.MaxRequestBodySize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:LittleFileSize"));
            });
        }


        /// <summary>
        /// AutoMapper注入
        /// </summary>
        /// <param name="builder"></param>
        public static void AddAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(ConfigAutoMapper =>
            {
                AutoMapperProfileFile.AutoMapperProfileInit(ConfigAutoMapper);
            });
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="builder"></param>
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddJwtService(x =>
            {
                x.SecurityKey = AppsettingsHelper.GetValue("AccessToken:SecretKey");
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppsettingsHelper.GetValue("AccessToken:SecretKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
            });

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            HttpContentAccessFactory.services = builder.Services;
        }

        /// <summary>
        /// 注入SqlSugar
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSqlSugar(this WebApplicationBuilder builder)
        {
            builder.Services.AddSqlSugarContext(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
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

                options.SwaggerDoc("v1", new OpenApiInfo { Title = $"船员管理系统API", Version = "v1" });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                var xmlModelFile = $"HNKC.CrewManagePlatform.Services.xml";
                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, xmlModelFile);
                options.IncludeXmlComments(xmlModelPath);

                var xmlModelDtoFile = $"HNKC.CrewManagePlatform.Models.xml";
                var xmlModelDtoPath = Path.Combine(AppContext.BaseDirectory, xmlModelDtoFile);
                options.IncludeXmlComments(xmlModelDtoPath);
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
                //var origins = builder.Configuration.GetSection("Origins").Get<string[]>();
                //builder.Services.AddCors(policy =>
                //{
                //    policy.AddPolicy("CorsPolicy", opt => opt
                //    .WithOrigins(origins)
                //    .AllowAnyHeader()
                //    .AllowAnyMethod()
                //    .WithExposedHeaders("Authorization"));
                //});
            }
        }


        /// <summary>
        /// 自动注入业务接口
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="Exception"></exception>
        public static void AddAutoDependencyInjection(this WebApplicationBuilder builder)
        {
            #region 自动注入
            //try
            //{
            //    //自定义特性名称
            //    var attrName = "DependencyInjectionAttribute";
            //    //获取bin目录
            //    var rootPath = AppContext.BaseDirectory;
            //    //符合条件的接口
            //    Dictionary<Type, Type> interfaceList = new Dictionary<Type, Type>();
            //    //符合条件接口的实现
            //    List<Type> interfaceImplList = new List<Type>();
            //    //加载dll文件
            //    var interfaceDll = Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}HNKC.CrewManagePlatform.Services.dll").GetTypes();
            //    //获取所有接口
            //    var allInterfaceList = Array.FindAll(interfaceDll, type => type.IsInterface);
            //    //获取所有类
            //    var classDll = Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}HNKC.CrewManagePlatform.Services.dll").GetTypes();
            //    var allClassList = Array.FindAll(classDll, type => type.IsPublic && type.IsClass).ToList();
            //    foreach (var item in allClassList)
            //    {
            //        //该类实现所实现的所有接口
            //        var allInterfaces = item.GetInterfaces();
            //        foreach (var interfaceItem in allInterfaces)
            //        {
            //            var isImplInterface = item.GetInterface(interfaceItem.Name);
            //            if (isImplInterface != null)
            //            {
            //                var customAttributeData = allInterfaces[0].CustomAttributes.Where(x => x.AttributeType.Name == attrName).FirstOrDefault();
            //                if (customAttributeData != null && interfaceList.Where(x => x.Key == item).Count() == 0)
            //                {
            //                    //interfaceList.Add(item, customAttributeData.GetType());
            //                    interfaceList.Add(item, allInterfaces[0]);
            //                }
            //            }

            //        }
            //    }

            //    if (interfaceList.Any())
            //    {
            //        foreach (var key in interfaceList)
            //        {
            //            builder.Services.AddScoped(key.Value, key.Key);
            //        }
            //        //注入工作单元
            //        //builder.RegisterType<SqlSugarUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("依赖注入容器初始化失败");
            //}
            #endregion

            builder.Services.AddScoped<IAuditLogService, AuditLog>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();




            builder.Services.AddScoped<IUserManagerService, UserManagerService>();
            builder.Services.AddScoped<ISalaryService, SalaryService>();
            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ISmsService, CtyunSmsService>();
            builder.Services.AddScoped<IDataDictionaryService, DataDictionaryService>();
            builder.Services.AddScoped<ICrewArchivesService, CrewArchivesService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IContractService, ContractService>();

        }
    }
}
