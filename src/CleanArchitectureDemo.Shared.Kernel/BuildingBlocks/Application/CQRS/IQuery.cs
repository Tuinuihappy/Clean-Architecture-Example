using MediatR;

namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
