
namespace ElevatorSimulation.Application.Models;

public class Floor(int id)
{
    public int Id { get; set; } = id;
    /*private Queue<Passenger> WaitingPassengers { get; set; } = new();
    public void AddPassenger(Passenger passenger)
    {
        WaitingPassengers.Enqueue(passenger);
    }*/
}