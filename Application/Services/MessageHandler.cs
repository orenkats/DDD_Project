using Application.Interfaces;
using Domain.Interfaces;
using Domain.Entities;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MessageHandler : IMessageHandler
    {
        private readonly INotificationService _notificationService;

        public MessageHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(string message)
        {
            // Deserialize and process the message
            var notification = JsonSerializer.Deserialize<Notification>(message);
            if (notification != null)
            {
                await _notificationService.AddNotificationAsync(notification);
            }
        }
    }
}
