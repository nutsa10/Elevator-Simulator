using ElevatorSimulation.Application.Models;

namespace ElevatorSimulation.Application.Interfaces.Repositories;

public interface IElevatorCacheRepository
{
    void SetState(Building building);
    void UpdateState(Elevator elevator);
    Building GetState();
}