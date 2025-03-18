using ElevatorSimulation.Domain.Enums;

namespace ElevatorSimulation.Application.Models;

public class Elevator(int id, int maxPassengerCapacity, ElevatorType type)
{
    public int Id { get; set; } = id;
    public int CurrentFloor { get; set; } = 0;
    public int DestinationFloor { get; set; } = 0;
    public int PassengerCount { get; set; } = 0;
    public bool IsMoving { get; set; } = false;
    public ElevatorType Type { get; set; } = type;
    public int MaxPassengerCapacity { get; set; } = maxPassengerCapacity;
    public Direction CurrentDirection { get; set; } = Direction.Stationary;
}