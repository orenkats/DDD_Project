using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Tests.Application.Services;

public class TraderServiceTests
{
    private readonly Mock<ITraderRepository> _traderRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMessagingPublisher> _messagingPublisherMock; // Updated interface
    private readonly TraderService _traderService;

    public TraderServiceTests()
    {
        _traderRepositoryMock = new Mock<ITraderRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _messagingPublisherMock = new Mock<IMessagingPublisher>(); // Updated to use IMessagingPublisher
        _traderService = new TraderService(
            _traderRepositoryMock.Object,
            _orderRepositoryMock.Object,
            _messagingPublisherMock.Object // Updated dependency
        );
    }

    [Fact]
    public async Task PlaceOrderAsync_Should_AddOrder_And_PublishMessage()
    {
        // Arrange
        var traderId = Guid.NewGuid();
        var trader = new Trader { Id = traderId, Name = "John Doe" };
        _traderRepositoryMock.Setup(repo => repo.GetByIdAsync(traderId)).ReturnsAsync(trader);

        // Act
        await _traderService.PlaceOrderAsync(traderId, "AAPL", 10, 150.5m, "Buy");

        // Assert
        _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<StockOrder>()), Times.Once);
        _messagingPublisherMock.Verify(publisher => publisher.PublishAsync("order_queue", It.IsAny<StockOrder>()), Times.Once);
    }
}
