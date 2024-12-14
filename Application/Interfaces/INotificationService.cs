public interface INotificationService
{
    Task SendNotificationAsync(string type, string message);
}
