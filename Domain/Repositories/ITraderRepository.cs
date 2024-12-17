using Domain.Entities;

namespace Domain.Interfaces;

public interface ITraderRepository
{
    Task<Trader?> GetByIdAsync(Guid traderId);
    Task<List<Trader>> GetAllAsync();
    Task AddAsync(Trader trader);
    Task UpdateAsync(Trader trader);
    Task DeleteAsync(Trader trader);
    Task<List<StockOrder>> GetOrdersByTraderIdAsync(Guid traderId);
}
