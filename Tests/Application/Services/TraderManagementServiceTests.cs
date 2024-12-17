using Application.Services;
using Domain.Interfaces;
using Domain.Entities;
using NUnit.Framework;
using Moq;

namespace Tests.Application.Services
{
    [TestFixture]
    public class TraderManagementServiceTests
    {
        private Mock<ITraderRepository> _mockRepo = null!;
        private TraderManagementService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ITraderRepository>();
            _service = new TraderManagementService(_mockRepo.Object);
        }

        [Test]
        public async Task AddTraderAsync_ShouldCallAddAsync_WhenTraderIsValid()
        {
            // Arrange
            var trader = new Trader { Name = "John Doe", AccountBalance = 1000m };

            // Act
            await _service.AddTraderAsync(trader);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(trader), Times.Once);
        }
    }
}
