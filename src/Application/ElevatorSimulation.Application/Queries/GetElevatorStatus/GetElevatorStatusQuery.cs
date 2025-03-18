using MediatR;

namespace ElevatorSimulation.Application.Queries.GetElevatorStatus;

public class GetElevatorStatusQuery : IRequest<List<string>>;