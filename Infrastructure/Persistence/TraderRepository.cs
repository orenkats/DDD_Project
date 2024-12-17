using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class TraderRepository : ITraderRepository
{
    private readonly AppDbContext _dbContext;

    public TraderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Trader?> GetByIdAsync(Guid traderId)
    {
        return await _dbContext.Traders.FindAsync(traderId);
    }

    public async Task<List<Trader>> GetAllAsync()
    {
        return await _dbContext.Traders.ToListAsync();
    }

    public async Task AddAsync(Trader trader)
    {
        await _dbContext.Traders.AddAsync(trader);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trader trader)
    {
        _dbContext.Traders.Update(trader);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Trader trader)
    {
        _dbContext.Traders.Remove(trader);
        await _dbContext.SaveChangesAsync();
    }
    // Implement GetOrdersByTraderIdAsync
    public async Task<List<StockOrder>> GetOrdersByTraderIdAsync(Guid traderId)
    {
        var trader = await _dbContext.Traders
            .Include(t => t.Orders)
            .FirstOrDefaultAsync(t => t.Id == traderId);

        return trader?.Orders ?? new List<StockOrder>();
    }
}
