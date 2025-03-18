namespace ElevatorSimulation.Application.Models;

public class Passenger(int destinationFloor, int numberOfPassengers)
{
    public int DestinationFloor { get; set; } = destinationFloor;
    public int NumberOfPassengers { get; set; } = numberOfPassengers;
}