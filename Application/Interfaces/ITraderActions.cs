using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines the actions that a trader can perform.
    /// </summary>
    public interface ITraderActions
    {
        
        Task PlaceOrderAsync(Guid traderId, string stockSymbol, int quantity, decimal price, string orderType);

    }
}
