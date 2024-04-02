using CH.Simple.EntityFrameworkCore;
using CH.Simple.Web.DateTimeHandler;
using CH.Simple.Web.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SqlSugar;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new HttpGlobalExceptionFilter());
    options.Filters.Add(typeof(ValidateModelFilter));
    options.Filters.Add(new APIResultFilter());
}).AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverterExtension());

});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "手机APP服务端API接口",
        Version = "v1"
    });
    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    var xmlPath = Path.Combine(basePath, "CH.Simple.APP.API.xml");
    c.IncludeXmlComments(xmlPath);
});

var conn = builder.Configuration.GetConnectionString("MySQL");
#region EF Core
builder.Services.AddDbContext<SimpleContext>(options =>
{

    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
});
#endregion

#region SqlSuger
//注册上下文：AOP里面可以获取IOC对象，如果有现成框架比如Furion可以不写这一行
builder.Services.AddHttpContextAccessor();
//注册SqlSugar用AddScoped
builder.Services.AddScoped<ISqlSugarClient>(s =>
{
    //Scoped用SqlSugarClient 
    SqlSugarClient sqlSugar = new SqlSugarClient(new ConnectionConfig()
    {
        DbType = SqlSugar.DbType.MySql,
        ConnectionString = conn,
        IsAutoCloseConnection = true,
    },
   db =>
   {
       //每次上下文都会执行

       //获取IOC对象不要求在一个上下文
       //var log=s.GetService<Log>()

       //获取IOC对象要求在一个上下文
       //var appServive = s.GetService<IHttpContextAccessor>();
       //var log= appServive?.HttpContext?.RequestServices.GetService<Log>();

       db.Aop.OnLogExecuting = (sql, pars) =>
       {

       };
   });
    return sqlSugar;
});
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "v1");
});

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.Run();
