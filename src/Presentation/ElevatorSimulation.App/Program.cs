using ElevatorSimulation.App.Controllers;
using ElevatorSimulation.Application;
using ElevatorSimulation.Application.Interfaces.Controller;
using ElevatorSimulation.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulation.App;

internal class Program
{
    public static async Task Main()
    {
        var serviceProvider = ConfigureServices();
        var elevatorController = serviceProvider.GetRequiredService<IElevatorController>();
        await elevatorController.Start();
    }
    
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.InfrastructureServices();
        services.ApplicationServices();
        services.AddScoped<IElevatorController, ElevatorElevatorController>();
        return services.BuildServiceProvider();
    }
}
