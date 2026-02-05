using LcvFlow.Service.Interfaces;
using LcvFlow.Service.Concretes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using LcvFlow.Domain.Common;
using LcvFlow.Data;
using FluentValidation;

namespace LcvFlow.Service;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped<IGuestService, GuestService>();

        return services;
    }
}