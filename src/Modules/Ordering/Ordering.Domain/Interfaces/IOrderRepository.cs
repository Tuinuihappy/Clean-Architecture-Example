using CleanArchitectureDemo.Modules.Ordering.Domain.Entities;

namespace CleanArchitectureDemo.Modules.Ordering.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
}
