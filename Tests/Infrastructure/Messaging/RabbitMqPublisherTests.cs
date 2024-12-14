using System.Threading.Tasks;
using Infrastructure.Messaging;
using Xunit;

namespace Tests.Infrastructure.Messaging;

/// <summary>
/// Tests for the RabbitMqPublisher implementation.
/// </summary>
public class RabbitMqPublisherTests
{
    private readonly RabbitMqPublisher _rabbitMqPublisher;

    public RabbitMqPublisherTests()
    {
        // Setup RabbitMQ publisher with test credentials
        _rabbitMqPublisher = new RabbitMqPublisher("localhost", "guest", "guest");
    }

    [Fact]
    public async Task PublishAsync_Should_NotThrow_When_ValidMessageIsSent()
    {
        // Act & Assert
        var exception = await Record.ExceptionAsync(() =>
            _rabbitMqPublisher.PublishAsync("test_queue", new { Message = "Test Message" }));

        Assert.Null(exception); // Ensure no exception is thrown
    }
}
