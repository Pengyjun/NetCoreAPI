<<<<<<<< HEAD:src/Services/Admin/HNKC.CrewManagePlatform.Services.Admin.Api/Program.cs
using HNKC.CrewManagePlatform.Services.Admin.Api;
using HNKC.CrewManagePlatform.SqlSugars;
using SqlSugar;
using UtilsSharp;
========
using HNKC.Demo.Services.Admin.Api;
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/Services/Admin/HNKC.Demo.Services.Admin.Api/Program.cs

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.AddApplicationServices();
builder.AddSqlSugar();
builder.AddCustomSwagger();
builder.AddCors();
//�ļ��ϴ�
builder.AddConfigUpload();
//ע��AutoMapper
builder.AddAutoMapper();
//��ʼ����
//CodeFirst.InitTable(AppsettingsHelper.GetValue("ConnectionStrings:ConnectionString"));
//�Զ�ע��ҵ��ӿڷ���
builder.AddAutoDependencyInjection();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
