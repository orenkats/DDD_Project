using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Infrastructure.Persistence;

public class TraderRepositoryTests
{
    private readonly AppDbContext _dbContext;
    private readonly TraderRepository _traderRepository;

    public TraderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _dbContext = new AppDbContext(options);
        _traderRepository = new TraderRepository(_dbContext);
    }

    [Fact]
    public async Task AddAsync_Should_AddTrader()
    {
        // Arrange
        var trader = new Trader { Id = Guid.NewGuid(), Name = "John Doe" };

        // Act
        await _traderRepository.AddAsync(trader);
        var result = await _dbContext.Traders.FindAsync(trader.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result?.Name);
    }
}
