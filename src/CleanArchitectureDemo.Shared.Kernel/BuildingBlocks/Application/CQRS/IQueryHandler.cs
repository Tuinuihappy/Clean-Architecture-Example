using MediatR;

namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
