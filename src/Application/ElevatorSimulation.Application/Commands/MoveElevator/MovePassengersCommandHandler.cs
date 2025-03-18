using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Domain.Constants;
using ElevatorSimulation.Domain.Enums;

namespace ElevatorSimulation.Application.Commands.MoveElevator;

public class MovePassengersCommandHandler(IElevatorCacheRepository elevatorCacheRepository)
    : IApplicationRequestHandler<MovePassengersCommand>
{
    public Task Handle(MovePassengersCommand request, CancellationToken cancellationToken)
    {
        request.Elevator.IsMoving = true;
        request.Elevator.CurrentFloor = request.CurrentFloor;
        request.Elevator.DestinationFloor = request.DestinationFloor;
        request.Elevator.CurrentDirection = request.DestinationFloor > request.CurrentFloor ? Direction.Up : Direction.Down;

        while (request.Elevator.CurrentFloor != request.DestinationFloor)
        {
            var nextFloor = request.Elevator.CurrentDirection == Direction.Up ? 1 : -1;
            Console.WriteLine(ElevatorConstants.Messages.ElevatorMoving(request.Elevator.CurrentDirection, request.Elevator.CurrentFloor + nextFloor));
            Thread.Sleep(ElevatorConstants.Building.ElevatorMovementDelayInMilliseconds); // Simulate movement with a delay
            request.Elevator.CurrentFloor += nextFloor;
        }

        request.Elevator.IsMoving = false;
        request.Elevator.CurrentDirection = Direction.Stationary;
        elevatorCacheRepository.UpdateState(request.Elevator);
        return Task.CompletedTask;
    }
}