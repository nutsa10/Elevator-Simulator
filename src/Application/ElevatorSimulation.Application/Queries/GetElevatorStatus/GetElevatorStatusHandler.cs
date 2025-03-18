using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Domain.Constants;

namespace ElevatorSimulation.Application.Queries.GetElevatorStatus;

public class GetElevatorStatusHandler(IElevatorCacheRepository elevatorCacheRepository)
    : IApplicationRequestHandler<GetElevatorStatusQuery, List<string>>
{
    public Task<List<string>> Handle(GetElevatorStatusQuery request, CancellationToken cancellationToken)
    {
        var building = elevatorCacheRepository.GetState();
        var response = building.Elevators
            .Select(elevator => ElevatorConstants.Messages.ElevatorStatus(
                elevator.Id, 
                elevator.CurrentFloor, 
                elevator.DestinationFloor, 
                building.Floors.Count, 
                elevator.CurrentDirection, 
                elevator.PassengerCount, 
                elevator.MaxPassengerCapacity)).ToList();
        return Task.FromResult(response);
    }
}