using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
namespace Application.Services;

/// <summary>
/// Service to manage trader-related operations, such as placing orders and handling traders.
/// </summary>
public class TraderService : ITraderService
{
    private readonly ITraderRepository _traderRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMessagingPublisher _messagingPublisher;

    public TraderService(
        ITraderRepository traderRepository,
        IOrderRepository orderRepository,
        IMessagingPublisher messagingPublisher)
    {
        _traderRepository = traderRepository;
        _orderRepository = orderRepository;
        _messagingPublisher = messagingPublisher;
    }

    /// <summary>
    /// Places a stock order for a trader and publishes it to the message queue.
    /// </summary>
    public async Task PlaceOrderAsync(Guid traderId, string stockSymbol, int quantity, decimal price, string orderType)
    {
        // Fetch the trader
        var trader = await _traderRepository.GetByIdAsync(traderId);
        if (trader == null)
            throw new KeyNotFoundException("Trader not found.");

        // Place the order using Trader's method
        trader.PlaceOrder(stockSymbol, quantity, price, orderType);

        // Create the StockOrder
        var order = new StockOrder
        {
            Id = Guid.NewGuid(),
            TraderId = traderId,
            StockSymbol = stockSymbol,
            Quantity = quantity,
            Price = price,
            OrderType = orderType,
            CreatedAt = DateTime.UtcNow
        };

        // Save the updated trader
        await _traderRepository.UpdateAsync(trader);

        // Save the order
        await _orderRepository.AddAsync(order);

        // Publish the order to the message queue
        await _messagingPublisher.PublishAsync("order_queue", order);

        await _messagingPublisher.PublishAsync("notifications", new
        {
            Type = "OrderPlaced",
            Message = $"Order placed for trader {traderId} on stock {stockSymbol}",
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Adds a new trader to the system.
    /// </summary>
    public async Task AddTraderAsync(Trader trader)
    {
        await _traderRepository.AddAsync(trader);
    }

    /// <summary>
    /// Retrieves all traders in the system.
    /// </summary>
    public async Task<List<Trader>> GetAllTradersAsync()
    {
        return await _traderRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a specific trader by their ID.
    /// </summary>
    public async Task<Trader?> GetTraderByIdAsync(Guid traderId)
    {
        return await _traderRepository.GetByIdAsync(traderId);
    }

    /// <summary>
    /// Deletes a trader from the system.
    /// </summary>
    public async Task DeleteTraderAsync(Guid traderId)
    {
        var trader = await _traderRepository.GetByIdAsync(traderId);
        if (trader == null)
            throw new KeyNotFoundException("Trader not found.");

        await _traderRepository.DeleteAsync(trader);
    }
}
