using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.Services
{
    public class TraderActionsService : ITraderActions
    {
        private readonly ITraderRepository _traderRepository;

        public TraderActionsService(ITraderRepository traderRepository)
        {
            _traderRepository = traderRepository;
        }

        public async Task PlaceOrderAsync(Guid traderId, string stockSymbol, int quantity, decimal price, string orderType)
        {
            // Fetch the trader from the repository
            var trader = await _traderRepository.GetByIdAsync(traderId);
            if (trader == null)
                throw new KeyNotFoundException("Trader not found.");

            if (quantity <= 0 || price <= 0)
                throw new ArgumentException("Quantity and price must be positive.");

            var totalCost = quantity * price;

            if (orderType.ToLower() == "buy" && trader.AccountBalance < totalCost)
                throw new InvalidOperationException("Insufficient funds.");

            // Update balance
            if (orderType.ToLower() == "buy")
                trader.AccountBalance -= totalCost;

            // Create the order and add it to the trader's list of orders
            trader.Orders.Add(new StockOrder
            {
                Id = Guid.NewGuid(),
                TraderId = trader.Id,
                StockSymbol = stockSymbol,
                Quantity = quantity,
                Price = price,
                OrderType = orderType,
                CreatedAt = DateTime.UtcNow
            });

            // Update the trader in the repository
            await _traderRepository.UpdateAsync(trader);
        }
    }
}
