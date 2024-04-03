using CH.Simple.APP.API;
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
builder.AddSqlSugar();
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
