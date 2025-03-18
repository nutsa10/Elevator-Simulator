using ElevatorSimulation.Application.Commands.OnBoardPassengers;
using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Constants;
using ElevatorSimulation.Domain.Enums;
using Moq;

namespace ElevatorSimulation.Application.Tests.Commands.OnBoardPassengers;

public class OnBoardPassengersCommandHandlerTests
{
    private readonly Mock<IElevatorCacheRepository> _repository;
    private readonly OnBoardPassengersCommandHandler _handler;
    
    public OnBoardPassengersCommandHandlerTests()
    {
        _repository = new Mock<IElevatorCacheRepository>();
        _handler = new OnBoardPassengersCommandHandler(_repository.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldDecreasePassengerCount_AndUpdateState()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger) { PassengerCount = 10 };
        var command = new OnBoardPassengersCommand
        {
            Elevator = elevator,
            NumOfPassengers = 3
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(7, elevator.PassengerCount); // Verify passenger count is decreased
        _repository.Verify(repo => repo.UpdateState(elevator), Times.Once); // Verify UpdateState is called once
    }

    [Fact]
    public async Task Handle_ShouldOutputCorrectMessage()
    {
        // Arrange
        var elevator = new Elevator(1, 20, ElevatorType.Passenger) { PassengerCount = 10 };
        var command = new OnBoardPassengersCommand
        {
            Elevator = elevator,
            NumOfPassengers = 3
        };

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains(ElevatorConstants.Messages.PassengersBoardedElevator(command.NumOfPassengers), consoleOutput.ToString());
    }
}