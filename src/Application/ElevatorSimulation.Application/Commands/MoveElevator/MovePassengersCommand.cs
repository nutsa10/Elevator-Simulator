using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Models;

namespace ElevatorSimulation.Application.Commands.MoveElevator;

public class MovePassengersCommand : ICommand
{
    public Elevator Elevator { get; set; }
    public int CurrentFloor { get; set; }
    public int DestinationFloor { get; set; }
}