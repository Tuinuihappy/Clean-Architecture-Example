using MediatR;

namespace CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
