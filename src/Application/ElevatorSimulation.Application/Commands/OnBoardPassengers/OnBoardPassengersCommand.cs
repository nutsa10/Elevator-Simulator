using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using ElevatorSimulation.Application.Models;

namespace ElevatorSimulation.Application.Commands.OnBoardPassengers;

public class OnBoardPassengersCommand : ICommand
{
    public Elevator Elevator { get; set; }
    public int NumOfPassengers { get; set; }
    
}