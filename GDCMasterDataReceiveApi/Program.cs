using Autofac;
using Autofac.Extensions.DependencyInjection;
using GDCMasterDataReceiveApi;
using GDCMasterDataReceiveApi.Application.Contracts.AutoMapper;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Filters;
using GDCMasterDataReceiveApi.Ioc;
using GDCMasterDataReceiveApi.SqlSugarCore;
using GHElectronicFileApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using UtilsSharp;


var builder = WebApplication.CreateBuilder(args);

#region 授权中间件
//配置中间件
builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddleware>();
#endregion

#region 配置访问IP
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//    // 仅当你知道信任你的代理时才设置为true  
//    options.KnownNetworks.Clear();
//    options.KnownProxies.Clear();
//});

#endregion

#region 配置过滤器 配置json格式化时间
//配置json格式化时间
builder.Services.AddControllers(options =>
{
    //添加过滤器
    options.Filters.Add(typeof(RecordRequestInfoFilter));
    options.Filters.Add(typeof(GlobalExceptionFilter));
    options.Filters.Add(typeof(UnitOfWorkFilter));

}).AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
});
builder.Services.AddHttpClient();
#endregion

#region 配置httpcontent访问工厂
builder.Services.AddHttpContextAccessor();
HttpContentAccessFactory.services = builder.Services;
builder.Services.AddEndpointsApiExplorer();
#endregion

#region IOC注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    //AutoFac初始化
    AutoFacInit.Init(container);
});
#endregion

#region 注册日志
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
#endregion

#region 配置swagger
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
        Description = "广航局-主数据API文档",
        Title = "广航局-主数据API文档",
        Version = "v1"
    });
    var xmlControllerAPIFilename = $"GDCMasterDataReceiveApi.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlControllerAPIFilename), true);
    var xmlControllerFilename = $"GDCMasterDataReceiveApi.Application.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlControllerFilename), true);
    var xmlApplicationFilename = $"GDCMasterDataReceiveApi.Application.Contracts.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlApplicationFilename));
    var xmlDomainFilename = $"GDCMasterDataReceiveApi.Domain.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDomainFilename));
    var xmlSharedFilename = $"GDCMasterDataReceiveApi.Domain.Shared.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlSharedFilename));
});
#endregion

#region 配置跨域
//配置跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", builder => builder.WithOrigins("http://localhost:5000")
                                              .AllowAnyMethod()
                                              .AllowAnyHeader()
                                              .WithExposedHeaders(new string[] { "Authorization", "ClientId" })
                                              );
});
#endregion

#region 配置JWT认证
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
    };
});
#endregion

#region 注入数据库上下文
//注入数据库上下文
builder.Services.AddSqlSugarContext(builder.Configuration, AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
#endregion

#region 表初始化不需要初始化的表结构数据则注释此方法
#if DEBUG
  //CodeFirst.InitTable(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
#endif
#endregion

#region 注入AutoMapper
//注入AutoMapper
builder.Services.AddAutoMapper(ConfigAutoMapper =>
{
    AutoMapperProfileFile.AutoMapperProfileInit(ConfigAutoMapper);
});
#endregion

var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke(context);
});
#region SwaggrUI
app.UseSwagger();
app.UseSwaggerUI();
#endregion


app.UseCors("Cors");
app.UseHttpsRedirection();
//身份验证
app.UseAuthentication();
//授权
app.UseAuthorization();
app.MapControllers();
//下面这行是因为没有显示启动日志
#region 设置线程数  麒麟系统优化
int workThread = 0, miPortThreads = 0;
ThreadPool.GetMinThreads(out workThread, out miPortThreads);
var isSuccess=ThreadPool.SetMinThreads(250, miPortThreads);
Console.WriteLine($"程序启动设置线程最小线程数:{isSuccess}");
#endregion

Console.WriteLine("-----------------------------------程序启动成功--------------------------------------");
app.Run();
