using Domain.Entities;

namespace Domain.Interfaces;

public interface ITraderRepository
{
    Task AddAsync(Trader trader);
    Task DeleteAsync(Trader trader);
    Task UpdateAsync(Trader trader);
    Task<Trader?> GetByIdAsync(Guid traderId);
    Task<List<Trader>> GetAllAsync();
    Task<List<StockOrder>> GetOrdersByTraderIdAsync(Guid traderId);
}
