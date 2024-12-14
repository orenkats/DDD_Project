using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(StockOrder order)
    {
        await _dbContext.StockOrders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }
}
