﻿using MediatR;

namespace ElevatorSimulation.Application.Interfaces.MediatorAdaptor;

public interface IApplicationMediator
{
    Task Send(IRequest request, CancellationToken cancellationToken = default);
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}