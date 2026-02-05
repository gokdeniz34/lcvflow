using LcvFlow.Service.Interfaces;
using LcvFlow.Service.Concretes;
using Microsoft.Extensions.DependencyInjection;
using LcvFlow.Service.Helpers;
using System.Reflection;

namespace LcvFlow.Service;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IAuthService, AuthService>();
        // services.AddScoped<IGuestService, GuestService>();

        return services;
    }
}