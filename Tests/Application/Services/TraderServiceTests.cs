using Application.Services;
using Domain.Interfaces;
using Domain.Entities;
using NUnit.Framework;
using Moq;

namespace Tests.Application.Services
{
    [TestFixture]
    public class TraderServiceTests
    {
        private Mock<ITraderRepository> _mockRepo = null!;
        private TraderService _service = null!;

        public TraderServiceTests()
        {
            _mockRepo = new Mock<ITraderRepository>();
            _service = new TraderService(_mockRepo.Object);
        }

        [SetUp]
        public void Setup()
        {
            _mockRepo.Reset();
        }

        [Test]
        public async Task AddTraderAsync_ShouldCallAddAsync_WhenTraderIsValid()
        {
            // Arrange
            var trader = new Trader 
            { 
                Name = "John Doe", 
                AccountBalance = 1000m, 
                Email = "john.doe@example.com" // Add a valid email
            };

            // Act
            await _service.AddTraderAsync(trader);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(trader), Times.Once);
        }
    }
}
