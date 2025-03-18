using ElevatorSimulation.Application.Commands.OffBoardPassengers;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Constants;
using ElevatorSimulation.Domain.Enums;
using Moq;

namespace ElevatorSimulation.Application.Tests.Commands.OffBoardPassengers;

public class OffBoardPassengersCommandHandlerTests
{
    private readonly Mock<IElevatorCacheRepository> _repository;
    private readonly OffBoardPassengersCommandHandler _handler;
    
    public OffBoardPassengersCommandHandlerTests()
    {
        _repository = new Mock<IElevatorCacheRepository>();
        _handler = new OffBoardPassengersCommandHandler(_repository.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldIncreasePassengerCount_AndUpdateState()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger) { PassengerCount = 5 };
        var command = new OffBoardPassengersCommand { Elevator = elevator, NumOfPassengers = 3 };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(8, elevator.PassengerCount); // Check if PassengerCount is updated correctly
        _repository.Verify(repo => repo.UpdateState(elevator), Times.Once); // Verify UpdateState is called once with the correct elevator
    }

    [Fact]
    public async Task Handle_ShouldPrintCorrectMessageToConsole()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger) { PassengerCount = 5 };
        var command = new OffBoardPassengersCommand { Elevator = elevator, NumOfPassengers = 3 };

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains(ElevatorConstants.Messages.PassengersOffBoardedElevator(command.NumOfPassengers), consoleOutput.ToString()); // Verify the correct message is printed
    }
}