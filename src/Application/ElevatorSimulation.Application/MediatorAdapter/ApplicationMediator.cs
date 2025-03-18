using ElevatorSimulation.Application.Interfaces.MediatorAdaptor;
using MediatR;

namespace ElevatorSimulation.Application.MediatorAdapter;

public class ApplicationMediator(IMediator mediator) : IApplicationMediator
{
    public Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        return mediator.Send(request, cancellationToken);
    }
        
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return mediator.Send(request, cancellationToken);
    }
}