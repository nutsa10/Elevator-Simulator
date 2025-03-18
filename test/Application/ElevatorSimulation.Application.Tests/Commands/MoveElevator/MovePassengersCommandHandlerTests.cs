using ElevatorSimulation.Application.Commands.MoveElevator;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Enums;
using Moq;

namespace ElevatorSimulation.Application.Tests.Commands.MoveElevator;

public class MovePassengersCommandHandlerTests
{
    private readonly Mock<IElevatorCacheRepository> _repository;
    private readonly MovePassengersCommandHandler _handler;
    
    public MovePassengersCommandHandlerTests()
    {
        _repository = new Mock<IElevatorCacheRepository>();
        _handler = new MovePassengersCommandHandler(_repository.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldMoveElevatorUp_WhenDestinationFloorIsAboveCurrentFloor()
    {
        // Arrange
        var elevator = new Elevator (1, 20, ElevatorType.Passenger)
        {
            IsMoving = false,
            CurrentFloor = 1,
            DestinationFloor = 5,
            CurrentDirection = Direction.Stationary
        };
        var command = new MovePassengersCommand { Elevator = elevator, CurrentFloor = 1, DestinationFloor = 5 };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(elevator.IsMoving); // Elevator should stop moving
        Assert.Equal(Direction.Stationary, elevator.CurrentDirection); // Elevator should be stationary
        Assert.Equal(5, elevator.CurrentFloor); // Elevator should reach the destination floor
        _repository.Verify(repo => repo.UpdateState(elevator), Times.Once); // Verify repository update
    }

    [Fact]
    public async Task Handle_ShouldMoveElevatorDown_WhenDestinationFloorIsBelowCurrentFloor()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger)
        {
            IsMoving = false,
            CurrentFloor = 5,
            DestinationFloor = 1,
            CurrentDirection = Direction.Stationary
        };
     
        var command = new MovePassengersCommand { Elevator = elevator, CurrentFloor = 5, DestinationFloor = 1 };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(elevator.IsMoving); // Elevator should stop moving
        Assert.Equal(Direction.Stationary, elevator.CurrentDirection); // Elevator should be stationary
        Assert.Equal(1, elevator.CurrentFloor); // Elevator should reach the destination floor
        _repository.Verify(repo => repo.UpdateState(elevator), Times.Once); // Verify repository update
    }

    [Fact]
    public async Task Handle_ShouldNotMoveElevator_WhenCurrentFloorEqualsDestinationFloor()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger)
        {
            IsMoving = false,
            CurrentFloor = 3,
            DestinationFloor = 3,
            CurrentDirection = Direction.Stationary
        };

        var command = new MovePassengersCommand { Elevator = elevator, CurrentFloor = 3, DestinationFloor = 3 };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(elevator.IsMoving); // Elevator should not move
        Assert.Equal(Direction.Stationary, elevator.CurrentDirection); // Elevator should remain stationary
        Assert.Equal(3, elevator.CurrentFloor); // Elevator should stay on the same floor
        _repository.Verify(repo => repo.UpdateState(elevator), Times.Once); // Verify repository update
    }

    [Fact]
    public async Task Handle_ShouldSimulateMovementWithDelay()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger)
        {
            IsMoving = false,
            CurrentFloor = 1,
            DestinationFloor = 3,
            CurrentDirection = Direction.Stationary
        };
     
        var command = new MovePassengersCommand { Elevator = elevator, CurrentFloor = 1, DestinationFloor = 3 };

        // Act
        var startTime = DateTime.UtcNow;
        await _handler.Handle(command, CancellationToken.None);
        var endTime = DateTime.UtcNow;

        // Assert
        var elapsedTime = endTime - startTime;
        Assert.True(elapsedTime.TotalSeconds >= 2); // Should take at least 2 seconds (1 second per floor)
    }
}