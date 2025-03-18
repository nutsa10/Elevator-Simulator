using ElevatorSimulation.Application.Commands.SetupBuilding;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using Moq;

namespace ElevatorSimulation.Application.Tests.Commands.SetupBuilding;

public class SetupBuildingCommandHandlerTests
{
    private readonly Mock<IElevatorCacheRepository> _repository;
    private readonly SetupBuildingCommandHandler _handler;
    
    public SetupBuildingCommandHandlerTests()
    {
        _repository = new Mock<IElevatorCacheRepository>();
        _handler = new SetupBuildingCommandHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateBuildingWithCorrectNumberOfFloors()
    {
        // Arrange
        var command = new SetupBuildingCommand
        {
            NumberOfFloors = 5,
            NumberOfElevators = 2,
            MaxPassengerCapacityPerElevator = 10
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
       _repository.Verify(repo => repo.SetState(It.Is<Building>(b => b.Floors.Count == 5)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateBuildingWithCorrectNumberOfElevators()
    {
        // Arrange
        var command = new SetupBuildingCommand
        {
            NumberOfFloors = 5,
            NumberOfElevators = 2,
            MaxPassengerCapacityPerElevator = 10
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repository.Verify(repo => repo.SetState(It.Is<Building>(b => b.Elevators.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateElevatorsWithCorrectCapacity()
    {
        // Arrange
        var command = new SetupBuildingCommand
        {
            NumberOfFloors = 5,
            NumberOfElevators = 2,
            MaxPassengerCapacityPerElevator = 10
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repository.Verify(repo => repo.SetState(It.Is<Building>(b => 
            b.Elevators.TrueForAll(e => e.MaxPassengerCapacity == 10))), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateFloorsWithCorrectNumbers()
    {
        // Arrange
        var command = new SetupBuildingCommand
        {
            NumberOfFloors = 5,
            NumberOfElevators = 2,
            MaxPassengerCapacityPerElevator = 10
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repository.Verify(repo => repo.SetState(It.Is<Building>(b => 
            b.Floors[0].Id == 1 &&
            b.Floors[1].Id == 2 &&
            b.Floors[2].Id == 3 &&
            b.Floors[3].Id == 4 &&
            b.Floors[4].Id == 5)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSetStateInRepository()
    {
        // Arrange
        var command = new SetupBuildingCommand
        {
            NumberOfFloors = 5,
            NumberOfElevators = 2,
            MaxPassengerCapacityPerElevator = 10
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repository.Verify(repo => repo.SetState(It.IsAny<Building>()), Times.Once);
    }
}