using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests.Application.Services
{
    [TestFixture]
    public class TraderActionsServiceTests
    {
        private Mock<ITraderRepository> _mockTraderRepository = null!;
        private TraderActionsService _service = null!;
        private Trader _sampleTrader = null!;

        [SetUp]
        public void Setup()
        {
            _mockTraderRepository = new Mock<ITraderRepository>();

            // Sample Trader setup
            _sampleTrader = new Trader
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john@example.com",
                AccountBalance = 5000m, // Initial balance
                Orders = new List<StockOrder>()
            };

            // Mock GetByIdAsync to return the sample trader
            _mockTraderRepository.Setup(repo => repo.GetByIdAsync(_sampleTrader.Id))
                .ReturnsAsync(_sampleTrader);

            _service = new TraderActionsService(_mockTraderRepository.Object);
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldAddOrderToTrader_WhenOrderIsValid()
        {
            // Arrange
            var stockSymbol = "AAPL";
            var quantity = 10;
            var price = 100m;
            var orderType = "buy";
            var expectedTotalCost = quantity * price;

            // Act
            await _service.PlaceOrderAsync(_sampleTrader.Id, stockSymbol, quantity, price, orderType);

            // Assert
            // Verify the order was added
            Assert.That(_sampleTrader.Orders.Count, Is.EqualTo(1));
            var placedOrder = _sampleTrader.Orders[0];

            Assert.Multiple(() =>
            {
                Assert.That(placedOrder.StockSymbol, Is.EqualTo(stockSymbol));
                Assert.That(placedOrder.Quantity, Is.EqualTo(quantity));
                Assert.That(placedOrder.Price, Is.EqualTo(price));
                Assert.That(placedOrder.OrderType, Is.EqualTo(orderType));
            });

            // Verify balance was updated
            var expectedBalance = 5000m - expectedTotalCost; // 4000m
            Assert.That(_sampleTrader.AccountBalance, Is.EqualTo(expectedBalance));
        }

        [Test]
        public void PlaceOrderAsync_ShouldThrowException_WhenTraderNotFound()
        {
            // Arrange
            var invalidTraderId = Guid.NewGuid();
            _mockTraderRepository.Setup(repo => repo.GetByIdAsync(invalidTraderId))
                .ReturnsAsync((Trader)null); // Return null for invalid ID

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.PlaceOrderAsync(invalidTraderId, "AAPL", 10, 100m, "buy"));
            Assert.That(ex.Message, Is.EqualTo("Trader not found."));
        }
    }
}
