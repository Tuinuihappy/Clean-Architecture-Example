using CleanArchitectureDemo.Modules.Ordering.Domain.Entities;
using CleanArchitectureDemo.Modules.Ordering.Domain.Interfaces;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure.Data;

namespace CleanArchitectureDemo.Modules.Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingDbContext _dbContext;

    public OrderRepository(OrderingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }
}
