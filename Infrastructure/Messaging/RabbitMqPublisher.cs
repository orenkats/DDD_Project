using Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging;

/// <summary>
/// RabbitMQ implementation for publishing messages.
/// </summary>
public class RabbitMqPublisher : IMessagingPublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMqPublisher(string hostname = "localhost", string username = "guest", string password = "guest")
    {
        _factory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };
    }

    /// <summary>
    /// Publishes a message to the specified RabbitMQ queue.
    /// </summary>
    public Task PublishAsync(string queueName, object message)
    {
        using var connection = _factory.CreateConnection(); // Corrected method
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

        Console.WriteLine($"Message published to queue: {queueName}");
        return Task.CompletedTask;
    }
}
