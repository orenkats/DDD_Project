using Domain.Interfaces;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessagingPublisher _publisher;
    private readonly IMessagingConsumer _consumer;
   
    public MessageController(
        IMessagingPublisher publisher,
        IMessagingConsumer consumer)
        // Inject the dependency
    {
        _publisher = publisher;
        _consumer = consumer;
         // Assign the dependency
    }

    public class QueueRequest
    {
        public string QueueName { get; set; } = null!;
        public StockOrder? Message { get; set; } = null!;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] QueueRequest request)
    {
        if (string.IsNullOrEmpty(request.QueueName) || request.Message == null)
        {
            return BadRequest("Queue name and message details are required.");
        }

        try
        {
            await _publisher.PublishAsync(request.QueueName, request.Message);
            return Ok("Message sent successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending message: {ex.Message}");
        }
    }

    [HttpPost("receive")]
    public IActionResult StartReceiving([FromBody] QueueRequest request)
    {
        if (string.IsNullOrEmpty(request.QueueName))
        {
            return BadRequest("Queue name is required.");
        }

        Task.Run(() =>
        {
            _consumer.StartConsumingAsync(
                request.QueueName,
                async (msg) =>
                {
                    Console.WriteLine($"Message received: {msg}");
                },
                CancellationToken.None
            );
        });

        return Ok($"Started listening to queue: {request.QueueName}");
    }
    
}
