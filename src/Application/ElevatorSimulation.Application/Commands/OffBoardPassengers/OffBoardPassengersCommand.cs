using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Models;

namespace ElevatorSimulation.Application.Commands.OffBoardPassengers;

public class OffBoardPassengersCommand : ICommand
{
    public Elevator Elevator { get; set; }
    public int NumOfPassengers { get; set; }
    
}