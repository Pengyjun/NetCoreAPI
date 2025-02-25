using Autofac;
using Autofac.Extensions.DependencyInjection;
using GHMonitoringCenterApi;
using GHMonitoringCenterApi.Application.Contracts.AutoMapper;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Filters;
using GHMonitoringCenterApi.Ioc;
using GHMonitoringCenterApi.Middleware;
using GHMonitoringCenterApi.SqlSugarCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using UtilsSharp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<GlobalObject>();
//配置中间件
builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddleware>();
//自定义固定鉴权
builder.Services.AddScoped<CustomFixedAuthAttribute>();
//配置json格式化时间
builder.Services.AddControllers(options =>
{
    //添加过滤器
    options.Filters.Add(typeof(GlobalObjectFilter));
    options.Filters.Add(typeof(GlobalExceptionFilter));
    options.Filters.Add(typeof(RecordRequestInfoFilter));
    options.Filters.Add(typeof(UserOperationAuthorityFilter));
    options.Filters.Add(typeof(UnitOfWorkFilter));

}).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
    });
//上传文件限制大小配置
builder.WebHost.ConfigureKestrel((context, options) =>
{
    //限制大小200M
    options.Limits.MaxRequestBodySize = int.Parse(AppsettingsHelper.GetValue("UpdateItem:DefaultSingleFileSize"));
});
builder.Services.AddHttpContextAccessor();
HttpContentAccessFactory.services = builder.Services;
builder.Services.AddEndpointsApiExplorer();
//判断是否是移动端访问判断
builder.Services.AddDetection();
//配置swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Description = "请输入token格式为Bearer xxxxxx(中间必须有空格)",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
         new OpenApiSecurityScheme(){
          Reference=new OpenApiReference{ Type=ReferenceType.SecurityScheme,
          Id="Bearer"}
         },new string[]{ }
        }
    });
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Description = "数字广航-智慧运营监控中心API文档",
        Title = "数字广航-智慧运营监控中心系统API",
        Version = "v1"
    });
    var xmlControllerAPIFilename = $"GHMonitoringCenterApi.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlControllerAPIFilename), true);
    var xmlControllerFilename = $"GHMonitoringCenterApi.Application.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlControllerFilename), true);
    var xmlApplicationFilename = $"GHMonitoringCenterApi.Application.Contracts.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlApplicationFilename));
    var xmlDomainFilename = $"GHMonitoringCenterApi.Domain.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDomainFilename));
    var xmlSharedFilename = $"GHMonitoringCenterApi.Domain.Shared.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlSharedFilename));
});
//配置跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", builder => builder.WithOrigins("http://localhost:5000")
                                              .AllowAnyMethod()
                                              .AllowAnyHeader()
                                              .WithExposedHeaders("Authorization")
                                              );
});
//配置认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    var securyKeyByte = Encoding.UTF8.GetBytes(AppsettingsHelper.GetValue("Authentication:SecurityKey"));
    //验证token相关参数
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {

        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(securyKeyByte)
    };
    x.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = fail =>
        {
            return Task.CompletedTask;
        },
        #region 暂时不用
        //OnTokenValidated = success =>
        //{
        //    var token = success.HttpContext.Request.Headers["Authorization"];
        //    if (!string.IsNullOrWhiteSpace(token.ToString()))
        //    {
        //        token = token.ToString().Replace("Bearer", " ").TrimStart();
        //    }
        //    var tokenResult = new JwtSecurityTokenHandler().ValidateToken(token, x.TokenValidationParameters, out SecurityToken securityToken);
        //    if (tokenResult != null && tokenResult.Claims.Any())
        //    {
        //        var account = tokenResult.Claims.FirstOrDefault(x => x.Type == "account")?.Value;
        //        if (!string.IsNullOrWhiteSpace(account))
        //        {
        //            var expStampTime = tokenResult.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
        //            var expTime = TimeHelper.TimeStampToDateTime(expStampTime);
        //            var exp = TimeHelper.GetTimeSpan(DateTime.Now, expTime);
        //            var redis = RedisUtil.Instance;
        //            var md5Token = token.ToString().TrimAll().ToMd5();
        //            if (!redis.Exists(account.ToMd5()))
        //            {
        //                redis.Set(md5Token, null, exp);
        //            }
        //        }
        //    }
        //    return Task.CompletedTask;
        //}
        #endregion

    };
});

//替换自带的IoC
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    //AutoFac初始化
    AutoFacInit.Init(container);
});
//注入数据库上下文
builder.Services.AddSqlSugarContext(builder.Configuration, AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));

#region 表初始化不需要初始化的表结构数据则注释此方法
#if DEBUG
//DbFirst.InitTable(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"), false);
//CodeFirst.InitTable(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
#endif
#endregion
//注入AutoMapper
builder.Services.AddAutoMapper(ConfigAutoMapper =>
{
    AutoMapperProfileFile.AutoMapperProfileInit(ConfigAutoMapper);
});
//清除所有日志
builder.Logging.ClearProviders();
//添加Serilog组件并配置日志
builder.Host.UseSerilog((host, logger) =>
{
    //读取Serilog配置文件
    var serilog = new ConfigurationManager()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("serilog.json", true)//日志配置文件
    .Build();
    //日志格式输出模版
    var outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" + new string('-', 100) + "{NewLine}";
    //读取配置文件
    logger.ReadFrom.Configuration(serilog)
     .WriteTo.Console(LogEventLevel.Verbose, outputTemplate: outputTemplate);
});
var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke(context);
});
#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
#endif
app.UseDetection();
app.UseCors("Cors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//下面这行是因为没有显示启动日志
Console.WriteLine("-----------------------------------程序启动成功--------------------------------------");
app.Run();

