using System;
using Domain.Entities;
using Xunit;

namespace Tests.Domain.Entities;

/// <summary>
/// Unit tests for the Trader entity.
/// </summary>
public class TraderTests
{
    [Fact]
    public void PlaceOrder_Should_AddOrderToTrader()
    {
        // Arrange
        var trader = new Trader { Id = Guid.NewGuid(), Name = "John Doe" };
        var stockSymbol = "AAPL";
        var quantity = 10;
        var price = 150.5m;
        var orderType = "Buy";

        // Act
        trader.PlaceOrder(stockSymbol, quantity, price, orderType);

        // Assert
        Assert.Single(trader.Orders); // Ensure one order is added
        var order = trader.Orders[0];
        Assert.Equal(stockSymbol, order.StockSymbol);
        Assert.Equal(quantity, order.Quantity);
        Assert.Equal(price, order.Price);
        Assert.Equal(orderType, order.OrderType);
        Assert.Equal(trader.Id, order.TraderId); // Ensure the order is linked to the trader
    }

    [Fact]
    public void PlaceOrder_Should_ThrowException_When_QuantityIsZeroOrNegative()
    {
        // Arrange
        var trader = new Trader { Id = Guid.NewGuid(), Name = "John Doe" };
        var stockSymbol = "AAPL";
        var invalidQuantity = -5;
        var price = 150.5m;
        var orderType = "Buy";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            trader.PlaceOrder(stockSymbol, invalidQuantity, price, orderType));

        Assert.Equal("Quantity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void PlaceOrder_Should_ThrowException_When_PriceIsZeroOrNegative()
    {
        // Arrange
        var trader = new Trader { Id = Guid.NewGuid(), Name = "John Doe" };
        var stockSymbol = "AAPL";
        var quantity = 10;
        var invalidPrice = 0m;
        var orderType = "Buy";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            trader.PlaceOrder(stockSymbol, quantity, invalidPrice, orderType));

        Assert.Equal("Price must be greater than zero.", exception.Message);
    }
}
