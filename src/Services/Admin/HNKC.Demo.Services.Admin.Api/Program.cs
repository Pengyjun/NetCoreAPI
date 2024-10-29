using HNKC.Demo.Services.Admin.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.AddApplicationServices();
builder.AddSqlSugar();
builder.AddCustomSwagger();
builder.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
