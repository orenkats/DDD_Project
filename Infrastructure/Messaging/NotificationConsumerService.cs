using Domain.Interfaces;
using Application.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Infrastructure.Messaging
{
    public class NotificationConsumerService : BackgroundService
    {
        private readonly IMessagingConsumer _messagingConsumer;
        private readonly INotificationService _notificationService;

        public NotificationConsumerService(IMessagingConsumer messagingConsumer, INotificationService notificationService)
        {
            _messagingConsumer = messagingConsumer;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Queue name for RabbitMQ
            const string queueName = "order_placed_queue";

            // Start consuming messages
            await _messagingConsumer.StartConsumingAsync(queueName, async message =>
            {
                // Parse and handle the message
                try
                {
                    var notification = JsonSerializer.Deserialize<Notification>(message);
                    if (notification != null)
                    {
                        await _notificationService.AddNotificationAsync(notification);
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (replace with actual logging)
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            }, stoppingToken);
        }
    }
}
