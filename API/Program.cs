using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Application.Services;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Register AppDbContext for persistence
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 39)) // Replace with your MySQL version
    ));

// Register repositories (Infrastructure Layer)
builder.Services.AddScoped<ITraderRepository, TraderRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register services (Application Layer)
builder.Services.AddScoped<ITraderService, TraderService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// Register RabbitMQ services
builder.Services.AddSingleton<IMessagingPublisher>(sp =>
    new RabbitMqPublisher(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    ));
builder.Services.AddSingleton<IMessagingConsumer, NotificationConsumer>(sp =>
    new NotificationConsumer(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    ));

var app = builder.Build();

// Configure the consumer to use NotificationService
var notificationService = app.Services.GetService<INotificationService>();
if (notificationService != null)
{
    var consumer = app.Services.GetService<IMessagingConsumer>();
    Task.Run(() =>
    {
        consumer?.StartConsumingAsync("order_placed_queue", async message =>
        {
            var notification = new Notification
            {
                Type = "OrderPlaced",
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await notificationService.AddNotificationAsync(notification);
        }, CancellationToken.None);
    });
}

// Map controllers
app.MapControllers();
app.MapGet("/", () => "Welcome to the RabbitMQ API!");

app.Run();
