using Domain.Entities;

namespace Application.Interfaces
{

    public interface ITraderManagement
    {
        Task AddTraderAsync(Trader trader);
        Task<List<Trader>> GetAllTradersAsync();
        Task<Trader?> GetTraderByIdAsync(Guid traderId);
        Task<List<StockOrder>> GetOrdersByTraderIdAsync(Guid traderId);
        Task DeleteTraderAsync(Guid traderId);
        
    }
}
