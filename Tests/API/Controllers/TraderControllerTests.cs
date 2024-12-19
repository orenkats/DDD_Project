using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using FluentValidation;
using API.Controllers;
using Domain.Entities;
using NUnit.Framework;
using Moq;

namespace Tests.API.Controllers
{
    [TestFixture]
    public class TraderControllerTests
    {
        private Mock<ITraderManagement> _mockTraderManagement;
        private TraderController _controller;

        public TraderControllerTests()
        {
            _mockTraderManagement = new Mock<ITraderManagement>();
            _controller = new TraderController(_mockTraderManagement.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _mockTraderManagement.Reset(); 
        }

        [Test]
        public async Task AddTrader_ShouldReturnOk_WhenTraderIsAddedSuccessfully()
        {
            _controller.ModelState.Clear();
            // Arrange
            var validTrader = new Trader
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                AccountBalance = 1000m,
                Email = "john@example.com"
            };

            _mockTraderManagement
                .Setup(x => x.AddTraderAsync(It.IsAny<Trader>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddTrader(validTrader);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo("Trader added successfully."));
        }

        [Test]
        public async Task AddTrader_ShouldReturn400_WhenNameIsInvalid()
        {
            // Arrange
            var invalidTrader = new Trader
            {
                Name = "",
                AccountBalance = 1000m,
                Email = "john@example.com"
            };
            AddModelStateError(_controller, "Name", "Trader name is required.");

            // Act
            var result = await _controller.AddTrader(invalidTrader);

            // Assert
            AssertBadRequest(result, "Name", "Trader name is required.");
        }

        [Test]
        public async Task AddTrader_ShouldReturn400_WhenAccountBalanceIsInvalid()
        {
            // Arrange
            var invalidTrader = new Trader
            {
                Name = "John Doe",
                AccountBalance = -100m,
                Email = "john@example.com"
            };
            AddModelStateError(_controller, "AccountBalance", "Account balance must be non-negative.");

            // Act
            var result = await _controller.AddTrader(invalidTrader);

            // Assert
            AssertBadRequest(result, "AccountBalance", "Account balance must be non-negative.");
        }

        [Test]
        public async Task AddTrader_ShouldReturn400_WhenEmailIsInvalid()
        {
            // Arrange
            var invalidTrader = new Trader
            {
                Name = "John Doe",
                AccountBalance = 1000m,
                Email = ""
            };
            AddModelStateError(_controller, "Email", "Email is required.");

            // Act
            var result = await _controller.AddTrader(invalidTrader);

            // Assert
            AssertBadRequest(result, "Email", "Email is required.");
        }

        // Helper method to add model state errors
        private void AddModelStateError(ControllerBase  controller, string key, string message)
        {
            controller.ModelState.AddModelError(key, message);
        }

        // Helper method to assert BadRequestObjectResult
        private void AssertBadRequest(IActionResult result, string key, string expectedMessage)
        {
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult?.Value, Is.TypeOf<SerializableError>());

            var errors = badRequestResult?.Value as SerializableError;
            Assert.That(errors, Contains.Key(key));
            Assert.That(errors?[key], Contains.Item(expectedMessage));
        }
    }
}
