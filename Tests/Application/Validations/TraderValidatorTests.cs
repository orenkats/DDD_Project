using FluentValidation.TestHelper;
using Application.Validations;
using Domain.Entities;
using NUnit.Framework;

namespace Tests.Application.Validations
{
    [TestFixture]
    public class TraderValidatorTests
    {
        private TraderValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new TraderValidator();
        }

        [Test]
        public void Validate_ValidTrader_ShouldPass()
        {
            // Arrange
            var trader = new Trader { Name = "John Doe", AccountBalance = 1000m };

            // Act
            var result = _validator.TestValidate(trader);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validate_EmptyName_ShouldFail()
        {
            // Arrange
            var trader = new Trader { Name = "", AccountBalance = 1000m };

            // Act
            var result = _validator.TestValidate(trader);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Name)
                  .WithErrorMessage("Trader name is required.");
        }

        [Test]
        public void Validate_NegativeAccountBalance_ShouldFail()
        {
            // Arrange
            var trader = new Trader { Name = "John Doe", AccountBalance = -100m };

            // Act
            var result = _validator.TestValidate(trader);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.AccountBalance)
                  .WithErrorMessage("Account balance must be non-negative.");
        }
    }
}
