using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LcvFlow.Data.Context;
using LcvFlow.Data.Repositories;
using LcvFlow.Domain.Interfaces;
using LcvFlow.Domain;

namespace LcvFlow.Data;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 31)), // Serilog çakışmasını önlemek için sabitlendi
                b => b.MigrationsAssembly("LcvFlow.Data")));

        // Repositories
        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

        return services;
    }
}