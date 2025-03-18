using ElevatorSimulation.App.Controllers;
using ElevatorSimulation.Domain.Constants;
using ElevatorSimulation.Domain.Exceptions;
using Moq;

namespace ElevatorSimulation.App.Tests.Controller;

[Collection("SequentialTestCollection")]
public class ElevatorRequestValidatorTests
{
    private readonly Mock<TextReader> _consoleReader = new();
    private readonly Mock<TextWriter> _consoleWriter = new();

    [Fact]
    public void GetAndValidateInput_ValidInput_ReturnsInput()
    {
        // Arrange
        _consoleReader.Setup(c => c.ReadLine()).Returns("5");
        const string message = "Enter a number:";
        var validator = new Func<int, bool>(x => x > 0);
        const string errorMessage = "Invalid input.";
        Console.SetIn(_consoleReader.Object);

        // Act
        var result = ElevatorRequestValidator.GetAndValidateInput(message, validator, errorMessage);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void GetAndValidateInput_InvalidInput_ThrowsBadRequestException()
    {
        // Arrange
        _consoleReader.Setup(c => c.ReadLine()).Returns("invalid input");
        const string message = "Enter a number:";
        var validator = new Func<int, bool>(x => x > 0);
        const string errorMessage = "Invalid input.";
        Console.SetIn(_consoleReader.Object);

        // Act & Assert
        Assert.Throws<BadRequestException>(() => ElevatorRequestValidator.GetAndValidateInput(message, validator, errorMessage));
    }

    [Fact]
    public void GetAndValidateInput_InvalidValidator_ThrowsBadRequestException()
    {
        // Arrange
        _consoleReader.Setup(c => c.ReadLine()).Returns("0");
        const string message = "Enter a number:";
        var validator = new Func<int, bool>(x => x > 0);
        const string errorMessage = "Invalid input.";
        Console.SetIn(_consoleReader.Object);
        
        // Act & Assert
        Assert.Throws<BadRequestException>(() => ElevatorRequestValidator.GetAndValidateInput(message, validator, errorMessage));
    }
    
    [Fact] 
    public void ValidatePassCode_ValidPassCode_AccessGranted()
     {
         // Arrange
         _consoleReader.Setup(c => c.ReadLine()).Returns(ElevatorConstants.Auth.ElevatorAdminPassCode.ToString());
         const int destinationFloor = 10;
         Console.SetIn(_consoleReader.Object);
         Console.SetOut(_consoleWriter.Object);
        
         // Act
         ElevatorRequestValidator.ValidatePassCode(destinationFloor);

         // Assert
         _consoleWriter.Verify(c => c.WriteLine("Access Granted."), Times.Once);
     }

    [Fact]
    public void ValidatePassCode_InvalidPassCode_ThrowsUnauthorizedException()
    {
        // Arrange
        _consoleReader.Setup(c => c.ReadLine()).Returns("wrong");
        const int destinationFloor = 10; //a floor that requires a passcode
        Console.SetIn(_consoleReader.Object);
        
        // Act & Assert
        Assert.Throws<UnauthorizedException>(() => ElevatorRequestValidator.ValidatePassCode(destinationFloor));
    }

    [Fact]
    public void ValidatePassCode_NoPassCodeRequired_NoExceptionThrown()
    {
        // Arrange
        const int destinationFloor = 5; // a floor that doesn't require a passcode

        // Act
        ElevatorRequestValidator.ValidatePassCode(destinationFloor);

        // Assert
        _consoleReader.Verify(c => c.ReadLine(), Times.Never);
    }
    
    [Fact]
    public void ValidateCurrentAndDestinationFloor_SameFloors_ThrowsBadRequestException()
    {
        // Arrange
        const int currentFloor = 5;
        const int destinationFloor = 5;

        // Act & Assert
        Assert.Throws<BadRequestException>(() => ElevatorRequestValidator.ValidateCurrentAndDestinationFloor(currentFloor, destinationFloor));
    }

    [Fact]
    public void ValidateCurrentAndDestinationFloor_DifferentFloors_NoExceptionThrown()
    {
        // Arrange
        const int currentFloor = 5;
        const int destinationFloor = 10;

        // Act
        ElevatorRequestValidator.ValidateCurrentAndDestinationFloor(currentFloor, destinationFloor);

        // Assert
        // No exception is thrown, so the test passes
    }
}