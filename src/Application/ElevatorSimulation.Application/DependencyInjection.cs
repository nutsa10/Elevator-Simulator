using System.Reflection;
using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.MediatorAdapter;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulation.Application;

public static class DependencyInjection
{
    public static void ApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        services.AddScoped<IApplicationMediator, ApplicationMediator>();
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    }
}