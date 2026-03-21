using CleanArchitectureDemo.Modules.Catalog.Application.DTOs;
using CleanArchitectureDemo.Modules.Catalog.Domain.Interfaces;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application;
using CleanArchitectureDemo.Shared.Kernel.BuildingBlocks.Application.CQRS;

namespace CleanArchitectureDemo.Modules.Catalog.Application.Products.Queries;

public record GetProductByIdQuery(int ProductId) : IQuery<ProductDto>;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId);
        if (product == null) return Result<ProductDto>.Failure("Product not found");

        var dto = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price, StockQuantity = product.StockQuantity, CategoryId = product.CategoryId, Status = product.Status.ToString() };
        return Result<ProductDto>.Success(dto);
    }
}

public record GetProductsQuery() : IQuery<IEnumerable<ProductDto>>;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;

    public GetProductsQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync();
        var dtos = products.Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price, StockQuantity = p.StockQuantity, CategoryId = p.CategoryId, Status = p.Status.ToString() });
        return Result<IEnumerable<ProductDto>>.Success(dtos);
    }
}
