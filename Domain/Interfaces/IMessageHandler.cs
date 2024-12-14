namespace Application.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleAsync(string message);
    }
}
