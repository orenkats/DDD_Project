namespace Domain.Interfaces;

public interface IMessagingConsumer
{
    Task StartConsumingAsync(string queueName, Func<string, Task> onMessageReceived, CancellationToken cancellationToken);
}
