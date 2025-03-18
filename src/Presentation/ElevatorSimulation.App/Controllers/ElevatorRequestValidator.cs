using ElevatorSimulation.Domain.Constants;
using ElevatorSimulation.Domain.Exceptions;

namespace ElevatorSimulation.App.Controllers;

public static class ElevatorRequestValidator
{
    public static int GetAndValidateInput(string message, Func<int, bool> validator, string errorMessage)
    {
        Console.WriteLine(message);
        if (!int.TryParse(Console.ReadLine(), out var input) || !validator(input))
            throw new BadRequestException(errorMessage);
        return input;
    }
    
    public static void ValidatePassCode(int destinationFloor)
    {
        if (!ElevatorConstants.Building.FloorAccess[ElevatorConstants.Auth.ElevatorAdminKey].Contains(destinationFloor)) return;
        Console.WriteLine(ElevatorConstants.Messages.EnterPassCode);
        if (Console.ReadLine() != ElevatorConstants.Auth.ElevatorAdminPassCode.ToString())
            throw new UnauthorizedException(ElevatorConstants.Messages.FloorAccessDenied);
        Console.WriteLine(ElevatorConstants.Messages.FloorAccessGranted);
    }
    
    public static void ValidateCurrentAndDestinationFloor(int currentFloor, int destinationFloor)
    {
        if (currentFloor == destinationFloor)
            throw new BadRequestException(ElevatorConstants.Messages.ElevatorAtDestinationFloor);
    }
}