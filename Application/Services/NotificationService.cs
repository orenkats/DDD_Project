using Application.Interfaces;
using Domain.Entities;
using System.Collections.Concurrent;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ConcurrentBag<Notification> _notifications = new();

        public Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return Task.FromResult(_notifications.AsEnumerable());
        }

        public Task AddNotificationAsync(Notification notification)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public Task SendNotificationAsync(string type, string message)
        {
            var notification = new Notification
            {
                Type = type,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            return AddNotificationAsync(notification);
        }
    }
}
