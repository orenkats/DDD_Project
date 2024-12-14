using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(StockOrder order);
}
