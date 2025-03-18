using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Enums;

namespace ElevatorSimulation.Application.Commands.SetupBuilding;

public class SetupBuildingCommandHandler(IElevatorCacheRepository repository)
    : IApplicationRequestHandler<SetupBuildingCommand>
{
    public Task Handle(SetupBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = new Building { Elevators = [], Floors = [] };

        for (var i = 0; i < request.NumberOfFloors; i++)
            building.Floors.Add(new Floor(i + 1));
        
        for (var i = 0; i < request.NumberOfElevators; i++)
            building.Elevators.Add(new Elevator(i + 1, request.MaxPassengerCapacityPerElevator, ElevatorType.Passenger));

        repository.SetState(building);
        return Task.CompletedTask;
    }
}