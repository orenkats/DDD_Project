using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Infrastructure.Persistence;

/// <summary>
/// Tests for the OrderRepository implementation.
/// </summary>
public class OrderRepositoryTests
{
    private readonly AppDbContext _dbContext;
    private readonly OrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_OrderRepository")
            .Options;

        _dbContext = new AppDbContext(options);
        _orderRepository = new OrderRepository(_dbContext);
    }

    [Fact]
    public async Task AddAsync_Should_AddOrderToDatabase()
    {
        // Arrange
        var order = new StockOrder
        {
            Id = Guid.NewGuid(),
            TraderId = Guid.NewGuid(),
            StockSymbol = "AAPL",
            Quantity = 10,
            Price = 150.5m,
            OrderType = "Buy",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _orderRepository.AddAsync(order);
        var result = await _dbContext.StockOrders.FindAsync(order.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("AAPL", result?.StockSymbol);
        Assert.Equal(150.5m, result?.Price);
    }
}
