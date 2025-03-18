using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;

namespace ElevatorSimulation.Application.Commands.SetupBuilding;

public class SetupBuildingCommand : ICommand
{
    public int NumberOfFloors { get; set; }
    public int NumberOfElevators { get; set; }
    public int MaxPassengerCapacityPerElevator { get; set; }
}