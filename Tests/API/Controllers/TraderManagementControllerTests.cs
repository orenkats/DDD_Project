
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using NUnit.Framework;
using API.Controllers;
using Domain.Entities;
using Moq;


namespace Tests.API.Controllers
{
    [TestFixture]
    public class TraderManagementControllerTests
    {
        private Mock<ITraderManagement> _mockTraderManagement = null!;
        private TraderManagementController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _mockTraderManagement = new Mock<ITraderManagement>();
            _controller = new TraderManagementController(_mockTraderManagement.Object);
        }

        [Test]
        public async Task AddTrader_ShouldReturnOk_WhenTraderIsAddedSuccessfully()
        {
            // Arrange
            var trader = new Trader 
            { 
                Id = Guid.NewGuid(), 
                Name = "John Doe", 
                AccountBalance = 1000m 
            };

            _mockTraderManagement.Setup(x => x.AddTraderAsync(It.IsAny<Trader>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddTrader(trader);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo("Trader added successfully."));
        }

        [Test]
        public async Task AddTrader_ShouldReturn400_WhenValidationFails()
        {
            // Arrange
            var invalidTrader = new Trader
            {
                Name = "",  // Invalid name
                AccountBalance = -100  // Invalid balance
            };

            _controller.ModelState.AddModelError("Name", "Trader name is required.");
            _controller.ModelState.AddModelError("AccountBalance", "Account balance must be non-negative.");

            // Act
            var result = await _controller.AddTrader(invalidTrader);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.StatusCode, Is.EqualTo(400));
        }

    }
}
