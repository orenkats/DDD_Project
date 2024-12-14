namespace Domain.Entities;

public class Notification
{
    public string Type { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}
