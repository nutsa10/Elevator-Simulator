using ElevatorSimulation.App.Controllers;
using ElevatorSimulation.Application.Commands.RequestElevator;
using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Queries.GetElevatorStatus;
using ElevatorSimulation.Domain.Constants;
using ElevatorSimulation.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElevatorSimulation.App.Tests.Controller;

[Collection("SequentialTestCollection")]
public class ElevatorElevatorControllerTests
{
    private readonly Mock<ILogger<ElevatorElevatorController>> _logger;
    private readonly Mock<IApplicationMediator> _mediator;
    private readonly ElevatorElevatorController _controller;
    private readonly Mock<TextReader> _consoleReader = new();
    
    public ElevatorElevatorControllerTests()
    {
        _mediator = new Mock<IApplicationMediator>();
        _logger = new Mock<ILogger<ElevatorElevatorController>>();
        _controller = new ElevatorElevatorController(_mediator.Object, _logger.Object);
    }
    
    [Fact]
    public async Task Start_ShouldCallElevator_WhenUserChoosesOption1()
    {
        // Arrange
        _consoleReader.SetupSequence(x => x.ReadLine())
            .Returns("1") // User chooses option 1
            .Returns("5") // Current floor
            .Returns("10") // Destination floor
            .Returns(ElevatorConstants.Auth.ElevatorAdminPassCode.ToString) // Valid Passcode
            .Returns("5") // passangers
            .Returns(ElevatorConstants.Auth.ElevatorAdminKey); // User chooses option 3 (back office user exit)
        Console.SetIn(_consoleReader.Object);

        // Act
        await _controller.Start();
        
        // Assert
        _mediator.Verify(x => x.Send(It.Is<RequestElevatorCommand>(cmd =>
            cmd.CurrentFloor == 5 && cmd.DestinationFloor == 10 && cmd.NumOfPassengers == 5), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Start_ShouldDisplayElevatorState_WhenUserChoosesOption2()
     {
         // Arrange
         _consoleReader.SetupSequence(x => x.ReadLine())
             .Returns("2") // User chooses option 2
             .Returns(ElevatorConstants.Auth.ElevatorAdminKey); 
         Console.SetIn(_consoleReader.Object);

         // Act
         await _controller.Start();

         // Assert
         _mediator.Verify(x => x.Send(It.IsAny<GetElevatorStatusQuery>(), 
             It.IsAny<CancellationToken>()), Times.Once);
     }

    [Fact]
    public async Task Start_ShouldLogError_WhenExceptionIsThrown()
     {
         // Arrange
         _consoleReader.SetupSequence(x => x.ReadLine())
             .Returns("1") // User chooses option 1
             .Returns("5") // Current floor
             .Returns("10") // Destination floor
             .Returns("123") // Invalid Passcode
             .Returns("5") // passengers
             .Returns(ElevatorConstants.Auth.ElevatorAdminKey); // User chooses option 3 (back office user exit)
         Console.SetIn(_consoleReader.Object);

         // Act
         await _controller.Start();

         // Assert
         _logger.Verify(x => x.Log(
             LogLevel.Error,                      
             It.IsAny<EventId>(),                 
             It.IsAny<It.IsAnyType>(), 
             It.IsAny<UnauthorizedException>(), 
             It.IsAny<Func<It.IsAnyType, Exception?, string>>() 
         ), Times.Once);
     }
}