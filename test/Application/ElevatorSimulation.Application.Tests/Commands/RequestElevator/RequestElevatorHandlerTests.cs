using ElevatorSimulation.Application.Commands.MoveElevator;
using ElevatorSimulation.Application.Commands.OffBoardPassengers;
using ElevatorSimulation.Application.Commands.OnBoardPassengers;
using ElevatorSimulation.Application.Commands.RequestElevator;
using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Enums;
using Moq;

namespace ElevatorSimulation.Application.Tests.Commands.RequestElevator;

public class RequestElevatorHandlerTests
{
    private readonly Mock<IElevatorCacheRepository> _repository;
    private readonly Mock<IApplicationMediator> _mediator;
    private readonly RequestElevatorHandler _handler;
    
    public RequestElevatorHandlerTests()
    {
        _repository = new Mock<IElevatorCacheRepository>();
        _mediator = new Mock<IApplicationMediator>();
        _handler = new RequestElevatorHandler(_repository.Object, _mediator.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldFindClosestElevator_AndSendCommands()
    {
        // Arrange
        var building = new Building
        {
            Elevators =
            [
                new Elevator(1, 20, ElevatorType.Passenger),
                new Elevator(2, 20, ElevatorType.Passenger),
                new Elevator(3, 20, ElevatorType.Passenger)
            ]
        };

        building.Elevators[0].CurrentFloor = 6;
        building.Elevators[1].CurrentFloor = 0;
        building.Elevators[2].CurrentFloor = 2;
        _repository.Setup(repo => repo.GetState()).Returns(building);

        var request = new RequestElevatorCommand
        {
            CurrentFloor = 7,
            DestinationFloor = 10,
            NumOfPassengers = 5
        };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert (Verify that the closest elevator (Id = 1, CurrentFloor = 5) was selected)
        _mediator.Verify(x => x.Send(It.Is<OnBoardPassengersCommand>(cmd =>
                cmd.Elevator.Id == 1 && cmd.NumOfPassengers == 5),It.IsAny<CancellationToken>()), Times.Once);

        _mediator.Verify(mediator => mediator.Send(It.Is<MovePassengersCommand>(cmd =>
            cmd.Elevator.Id == 1 && cmd.CurrentFloor == 7 && cmd.DestinationFloor == 10), It.IsAny<CancellationToken>()), Times.Once);

        _mediator.Verify(mediator => mediator.Send(It.Is<OffBoardPassengersCommand>(cmd =>
            cmd.Elevator.Id == 1 && cmd.NumOfPassengers == 5), It.IsAny<CancellationToken>()), Times.Once);
    }
}