using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Messaging;
using Xunit;

namespace Tests.Infrastructure.Messaging;

/// <summary>
/// Tests for the RabbitMqConsumer implementation.
/// </summary>
public class RabbitMqConsumerTests
{
    private readonly RabbitMqConsumer _rabbitMqConsumer;

    public RabbitMqConsumerTests()
    {
        // Setup RabbitMQ consumer with test credentials
        _rabbitMqConsumer = new RabbitMqConsumer("localhost", "guest", "guest");
    }

    [Fact]
    public async Task StartConsumingAsync_Should_InvokeCallback_When_MessageIsReceived()
    {
        // Arrange
        var messageReceived = false;
        Func<string, Task> callback = message =>
        {
            messageReceived = true;
            Assert.Equal("Test Message", message); // Verify the message content
            return Task.CompletedTask;
        };

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5)); // Timeout for safety

        // Simulate sending a message before consuming
        var publisher = new RabbitMqPublisher("localhost", "guest", "guest");
        await publisher.PublishAsync("test_queue", "Test Message");

        // Act
        await _rabbitMqConsumer.StartConsumingAsync("test_queue", callback, cancellationTokenSource.Token);

        // Assert
        Assert.True(messageReceived); // Ensure the callback was invoked
    }
}
