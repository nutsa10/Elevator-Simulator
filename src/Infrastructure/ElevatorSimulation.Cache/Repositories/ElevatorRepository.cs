using ElevatorSimulation.Application.Interfaces.Repositories;
using ElevatorSimulation.Application.Models;
using ElevatorSimulation.Domain.Constants;

namespace ElevatorSimulation.Cache.Repositories;

public class ElevatorRepository : IElevatorCacheRepository
{
    private readonly Dictionary<string, Building> _state = new();

    public void SetState(Building building)
    {
        _state[ElevatorConstants.Cache.Key] = building;
    }
    
    public void UpdateState(Elevator elevator)
    {
        var building = GetState();
        building.Elevators.RemoveAll(x => x.Id == elevator.Id);
        building.Elevators.Add(elevator);
        _state[ElevatorConstants.Cache.Key] = building;
    }

    public Building GetState()
    {
        return _state[ElevatorConstants.Cache.Key];
    }
}