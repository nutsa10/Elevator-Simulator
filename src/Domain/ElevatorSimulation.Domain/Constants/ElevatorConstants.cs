using System.Collections.ObjectModel;
using ElevatorSimulation.Domain.Enums;

namespace ElevatorSimulation.Domain.Constants;

public static class ElevatorConstants
{
    public static class Building
    {
        public static readonly ReadOnlyDictionary<string, List<int>> FloorAccess = new(new Dictionary<string, List<int>>
        {
            { "*", [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },    //all floors
            { "public", [0, 1, 2, 3, 4, 5, 6, 7, 8] },      //public access
            { "admin", [9, 10] }                            //auth access
        });

        public const int NumberOfFloors = 10;
        public const int NumOfElevators = 3;
        public const int MaxPassengerCapacityPerElevator = 20;
        
        public const int ElevatorMovementDelayInMilliseconds = 1000;
    }

    public static class Auth
    {
        public const int ElevatorAdminPassCode = 1893;
        public const string ElevatorAdminKey = "admin";
    }
    
    public static class Cache
    {
        public const string Key = "state";
    }

    public static class Messages
    {
        public const string FloorAccessDenied = "Access Denied.";
        public const string FloorAccessGranted = "Access Granted.";
        public const string ElevatorAtDestinationFloor = "Elevator is already at the Destination floor.";
        public const string EnterPassCode = "Please enter passcode: ";
        public const string Option1 = "1. Call Elevator";
        public const string Option2 = "2. Display Elevator Status";
        public static readonly string EnterCurrentFloorNumber = $"Enter current floor number ({string.Join(", ", Building.FloorAccess["*"])}):";
        public static readonly string EnterDestinationFloorNumber = $"Enter destination floor number ({string.Join(", ", Building.FloorAccess["*"])}):";
        public const string InvalidFloorNumber = "Invalid floor number.";
        public const string InvalidNumberOfPassenger = "Invalid number of passengers.";
        public const string EnterNumberOfPassengers = "Enter number of passengers between (1 and 20):";
        public static string PassengersBoardedElevator(int numOfPassengers) => $"{numOfPassengers} Passengers boarded the elevator.";
        public static string PassengersOffBoardedElevator(int numOfPassengers) => $"{numOfPassengers} Passengers Off-boarded the elevator.";
        public static string ElevatorMoving(Direction direction, int nextFloor) => $"{ElevatorType.Passenger} Elevator moving {direction} to floor {nextFloor}";

        public static string ElevatorStatus(int elevatorId, int currentFloor, int destinationFloor, int floorCount, Direction direction, int numberOfPassengers, int maxPassengerCapacity) =>
            $"Elevator {elevatorId} - Current Floor: {currentFloor}/{floorCount}, Destination Floor: {destinationFloor}/{floorCount}, Direction: {direction}, Passengers: {numberOfPassengers}/{maxPassengerCapacity}";
    }
}