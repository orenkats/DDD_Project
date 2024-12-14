using Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infrastructure.Messaging
{
    public class NotificationConsumer : IMessagingConsumer
    {
        private readonly ConnectionFactory _factory;

        public NotificationConsumer(string hostname, string username, string password)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };
        }

        public async Task StartConsumingAsync(string queueName, Func<string, Task> onMessageReceived, CancellationToken cancellationToken)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await onMessageReceived(message);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
