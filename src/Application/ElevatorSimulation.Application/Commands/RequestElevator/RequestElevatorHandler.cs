using ElevatorSimulation.Application.Commands.MoveElevator;
using ElevatorSimulation.Application.Commands.OffBoardPassengers;
using ElevatorSimulation.Application.Commands.OnBoardPassengers;
using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;

namespace ElevatorSimulation.Application.Commands.RequestElevator;

public class RequestElevatorHandler(IElevatorCacheRepository elevatorCacheRepository, 
    IApplicationMediator mediator)
    : IApplicationRequestHandler<RequestElevatorCommand>
{
    public async Task Handle(RequestElevatorCommand request, CancellationToken cancellationToken )
    {
        var building = elevatorCacheRepository.GetState();

        var closestElevator = building.Elevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.CurrentFloor)).First();
       
        await mediator.Send(new OnBoardPassengersCommand
        {
            Elevator = closestElevator,
            NumOfPassengers = request.NumOfPassengers
        }, cancellationToken);

        await mediator.Send(new MovePassengersCommand
        {
            Elevator = closestElevator,
            CurrentFloor = request.CurrentFloor,
            DestinationFloor = request.DestinationFloor
        }, cancellationToken);
        
        await mediator.Send(new OffBoardPassengersCommand
        {
            Elevator = closestElevator,
            NumOfPassengers = request.NumOfPassengers
        }, cancellationToken);
    }
}

