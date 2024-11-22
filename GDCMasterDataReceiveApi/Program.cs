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

#region ��Ȩ�м��
//�����м��
builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddleware>();
#endregion

#region ���÷���IP
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//    // ������֪��������Ĵ���ʱ������Ϊtrue  
//    options.KnownNetworks.Clear();
//    options.KnownProxies.Clear();
//});

#endregion

#region ���ù����� ����json��ʽ��ʱ��
//����json��ʽ��ʱ��
builder.Services.AddControllers(options =>
{
    //��ӹ�����
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

#region ����httpcontent���ʹ���
builder.Services.AddHttpContextAccessor();
HttpContentAccessFactory.services = builder.Services;
builder.Services.AddEndpointsApiExplorer();
#endregion

#region IOCע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    //AutoFac��ʼ��
    AutoFacInit.Init(container);
});
#endregion

#region ע����־
//���������־
builder.Logging.ClearProviders();
//���Serilog�����������־
builder.Host.UseSerilog((host, logger) =>
{
    //��ȡSerilog�����ļ�
    var serilog = new ConfigurationManager()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("serilog.json", true)//��־�����ļ�
    .Build();
    //��־��ʽ���ģ��
    var outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" + new string('-', 100) + "{NewLine}";
    //��ȡ�����ļ�
    logger.ReadFrom.Configuration(serilog)
     .WriteTo.Console(LogEventLevel.Verbose, outputTemplate: outputTemplate);
});
#endregion

#region ����swagger
//����swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Description = "������token��ʽΪBearer xxxxxx(�м�����пո�)",
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
        Description = "�㺽��-������API�ĵ�",
        Title = "�㺽��-������API�ĵ�",
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

#region ���ÿ���
//���ÿ���
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", builder => builder.WithOrigins("http://localhost:5000")
                                              .AllowAnyMethod()
                                              .AllowAnyHeader()
                                              .WithExposedHeaders(new string[] { "Authorization", "ClientId" })
                                              );
});
#endregion

#region ����JWT��֤
//������֤
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    var securyKeyByte = Encoding.UTF8.GetBytes(AppsettingsHelper.GetValue("Authentication:SecurityKey"));
    //��֤token��ز���
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

#region ע�����ݿ�������
//ע�����ݿ�������
builder.Services.AddSqlSugarContext(builder.Configuration, AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
#endregion

#region ���ʼ������Ҫ��ʼ���ı�ṹ������ע�ʹ˷���
#if DEBUG
  //CodeFirst.InitTable(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
#endif
#endregion

#region ע��AutoMapper
//ע��AutoMapper
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
//�����֤
app.UseAuthentication();
//��Ȩ
app.UseAuthorization();
app.MapControllers();
//������������Ϊû����ʾ������־
#region �����߳���  ����ϵͳ�Ż�
int workThread = 0, miPortThreads = 0;
ThreadPool.GetMinThreads(out workThread, out miPortThreads);
var isSuccess=ThreadPool.SetMinThreads(250, miPortThreads);
Console.WriteLine($"�������������߳���С�߳���:{isSuccess}");
#endregion

Console.WriteLine("-----------------------------------���������ɹ�--------------------------------------");
app.Run();
