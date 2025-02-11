using DAL.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DAL.SqlServer;

public static class DependencyInjections
{

    public static IServiceCollection AddSqlServerServices(this IServiceCollection services, string connectionstring)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionstring));
        return services;
    }
}