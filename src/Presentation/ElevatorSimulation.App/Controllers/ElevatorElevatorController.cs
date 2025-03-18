using ElevatorSimulation.Application.Commands.RequestElevator;
using ElevatorSimulation.Application.Commands.SetupBuilding;
using ElevatorSimulation.Application.Interfaces.Controller;
using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Queries.GetElevatorStatus;
using ElevatorSimulation.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace ElevatorSimulation.App.Controllers;

public class ElevatorElevatorController(
    IApplicationMediator mediator, 
    ILogger<ElevatorElevatorController> logger) : IElevatorController
{
    public async Task Start()
    {
        await ConstructBuilding();

        while (true)
        {
            Console.WriteLine(ElevatorConstants.Messages.Option1);
            Console.WriteLine(ElevatorConstants.Messages.Option2);
            var choice = Console.ReadLine();

            try
            {
                if (choice == "1")
                {
                    var currentFloor = ElevatorRequestValidator.GetAndValidateInput(
                        ElevatorConstants.Messages.EnterCurrentFloorNumber,
                        input => ElevatorConstants.Building.FloorAccess["*"].Contains(input),
                        ElevatorConstants.Messages.InvalidFloorNumber
                    );

                    var destinationFloor = ElevatorRequestValidator.GetAndValidateInput(
                        ElevatorConstants.Messages.EnterDestinationFloorNumber,
                        input => ElevatorConstants.Building.FloorAccess["*"].Contains(input),
                        ElevatorConstants.Messages.InvalidFloorNumber
                    );

                    ElevatorRequestValidator.ValidateCurrentAndDestinationFloor(currentFloor, destinationFloor);

                    ElevatorRequestValidator.ValidatePassCode(destinationFloor);

                    var passengers = ElevatorRequestValidator.GetAndValidateInput(
                        ElevatorConstants.Messages.EnterNumberOfPassengers,
                        input => input is >= 1 and <= ElevatorConstants.Building.MaxPassengerCapacityPerElevator,
                        ElevatorConstants.Messages.InvalidNumberOfPassenger
                    );

                    await RequestElevator(currentFloor, destinationFloor, passengers);
                }
                else if (choice == "2")
                {
                    await DisplayElevatorState();
                }
                else if (choice == ElevatorConstants.Auth.ElevatorAdminKey)
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.LogError(e.Message, e);
            }
        }
    }

    private async Task ConstructBuilding()
    {
        await mediator.Send(new SetupBuildingCommand
        {
            NumberOfFloors = ElevatorConstants.Building.NumberOfFloors,
            NumberOfElevators = ElevatorConstants.Building.NumOfElevators,
            MaxPassengerCapacityPerElevator = ElevatorConstants.Building.MaxPassengerCapacityPerElevator
        });
    }

    private async Task RequestElevator(int currentFloor, int destinationFloor, int passengers)
    {
        await mediator.Send(new RequestElevatorCommand
        {
            CurrentFloor = currentFloor,
            DestinationFloor = destinationFloor,
            NumOfPassengers = passengers
        });
    }

    private async Task DisplayElevatorState()
    {
        var lines = await mediator.Send(new GetElevatorStatusQuery());
        foreach (var line in lines)
            Console.WriteLine(line);
    }
}