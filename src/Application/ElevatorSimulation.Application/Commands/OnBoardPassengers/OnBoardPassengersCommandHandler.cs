using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Domain.Constants;

namespace ElevatorSimulation.Application.Commands.OnBoardPassengers;

public class OnBoardPassengersCommandHandler(IElevatorCacheRepository elevatorCacheRepository)
    : IApplicationRequestHandler<OnBoardPassengersCommand>
{
    public Task Handle(OnBoardPassengersCommand request, CancellationToken cancellationToken)
    {
        request.Elevator.PassengerCount -= request.NumOfPassengers;
        elevatorCacheRepository.UpdateState(request.Elevator);
        
        Console.WriteLine(ElevatorConstants.Messages.PassengersBoardedElevator(request.NumOfPassengers));
        return Task.CompletedTask;
    }
}