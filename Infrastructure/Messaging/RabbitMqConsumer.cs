using Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infrastructure.Messaging;

/// <summary>
/// RabbitMQ implementation for consuming messages.
/// </summary>
public class RabbitMqConsumer : IMessagingConsumer
{
    private readonly ConnectionFactory _factory;

    public RabbitMqConsumer(string hostname = "localhost", string username = "guest", string password = "guest")
    {
        _factory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };
    }

    /// <summary>
    /// Starts consuming messages from a RabbitMQ queue.
    /// </summary>
    public Task StartConsumingAsync(string queueName, Func<string, Task> onMessageReceived, CancellationToken cancellationToken)
    {
        var connection = _factory.CreateConnection(); // Corrected method
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        Console.WriteLine($"[RabbitMqConsumer] Waiting for messages in queue: {queueName}");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[RabbitMqConsumer] Received message: {message}");

            if (onMessageReceived != null)
            {
                await onMessageReceived(message);
            }

            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }
}
