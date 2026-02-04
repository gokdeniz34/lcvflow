using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LcvFlow.Data.Context;
using LcvFlow.Domain.Guests;
using LcvFlow.Data.Repositories;

namespace LcvFlow.Data;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
            b => b.MigrationsAssembly("LcvFlow.Data")));

        services.AddScoped<IGuestRepository, GuestRepository>();

        return services;
    }
}