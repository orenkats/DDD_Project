namespace Domain.Interfaces;

public interface IMessagingPublisher
{
    Task PublishAsync(string queueName, object message);
}
