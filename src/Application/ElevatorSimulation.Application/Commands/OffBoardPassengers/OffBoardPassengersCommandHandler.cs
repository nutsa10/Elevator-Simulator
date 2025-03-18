using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Domain.Constants;

namespace ElevatorSimulation.Application.Commands.OffBoardPassengers;

public class OffBoardPassengersCommandHandler(IElevatorCacheRepository elevatorCacheRepository)
    : IApplicationRequestHandler<OffBoardPassengersCommand>
{
    public Task Handle(OffBoardPassengersCommand request, CancellationToken cancellationToken)
    {
        request.Elevator.PassengerCount += request.NumOfPassengers;
        elevatorCacheRepository.UpdateState(request.Elevator);
        
        Console.WriteLine(ElevatorConstants.Messages.PassengersOffBoardedElevator(request.NumOfPassengers));
        return Task.CompletedTask;
    }
}