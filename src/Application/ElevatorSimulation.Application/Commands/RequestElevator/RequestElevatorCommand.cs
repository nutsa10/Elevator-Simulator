using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;

namespace ElevatorSimulation.Application.Commands.RequestElevator;

public class RequestElevatorCommand : ICommand
{
    public int CurrentFloor { get; set; }
    public int DestinationFloor { get; set; }
    public int NumOfPassengers { get; set; }
}