using MediatR;

namespace ElevatorSimulation.Application.Interfaces.MediatorAdaptor;

public interface IApplicationRequestHandler<in TRequest, TResponse>: IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>;

public interface IApplicationRequestHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest;