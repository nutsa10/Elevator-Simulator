using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Enums;

namespace ElevatorSimulation.Cache.Tests;

public class ElevatorRepository
{
    [Fact]
    public void SetState_ShouldStoreBuildingInCache()
    {
        // Arrange
        var repository = new Repositories.ElevatorRepository();
        var building = new Building
        {
            Elevators = []
        };

        // Act
        repository.SetState(building);

        // Assert
        var cachedBuilding = repository.GetState();
        Assert.Equal(building, cachedBuilding);
    }

    [Fact]
    public void UpdateState_ShouldUpdateElevatorInBuilding()
    {
        // Arrange
        var repository = new Repositories.ElevatorRepository();
        var initialElevator = new Elevator(1, 20, ElevatorType.Passenger) { Id = 1, CurrentFloor = 1 };
        var updatedElevator = new Elevator(2, 20, ElevatorType.Passenger) { Id = 1, CurrentFloor = 2 };
        var building = new Building
        {
            Elevators = [initialElevator]
        };
        repository.SetState(building);

        // Act
        repository.UpdateState(updatedElevator);

        // Assert
        var cachedBuilding = repository.GetState();
        var elevatorInCache = cachedBuilding.Elevators.Find(e => e.Id == updatedElevator.Id);
        Assert.NotNull(elevatorInCache);
        Assert.Equal(updatedElevator.CurrentFloor, elevatorInCache.CurrentFloor);
    }

    [Fact]
    public void UpdateState_ShouldAddNewElevator_IfElevatorDoesNotExist()
    {
        // Arrange
        var repository = new Repositories.ElevatorRepository();
        var building = new Building
        {
            Elevators = []
        };
        repository.SetState(building);
        var newElevator = new Elevator(1, 20, ElevatorType.Passenger) { Id = 1, CurrentFloor = 1 };

        // Act
        repository.UpdateState(newElevator);

        // Assert
        var cachedBuilding = repository.GetState();
        var elevatorInCache = cachedBuilding.Elevators.Find(e => e.Id == newElevator.Id);
        Assert.NotNull(elevatorInCache);
        Assert.Equal(newElevator.CurrentFloor, elevatorInCache.CurrentFloor);
    }
}