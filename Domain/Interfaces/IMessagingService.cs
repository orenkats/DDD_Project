namespace Domain.Interfaces;

/// <summary>
/// Interface for messaging services, abstracting operations like publishing and consuming messages.
/// </summary>
public interface IMessagingService
{
    /// <summary>
    /// Publishes a message to a specified queue.
    /// </summary>
    /// <param name="queueName">The name of the queue.</param>
    /// <param name="message">The message to publish.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task PublishAsync(string queueName, object message);

    /// <summary>
    /// Starts consuming messages from a specified queue.
    /// </summary>
    /// <param name="queueName">The name of the queue.</param>
    /// <param name="onMessageReceived">Callback to handle received messages.</param>
    /// <param name="cancellationToken">Cancellation token to stop consuming.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task StartConsumingAsync(string queueName, Func<string, Task> onMessageReceived, CancellationToken cancellationToken);
}
