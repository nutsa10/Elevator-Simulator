using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Cache.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulation.Cache;

public static class DependencyInjection
{
    public static void InfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IElevatorCacheRepository,ElevatorRepository>();
    }
}