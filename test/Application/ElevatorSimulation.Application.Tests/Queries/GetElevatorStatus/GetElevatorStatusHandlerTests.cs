using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Application.Queries.GetElevatorStatus;
using ElevatorSimulation.Domain.Enums;
using Moq;

namespace ElevatorSimulation.Application.Tests.Queries.GetElevatorStatus;

public class GetElevatorStatusHandlerTests
{
    private readonly Mock<IElevatorCacheRepository> _repository;
    private readonly GetElevatorStatusHandler _handler;
    
    public GetElevatorStatusHandlerTests()
    {
        _repository = new Mock<IElevatorCacheRepository>();
        _handler = new GetElevatorStatusHandler(_repository.Object);
    }
    
    [Fact]
    public async Task Handle_ReturnsCorrectElevatorStatus()
    {
        // Arrange
        var building = new Building
        {
            Floors = [new Floor(1), new Floor(2), new Floor(3)],
            Elevators =
            [
                new Elevator(1, 20, ElevatorType.Passenger)
                {
                    Id = 1,
                    CurrentFloor = 1,
                    DestinationFloor = 3,
                    CurrentDirection = Direction.Up,
                    PassengerCount = 2,
                    MaxPassengerCapacity = 10
                },

                new Elevator(2, 20, ElevatorType.Passenger)
                {
                    Id = 2,
                    CurrentFloor = 2,
                    DestinationFloor = 1,
                    CurrentDirection = Direction.Down,
                    PassengerCount = 5,
                    MaxPassengerCapacity = 10
                }
            ]
        };

        _repository.Setup(repo => repo.GetState()).Returns(building);

        var request = new GetElevatorStatusQuery();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var elevator1Status = result[0];
        Assert.Equal("Elevator 1 - Current Floor: 1/3, Destination Floor: 3/3, Direction: Up, Passengers: 2/10", elevator1Status);

        var elevator2Status = result[1];
        Assert.Equal("Elevator 2 - Current Floor: 2/3, Destination Floor: 1/3, Direction: Down, Passengers: 5/10", elevator2Status);
    }
}