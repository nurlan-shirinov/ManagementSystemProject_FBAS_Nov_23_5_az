using Application;
using Application.Security;
using DAL.SqlServer;
using ManagementSystem.Api.Infrastructure;
using ManagementSystem.Api.Infrastructure.Middlewares;
using ManagementSystem.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IHttpContextAccessor , HttpContextAccessor>();
builder.Services.AddSwaggerService();
builder.Services.AddScoped<IUserContext, HttpUserContext>();

var conn = builder.Configuration.GetConnectionString("MyConn");
builder.Services.AddSqlServerServices(conn!);
builder.Services.AddApplicationServices();
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseMiddleware<RateLimitMiddleware>(2, TimeSpan.FromMinutes(1));
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();
