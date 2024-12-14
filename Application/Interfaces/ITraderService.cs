using Domain.Entities;

namespace Application.Interfaces;
/// <summary>
/// Defines the contract for trader-related operations.
/// </summary>
public interface ITraderService
{
    /// <summary>
    /// Places a stock order for a trader.
    /// </summary>
    /// <param name="traderId">The ID of the trader placing the order.</param>
    /// <param name="stockSymbol">The stock symbol being traded.</param>
    /// <param name="quantity">The quantity of the stock.</param>
    /// <param name="price">The price per unit of the stock.</param>
    /// <param name="orderType">The type of the order (e.g., buy, sell).</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task PlaceOrderAsync(Guid traderId, string stockSymbol, int quantity, decimal price, string orderType);

    /// <summary>
    /// Adds a new trader to the system.
    /// </summary>
    /// <param name="trader">The trader entity to add.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddTraderAsync(Trader trader);

    /// <summary>
    /// Retrieves all traders in the system.
    /// </summary>
    /// <returns>A list of all traders.</returns>
    Task<List<Trader>> GetAllTradersAsync();

    /// <summary>
    /// Retrieves a specific trader by their ID.
    /// </summary>
    /// <param name="traderId">The ID of the trader to retrieve.</param>
    /// <returns>The trader entity, or null if not found.</returns>
    Task<Trader?> GetTraderByIdAsync(Guid traderId);

    /// <summary>
    /// Deletes a trader from the system by their ID.
    /// </summary>
    /// <param name="traderId">The ID of the trader to delete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteTraderAsync(Guid traderId);
}
